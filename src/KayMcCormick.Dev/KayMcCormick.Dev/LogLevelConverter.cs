using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter'
    public class LogLevelConverter : JsonConverter<LogLevel>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter'
    {
        #region Overrides of JsonConverter<LogLevel>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)'
        public override LogLevel Read (
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)'
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return LogLevel.FromString ( reader.GetString ( ) ) ;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter.Write(Utf8JsonWriter, LogLevel, JsonSerializerOptions)'
        public override void Write (
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogLevelConverter.Write(Utf8JsonWriter, LogLevel, JsonSerializerOptions)'
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