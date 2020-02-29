using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon ;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading ;
using System.Xaml;
using Autofac;
using Autofac.Core.Lifetime;
using Microsoft.CodeAnalysis; 
using Microsoft.VisualStudio.Shell ;
using NLog;
using ProjLib;
using Xunit ;
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

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            AnalyzeResults results = new AnalyzeResults(ViewModel);
            results.ShowActivated = true ;
            results.Show ( ) ;
            var sender2SelectedItem = (IMruItem)mru.SelectedItem ;
            var vsSelectedItem = ( VsInstance ) vs.SelectedItem ;
            var workspacesViewModel = ViewModel ;
            Cursor = Cursors.Wait ;
            codeWindow  = new CodeWindow();
            codeWindow.Show ( ) ;
            
                Task.Run (
                      ( ) => {
                          workspacesViewModel
                             .LoadSolutionAsync ( vsSelectedItem , sender2SelectedItem,  _factory, new DispatcherSynchronizationContext())
                             .ContinueWith (
                                            ContinuationFunction
                                           ) ;
                      }
                     ) ;

        }

        private async Task<object> ContinuationFunction ( Task task )
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
