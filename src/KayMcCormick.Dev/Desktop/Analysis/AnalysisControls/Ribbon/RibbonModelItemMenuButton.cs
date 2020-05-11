using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemMenuButton : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelMenuItem CreateMenuItem(string label)
        {
            var r = new RibbonModelMenuItem() { Header = label};
            Items.Add(r);
            return r;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelMenuItem: RibbonModelItem
    {
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        public object Header
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        public RibbonModelMenuItem CreateMenuItem(string label)
        {
            var r = new RibbonModelMenuItem() { Header = label };
            Items.Add(r);
            return r;
        }
    }
}