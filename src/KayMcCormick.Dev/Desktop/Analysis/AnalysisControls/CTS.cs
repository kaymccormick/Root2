using System;
using System.Windows;
using System.Windows.Controls;
using AnalysisControls.TypeDescriptors;

namespace AnalysisControls
{
    public class CTS : DataTemplateSelector
    {
        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            InstanceProperty p = item as InstanceProperty;
            if (p.PropertyDescriptor.PropertyType.IsEnum)
            {
                bool flags = false;
                foreach (Attribute propertyDescriptorAttribute in p.PropertyDescriptor.Attributes)
                {
                    if (propertyDescriptorAttribute is FlagsAttribute)
                    {
                        flags = true;
                    }
                }

                if (flags)
                {
                    return ((FrameworkElement)container).TryFindResource("EnumFlags") as DataTemplate;
                }
            }

            if (p.PropertyDescriptor.Converter.GetStandardValuesSupported() && p.StandardValues.Count > 0)
            {
                return ((FrameworkElement) container).TryFindResource("StandardValues") as DataTemplate;
            }


            return ((FrameworkElement)container).TryFindResource("DefaultValue") as DataTemplate;

        }
    }
}