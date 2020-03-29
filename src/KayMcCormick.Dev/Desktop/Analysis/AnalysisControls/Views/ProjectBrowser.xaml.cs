using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    /// <summary>
    ///     Interaction logic for ProjectBrowser.xaml
    /// </summary>
    [ TitleMetadata ( "Project Browser" ) ]
    public partial class ProjectBrowser : UserControl
      , IViewWithTitle
      , IView < IProjectBrowserViewModel >
      , IControlView
    {
        private readonly IProjectBrowserViewModel _viewModel ;

        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        public ProjectBrowser ( IProjectBrowserViewModel viewModel )
        {
            InitializeComponent ( ) ;
            _viewModel = viewModel ;
        }

        /// <summary>
        /// </summary>
        public IProjectBrowserViewModel ViewModel { get { return _viewModel ; } }

        /// <summary>
        /// </summary>
        [ NotNull ] public string ViewTitle { get { return "Project Browser" ; } }

        private void Selector_OnSelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            if ( TryFindResource ( "Root" ) is CollectionViewSource v )
            {
                v.View?.MoveCurrentTo ( e.AddedItems[ 0 ] ) ;
            }
        }
    }
}