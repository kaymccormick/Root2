using System ;
using System.Collections.Generic ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using NLog ;

namespace KayMcCormick.Dev
{
    public class LogEventInstance
    {
        [JsonPropertyName( "logger")]
        public string Logger { get ; set ; }
        [JsonPropertyName( "@mt")]
        public string MessageTemplate { get ; set; }
        [JsonPropertyName( "@t")]
        [JsonConverter( typeof(EventTimeConverter))]
        public DateTime EventTime { get ; set ; }

        [ JsonPropertyName ( "properties" ) ]
        public Dictionary < string , JsonElement > Properties { get ; set ; }

        [ JsonPropertyName ( "level" ) ]
        [ JsonConverter ( typeof ( LogLevelConverter ) ) ]
        public LogLevel Level { get ; set ; }

    }
}