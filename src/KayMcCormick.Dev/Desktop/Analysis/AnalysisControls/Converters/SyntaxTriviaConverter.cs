using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls.Converters
{
    public class SyntaxTriviaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SyntaxTrivia? v = (SyntaxTrivia?) value;
            if (value == null || !v.HasValue)
                return null;
            var s = v.Value;
            return s.Kind().ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}