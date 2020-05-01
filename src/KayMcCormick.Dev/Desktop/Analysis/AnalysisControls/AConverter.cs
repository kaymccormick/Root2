using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AnalysisControls
{
    public class AConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string s = (string) value;
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}