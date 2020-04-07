using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;

namespace ProjInterface
{
    public class ImageSourceConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return new BitmapImage ( value as Uri ?? throw new InvalidOperationException ( ) ) ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}