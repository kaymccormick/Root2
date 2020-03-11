using System.Windows.Controls ;
using System.Windows.Data ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for ProjectBrowser.xaml
    /// </summary>
    public partial class ProjectBrowser : UserControl
    {
        public ProjectBrowser()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            var v =TryFindResource("Root") as CollectionViewSource;
            v.View.MoveCurrentTo ( e.AddedItems[ 0 ] ) ;
        }
    }
}
