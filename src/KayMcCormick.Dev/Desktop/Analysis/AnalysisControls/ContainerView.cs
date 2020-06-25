using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Interfaces;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ContainerView/>
    ///
    /// </summary>
    public class ContainerView : Control
    {
        /// <inheritdoc />
        public ContainerView()

        {
            CommandBindings.Add(new CommandBinding(KmDevWpfControls.CustomTreeView.ToggleNodeIsExpanded,
                OnExpandNodeExecutedAsync));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ResolveComponent, Exec));

        }

        private void Exec(object sender, ExecutedRoutedEventArgs e)
        {
            IComponentRegistration r = (IComponentRegistration)e.Parameter;
            DebugUtils.WriteLine($"{r}");
            var comp = Root.Cast<LifetimeScopeNode>().First().LifetimeScope.Resolve(((TypedService)r.Services.First()).ServiceType);
            if (e.Source is Border bd)
            {
                bd.Child = new ContentPresenter() { Content = comp };
            }


        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _treeView = (KmDevWpfControls.CustomTreeView)GetTemplateChild("TreeView");
        }

        private async void OnExpandNodeExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("Received expand node command with param " + e.Parameter);
            try
            {
                if (!(e.Parameter is KmDevWpfControls.CustomTreeViewItem cc))
                {
                    Debug.WriteLine("PArameter is not CustomTreeViewItem");
                    return;
                }

                if (cc.IsExpanded)
                    cc.Collapse();
                else
                {
                    SetCurrentValue(Control.CursorProperty, Cursors.Wait);
                    Dispatcher.BeginInvoke(new Action(() => ClearValue(CursorProperty)), DispatcherPriority.ContextIdle, null);
                    await cc.ExpandAsync();
                    Debug.WriteLine("return from expand async");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // ignored
            }
        }

        static ContainerView()
        {
            AttachedProperties.LifetimeScopeProperty.OverrideMetadata(typeof(ContainerView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, LifetimeScopeChanged));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContainerView), new FrameworkPropertyMetadata(typeof(ContainerView)));
        }

        private static void LifetimeScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ContainerView)d).OnLifetimeScopeChanged((ILifetimeScope)e.OldValue, (ILifetimeScope)e.NewValue);
        }

        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(
            "Root", typeof(IEnumerable), typeof(ContainerView), new PropertyMetadata(default(IEnumerable), OnRootChanged));

        private IObjectIdProvider _objectIdprovider;
        private KmDevWpfControls.CustomTreeView _treeView;

        public IEnumerable Root
        {
            get { return (IEnumerable)GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        private static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ContainerView)d).OnRootChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }



        protected virtual void OnRootChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }


        protected virtual void OnLifetimeScopeChanged(ILifetimeScope eOldValue, ILifetimeScope eNewValue)
        {
            if (eNewValue != null)
            {
                if (eNewValue is LifetimeScope s)
                {
                    Stack<LifetimeScope> scopes = new Stack<LifetimeScope>();
                    var s1 = s;
                    while (s1 != null)
                    {
                        scopes.Push(s1);
                        s1 = s1.ParentLifetimeScope as LifetimeScope;
                    }

                    var _list = new ObservableCollection<object>();
                    Root = _list;

                    _objectIdprovider = eNewValue.Resolve<IObjectIdProvider>();

                    var r = scopes.Peek().ComponentRegistry.Registrations;
                    var keys = r.SelectMany(z => z.Metadata.Keys).Distinct();
                    foreach (var k in keys)
                    {
                        foreach (var c in r.Where(r1 => r1.Metadata.ContainsKey(k)))
                        {
                            DebugUtils.WriteLine($"{c} - {k} - {c.Metadata[k]}]");

                        }
                    }

                    while (scopes.Any())
                    {
                        _list.Add(new LifetimeScopeNode()
                        { LifetimeScope = scopes.Pop(), IdProvider = _objectIdprovider });
                    }
                    Dictionary<string,object> d = new Dictionary<string, object>();
                    foreach (var reg in ((LifetimeScopeNode)_list[0]).LifetimeScope.ComponentRegistry.Registrations)
                    {
                        foreach (var metadataKey in reg.Metadata.Keys)
                        {
                            if (!d.ContainsKey(metadataKey))
                            {
                                d[metadataKey] = new object();
                            }
                        }
                    }
                    foreach (var keyValuePair in d)
                    {
                        DebugUtils.WriteLine(keyValuePair.Key);
                    }

                }
            }
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        /// <inheritdoc />
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        /// <inheritdoc />
        public override void BeginInit()
        {
            base.BeginInit();
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            base.EndInit();
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        /// <inheritdoc />
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }

        /// <inheritdoc />
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
    }
}
