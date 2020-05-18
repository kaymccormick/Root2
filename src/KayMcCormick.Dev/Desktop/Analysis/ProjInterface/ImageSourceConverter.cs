using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Windows.Media.Imaging ;
using JetBrains.Annotations ;

namespace ProjInterface
{
    internal sealed class ImageSourceConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        [ NotNull ]
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return new BitmapImage ( value as Uri ?? throw new AppInvalidOperationException ( ) ) ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}