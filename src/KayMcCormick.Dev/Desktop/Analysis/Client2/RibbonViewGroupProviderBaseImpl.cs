using System.Windows.Media;
using System.Windows.Shapes;
using AnalysisControls.RibbonM;
using Autofac;

namespace Client2
{
    public class RibbonViewGroupProviderBaseImpl : RibbonViewGroupProviderBase
    {
        public override RibbonModelGroup ProvideModelItem(IComponentContext context)
        {
            var group = new RibbonModelGroup();
            var combo = group.CreateRibbonComboBox("Combo 1");
            var gallery = combo.CreateGallery();

            {


                var cat1 = RibbonModel.CreateGalleryCategory(gallery, "Cat 1");
                Brush[] colors = new[] {Brushes.Pink, Brushes.Green, Brushes.Aqua};
                foreach (var color in colors)
                {
                    RibbonModel.CreateGalleryItem(cat1, new Rectangle() {Width = 25, Height = 25, Fill = color});
                }
            }
            {
                var cat1 = RibbonModel.CreateGalleryCategory(gallery, "Cat 2");
                Brush[] colors = new[] { Brushes.Pink, Brushes.Green, Brushes.Aqua };
                foreach (var color in colors)
                {
                    RibbonModel.CreateGalleryItem(cat1, new Rectangle() { Width = 25, Height = 25, Stroke = color });
                }
            }
            return group;
        }
    }
}