using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using Autofac;
using Autofac.Core.Lifetime;
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
        public IWorkspacesViewModel ViewModel { get; set; }

        public ProjMainWindow(IWorkspacesViewModel viewModel)
        {
            ViewModel = viewModel ;
            if (! viewModel.VsCollection.Any ( ) )
            {
                throw new Exception("no data");
            }
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
            Task.Run (
                      ( ) => {
                          workspacesViewModel
                             .LoadSolutionAsync ( vsSelectedItem , sender2SelectedItem )
                             .ContinueWith (
                                            ContinuationFunction
                                           ) ;
                      }
                     ) ;

        }

        private async Task ContinuationFunction ( Task task )
        {
            await ViewModel.ProcessSolutionAsync(Dispatcher);

        }
    }
}
