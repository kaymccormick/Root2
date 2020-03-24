using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using AnalysisAppLib ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    [TitleMetadata("Log Viewer Window")]
#pragma warning disable DV2002 // Unmapped types
    public partial class LogViewerWindow : AppWindow, INotifyPropertyChanged, IViewWithTitle, IView<LogViewerAppViewModel>
#pragma warning restore DV2002 // Unmapped types
    {
        private string _viewTitle ;
        private LogViewerAppViewModel _logViewerAppViewModel ;

        public LogViewerWindow(ILifetimeScope scope, LogViewerAppViewModel logViewerAppViewModel) : base(scope)
        {
            _logViewerAppViewModel = logViewerAppViewModel ;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged ;

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
            int port = int.Parse ( this.port.Text ) ;
            LogViewModel logViewModel = new LogViewModel();
            LogListener x = new LogListener(port, logViewModel);
            logViewModel.DisplayName = port.ToString ( ) ;
            _logViewerAppViewModel.LogViewModels.Add(logViewModel);
            mainTabControl.SelectedItem = logViewModel ;
            x.Start();
        }


        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion

        #region Implementation of IView<out LogViewerAppViewModel>
        public LogViewerAppViewModel ViewModel { get { return _logViewerAppViewModel ; } set { _logViewerAppViewModel = value ; } }
        #endregion
    }
    
}
