using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnalysisControls.RibbonM;
using Autofac;
using Microsoft.Graph;
using Image = System.Windows.Controls.Image;

namespace AnalysisControls
{
    public class RibbonViewGroupProviderBaseImpl2 : RibbonViewGroupProviderBase
    {
        public override RibbonModelGroup ProvideModelItem(IComponentContext context)
        {
            var group = new RibbonModelGroup(){Header="Super group"};
            group.SmallImageSource = "pack://application:,,,/WpfLib;component/Assets/ASPWebSite_16x.png";
            var cg2 = @group.CreateControlGroup();
            var calendar = new ModelCalendar();
            var datePicker = new ModelDatePicker(){};
            cg2.Items.Add(calendar);
            cg2.Items.Add(datePicker);

            var mb = group.CreateRibbonMenuButton("Menu");
            var gal = RibbonModel.CreateModelGallery();
            mb.Items.Add(gal);
            var cat = gal.CreateGalleryCategory("Cat1");
            cat.CreateGalleryItem(new Image {Source = new BitmapImage {UriSource = new Uri(@group.SmallImageSource.ToString())}});
            group.CreateTextBox("Test");
            return @group;
        }
    }

    public class ModelCalendar
    {
        public DateTime SelectedDate { get; set; }
    }
}