using System.Windows.Controls ;
using ProjLib.Interfaces ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for TestView1.xaml
    /// </summary>
    public partial class TestView1 : UserControl, IView1
    {
        private string _viewTitle = "Test View 1";

        public TestView1()
        {
            InitializeComponent();
        }

        #region Implementation of IView1
        public string ViewTitle { get => _viewTitle ; set => _viewTitle = value ; }
        #endregion
    }
}
