using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RibbonModelApplicationMenu AppMenu { get; set; } = new RibbonModelApplicationMenu();

        /// <summary>
        /// 
        /// </summary>
        public RibbonModel()
        {
            var menuItem = AppMenu.CreateSplitMenuItem("First split");
            var menu1 = menuItem.CreateAppMenuItem("Menu Item1");
            menuItem.CreateAppMenuItem("Menu Item2");

            var tab1 = CreateTab("Tab 1");
            var tab1Group1 = tab1.CreateGroup("Group1");

            var ribbonModelTab = CreateTab("Tab 2");
            var group = new RibbonModelTabItemGroup() {Header = "my group"};
            {
                var dictionary = new Dictionary<object, object>();
                dictionary["green"] = new[] {"foo", "Bar", "baz"};
                dictionary["test123"] = new[] {"foo", "Bar", "baz"};
                var combo = group.CreateRibbonComboBox(dictionary);
                var ribbonModelGallery = new RibbonModelGallery();
            }

            {
                var dictionary = new Dictionary<object, object>();

                dictionary["green"] = new[] {"foo", "Bar", "baz"};
                dictionary["test123"] = new[] {"foo", "Bar", "baz"};
                group.CreateRibbonSplitButton(dictionary);
            }

            // var addItemsTo = ribbonModelGalleryCatgory.Items;
            // var ribbonSplitButton = new RibbonModelItemSplitButton() {Label = "test"};
            // var modelGalleryCatgory = new RibbonModelGalleryCategory();
            // ribbonSplitButton.Items.Add(modelGalleryCatgory);
            // modelGalleryCatgory.Items.Add(new RibbonModelGalleryItem(){Content = "cheers"});
            // group.Items.Add(ribbonSplitButton);
            // var cmds = new[] {"Foo", "bar", "baaz"};
            // foreach (var cmd in cmds)
            // {
            // var item
            // = new RibbonModelGalleryItem();
            // item.Content = cmd;
            // addItemsTo.Add(item);

            // }

            // foreach (var solidColorBrush in new[] {Brushes.Pink, Brushes.Green, Brushes.Bisque})
            // {
            // var item = new RibbonModelGalleryItem()
            // {
            // Content = new Border {Background = solidColorBrush, Child = new Rectangle {Width = 40, Height = 40}}

            // };
            // addItemsTo.Add(item);

            // }

            // var ribbonModelItemComboBox = new RibbonModelItemComboBox() { Label = "MCombo1" };
            // var ribbonModelGalleryCatgory = new RibbonModelGalleryCategory() { Content = "fo", Header = "test" };
            // ribbonModelGallery.Items.Add(ribbonModelGalleryCatgory);

//            ribbonModelItemComboBox.Items.Add(ribbonModelGallery);
            //group.Items.Add(ribbonModelItemComboBox);

            var group2 = ribbonModelTab.CreateGroup("Group 2");
            group2.Items.Add(new RibbonModelItemTextBox() {Label = "EAt me"});
            var b = group2.CreateRibbonMenuButton("test");
            b.Items.Add(new RibbonModelItemMenuButton {Label = "derp"});
            //group2.CreateRibbonRadioButton("");
            ribbonModelTab.Items.Add(group);
        }

        private RibbonModelTab CreateTab(string header
        )
        {
            var tab = new RibbonModelTab {Header = header};
            RibbonItems.Add(tab);
            return tab;
        }


        public ObservableCollection<RibbonModelTab> RibbonItems { get; } = new ObservableCollection<RibbonModelTab>();
    }
}