using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;

namespace RibbonLib.Model
{
    /// <summary>
    /// <see cref="RibbonQuickAccessToolBar"/>
    /// </summary>
    public class RibbonModelQuickAccessToolBar
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; set; }= new ObservableCollection<object>();

        public RibbonModelItemMenuButton CustomizeMenuButton { get; set; }
    }
}