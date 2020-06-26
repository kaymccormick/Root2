﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
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
        private IDocumentHost docHost;
        private readonly IContentSelector _contentSelector;
        private RibbonModelGroup _group;
        private RibbonModelTab _tab;
        private RibbonModelGroup _focusGroup;
        private Dictionary<DependencyObject, RibbonModelItemTextBox> scopes = new Dictionary<DependencyObject, RibbonModelItemTextBox>();

        public NavigationTabProvider(IDocumentHost docHost, IContentSelector contentSelector, Func<RibbonModelTab> factory) : base(factory)
        {
            this.docHost = docHost;
            _contentSelector = contentSelector;
        }

        public override RibbonModelTab ProvideModelItem()
        {
            _tab = new RibbonModelTab {Header = "Navigation"};
            _group = new RibbonModelGroup(){Label="Windows"};
            _focusGroup = new RibbonModelGroup() {Label = "Focus"};
            // var focus1Text = new RibbonModelTwoLineText();
            // _focusGroup.Items.Add(focus1Text);
            _tab.ItemsCollection.Add(_group);
            _tab.ItemsCollection.Add(_focusGroup);
            EventManager.RegisterClassHandler(typeof(UIElement), FocusManager.GotFocusEvent, new RoutedEventHandler(Target));
            EventManager.RegisterClassHandler(typeof(UIElement), FocusManager.LostFocusEvent, new RoutedEventHandler(OnLostFocus));
            if (docHost.Documents is INotifyCollectionChanged cc)
            {
                foreach (var docHostDocument in docHost.Documents)
                {
                    Handle(docHostDocument);
                }

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
            if (!scopes.TryGetValue(scope, out var modelItem))
            {
                modelItem = new RibbonModelItemTextBox(){Value = x.ToString(), Label = scope.ToString()};
                _focusGroup.Items.Add(modelItem);
                scopes[scope] = modelItem;
            }
            else
            {
                modelItem.Value= x.ToString();
            }

            e.Handled = true;
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
                _contentSelector.SetActiveContent(eNewItem);
                return AppCommandResult.Success;
            }, eNewItem);
            b.Command = lambdaAppCommand.Command;
            IRibbonModelGroup ourGroup = _group;
            if (eNewItem is DocModel dm)
            {
                var h = dm.GroupHeader;
                
                foreach (var tabItem in _tab.Items)
                {
                    if(tabItem is RibbonModelGroup rmg)
                    {
                        
                        if (rmg.Label == dm.GroupHeader)
                        {
                            ourGroup = rmg;
                            break;
                        }
                    }
                }

                if (ourGroup == null)
                {
                    ourGroup = new RibbonModelGroup(){Label=h};
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

            
            ourGroup.Items.Add(b);
        }
    }
}