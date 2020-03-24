using System ;
using System.Globalization ;
using System.Reflection;
using System.Windows ;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    public class MethodInfoConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            MethodInfo methodInfo = ( MethodInfo ) value ;
            if ( value == null )
            {
                return "(null)" ;
            }

            if ((string)parameter == "Parameters" )
            {
                return methodInfo.GetParameters ( ) ;
            }

            return "(undefined)" ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}