using System.Windows.Controls ;
using KayMcCormick.Lib.Wpf ;
using ProjLib.Interfaces ;

namespace AnalysisControls.Views
{
    [TitleMetadata("TEst View 1")]
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
