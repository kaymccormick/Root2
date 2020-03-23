#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonSolidColorBrushConverter.cs
// 
// 2020-03-20-4:00 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Media ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    public class JsonSolidColorBrushConverter : JsonConverter < SolidColorBrush >
    {
        #region Overrides of JsonConverter<SolidColorBrush>
        public override SolidColorBrush Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , SolidColorBrush       value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "Color" , value.Color.ToString ( ) ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}