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
