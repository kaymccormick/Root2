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
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginAuthenticationViewModel : IViewModel
    {
        private readonly Func < string , GraphServiceClient > _graphFunc ;
        private readonly IPublicClientApplication             _publicClient ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphFunc"></param>
        /// <param name="publicClient"></param>
        public LoginAuthenticationViewModel (
            Func < string , GraphServiceClient > graphFunc
          , IPublicClientApplication             publicClient
        )
        {
            _graphFunc    = graphFunc ;
            _publicClient = publicClient ;
        }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly IEnumerable < IExplorerItemProvider > _providers ;

        private readonly IEnumerable < Meta < Lazy < IViewWithTitle > > > _views ;
        private          IAccount                                         _account ;

        private string _defaultInputPath =
            Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ) ;

        private GraphServiceClient _graphClient ;

        private IntPtr      _hWnd ;
        private IDictionary _iconsResources ;

        private readonly ObservableCollection < AppExplorerItem > _rootCollection =
            new ObservableCollection < AppExplorerItem > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="views"></param>
        /// <param name="providers"></param>
        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IViewWithTitle > > > views
          , IEnumerable < IExplorerItemProvider >            providers
            
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


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < Meta < Lazy < IViewWithTitle > > > Views { get { return _views ; } }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < AppExplorerItem > RootCollection
        {
            get { return _rootCollection ; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SethWnd ( IntPtr value ) { _hWnd = value ; }

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}