using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelGroup : RibbonModelItem
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"Group[{Header}, Count={Items.Count}]";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelItemMenuButton CreateRibbonMenuButton(string label)
        {
            var r = new RibbonModelItemMenuButton {Label = label};
            Items.Add(r);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelItemComboBox CreateRibbonComboBox(string label)
        {
            var r = new RibbonModelItemComboBox() {Label = label};
            Items.Add(r);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public RibbonModelItemComboBox CreateRibbonComboBox_(Dictionary<object, object> dictionary)
        {
            var r = new RibbonModelItemComboBox();
            Items.Add(r);
            AddGalleryItems_(dictionary, r.Items);
            var ribbonModelItemComboBox = CreateRibbonGallery_(dictionary, "gal2");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public RibbonModelItemSplitButton CreateRibbonSplitButton_(Dictionary<object, object> dictionary)
        {
            var r = new RibbonModelItemSplitButton();
            Items.Add(r);
            var ribbonModelItemComboBox = CreateRibbonGallery_(dictionary, "gal1");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }

        private static RibbonModelGallery CreateRibbonGallery_(Dictionary<object, object> dictionary, string header)
        {
            var g = new RibbonModelGallery() {Header = header};

            AddGalleryItems_(dictionary, g.Items);

            return g;
        }


        private static void AddGalleryItems_(Dictionary<object, object> dictionary,
            IList g)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelItemButton CreateButton(string label)
        {
            var button = new RibbonModelItemButton() { Label = Label };
            return button;
        }
    }
}