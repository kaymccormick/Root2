using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelMenuItem: RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        public object Header
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelMenuItem CreateMenuItem(string label)
        {
            var r = new RibbonModelMenuItem() { Header = label };
            Items.Add(r);
            return r;
        }

        /// <inheritdoc />
        public override ControlKind Kind => ControlKind.RibbonMenuItem;
    }
}