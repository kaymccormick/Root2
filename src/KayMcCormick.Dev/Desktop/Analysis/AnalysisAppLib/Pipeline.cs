using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Buildalyzer ;
using Buildalyzer.Logging ;
using Buildalyzer.Workspaces ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Build.Framework ;
using Microsoft.Build.Logging.StructuredLogger ;
using Microsoft.CodeAnalysis ;
using Microsoft.Extensions.Logging ;
using Newtonsoft.Json ;
using NLog ;
using NLog.Fluent ;
using ILogger = Microsoft.Extensions.Logging.ILogger ;
using JsonSerializer = System.Text.Json.JsonSerializer ;
using LogLevel = NLog.LogLevel ;
using ProjectEvaluationFinishedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectEvaluationFinishedEventArgs ;
using ProjectEvaluationStartedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectEvaluationStartedEventArgs ;
using ProjectImportedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectImportedEventArgs ;
using TargetSkippedEventArgs = Microsoft.Build.Logging.StructuredLogger.TargetSkippedEventArgs ;
using Task = System.Threading.Tasks.Task ;

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
        public Pipeline ( ILoggerFactory  loggerProvider, Action <string> outAct )
        {
            this.loggerProvider = loggerProvider ;
            _outAct = outAct ;
        }

        private readonly Func < ILogInvocation > _invocationFactory ;
        private readonly IEnumerable < Action < Tuple < Workspace , Document > > > _documentAction1 ;
        private readonly IEnumerable < Action < Document > > _documentAction ;
        private readonly IEnumerable < Action < ILogInvocation > > _invocActions ;
        private readonly ILoggerFactory _loggerProvider ;

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
        private BufferBlock < RejectedItem >                           _rejectBlock ;
        private ILoggerFactory loggerProvider ;
        private readonly Action < string > _outAct ;
        private readonly IEnumerable < Action < IEventMisc > > _miscs ;
        private WriteOnceBlock < AnalysisRequest > _input ;
        private BufferBlock < Document > _bufferBlock ;

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
        public Pipeline ( Func < ILogInvocation > invocationFactory , IEnumerable<Action<Tuple<Workspace, Document>>> documentAction1 ,
                          IEnumerable<Action<Document>> documentAction, IEnumerable <Action <ILogInvocation> > invocActions , ILoggerFactory loggerProvider , Action <string> outAct,
                          IEnumerable<Action<IEventMisc>> miscs
                          )
        {
            _outAct = outAct ;
            _miscs = miscs ;
            _invocationFactory = invocationFactory ;
            _documentAction1 = documentAction1 ;
            _documentAction = documentAction ;
            _invocActions = invocActions ;
            _loggerProvider = loggerProvider ;

            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void BuildPipeline ( )
        {
            
            var initWorkspace = Register ( Workspaces.InitializeWorkspace2Block (loggerProvider ,_outAct, _miscs) ) ;
            var cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register ( Workspaces.SolutionDocumentsBlock (_documentAction,_documentAction1 ) ) ;
            IPropagatorBlock <Document,Document> prop = new BroadcastBlock < Document > ( document => document );
            prop.LinkTo ( new ActionBlock < Document > ( d => Logger.Info("pop {name}", d.Name)), LinkOptions);
            toDocuments.LinkTo ( prop , LinkOptions ) ;
            RejectBlock = new BufferBlock < RejectedItem > ( ) ;

            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register (
                                          DataflowBlocks.FindLogUsages1 (
                                                                         _invocationFactory
                                                                       , RejectBlock, _invocActions
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

        [ NotNull ]
        private T Register < T > ( [ NotNull ] T block )
            where T : IDataflowBlock
        {
            Blocks.Add ( block ) ;
            var task = Continuation ( block , ( ( object ) block ).ToString ( ) ) ;
            return block ;
        }

        private static Task Continuation ( [ NotNull ] IDataflowBlock block , string writeOnceBlockName )
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

                var faultReaon = task.Exception.Message ;
                new LogBuilder ( Logger )
                   .LoggerName ( $"{Logger.Name}.{logName}" )
                   .Level ( LogLevel.Trace )
                   .Exception ( task.Exception )
                   .Message ( "fault is {ex}" , faultReaon )
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
            Head = Input ;
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

        public List < IDataflowBlock > Blocks { get { return _dataflowBlocks ; } }

        public WriteOnceBlock < AnalysisRequest > Input
        {
            get { return _input ; }
            set { _input = value ; }
        }

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
                ILoggerFactory provider, Action<string> outAct,
                IEnumerable<Action <IEventMisc> > miscs
            )
            {
                var makeWs =
                    new TransformBlock < AnalysisRequest , Workspace > ( req => MakeWorkspace2Async ( req , provider,outAct,miscs ) ) ;
                return makeWs ;
            }


#pragma warning disable 1998
            [ ItemNotNull ]
            private static async Task < Workspace > MakeWorkspace2Async (
#pragma warning restore 1998
                [ NotNull ] AnalysisRequest req
              , ILoggerFactory              lo, [ NotNull ] Action <string> outAct,
                IEnumerable<Action<IEventMisc>> misc
            )

            {
                using ( MappedDiagnosticsLogicalContext.SetScoped ( "Workspace" , req.Info ) )
                {
                    if ( outAct == null )
                    {
                        throw new ArgumentNullException ( nameof ( outAct ) ) ;
                    }

                    try
                    {
                        var manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                        var workspace = new AdhocWorkspace ( ) ;

                        manager.LoggerFactory = lo ;
                        foreach ( var keyValuePair in manager.Projects )
                        {
                            Logger.Debug ( keyValuePair.Key ) ;
                            keyValuePair.Value.AddBuildLogger (
                                                               new Log1 (
                                                                         outAct
                                                                       , keyValuePair.Value
                                                                       , misc
                                                                        )
                                                              ) ;
                            var b = keyValuePair.Value.Build ( ) ;
                            foreach ( var analyzerResult in b.Results )
                            {
                                Logger.Info ( "{r}" , analyzerResult.ToString() ) ;
                            }

                            if ( ! b.OverallSuccess )
                            {
                                outAct ( $"{keyValuePair.Key} failed" ) ;
                            }
                            else
                            {
                                keyValuePair.Value.AddToWorkspace ( workspace ) ;
                            }
                        }

                        return workspace ;

                    }
                    catch ( Exception ex )
                    {
                        Logger.Error ( ex , "here" ) ;
                        throw ;
                    }
                }
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

                                                                                           ).Select(
                                                                                                    xx1
                                                                                                        => {
                                                                                                        foreach
                                                                                                        (var
                                                                                                             f in
                                                                                                            documentAction1
                                                                                                        )
                                                                                                        {
                                                                                                            f(
                                                                                                             Tuple.Create(workspace, xx1)
                                                                                                             );
                                                                                                        }

                                                                                                        return
                                                                                                            xx1;
                                                                                                    })
                                                                                           .Select(
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
                                                                                                    }));
                    
            }
        }

        internal static class DataflowBlocks
        {
            [ NotNull ]
            public static TransformManyBlock < Document , ILogInvocation > FindLogUsages1 (
                Func < ILogInvocation >                   invocationFactory
              , BufferBlock < RejectedItem >              rejectBlock
              , IEnumerable < Action < ILogInvocation > > invocActions
            )
            {
                Logger.Trace ( "Constructing FindUsagesBlock" ) ;
                var flu = new FindLogUsagesMain ( invocationFactory ) ;

                Task < IEnumerable < ILogInvocation > > Transform ( Document document ) => flu.FindUsagesFuncAsync ( document , rejectBlock, invocActions ) ;

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

    public interface IEventMisc
    {
        int ThreadId { get ; }
        object    Obj     { get; }
        string    Message { get; }
        MiscLevel Level   { get; set; }
        string    RawJson { get; set; }

        string File { get  ; }

        string PropKeys { get ; }
    }

    class MSBuildEventMisc : IEventMisc
    {
        private int _threadId ;
        private object _obj ;
        private string _message ;
        private MiscLevel _level ;
        private string _rawJson ;
        private string _propKeys ;
        private string _file ;

        public MSBuildEventMisc ( BuildEventArgs args , MiscLevel level )
        {
            _level = level ;
            Obj = args ;
        }

        #region Implementation of IEventMisc
        public int ThreadId { get { return _threadId ; } }

        public object Obj
        {
            get { return _obj ; }
            set { _obj = value ; }
        }

        public string Message { get { return _message ; } }

        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }

        public string PropKeys { get { return _propKeys ; } }

        public string File { get { return _file ; } set { _file = value ; } }
        #endregion
    }

    public class InvocationMisc : IEventMisc2 <ILogInvocation>
    {
        public InvocationMisc ( string rawJson , ILogInvocation instance )
        {
            _rawJson = rawJson ;
            _instance = instance ;
        }

        private int _threadId ;
        private object _obj ;
        private string _message ;
        private MiscLevel _level ;
        private string _rawJson ;
        private ILogInvocation _instance ;
        private string _propKeys ;
        private string _file ;
        #region Implementation of IEventMisc
        public int ThreadId { get { return _threadId ; } }

        public object Obj => Instance ;

        public string Message { get { return _message ; } }

        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }

        public string PropKeys { get { return _propKeys ; } }

        public string File => _instance.SourceLocation ;
        #endregion

        #region Implementation of IEventMisc2<out ILogInvocation>
        public ILogInvocation Instance { get { return _instance ; } }
        #endregion
    }

    public class LogEventMisc : IEventMisc
    {
        public int ThreadId => _inst.ManagedThreadId ;

        public string PropKeys
        {
            get
            {
                var o = JsonSerializer.Deserialize < JsonElement > ( RawJson ) ;
                List <string> ks=new List < string > ();
                if ( o.TryGetProperty ( "Properties" , out var props ) )
                {
                    foreach ( var keyValuePair in props.EnumerateObject ( ) )
                    {
                        if ( keyValuePair.NameEquals ( "CallerFilePath" )
                             || keyValuePair.NameEquals ( "CallerMemberName" )
                             || keyValuePair.NameEquals("CallerLineNumber"))
                        {
                            continue ;
                        }
                        ks.Add ( keyValuePair .ToString()) ;
                    }
                }
                if (o.TryGetProperty("MDLC", out var mdlc))
                {
                    foreach (var keyValuePair in mdlc.EnumerateObject())
                    {
                        ks.Add(keyValuePair.ToString());
                    }
                }
                return string.Join (
                                    ";",ks);
            }
        }

        public string File => _inst.CallerFilePath ;

        private readonly LogEventInstance _inst ;
        private MiscLevel _level = MiscLevel.INFO ;
        private string _rawJson ;
        private string _file ;
        #region Implementation of IEventMisc
        public LogEventMisc (LogEventInstance inst , string rawJson)
        {
            _inst = inst ;
            _rawJson = rawJson ;
        }

        public object Obj => _inst ;

        public string Message => _inst.FormattedMessage ;

        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }
        #endregion
    }


    public interface IEventMisc2< out T>:IEventMisc
    {
        T Instance { get ; }
    }

    public class Log1 : Microsoft.Build.Framework.ILogger
    {
        private readonly Action < string > _outAct ;
        private readonly IProjectAnalyzer _value ;
        private readonly IEnumerable < Action < IEventMisc > > _misc ;

        public Log1 (
            Action < string >                     outAct
          , IProjectAnalyzer                      value
          , IEnumerable < Action < IEventMisc > > misc
        )
        {
            _outAct = outAct ;
            _value = value ;
            _misc = misc ;
        }

        private LoggerVerbosity _verbosity ;
        private string _parameters ;
        #region Implementation of ILogger
        public void Initialize ( IEventSource eventSource )
        {
            
            eventSource.AnyEventRaised += EventSourceOnAnyEventRaised;
            eventSource.ErrorRaised += EventSource_ErrorRaised;
        }

        private void EventSourceOnAnyEventRaised ( object sender , BuildEventArgs e )
        {
            bool? succeeded ;
            MiscLevel level = MiscLevel.DEBUG;
            int imp ;
            string file="" ;
            if ( e is BuildMessageEventArgs xx1 )
            {
                file = xx1.File ;
            }
            switch ( e )
            {
                
                case BuildFinishedEventArgs buildFinishedEventArgs :
                    level = MiscLevel.INFO ;
                    succeeded = buildFinishedEventArgs.Succeeded ;break;
                case CriticalBuildMessageEventArgs criticalBuildMessageEventArgs :
                    level = MiscLevel.CRIT ;
                    imp = 2 - ( int ) criticalBuildMessageEventArgs.Importance ;
                    break ;
                case MetaprojectGeneratedEventArgs metaprojectGeneratedEventArgs : break ;
                case ProjectImportedEventArgs projectImportedEventArgs : break ;
                case TargetSkippedEventArgs targetSkippedEventArgs : break ;
                case TaskCommandLineEventArgs taskCommandLineEventArgs : break ;
                
                case BuildStartedEventArgs buildStartedEventArgs :
                    level = MiscLevel.INFO ;
                    break ;
                case ProjectEvaluationFinishedEventArgs projectEvaluationFinishedEventArgs : break ;
                
                case ProjectEvaluationStartedEventArgs projectEvaluationStartedEventArgs : break ;
                case ProjectFinishedEventArgs projectFinishedEventArgs : level = MiscLevel.INFO;
                    break ;
                case ProjectStartedEventArgs projectStartedEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case TargetFinishedEventArgs targetFinishedEventArgs : break ;
                case TargetStartedEventArgs2 targetStartedEventArgs2 : break ;
                case TargetStartedEventArgs targetStartedEventArgs : break ;
                case TaskFinishedEventArgs taskFinishedEventArgs : break ;
                case TaskStartedEventArgs taskStartedEventArgs : break ;
                case BuildStatusEventArgs buildStatusEventArgs :
                    level = MiscLevel.DEBUG;
                    break ;
                case BuildWarningEventArgs buildWarningEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case ExternalProjectFinishedEventArgs externalProjectFinishedEventArgs : break ;
                case ExternalProjectStartedEventArgs externalProjectStartedEventArgs : break ;
                case CustomBuildEventArgs customBuildEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case BuildMessageEventArgs buildMessageEventArgs:
                    break;
                case BuildErrorEventArgs buildErrorEventArgs :
                    level=MiscLevel.ERROR ;
                    break ;
                case LazyFormattedBuildEventArgs lazyFormattedBuildEventArgs : break ;
                case TelemetryEventArgs telemetryEventArgs : break ;
                default : throw new ArgumentOutOfRangeException ( nameof ( e ) ) ;
            }

            var msBuildEventMisc = new MSBuildEventMisc(e, level) ;
            msBuildEventMisc.File = file ;
            IEventMisc eventMisc = msBuildEventMisc;
            foreach (var action in _misc)
            {
                action(eventMisc);
            }
        }

        private void EventSource_ErrorRaised(object sender, LazyFormattedBuildEventArgs e)
        {
            var n1 = _value.ProjectInSolution.ProjectName ;
           // _outAct?.Invoke ( n1 + ":" +e.Message ) ;
            foreach ( var action in _misc )
            {
                // action ( new Misc ( e ) ) ;
            }
        }

        public void Shutdown ( ) { }

        public LoggerVerbosity Verbosity { get { return _verbosity ; } set { _verbosity = value ; } }

        public string Parameters { get { return _parameters ; } set { _parameters = value ; } }
        #endregion
    }

    public enum MiscLevel
    {
        INFO = 3,
        WARN = 4,
        ERROR = 5
       , CRIT
        , DEBUG
    }

    public class F : ILoggerFactory
    {
        #region Implementation of IDisposable
        public void Dispose ( ) { }
        #endregion
        #region Implementation of ILoggerFactory
        public ILogger CreateLogger ( string categoryName ) { return null ; }

        public void AddProvider ( ILoggerProvider provider ) { }
        #endregion
    }
    public class Myw : ILoggerProvider
    {
        private readonly Action < string > _unknown ;

        public Myw ( Action<string> unknown ) { _unknown = unknown ; }

        #region Implementation of ILoggerProvider
        public ILogger CreateLogger ( string categoryName ) { return new MyL(_unknown) ; }
        #endregion
        #region Implementation of IDisposable
        public void Dispose ( ) { }
        #endregion
    }

    public class MyL : ILogger
    {
        private readonly Action < string > _unknown ;

        public MyL ( Action < string > unknown ) { _unknown = unknown ; }

        #region Implementation of ILogger
        public void Log < TState > (
            Microsoft.Extensions.Logging.LogLevel  logLevel
          , EventId   eventId
          , TState    state
          , Exception exception
          , Func < TState , Exception , string > formatter
        )
        {
            _unknown ( formatter ( state , exception ) ) ;
        }

        public bool IsEnabled ( Microsoft.Extensions.Logging.LogLevel logLevel ) { return false ; }

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