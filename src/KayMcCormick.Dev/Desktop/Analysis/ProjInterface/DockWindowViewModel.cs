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
using System.Runtime.Serialization ;
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
        )

        {

            Logger.Debug ( "Constructor" ) ;
            _views       = views ;
            _iconsSource = iconsSource ;
            _providers   = providers ;

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
            }
        }


        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
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

        public IntPtr GethWnd ( ) { return _hWnd ; }

        public void SethWnd ( IntPtr value ) { _hWnd = value ; }

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context )
        {
        }
        #endregion
    }

    public interface ITakesHwnd
    {
        void SetHwnd ( IntPtr hWnd ) ;
    }
}
