#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonBrushConverter.cs
// 
// 2020-03-20-2:55 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Markup ;
using System.Windows.Media ;

namespace ProjInterface.JSON
{
    public class JsonBrushConverter : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( Brush ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonBrushConverter1 ( typeToConvert , options ) ;
        }

        public class JsonBrushConverter1 : JsonConverter < Brush >
        {
            public JsonBrushConverter1 ( Type typeToConvert , JsonSerializerOptions options ) { }
            #region Overrides of JsonConverter<Brush>
            public override Brush Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , Brush                 value
              , JsonSerializerOptions options
            )
            {
                var xaml = XamlWriter.Save ( value ) ;
                writer.WriteStartObject ( ) ;
                writer.WriteString ( "Xaml" , xaml ) ;
                writer.WriteEndObject ( ) ;
            }
            #endregion
        }
        #endregion
    }
}