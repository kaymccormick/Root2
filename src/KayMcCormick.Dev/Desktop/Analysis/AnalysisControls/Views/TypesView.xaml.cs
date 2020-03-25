using System.ComponentModel ;
using System.Diagnostics;
using System.Runtime.CompilerServices ;
using System.Windows.Controls ;
using System.Windows.Media ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for Types.xaml
    /// </summary>
    [TitleMetadata("Types view")]
    public sealed partial class TypesView : UserControl , IView < ITypesViewModel > , IViewWithTitle, IControlView, INotifyPropertyChanged
    {
        public TypesView ( ) {
        }

        private ITypesViewModel _viewModel ;

        public TypesView ( ITypesViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            var childrenCount = VisualTreeHelper.GetChildrenCount ( this ) ;
            for ( int i = 0 ; i < childrenCount ; i ++ )
            {
                var dependencyObject = VisualTreeHelper.GetChild ( this , i ) ;
                Visual v = ( Visual ) dependencyObject ;
                var contentBounds = VisualTreeHelper.GetContentBounds ( v ) ;
                Debug.WriteLine ($"{v}{i} {contentBounds.Left},{contentBounds.Top} - {contentBounds.Right},{contentBounds.Bottom}");
            }
        }
        #endregion

        #region Implementation of IView<ITypesViewModel>
        public ITypesViewModel ViewModel
        {
            get { return _viewModel ; }
            set
            {
                _viewModel = value ;
                Debug.WriteLine ( "Set viewModel" ) ;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Implementation of IView1
        [ NotNull ] public string ViewTitle
        {
            get { return "Types View" ; }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}