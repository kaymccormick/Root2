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
#if !NETSTANDARD2_0
using System.Text.Json ;
using System.Text.Json.Serialization ;
#endif
using JetBrains.Annotations ;
using NLog ;

namespace ProjLib
{
    public class LogEventInstance
    {
        #if !NETSTANDARD2_0
        [JsonPropertyName( "logger")]
#endif
        public string Logger { get ; set ; }
#if !NETSTANDARD2_0
        [JsonPropertyName( "@mt")]
#endif
        public string MessageTemplate { get ; set; }

#if !NETSTANDARD2_0
        [JsonPropertyName( "@t")]
        [JsonConverter(typeof(EventTimeConverter))]
#endif
        public DateTime EventTime { get ; set ; }
#if !NETSTANDARD2_0
        [ JsonPropertyName ( "properties" ) ]
        public Dictionary < string , JsonElement > Properties { get ; set ; }
#else
        public Dictionary<string, object> Properties { get; set; }
#endif
#if !NETSTANDARD2_0
        [ JsonPropertyName ( "level" ) ]
        [ JsonConverter ( typeof ( LogLevelConverter ) ) ]
#endif
        public LogLevel Level { get ; set ; }

    }
    #if !NETSTANDARD2_0
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
#endif
}
