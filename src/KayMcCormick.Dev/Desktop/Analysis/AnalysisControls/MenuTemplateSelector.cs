using System.Windows;
using System.Windows.Controls;
using AnalysisControls.RibbonModel;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class MenuTemplateSelector : ItemContainerTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
        {
            DebugUtils.WriteLine($"{item}{item?.GetType()} {parentItemsControl}");

            if (item is RibbonModelGallery g)
            {
                return parentItemsControl.TryFindResource("GalleryMenuContainer") as DataTemplate;
            }
            return base.SelectTemplate(item, parentItemsControl);
        }
    }
}