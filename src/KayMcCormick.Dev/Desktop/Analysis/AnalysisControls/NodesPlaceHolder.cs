using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class NodesPlaceHolder : INodeData
    {
        /// <inheritdoc />
        public Assembly Assembly { get; set; }

        /// <inheritdoc />
        public object Name { get; set; }

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

        public Task ExpandAsync()
        {
            throw new NotImplementedException();
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
}