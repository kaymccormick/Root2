#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// ResolveUiComponentTypeConverter.cs
// 
// 2020-03-17-12:08 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.ComponentModel.Design.Serialization ;
using System.Globalization ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class ResolveUiComponentTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            return destinationType == typeof ( InstanceDescriptor )
                   || base.CanConvertTo ( context , destinationType ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( ! ( destinationType == typeof ( InstanceDescriptor ) ) )
            {
                return base.ConvertTo ( context , culture , value , destinationType ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            if ( ! ( value is ResolveUiComponentExtension extension ) )
            {
                throw new ArgumentException (
                                             $"{nameof ( value )} must be of type ResolveExtension"
                                           , nameof ( value )
                                            ) ;
            }

            return new InstanceDescriptor (
                                           typeof ( ResolveUiComponentExtension ).GetConstructor (
                                                                                                  new[]
                                                                                                  {
                                                                                                      typeof
                                                                                                      ( object
                                                                                                      )
                                                                                                  }
                                                                                                 )
                                         , new object[] { extension.ComponentType }
                                          ) ;
        }
        #endregion
    }
}