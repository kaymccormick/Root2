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
    public sealed class AllResourcesTreeViewModel
    {
        private readonly ModelResources _modelResources ;

        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo _appNode ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelResources"></param>
        public AllResourcesTreeViewModel ( ModelResources modelResources )
        {
            _modelResources = modelResources ;
        }
#if false
        private void PopulateInstances (
            ResourceNodeInfo       res
          , IComponentRegistration registration
        )
        {
            var n = CreateNode ( res , "Instances" , null , false ) ;
            foreach ( var instanceInfo in
                _m.GetInstanceByComponentRegistration ( registration ) )
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
#endif
        private ResourceNodeInfo CreateNode(
            [CanBeNull] ResourceNodeInfo parent
          , object                       key
          , object                       data
          , bool?                        isValueChildren = null)
        {
            return _modelResources.CreateNode ( parent , key , data , isValueChildren ) ;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resNode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        protected void AddResourceNodeInfos ( [ NotNull ] ResourceNodeInfo resNode )
        {
            if ( resNode == null )
            {
                throw new ArgumentNullException ( nameof ( resNode ) ) ;
            }

            var resNodeData = resNode.Data ;
            var res = ( ResourceDictionary ) resNodeData ;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected object WrapValue ( object data )
        {
            if (data is UIElement uie)
            {
                return new ControlWrap<UIElement>(uie);
            }

            return data ;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void PopulateResourcesTree ( )
        {
            PopulateAppNode();
        }
#endregion
    }
}