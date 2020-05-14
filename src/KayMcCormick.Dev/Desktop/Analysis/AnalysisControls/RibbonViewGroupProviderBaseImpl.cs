using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AnalysisControls.RibbonModel;
using Autofac;

namespace AnalysisControls
{
    public class RibbonViewGroupProviderBaseImpl : RibbonViewGroupProviderBase
    {
        public override RibbonModelGroup ProvideModelItem(IComponentContext context)
        {
            var group = new RibbonModelGroup();
            group.CreateRibbonToggleButton("Toggle");
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

            var m = group.CreateRibbonMenuButton("Menu 1");
            var gal = PrimaryRibbonModel.CreateGallery();
            m.Items.Add(gal);
            var cat0 = PrimaryRibbonModel.CreateGalleryCategory(gal, "Cat0");
            var item0 = PrimaryRibbonModel.CreateGalleryItem(cat0, new TextBlock() {Text = typeof(Type).FullName});
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