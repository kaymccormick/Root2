using System.Windows.Controls ;
using System.Windows.Input ;
using AnalysisAppLib;
using AnalysisControls.ViewModel ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls
{
    /// <summary>
    ///     Interaction logic for PythonControl.xaml
    /// </summary>
    [ TitleMetadata ( "Python" ) ]
    [ GroupMetadata ( "Views" ) ]
    public sealed partial class PythonControl : UserControl
      , IView < PythonViewModel >
      , IView1
      , IControlView
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILifetimeScope _scope ;

        /// <summary>
        /// 
        /// </summary>
        public PythonControl ( ) : this ( null , null ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="viewModel"></param>
        [UsedImplicitly]
        public PythonControl ( ILifetimeScope scope , PythonViewModel viewModel )
        {
            _scope    = scope ;
            ViewModel = viewModel ;
            InitializeComponent ( ) ;

            ViewModel.FlowDocument = Flow ;
        }

        #region Implementation of IView<out PythonViewModel>
        /// <summary>
        /// Retreive the view model.
        /// </summary>
        public PythonViewModel ViewModel { get ; }
        #endregion

        // ReSharper disable once UnusedMember.Local
        private void UIElement_OnKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
        {
            switch ( e.Key )
            {
                case Key.Enter :
                {
                    DebugUtils.WriteLine ( $"received key {e.Key}" ) ;
                    var textBox = ( TextBox ) sender ;
                    ViewModel.TakeLine ( textBox.Text ) ;
                    textBox.Text = "" ;
                    break ;
                }
                case Key.Up :
                    ViewModel.HistoryUp ( ) ;
                    break ;
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private void UIElement_OnPreviewKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
        {
            DebugUtils.WriteLine ( $"received key {e.Key}" ) ;
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch ( e.Key )
            {
                case Key.Up :
                    ViewModel.HistoryUp ( ) ;
                    e.Handled = true ;
                    break ;
                case Key.Down :
                    ViewModel.HistoryDown ( ) ;
                    e.Handled = true ;
                    break ;
            }
        }
    }
}