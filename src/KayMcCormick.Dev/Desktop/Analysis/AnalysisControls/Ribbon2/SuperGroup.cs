namespace AnalysisControls.Ribbon2
{
    /// <summary>
    /// 
    /// </summary>
    public class SuperGroup : RibbonViewGroupProviderBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override RibbonModelGroup ProvideModelItem()
        {
            var group = new RibbonModelGroup(){Header="Super group"};
            group.SmallImageSource = "pack://application:,,,/WpfLib;component/Assets/ASPWebSite_16x.png";
            var cg2 = @group.CreateControlGroup();
            var calendar = new ModelCalendar();
            var datePicker = new ModelDatePicker(){};
            cg2.Items.Add(calendar);
            cg2.Items.Add(datePicker);

            var mb = group.CreateRibbonMenuButton("Menu XX");
            var gal = PrimaryRibbonModel.CreateModelGallery();
            mb.ItemsCollection.Add(gal);
            var cat = gal.CreateGalleryCategory("Cat1");
            cat.CreateGalleryItem(new Image {Source = new BitmapImage {UriSource = new Uri(@group.SmallImageSource.ToString())}});
            group.CreateTextBox("Test");
            return @group;
        }
    }
}