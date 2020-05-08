using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelItemSplitButton : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }
}