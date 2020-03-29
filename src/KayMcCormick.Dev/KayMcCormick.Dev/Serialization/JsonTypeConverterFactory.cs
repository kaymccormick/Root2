using System ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// </summary>
    public class JsonTypeConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( Type ).IsAssignableFrom ( typeToConvert ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            if ( typeof ( TypeInfo ).IsAssignableFrom ( typeToConvert ) )
            {
                return new JsonTypeInfoConverter ( ) ;
            }

            return new JsonTypeConverter ( ) ;
        }
        #endregion
    }
}