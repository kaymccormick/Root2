#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// HashtableConverter.cs
// 
// 2020-03-20-3:58 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace ProjInterface
{
    public class HashtableConverter : JsonConverter < Hashtable >
    {
        #region Overrides of JsonConverter<Hashtable>
        public override Hashtable Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , Hashtable             value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            foreach ( var q in value.Keys )
            {
                writer.WritePropertyName ( q.ToString ( ) ) ;
                JsonSerializer.Serialize ( writer , value[ q ] , options ) ;
            }

            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}