using System ;
using System.Collections ;
using System.Globalization ;
using System.Windows.Data ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    internal sealed class DictionaryConverter : IValueConverter
    {
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
            if ( ! ( value is IDictionary d ) )
            {
                return null ;
            }

            if ( parameter == null
                 || ! d.Contains ( parameter ) )
            {
                return null ;
            }

            DebugUtils.WriteLine( d[ parameter ].ToString() ) ;
            return d[ parameter ] ;

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