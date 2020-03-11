using System.Windows;
using ProjLib;
using ProjLib.Interfaces ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for AnalyzeResults.xaml
    /// </summary>
    public partial class AnalyzeResults : Window
    {
        public IWorkspacesViewModel ViewModel { get ; }
        public AnalyzeResults ( IWorkspacesViewModel workspacesViewModel )
        {
            ViewModel = workspacesViewModel ;
            InitializeComponent();
        }
    }
}
