using System.Windows.Controls;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf.ViewModel ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for ResourcesTreeView.xaml
    /// </summary>
    public partial class ResourcesTreeView : UserControl, IControlView, IView1, IView <AllResourcesTreeViewModel>
    {
        private AllResourcesTreeViewModel _viewModel ;

        /// <summary>
        /// 
        /// </summary>
        public ResourcesTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public ResourcesTreeView ( AllResourcesTreeViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent();
        }

        #region Implementation of IView<out AllResourcesTreeViewModel>
        /// <summary>
        /// 
        /// </summary>
        public AllResourcesTreeViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        #endregion
    }
}
