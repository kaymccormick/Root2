using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NodeBase : INotifyPropertyChanged, INodeData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
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
            _items?.Add(new NodesPlaceHolder());
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
        public object Name { get; set; }

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
            DebugUtils.WriteLine("Setting expanded.");
            
            ExpandedState = value ? NodeExpandedState.Expanded : NodeExpandedState.Collapsed;
        }

        /// <inheritdoc />
        public virtual async Task ExpandAsync()
        {
            DebugUtils.WriteLine($"Items has any {Items.Any()} and ExpandedState is {ExpandedState}");
            if (Items.Any() && ExpandedState != NodeExpandedState.Expanded)
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

                    if (result != null) LoadResult(result);

                    if (!Items.Any()) return;

                    DebugUtils.WriteLine("expanded is " + ExpandedState);
                }

                SetIsExpanded(true);
            }
        }

        /// <inheritdoc />
        public virtual void Collapse()
        {
            if (ExpandedState != NodeExpandedState.Collapsed) SetIsExpanded(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        protected static void SetExpandedAction(Task<TempLoadData> arg1, object arg2)
        {
            var state = (TaskState<NodeBase>) arg2;
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

        /// <summary>
        /// 
        /// </summary>
        public abstract DataLoadStrategy LoadStrategy { get; set; }
    }
}