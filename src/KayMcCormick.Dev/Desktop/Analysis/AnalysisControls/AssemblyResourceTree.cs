using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Lib.Wpf;

// ReSharper disable RedundantOverriddenMember

namespace AnalysisControls
{
   
    /// <summary>
    /// 
    /// </summary>
    [DefaultProperty("Assembly")]
    [TitleMetadata("Assembly Resource Tree")]
    public class AssemblyResourceTree : Control, IAppCustomControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AssemblyProperty = DependencyProperty.Register(
            "Assembly", typeof(Assembly), typeof(AssemblyResourceTree),
            new PropertyMetadata(default(Assembly), _OnAssemblyUpdated));

        private static void _OnAssemblyUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AssemblyResourceTree) d).OnAssemblyUpdated((Assembly) e.OldValue,
                (Assembly) e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RootNodesProperty = DependencyProperty.Register(
            "RootNodes", typeof(ObservableCollection<NodeBase>), typeof(AssemblyResourceTree),
            new PropertyMetadata(default(ObservableCollection<NodeBase>)));

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NodeBase> RootNodes
        {
            get { return (ObservableCollection<NodeBase>) GetValue(RootNodesProperty); }
            set { SetValue(RootNodesProperty, value); }
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnAssemblyUpdated(Assembly old, Assembly newVal)
        {
            PopulateResourceTree(newVal);
        }

        private void PopulateResourceTree(Assembly assembly)
        {
            RootNodes.Clear();
            var resourceNames = assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                var info = assembly.GetManifestResourceInfo(resourceName);
                if (info != null)
                {
                    var data = new RootNode
                    {
                        Assembly = assembly,
                        FileName = info.FileName,
                        ResourceLocation = info.ResourceLocation,
                        ReferencedAssembly = info.ReferencedAssembly,
                        Name = resourceName
                    };
                    RootNodes.Add(data);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ObservableCollection<NodeBase> RootItems { get; } = new ObservableCollection<NodeBase>();

        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly
        {
            get { return (Assembly) GetValue(AssemblyProperty); }
            set { SetValue(AssemblyProperty, value); }
        }

        static AssemblyResourceTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssemblyResourceTree),
                new FrameworkPropertyMetadata(typeof(AssemblyResourceTree)));
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly RoutedEvent SelectedItemChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<object>), typeof(AssemblyResourceTree));

        private TreeView _treeView;

        /// <summary>
        /// 
        /// </summary>
        [Category("Behavior")]
        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
        {
            add { AddHandler(SelectedItemChangedEvent, value); }
            remove { RemoveHandler(SelectedItemChangedEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyResourceTree()
        {
            AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(OnExpanded) );
            RootNodes = new ObservableCollection<NodeBase>();
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ToggleNodeIsExpanded, OnExpandNodeExecutedAsync,
                OnToggleNodeCanExecute));
        }

        private void OnExpanded(object sender, RoutedEventArgs e)
        {
            //ClearValue(CursorProperty);
        }

        private async void OnExpandNodeExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine("Received expand node command with param " + e.Parameter);
            try
            {
                if (!(e.Parameter is CustomTreeViewItem cc))
                {
                    DebugUtils.WriteLine("PArameter is not CustomTreeViewItem");
                    return;
                }

                if (cc.IsExpanded)
                    cc.Collapse();
                else
                {
                    SetCurrentValue(Control.CursorProperty, Cursors.Wait);
                    Dispatcher.BeginInvoke(new Action(() => ClearValue(CursorProperty)), DispatcherPriority.ContextIdle, null);
                    await cc.ExpandAsync();
                    DebugUtils.WriteLine("return from expand async");
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                // ignored
            }
        }

        private void OnToggleNodeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (!(e.Parameter is DependencyObject dependencyObject)) return;

                var itemFromContainer = _treeView.ItemContainerGenerator.ItemFromContainer(dependencyObject);
                if (!(itemFromContainer is INodeData node)) return;
                //DebugUtils.WriteLine("param is " + node);
                if (node.Items.Any() && node.ExpandedState != NodeExpandedState.Expanded)
                    //  DebugUtils.WriteLine("can execute");
                    e.CanExecute = true;
                else if (node.ExpandedState == NodeExpandedState.Expanded) e.CanExecute = true;
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                // ignored
            }
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _treeView = (TreeView) GetTemplateChild("TreeView");
            if (_treeView != null) _treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue, SelectedItemChangedEvent));
            e.Handled = true;
        }
    }
}