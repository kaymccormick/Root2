using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using ProjLib.Interfaces;
using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisAppLib ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for ProjectBrowser.xaml
    /// </summary>
    [TitleMetadata("Project Browser")]
    public partial class ProjectBrowser : UserControl, IView1, IView<IProjectBrowserViewModel>, IControlView
    {
        private IProjectBrowserViewModel _viewModel;

        public ProjectBrowser(IProjectBrowserViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        public string ViewTitle => "Project Browser";

        public IProjectBrowserViewModel ViewModel => _viewModel;

        private void Selector_OnSelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            if ( TryFindResource("Root") is CollectionViewSource v )
            {
                if ( v.View != null )
                {
                    v.View.MoveCurrentTo ( e.AddedItems[ 0 ] ) ;
                }
            }
        }
    }
}
