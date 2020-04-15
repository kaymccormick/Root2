using System ;
using System.Linq ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Serialization ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib.XmlDoc.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public class JsonElementCodeConverter
    {
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ExpressionSyntax ConvertJsonElementToCode ( JsonElement element )
        {
            ExpressionSyntax elementSyntax = null ;
            switch ( element.ValueKind )
            {
                case JsonValueKind.Undefined :
                    elementSyntax =
                        SyntaxFactory.LiteralExpression ( SyntaxKind.NullLiteralExpression ) ;
                    break ;
                case JsonValueKind.Object :
                    elementSyntax = AnonymousObject ( element ) ;
                    

                    break ;
                case JsonValueKind.Array :

                    var elems = element.EnumerateArray ( )
                                       .Select ( ConvertJsonElementToCode )
                                       .ToList ( ) ;

                    elementSyntax = SyntaxFactory.ImplicitArrayCreationExpression (
                                                                                   SyntaxFactory
                                                                                      .InitializerExpression (
                                                                                                              SyntaxKind
                                                                                                                 .ArrayInitializerExpression
                                                                                                            , new
                                                                                                                      SeparatedSyntaxList
                                                                                                                      < ExpressionSyntax
                                                                                                                      > ( )
                                                                                                                 .AddRange (
                                                                                                                            elems
                                                                                                                           )
                                                                                                             )
                                                                                  ) ;

                    break ;
                case JsonValueKind.String :
                    elementSyntax = SyntaxFactory.LiteralExpression (
                                                                     SyntaxKind
                                                                        .StringLiteralExpression
                                                                   , SyntaxFactory.Literal (
                                                                                            element
                                                                                               .GetString ( )
                                                                                           )
                                                                    ) ;
                    break ;
                case JsonValueKind.Number :
                    SyntaxToken literal = default ;
                    if ( element.TryGetInt32 ( out var val1 ) )
                    {
                        literal = SyntaxFactory.Literal ( val1 ) ;
                    }
                    else if ( element.TryGetUInt32 ( out var val2 ) )
                    {
                        literal = SyntaxFactory.Literal ( val2 ) ;
                    }
                    else if ( element.TryGetInt64 ( out var val3 ) )
                    {
                        literal = SyntaxFactory.Literal ( val3 ) ;
                    }

                    if ( literal != default )
                    {
                        elementSyntax = SyntaxFactory.LiteralExpression (
                                                                         SyntaxKind
                                                                            .NumericLiteralExpression
                                                                       , literal
                                                                        ) ;
                    }
                    else
                    {
                        throw new InvalidOperationException ( ) ;
                    }

                    break ;
                case JsonValueKind.True :
                    elementSyntax =
                        SyntaxFactory.LiteralExpression ( SyntaxKind.TrueLiteralExpression ) ;
                    break ;
                case JsonValueKind.False :
                    elementSyntax =
                        SyntaxFactory.LiteralExpression ( SyntaxKind.FalseLiteralExpression ) ;
                    break ;
                case JsonValueKind.Null :
                    elementSyntax =
                        SyntaxFactory.LiteralExpression ( SyntaxKind.NullLiteralExpression ) ;
                    break ;
                default : throw new ArgumentOutOfRangeException ( ) ;
            }

            return elementSyntax ;
        }

        private static ExpressionSyntax AnonymousObject ( JsonElement element )
        {
            ExpressionSyntax elementSyntax ;
            var anon = new SeparatedSyntaxList < AnonymousObjectMemberDeclaratorSyntax > ( ).AddRange (
                                                                                                       element
                                                                                                          .EnumerateObject ( )
                                                                                                          .Where (
                                                                                                                  p => p.Value
                                                                                                                        .ValueKind
                                                                                                                       != JsonValueKind
                                                                                                                          .Null
                                                                                                                 )
                                                                                                          .Select (
                                                                                                                   p
                                                                                                                       => SyntaxFactory
                                                                                                                          .AnonymousObjectMemberDeclarator (
                                                                                                                                                            SyntaxFactory
                                                                                                                                                               .NameEquals (
                                                                                                                                                                            p.Name
                                                                                                                                                                           )
                                                                                                                                                          , ConvertJsonElementToCode (
                                                                                                                                                                                      p.Value
                                                                                                                                                                                     )
                                                                                                                                                           )
                                                                                                                  )
                                                                                                      ) ;
            elementSyntax =
                SyntaxFactory.AnonymousObjectCreationExpression (
                                                                 new SeparatedSyntaxList <
                                                                     AnonymousObjectMemberDeclaratorSyntax
                                                                 > ( )
                                                                ) ;
            return elementSyntax ;
        }
    }

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