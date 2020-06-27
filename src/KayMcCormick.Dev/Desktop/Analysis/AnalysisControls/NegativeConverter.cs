using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalysisControls
{
    public class NegativeConverter:IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            double f = (double) value;
            return -1 * f;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}