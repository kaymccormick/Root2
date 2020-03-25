using System ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Media ;

namespace KayMcCormick.Lib.Wpf
{
    public class ColorConverter : IValueConverter
    {

        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            SolidColorBrush brush ;
            if ( value != null )
            {
                uint color = ( uint ) value ;
                brush = new SolidColorBrush(Color.FromArgb((byte)((color & 0xff000000) >> 24),
                                                           (byte)((color & 0xff0000)   >> 16),
                                                           (byte)((color & 0xff00)     >> 8),
                                                           (byte)(color & 0xff)
                                                          ));
                return brush ;
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}