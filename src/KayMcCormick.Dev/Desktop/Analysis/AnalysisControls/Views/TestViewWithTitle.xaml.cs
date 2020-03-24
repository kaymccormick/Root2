using System.Windows.Controls ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    [TitleMetadata("TEst View 1")]
    [ UsedImplicitly ]
    public partial class TestViewWithTitle : UserControl, IViewWithTitle, IControlView
    {
        private string _viewTitle = "Test View 1";

        public TestViewWithTitle()
        {
            InitializeComponent();
        }

        #region Implementation of IView1
        public string ViewTitle { get => _viewTitle ; set => _viewTitle = value ; }
        #endregion
    }
}
