using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTemplateSelector : DataTemplateSelector
    {
        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DebugUtils.WriteLine($"{item} ({item?.GetType()?.FullName}");
            if (item is IRibbonModelItem item2)
            {
                DebugUtils.WriteLine($"got IRobbonModelItem");
                if (item2.TemplateKey != null)
                {
                    DebugUtils.WriteLine($"TemplateKEy is {item2.TemplateKey}");
                    var x = ((FrameworkElement) container).TryFindResource(item2.TemplateKey) as DataTemplate;
                    if (x != null) return x;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}