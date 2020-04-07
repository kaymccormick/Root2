using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Threading.Tasks;

namespace AnalysisAppLib
{
    public sealed class JsonPocoSyntaxConverter : JsonConverterFactory
    {
        private readonly Type _excludeType ;
        public JsonPocoSyntaxConverter ( Type excludeType ) { _excludeType = excludeType ; }
        #region Overrides of JsonConverter
        public JsonPocoSyntaxConverter ( ) {
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != _excludeType && typeof(FindLogUsages.PocoCSharpSyntaxNode).IsAssignableFrom(typeToConvert);
        }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            var ty = typeof ( SyntaxConverter <> ).MakeGenericType ( typeToConvert ) ;
            return ( JsonConverter ) Activator.CreateInstance ( ty ) ;
        }

        private class SyntaxConverter < T > : JsonConverter<T>
        {
            #region Overrides of JsonConverter<T>
            public override T Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { throw new InvalidOperationException(); }

            public override void Write (
                Utf8JsonWriter        writer
              , T                     value
              , JsonSerializerOptions options
            )
            {
                var jsonSerializerOptions = new JsonSerializerOptions() ;
                jsonSerializerOptions.Converters.Add (new JsonPocoSyntaxConverter ( value.GetType (  ) )  );
                JsonSerializer.Serialize(writer, value, value.GetType (  ), jsonSerializerOptions);
            }
            #endregion
        }
        #endregion
    }
}
