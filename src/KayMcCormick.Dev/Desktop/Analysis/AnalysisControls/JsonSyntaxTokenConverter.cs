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
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    [ UsedImplicitly ]
    public class JsonSyntaxTokenConverter : JsonConverter < SyntaxToken >
    {
        #region Overrides of JsonConverter<SyntaxToken>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override SyntaxToken Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
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