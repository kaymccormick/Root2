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
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Threading.Tasks ;
using System.Windows ;
using Autofac.Features.Metadata ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;
using Microsoft.SharePoint.Client ;
using Newtonsoft.Json ;
using NLog ;
using Logger = NLog.Logger ;

namespace ProjInterface
{
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly IIconsSource                          _iconsSource ;
        private readonly IEnumerable < IExplorerItemProvider > _providers ;
        private readonly Func < string , GraphServiceClient >  _graphFunc ;
        private readonly IPublicClientApplication              _publicClient ;

        private readonly ObservableCollection < IExplorerItem > _rootCollection =
            new ObservableCollection < IExplorerItem > ( ) ;


        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;
        private          IAccount                                 _account ;
        private          string                                   _defaultInputPath ;
        private          IntPtr                                   _hWnd ;
        private          IDictionary                              _iconsResources ;
        private          FrameworkElement                         _resourcesElement ;
        private GraphServiceClient _graphClient ;

        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IView1 > > > views
          , IIconsSource                             iconsSource
          , IEnumerable < IExplorerItemProvider >    providers
          , Func < Guid , IPublicClientApplication > publicClientFunc
          , Func < string , GraphServiceClient >     graphFunc
        )

        {
            Logger.Debug ( "Constructor" ) ;
            _views       = views ;
            _iconsSource = iconsSource ;
            _providers   = providers ;
            _graphFunc   = graphFunc ;
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

        public bool CanLogin { get { return Account == null ; } }

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

        public GraphServiceClient GraphClient
        {
            get { return _graphClient ; }
            set
            {
                if ( Equals ( value , _graphClient ) ) return ;
                _graphClient = value ;
                OnPropertyChanged ( ) ;
                Request1 ( ) ;
            }
        }

        public void Test1 ( )
        {
            ClientContext client = new ClientContext (
                                                      "https://satoridev.sharepoint.com/sites/Dev/SitePages/DevHome.aspx"
                                                     ) ;
            WebCollection w = client.Web.Webs ;


        }
        public async Task Request1 ( )

        {
            if ( _graphClient != null ) {
                var results = await _graphClient.Me.Contacts.Request ( ).GetAsync ( ) ;
                foreach ( var contact in results )
                {
                    Logger.Debug ( JsonConvert.SerializeObject ( contact ) ) ;
                    //Logger.Info ( "{contact}" , contact.DisplayName ) ;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged ;

        public async Task Login ( )
        {
            var scopes = new[] { "user.read.all" , "group.read.all", "contacts.read" } ;

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

            GraphClient = _graphFunc(result.AccessToken);
            Account = result.Account ;
        }

        public async Task < bool > LoginSilentAsync ( )
        {
            var app = _publicClient ;
            var accounts = await app.GetAccountsAsync ( ) ;
            var account = accounts.FirstOrDefault ( ) ;

            var scopes = new[] { "user.read.all" , "group.read.all" } ;

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

            GraphClient = _graphFunc ( result.AccessToken ) ;
            Account      = result.Account ;
            return true ;
        }

        public IntPtr GethWnd ( ) { return _hWnd ; }

        public void SethWnd ( IntPtr value ) { _hWnd = value ; }

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
