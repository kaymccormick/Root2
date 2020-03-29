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

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonConverterImage : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( ImageSource ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }

        /// <summary>
        /// 
        /// </summary>
        public class MyImageSourceConverter : JsonConverter < ImageSource >
        {
            private Type                  typeToConvert ;
            private JsonSerializerOptions options ;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            public MyImageSourceConverter ( Type typeToConvert , JsonSerializerOptions options )
            {
                this.typeToConvert = typeToConvert ;
                this.options       = options ;
            }

            #region Overrides of JsonConverter<ImageSource>
            /// <summary>
            /// 
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public override ImageSource Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="options"></param>
            public override void Write (
                Utf8JsonWriter        writer
              , ImageSource           value
              , JsonSerializerOptions options
            )
            {
                var att = ( ValueSerializerAttribute ) value
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
                        ser = ( ValueSerializer ) Activator.CreateInstance (
                                                                            att.ValueSerializerType
                                                                           ) ;
                    }
                }

                if ( ser != null )
                {
                    var p = TypeDescriptor.GetProvider ( value ) ;
                    var t = p.GetTypeDescriptor ( value ) ;

                    var str = ser.ConvertToString ( value , null ) ;
                    if ( str != null )
                    {
                        writer.WriteStringValue ( str ) ;
                    }
                    else
                    {
                        writer.WriteNullValue ( ) ;
                    }
                }
                else
                {
                    writer.WriteStringValue ( value.ToString ( ) ) ;
                }
            }
            #endregion
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override System.Text.Json.Serialization.JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new MyImageSourceConverter ( typeToConvert , options ) ;
        }
        #endregion
    }
}