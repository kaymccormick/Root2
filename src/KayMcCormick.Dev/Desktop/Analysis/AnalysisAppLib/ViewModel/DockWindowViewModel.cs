using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Threading.Tasks ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;
using Newtonsoft.Json ;
using NLog ;
using Logger = NLog.Logger ;

namespace AnalysisAppLib.ViewModel
{
    public sealed class LoginAuthenticationViewModel : IViewModel
    {
        private readonly Func<string, GraphServiceClient> _graphFunc;
        private readonly IPublicClientApplication         _publicClient;

        public LoginAuthenticationViewModel ( Func < string , GraphServiceClient > graphFunc , IPublicClientApplication publicClient )
        {
            _graphFunc = graphFunc ;
            _publicClient = publicClient ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly IEnumerable < IExplorerItemProvider > _providers ;
        
        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;
        private          IAccount                                 _account ;

        private string _defaultInputPath =
            Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ) ;

        private IntPtr             _hWnd ;
        private IDictionary        _iconsResources ;
        private GraphServiceClient _graphClient ;

        private ObservableCollection < AppExplorerItem > _rootCollection =
            new ObservableCollection < AppExplorerItem > ( ) ;

        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IView1 > > > views
          , IEnumerable < IExplorerItemProvider >    providers
        )
        {
            _views     = views ;
            _providers = providers ;

            foreach ( var explorerItemProvider in _providers )
            {
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

        public ObservableCollection < AppExplorerItem > RootCollection
        {
            get { return _rootCollection ; }
        }


        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
        }


        public async Task Request1 ( )

        {
            if ( _graphClient != null )
            {
                var results = await _graphClient.Me.Contacts.Request ( ).GetAsync ( ) ;
                foreach ( var contact in results )
                {
                    Logger.Debug ( JsonConvert.SerializeObject ( contact ) ) ;
                    //Logger.Info ( "{contact}" , contact.DisplayName ) ;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        public void SethWnd ( IntPtr value ) { _hWnd = value ; }

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}