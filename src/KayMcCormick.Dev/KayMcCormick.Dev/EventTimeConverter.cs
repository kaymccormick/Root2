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
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace KayMcCormick.Dev
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter'
    public class EventTimeConverter : JsonConverter<DateTime>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter'
    {
        #region Overrides of JsonConverter<DateTime>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)'
        public override DateTime Read (
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)'
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        ) => DateTime.Parse ( reader.GetString ( ) ) ;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter.Write(Utf8JsonWriter, DateTime, JsonSerializerOptions)'
        public override void Write (
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EventTimeConverter.Write(Utf8JsonWriter, DateTime, JsonSerializerOptions)'
            Utf8JsonWriter        writer
          , DateTime              value
          , JsonSerializerOptions options
        ) => writer.WriteStringValue ( value ) ;
        #endregion
    }
}