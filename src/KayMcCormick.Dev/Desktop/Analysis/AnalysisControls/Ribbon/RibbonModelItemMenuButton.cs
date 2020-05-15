using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    /// 
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

        public override ControlKind Kind => ControlKind.RibbonMenuButton;
    }
}