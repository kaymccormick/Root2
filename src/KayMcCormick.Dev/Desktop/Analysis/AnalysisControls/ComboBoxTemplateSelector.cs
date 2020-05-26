using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalysisControls.RibbonModel;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ComboBoxTemplateSelector: ItemContainerTemplateSelector
    {
        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
        {
            Func<DependencyObject, DependencyObject> p = VisualTreeHelper.GetParent;
            DependencyObject d = parentItemsControl;
            ItemsControl x;
            object o = null;
            do
            {
                var d2 = p(d);
                x = ItemsControl.ItemsControlFromItemContainer(d2);
                if (x != null)
                {
                    o = x.ItemContainerGenerator.ItemFromContainer(d2);
                }
                d = d2;
            } while (d != null && x == null);

            if (o != null)
            {
                if (o is RibbonModelItemMenuButton xxx)
                {
                    var key = xxx.ItemContainerTemplateKey;
                    if (key != null)
                    {
                        var rr = x.TryFindResource(key) as DataTemplate;
                        if (rr != null)
                        {
                            return rr;
                        }
                    }
                }
            }

            if (item is RibbonModelGallery)
            {
                return parentItemsControl.TryFindResource("RibbonModelGalleryContainer") as DataTemplate;
            }else if (item is RibbonModelGalleryCategory)
            {
                return parentItemsControl.TryFindResource("RibbonModelGalleryCategoryContainer") as DataTemplate;
            } else if (item is RibbonModelGalleryItem)
            {
                return parentItemsControl.TryFindResource("RibbonModelGalleryItemContainer") as DataTemplate;
            } else if(item is RibbonModelSeparator)
            {
                return parentItemsControl.TryFindResource("RibbonModelSeparatorContainer") as DataTemplate;
            } else if (item is RibbonModelItem i)
            {
                switch (i.Kind)
                {
                    case ControlKind.RibbonGallery:
                        return parentItemsControl.TryFindResource("RibbonModelGalleryContainer") as DataTemplate;
                    case ControlKind.RibbonGalleryCategory:
                        return parentItemsControl.TryFindResource("RibbonModelGalleryCategoryContainer") as DataTemplate;
                    case ControlKind.RibbonGalleryItem:
                        return parentItemsControl.TryFindResource("RibbonModelGalleryItemContainer") as DataTemplate;
                    case ControlKind.RibbonSeparator:
                        return parentItemsControl.TryFindResource("RibbonKindSeparator") as DataTemplate;
                }
            }

            // if (item != null)
            // {
                // var r = parentItemsControl.TryFindResource(new DataTemplateKey(item.GetType())) as DataTemplate;
                // return r;
            // }
            return base.SelectTemplate(item, parentItemsControl);
        }
    }
}