#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonDependencyPropertyConverter.cs
// 
// 2020-03-20-3:58 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;

namespace ProjInterface
{
    public class JsonDependencyPropertyConverter : JsonConverter < DependencyProperty >
    {
        #region Overrides of JsonConverter<DependencyProperty>
        public override DependencyProperty Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , DependencyProperty    value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "DependencyPropertyName" , value.Name ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}