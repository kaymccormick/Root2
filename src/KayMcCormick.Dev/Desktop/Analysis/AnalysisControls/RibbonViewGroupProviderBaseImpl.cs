using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Autofac;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonViewGroupProviderBaseImpl : RibbonViewGroupProviderBase
    {
        /// <inheritdoc />
        public override RibbonModelGroup ProvideModelItem()
        {
            var group = new RibbonModelGroup();
            group.CreateRibbonToggleButton("Toggle");
            var combo0 = @group.CreateRibbonComboBox("Combo!");
            var gal0 = PrimaryRibbonModel.CreateGallery();
            var gal2 = PrimaryRibbonModel.CreateGallery();
            combo0.Items.Add(gal0);
            combo0.Items.Add(gal2);
            var galleryCategory = PrimaryRibbonModel.CreateGalleryCategory(gal0, "eep");
            PrimaryRibbonModel.CreateGalleryItem(galleryCategory, "boop");
            PrimaryRibbonModel.CreateGalleryItem(galleryCategory, "boop2");

            var galleryCategory3 = PrimaryRibbonModel.CreateGalleryCategory(gal0, "eep2");
            PrimaryRibbonModel.CreateGalleryItem(galleryCategory3, "boop");
            PrimaryRibbonModel.CreateGalleryItem(galleryCategory3, "boop5");

            var galleryCategory2 = PrimaryRibbonModel.CreateGalleryCategory(gal2, "bop");
            PrimaryRibbonModel.CreateGalleryItem(galleryCategory2, "boop");

            var cg = group.CreateControlGroup();
            cg.CreateRibbonRadioButton("One");
            cg.CreateRibbonRadioButton("Two");
            cg.CreateRibbonRadioButton("Three");
            var cg2 = group.CreateControlGroup();
            cg2.createTwoLineText("beep");
            cg2.CreateButton("derp");
            cg2.CreateRibbonRadioButton("lala");
            var datePicker = new ModelDatePicker(){};
            cg2.Items.Add(datePicker);

            group.Background = Brushes.GreenYellow;
            
            var m = group.CreateRibbonMenuButton("Menu 1");
            m.IsChecked = true;
            var item = m.CreateMenuItem("test 1");
            item.IsVerticallyResizable = true;
            item.IsHorizontallyResizable = true;
            item.IsCheckable = true;
            item.ToolTipDescription= "test";
            item.ToolTipTitle = "poo";
            
            
            var m2 = group.CreateRibbonMenuButton("Menu 2");
            var m3 = m2.CreateMenuItem("label");
            m3.CreateMenuItem("label");

            var tw = @group.CreateRibbonTwoLineText("Derp");
            tw.HasTwoLines = true;
            var g = new EllipseGeometry(new Rect(new Size(12, 12)));
            tw.PathData = g;
            group.CreateTextBox("Input  1");
            var combo = group.CreateRibbonComboBox("Combo 1");
            var gallery = combo.CreateGallery();

            {


                var cat1 = PrimaryRibbonModel.CreateGalleryCategory(gallery, "Cat 1");
                Brush[] colors = new[] {Brushes.Pink, Brushes.Green, Brushes.Aqua};
                foreach (var color in colors)
                {
                    PrimaryRibbonModel.CreateGalleryItem(cat1, new Rectangle() {Width = 25, Height = 25, Fill = color});
                }
            }
            {
                var cat1 = PrimaryRibbonModel.CreateGalleryCategory(gallery, "Cat 2");
                Brush[] colors = new[] { Brushes.Pink, Brushes.Green, Brushes.Aqua };
                foreach (var color in colors)
                {
                    PrimaryRibbonModel.CreateGalleryItem(cat1, new Rectangle() { Width = 25, Height = 25, Stroke = color });
                }
            }
            return group;
        }
    }

    public class ModelDatePicker
    {
        public DateTime SelectedDate { get; set; }
    }
}