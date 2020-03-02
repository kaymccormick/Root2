#region header
// Kay McCormick (mccor)
// 
// ConsoleApp1
// ProjLib
// VsInstanceConverter.cs
// 
// 2020-03-01-11:26 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Globalization ;
using System.IO ;
using MessageTemplates.Structure ;

namespace ProjLib
{
    class VsInstanceConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if ( typeof ( TemplatePropertyValue ).IsAssignableFrom ( destinationType ) )
            {
                return true ;
            }
            return base.CanConvertTo ( context , destinationType ) ;

        }

        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( typeof ( TemplatePropertyValue ).IsAssignableFrom ( destinationType ) )
            {
                var val = ( IVsInstance ) value ;
                List< TemplateProperty > prop  = new List < TemplateProperty > ();
                foreach ( var propertyInfo in typeof ( IVsInstance ).GetProperties ( ) )
                {
                    var propVal = propertyInfo.GetValue ( value ) ;
                    TemplatePropertyValue outPropVal = null ;
                    if ( propertyInfo.PropertyType != typeof ( string )
                         && typeof ( IEnumerable ).IsAssignableFrom ( propertyInfo.PropertyType ) )
                    {

                    } else
                    {
                        ScalarValue tPropVal = new ScalarValue ( propVal ) ;
                        outPropVal = tPropVal ;
                    }

                    if ( outPropVal != null )
                    {
                        prop.Add ( new TemplateProperty(propertyInfo.Name, outPropVal) );
                    }
                }
                var outVal = new VsInstancePropertyValue(val);
                return outVal ;
            }

            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }
        #endregion
    }

    public class VsInstancePropertyValue : TemplatePropertyValue
    {
        private IVsInstance instance ;
        public VsInstancePropertyValue ( IVsInstance instance ) { this.instance = instance ; }

        #region Overrides of TemplatePropertyValue
        public override void Render (
            TextWriter      output
          , string          format         = null
          , IFormatProvider formatProvider = null
        )
        {
            output.Write ( this.instance.InstallationVersion ) ;
        }
        #endregion
    }
}