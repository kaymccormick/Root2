#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// DockWindowViewModel.cs
// 
// 2020-03-16-10:04 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Threading.Tasks ;
using System.Windows ;
using Autofac.Features.Metadata ;
using DynamicData ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.Identity.Client ;
using NLog ;
using Logger = NLog.Logger ;

namespace ProjInterface
{
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly ObservableCollection < IExplorerItem > _rootCollection =
            new ObservableCollection < IExplorerItem > ( ) ;


        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;
        private readonly IIconsSource                             _iconsSource ;
        private readonly IEnumerable < IExplorerItemProvider >    _providers ;
        private readonly IPublicClientApplication                 _publicClient ;
        private          string                                   _defaultInputPath ;
        private          FrameworkElement                         _resourcesElement ;
        private          IDictionary                              _iconsResources ;
        private          IntPtr                                   _hWnd ;
        private          IAccount                                 _account ;

        public IAccount Account
        {
            get { return _account ; }
            private set
            {
                if ( Equals ( value , _account ) )
                {
                    return ;
                }

                _account = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public async Task Login ( )
        {
            var scopes = new string[] { "user.read" } ;
            var app = _publicClient ;
            var accounts = await app.GetAccountsAsync ( ) ;
            AuthenticationResult result ;
            try
            {
                result = await app.AcquireTokenSilent ( scopes , accounts.FirstOrDefault ( ) )
                                  .ExecuteAsync ( ) ;
            }
            catch ( MsalUiRequiredException )
            {
                result = await app.AcquireTokenInteractive ( scopes ).ExecuteAsync ( ) ;
            }

            Account = result.Account ;
        }

        public bool CanLogin => Account == null ;

        public async Task < bool > LoginSilentAsync ( )
        {
            var app = _publicClient ;
            var accounts = await app.GetAccountsAsync ( ) ;
            var account = accounts.FirstOrDefault ( ) ;

            var scopes = new string[] { "user.read" } ;
            // if the app manages is at most one account  
            AuthenticationResult result ;
            try
            {
                result = await app.AcquireTokenSilent ( scopes , account ).ExecuteAsync ( ) ;
            }
            catch ( MsalUiRequiredException ex )
            {
                return false ;
            }

            Account = result.Account ;
            return true ;
        }

        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IView1 > > > views
          , IIconsSource                             iconsSource
          , IEnumerable < IExplorerItemProvider >    providers
          , Func < Guid , IPublicClientApplication > publicClientFunc
        )

        {
            Logger.Debug ( "Constructor" ) ;
            _views       = views ;
            _iconsSource = iconsSource ;
            _providers   = providers ;
            _publicClient =
                publicClientFunc ( Guid.Parse ( "73d9e90c-5cd2-4fd7-9e36-4faab9404a7c" ) ) ;

            foreach ( var explorerItemProvider in _providers )
            {
                if ( explorerItemProvider is ITakesHwnd h )
                {
                    h.SetHwnd ( _hWnd ) ;
                }

                var items = explorerItemProvider.GetRootItems ( ) ;
                foreach ( var item in items )
                {
                    if ( item.IsDirectory )
                    {
                        Logger.Info ( "{item} is directory" , item.FullName ) ;
                    }

                    RootCollection.Add ( item ) ;
                }
            }
        }

        public IEnumerable < Meta < Lazy < IView1 > > > Views { get { return _views ; } }

        public ObservableCollection < IExplorerItem > RootCollection
        {
            get { return _rootCollection ; }
        }

        public FrameworkElement ResourcesElement
        {
            get { return _resourcesElement ; }
            set
            {
                _resourcesElement = value ;
                if ( _resourcesElement != null )
                {
                    // var item = new FileSystemAppExplorerItem ( DefaultInputPath , _iconsSource ) ;
                    // RootCollection.AddRange ( item.Children.Where ( item1 => item1.IsDirectory ) ) ;
                    // _iconsResources =
                    //     _resourcesElement.TryFindResource ( "IconsResources" ) as IDictionary ;
                }
            }
        }


        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
        }

        public IntPtr GethWnd ( ) { return _hWnd ; }

        public void SethWnd ( IntPtr value ) { _hWnd = value ; }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    public interface ITakesHwnd
    {
        void SetHwnd ( IntPtr hWnd ) ;
    }
}