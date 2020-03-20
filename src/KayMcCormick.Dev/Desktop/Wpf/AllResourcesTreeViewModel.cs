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
using System.Windows ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace KayMcCormick.Lib.Wpf
{
    public class AllResourcesTreeViewModel
    {
        private ObservableCollection<ResourceNodeInfo> _allResourcesCollection =
            new ObservableCollection<ResourceNodeInfo>();

        private ResourceNodeInfo _appNode;

        public AllResourcesTreeViewModel ( ) {
            PopulateResourcesTree();

        }
        private void PopulateResourcesTree()
        {
            try
            {
                var current = (BaseApp)Application.Current;
                _appNode = new ResourceNodeInfo { Key = "Application", Data = current };
                var appResources = new ResourceNodeInfo
                                   {
                                       Key  = "Resources",
                                       Data = current.Resources
                                   };
                _appNode.Children.Add(appResources);
                AddResourceNodeInfos(appResources);
                AllResourcesCollection.Add(_appNode);
                foreach ( Window currentWindow in current.Windows )
                {
                    HandleWindow(currentWindow);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw ;
            }
        }
        public ObservableCollection<ResourceNodeInfo> AllResourcesCollection
            => _allResourcesCollection;

        private static void AddResourceNodeInfos(ResourceNodeInfo resNode)
        {
            var res = (ResourceDictionary)resNode.Data;
            resNode.SourceUri = res.Source;

            foreach (var md in res.MergedDictionaries)
            {
                var mdr = new ResourceNodeInfo { Key = md.Source, Data = md };
                AddResourceNodeInfos(mdr);
                resNode.Children.Add(mdr);
            }

            foreach (DictionaryEntry haveResourcesResource in res)
            {
                if (haveResourcesResource.Key      != null
                    && haveResourcesResource.Value != null)
                {
                    ResourceNodeInfo resourceInfo;
                    var key = haveResourcesResource.Key;
                    if ( key is ResourceKey rkey )
                    {
                        switch (rkey)
                        {
                            case ComponentResourceKey componentResourceKey :
                                break;

                            case ItemContainerTemplateKey itemContainerTemplateKey : break ;
                            case DataTemplateKey dataTemplateKey : break ;
                            case TemplateKey templateKey : break ;
                            default :
                                key = new ResourceKeyWrapper < ResourceKey > ( rkey ) ;
                                break;
                        }

                        
                    }

                    resourceInfo = CreateResourceNodeInfo ( key , haveResourcesResource.Value ) ;
                    resNode.Children.Add(resourceInfo);
                    if ( haveResourcesResource.Value is Style sty)
                    {
                        var settersNode = CreateResourceNodeInfo ( "Setters" ) ;
                        settersNode.IsExpanded = true ;
                        foreach ( var setter in sty.Setters)
                        {
                            ResourceNodeInfo setterNode = null ;
                            switch ( setter )
                            {
                                case EventSetter eventSetter : break ;
                                case Setter setter1 :
                                    setterNode = CreateResourceNodeInfo ( setter1.Property , setter1.Value ) ;
                                    break ;
                                default : throw new ArgumentOutOfRangeException ( nameof ( setter ) ) ;
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
                            var childNode = CreateResourceNodeInfo ( key2 , dict[ key2] ) ;
                            resourceInfo.Children.Add(childNode);
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

        private static ResourceNodeInfo CreateResourceNodeInfo ( object child, object data= null )
        {
            if ( data == null )
            {
                data = child ;
            }

            object wrapped = WrapValue ( data ) ;
            return new ResourceNodeInfo { Key = child , Data = wrapped ?? data} ;
        }

        private static object WrapValue ( object data )
        {
            object wrapped = null ;
            if (data is UIElement uie)
            {
                wrapped = new ControlWrap < UIElement > ( uie ) ;
            }

            return wrapped ;
        }

        private void HandleWindow(Window w)
        {
            var winNode = new ResourceNodeInfo
                          {
                              Key         = w.GetType(),
                              Data        = new ControlWrap<Window>(w),
                              TemplateKey = "WindowTemplate"
                          };
            _appNode.Children.Add(winNode);
            var winRes = new ResourceNodeInfo { Key = "Resources", Data = w.Resources };
            winNode.Children.Add(winRes);
            AddResourceNodeInfos(winRes);
        }
    }
}