using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using KayMcCormick.Dev;
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
    ///     <MyNamespace:AssemblyResourceTree/>
    ///
    /// </summary>
    public class AssemblyResourceTree : Control
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
            RootNodes = new ObservableCollection<NodeBase>();
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ExpandNode, OnExpandNodeExecuted,
                OnExpandNodeCanExecute));
        }
        
        private async void OnExpandNodeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine("Receieved expand node command with param " + e.Parameter ?? "");
            try
            {
                if (!(e.Parameter is CustomTreeViewItem cc))
                {
                    DebugUtils.WriteLine("PArameter is not CustomTreeViewItem");
                    return;
                }

                if (cc.IsExpanded)
                {
                    cc.Collapse();
                }
                else
                {
                    cc.Expand();
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                // ignored
            }
        }

        private void OnExpandNodeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (!(e.Parameter is DependencyObject dependencyObject))
                {
                    return;
                }

                var itemFromContainer = _treeView.ItemContainerGenerator.ItemFromContainer(dependencyObject);
                if (itemFromContainer is INodeData node)
                {
                    //DebugUtils.WriteLine("param is " + node);
                    if ((node.Items.Any() && node.ExpandedState != NodeExpandedState.Expanded))
                    {
                      //  DebugUtils.WriteLine("can execute");
                        e.CanExecute = true;
                    }
                    else if(node.ExpandedState == NodeExpandedState.Expanded)
                    {
                        e.CanExecute = true;
                    }
                }
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


    /// <summary>
    /// 
    /// </summary>
    public interface INodeData
    {
        /// <summary>
        /// 
        /// </summary>
        Assembly Assembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ObservableCollection<INodeData> Items { get; }

        /// <summary>
        /// 
        /// </summary>
        NodeExpandedState ExpandedState { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsExpanded { get; }

        /// <summary>
        /// 
        /// </summary>
        NodeDataLoadState DataState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Subnode CreateSubnode();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TempLoadData> CheckLoadItemsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        void LoadResult(TempLoadData result);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void SetIsExpanded(bool value);

        void Expand();
        /// <summary>
        /// 
        /// </summary>
        void Collapse();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class NodeBase : INotifyPropertyChanged, INodeData
    {
        public static Task<TaskScheduler> GetScheduler(Dispatcher d)
        {
            var schedulerResult = new TaskCompletionSource<TaskScheduler>();
            d.BeginInvoke(new Action(() =>
                schedulerResult.SetResult(
                    TaskScheduler.FromCurrentSynchronizationContext())));
            return schedulerResult.Task;
        }

        private NodeExpandedState _expandedState = NodeExpandedState.Indeterminate;
        private Task<TempLoadData> _loadTask;
        private Dispatcher _dispatcher;
        private TaskScheduler _taskScheduler;
        private readonly ObservableCollection<INodeData> _items = new ObservableCollection<INodeData>();
        private NodeDataLoadState _dataState;

        /// <summary>
        /// 
        /// </summary>
        protected NodeBase()
        {
            if (_items != null) _items.Add(new NodesPlaceHolder());
            Dispatcher = Dispatcher.CurrentDispatcher;
                _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResourceLocation ResourceLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly ReferencedAssembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ObservableCollection<INodeData> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual NodeExpandedState ExpandedState
        {
            get { return _expandedState; }
            protected set
            {
                if (value == _expandedState) return;
                _expandedState = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract bool CheckLoadItems(out NodeDataLoadState state);

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsExpanded
        {
            get { return ExpandedState == NodeExpandedState.Expanded; }
        }

        /// <inheritdoc />
        public virtual void SetIsExpanded(bool value)
        {
            ExpandedState = value ? NodeExpandedState.Expanded : NodeExpandedState.Collapsed;
        }

        /// <inheritdoc />
        public virtual async void Expand()
        {
            DebugUtils.WriteLine($"Items has any {Items.Any()} and ExpandedState is {ExpandedState}");
            if ((Items.Any() && ExpandedState != NodeExpandedState.Expanded))
            {
                DebugUtils.WriteLine("Data state is " + DataState);
                if (DataState != NodeDataLoadState.DataLoaded)
                {
                    DebugUtils.WriteLine("Attempting to load");
                    TempLoadData result = null;
                    try
                    {
                        result = await CheckLoadItemsAsync();
                        DebugUtils.WriteLine("return from async");
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }

                    if (result != null)
                    {
                        LoadResult(result);
                    }

                    if (!Items.Any())
                    {
                        return;
                    }

                    DebugUtils.WriteLine("expanded is " + ExpandedState);
                }

                SetIsExpanded(true);
            }
        }

        /// <inheritdoc />
        public virtual void Collapse()
        {
            if (ExpandedState != NodeExpandedState.Collapsed)
            {
                SetIsExpanded(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        protected static void SetExpandedAction(Task<TempLoadData> arg1, object arg2)
        {
            TaskState<NodeBase> state = (TaskState<NodeBase>) arg2;
            DebugUtils.WriteLine(nameof(SetExpandedAction));
            state.Node.Items.Clear();
            state.Node.ExpandedState = NodeExpandedState.Expanded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<TempLoadData> CheckLoadItemsAsync();

        /// <inheritdoc />
        public abstract void LoadResult(TempLoadData result);
        
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public abstract Subnode CreateSubnode();

        /// <summary>
        /// 
        /// </summary>
        public NodeDataLoadState DataState
        {
            get { return _dataState; }
            set
            {
                if (value == _dataState) return;
                _dataState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dispatcher Dispatcher
        {
            get { return _dispatcher; }
            set { _dispatcher = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NodeDataLoadState
    {
        /// <summary>
        /// 
        /// </summary>
        NoAction,
        /// <summary>
        /// 
        /// </summary>
        DataLoaded,
        /// <summary>
        /// 
        /// </summary>
        RequiresAsync,
        /// <summary>
        /// 
        /// </summary>
        LoadFailure
    }

    /// <summary>
    /// 
    /// </summary>
    public class RootNode : NodeBase
    {
        private Task<TempLoadData> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLoadItems(out NodeDataLoadState state)
        {
            state = NodeDataLoadState.RequiresAsync;
            return false;
        }

        /// <inheritdoc />
        public override Subnode CreateSubnode() => new Subnode();

        /// <inheritdoc />
        public override Task<TempLoadData> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running)
            {
                throw new InvalidOperationException();
            }
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResources,
                new TaskState<RootNode>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        private static TempLoadData LoadResources(object state1)
        {
            TaskState<RootNode> st = (TaskState<RootNode>) state1;
            var node = st.Node;
            TempLoadData res = new TempLoadData();
            try
            {
                List<SubnodeData> dta = new List<SubnodeData>();
                using (var stream = node.Assembly.GetManifestResourceStream(node.Name))
                {
                    if (stream != null)
                        using (var reader = new ResourceReader(stream))
                        {
                            foreach (var dictionaryEntry in reader.Cast<DictionaryEntry>())
                                try
                                {
                                    var data = new SubnodeData
                                    {
                                        Name = dictionaryEntry.Key as string,
                                        ResourceName = node.Name,
                                        Assembly = node.Assembly
                                    };
                                    dta.Add(data);
                                }
                                catch (Exception ex)
                                {
                                    DebugUtils.WriteLine(ex.ToString());
                                }
                        }
                }

                res.Value = dta;
                return res;
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }

            return res;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData result)
        {
            DebugUtils.WriteLine(result.ToString());
            Items.Clear();
            if (result.Value is IEnumerable x && !(result.Value is string))
            {
                foreach (var o in x)
                {
                    var subnodeData = (SubnodeData) o;
                    DebugUtils.WriteLine("Adding " + subnodeData.Name);
                    Items.Add(new Subnode()
                    {
                        Assembly = subnodeData.Assembly, Name = subnodeData.Name,
                        ResourceName = subnodeData.ResourceName
                    });
                }

                DebugUtils.WriteLine("Setting data loaded");
                DataState = NodeDataLoadState.DataLoaded;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubnodeData  

    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public string ResourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NodeExpandedState
    {
        /// <summary>
        /// 
        /// </summary>
        Indeterminate = 0,

        /// <summary>
        /// 
        /// </summary>
        Collapsed = 1,

        /// <summary>
        /// 
        /// </summary>
        Expanded = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class NodesPlaceHolder : INodeData
    {
        /// <inheritdoc />
        public Assembly Assembly { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public ObservableCollection<INodeData> Items { get; } = new ObservableCollection<INodeData>();

        /// <inheritdoc />
        public NodeExpandedState ExpandedState { get; set; }

        /// <inheritdoc />
        public bool IsExpanded
        {
            get { return ExpandedState == NodeExpandedState.Expanded; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetIsExpanded(bool value)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Expand()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Collapse()
        {
            
        }

        /// <inheritdoc />
        public NodeDataLoadState DataState { get; set; }

        /// <inheritdoc />
        public Subnode CreateSubnode()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<TempLoadData> CheckLoadItemsAsync()
        {
            return Task.FromResult(new TempLoadData());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void LoadResult(TempLoadData result)
        {
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Subnode : NodeBase
    {
        private Task<TempLoadData> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResourceName { get; set; }

        /// <inheritdoc />
        public override bool CheckLoadItems(out NodeDataLoadState state)
        {
            state = DataState;
            switch (DataState)
            {
                case NodeDataLoadState.NoAction:
                    return true;
                case NodeDataLoadState.DataLoaded:
                    return true;
                case NodeDataLoadState.RequiresAsync:
                    return false;
                case NodeDataLoadState.LoadFailure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();


            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public DataLoadStrategy LoadStrategy { get; set; }

        /// <inheritdoc />
        public override Subnode CreateSubnode()
        {
            return new Subnode();
        }

        /// <inheritdoc />
        public override Task<TempLoadData> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running)
            {
                throw new InvalidOperationException();
            }
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResourceData,
                new TaskState<Subnode>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData result)
        {
            DebugUtils.WriteLine(result.ToString());
        }

        private static TempLoadData LoadResourceData(object state)
        {
            var st = (TaskState<Subnode>) state;
            Subnode subnode = st.Node;
            var stream = subnode.Assembly.GetManifestResourceStream(subnode.ResourceName);
            if (stream != null)
                using (var reader = new ResourceReader(stream))
                {
                    reader.GetResourceData(subnode.Name, out var dataType, out var data);

                    // Display the data type.
                    // DebugUtils.WriteLine("   Data Type: {0}", dataType);
                    // Display the bytes that form the available data.      
                    // Console.Write("   Data: ");
                    var lines = 0;
                    foreach (var dataItem in data)
                        lines++;
                    // Console.Write("{0:X2} ", dataItem);
                    // if (lines % 25 == 0)
                    // Console.Write("\n         ");
                    // DebugUtils.WriteLine();
                    // Try to recreate current state of  data.
                    // Do: Bitmap, DateTimeTZI
                    switch (dataType)
                    {
                        // Handle internally serialized string data (ResourceTypeCode members).
                        case "ResourceTypeCode.String":
                            var binaryReader = new BinaryReader(new MemoryStream(data));
                            var binData = binaryReader.ReadString();
                            DebugUtils.WriteLine("   Recreated Value: {0}", binData);
                            return new TempLoadData() { Value = binData };
                        
                        case "ResourceTypeCode.Int32":
                            var int32 = BitConverter.ToInt32(data, 0);
                            return new TempLoadData() {Value = int32};
                        
                        case "ResourceTypeCode.Boolean":
                            DebugUtils.WriteLine($"   Recreated Value: {BitConverter.ToBoolean(data, 0)}");
                            break;
                        // .jpeg image stored as a stream.
                        case "ResourceTypeCode.Stream":
                            int offset = 4;

                            var size = BitConverter.ToInt32(data, 0);
                            var memoryStream = new MemoryStream(data, offset, size);
                            var load = new TempLoadData()
                            {
                                Length = size,
                                MemoryStream = memoryStream,
                                Data = data
                            };
                            return load;

                    }
                }

            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskState<T> where T : INodeData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subnodeData"></param>
        public TaskState(T subnodeData)
        {
            Node = subnodeData;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Node { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DataLoadStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        
        LoadAsync,
        
        LoadSync,
    }

    /// <summary>
    /// 
    /// </summary>
    public class TempLoadData {
        /// <summary>
        /// 
        /// </summary>
        public Subnode Node { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MemoryStream MemoryStream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Node)}: {Node}, {nameof(Data)}: {Data}, {nameof(Stream)}: {Stream}, {nameof(Length)}: {Length}, {nameof(MemoryStream)}: {MemoryStream}, {nameof(Value)}: {Value}";
        }
    }

     /// <summary>
     /// 
     /// </summary>
     public class CustomTreeViewItem : TreeViewItem {
         public static readonly RoutedEvent ExpandingEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));
         /// <summary>Identifies the <see cref="E:System.Windows.Controls.TreeViewItem.Collapsed" /> routed event. </summary>
         
        protected override void OnExpanded(RoutedEventArgs e)
         {
             base.OnExpanded(e);
         }

         protected override void OnCollapsed(RoutedEventArgs e)
         {
             base.OnCollapsed(e);
         }

         private static bool IsControlKeyDown
         {
             get
             {
                 return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
             }
         }

         /// <inheritdoc />
         protected override void OnKeyDown(KeyEventArgs e)
         {
             switch (e.Key)
             {
                 case Key.Left when IsControlKeyDown || !this.CanExpandOnInput || !this.IsExpanded:
                     return;
                 case Key.Left:
                 {
                     if (this.IsFocused)
                         Collapse();
                     else
                         this.Focus();
                     e.Handled = true;
                     return;
                 }
                 case Key.Right when IsControlKeyDown || !this.CanExpandOnInput:
                     break;
                 case Key.Right:
                     if (!this.IsExpanded)
                     {
                         Expand();
                     }

                     e.Handled = true;
                     break;
                 case Key.Add:
                     if (!this.CanExpandOnInput || this.IsExpanded)
                         break;
                     Expand();
                     e.Handled = true;
                     break;
                 case Key.Subtract:
                     if (!this.CanExpandOnInput || !this.IsExpanded)
                         break;
                     Collapse();
                     e.Handled = true;
                     break;
                default:
                    base.OnKeyDown(e);
                    return;
             }

         }

         protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
         {
             if (!e.Handled && this.IsEnabled)
             {
                 if (e.ClickCount % 2 == 0)
                 {
                     if (this.IsExpanded)
                     {
                         this.Collapse();
                     }
                     else
                     {
                         this.Expand();
                     }
                     e.Handled = true;
                 }
             }
             base.OnMouseLeftButtonDown(e);
         }
         /// <summary>
         /// 
         /// </summary>
         public void Expand()
        {

            var item = ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this);
            if (item is INodeData d)
            {
                d.Expand();
            }
            else
            {
                DebugUtils.WriteLine($"{item}");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanExpandOnInput => HasItems && IsEnabled;

        /// <summary>
        /// 
        /// </summary>
        public void Collapse()
        {

            var item = ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this);
            if (item is INodeData d)
            {
                d.Collapse();
            }
            else
            {
                DebugUtils.WriteLine($"{item}");
            }

        }

        ItemsControl ParentItemsControl
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer((DependencyObject)this);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// 
    public class CustomTreeView : TreeView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override bool ExpandSubtree(TreeViewItem container)
        {
            return base.ExpandSubtree(container);
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        protected override void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
        {
            base.OnItemContainerStyleChanged(oldItemContainerStyle, newItemContainerStyle);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
        {
            return base.ShouldApplyItemContainerStyle(container, item);
        }
    }

/// <summary>
/// 
/// </summary>
public class CustomToggleButton : ToggleButton
{
    /// <summary>
    /// 
    /// </summary>
    protected override void OnToggle()
    {
    }
}
}
