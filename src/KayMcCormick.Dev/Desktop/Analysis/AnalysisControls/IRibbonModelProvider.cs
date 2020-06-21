using System.Collections.Specialized;
using System.Linq;
using AnalysisControls.ViewModel;
using Autofac;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRibbonModelProvider<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T ProvideModelItem();
    }

    class NavigationTabProvider : IRibbonModelProvider<RibbonModelTab>
    {
        private IDocumentHost docHost;
        private RibbonModelGroup _group;

        public NavigationTabProvider(IDocumentHost docHost)
        {
            this.docHost = docHost;
        }

        public RibbonModelTab ProvideModelItem()
        {
            var tab = new RibbonModelTab();
            tab.Header = "Navigation";
            _group = new RibbonModelGroup(){Header="Windows"};
            tab.ItemsCollection.Add(_group);
            if (docHost.Documents is INotifyCollectionChanged cc)
            {
                foreach (var docHostDocument in docHost.Documents)
                {
                    Handle(docHostDocument);
                }

                cc.CollectionChanged += CcOnCollectionChanged;
            }
            return tab;
            //var menuButton = new RibbonModelItemMenuButton(){Header=};
        }

        private void CcOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var eOldItem in e.OldItems)
                    {
                        _group.Items.Remove(_group.Items.Where(z => ((RibbonModelButton) z).ModelInstance == eOldItem)
                            .FirstOrDefault());
                    }

                    break;
                case NotifyCollectionChangedAction.Add:
                {
                    foreach (var eNewItem in e.NewItems)
                    {
                        Handle(eNewItem);
                    }

                    break;
                }
            }
        }

        private void Handle(object eNewItem)
        {
            var label = "";
            if (eNewItem is DocModel dm)
            {
                label = dm.Title;
            }

            if (string.IsNullOrEmpty(label))
                label = eNewItem.ToString();

            RibbonModelButton b = new RibbonModelButton() {Label = label, ModelInstance = eNewItem};
            _group.Items.Add(b);
        }
    }
}