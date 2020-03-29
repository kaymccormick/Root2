using System ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// </summary>
    public class JsonTypeInfoConverter : JsonConverter < TypeInfo >
    {
        #region Overrides of JsonConverter<TypeInfo>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TypeInfo Read (
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
          , TypeInfo              value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "AssemblyQualifiedName" , value.AssemblyQualifiedName ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}