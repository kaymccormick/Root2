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
using System.Text.Json.Serialization ;

namespace ProjLib
{
    public class LogEventInstance
    {
        [JsonPropertyName( "logger")]
        public string Logger { get ; set ; }
        [JsonPropertyName( "@mt")]
        public string MessageTemplate { get ; set; }
        [JsonPropertyName( "@t")]
        public DateTime EventTime { get ; set ; }

    }
}