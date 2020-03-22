using System ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    public class ExceptionConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            Exception ex = ( Exception ) value ;
            return ex?.ToString ( ) ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}