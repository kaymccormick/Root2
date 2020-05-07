using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.Extensions.Logging ;
using NLog ;
using NLog.Fluent ;
using ILogger = Microsoft.Extensions.Logging.ILogger ;
using LogLevel = NLog.LogLevel ;
using Task = System.Threading.Tasks.Task ;

// ReSharper disable UnusedVariable

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
        public Pipeline (
            IAddRuntimeResource add
        )
        {
            
            
            _add           = add ;
        }

        private readonly Func < ILogInvocation > _invocationFactory ;

        private readonly IEnumerable < Action < Tuple < Workspace , Document > > >
            _documentAction1 ;

        private readonly IEnumerable < Action < Document > >       _documentAction ;
        private readonly IEnumerable < Action < ILogInvocation > > _invocActions ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IPropagatorBlock < AnalysisRequest , ILogInvocation > PipelineInstance
        {
            get ;
            private set ;
        }


        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        private DataflowLinkOptions _linkOptions =
            new DataflowLinkOptions { PropagateCompletion = true } ;

#pragma warning disable 649
        private IPropagatorBlock < AnalysisRequest , AnalysisRequest > _currentBlock ;
#pragma warning restore 649
        private          BufferBlock < RejectedItem >          _rejectBlock ;
        
        
        private readonly IAddRuntimeResource                   _add ;
        private readonly IEnumerable < Action < IEventMisc > > _miscs ;
        private          WriteOnceBlock < AnalysisRequest >    _input ;
        private          BufferBlock < Document >              _bufferBlock ;

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
            // ReSharper disable once UnusedMember.Global
            set { _linkOptions = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPropagatorBlock < AnalysisRequest , AnalysisRequest > CurrentBlock
        {
            get { return _currentBlock ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationFactory"></param>
        /// <param name="documentAction1"></param>
        /// <param name="documentAction"></param>
        /// <param name="invocActions"></param>
        /// <param name="outAct"></param>
        /// <param name="miscs"></param>
        /// <param name="add"></param>
        public Pipeline (
            Func < ILogInvocation >                                   invocationFactory
          , IEnumerable < Action < Tuple < Workspace , Document > > > documentAction1
          , IEnumerable < Action < Document > >                       documentAction
          , IEnumerable < Action < ILogInvocation > >                 invocActions
        
          , IEnumerable < Action < IEventMisc > >                     miscs
          , IAddRuntimeResource                                       add
        )
        {
        
            _miscs             = miscs ;
            _add               = add ;
            _invocationFactory = invocationFactory ;
            _documentAction1   = documentAction1 ;
            _documentAction    = documentAction ;
            _invocActions      = invocActions ;

            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void BuildPipeline ( )
        {
            var initWorkspace = Register (
                                          Workspaces.InitializeWorkspace2Block (
                                                                                null
                                                                              , null
                                                                              , _miscs
                                                                               )
                                         ) ;
            var cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register (
                                        Workspaces.SolutionDocumentsBlock (
                                                                           _documentAction
                                                                         , _documentAction1
                                                                          )
                                       ) ;
            IPropagatorBlock < Document , Document > prop =
                new BroadcastBlock < Document > ( document => document ) ;
            var actionBlock = new ActionBlock < Document > ( Action ) ;
            prop.LinkTo ( actionBlock , LinkOptions ) ;

            toDocuments.LinkTo ( prop , LinkOptions ) ;
            RejectBlock = new BufferBlock < RejectedItem > ( ) ;

            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register (
                                          DataflowBlocks.FindLogUsages1 (
                                                                         _invocationFactory
                                                                       , RejectBlock
                                                                       , _invocActions
                                                                        )
                                         ) ;
            prop.LinkTo ( findLogUsages , LinkOptions ) ;
            Block = new BufferBlock < Document > ( ) ;
            // toDocuments.LinkTo ( Block , LinkOptions ) ;
            // toDocuments.LinkTo ( findLogUsages , LinkOptions ) ;
            // Block.LinkTo ( findLogUsages , LinkOptions ) ;
            findLogUsages.LinkTo ( Register ( ResultBufferBlock ) , LinkOptions ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( Head , ResultBufferBlock ) ;
        }

        private async Task Action ( [ NotNull ] Document d )
        {
            var resourceNodeInfo = ResourceNodeInfo.CreateInstance ( ) ;
            resourceNodeInfo.Key            = d.Name ;
            resourceNodeInfo.Data           = d ;
            resourceNodeInfo.CreateNodeFunc = CreateNodeFunc ;
            var sem = await d.GetSemanticModelAsync ( ) ;
            var tree = await d.GetSyntaxTreeAsync ( ) ;
            var n = ResourceNodeInfo.CreateInstance ( ) ;
            n.Key  = "Syntax Tree" ;
            n.Data = tree ;
            var instance = ResourceNodeInfo.CreateInstance ( CreateNodeFunc ) ;
            instance.Key = "Root" ;
            if ( tree != null )
            {
                var syntaxNode = await tree.GetRootAsync ( ) ;
                instance.Data = new AnalysisContext { Node = syntaxNode , Model = sem } ;
            }

            n.Children.Add ( instance ) ;
            instance.GetChildrenFunc = InstanceGetChildrenFunc ;
            var nodeInfo = ResourceNodeInfo.CreateInstance ( CreateNodeFunc ) ;
            nodeInfo.Key = "compilation" ;

            if ( sem == null ) { }
            else
            {
                var comp = sem.Compilation ;
                nodeInfo.Data = comp ;
                var glob = comp.SourceModule.GlobalNamespace ;
                var collection = glob.GetNamespaceMembers ( )
                                     .Select (
                                              cn => Func (
                                                          cn
                                                        , ( o , o1 ) => {
                                                              var x1 = ResourceNodeInfo
                                                                 .CreateInstance (
                                                                                  CreateNodeFunc
                                                                                 ) ;
                                                              x1.Key  = o ;
                                                              x1.Data = o1 ;
                                                              x1.GetChildrenFunc = ( info , func11 )
                                                                  => cn.GetMembers ( )
                                                                       .Select (
                                                                                symbol => Func (
                                                                                                symbol
                                                                                              , func11
                                                                                               )
                                                                               ) ;
                                                              return x1 ;
                                                          }
                                                         )
                                             ) ;
                nodeInfo.Children.AddRange ( collection ) ;
                nodeInfo.Children.AddRange (
                                            comp.References.Select (
                                                                    r => nodeInfo.CreateNodeFunc (
                                                                                                  nodeInfo
                                                                                                , r
                                                                                                     .Display
                                                                                                , r
                                                                                                , true
                                                                                                , false
                                                                                                 )
                                                                   )
                                           ) ;
            }

            resourceNodeInfo.Children.Add ( nodeInfo ) ;
            resourceNodeInfo.Children.Add ( n ) ;
            _add.AddResource ( resourceNodeInfo ) ;
            Logger.Info ( "pop {name}" , d.Name ) ;
        }

        [ NotNull ]
        private ResourceNodeInfo CreateNodeFunc (
            [ CanBeNull ] ResourceNodeInfo arg1
          , object                         arg2
          , object                         arg3
          , bool ?                         arg4
          , bool                           arg5
        )
        {
            var r = ResourceNodeInfo.CreateInstance ( ) ;
            r.CreateNodeFunc  = CreateNodeFunc ;
            r.Key             = arg2 ;
            r.Data            = arg3 ;
            r.IsValueChildren = arg4 ;
            if ( arg5 )
            {
                arg1?.Children.Add ( r ) ;
            }

            return r ;
        }

        [ NotNull ]
        private IEnumerable < ResourceNodeInfo > InstanceGetChildrenFunc (
            [ NotNull ] ResourceNodeInfo                info
          , Func < object , object , ResourceNodeInfo > func
        )
        {
            var infoData = ( AnalysisContext ) info.Data ;
            var syntaxNodes = infoData.Node.ChildNodes ( ) ;
            return syntaxNodes.Select (
                                       xx => {
                                           var resourceNodeInfo1 = func (
                                                                         xx.Kind ( ).ToString ( )
                                                                       , new AnalysisContext
                                                                         {
                                                                             Node  = xx
                                                                           , Model = infoData.Model
                                                                         }
                                                                        ) ;
                                           resourceNodeInfo1.GetChildrenFunc =
                                               InstanceGetChildrenFunc ;
                                           return resourceNodeInfo1 ;
                                       }
                                      ) ;
        }

        [ NotNull ]
        private ResourceNodeInfo Func (
            [ NotNull ] ISymbol                                     cn
          , [ NotNull ] Func < object , object , ResourceNodeInfo > func1
        )
        {
            var rr = func1 ( cn.Name , cn ) ;
            rr.GetChildrenFunc = ( info , func ) => {
                var arg1Data = info.Data as ISymbol ;
                IEnumerable < object > @enum = new object[] { } ;
                switch ( arg1Data )
                {
                    case IAliasSymbol aliasSymbol :                   break ;
                    case IArrayTypeSymbol arrayTypeSymbol :           break ;
                    case ISourceAssemblySymbol sourceAssemblySymbol : break ;
                    case IAssemblySymbol assemblySymbol :             break ;
                    case IDiscardSymbol discardSymbol :               break ;
                    case IDynamicTypeSymbol dynamicTypeSymbol :       break ;
                    case IErrorTypeSymbol errorTypeSymbol :           break ;
                    case IEventSymbol eventSymbol :                   break ;
                    case IFieldSymbol fieldSymbol :                   break ;
                    case ILabelSymbol labelSymbol :                   break ;
                    case ILocalSymbol localSymbol :                   break ;
                    case IMethodSymbol methodSymbol :
                        @enum = methodSymbol.Parameters ;
                        break ;
                    case IModuleSymbol moduleSymbol : break ;
                    case INamedTypeSymbol namedTypeSymbol :
                        @enum = namedTypeSymbol.GetMembers ( ) ;
                        break ;
                    case INamespaceSymbol namespaceSymbol :
                        @enum = namespaceSymbol.GetMembers ( ) ;
                        break ;
                    case IPointerTypeSymbol pointerTypeSymbol :     break ;
                    case ITypeParameterSymbol typeParameterSymbol : break ;
                    case ITypeSymbol typeSymbol :
                        @enum = typeSymbol.GetMembers ( ) ;
                        break ;
                    case INamespaceOrTypeSymbol namespaceOrTypeSymbol : break ;
                    case IParameterSymbol parameterSymbol :             break ;
                    case IPreprocessingSymbol preprocessingSymbol :     break ;
                    case IPropertySymbol propertySymbol :               break ;
                    case IRangeVariableSymbol rangeVariableSymbol :     break ;
                    default :
                        throw new ArgumentOutOfRangeException ( nameof ( arg1Data ) ) ;
                }

                return @enum.OfType < ISymbol > ( ).Select ( xxx => Func ( xxx , func ) ) ;
            } ;
            return rr ;
        }

        [ NotNull ]
        private T Register < T > ( [ NotNull ] T block )
            where T : IDataflowBlock
        {
            Blocks.Add ( block ) ;
            var task = Continuation ( block , ( ( object ) block ).ToString ( ) ) ;
            return block ;
        }

        [ NotNull ]
        private static Task Continuation (
            [ NotNull ] IDataflowBlock block
          , string                     writeOnceBlockName
        )
        {
            return block.Completion.ContinueWith (
                                                  task => ContinuationFunction (
                                                                                task
                                                                              , writeOnceBlockName
                                                                               )
                                                 ) ;
        }

        private static void ContinuationFunction ( [ NotNull ] Task task , string logName )
        {
            Logger.Debug ( $"Continuatuin, {task.Status}. Task id {task.Id}, Log Name {logName}" ) ;
            if ( task.IsFaulted )
            {
                if ( task.Exception == null )
                {
                    return ;
                }

                var faultReason = task.Exception.Message ;
                new LogBuilder ( Logger )
                   .LoggerName ( $"{Logger.Name}.{logName}" )
                   .Level ( LogLevel.Trace )
                   .Exception ( task.Exception )
                   .Message ( "fault is {ex}" , faultReason )
                   .Write ( ) ;
            }
            else { Logger.Trace ( $"{logName} complete - not faulted" ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        protected virtual IPropagatorBlock < AnalysisRequest , AnalysisRequest > ConfigureInput ( )
        {
            Input = new WriteOnceBlock < AnalysisRequest > ( s => s ) ;
            Head  = Input ;
            return Input ;
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

        /// <summary>
        /// 
        /// </summary>
        public List < IDataflowBlock > Blocks { get { return _dataflowBlocks ; } }

        /// <summary>
        /// 
        /// </summary>
        public WriteOnceBlock < AnalysisRequest > Input
        {
            get { return _input ; }
            set { _input = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferBlock < Document > Block
        {
            get { return _bufferBlock ; }
            set { _bufferBlock = value ; }
        }

        private static class Workspaces
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


            [ NotNull ]
            public static TransformBlock < AnalysisRequest , Workspace > InitializeWorkspace2Block (
                ILoggerFactory                        provider
              , Action < string >                     outAct
              , IEnumerable < Action < IEventMisc > > miscs
            )
            {
                var makeWs =
                    new TransformBlock < AnalysisRequest , Workspace > (
                                                                        req => MakeWorkspace2Async (
                                                                                                    req
                                                                                                  , provider
                                                                                                  , outAct
                                                                                                  , miscs
                                                                                                   )
                                                                       ) ;
                return makeWs ;
            }


#pragma warning disable 1998
            [ ItemNotNull ]
            private static async Task < Workspace > MakeWorkspace2Async (
#pragma warning restore 1998
                [ NotNull ] AnalysisRequest                         req
              , ILoggerFactory                                      lo
              , [ NotNull ]   Action < string >                     outAct
              , [ CanBeNull ] IEnumerable < Action < IEventMisc > > misc
            )

            {
                using ( MappedDiagnosticsLogicalContext.SetScoped ( "Workspace" , req.Info ) )
                {
                    // try
                    // {
                    //     var manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                    //     Workspace workspace = null ;
                    //     try
                    //     {
                    //         workspace = new AdhocWorkspace ( ) ;
                    //     }
                    //     catch
                    //     {
                    //         // ignored
                    //     }
                    //
                    //     manager.LoggerFactory = lo ;
                    //     foreach ( var keyValuePair in manager.Projects )
                    //     {
                    //         Logger.Debug ( keyValuePair.Key ) ;
                    //         var projectAnalyzer = keyValuePair.Value ;
                    //         var logger = new Log1 ( outAct , projectAnalyzer , misc ) ;
                    //         projectAnalyzer.AddBuildLogger ( logger ) ;
                    //         var b = projectAnalyzer.Build ( ) ;
                    //         foreach ( var analyzerResult in b.Results )
                    //         {
                    //             Logger.Info ( "{r}" , analyzerResult.ToString ( ) ) ;
                    //         }
                    //
                    //         if ( ! b.OverallSuccess )
                    //         {
                    //             outAct?.Invoke ( $"{keyValuePair.Key} failed" ) ;
                    //         }
                    //         else
                    //         {
                    //             projectAnalyzer.AddToWorkspace ( workspace ) ;
                    //         }
                    //     }
                    //
                    //     if ( workspace != null )
                    //     {
                    //         return workspace ;
                    //     }
                    //
                    //     return null ;
                    // }
                    // catch ( Exception ex )
                    // {
                    //     Logger.Error ( ex , "here" ) ;
                    //     throw ;
                    // }
                }

                return null;
            }


            [ NotNull ]
            public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock (
                IEnumerable < Action < Document > >                       documentAction
              , IEnumerable < Action < Tuple < Workspace , Document > > > documentAction1
            )
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
                                                                                    .Select (
                                                                                             xx1
                                                                                                 => {
                                                                                                 foreach
                                                                                                 ( var
                                                                                                       f in
                                                                                                     documentAction1
                                                                                                 )
                                                                                                 {
                                                                                                     f (
                                                                                                        Tuple
                                                                                                           .Create (
                                                                                                                    workspace
                                                                                                                  , xx1
                                                                                                                   )
                                                                                                       ) ;
                                                                                                 }

                                                                                                 return
                                                                                                     xx1 ;
                                                                                             }
                                                                                            )
                                                                                    .Select (
                                                                                             xx1
                                                                                                 => {
                                                                                                 foreach
                                                                                                 ( var
                                                                                                       f in
                                                                                                     documentAction
                                                                                                 )
                                                                                                 {
                                                                                                     f (
                                                                                                        xx1
                                                                                                       ) ;
                                                                                                 }

                                                                                                 return
                                                                                                     xx1 ;
                                                                                             }
                                                                                            )
                                                                       ) ;
            }
        }

        internal static class DataflowBlocks
        {
            [ NotNull ]
            public static TransformManyBlock < Document , ILogInvocation > FindLogUsages1 (
                Func < ILogInvocation >                                 invocationFactory
              , BufferBlock < RejectedItem >                            rejectBlock
              , [ CanBeNull ] IEnumerable < Action < ILogInvocation > > invocActions
            )
            {
                Logger.Trace ( "Constructing FindUsagesBlock" ) ;
                var flu = new FindLogUsagesMain ( invocationFactory ) ;

                Task < IEnumerable < ILogInvocation > > Transform ( Document document )
                {
                    return flu.FindUsagesFuncAsync ( document , rejectBlock , invocActions ) ;
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
    public class AnalysisContext
    {
        private SemanticModel _model ;
        private SyntaxNode    _node ;

        /// <summary>
        /// 
        /// </summary>
        public SemanticModel Model { get { return _model ; } set { _model = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode Node { get { return _node ; } set { _node = value ; } }
    }


    /// <summary>
    /// 
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        #region Implementation of IDisposable
        /// <inheritdoc />
        public void Dispose ( ) { }
        #endregion
        #region Implementation of ILoggerFactory
        /// <inheritdoc />
        [ CanBeNull ]
        public ILogger CreateLogger ( string categoryName ) { return null ; }

        /// <inheritdoc />
        public void AddProvider ( ILoggerProvider provider ) { }
        #endregion
    }

    /// <inheritdoc />
    public class Myw : ILoggerProvider
    {
        private readonly Action < string > _unknown ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknown"></param>
        public Myw ( Action < string > unknown ) { _unknown = unknown ; }

        #region Implementation of ILoggerProvider
        /// <inheritdoc />
        [ NotNull ]
        public ILogger CreateLogger ( string categoryName ) { return new MyL ( _unknown ) ; }
        #endregion
        #region Implementation of IDisposable
        /// <inheritdoc />
        public void Dispose ( ) { }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyL : ILogger
    {
        private readonly Action < string > _unknown ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknown"></param>
        public MyL ( Action < string > unknown ) { _unknown = unknown ; }

        #region Implementation of ILogger
        /// <inheritdoc />
        public void Log < TState > (
            Microsoft.Extensions.Logging.LogLevel            logLevel
          , EventId                                          eventId
          , TState                                           state
          , Exception                                        exception
          , [ NotNull ] Func < TState , Exception , string > formatter
        )
        {
            _unknown ( formatter ( state , exception ) ) ;
        }

        /// <inheritdoc />
        public bool IsEnabled ( Microsoft.Extensions.Logging.LogLevel logLevel ) { return false ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        [ CanBeNull ]
        public IDisposable BeginScope < TState > ( TState state ) { return null ; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MyTest
    {
    }
}