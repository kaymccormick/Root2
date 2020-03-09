using AnalysisControls ;
using AnalysisFramework ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis;
using NLog ;
using ProjLib ;
using System ;
using System.Collections.Concurrent ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Runtime.CompilerServices ;
using System.Text ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading ;
using Task = System.Threading.Tasks.Task ;

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

        public ProjMainWindow(IWorkspacesViewModel viewModel, ILifetimeScope scope)
        {
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext() ;
            _factory = new TaskFactory(_taskScheduler);

            ViewModel = viewModel ;
            Scope     = scope ;
            _factory  = new TaskFactory ( _taskScheduler ) ;
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                 invocation
                                                                     => {
                                                                     ViewModel
                                                                        .LogInvocations
                                                                        .Add (
                                                                              invocation
                                                                             ) ;
                                                                 }, new ExecutionDataflowBlockOptions() { TaskScheduler = _taskScheduler}) ;
            // DataflowHead = Pipeline.BuildPipeline(  actionBlock ) ;

            // XamlXmlReader x = new XamlXmlReader();
            //Typography t = Typography.SetCapitals ( FontCapitals.AllSmallCaps ) ;
            // foreach ( var systemFontFamily in Fonts.SystemFontFamilies )
            // {
            //     Logger.Info (
            //                  "{font}"
            //                , systemFontFamily.FamilyNames.Select(pair => $"{pair.Key} = {pair.Value}"));
            //     foreach ( var familyTypeface in systemFontFamily.FamilyTypefaces )
            //     {
            //         Logger.Info ( "{name} {style}" ,familyTypeface.DeviceFontName,familyTypeface.Style ) ;
            //     }
            // }

            var myCacheTarget2 = MyCacheTarget2.GetInstance(1000);
            myCacheTarget2.Cache.SubscribeOn(Scheduler.Default)
                         .Buffer(TimeSpan.FromMilliseconds(100))
                         .Where(x => x.Any())
                         .ObserveOnDispatcher(DispatcherPriority.Background)
                         .Subscribe(
                                    infos => {
                                        // ReSharper disable once UnusedVariable
                                        foreach (var logEventInfo in infos)
                                        {
                                            ViewModel.Events.Add(logEventInfo);
                                            // flow.Document.Blocks.Add (
                                            // new Paragraph (
                                            // new Run (
                                            // logEventInfo
                                            // .FormattedMessage
                                            // )
                                            // )
                                            // ) ;
                                        }
                                    }
                                   );


            var myCacheTarget = MyCacheTarget.GetInstance(1000);
            myCacheTarget.Cache.SubscribeOn(Scheduler.Default)
                         .Buffer(TimeSpan.FromMilliseconds(100))
                         .Where(x => x.Any())
                         .ObserveOnDispatcher(DispatcherPriority.Background)
                         .Subscribe(
                                    infos => {
                                        // ReSharper disable once UnusedVariable
                                        foreach (var logEventInfo in infos)
                                        {
                                            ViewModel.EventInfos.Add ( logEventInfo ) ;
                                            // flow.Document.Blocks.Add (
                                                                      // new Paragraph (
                                                                                     // new Run (
                                                                                              // logEventInfo
                                                                                                 // .FormattedMessage
                                                                                             // )
                                                                                    // )
                                                                     // ) ;
                                        }
                                    }
                                   );
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
            private bool _processing ;
            private string _currentProject ;
            private string _currentDocumentPath ;
            #if VSSETTINGS
            private VisualStudioInstancesCollection _vsCollection ;
#endif
            private MyProjectLoadProgress _currentProgress ;
            private ObservableCollection < ILogInvocation > _logInvocations ;
            private IPipelineViewModel _pipelineViewModel ;
            private IProjectBrowserViewModel _projectBrowserViewModel ;
            private PipelineResult _pipelineResult ;
            private string _applicationMode ;
            private AdhocWorkspace _workspace ;
            private ObservableCollection < LogEventInfo > _eventInfos ;
            private ObservableCollection < string > _events ;
            #region Implementation of INotifyPropertyChanged
            public event PropertyChangedEventHandler PropertyChanged ;
            #endregion

            #region Implementation of IAppState
            public bool Processing { get => _processing ; set => _processing = value ; }

            public string CurrentProject { get => _currentProject ; set => _currentProject = value ; }

            public string CurrentDocumentPath { get => _currentDocumentPath ; set => _currentDocumentPath = value ; }
            #endregion

            #region Implementation of IWorkspacesViewModel
            #if VSSETTINGS
            public VisualStudioInstancesCollection VsCollection { get => _vsCollection ; set => _vsCollection = value ; }
#endif

            public MyProjectLoadProgress CurrentProgress { get => _currentProgress ; set => _currentProgress = value ; }

            public ObservableCollection < ILogInvocation > LogInvocations { get => _logInvocations ; set => _logInvocations = value ; }

            public IPipelineViewModel PipelineViewModel { get => _pipelineViewModel ; set => _pipelineViewModel = value ; }

            public IProjectBrowserViewModel ProjectBrowserViewModel { get => _projectBrowserViewModel ; set => _projectBrowserViewModel = value ; }

            public PipelineResult PipelineResult { get => _pipelineResult ; set => _pipelineResult = value ; }

            public Task AnalyzeCommand ( object viewCurrentItem ) { return null ; }

            public string ApplicationMode { get => _applicationMode ; set => _applicationMode = value ; }

            public AdhocWorkspace Workspace { get => _workspace ; set => _workspace = value ; }

            public ObservableCollection < LogEventInfo > EventInfos { get => _eventInfos ; set => _eventInfos = value ; }

            public ObservableCollection < string > Events { get => _events ; set => _events = value ; }
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

        private ConcurrentBag <Task> _tasks = new ConcurrentBag < Task > ();
        private TaskScheduler _taskScheduler ;
        private ObservableCollection<TaskWrap> _obsTasks = new ObservableCollection < TaskWrap > ();

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

        private async void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            if ( e.OriginalSource is ListView )
            {
                var v = _projectBrowser.TryFindResource ( "Root" ) as CollectionViewSource ;

                Task t = ViewModel.AnalyzeCommand ( v.View.CurrentItem ) ;
                TaskWrap tw = new TaskWrap ( t , "Analyze Command" ) ;
                AddTask ( t, tw ) ;
            }
            else
            {
                var path = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\Root\src\KayMcCormick.Dev\KayMcCormick.Dev\Logging\AppLoggingConfigHelper.cs" ;
                ISemanticModelContext c = AnalysisService.Parse (
                                                                 File.ReadAllText (
                                                                                   path
                                                                                  ), "test"
                                                                ) ;
                ViewModel.Workspace = new AdhocWorkspace();
                
                var projectId = ProjectId.CreateNewId("Test") ;
                ViewModel.Workspace.AddProject(ProjectInfo.Create(projectId, VersionStamp.Create(), "test project", "tesst", LanguageNames.CSharp));
                  ViewModel.Workspace.AddDocument(DocumentInfo.Create(DocumentId.CreateNewId(projectId, "test"), "test", null, SourceCodeKind.Regular, new FileTextLoader(path, Encoding.UTF8), path));
                  ViewModel.Workspace.CurrentSolution.Projects.First ( )
                           .Documents.First ( )
                           .GetSemanticModelAsync ( )
                           .ContinueWith (
                                          ( task  ) => {
                                              var w = Scope.Resolve < CompilationView > (
                                                                                         new
                                                                                             TypedParameter (
                                                                                                             typeof
                                                                                                                 ( ICodeAnalyseContext
                                                                                                             )
                                                                                                           , new
                                                                                                                 CodeAnalyseContext2 (
                                                                                                                                      task
                                                                                                                                         .Result
                                                                                                                                     )
                                                                                                            )
                                                                                        ) ;
                                              w.Show ( ) ;
                                          }, TaskScheduler.FromCurrentSynchronizationContext()
                                         ) ;

            }
        }

        private void AddTask ( Task task , TaskWrap tw )
        {
            _obsTasks.Add(tw);
            task.ContinueWith (
                               delegate ( Task t ) {
                                   tw.Status = t.Status.ToString() ;
                                   //ObsTasks.Remove ( t ) ;
                               }, _taskScheduler
                              ) ;
            _tasks.Add ( task ) ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        private void DataGrid_OnAutoGeneratingColumn (
            object                                sender
          , DataGridAutoGeneratingColumnEventArgs e
        )
        {
            switch ( e.PropertyName )
            {
                case "StackTrace" :
                case "MessageTemplateParameters":
                case "HasStackTrace":
                case "UserStackFrame":
                case "UserStackFrameNumber":
                case "CallerClassName":
                case "CallerMemberName":
                case "CallerFilePath":
                case "CallerLineNumber":
                case "LoggerShortName":
                case "MessageFormatter":
                case "Message":
                    case "HasProperties":

                    e.Cancel = true ;
                    break ;
                
            }
        }

        #region Implementation of IView1
        public string ViewTitle => "Main View" ;

        public ObservableCollection < TaskWrap > ObsTasks
        {
            get => _obsTasks ;
            set => _obsTasks = value ;
        }
        #endregion
    }

    public class TaskWrap : INotifyPropertyChanged
    {
        private string _status ;

        public Task Task { get ; }

        public string Desc { get ; }

        public string Status
        {
            get => _status ;
            set
            {
                _status = value ;
                OnPropertyChanged();
                OnPropertyChanged("Task");
            }
        }

        public TaskWrap ( Task task , string desc )
        {
            Task = task ;
            Desc = desc ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}
