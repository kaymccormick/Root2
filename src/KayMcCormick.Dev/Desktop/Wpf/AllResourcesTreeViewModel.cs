#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// AllResourcesTreeViewModel.cs
// 
// 2020-03-16-7:25 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.Threading ;
using System.Windows ;
using System.Windows.Controls ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Lib.Wpf
{
    public class AllResourcesTreeViewModel
    {
        private readonly ILifetimeScope _lifetimeScope ;
        private readonly IObjectIdProvider _idProvider ;

        private ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo                          _appNode ;
        private ObservableCollection < ResourceNodeInfo > _allResourcesItemList = new ObservableCollection < ResourceNodeInfo > ();

        public AllResourcesTreeViewModel ( ILifetimeScope lifetimeScope , IObjectIdProvider idProvider)
        {
            _lifetimeScope = lifetimeScope ;
            _idProvider = idProvider ;
            PopulateResourcesTree ( ) ;
        }

        private void PopulateResourcesTree ( )
        {
            try
            {
                // PopulateObjects ( ) ;
                PopulateLifetimeScope ( ) ;

                PopulateAppNode ( ) ;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch ( Exception ex )
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw ;
            }
        }

        private void PopulateObjects ( )
        {
            var n1 = CreateNode ( null , "Objects" , null , false ) ;
            foreach ( var rootNode in _idProvider.GetRootNodes ( ) )
            {
                var c = _idProvider.GetComponentInfo ( rootNode ) ;
                var n2 = CreateNode ( n1 , rootNode , c , true ) ;
                foreach ( var instanceInfo in c.Instances )
                {
                    var n3 = CreateNode ( n2 , instanceInfo , instanceInfo , true ) ;
                    CreateNode ( n3 , instanceInfo.Instance , instanceInfo.Instance , false ) ;
                    if ( instanceInfo.Instance is FrameworkElement fe )
                    {
                        var res = CreateNode ( n3 , "Resources" , fe.Resources , true ) ;
                        AddResourceNodeInfos ( res ) ;
                    }

                    var n4 = CreateNode ( n3 , "Parameters" , instanceInfo.Parameters , true ) ;
                    foreach ( var p in instanceInfo.Parameters )
                    {
                        CreateNode ( n4 , p , p , false ) ;
                    }
                }
            }
        }

        private void PopulateInstances (ResourceNodeInfo res, IComponentRegistration registration )
        {
            var n = CreateNode ( res , "Instances" , null , false ) ;
            foreach ( var instanceInfo in
                _idProvider.GetInstanceByComponentRegistration ( registration ) )
            {
                var key = _idProvider.GetObjectInstanceIdentifier ( instanceInfo.Instance ) ;
                var n2 = CreateNode ( n , new CompositeResourceNodeKey(key, instanceInfo.Instance), instanceInfo.Parameters , true ) ;

            }
        }

        private ResourceNodeInfo CreateNode (
            ResourceNodeInfo parent
          , object           key
          , object           data
          , bool             isValueChildren
        )
        {
            var r = new ResourceNodeInfo { Key = key , Data = data } ;
            if ( parent == null )
            {
                AllResourcesCollection.Add ( r ) ;
            }
            else
            {
                parent.Children.Add ( r ) ;
            }

            _allResourcesItemList.Add ( r ) ;
            return r ;
        }

        private void PopulateLifetimeScope ( )
        {
            var n = CreateNode ( null , "LifetimeScope" , _lifetimeScope , true ) ;
            var regs = CreateNode (
                                   n
                                 , nameof ( _lifetimeScope.ComponentRegistry.Registrations )
                                 , _lifetimeScope.ComponentRegistry.Registrations
                                 , true
                                  ) ;

            foreach ( var reg in _lifetimeScope.ComponentRegistry.Registrations )
            {
                var n2 = CreateNode ( regs , reg ,        reg ,          true ) ;
                var s = CreateNode ( n2 ,    "Services" , reg.Services , true ) ;
                foreach ( var regService in reg.Services )
                {
                    var s1 = CreateNode ( s , regService.Description , regService , false ) ;
                }

                // PopulateInstances ( n2 , reg ) ;
            }
        }

        private bool PopulateAppNode ( )
        {
            var current = Application.Current ;
            _appNode = CreateNode ( null , "Application" , current , false ) ;
            
            if ( current == null )
            {
                return true ;
            }

            var appResources =
                CreateNode (  _appNode , "Resources" , current.Resources , true  ) ;
            AddResourceNodeInfos ( appResources ) ;

            Debug.WriteLine ( Thread.CurrentThread.ManagedThreadId ) ;
            foreach ( Window currentWindow in current.Windows )
            {
                HandleWindow ( currentWindow ) ;
            }

            return false ;
        }

        public ObservableCollection < ResourceNodeInfo > AllResourcesCollection
        {
            get { return _allResourcesCollection ; }
        }

        public ObservableCollection < ResourceNodeInfo > AllResourcesItemList
        {
            get { return _allResourcesItemList ; }
            set { _allResourcesItemList = value ; }
        }

        private static void AddResourceNodeInfos ( ResourceNodeInfo resNode )
        {
            var res = ( ResourceDictionary ) resNode.Data ;
            resNode.SourceUri = res.Source ;

            foreach ( var md in res.MergedDictionaries )
            {
                var mdr = new ResourceNodeInfo { Key = md.Source , Data = md } ;
                AddResourceNodeInfos ( mdr ) ;
                resNode.Children.Add ( mdr ) ;
            }

            foreach ( DictionaryEntry haveResourcesResource in res )
            {
                if ( haveResourcesResource.Key      != null
                     && haveResourcesResource.Value != null )
                {
                    ResourceNodeInfo resourceInfo ;
                    var key = haveResourcesResource.Key ;
                    if ( key is ResourceKey rkey )
                    {
                        switch ( rkey )
                        {
                            case ComponentResourceKey componentResourceKey : break ;

                            case ItemContainerTemplateKey itemContainerTemplateKey : break ;
                            case DataTemplateKey dataTemplateKey :                   break ;
                            case TemplateKey templateKey :                           break ;
                            default :
                                key = new ResourceKeyWrapper < ResourceKey > ( rkey ) ;
                                break ;
                        }
                    }

                    resourceInfo = CreateResourceNodeInfo ( key , haveResourcesResource.Value ) ;
                    resNode.Children.Add ( resourceInfo ) ;
                    if ( haveResourcesResource.Value is FrameworkTemplate ft )
                    {
                        var resourcesNode = CreateResourceNodeInfo ( "Resources" , ft.Resources ) ;
                        resourceInfo.Children.Add ( resourcesNode ) ;
                        AddResourceNodeInfos ( resourcesNode ) ;
                    }

                    if ( haveResourcesResource.Value is Style sty )
                    {
                        var settersNode = CreateResourceNodeInfo ( "Setters" ) ;
                        settersNode.IsExpanded = true ;
                        foreach ( var setter in sty.Setters )
                        {
                            ResourceNodeInfo setterNode = null ;
                            switch ( setter )
                            {
                                case EventSetter eventSetter : break ;
                                case Setter setter1 :
                                    setterNode =
                                        CreateResourceNodeInfo (
                                                                setter1.Property
                                                              , setter1.Value
                                                               ) ;
                                    break ;
                                default :
                                    throw new ArgumentOutOfRangeException ( nameof ( setter ) ) ;
                            }

                            if ( setterNode != null )
                            {
                                settersNode.Children.Add ( setterNode ) ;
                            }
                        }

                        resourceInfo.Children.Add ( settersNode ) ;
                    }

                    if ( haveResourcesResource.Value is IDictionary dict )
                    {
                        foreach ( var key2 in dict.Keys )
                        {
                            var childNode = CreateResourceNodeInfo ( key2 , dict[ key2 ] ) ;
                            resourceInfo.Children.Add ( childNode ) ;
                        }
                    }
                    else
                    {
                        if ( haveResourcesResource.Value is IEnumerable enumerable
                             && haveResourcesResource.Value.GetType ( ) != typeof ( string ) )
                        {
                            foreach ( var child in enumerable )
                            {
                                var childNode = CreateResourceNodeInfo ( child , null ) ;
                                resourceInfo.Children.Add ( childNode ) ;
                            }
                        }
                    }
                }
            }
        }

        private static ResourceNodeInfo CreateResourceNodeInfo ( object child , object data = null )
        {
            if ( data == null )
            {
                data = child ;
            }

            var wrapped = WrapValue ( data ) ;
            return new ResourceNodeInfo { Key = child , Data = wrapped ?? data } ;
        }

        private static object WrapValue ( object data )
        {
            object wrapped = null ;
            if ( data is UIElement uie )
            {
                wrapped = new ControlWrap < UIElement > ( uie ) ;
            }

            return wrapped ;
        }

        private void HandleWindow ( Window w )
        {
            var winNode = new ResourceNodeInfo
                          {
                              Key         = w.GetType ( )
                            , Data        = new ControlWrap < Window > ( w )
                            , TemplateKey = "WindowTemplate"
                          } ;
            _appNode.Children.Add ( winNode ) ;
            var winRes = new ResourceNodeInfo { Key = "Resources" , Data = w.Resources } ;
            winNode.Children.Add ( winRes ) ;
            AddResourceNodeInfos ( winRes ) ;
        }
    }
}