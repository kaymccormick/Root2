using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using JetBrains.Annotations ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    public partial class LogViewerWindow : AppWindow, INotifyPropertyChanged
    {
        AppViewModel appViewModel = new AppViewModel();
        public LogViewerWindow()
        {

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        public AppViewModel ViewModel { get => appViewModel ; set => appViewModel = value ; }

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
            LogViewModel viewModel = new LogViewModel();
            LogListener x = new LogListener(port, viewModel);
            viewModel.DisplayName = port.ToString ( ) ;
            appViewModel.LogViewModels.Add(viewModel);
            mainTabControl.SelectedItem = viewModel ;
            x.Start();
        }

     
    }
    
}
