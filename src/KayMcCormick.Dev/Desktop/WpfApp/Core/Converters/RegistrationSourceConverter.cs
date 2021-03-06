using System ;
using System.Globalization ;
using System.Windows.Data ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev.Interfaces ;

namespace WpfApp.Core.Converters
{
    /// <summary></summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for RegistrationSourceConverter
    public class RegistrationSourceConverter : IValueConverter
    {
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />, the
        ///     valid null value is used.
        /// </returns>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var c = value as IComponentRegistration ;
            var x = parameter as ILifetimeScope ;
            if ( x == null )
            {
                if ( c != null )
                {
                    return c.Id ;
                }

                //return new object[0];
            }

            if ( x != null )
            {
                var objectIdProvider = x.Resolve < IObjectIdProvider > ( ) ;
                var instanceByComponentRegistration =
                    objectIdProvider.GetInstanceByComponentRegistration ( c ) ;
                return instanceByComponentRegistration ;
            }

            throw new InvalidOperationException ( ) ;
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />, the
        ///     valid null value is used.
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