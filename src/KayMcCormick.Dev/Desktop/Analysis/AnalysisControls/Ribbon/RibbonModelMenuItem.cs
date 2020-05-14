using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
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

        public override ControlKind Kind => ControlKind.RibbonMenuItem;
    }
}