using System ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// </summary>
    public sealed class JsonTypeInfoConverter : JsonConverter < TypeInfo >
    {
        #region Overrides of JsonConverter<TypeInfo>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
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
            [ JetBrains.Annotations.NotNull ] Utf8JsonWriter        writer
          , [ JetBrains.Annotations.NotNull ] TypeInfo              value
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