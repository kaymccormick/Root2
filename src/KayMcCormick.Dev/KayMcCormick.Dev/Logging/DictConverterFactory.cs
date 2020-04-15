#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// DictConverterFactory.cs
// 
// 2020-03-19-11:57 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public class DictConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeToConvert == typeof ( IDictionary < object , object > ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ NotNull ]
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new Inner ( ) ;
        }

        /// <summary>
        /// </summary>
        private class Inner : JsonConverter < IDictionary < object , object > >
        {
            #region Overrides of JsonConverter<IDictionary<object,object>>
            public override IDictionary < object , object > Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                IDictionary < object , object > dict = new Dictionary < object , object > ( ) ;
                while ( reader.Read ( ) )
                {
                    if ( reader.TokenType == JsonTokenType.EndObject )
                    {
                        return dict ;
                    }

                    if ( reader.TokenType != JsonTokenType.PropertyName )
                    {
                        throw new JsonException ( ) ;
                    }

                    var propertyName = reader.GetString ( ) ;
                    var value = JsonSerializer.Deserialize < object > ( ref reader , options ) ;
                    dict[ propertyName ?? throw new InvalidOperationException ( ) ] = value ;
                }

                return dict ;
            }

            public override void Write (
                Utf8JsonWriter                  writer
              , IDictionary < object , object > value
              , JsonSerializerOptions           options
            )
            {
                writer.WriteStartObject ( ) ;
                foreach ( var keyValuePair in value )
                {
                    writer.WritePropertyName ( keyValuePair.Key.ToString ( ) ) ;
                    JsonSerializer.Serialize (
                                              writer
                                            , keyValuePair.Value
                                            , keyValuePair.Value.GetType ( )
                                            , options
                                             ) ;
                }

                writer.WriteEndObject ( ) ;
            }
            #endregion
        }
        #endregion
    }
}