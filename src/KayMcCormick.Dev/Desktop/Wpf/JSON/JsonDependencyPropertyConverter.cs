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
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    public sealed class JsonDependencyPropertyConverter : JsonConverter < DependencyProperty >
    {
        #region Overrides of JsonConverter<DependencyProperty>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override DependencyProperty Read (
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
            [ NotNull ] Utf8JsonWriter        writer
          , [ NotNull ] DependencyProperty    value
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