using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace AnalysisControls
{
    public class DirectoryInfoConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DirectoryInfo d) return d.EnumerateFileSystemInfos();
            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}