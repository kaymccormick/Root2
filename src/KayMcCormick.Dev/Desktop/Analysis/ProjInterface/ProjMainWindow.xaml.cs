using System;
using System.Collections.Concurrent ;
using System.Collections.ObjectModel ;
using System.Linq;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon ;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading ;
using Autofac;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis;
using NLog;
using ProjLib;
using Task = System.Threading.Tasks.Task ;

namespace ProjInterface
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjMainWindow : Window
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TaskFactory _factory ;
        private CodeWindow codeWindow ;

        public IWorkspacesViewModel ViewModel { get; set; }

        public ProjMainWindow(IWorkspacesViewModel viewModel)
        {
            ViewModel = viewModel ;
            _factory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()) ;
            var actionBlock = new ActionBlock < LogInvocation > (
                                                                 invocation
                                                                     => {
                                                                     ViewModel
                                                                        .LogInvocations
                                                                        .Add (
                                                                              invocation
                                                                             ) ;
                                                                 }, new ExecutionDataflowBlockOptions() { TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()}) ;
            // DataflowHead = Pipeline.BuildPipeline(  actionBlock ) ;
            if (! viewModel.VsCollection.Any ( ) )
            {
                throw new Exception("no data");
            }

            ((WorkspacesViewModel)viewModel)._d = Dispatcher;

            InitializeComponent();
            // XamlXmlReader x = new XamlXmlReader();
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

        private void ButtonBase_OnClick ( object sender , RoutedEventArgs e )
        {
            GridView v = ( GridView ) vs.View ;
            new CollectionView(ViewModel.VsCollection).Refresh (  );
        }

        private ConcurrentQueue < IBoundCommandOperation > opqueue = new ConcurrentQueue < IBoundCommandOperation > ();
        private ObservableCollection < Task < bool > > waitingTasks = new ObservableCollection < Task < bool > > ();

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

        private void PostPath ( string filePath ) { _ = ViewModel.PipelineViewModel.Pipeline.PipelineInstance.Post ( filePath ) ; }

        private async Task < object > ContinuationFunction ( Task task )
        {
            return ViewModel.ProcessSolutionAsync (
                                                  Dispatcher
                                                , _factory
                                                , codeWindow.GetFormattedCode
                                                 ) ;
        }

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
    }

    internal interface ICodeReference
    {
        Workspace Workspace { get ; }
        Triple Trifecta  { get ; }
    }

    public class WorkspaceTable : RibbonWindow
    {
        public WorkspaceTable ( )
        {
            Ribbon ribbon = new Ribbon ( ) ;
            RibbonQuickAccessToolBar qat = new RibbonQuickAccessToolBar ( ) ;
            qat.Items.Add(new RibbonButton());
            qat.Items.Add(new RibbonButton());
            ribbon.QuickAccessToolBar = qat ;
            RibbonApplicationMenu appMenu = new RibbonApplicationMenu();
            ribbon.ApplicationMenu = appMenu;
                appMenu.Items.Add (
                                              new RibbonApplicationMenuItem { Header = "Fun Times" }
                                             ) ;
            var filesTab = new RibbonTab { Header = "Files" } ;
            ribbon.Items.Add ( filesTab );
            RibbonGroup group = new RibbonGroup ( ) { Header = "Create" } ;
            group.Items.Add(new RibbonButton { Content = "CSharp" });
            group.Items.Add(new RibbonButton { Content = "XAML" });
            var content = new Grid ( ) ;
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add (
                                        new RowDefinition
                                        {
                                            Height = new GridLength ( 1 , GridUnitType.Star )
                                        }
                                       ) ;
            ribbon.SetValue(Grid.RowProperty, 0);
            ribbon.SetValue ( Grid.ColumnProperty , 0 ) ;
            // ribbon.SetValue ( Grid.ColumnSpanProperty , content.ColumnDefinitions.Count ) ;
            content.Children.Add ( ribbon ) ;
            Content = content ;
        }
    }
}
