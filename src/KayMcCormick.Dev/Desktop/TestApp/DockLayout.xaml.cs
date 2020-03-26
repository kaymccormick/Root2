using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExplorerCtrl ;

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
