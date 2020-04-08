﻿using System ;
using System.Linq ;
using System.Text.Json ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib.Serialization
{
    public class JsonElementCodeConverter
    {
        // ReSharper disable once UnusedMember.Global
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
                    var anon =
                        new SeparatedSyntaxList < AnonymousObjectMemberDeclaratorSyntax > ( ) ;
                    anon = anon.AddRange (
                                          element.EnumerateObject ( ).Where (p => p.Value.ValueKind != JsonValueKind.Null  )
                                                 .Select (
                                                          p => SyntaxFactory
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
                    elementSyntax = SyntaxFactory.AnonymousObjectCreationExpression ( anon ) ;
                    break ;
                case JsonValueKind.Array :
                    
                    var elems = element.EnumerateArray ( )
                                       .Select ( ConvertJsonElementToCode )
                                       .ToList ( ) ;

                    elementSyntax = SyntaxFactory.ArrayCreationExpression (
                                                                           SyntaxFactory.ArrayType (
                                                                                                    SyntaxFactory
                                                                                                       .PredefinedType (
                                                                                                                        SyntaxFactory
                                                                                                                           .Token (
                                                                                                                                   SyntaxKind
                                                                                                                                      .ObjectKeyword
                                                                                                                                  )
                                                                                                                       ), new SyntaxList < ArrayRankSpecifierSyntax > ().Add(SyntaxFactory.ArrayRankSpecifier(new SeparatedSyntaxList < ExpressionSyntax > ().Add (SyntaxFactory.OmittedArraySizeExpression()  )))
                                                                                                   )
                                                                         , SyntaxFactory
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
    }
}