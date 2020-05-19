using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    public class QATItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectTemplate = base.SelectTemplate(item, container);
            if (item is RibbonButton)
            {
                var altTemplate =
                    ((FrameworkElement) container).TryFindResource(new DataTemplateKey(typeof(RibbonButton))) as
                    DataTemplate;
                return altTemplate;
            }
            return selectTemplate;
        }
    }
}