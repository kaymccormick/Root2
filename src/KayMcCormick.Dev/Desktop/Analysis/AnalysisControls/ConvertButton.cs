using System;
using System.Globalization;
using System.Windows.Data;
using AnalysisControls.RibbonModel;

namespace AnalysisControls
{
    public class ConvertButton : IValueConverter    
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RibbonModelItemMenuButton button = value as RibbonModelItemMenuButton;
            MyRibbonMenuButton r = new MyRibbonMenuButton();
            if (button != null)
            {
                r.Label = button.Label?.ToString();
                r.ItemsSource = button.Items;
                r.DataContext = button;
                
            }

            r.ToolTip = "Customize";
            return r;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}