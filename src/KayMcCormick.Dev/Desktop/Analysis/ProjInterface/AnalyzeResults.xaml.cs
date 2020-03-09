using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.CodeAnalysis.CSharp;
using ProjLib;
using Xunit.Sdk;

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
