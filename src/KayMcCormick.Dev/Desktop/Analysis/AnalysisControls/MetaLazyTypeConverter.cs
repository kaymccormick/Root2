using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AnalysisControls
{
    public class MetaLazyTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var type = value.GetType(); // meta lazy interface
            if (!type.IsGenericType)
            {
                return null;
            }

            var t2 = type.GetGenericTypeDefinition();
            var t3 = type.GenericTypeArguments[0];
            if (!t3.IsGenericType)
            {
                return null;
            }

            var t4 = t3.GenericTypeArguments[0];
            return t4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}