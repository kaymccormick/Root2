using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace ProjTests
{
    internal class MyConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PropertyDescriptor p = (PropertyDescriptor) parameter;
            if (value != null)
            {
                var val = p.GetValue(value);
                return val;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}