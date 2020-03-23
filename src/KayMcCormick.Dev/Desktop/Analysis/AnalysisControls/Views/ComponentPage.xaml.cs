using System.Windows.Controls ;
using AnalysisAppLib ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.Interfaces ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for ComponentPage.xaml
    /// </summary>
    public partial class ComponentPage : Page
    {
        public IComponentViewModel ViewModel { get ; }

        public ComponentPage(IComponentViewModel viewModel)
        {
            ViewModel = viewModel ;
            InitializeComponent();
        }
    }
}
