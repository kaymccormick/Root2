#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonConverterResourceDictionary.cs
// 
// 2020-03-20-3:59 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    public class JsonConverterResourceDictionary : JsonConverter < ResourceDictionary >
    {
        #region Overrides of JsonConverter<ResourceDictionary>
        public override ResourceDictionary Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , ResourceDictionary    value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "Source" , value.Source?.ToString ( ) ?? "" ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}