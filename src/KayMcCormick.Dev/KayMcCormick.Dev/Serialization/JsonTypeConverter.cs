#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// JsonTypeConverter.cs
// 
// 2020-03-19-11:57 PM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// </summary>
    public sealed class JsonTypeConverter : JsonConverter < Type >
    {
        #region Overrides of JsonConverter<Type>
        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override Type Read (
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
            [ JetBrains.Annotations.NotNull ] Utf8JsonWriter writer
          , [ JetBrains.Annotations.NotNull ] Type           value
          , JsonSerializerOptions      options
        )
        {
            if ( writer == null )
            {
                throw new ArgumentNullException ( nameof ( writer ) ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStartObject ( ) ;
            writer.WriteString ( "FullName" ,              value.FullName ) ;
            writer.WriteString ( "AssemblyQualifiedName" , value.AssemblyQualifiedName ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}