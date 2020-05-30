using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelControlGroup : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelItemMenuButton CreateRibbonMenuButton(string label)
        {
            var r = new RibbonModelItemMenuButton { Label = label };
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
            var r = new RibbonModelItemComboBox() { Label = label };
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
            var g = new RibbonModelGallery() { Header = header };

            AddGalleryItems_(dictionary, g.Items);

            return g;
        }


        private static void AddGalleryItems_(Dictionary<object, object> dictionary,
            IList g)
        {
            foreach (var keyValuePair in dictionary)
            {
                var cat = new RibbonModelGalleryCategory() { Label = keyValuePair.Key };
                foreach (var vv in (IEnumerable)keyValuePair.Value)

                {
                    var item = new RibbonModelGalleryItem() { Content = vv };
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
        public RibbonModelButton CreateButton(string label)
        {
            var button = new RibbonModelButton() { Label = label };
            Items.Add(button);
            return button;
        }

        public RibbonModelTwoLineText createTwoLineText(string label)
        {
            var t = new RibbonModelTwoLineText() { Label = label };
            Items.Add(t);
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelItemTextBox CreateTextBox(string label)
        {
            var b = new RibbonModelItemTextBox() { Label = label };
            Items.Add(b);
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public RibbonModelTwoLineText CreateRibbonTwoLineText(string text)
        {
            var r = new RibbonModelTwoLineText() { Text = text };
            Items.Add(r);
            return r;
        }
        /// <returns></returns>
        public RibbonModelToggleButton CreateRibbonToggleButton(string text)
        {
            var r = new RibbonModelToggleButton() { Label = text };
            Items.Add(r);
            return r;
        }

        public RibbonModelControlGroup CreateControlGroup()
        {
            var r = new RibbonModelControlGroup();
            Items.Add(r);
            return r;
        }

        public RibbonModelRadioButton CreateRibbonRadioButton(string label)
        {
            var r = new RibbonModelRadioButton(){Label = label};
            Items.Add(r);
            return r;
        }

        public override ControlKind Kind => ControlKind.RibbonControlGroup;
    }
}