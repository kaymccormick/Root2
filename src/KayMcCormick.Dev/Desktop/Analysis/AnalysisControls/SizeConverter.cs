using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SizeConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            Size r = (Size) value;
            var height = (int)r.Height;
            var width = (int) r.Width;
            return $"( {width} x {height} )";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}