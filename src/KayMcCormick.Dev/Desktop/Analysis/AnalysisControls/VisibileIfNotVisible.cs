using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class VisibileIfNotVisible : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            Visibility v = (Visibility) value;
            return v == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}