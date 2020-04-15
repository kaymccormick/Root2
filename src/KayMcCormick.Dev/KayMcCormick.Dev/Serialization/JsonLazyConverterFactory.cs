#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisControls
// JsonLazyConverterFactory.cs
// 
// 2020-03-29-11:13 AM
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
    /// 
    /// </summary>
    public class JsonLazyConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( [ NotNull ] Type typeToConvert )
        {
            if ( typeToConvert.IsGenericType
                 && typeToConvert.GetGenericTypeDefinition ( ) == typeof ( Lazy <> ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            var t = typeToConvert.GetGenericArguments ( )[ 0 ] ;
            var ctype = typeof ( InnerConverter <,> ).MakeGenericType ( typeToConvert, t) ;
            return ( JsonConverter ) Activator.CreateInstance ( ctype ) ;
        }

        private class InnerConverter<T, X> : JsonConverter <T> where T : Lazy<X>
        {
                #region Overrides of JsonConverter<T>
                public override T Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { return null; }

            public override void Write (
                Utf8JsonWriter        writer
              , T                     value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue ( "Lazy" ) ;
            }
            #endregion
        }
        #endregion
    }
}