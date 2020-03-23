#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonSyntaxTokenConverter.cs
// 
// 2020-03-17-1:50 PM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using Microsoft.CodeAnalysis ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    // ReSharper disable once UnusedType.Global
    public class JsonSyntaxTokenConverter : JsonConverter < SyntaxToken >
    {
        #region Overrides of JsonConverter<SyntaxToken>
        public override SyntaxToken Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return default ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , SyntaxToken           value
          , JsonSerializerOptions options
        )
        {
            
        }
        #endregion
    }
}