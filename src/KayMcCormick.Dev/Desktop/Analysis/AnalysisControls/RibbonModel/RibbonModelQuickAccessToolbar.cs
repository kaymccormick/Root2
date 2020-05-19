using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// <see cref="System.Windows.Controls.Ribbon.RibbonQuickAccessToolbar"/>
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