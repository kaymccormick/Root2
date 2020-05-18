using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelQuickAccessToolbar
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; set; }= new ObservableCollection<object>();
    }
}