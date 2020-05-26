﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Interfaces;
using KayMcCormick.Lib.Wpf;
using KmDevWpfControls;

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

        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _treeView = (KmDevWpfControls.CustomTreeView) GetTemplateChild("TreeView");
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
            ((ContainerView) d).OnLifetimeScopeChanged((ILifetimeScope) e.OldValue, (ILifetimeScope) e.NewValue);
        }

        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(
            "Root", typeof(IEnumerable), typeof(ContainerView), new PropertyMetadata(default(IEnumerable), OnRootChanged));

        private IObjectIdProvider _objectIdprovider;
        private KmDevWpfControls.CustomTreeView _treeView;

        public IEnumerable Root
        {
            get { return (IEnumerable) GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        private static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ContainerView) d).OnRootChanged((IEnumerable) e.OldValue, (IEnumerable) e.NewValue);
        }



        protected virtual void OnRootChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }


        protected virtual void OnLifetimeScopeChanged(ILifetimeScope eOldValue, ILifetimeScope eNewValue)
        {
            if(eNewValue != null)
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
                    while (scopes.Any())
                    {
                        _list.Add(new LifetimeScopeNode()
                            {LifetimeScope = scopes.Pop(), IdProvider = _objectIdprovider});
                    }

                }
            }
        }
    }

    public abstract class BaseNode : ITreeViewNode, INotifyPropertyChanged, KmDevWpfControls.IAsyncExpand
    {
        protected BaseNode()
        {
            _items.Add(new object());
            Items = _items;
        }

        protected bool _isExpanded;
        private bool _isSelected;
        private LifetimeScope _lifetimeScope;
        protected ObservableCollection<object> _items = new ObservableCollection<object>();

        /// <inheritdoc />
        public abstract object Header { get; }

        /// <inheritdoc />
        public virtual bool IsExpanded
        {
            get { return _isExpanded; }
        }

        /// <inheritdoc />
        public virtual IEnumerable Items { get; }

        /// <inheritdoc />
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public LifetimeScope LifetimeScope
        {
            get { return _lifetimeScope; }
            set
            {
                if (Equals(value, _lifetimeScope)) return;
                _lifetimeScope = value;
                OnPropertyChanged();
            }
        }

        public IObjectIdProvider IdProvider { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual void Collapse()
        {
            _isExpanded = false;
            OnPropertyChanged(nameof(IsExpanded));
        }

        /// <inheritdoc />
        public abstract Task ExpandAsync();
    }
    public class LifetimeScopeNode : BaseNode
    {
        /// <inheritdoc />
        public override object Header => $"LifetimeScopoe {LifetimeScope.Tag}";

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            _items.Clear();
            foreach (var reg in LifetimeScope.ComponentRegistry.Registrations)
            {
                var node = new RegNode() {Registration = reg, LifetimeScope = LifetimeScope,IdProvider = IdProvider};
                _items.Add(node);
                
            }

            _isExpanded = true;
            OnPropertyChanged(nameof(IsExpanded));
            return Task.CompletedTask;
        }
    }

    public class RegNode : BaseNode
    {
        public IComponentRegistration Registration { get; set; }

        /// <inheritdoc />
        public override object Header => Registration.Activator.LimitType.FullName;

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            _items.Clear();
            var i = IdProvider.GetComponentInfo(Registration.Id);
            if (i != null)
                foreach (var instanceInfo in i.Instances)
                {
                    _items.Add(new InstanceNode()
                    {
                        LifetimeScope = LifetimeScope, IdProvider = IdProvider, Registration = Registration,
                        InstanceInfo = instanceInfo
                    });
                }

            _isExpanded = true;
            OnPropertyChanged(nameof(IsExpanded));
            return Task.CompletedTask;
        }
    }

    public class InstanceNode : BaseNode
    {
        /// <inheritdoc />
        public override object Header => InstanceInfo.Instance.GetType().FullName;

        public IComponentRegistration Registration { get; set; }
        public InstanceInfo InstanceInfo { get; set; }

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            return Task.CompletedTask;
        }
    }
}