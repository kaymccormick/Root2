using System.Collections.Specialized;

using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using AnalysisControls.ViewModel;
using Autofac;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
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
            RibbonModelButton b = new RibbonModelButton() { ModelInstance = eNewItem };
            var lambdaAppCommand = new LambdaAppCommand("", async (command, o) =>
            {
                docHost.SetActiveDocument(eNewItem);
                return AppCommandResult.Success;
            }, eNewItem);
            b.Command = lambdaAppCommand.Command;
            if (eNewItem is DocModel dm) 
            {
                b.LargeImageSource = dm.LargeImageSource;
                dm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsActive")
                    {
                        if (dm.IsActive)
                        {
                            b.Background = Brushes.Gold;
                            //b.FontSize = 18.0;
                            b.FontWeight = FontWeights.Bold;
                        } else
                        {
                            b.Background = null;
                            b.FontWeight = FontWeights.Normal;
                        }
                    }
                };
                label = dm.Title;
                BindingOperations.SetBinding(b, RibbonModelItem.LabelProperty, new Binding("Title") {Source = dm});
            } else
            {
                label = eNewItem.ToString();
            }

            
            _group.Items.Add(b);
        }
    }
}