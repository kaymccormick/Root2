using System ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    public class TypeInfoConverter : IValueConverter

    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            Type source = ( Type ) value ;
            if ( value == null ) return null;
            if ( (string)parameter == "Interfaces" )
            {
                return source.GetInterfaces ( ) ;
            }

            return null ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}