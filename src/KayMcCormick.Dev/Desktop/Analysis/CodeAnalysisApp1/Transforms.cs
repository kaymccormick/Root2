using System ;
using System.Linq ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace CodeAnalysisApp1
{
    /// <summary>
    /// Transforms for Code Analysis nodes.
    /// </summary>
    public static class Transforms
    {
        /// <summary>Transforms the expr.</summary>
        /// <param name="expressionSyntaxNode">The argument.</param>
        /// <returns></returns>
        public static object TransformExpr ( ExpressionSyntax expressionSyntaxNode )
        {
            if ( expressionSyntaxNode == null )
            {
                throw new ArgumentNullException ( nameof ( expressionSyntaxNode ) ) ;
            }

            try
            {
                switch ( expressionSyntaxNode )
                {
                    case MemberBindingExpressionSyntax binding :
                        return new
                               {
                                   Name = TransformSimpleNameSyntax ( binding.Name )
                                 , Op   = TransformOperatorToken ( binding.OperatorToken )
                               } ;
                    case ConditionalAccessExpressionSyntax cond :
                        return new
                               {
                                   Op          = TransformOperatorToken ( cond.OperatorToken )
                                 , Expression  = TransformExpr ( cond.Expression )
                                 , WhenNotNull = TransformExpr ( cond.WhenNotNull )
                               } ;
                    case LambdaExpressionSyntax l :
                        return new
                               {
                                   Parameters =
                                       ( l as ParenthesizedLambdaExpressionSyntax )
                                     ?.ParameterList.Parameters.Select ( TransformParameter )
                                      .ToList ( )
                                 , Block = l.Block?.Statements.Select ( TransformStatement )
                                            .ToList ( )
                                 , ExpressionBody = TransformExpr ( l.ExpressionBody )
                               } ;
                    case PredefinedTypeSyntax preDef :
                        return new { PredefinedTypeSyntax = TransformKeyword ( preDef.Keyword ) } ;
                    case InvocationExpressionSyntax invoc :
                        return new
                               {
                                   Expression = TransformExpr ( invoc.Expression )
                                 , Args = invoc.ArgumentList.Arguments
                                               .Select (
                                                        syntax => TransformExpr (
                                                                                 syntax.Expression
                                                                                )
                                                       )
                                               .ToList ( )
                               } ;
                    case BinaryExpressionSyntax bin :
                        return new
                               {
                                   Left  = TransformExpr ( bin.Left )
                                 , Op    = bin.OperatorToken.ValueText
                                 , Right = TransformExpr ( bin.Right )
                               } ;
                    case MemberAccessExpressionSyntax macc :
                        return new
                               {
                                   Expression = TransformExpr ( macc.Expression )
                                 , Operator   = macc.OperatorToken.ValueText
                                 , Name       = TransformExpr ( macc.Name )
                               } ;
                    case IdentifierNameSyntax ident :
                        return new { Identifier = ident.Identifier.ValueText } ;
                    case LiteralExpressionSyntax lit : return new { Literal = lit.Token.Value } ;
                    case InterpolatedStringExpressionSyntax i :
                        return i.Contents.Select ( TransformInterpolated ).ToList ( ) ;
                    default :
                        throw new NotImplementedException (
                                                           expressionSyntaxNode
                                                             ?.GetType ( )
                                                              .FullName
                                                          ) ;
                }
            }
            catch ( InvalidOperationException invOp )
            {
                return new { Exception = invOp , Argument = expressionSyntaxNode } ;
            }
        }

        /// <summary>
        /// Transform operator token.
        /// </summary>
        /// <param name="condOperatorToken"></param>
        /// <returns></returns>
        public static object TransformOperatorToken (in SyntaxToken condOperatorToken )
        {
            return condOperatorToken.Kind ( ).ToString ( ) ;
        }

        /// <summary>
        /// Transform <see cref="ParameterSyntax"/>
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object TransformParameter ( [ NotNull ] ParameterSyntax arg )
        {
            if ( arg == null )
            {
                throw new ArgumentNullException ( nameof ( arg ) ) ;
            }

            return new
                   {
                       Type       = TransformTypeSyntax ( arg.Type )
                     , Identifier = TransformIdentifier ( arg.Identifier )
                   } ;
        }

        /// <summary>
        /// Transform identifier.
        /// </summary>
        /// <param name="argIdentifier"></param>
        /// <returns></returns>
        public static object TransformIdentifier ( in SyntaxToken argIdentifier )
        {
            return argIdentifier.Value ;
        }

        /// <summary>
        /// Transform TypeSyntax
        /// </summary>
        /// <param name="argType"></param>
        /// <param name="dispatch"></param>
        /// <returns></returns>
        public static object TransformTypeSyntax ( TypeSyntax argType , bool dispatch = true )
        {
            if ( argType == null )
            {
                return null ;
            }

            switch ( argType )
            {
                case NameSyntax name : return TransformNameSyntax ( name ) ;
                default : return dispatch ? argType.ToString ( ) : null ;
            }
        }

        /// <summary>
        /// Transform NameSyntax
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object TransformNameSyntax ( NameSyntax name )
        {
            switch ( name )
            {
                case SimpleNameSyntax simple : return TransformSimpleNameSyntax ( simple ) ;
                default :                      return TransformTypeSyntax ( name , false ) ;
            }
        }

        /// <summary>
        /// Transform SimpleNameSyntax
        /// </summary>
        /// <param name="simple"></param>
        /// <returns></returns>
        public static object  TransformSimpleNameSyntax ( SimpleNameSyntax simple )
        {
            switch ( simple )
            {
                case GenericNameSyntax gen :   return TransformGenericNameSyntax ( gen ) ;
                case IdentifierNameSyntax id : return TransformIdentifierNameSyntax ( id ) ;
                default :
                    throw new NotImplementedException ( simple.GetType ( ).FullName ) ;
            }
        }

        /// <summary>
        /// Transform an IdentifierNameSyntax to an appropriate structure.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object TransformIdentifierNameSyntax ( IdentifierNameSyntax id )
        {
            return TransformIdentifier ( id.Identifier ) ;
        }

        /// <summary>
        /// Transform GenericNameSyntax
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        public static object TransformGenericNameSyntax ( GenericNameSyntax gen )
        {
            return new
                   {
                       Identifier = TransformIdentifier ( gen.Identifier )
                     , gen.IsUnboundGenericName
                     , TypeArgumentList = gen.TypeArgumentList.Arguments
                                             .Select ( TransformGenericNameTypeArgument )
                                             .ToList ( )
                   } ;
        }

        /// <summary>
        /// Transform TypeSyntax as a GenericNameTypeArgument.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object TransformGenericNameTypeArgument ( TypeSyntax arg )
        {
            return TransformTypeSyntax ( arg ) ;
        }

        /// <summary>
        /// Transform a statement (simply calls ToString())
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object TransformStatement ( StatementSyntax arg )
        {
            return arg.ToString ( ) ;
        }

        /// <summary>
        /// Transform keywords <see cref="SyntaxToken"/>
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static object TransformKeyword ( in SyntaxToken keyword )
        {
            return keyword.Kind ( ).ToString ( ) ;
        }

        /// <summary>
        /// Transform <see cref="InterpolatedStringContentSyntax"/>
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object TransformInterpolated ( InterpolatedStringContentSyntax arg )
        {
            switch ( arg )
            {
                case InterpolatedStringTextSyntax text : return text.TextToken.Value ;
                case InterpolationSyntax syntax :
                    return TransformExpr ( syntax.Expression ) ;
                default :
                    throw new NotImplementedException ( arg.GetType ( ).FullName ) ;
            }
        }

        /// <summary>
        /// Transform method symbol.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static object TransformMethodSymbol ( IMethodSymbol method )
        {
            return new { method.MethodKind , Parameters = method.Parameters.Select (TransformMethodParameter ).ToList() } ;
        }

        private static object TransformMethodParameter  (
            IParameterSymbol arg1
          , int              arg2
        )
        {
            return new
                   {
                       arg1.CustomModifiers
//                     , arg1.ExplicitDefaultValue
                     , arg1.HasExplicitDefaultValue
                     , arg1.IsOptional
                     , arg1.IsParams
                     , arg1.Name
                     , Type = TransformTypeSymbol ( arg1.Type )
                   } ;
        }

        private static object TransformTypeSymbol ( ITypeSymbol arg1Type )
        {
            return new { arg1Type.Name } ;
        }
    }
}