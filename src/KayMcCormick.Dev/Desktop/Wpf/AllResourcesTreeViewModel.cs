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
using System.Windows ;

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

        private void AddResourceNodeInfos(ResourceNodeInfo appResources)
        {
            var res = (ResourceDictionary)appResources.Data;
            appResources.SourceUri = res.Source;

            foreach (var md in res.MergedDictionaries)
            {
                var mdr = new ResourceNodeInfo { Key = md.Source, Data = md };
                AddResourceNodeInfos(mdr);
                appResources.Children.Add(mdr);
            }

            foreach (DictionaryEntry haveResourcesResource in res)
            {
                if (haveResourcesResource.Key      != null
                    && haveResourcesResource.Value != null)
                {
                    var resourceInfo = new ResourceNodeInfo
                                       {
                                           Key = haveResourcesResource.Key
                                          ,
                                           Data = haveResourcesResource.Value
                                       };
                    appResources.Children.Add(resourceInfo);
                }
            }
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