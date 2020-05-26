using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    public class DiagnosticConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
         Diagnostic d = value as Diagnostic;
         return d?.GetMessage();

        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}