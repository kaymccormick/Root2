using System ;
using System.ComponentModel;
using System.Globalization ;
using System.Linq;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Helper converter to get info on types.</summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class TypeInfoConverter : IValueConverter

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
            var source = ( Type ) value ;
            if ( value == null )
            {
                return null ;
            }

            if ( ( string ) parameter == "Interfaces" )
            {
                return source.GetInterfaces ( ) ;
            }

            if ((string) parameter == "GenericTypeDefinition" && source.IsGenericType)
            {
                var genericTypeDefinition = source.GetGenericTypeDefinition();
                return genericTypeDefinition;
            }

            if ((string) parameter == "GetInterfaces")
            {
                return source.GetInterfaces();
            }


            if ((string)parameter == "Methods")
            {
                return source.GetMethods().Where(m => m.IsSpecialName == false);
            }

            if ((string) parameter == "Properties")
            {
                return source.GetProperties();
            }

            
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