using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    public class UiConverter  : IValueConverter
    {
        UiElementTypeConverter typeConverter = new UiElementTypeConverter(null);
        public UiConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return typeConverter.ConvertTo(value, typeof(UIElement));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}