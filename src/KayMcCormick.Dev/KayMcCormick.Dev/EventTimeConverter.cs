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