using System;
using System.Collections.Concurrent ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Runtime.CompilerServices ;
using System.Text ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows;
using System.Windows.Controls ;
using System.Windows.Data;
using System.Windows.Documents ;
using System.Windows.Input;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using AnalysisControls ;
using AnalysisFramework ;
using Autofac;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis;
using NLog;
using ProjLib;
using Task = System.Threading.Tasks.Task ;

namespace ProjInterface
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjMainWindow : Window, IView<IWorkspacesViewModel>, IView1, IWorkspacesView, INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TaskFactory _factory ;

        public IWorkspacesViewModel ViewModel
        {
            get => _viewModel ;
            set
            {
                _viewModel = value ;
                OnPropertyChanged();
            }
        }

        public ProjMainWindow ( ) {
            InitializeComponent();
            _factory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ProjMainWindow(IWorkspacesViewModel viewModel, ILifetimeScope scope) : this()
        {
            
            ViewModel = viewModel ;
            Scope = scope ;
            
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                 invocation
                                                                     => {
                                                                     ViewModel
                                                                        .LogInvocations
                                                                        .Add (
                                                                              invocation
                                                                             ) ;
                                                                 }, new ExecutionDataflowBlockOptions() { TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()}) ;
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
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Logger.Info("{methodName} {typeEvent}", nameof(OnInitialized), e.GetType());
            // Scope = Container.GetContainer();
            // ViewModel = Scope.Resolve<IWorkspacesViewModel>();
            // ViewModel.BeginInit();
        }

        public ILifetimeScope Scope { get; set; }
        #if false
        private void ButtonBase_OnClick ( object sender , RoutedEventArgs e )
        {
            GridView v = ( GridView ) vs.View ;
            new CollectionView(ViewModel.VsCollection).Refresh (  );
        }
        #endif
        private ConcurrentQueue < IBoundCommandOperation > opqueue = new ConcurrentQueue < IBoundCommandOperation > ();
        private ObservableCollection < Task < bool > > waitingTasks = new ObservableCollection < Task < bool > > ();
        private IWorkspacesViewModel _viewModel = new DesignWorkspacesViewModel() ;

        internal struct DesignWorkspacesViewModel : IWorkspacesViewModel
        {
            private bool _processing;
            private string _currentProject;
            private string _currentDocumentPath;
            private VisualStudioInstancesCollection _vsCollection;
            private MyProjectLoadProgress _currentProgress;
            private ObservableCollection<ILogInvocation> _logInvocations;
            private IPipelineViewModel _pipelineViewModel;
            private IProjectBrowserViewModoel _projectBrowserViewModel;
            private PipelineResult _pipelineResult;
            private string _applicationMode;
            private AdhocWorkspace _workspace ;
            private ObservableCollection < LogEventInfo > _eventInfos ;
            #region Implementation of INotifyPropertyChanged
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            #region Implementation of IAppState
            public bool Processing { get => _processing; set => _processing = value; }

            public string CurrentProject { get => _currentProject; set => _currentProject = value; }

            public string CurrentDocumentPath { get => _currentDocumentPath; set => _currentDocumentPath = value; }
            #endregion

            #region Implementation of IWorkspacesViewModel
            public VisualStudioInstancesCollection VsCollection { get => _vsCollection; set => _vsCollection = value; }

            public MyProjectLoadProgress CurrentProgress { get => _currentProgress; set => _currentProgress = value; }

            public ObservableCollection<ILogInvocation> LogInvocations { get => _logInvocations; set => _logInvocations = value; }

            public IPipelineViewModel PipelineViewModel { get => _pipelineViewModel; set => _pipelineViewModel = value; }

            public IProjectBrowserViewModoel ProjectBrowserViewModel { get => _projectBrowserViewModel; set => _projectBrowserViewModel = value; }

            public PipelineResult PipelineResult { get => _pipelineResult; set => _pipelineResult = value; }

            public Task AnalyzeCommand(object viewCurrentItem) { return null; }

            public string ApplicationMode => _applicationMode = "Design Mode";

            public AdhocWorkspace Workspace { get => _workspace ; set => _workspace = value ; }

            public ObservableCollection < LogEventInfo > EventInfos { get => _eventInfos ; set => _eventInfos = value ; }
            #endregion
        }

        public event RoutedEventHandler TaskCompleted
        {
            add { AddHandler(TaskCompleteEvent, value) ;}
            remove { RemoveHandler(TaskCompleteEvent, value);}
        }
        public static RoutedEvent TaskCompleteEvent =
            EventManager.RegisterRoutedEvent (
                                              "task completed",
                                              RoutingStrategy.Direct
                                            , typeof ( RoutedEventHandler )
                                            , typeof ( ProjMainWindow )
                                             ) ;

        public ActionBlock < Workspace > WorkspaceActionBlock { get ; private set ; }

        private ITargetBlock<string> DataflowHead { get; }
        #if false
        private void PerformAnalysis ( object sender , ExecutedRoutedEventArgs e )
        {
            AnalyzeResults results = new AnalyzeResults ( ViewModel ) { ShowActivated = true } ;
            results.Show ( ) ;
            ViewModel.BrowserVisibility = Visibility.Hidden ;
            this.results.Visibility = Visibility.Visible ;
            var sender2SelectedItem = (IMruItem)mru.SelectedItem ;
            var filePath = sender2SelectedItem.FilePath ;
            PostPath ( filePath ) ;
            // DataflowHead.Completion.ContinueWith (
                                              // ( task ) => {
                                                  // Logger.Info ( "completipn" ) ;
                                              // }
                                             // ) ;
            // result.ContinueWith (
            //                      task => {
            //                          waitingTasks.Remove ( task ) ;
            //                          Logger.Info ( "task complete" ) ;
            //                          ( ( FrameworkElement ) sender ).RaiseEvent (
            //                                                                      new
            //                                                                          RoutedEventArgs (
            //                                                                                           TaskCompleteEvent
            //                                                                                          )
            //                                                                     ) ;
            //                      }, TaskScheduler.Current
            //                     ) ;


            // var vsSelectedItem = ( VsInstance ) vs.SelectedItem ;
            // var workspacesViewModel = ViewModel ;
            // Cursor = Cursors.Wait ;
            // codeWindow  = new CodeWindow();
            // codeWindow.Show ( ) ;
            //
            //     Task.Run (
            //           ( ) => {
            //               workspacesViewModel
            //                  .LoadSolutionAsync ( vsSelectedItem , sender2SelectedItem,  _factory, new DispatcherSynchronizationContext())
            //                  .ContinueWith (
            //                                 ContinuationFunction
            //                                ) ;
            //           }
            //          ) ;

        }
        #endif
        #if false
        private void PostPath ( string filePath )
        {
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                  invocation => {
                                                                      Logger.Debug ( "{invocation}" , invocation ) ;
                                                                  }
                                                                 ) ;
            ViewModel.PipelineViewModel.Pipeline.PipelineInstance.LinkTo ( actionBlock , new DataflowLinkOptions() {PropagateCompletion = true}) ;
            Task.Run (
                      async delegate {
                          await actionBlock.Completion ;
                      }
                     ) ;
            var R = ViewModel.PipelineViewModel.Pipeline.PipelineInstance.Post(filePath);
        }
        #endif
        private static bool Fault ( Task task )
        {
            Logger
               .Error (
                       task
                          .Exception
                     , "faulted: {msg}"
                     , task
                      .Exception
                      .ToString ( )
                      ) ;
            return false;
        }
    

#if false
        private void Mru_OnSelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            var sender2SelectedItem = (IMruItem)mru.SelectedItem;
            var vsSelectedItem = (VsInstance)vs.SelectedItem;
            var workspacesViewModel = ViewModel;

            Task.Run (
                      ( ) => {
                          workspacesViewModel.LoadSolutionAsync (
                                                                 vsSelectedItem
                                                               , sender2SelectedItem
                                                               , _factory, new DispatcherSynchronizationContext()
                                                                ) ;
                      }
                     ) ;

        }
        #endif
        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            AdhocWorkspace workspace = new AdhocWorkspace();
            WorkspaceTable table = new WorkspaceTable ( ) ;
            table.Show ( ) ;
        }

        private void ProjMainWindow_OnDrop ( object sender , DragEventArgs e )
        {
            e.Data.GetData ( DataFormats.FileDrop ) ;
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

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}
