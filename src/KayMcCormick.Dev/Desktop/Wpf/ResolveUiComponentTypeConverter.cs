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
using System.Collections ;
using System.ComponentModel ;
using System.ComponentModel.Design.Serialization ;
using System.Globalization ;
using System.Reflection ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResolveUiComponentTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            return destinationType == typeof ( InstanceDescriptor )
                   || base.CanConvertTo ( context , destinationType ) ;
        }

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

            return ( object ) new InstanceDescriptor (
                                                      ( MemberInfo )
                                                      typeof ( ResolveUiComponentExtension )
                                                         .GetConstructor (
                                                                          new Type[ 1 ]
                                                                          {
                                                                              typeof ( object )
                                                                          }
                                                                         )
                                                    , ( ICollection ) new object[ 1 ]
                                                                      {
                                                                          extension.ComponentType
                                                                      }
                                                     ) ;
        }
        #endregion
    }
}