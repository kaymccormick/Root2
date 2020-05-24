using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using AnalysisControls.TypeDescriptors;

namespace AnalysisControls
{
    public class InstPropValTempSel : DataTemplateSelector
    {
        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            
            if (item != null)
            {
                var t = item.GetType().IsEnum && item.GetType().GetCustomAttributes<FlagsAttribute>().Any(); //i.PropertyDescriptor.PropertyType.IsEnum)
                if (t)
                {
                    var tryFindResource =
                        ((FrameworkElement) container).TryFindResource(typeof(Enum)) as
                        DataTemplate;
                    if (tryFindResource != null) return tryFindResource;
                }

            }
            return base.SelectTemplate(item, container);
        }
    }
}