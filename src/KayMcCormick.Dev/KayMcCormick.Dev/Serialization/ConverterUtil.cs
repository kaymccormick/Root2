using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace KayMcCormick.Dev.Serialization
{
    public class ConverterUtil
    {
        public static void WritePreamble (
            JsonConverter         converter
          , Utf8JsonWriter        writer
          , object                value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();
            writer.WriteString(
                               "Converter"
                             , converter.GetType().AssemblyQualifiedName
                              );
            JsonTypeConverter typeConverter = new JsonTypeConverter();
            writer.WritePropertyName("ObjectType");
            typeConverter.Write(writer, value.GetType(), options);
            writer.WritePropertyName("ObjectInstance");
        }

        public static void WriteTerminal ( Utf8JsonWriter writer)
        {
            writer.WriteEndObject();
        }
    }
}