using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Navigation ;
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
        private Frame _targetFrame ;

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
        public ResourcesTreeView ( AllResourcesTreeViewModel viewModel, object frameDoc )
        {
            _viewModel = viewModel ;
            if ( frameDoc is Frame tFrame )
            {
                TargetFrame = tFrame ;
            }
            InitializeComponent();
        }

        public Frame TargetFrame { get { return _targetFrame ; } set { _targetFrame = value ; } }

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

        private void TreeView_OnSelectedItemChanged (
            object                                    sender
          , RoutedPropertyChangedEventArgs < object > e
        )
        {
            var item = MainTree.SelectedItem ;
            if ( item is ResourceNodeInfo node )
            {
                var data = node.Data ;
                PageFunction < string > loadData = new PageFunction < string > ();
                Page npage= new Page();
                var grid = new Grid ( ) ;
                DebugUtils.WriteLine($"{node}");
                var navigate = TargetFrame.NavigationService.Navigate ( node.Data ) ;
                DebugUtils.WriteLine ( $"{navigate}" ) ;
                    return;



                ContentControl cc = new ContentControl ( ) { Content = node } ;
                
                var element = data.ToString ( ) ;
                var tb = new TextBlock { Text = element } ;
                grid.Children.Add ( tb) ;
                npage.Content = grid ;
                npage.Content = cc ;
                TargetFrame.Content = npage ;

            }
            if ( item is IHierarchicalNode )
            {
                
            }
        }
    }
}
