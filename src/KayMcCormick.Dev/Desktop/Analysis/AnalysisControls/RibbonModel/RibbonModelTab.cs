using System.Collections.ObjectModel;
using System.Windows;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTab
    {
        /// <summary>
        /// 
        /// </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
        /// <summary>
        /// 
        /// </summary>
        public object ContextualTabGroupHeader { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public RibbonModelGroup CreateGroup(string @group)
        {
            var r = new RibbonModelGroup() {Header = @group};
            Items.Add(r);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"RibbonModelTab[{Header}]";
        }
    }

}