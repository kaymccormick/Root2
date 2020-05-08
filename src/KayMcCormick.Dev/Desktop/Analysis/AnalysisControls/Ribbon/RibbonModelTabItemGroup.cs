using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelTabItemGroup : RibbonModelItem
    {
        public override string ToString()
        {
            return $"Group[{Header}, Count={Items.Count}]";
        }

        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        public RibbonModelItemMenuButton CreateRibbonMenuButton(string label)
        {
            var r = new RibbonModelItemMenuButton {Label = label};
            Items.Add(r);
            return r;
        }

        public RibbonModelItemComboBox CreateRibbonComboBox(Dictionary<object, object> dictionary)
        {
            var r = new RibbonModelItemComboBox();
            Items.Add(r);
            AddGalleryItems(dictionary, r.Items);
            var ribbonModelItemComboBox = CreateRibbonGallery(dictionary, "gal2");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }

        public RibbonModelItemSplitButton CreateRibbonSplitButton(Dictionary<object, object> dictionary)
        {
            var r = new RibbonModelItemSplitButton();
            Items.Add(r);
            var ribbonModelItemComboBox = CreateRibbonGallery(dictionary, "gal1");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }

        private static RibbonModelGallery CreateRibbonGallery(Dictionary<object, object> dictionary, string header)
        {
            var g = new RibbonModelGallery() {Header = header};

            AddGalleryItems(dictionary, g.Items);

            return g;
        }

        private static void AddGalleryItems(Dictionary<object, object> dictionary,
            ObservableCollection<RibbonModelItem> g)
        {
            foreach (var keyValuePair in dictionary)
            {
                var cat = new RibbonModelGalleryCategory() {Label = keyValuePair.Key};
                foreach (var vv in (IEnumerable) keyValuePair.Value)

                {
                    var item = new RibbonModelGalleryItem() {Content = vv};
                    cat.Items.Add(item);
                }

                g.Add(cat);
            }
        }
    }
}