using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Buildalyzer ;
using Buildalyzer.Workspaces ;
using FindLogUsages ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using NLog ;
using NLog.Fluent ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Pipeline
    {
        /// <summary>
        /// 
        /// </summary>
        public Pipeline ( ) { }

        private readonly Func < ILogInvocation > _invocationFactory ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IPropagatorBlock < AnalysisRequest , ILogInvocation > PipelineInstance
        {
            get ;
            private set ;
        }


        private readonly List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        private DataflowLinkOptions _linkOptions =
            new DataflowLinkOptions { PropagateCompletion = true } ;

        private IPropagatorBlock < AnalysisRequest , AnalysisRequest > _currentBlock ;
        private BufferBlock < RejectedItem >                           _rejectBlock ;

        /// <summary>
        /// 
        /// </summary>
        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        /// <summary>
        /// 
        /// </summary>
        public DataflowLinkOptions LinkOptions
        {
            get { return _linkOptions ; }
            set { _linkOptions = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPropagatorBlock < AnalysisRequest , AnalysisRequest > CurrentBlock
        {
            get { return _currentBlock ; }
            set { _currentBlock = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationFactory"></param>
        public Pipeline ( Func < ILogInvocation > invocationFactory )
        {
            _invocationFactory = invocationFactory ;
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void BuildPipeline ( )
        {
            var initWorkspace = Register ( Workspaces.InitializeWorkspace2Block ( ) ) ;

            var cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register ( Workspaces.SolutionDocumentsBlock ( ) ) ;


            RejectBlock = new BufferBlock < RejectedItem > ( ) ;

            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register (
                                          DataflowBlocks.FindLogUsages1 (
                                                                         _invocationFactory
                                                                       , RejectBlock
                                                                        )
                                         ) ;
            toDocuments.LinkTo ( findLogUsages , LinkOptions ) ;
            findLogUsages.LinkTo ( Register ( ResultBufferBlock ) , LinkOptions ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( Head , ResultBufferBlock ) ;
        }

        private T Register < T > ( T block )
            where T : IDataflowBlock
        {
            _dataflowBlocks.Add ( block ) ;
            Continuation ( block , ( ( object ) block ).ToString ( ) ) ;
            return block ;
        }

        private static void Continuation ( IDataflowBlock block , string writeOnceBlockName )
        {
            block.Completion.ContinueWith (
                                           task => ContinuationFunction (
                                                                         task
                                                                       , writeOnceBlockName
                                                                        )
                                          ) ;
        }

        private static void ContinuationFunction ( Task task , string logName )
        {
            if ( task.IsFaulted )
            {
                if ( task.Exception != null )
                {
                    var faultReaon = task.Exception.Message ;
                    new LogBuilder ( Logger )
                       .LoggerName ( $"{Logger.Name}.{logName}" )
                       .Level ( LogLevel.Trace )
                       .Exception ( task.Exception )
                       .Message ( "fault is {ex}" , faultReaon )
                       .Write ( ) ;
                }
            }
            else { Logger.Trace ( $"{logName} complete - not faulted" ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IPropagatorBlock < AnalysisRequest , AnalysisRequest > ConfigureInput ( )
        {
            var input = new WriteOnceBlock < AnalysisRequest > ( s => s ) ;
            Head = input ;
            return input ;
        }

        /// <summary>
        /// 
        /// </summary>
        public ITargetBlock < AnalysisRequest > Head { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public BufferBlock < RejectedItem > RejectBlock
        {
            get { return _rejectBlock ; }
            set { _rejectBlock = value ; }
        }

        private static class Workspaces
        {
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


            public static TransformBlock < AnalysisRequest , Workspace >
                InitializeWorkspace2Block ( )
            {
                var makeWs =
                    new TransformBlock < AnalysisRequest , Workspace > ( MakeWorkspace2Async ) ;
                return makeWs ;
            }


            public static async Task < Workspace > MakeWorkspace2Async (
                [ NotNull ] AnalysisRequest req
            )

            {
                try
                {
                    var manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                    var workspace = new AdhocWorkspace ( ) ;
                    foreach ( var keyValuePair in manager.Projects )
                    {
                        Logger.Debug ( keyValuePair.Key ) ;
                        keyValuePair.Value.Build ( ) ;
                        keyValuePair.Value.AddToWorkspace ( workspace ) ;
                    }

                    return workspace ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , "here" ) ;
                    throw ;
                }
            }


            [ NotNull ]
            public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock ( )
            {
                return new TransformManyBlock < Workspace , Document > (
                                                                        workspace => workspace
                                                                                    .CurrentSolution
                                                                                    .Projects
                                                                                    .AsParallel ( )
                                                                                    .Where (
                                                                                            project
                                                                                                => {
                                                                                                Logger
                                                                                                   .Warn (
                                                                                                          "{project}"
                                                                                                        , project
                                                                                                             .Name
                                                                                                         ) ;
                                                                                                return
                                                                                                    true ;
                                                                                            }
                                                                                           )
                                                                                    .SelectMany (
                                                                                                 project
                                                                                                     => project
                                                                                                        .Documents
                                                                                                )
                                                                                    .Where (
                                                                                            document
                                                                                                => {
                                                                                                Logger
                                                                                                   .Info (
                                                                                                          "{document}"
                                                                                                        , document
                                                                                                             .Name
                                                                                                         ) ;
                                                                                                return
                                                                                                    true ;
                                                                                            }
                                                                                           )
                                                                       ) ;
            }
        }

        internal static class DataflowBlocks
        {
            [ NotNull ]
            public static TransformManyBlock < Document , ILogInvocation > FindLogUsages1 (
                Func < ILogInvocation >      invocationFactory
              , BufferBlock < RejectedItem > rejectBlock
            )
            {
                Logger.Trace ( "Constructing FindUsagesBlock" ) ;
                var flu = new FindLogUsagesMain ( invocationFactory ) ;

                Task < IEnumerable < ILogInvocation > > Transform ( Document document )
                {
                    return flu.FindUsagesFuncAsync ( document , rejectBlock ) ;
                }

                var findLogUsagesBlock =
                    new TransformManyBlock < Document , ILogInvocation > (
                                                                          Transform
                                                                        , new
                                                                          ExecutionDataflowBlockOptions
                                                                          {
                                                                              MaxDegreeOfParallelism
                                                                                  = 4
                                                                          }
                                                                         ) ;
                return findLogUsagesBlock ;
            }
#if USEMSBUILD
        public static TransformBlock < string , BuildResults > PackagesRestore ( )
        {
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => BuildTool.BuildRepository ( s )
                                                             ) ;
            return buildTransformBlock ;
        }
#else
#if NUGET
        public static TransformBlock<string, string> PackagesRestore()
        {
            var buildTransformBlock =
                new TransformBlock<string, string>(
                                                         s => NugetTool.RestorePackages(s)
                                                        );
            return buildTransformBlock;
        }
#endif
#endif

#if VERSIONCONTROL
        public static TransformBlock <AnalysisRequest, AnalysisRequest> CloneSource ( )
        {
            return new TransformBlock <AnalysisRequest, AnalysisRequest> ( VersionControl.CloneProjectAsync ) ;
        }
#endif
        }
#if NUGET
    internal static class NugetTool
    {
        public static string RestorePackages ( string s ) { return s ; }
    }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyTest
    {
    }
}