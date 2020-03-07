using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public class Pipeline
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public IPropagatorBlock <AnalysisRequest, ILogInvocation > PipelineInstance { get ; private set ; }

        private List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;
        private DataflowLinkOptions _linkOptions = new DataflowLinkOptions { PropagateCompletion = true } ;
        private IPropagatorBlock < AnalysisRequest , AnalysisRequest > _currentBlock ;

        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        public DataflowLinkOptions LinkOptions
        {
            get => _linkOptions ;
            set => _linkOptions = value ;
        }

        public IPropagatorBlock<AnalysisRequest, AnalysisRequest> CurrentBlock { get { return _currentBlock ; } set { _currentBlock = value ; } }

        public Pipeline ( )
        {
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
            _ = BuildPipeline ( ) ;
        }

        public virtual IPropagatorBlock < AnalysisRequest , ILogInvocation > BuildPipeline ( )
        {
            var initWorkspace = Register ( DataflowBlocks.InitializeWorkspace ( ) ) ;

            IPropagatorBlock<AnalysisRequest, AnalysisRequest> cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register ( Workspaces.SolutionDocumentsBlock ( ) ) ;
            
            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register(DataflowBlocks.FindLogUsages());
            toDocuments.LinkTo ( findLogUsages , LinkOptions ) ;
            findLogUsages.LinkTo ( Register ( ResultBufferBlock ), LinkOptions ) ;

            // Continuation ( clone ,             nameof ( clone ) ) ;
            // Continuation ( build ,             nameof ( build ) ) ; 
            // Continuation ( initWorkspace ,     nameof ( initWorkspace ) ) ;
            // Continuation ( toDocuments ,       nameof ( toDocuments ) ) ;
            // Continuation ( findLogUsages ,     nameof ( findLogUsages ) ) ;
            // Continuation ( ResultBufferBlock , nameof ( ResultBufferBlock ) ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( Head , ResultBufferBlock ) ;
            return PipelineInstance ;
        }

        private T Register <T>( T block ) where T:IDataflowBlock
        {
            _dataflowBlocks.Add ( ( block ) ) ;
            Continuation ( block , block.ToString ( ) ) ;
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
                var faultReaon = task.Exception.Message ;
                new LogBuilder ( Logger )
                                                              .LoggerName ( $"{Logger.Name}.{logName}" )
                                                              .Level ( LogLevel.Trace )
                                                              .Exception ( task.Exception )
                                                              .Message ( "fault is {ex}" , faultReaon )
                                                              .Write ( ) ;
            }
            else { Logger.Trace( $"{logName} complete - not faulted" ) ; }
        }

        protected virtual IPropagatorBlock <AnalysisRequest, AnalysisRequest> ConfigureInput ( )
        {
            
            var input = new WriteOnceBlock <AnalysisRequest> ( s => s ) ;
            Head = input ;
            return input ;
        }
        
        public ITargetBlock<AnalysisRequest> Head { get ; set ; }
    }

    public class AnalysisRequest
    {
        private IProjectBrowserNode projectInfo ;

        public IProjectBrowserNode Info { get => projectInfo ; set => projectInfo = value ; }
    }

    class PipelineRemoteSource : Pipeline
    {
        #region Overrides of PipeLine
        public override IPropagatorBlock < AnalysisRequest , ILogInvocation > BuildPipeline ( )
        {
            var opt = LinkOptions ;

            var input = ConfigureInput ( ) ;
            var clone = DataflowBlocks.CloneSource();
            input.LinkTo(clone, opt);

            #if NUGET
            var build = DataflowBlocks.PackagesRestore();
            clone.LinkTo(build, opt);

            CurrentBlock = build ;
#endif

            return base.BuildPipeline ( ) ;
        }
        #endregion
    }
}
