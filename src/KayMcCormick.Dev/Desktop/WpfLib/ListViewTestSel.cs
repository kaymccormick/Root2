using System.ComponentModel;
using System.Windows;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    public class ListViewTestSel : CustomDataTemplateSelector
    {
        private readonly PropertyDescriptor _prop;

        public ListViewTestSel(PropertyDescriptor prop)
        {
            _prop = prop;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // var selectTemplate = base.SelectTemplate(_prop.GetValue(item), container);
            // if (selectTemplate == null)
            
            object resourceKey = $"{item.GetType().Name}.{_prop.Name}";
            
            var tryFindResource = ((FrameworkElement) container).TryFindResource(resourceKey) as DataTemplate;
            if (tryFindResource == null)
            {
                DebugUtils.WriteLine(resourceKey.ToString());
                return ((FrameworkElement)container).TryFindResource("Fallback") as DataTemplate;
            }
            return tryFindResource;
            
            //return selectTemplate;
        }
    }
}