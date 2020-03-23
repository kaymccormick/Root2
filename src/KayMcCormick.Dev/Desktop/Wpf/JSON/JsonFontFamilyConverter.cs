#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonFontFamilyConverter.cs
// 
// 2020-03-20-3:58 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Markup ;
using System.Windows.Media ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    public class JsonFontFamilyConverter : JsonConverter < FontFamily >
    {
        #region Overrides of JsonConverter<FontFamily>
        public override FontFamily Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , FontFamily            value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString (
                                "FamilyName"
                              , value.FamilyNames[ XmlLanguage.GetLanguage ( "en-US" ) ]
                               ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}