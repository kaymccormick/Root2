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
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    /// <summary>
    /// </summary>
    public sealed class AllResourcesTreeViewModel : ModelResources
    {
        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo _appNode ;

        public AllResourcesTreeViewModel ( ILifetimeScope lifetimeScope , IObjectIdProvider idProvider ) : base ( lifetimeScope , idProvider )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="idProvider"></param>


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

        protected void AddResourceNodeInfos ( [ NotNull ] ResourceNodeInfo resNode )
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

        #region Overrides of ModelResources
        protected override object WrapValue ( object data )
        {
            if (data is UIElement uie)
            {
                return new ControlWrap<UIElement>(uie);
            }

            return base.WrapValue ( data ) ;

        }

        protected override void PopulateResourcesTree ( )
        {
            base.PopulateResourcesTree ( ) ;
            PopulateAppNode();
        }
        #endregion
    }
}