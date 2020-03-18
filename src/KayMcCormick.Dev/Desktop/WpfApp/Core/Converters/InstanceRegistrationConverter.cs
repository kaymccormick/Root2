using System ;
using System.Collections.Generic ;
using System.Globalization ;
using System.Windows.Controls ;
using System.Windows.Data ;
using Autofac.Features.Metadata ;
using KayMcCormick.Lib.Wpf ;
using WpfApp.Core.Model ;


namespace WpfApp.Core.Converters
{
    /// <summary>
    ///     IValueConverter implementation for converting instances of type
    ///     InstanceRegistration to a list or enumerable of Type.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    [ ValueConversion ( typeof ( InstanceRegistration ) , typeof ( IEnumerable < Type > ) ) ]
    public class InstanceRegistrationConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a value of type <see cref="InstanceRegistration" /> to an
        ///     Enumerable of Type.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />,
        ///     the valid null value is used.
        /// </returns>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var x = ( InstanceRegistration ) value ;
            if ( x == null )
            {
                return null ;
            }

            var r = new List < object > ( ) ;
            if ( x.Type.IsGenericType
                 && x.Type.GetGenericTypeDefinition ( )
                 == typeof ( Lazy < object > ).GetGenericTypeDefinition ( ) )
            {
                r.Add (
                       new Button
                       {
                           Content          = "Load"
                         , Command          = WpfAppCommands.Load
                         , CommandParameter = x.Instance
                       }
                      ) ;
            }

            if ( x.Type.IsGenericType
                 && x.Type.GetGenericTypeDefinition ( )
                 == typeof ( Meta < object > ).GetGenericTypeDefinition ( ) )
            {
                r.Add (
                       new Button
                       {
                           Content          = "Metadata"
                         , Command          = WpfAppCommands.Metadata
                         , CommandParameter = x.Instance
                       }
                      ) ;
            }

            return r ;
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />,
        ///     the valid null value is used.
        /// </returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            throw new NotImplementedException ( ) ;
        }
    }
}