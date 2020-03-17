using System.Windows.Controls ;
using System.Windows.Data ;

namespace AnalysisControls.Views
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
