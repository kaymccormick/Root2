using System;
using System.Globalization;
using System.Windows.Data;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class TestConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DebugUtils.WriteLine($"{parameter} - {value} - {targetType.FullName}");
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}