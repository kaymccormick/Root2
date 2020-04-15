using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConverterUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public static void WritePreamble (
            [ NotNull ] JsonConverter         converter
          , [ NotNull ] Utf8JsonWriter        writer
          , [ NotNull ] object                value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();
            writer.WriteString(
                               "Converter"
                             , converter.GetType().AssemblyQualifiedName
                              );
            var typeConverter = new JsonTypeConverter();
            writer.WritePropertyName("ObjectType");
            typeConverter.Write(writer, value.GetType(), options);
            writer.WritePropertyName("ObjectInstance");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public static void WriteTerminal ( [ NotNull ] Utf8JsonWriter writer)
        {
            writer.WriteEndObject();
        }
    }
}