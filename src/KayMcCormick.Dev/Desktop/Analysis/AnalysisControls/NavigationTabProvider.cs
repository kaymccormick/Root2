using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
using RibbonLib.Model;

namespace AnalysisControls
{
    internal class NavigationTabProvider : RibbonModelTabProvider1
    {
        private readonly IDocumentHost _docHost;
        private readonly IContentSelector _contentSelector;
        private RibbonModelGroup _group;
        private RibbonModelTab _tab;
        private RibbonModelGroup _focusGroup;

        private readonly Dictionary<DependencyObject, RibbonModelControl> _scopes =
            new Dictionary<DependencyObject, RibbonModelControl>();

        public NavigationTabProvider(IDocumentHost docHost, IContentSelector contentSelector,
            Func<RibbonModelTab> factory) : base(factory)
        {
            _docHost = docHost;
            _contentSelector = contentSelector;
        }

        public override RibbonModelTab ProvideModelItem()
        {
            _tab = new RibbonModelTab {Header = "Navigation"};
            _group = new RibbonModelGroup() {Label = "Windows"};
            _focusGroup = new RibbonModelGroup() {Label = "Focus"};
            _tab.ItemsCollection.Add(_group);
            _tab.ItemsCollection.Add(_focusGroup);

            EventManager.RegisterClassHandler(typeof(UIElement), FocusManager.GotFocusEvent,
                new RoutedEventHandler(Target));
            EventManager.RegisterClassHandler(typeof(UIElement), FocusManager.LostFocusEvent,
                new RoutedEventHandler(OnLostFocus));
            if (_docHost.Documents is INotifyCollectionChanged cc)
            {
                foreach (var docHostDocument in _docHost.Documents) Handle(docHostDocument);

                cc.CollectionChanged += CcOnCollectionChanged;
            }

            return _tab;
            //var menuButton = new RibbonModelItemMenuButton(){Header=};
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            DebugUtils.WriteLine("Lost focus " + e.OriginalSource.ToString());
            e.Handled = true;
        }

        private void Target(object sender, RoutedEventArgs e)
        {
            DebugUtils.WriteLine($"Got Scope - {e.OriginalSource}");
            var x = (DependencyObject) e.OriginalSource;
            var scope = FocusManager.GetFocusScope(x);
            var modelItemValue = x.ToString(); //.ToString();
            var textBlock = new TextBlock {Text = modelItemValue};
            if (!_scopes.TryGetValue(scope, out var modelItem))
            {
                modelItem = new RibbonModelControl() {Content = textBlock, Label = scope.ToString()};
                _focusGroup.Items.Add(modelItem);
                _scopes[scope] = modelItem;
            }
            else
            {
                modelItem.Content = textBlock;
            }

            e.Handled = true;
        }

        private void CcOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (var eOldItem in e.OldItems)
                        _group.Items.Remove(_group.Items.Where(z => ((RibbonModelButton) z).ModelInstance == eOldItem)
                            .FirstOrDefault());

                    break;
                case NotifyCollectionChangedAction.Add:
                {
                    foreach (var eNewItem in e.NewItems) Handle(eNewItem);

                    break;
                }
            }
        }

        private void Handle(object eNewItem)
        {
            var label = "";
            var b = new RibbonModelButton() {ModelInstance = eNewItem};
            var lambdaAppCommand = new LambdaAppCommand("", async (command, o) =>
            {
                _contentSelector.SetActiveContent(eNewItem);
                return AppCommandResult.Success;
            }, eNewItem);
            b.Command = lambdaAppCommand.Command;
            IRibbonModelGroup ourGroup = _group;
            if (eNewItem is DocModel dm)
            {
                var h = dm.GroupHeader;

                foreach (var tabItem in _tab.Items)
                    if (tabItem is RibbonModelGroup rmg)
                        if (rmg.Label == dm.GroupHeader)
                        {
                            ourGroup = rmg;
                            break;
                        }

                if (ourGroup == null)
                {
                    ourGroup = new RibbonModelGroup() {Label = h};
                    _tab.ItemsCollection.Add(ourGroup);
                }

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
                        }
                        else
                        {
                            b.Background = null;
                            b.FontWeight = FontWeights.Normal;
                        }
                    }
                };
                label = dm.Title;
                BindingOperations.SetBinding(b, RibbonModelItem.LabelProperty, new Binding("Title") {Source = dm});
            }
            else
            {
                label = eNewItem.ToString();
            }


            ourGroup.Items.Add(b);
        }
    }
}
