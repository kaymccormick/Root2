using System ;
using System.Globalization ;
using System.Reflection;
using System.Windows ;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodInfoConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}