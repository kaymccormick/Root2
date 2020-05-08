using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelGallery : RibbonModelItem
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }
}