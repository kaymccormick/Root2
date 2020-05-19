using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AnalysisAppLib.Syntax;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class KindsConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AppTypeInfo v = (AppTypeInfo) value;
            if (value == null)
            {
                return null;
            }

            if (v.Kinds.Count == 1)
            {
                return v.SyntaxKinds[0];
            }
            else
            {
                return v.SyntaxKinds;
            }
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}