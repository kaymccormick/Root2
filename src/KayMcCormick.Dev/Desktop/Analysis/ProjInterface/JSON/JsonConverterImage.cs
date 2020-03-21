#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonConverterImage.cs
// 
// 2020-03-19-6:23 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Markup ;
using System.Windows.Media ;

namespace ProjInterface.JSON
{
    public class JsonConverterImage : JsonConverterFactory
    {
/*        #region Overrides of JsonConverter<ImageSource>
        public override ImageSource Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , ImageSource           value
          , JsonSerializerOptions options
        )
        {
            ValueSerializerAttribute att = ( ValueSerializerAttribute ) value
                                                                       .GetType ( )
                                                                       .GetCustomAttribute (
                                                                                            typeof (
                                                                                                ValueSerializerAttribute
                                                                                            )
                                                                                           ) ;
            ValueSerializer ser = null ;
            if ( att != null )
            {
                if ( att.ValueSerializerType != null )
                {
                    ser = ( ValueSerializer ) Activator.CreateInstance ( att.ValueSerializerType ) ;
                }
            }

            if ( ser != null )
            {
                var p = TypeDescriptor.GetProvider ( value ) ;
                var t = p.GetTypeDescriptor ( value ) ;
                var str = ser.ConvertToString ( value , null ) ;
                if ( str != null )
                {
                    writer.WriteStringValue(str);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
        #endregion*/
#region Overrides of JsonConverter
public override bool CanConvert ( Type typeToConvert )
{
    if ( typeof ( ImageSource ).IsAssignableFrom ( typeToConvert ) )
    {
        return true ;
    }

    return false ;
}

public class MyImageSourceConverter : JsonConverter<ImageSource>
{
            private Type typeToConvert;
            private JsonSerializerOptions options;

            public MyImageSourceConverter(Type typeToConvert, JsonSerializerOptions options)
            {
                this.typeToConvert = typeToConvert;
                this.options = options;
            }
            #region Overrides of JsonConverter<ImageSource>
            public override ImageSource Read (
        ref Utf8JsonReader    reader
      , Type                  typeToConvert
      , JsonSerializerOptions options
    )
    {
        return null ;
    }

            public override void Write (
                Utf8JsonWriter        writer
              , ImageSource           value
              , JsonSerializerOptions options
            )
            {

                ValueSerializerAttribute att = (ValueSerializerAttribute)value
                                                                        .GetType()
                                                                        .GetCustomAttribute(
                                                                                            typeof(
                                                                                                ValueSerializerAttribute
                                                                                            )
                                                                                           );
                ValueSerializer ser = null;
                if (att != null)
                {
                    if (att.ValueSerializerType != null)
                    {
                        ser = (ValueSerializer)Activator.CreateInstance(att.ValueSerializerType);
                    }
                }

                if (ser != null)
                {
                    var p = TypeDescriptor.GetProvider(value);
                    var t = p.GetTypeDescriptor(value);
                    var str = ser.ConvertToString(value, null);
                    if (str != null)
                    {
                        writer.WriteStringValue(str);
                    }
                    else
                    {
                        writer.WriteNullValue();
                    }
                }
                else
                {
                    writer.WriteStringValue(value.ToString());
                }
            }
    #endregion
}
#endregion
#region Overrides of JsonConverterFactory
public override JsonConverter CreateConverter (
    Type                  typeToConvert
  , JsonSerializerOptions options
)
{
    return new MyImageSourceConverter ( typeToConvert , options )  ;
}
#endregion
    }
}