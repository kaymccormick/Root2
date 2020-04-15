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
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    public class JsonSolidColorBrushConverter : JsonConverter < SolidColorBrush >
    {
        #region Overrides of JsonConverter<SolidColorBrush>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override SolidColorBrush Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
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