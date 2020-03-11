using System ;
using System.Collections.Generic ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using NLog ;

namespace KayMcCormick.Dev
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance'
    public class LogEventInstance
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance'
    {
        [JsonPropertyName( "logger")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Logger'
        public string Logger { get ; set ; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Logger'
        [JsonPropertyName( "@mt")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.MessageTemplate'
        public string MessageTemplate { get ; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.MessageTemplate'
        [JsonPropertyName( "@t")]
        [JsonConverter( typeof(EventTimeConverter))]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.EventTime'
        public DateTime EventTime { get ; set ; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.EventTime'

        [ JsonPropertyName ( "properties" ) ]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Properties'
        public Dictionary < string , JsonElement > Properties { get ; set ; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Properties'

        [ JsonPropertyName ( "level" ) ]
        [ JsonConverter ( typeof ( LogLevelConverter ) ) ]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Level'
        public LogLevel Level { get ; set ; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LogEventInstance.Level'

    }
}