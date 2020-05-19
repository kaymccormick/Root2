using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class GuidConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "null";
            }

            Guid? guid = value as Guid?;
            return guid?.ToString();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}