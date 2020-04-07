#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// DataTemplateKeyConverter.cs
// 
// 2020-03-12-7:06 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class DataTemplateKeyConverter : JsonConverter < DataTemplateKey >
    {
        #region Overrides of JsonConverter<DataTemplateKey>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DataTemplateKey Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            Utf8JsonWriter        writer
          , DataTemplateKey       value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            var dt = value.DataType ;
            if ( dt is Type t )
            {
                writer.WritePropertyName ( "DataType" ) ;
                JsonSerializer.Serialize ( writer , t , options ) ;
            }
            else if ( dt != null )
            {
                writer.WriteString ( "DataType" , dt.ToString ( ) ) ;
            }

            if ( value.Assembly != null )
            {
                writer.WriteString ( "Assembly" , value.Assembly.FullName ) ;
            }

            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}