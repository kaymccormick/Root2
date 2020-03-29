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
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.Threading ;
using System.Windows ;
using System.Windows.Controls ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    /// <summary>
    /// </summary>
    public sealed class AllResourcesTreeViewModel
    {
        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private readonly IObjectIdProvider _idProvider ;
        private readonly ILifetimeScope    _lifetimeScope ;

        private ResourceNodeInfo _appNode ;

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="idProvider"></param>
        public AllResourcesTreeViewModel (
            ILifetimeScope    lifetimeScope
          , IObjectIdProvider idProvider
        )
        {
            _lifetimeScope = lifetimeScope ;
            _idProvider    = idProvider ;
            PopulateResourcesTree ( ) ;
        }

        /// <summary>
        /// </summary>
        public bool IsEnabledPopulateObjects { get ; set ; } = true ;

        /// <summary>
        /// </summary>
        public ObservableCollection < ResourceNodeInfo > AllResourcesCollection
        {
            get { return _allResourcesCollection ; }
        }

        /// <summary>
        /// </summary>
        public ObservableCollection < ResourceNodeInfo > AllResourcesItemList { get ; set ; } =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private void PopulateResourcesTree ( )
        {
            if ( IsEnabledPopulateObjects )
            {
                PopulateObjects ( ) ;
            }

            PopulateLifetimeScope (
                                   _lifetimeScope
                                 , CreateNode ( null , "LifetimeScope" , _lifetimeScope , true )
                                  ) ;

            PopulateAppNode ( ) ;
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
                    var type = instanceInfo.Instance.GetType ( ) ;
                    if ( type.IsGenericType
                         && type.GetGenericTypeDefinition ( ) == typeof ( Meta <> ) )
                    {
                        var metadata = ( IDictionary < string , object > ) type
                                                                          .GetProperty (
                                                                                        "Metadata"
                                                                                       )
                                                                          .GetValue (
                                                                                     instanceInfo
                                                                                        .Instance
                                                                                    ) ;
                        var c4 = CreateNode ( n3 , "Metadata" , metadata , true ) ;
                        foreach ( var keyValuePair in metadata )
                        {
                            CreateNode ( c4 , keyValuePair.Key , keyValuePair.Value , false ) ;
                        }
                    }

                    CreateNode ( n3 , instanceInfo.Instance , instanceInfo.Instance , false ) ;
                    if ( instanceInfo.Instance is LifetimeScope ls )
                    {
                    }

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


        private void PopulateInstances (
            ResourceNodeInfo       res
          , IComponentRegistration registration
        )
        {
            var n = CreateNode ( res , "Instances" , null , false ) ;
            foreach ( var instanceInfo in
                _idProvider.GetInstanceByComponentRegistration ( registration ) )
            {
                var key = _idProvider.GetObjectInstanceIdentifier ( instanceInfo.Instance ) ;
                CreateNode (
                            n
                          , new CompositeResourceNodeKey ( key , instanceInfo.Instance )
                          , instanceInfo.Parameters
                          , true
                           ) ;
            }
        }

        [ NotNull ]
        private ResourceNodeInfo CreateNode (
            [ CanBeNull ] ResourceNodeInfo parent
          , object                         key
          , object                         data
          , bool ?                         isValueChildren = null
        )
        {
            var wrapped = WrapValue ( data ) ;
            var r = new ResourceNodeInfo
                    {
                        Key = key , Data = wrapped , IsValueChildren = isValueChildren
                    } ;
            if ( parent == null )
            {
                AllResourcesCollection.Add ( r ) ;
            }
            else
            {
                parent.Children.Add ( r ) ;
            }

            AllResourcesItemList.Add ( r ) ;
            return r ;
        }

        private void PopulateLifetimeScope ( ILifetimeScope lifetimeScope , ResourceNodeInfo node )
        {
            var regs = CreateNode (
                                   node
                                 , nameof ( lifetimeScope.ComponentRegistry.Registrations )
                                 , lifetimeScope.ComponentRegistry.Registrations
                                 , true
                                  ) ;

            foreach ( var reg in lifetimeScope.ComponentRegistry.Registrations )
            {
                var n2 = CreateNode ( regs , reg ,        reg ,          true ) ;
                var s = CreateNode ( n2 ,    "Services" , reg.Services , true ) ;
                foreach ( var regService in reg.Services )
                {
                    CreateNode ( s , regService.Description , regService , false ) ;
                }

                // PopulateInstances ( n2 , reg ) ;
            }

            if ( lifetimeScope is LifetimeScope ls )
            {
                var parentScope = ls.ParentLifetimeScope ;
                if ( parentScope != null )
                {
                    var parent = CreateNode ( node , "ParentLifetimeScope" , ls , true ) ;
                    PopulateLifetimeScope ( parentScope , parent ) ;
                }
            }
        }

        private void PopulateAppNode ( )
        {
            var current = Application.Current ;
            _appNode = CreateNode ( null , "Application" , current , false ) ;

            if ( current == null )
            {
                return ;
            }

            var appResources = CreateNode ( _appNode , "Resources" , current.Resources , true ) ;
            AddResourceNodeInfos ( appResources ) ;

            Debug.WriteLine ( Thread.CurrentThread.ManagedThreadId ) ;
            foreach ( Window currentWindow in current.Windows )
            {
                HandleWindow ( currentWindow ) ;
            }
        }

        private void AddResourceNodeInfos ( [ NotNull ] ResourceNodeInfo resNode )
        {
            if ( resNode == null )
            {
                throw new ArgumentNullException ( nameof ( resNode ) ) ;
            }

            var resNodeData = resNode.Data ;
            var res = ( ResourceDictionary ) resNodeData ;
            resNode.SourceUri = res?.Source ;

            if ( res == null )
            {
                return ;
            }

            foreach ( var md in res.MergedDictionaries )
            {
                var mdr = CreateNode ( resNode , md.Source , md , true ) ;
                AddResourceNodeInfos ( mdr ) ;
            }

            foreach ( DictionaryEntry haveResourcesResource in res )
            {
                if ( haveResourcesResource.Key      == null
                     || haveResourcesResource.Value == null )
                {
                    continue ;
                }

                var key = haveResourcesResource.Key ;
                if ( key is ResourceKey rkey )
                {
                    switch ( rkey )
                    {
                        case ComponentResourceKey componentResourceKey : break ;


                        case ItemContainerTemplateKey itemContainerTemplateKey : break ;

                        case DataTemplateKey dataTemplateKey : break ;

                        case TemplateKey templateKey : break ;
                        default :
                            key = new ResourceKeyWrapper < ResourceKey > ( rkey ) ;
                            break ;
                    }
                }

                var resourceInfo = CreateNode ( resNode , key , haveResourcesResource.Value ) ;
                if ( haveResourcesResource.Value is FrameworkTemplate ft )
                {
                    var resourcesNode = CreateNode ( resourceInfo , "Resources" , ft.Resources ) ;
                    AddResourceNodeInfos ( resourcesNode ) ;
                }

                if ( haveResourcesResource.Value is Style sty )
                {
                    var settersNode = CreateNode ( resourceInfo , "Setters" , null ) ;
                    settersNode.IsExpanded = true ;
                    foreach ( var setter in sty.Setters )
                    {
                        switch ( setter )
                        {
                            case EventSetter _ : break ;
                            case Setter setter1 :
                                CreateNode ( settersNode , setter1.Property , setter1.Value ) ;

                                break ;
                            default : throw new InvalidOperationException ( ) ;
                        }
                    }
                }

                if ( haveResourcesResource.Value is IDictionary dict )
                {
                    foreach ( var key2 in dict.Keys )
                    {
                        CreateNode ( resourceInfo , key2 , dict[ key2 ] ) ;
                    }
                }
                else
                {
                    if ( ! ( haveResourcesResource.Value is IEnumerable enumerable )
                         || haveResourcesResource.Value is string )
                    {
                        continue ;
                    }

                    foreach ( var child in enumerable )
                    {
                        CreateNode ( resourceInfo , child , null ) ;
                    }
                }
            }
        }

        [ CanBeNull ]
        private static object WrapValue ( object data )
        {
            var wrapped = data ;
            if ( data is UIElement uie )
            {
                wrapped = new ControlWrap < UIElement > ( uie ) ;
            }

            return wrapped ;
        }

        private void HandleWindow ( [ NotNull ] Window w )
        {
            var winNode = CreateNode (
                                      _appNode
                                    , w.GetType ( )
                                    , new ControlWrap < Window > ( w )
                                     ) ;
            winNode.TemplateKey = "WindowTemplate" ;


            var winRes = CreateNode ( winNode , "Resources" , w.Resources , true ) ;
//            AddResourceNodeInfos ( winRes ) ;
        }
    }
}