using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelGalleryCategory : RibbonModelItem
    {
        public RibbonModelGalleryCategory()
        {
        }

        public ObservableCollection<RibbonModelGalleryItem> Items { get; } =
            new ObservableCollection<RibbonModelGalleryItem>();

        public object Content { get; set; }
    }
}