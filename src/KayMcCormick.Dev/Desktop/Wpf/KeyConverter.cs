using System ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Data ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class KeyConverter : IValueConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
#pragma warning disable CA1305 // Specify IFormatProvider
            Logger.Info (
                         "{method} {value} {valueType} {targetType} {parameter} {parameterType}"
                       , nameof ( Convert )
                       , value?.ToString ( ) ?? ""
                       , value?.GetType ( ).FullName ?? ""
                       , targetType?.FullName ?? ""
                       , parameter?.ToString ( ) ?? ""
                       , parameter?.GetType ( ).FullName ?? ""
                        );
#pragma warning restore CA1305 // Specify IFormatProvider

            if ( targetType == typeof ( string ) )
            {
                return value?.ToString ( ) ?? "(null)" ;
            }

            Logger.Error ( "Not sure how to convert to type" ) ;
            return null ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}