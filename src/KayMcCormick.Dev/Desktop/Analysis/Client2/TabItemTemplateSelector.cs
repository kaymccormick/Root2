using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;
using RibbonLib.Model;

namespace Client2
{
    public class TabItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DebugUtils.WriteLine($"{item} {container}");
            if (item is RibbonModelTab)
            {
                DebugUtils.WriteLine($"{item} {container}");
                FrameworkElement c = (FrameworkElement) container;
                var r = c.TryFindResource(new DataTemplateKey(typeof(RibbonModelTab)));
                if (r != null)
                {
                    return (DataTemplate) r;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}