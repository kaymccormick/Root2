using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelItemComboBox : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }
}