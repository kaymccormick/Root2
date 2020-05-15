using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace AnalysisControls
{
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
        object Name { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        Task ExpandAsync();

        /// <summary>
        /// 
        /// </summary>
        void Collapse();
    }
}