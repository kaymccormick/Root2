using System.Windows.Controls ;
using AnalysisAppLib.ViewModel ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for Types.xaml
    /// </summary>
    [TitleMetadata("Types view")]
    public partial class TypesView : UserControl , IView < ITypesViewModel > , IView1, IControlView
    {
        private ITypesViewModel _viewModel ;

        public TypesView ( ITypesViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        #region Implementation of IView<ITypesViewModel>
        public ITypesViewModel ViewModel { get => _viewModel ; set => _viewModel = value ; }
        #endregion

        #region Implementation of IView1
        public string ViewTitle => "Types View" ;
        #endregion
    }
}