using System ;
using System.Globalization ;
using System.Windows.Data ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class KeyConverter : IValueConverter
    {
#if TRACE_TEMPLATES
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif
        #region Implementation of IValueConverter
        /// <summary>
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
            Logger.Info (
                         "{method} {value} {valueType} {targetType} {parameter} {parameterType}"
                       , nameof ( Convert )
                       , value?.ToString ( )             ?? ""
                       , value?.GetType ( ).FullName     ?? ""
                       , targetType?.FullName            ?? ""
                       , parameter?.ToString ( )         ?? ""
                       , parameter?.GetType ( ).FullName ?? ""
                        ) ;


            if ( targetType == typeof ( string ) )
            {
                return value?.ToString ( ) ?? "(null)" ;
            }

            Logger.Error ( "Not sure how to convert to type" ) ;
            return null ;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}