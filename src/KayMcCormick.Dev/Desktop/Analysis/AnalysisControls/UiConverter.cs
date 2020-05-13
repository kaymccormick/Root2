using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class UiConverter  : IValueConverter
    {
        readonly UiElementTypeConverter _typeConverter = new UiElementTypeConverter(null);

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _typeConverter.ConvertTo(value, typeof(UIElement));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}