using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls.RibbonModel
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