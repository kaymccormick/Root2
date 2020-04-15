using System.Windows.Controls;
using System.Windows.Navigation;
#if EXPLORER
using ExplorerCtrl ;
#endif



namespace TestApp
{
    /// <summary>
    /// Interaction logic for DockLayout.xaml
    /// </summary>
    public partial class DockLayout : UserControl
    {
        public DockLayout()
        {
            InitializeComponent();
            // Explorer explorer = new Explorer();
            // explorer.BeginInit();
            // explorer.EndInit();
            // var x = explorer.TreeView.Parent ;
            // test.Content = explorer.TreeView ;
        }

        private void BrowserFrame_OnNavigating ( object sender , NavigatingCancelEventArgs e ) { }
    }
}
