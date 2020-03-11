using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev
{
    public class LogLevelConverter : JsonConverter<LogLevel>
    {
        #region Overrides of JsonConverter<LogLevel>
        public override LogLevel Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return LogLevel.FromString ( reader.GetString ( ) ) ;
        }

        public override void Write (
            [ NotNull ] Utf8JsonWriter writer
          , [ NotNull ] LogLevel       value
          , JsonSerializerOptions      options
        )
        {
            if ( writer == null )
            {
                throw new ArgumentNullException ( nameof ( writer ) ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStringValue(value.Name);
        }
        #endregion
    }
}