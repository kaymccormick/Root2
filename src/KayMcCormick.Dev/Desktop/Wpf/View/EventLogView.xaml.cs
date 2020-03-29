using System.Windows.Controls ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf.ViewModel ;

namespace KayMcCormick.Lib.Wpf.View
{
    /// <summary>
    ///     Interaction logic for EventLogView.xaml
    /// </summary>
    public partial class EventLogView : UserControl
      , IView1
      , IView < EventLogViewModel >
      , IControlView
    {
        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        public EventLogView ( EventLogViewModel viewModel )
        {
            ViewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        /// <summary>
        /// </summary>
        public EventLogView ( ) : this ( null ) { }

        #region Implementation of IView<out EventLogViewModel>
        /// <summary>
        /// </summary>
        public EventLogViewModel ViewModel { get ; }
        #endregion

        #region Overrides of FrameworkElement
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate ( ) { ViewModel.View = this ; }
        #endregion
    }
}