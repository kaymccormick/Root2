using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelItemMenuButton : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }
}