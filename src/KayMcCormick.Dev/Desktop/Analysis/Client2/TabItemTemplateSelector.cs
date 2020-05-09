using System.Windows;
using System.Windows.Controls;
using AnalysisControls.RibbonM;

namespace Client2
{
    public class TabItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            
            if (item is RibbonModelTab tab)
            {
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