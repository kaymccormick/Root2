#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// LogEventInstance.cs
// 
// 2020-03-08-8:31 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using NLog ;

namespace ProjLib
{
    public class LogEventInstance
    {
        [JsonPropertyName( "logger")]
        public string Logger { get ; set ; }
        [JsonPropertyName( "@mt")]
        public string MessageTemplate { get ; set; }
        [JsonPropertyName( "@t")]
        [JsonConverter(typeof(EventTimeConverter))]
        public DateTime EventTime { get ; set ; }

        [ JsonPropertyName ( "properties" ) ]
        public Dictionary < string , JsonElement > Properties { get ; set ; }

        [ JsonPropertyName ( "level" ) ]
        [ JsonConverter ( typeof ( LogLevelConverter ) ) ]
        public LogLevel Level { get ; set ; }

    }

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
            [ NotNull ] Utf8JsonWriter        writer
          , [ NotNull ] LogLevel              value
          , JsonSerializerOptions options
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

    public class EventTimeConverter : JsonConverter<DateTime>
    {
        #region Overrides of JsonConverter<DateTime>
        public override DateTime Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        ) => DateTime.Parse ( reader.GetString ( ) ) ;

        public override void Write (
            Utf8JsonWriter        writer
          , DateTime              value
          , JsonSerializerOptions options
        ) => writer.WriteStringValue ( value ) ;
        #endregion
    }
}