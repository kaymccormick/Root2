#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// WorkspacesViewModel.cs
// 
// 2020-02-19-7:26 AM
// 
// ---
#endregion
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog ;
using ProjLib.Properties ;
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Data.SqlClient ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Microsoft.TeamFoundation.TestManagement.WebApi.Legacy ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel
      , ISupportInitialize
      , INotifyPropertyChanged
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate (
            Tuple < SyntaxTree , SemanticModel , CompilationUnitSyntax > tuple
        ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate2 ( ) ;

        // private IList<IVsInstance> vsInstances;
        private readonly IVsInstanceCollector vsInstanceCollector ;

        public IPipelineViewModel PipelineViewModel { get ; }

        private string                _currentDocumentPath ;
        private MyProjectLoadProgress _currentProgress ;
        private string                _currentProject ;

        private bool                      _processing ;
        private IProjectBrowserViewModoel _projectBrowserViewModel ;
        private readonly SqlConnection _sqlConn ;
        private PipelineResult            _pipelineResult ;
        private string _applicationMode = "Runtime mode" ;
        private AdhocWorkspace _workspace ;

        public WorkspacesViewModel (
            IVsInstanceCollector      collector
          , IPipelineViewModel        pipelineViewModel
          , IProjectBrowserViewModoel projectBrowserViewModel
            , SqlConnection sqlConn
        )
        {
            vsInstanceCollector      = collector ;
            _projectBrowserViewModel = projectBrowserViewModel ;
            _sqlConn = sqlConn ;
            PipelineViewModel        = pipelineViewModel ;
        }

        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit ( )
        {
            var vsInstances = vsInstanceCollector.CollectVsInstances ( ) ;
            foreach ( var vsInstance in vsInstances )
            {
                VsCollection.Add ( vsInstance ) ;
            }

            OnPropertyChanged ( nameof ( VsCollection ) ) ;
        }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( ) { }

        public MyProjectLoadProgress CurrentProgress
        {
            get => _currentProgress ;
            set
            {
                _currentProgress = value ;
                OnPropertyChanged ( nameof ( CurrentProgress ) ) ;
            }
        }

        public VisualStudioInstancesCollection VsCollection { get ; } =
            new VisualStudioInstancesCollection ( ) ;
 
        public IProjectBrowserViewModoel ProjectBrowserViewModel
        {
            get => _projectBrowserViewModel ;
            set => _projectBrowserViewModel = value ;
        }

        public async Task AnalyzeCommand ( object viewCurrentItem )
        {
            PipelineResult = new PipelineResult(ResultStatus.Pending);
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                  invocation => {
                                                                      Logger.Warn(
                                                                                    "{invocation}"
                                                                                  , invocation
                                                                                   ) ;
                                                                  }
                                                                 ) ;
            PipelineViewModel.Pipeline.PipelineInstance.LinkTo (
                                                                actionBlock
                                                              , new DataflowLinkOptions ( )
                                                                {
                                                                    PropagateCompletion = true
                                                                }
                                                               ) ;
            var projectBrowserNode = ( IProjectBrowserNode ) viewCurrentItem ;
            await PipelineViewModel.Pipeline.PipelineInstance
                                   .SendAsync ( projectBrowserNode.RepositoryUrl )
                                   .ConfigureAwait ( true ) ;
            var result = await actionBlock.Completion.ContinueWith (
                                                                    task => {
                                                                        Logger.Info (
                                                                                     "received task {fault}"
                                                                                   , task.IsFaulted
                                                                                    ) ;
                                                                        if ( task.IsFaulted )
                                                                        {
                                                                            return new
                                                                                PipelineResult (
                                                                                                ResultStatus
                                                                                                   .Failed
                                                                                              , task
                                                                                                   .Exception
                                                                                               ) ;
                                                                        }
                                                                        else
                                                                        {
                                                                            return new
                                                                                PipelineResult (
                                                                                                ResultStatus
                                                                                                   .Success
                                                                                               ) ;
                                                                        }
                                                                    }
                                                                  , CancellationToken.None
                                                                  , TaskContinuationOptions.None
                                                                  , TaskScheduler.Current)
                                                                  
                                          .ConfigureAwait ( true ) ;
            PipelineResult = result ;
            if ( result.Status == ResultStatus.Failed )
            {
                Logger.Error (
                              result.TaskException
                            , "Failed: {}"
                            , result.TaskException.Message
                             ) ;
            }
            Logger.Debug("{id} {result}", Thread.CurrentThread.ManagedThreadId, result.Status);
        }

        private ObservableCollection < LogEventInfo > eventInfos  = new ObservableCollection < LogEventInfo > ();

        public string ApplicationMode => _applicationMode ;

        public AdhocWorkspace Workspace { get => _workspace ; set => _workspace = value ; }


        public PipelineResult PipelineResult
        {
            get => _pipelineResult ;
            set
            {
                _pipelineResult = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public bool Processing
        {
            get => _processing ;
            set
            {
                _processing = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentProject
        {
            get => _currentProject ;
            set
            {
                _currentProject = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentDocumentPath
        {
            get => _currentDocumentPath ;
            set
            {
                _currentDocumentPath = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ObservableCollection < ILogInvocation > LogInvocations { get ; } =
            new ObservableCollection < ILogInvocation > ( ) ;

        public ObservableCollection < LogEventInfo > EventInfos
        {
            get => eventInfos ;
            set => eventInfos = value ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        private void DoSomething ( ) { }

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    public class PipelineResult
    {
        public ResultStatus Status { get ; }

        public Exception TaskException { get ; }

        public PipelineResult ( ResultStatus status , Exception taskException = null )
        {
            Status        = status ;
            TaskException = taskException ;
        }
    }

    public enum ResultStatus { Failed , Success, Pending , None }

    public class MyProjectLoadProgress
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public MyProjectLoadProgress (
            string   filePath
          , string   operation
          , string   targetFramework
          , TimeSpan elapsedTime
        )
        {
            FilePath        = filePath ;
            Operation       = operation ;
            TargetFramework = targetFramework ;
            ElapsedTime     = elapsedTime ;
        }

        /// <summary>
        ///     The project for which progress is being reported.
        /// </summary>
        public string FilePath { get ; }

        public string FileName => Path.GetFileNameWithoutExtension ( FilePath ) ;

        /// <summary>
        ///     The operation that has just completed.
        /// </summary>
        public string Operation { get ; }

        /// <summary>
        ///     The target framework of the project being built or resolved. This
        ///     property is only valid for SDK-style projects
        ///     during the <see cref="ProjectLoadOperation.Resolve" /> operation.
        /// </summary>
        public string TargetFramework { get ; }

        /// <summary>
        ///     The amount of time elapsed for this operation.
        /// </summary>
        public TimeSpan ElapsedTime { get ; }
    }

    public class MyProgress : IProgress < ProjectLoadProgress >
    {
        private readonly WorkspacesViewModel _workspacesViewModel ;

        public MyProgress ( WorkspacesViewModel workspacesViewModel )
        {
            _workspacesViewModel = workspacesViewModel ;
        }

        /// <summary>Reports a progress update.</summary>
        /// <param name="value">The value of the updated progress.</param>
        public void Report ( ProjectLoadProgress value )
        {
            _workspacesViewModel.CurrentProgress = new MyProjectLoadProgress (
                                                                              value.FilePath
                                                                            , value
                                                                             .Operation.ToString ( )
                                                                            , value.TargetFramework
                                                                            , value.ElapsedTime
                                                                             ) ;
        }
    }
}