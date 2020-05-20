using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace KmDevWpfControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyResourceNode : ITreeViewNode
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
        ObservableCollection<IAssemblyResourceNode> Items { get; }

        /// <summary>
        /// 
        /// </summary>
        NodeExpandedState1 ExpandedState { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsExpanded { get; }

        /// <summary>
        /// 
        /// </summary>
        NodeDataLoadState1 DataState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Subnode1 CreateSubnode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TempLoadData1> CheckLoadItemsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        void LoadResult(TempLoadData1 result);

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