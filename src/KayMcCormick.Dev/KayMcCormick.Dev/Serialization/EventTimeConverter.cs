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

namespace KayMcCormick.Dev.Serialization
{

    /// <summary>
    /// Basic DAteTime converter that supports broken time parsing.
    /// </summary>
    public class EventTimeConverter : JsonConverter<DateTime>

    {
        #region Overrides of JsonConverter<DateTime>

        /// <summary>
        /// REad method
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read (

            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        ) => DateTime.Parse ( reader.GetString ( ) ) ;


        /// <summary>
        /// Write method
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (

            Utf8JsonWriter        writer
          , DateTime              value
          , JsonSerializerOptions options
        ) => writer.WriteStringValue ( value ) ;
        #endregion
    }
}