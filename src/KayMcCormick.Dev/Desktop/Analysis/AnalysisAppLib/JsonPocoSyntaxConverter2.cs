﻿#if POCO
#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// JsonPocoSyntaxConverter2.cs
// 
// 2020-04-05-2:22 PM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using PocoSyntax ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public sealed class JsonPocoSyntaxConverter2 : JsonConverterFactory
    {
        private readonly Type _excludeType ;

        /// <inheritdoc />
        public JsonPocoSyntaxConverter2 ( Type excludeType ) { _excludeType = excludeType ; }
        #region Overrides of JsonConverter
        /// <inheritdoc />
        public JsonPocoSyntaxConverter2 ( ) {
        }

        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != _excludeType && typeof(PocoCSharpSyntaxNode).IsAssignableFrom(typeToConvert);
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <inheritdoc />
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            var ty = typeof ( SyntaxConverter <> ).MakeGenericType ( typeToConvert ) ;
            return ( JsonConverter ) Activator.CreateInstance ( ty ) ;
        }

        private sealed class SyntaxConverter < T > : JsonConverter<T>
        {
            #region Overrides of JsonConverter<T>
            public override T Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                throw new AppInvalidOperationException ( ) ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , [ NotNull ] T                     value
              , JsonSerializerOptions options
            )
            {
                var jsonSerializerOptions = new JsonSerializerOptions() ;
                jsonSerializerOptions.Converters.Add (new JsonPocoSyntaxConverter2 ( value.GetType (  ) )  );
                JsonSerializer.Serialize(writer, value, value.GetType (  ), jsonSerializerOptions);
            }
            #endregion
        }
        #endregion
    }
}
#endif