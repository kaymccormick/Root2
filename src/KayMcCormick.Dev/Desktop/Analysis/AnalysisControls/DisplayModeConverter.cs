using System;
using System.Globalization;
using System.Windows.Data;

namespace AnalysisControls
{
    public class DisplayModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            SyntaxTypeDisplayMode m = (SyntaxTypeDisplayMode) value;
            SyntaxTypeDisplayMode disp1 = SyntaxTypeDisplayMode.None;
            if (parameter is SyntaxTypeDisplayMode disp)
            {
                return m == disp;
            } else if (SyntaxTypeDisplayMode.TryParse(parameter.ToString(), out disp1))
            {
                return m == disp1;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}