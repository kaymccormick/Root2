#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// JsonSymbolConverterFactory.cs
// 
// 2020-04-15-1:05 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using KayMcCormick.Dev.Serialization ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSymbolConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( ISymbol ).IsAssignableFrom ( typeToConvert ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new Converter (options ) ;
        }

        private class Converter : JsonConverter<ISymbol>
        {
            private JsonSerializerOptions options;

            public Converter(JsonSerializerOptions options)
            {
                this.options = options;
            }

            #region Overrides of JsonConverter<ISymbol>
            public override ISymbol Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { return null ; }

            public override void Write (
                Utf8JsonWriter        writer
              , ISymbol               value
              , JsonSerializerOptions options
            )
            {
                ConverterUtil.WritePreamble(this, writer, value, options);
                writer.WriteStartObject();
                writer.WriteNumber("Kind", (int)value.Kind);
                writer.WriteStartArray("DisplayParts");
                foreach ( var dp in value.ToDisplayParts ( ) )
                {
                    writer.WriteStringValue( dp.ToString (  )  );
                    writer.WriteNumberValue ( (int)dp.Kind );
                    writer.WriteStringValue(dp.Symbol.MetadataName);
                    if ( dp.Symbol?.ContainingAssembly?.Identity != null )
                    {
                        writer.WriteStringValue (
                                                 dp.Symbol.ContainingAssembly.Identity
                                                   .GetDisplayName ( )
                                                ) ;
                    }
                }
                writer.WriteEndArray();
                writer.WriteEndObject ( ) ;
                ConverterUtil.WriteTerminal(writer);
            }
            #endregion
        }

        #endregion
    }
}