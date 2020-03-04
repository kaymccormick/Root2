
using System ;
using System.Collections.Concurrent ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
using AnalysisControls ;
using AnalysisFramework ;
using Autofac ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.TeamFoundation.Server ;
using NLog ;
using ProjLib ;
using Rxx ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjMainWindow : Window
      , IView < IWorkspacesViewModel >
      , IView1
      , IWorkspacesView
      , INotifyPropertyChanged
    {
        private static readonly Logger      Logger = LogManager.GetCurrentClassLogger ( ) ;
        private                 TaskFactory _factory ;

        public IWorkspacesViewModel ViewModel
        {
            get => _viewModel ;
            set
            {
                _viewModel = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ProjMainWindow ( )
        {
            InitializeComponent ( ) ;
            _factory = new TaskFactory ( TaskScheduler.FromCurrentSynchronizationContext ( ) ) ;
        }


        public ProjMainWindow ( IWorkspacesViewModel viewModel , ILifetimeScope scope )
        {
            InitializeComponent ( ) ;
            ViewModel = viewModel ;
            Scope     = scope ;
            _factory  = new TaskFactory ( TaskScheduler.FromCurrentSynchronizationContext ( ) ) ;
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                  invocation => {
                                                                      ViewModel.LogInvocations
                                                                         .Add ( invocation ) ;
                                                                  }
                                                                , new
                                                                  ExecutionDataflowBlockOptions ( )
                                                                  {
                                                                      TaskScheduler =
                                                                          TaskScheduler
                                                                             .FromCurrentSynchronizationContext ( )
                                                                  }
                                                                 ) ;
        }


        /// <summary>Raises the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to <see langword="true " />internally. </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnInitialized ( EventArgs e )
        {
            base.OnInitialized ( e ) ;
            Logger.Info ( "{methodName} {typeEvent}" , nameof ( OnInitialized ) , e.GetType ( ) ) ;
            // Scope = Container.GetContainer();
            // ViewModel = Scope.Resolve<IWorkspacesViewModel>();
            // ViewModel.BeginInit();
        }

        public ILifetimeScope Scope { get ; set ; }


        private ConcurrentQueue < IBoundCommandOperation > opqueue =
            new ConcurrentQueue < IBoundCommandOperation > ( ) ;

        private ObservableCollection < Task < bool > > waitingTasks =
            new ObservableCollection < Task < bool > > ( ) ;

        private IWorkspacesViewModel _viewModel = new DesignWorkspacesViewModel ( ) ;

        internal struct DesignWorkspacesViewModel : IWorkspacesViewModel
        {
            private bool                                    _processing ;
            private string                                  _currentProject ;
            private string                                  _currentDocumentPath ;
            private VisualStudioInstancesCollection         _vsCollection ;
            private MyProjectLoadProgress                   _currentProgress ;
            private ObservableCollection < ILogInvocation > _logInvocations ;
            private IPipelineViewModel                      _pipelineViewModel ;
            private IProjectBrowserViewModoel               _projectBrowserViewModel ;
            private PipelineResult                          _pipelineResult ;
            private string                                  _applicationMode ;
            #region Implementation of INotifyPropertyChanged
            public event PropertyChangedEventHandler PropertyChanged ;
            #endregion

            #region Implementation of IAppState
            public bool Processing { get => _processing ; set => _processing = value ; }

            public string CurrentProject
            {
                get => _currentProject ;
                set => _currentProject = value ;
            }

            public string CurrentDocumentPath
            {
                get => _currentDocumentPath ;
                set => _currentDocumentPath = value ;
            }
            #endregion

            #region Implementation of IWorkspacesViewModel
            public VisualStudioInstancesCollection VsCollection
            {
                get => _vsCollection ;
                set => _vsCollection = value ;
            }

            public MyProjectLoadProgress CurrentProgress
            {
                get => _currentProgress ;
                set => _currentProgress = value ;
            }

            public ObservableCollection < ILogInvocation > LogInvocations
            {
                get => _logInvocations ;
                set => _logInvocations = value ;
            }

            public IPipelineViewModel PipelineViewModel
            {
                get => _pipelineViewModel ;
                set => _pipelineViewModel = value ;
            }

            public IProjectBrowserViewModoel ProjectBrowserViewModel
            {
                get => _projectBrowserViewModel ;
                set => _projectBrowserViewModel = value ;
            }

            public PipelineResult PipelineResult
            {
                get => _pipelineResult ;
                set => _pipelineResult = value ;
            }

            public Task AnalyzeCommand ( object viewCurrentItem ) { return null ; }

            public string ApplicationMode => _applicationMode = "Design Mode" ;
            #endregion
        }

        public event RoutedEventHandler TaskCompleted
        {
            add => AddHandler ( TaskCompleteEvent , value ) ;
            remove => RemoveHandler ( TaskCompleteEvent , value ) ;
        }

        public static RoutedEvent TaskCompleteEvent =
            EventManager.RegisterRoutedEvent (
                                              "task completed"
                                            , RoutingStrategy.Direct
                                            , typeof ( RoutedEventHandler )
                                            , typeof ( ProjMainWindow )
                                             ) ;

        public ActionBlock < Workspace > WorkspaceActionBlock { get ; private set ; }

        private ITargetBlock < string > DataflowHead { get ; }

        private static bool Fault ( Task task )
        {
            Logger.Error ( task.Exception , "faulted: {msg}" , task.Exception.ToString ( ) ) ;
            return false ;
        }

        private void ContinuationFunction ( Task task )
        {
            if ( task.IsFaulted )
            {
                Logger.Error ( task.Exception , "Faulted" ) ;

            }
            else { Logger.Info ( "successful" ) ; }
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {

            AdhocWorkspace workspace = new AdhocWorkspace ( ) ;
            WorkspaceTable table = new WorkspaceTable ( ) ;
            table.Show ( ) ;
        }

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            if ( e.OriginalSource is ListView )
            {
                var v = ProjectBrowser.TryFindResource ( "Root" ) as CollectionViewSource ;

                ViewModel.AnalyzeCommand ( v.View.CurrentItem ) ;
            }
            else
            {
                CodeAnalyseContext c = CodeAnalyseContext.Parse (
                                                                 File.ReadAllText (
                                                                                   @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\Root\src\KayMcCormick.Dev\KayMcCormick.Dev\Logging\AppLoggingConfigHelper.cs"
                                                                                  )
                                                               , "test"
                                                                ) ;
                var w = Scope.Resolve < CompilationView > (
                                                           new TypedParameter (
                                                                               typeof (
                                                                                   CodeAnalyseContext
                                                                               )
                                                                             , c
                                                                              )
                                                          ) ;
                w.Show ( ) ;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        internal interface ICodeReference
        {
            Workspace Workspace { get ; }

            Triple Trifecta { get ; }
        }

    }
}
