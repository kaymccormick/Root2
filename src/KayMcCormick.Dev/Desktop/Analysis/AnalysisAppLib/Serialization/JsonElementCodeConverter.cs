using System ;
using System.Linq ;
using System.Text.Json ;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib.Serialization
{
    /// <summary>
    /// </summary>
    [ UsedImplicitly ]
    public class JsonElementCodeConverter
    {
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [ NotNull ]
        public static ExpressionSyntax ConvertJsonElementToCode ( JsonElement element )
        {
            ExpressionSyntax elementSyntax ;
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
                        throw new AppInvalidOperationException ( ) ;
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

        [ NotNull ]
        private static ExpressionSyntax AnonymousObject ( JsonElement element )
        {
            // ReSharper disable once UnusedVariable
            var anon =
                new SeparatedSyntaxList < AnonymousObjectMemberDeclaratorSyntax > ( ).AddRange (
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
            ExpressionSyntax elementSyntax = SyntaxFactory.AnonymousObjectCreationExpression (
                                                                                              // ReSharper disable once RedundantArgumentDefaultValue
                                                                                              new
                                                                                                  SeparatedSyntaxList
                                                                                                  < AnonymousObjectMemberDeclaratorSyntax
                                                                                                  > ( )
                                                                                             ) ;
            return elementSyntax ;
        }
    }
}