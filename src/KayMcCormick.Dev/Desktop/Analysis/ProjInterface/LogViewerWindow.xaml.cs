using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using System.Windows.Controls.Primitives ;
using AnalysisAppLib ;
using AnalysisAppLib.XmlDoc ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    [ TitleMetadata ( "Log Viewer Window" ) ]
    public partial class LogViewerWindow : AppWindow
      , INotifyPropertyChanged
      , IViewWithTitle
      , IView < LogViewerAppViewModel >

    {
        private string _viewTitle ;

        public LogViewerWindow (
            ILifetimeScope        scope
          , LogViewerAppViewModel logViewerAppViewModel
        ) : base ( scope )
        {
            ViewModel = logViewerAppViewModel ;
            InitializeComponent ( ) ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of IView<out LogViewerAppViewModel>
        public LogViewerAppViewModel ViewModel { get ; set ; }
        #endregion


        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion

        [ NotifyPropertyChangedInvocator ]
        protected void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        private void ButtonBase_OnClick ( object sender , RoutedEventArgs e )
        {
            // tv.InvalidateProperty ( ItemsControl.ItemsSourceProperty ) ;
        }

        private void ButtonBase_OnClick2 ( object sender , RoutedEventArgs e )
        {
            var port = int.Parse ( this.port.Text ) ;
            var logViewModel = new LogViewModel ( ) ;
            var x = new LogListener ( port , logViewModel ) ;
            logViewModel.DisplayName = port.ToString ( ) ;
            ViewModel.LogViewModels.Add ( logViewModel ) ;
            mainTabControl.SetCurrentValue ( Selector.SelectedItemProperty , logViewModel ) ;
            x.Start ( ) ;
        }
    }
}