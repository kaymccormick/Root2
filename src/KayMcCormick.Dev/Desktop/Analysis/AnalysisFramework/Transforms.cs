using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
{
    class TransformNode
    {
        private string type ;

        public int RawKind { get ; set ; }
        public string Kind { get ; set ; }
        public IEnumerable<string> Tokens { get ; set ; }
        public string StringRepr { get ; set ; }
        public string Type { get { return type ; } set { type = value ; } }
    }
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
                return null ;
            }

            try
            {
                switch ( expressionSyntaxNode )
                {
                    case ThisExpressionSyntax thise :
                        return new TransformNode
                               {
                                   RawKind = thise.RawKind
                                 , Kind    = thise.Kind ( ).ToString ( )
                                   , Type = thise.GetType ().Name
                                 , Tokens = thise.ChildTokens ( )
                                                 .Select ( token => token.ToString ( ) )
                                                 .ToList ( )
                               } ;
                    case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
                    case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax : break ;
                    case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax : break ;
                    case ArrayTypeSyntax arrayTypeSyntax : break ;
                    case AssignmentExpressionSyntax assignmentExpressionSyntax : break ;
                    case AwaitExpressionSyntax awaitExpressionSyntax : break ;
                    case BaseExpressionSyntax baseExpressionSyntax : break ;
                    case CastExpressionSyntax castExpressionSyntax : return new { } ;
                    case CheckedExpressionSyntax checkedExpressionSyntax : break ;
                    case DeclarationExpressionSyntax declarationExpressionSyntax : break ;
                    case DefaultExpressionSyntax defaultExpressionSyntax : break ;
                    case ElementAccessExpressionSyntax elementAccessExpressionSyntax : break ;
                    case ElementBindingExpressionSyntax elementBindingExpressionSyntax : break ;
                    case GenericNameSyntax genericNameSyntax : break ;
                    case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax : break ;
                    case ImplicitElementAccessSyntax implicitElementAccessSyntax : break ;
                    case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax : break ;
                    case InitializerExpressionSyntax initializerExpressionSyntax : break ;
                    case MakeRefExpressionSyntax makeRefExpressionSyntax : break ;
                    case NullableTypeSyntax nullableTypeSyntax : break ;
                    case ObjectCreationExpressionSyntax objectCreationExpressionSyntax:
                        return new { } ;
                    case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax : break ;
                    case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax : break ;
                    case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax : break ;
                    case PointerTypeSyntax pointerTypeSyntax : break ;
                    case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax : break ;
                    case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax : break ;
                    case QualifiedNameSyntax qualifiedNameSyntax : break ;
                    case QueryExpressionSyntax queryExpressionSyntax : break ;
                    case RangeExpressionSyntax rangeExpressionSyntax : break ;
                    case RefExpressionSyntax refExpressionSyntax : break ;
                    case RefTypeExpressionSyntax refTypeExpressionSyntax : break ;
                    case RefTypeSyntax refTypeSyntax : break ;
                    case RefValueExpressionSyntax refValueExpressionSyntax : break ;
                    case SizeOfExpressionSyntax sizeOfExpressionSyntax : break ;
                    case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax : break ;
                    case SwitchExpressionSyntax switchExpressionSyntax : break ;
                    case ThrowExpressionSyntax throwExpressionSyntax : break ;
                    case TupleExpressionSyntax tupleExpressionSyntax : break ;
                    case TupleTypeSyntax tupleTypeSyntax : break ;
                    case TypeOfExpressionSyntax typeOfExpressionSyntax : return new
                                                                                {
                                                                                    typeOfExpressionSyntax.RawKind
                                                                                   ,
                                                                                    Kind = typeOfExpressionSyntax.Kind().ToString()
                                                                                } ;
                    case ArrayCreationExpressionSyntax ac :
                        return new
                               {
                                   ac.RawKind
                                 , Kind     = ac.Kind ( ).ToString()
                                 , InitExpr = ac.Initializer.Expressions.Select ( TransformExpr )
                                 , ac.Type.ElementType
                                 , RankSpec = ac.Type.RankSpecifiers.Select (
                                                                             syntax => new
                                                                                       {
                                                                                           syntax
                                                                                              .RawKind
                                                                                         , syntax
                                                                                              .Rank
                                                                                         , Sizes =
                                                                                               syntax
                                                                                                  .Sizes
                                                                                                  .Select (
                                                                                                           TransformExpr
                                                                                                          )
                                                                                       }
                                                                            )
                               } ;
                    case MemberBindingExpressionSyntax binding :
                        return new
                               {
                                   binding.RawKind
                                 , Kind = binding.Kind ( ).ToString()
                                 , Name = TransformSimpleNameSyntax ( binding.Name )
                                 , Op   = TransformOperatorToken ( binding.OperatorToken )
                               } ;
                    case ConditionalAccessExpressionSyntax cond :
                        return new
                               {
                                   cond.RawKind
                                 , Kind        = cond.Kind ( ).ToString()
                                 , Op          = TransformOperatorToken ( cond.OperatorToken )
                                 , Expression  = TransformExpr ( cond.Expression )
                                 , WhenNotNull = TransformExpr ( cond.WhenNotNull )
                               } ;
                    case SimpleLambdaExpressionSyntax l:
                        return new
                               {
                                   l.RawKind
                                  ,
                                   Kind = l.Kind().ToString()
                                  ,
                                   Parameter = TransformParameter (  l.Parameter)
                                  ,
                                   Block = l.Block?.Statements.Select(TransformStatement)
                                            .ToList()
                                  ,
                                   ExpressionBody = TransformExpr(l.ExpressionBody)
                               };
                    case ParenthesizedLambdaExpressionSyntax pl :
                        return new
                               {
                                   pl.RawKind
                                  ,
                                   Kind = pl.Kind().ToString()
                                  ,
                                   Parameters =
                                       pl.ParameterList.Parameters.Select(TransformParameter)
                                          .ToList()
                                  ,
                                   Block = pl.Block?.Statements.Select(TransformStatement)
                                            .ToList()
                                  ,
                                   ExpressionBody = TransformExpr(pl.ExpressionBody)
                               };
                    case InstanceExpressionSyntax instanceExpressionSyntax : break ;
                    
                    case AnonymousFunctionExpressionSyntax anonymousFunctionExpressionSyntax : break ;
                    
                    case PredefinedTypeSyntax preDef :
                        return new
                               {
                                   preDef.RawKind
                                 , Kind                 = preDef.Kind ( ).ToString()
                                 , PredefinedTypeSyntax = TransformKeyword ( preDef.Keyword )
                               } ;
                    case InvocationExpressionSyntax invoc :
                        return new
                               {
                                   invoc.RawKind
                                 , Kind       = invoc.Kind ( ).ToString()
                                 , Expression = TransformExpr ( invoc.Expression )
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
                                   bin.RawKind
                                 , Kind  = bin.Kind ( ).ToString()
                                 , Left  = TransformExpr ( bin.Left )
                                 , Op    = bin.OperatorToken.ValueText
                                 , Right = TransformExpr ( bin.Right )
                               } ;
                    case MemberAccessExpressionSyntax macc :
                        return new
                               {
                                   macc.RawKind
                                 , Kind       = macc.Kind ( ).ToString()
                                   , Type = macc.GetType (  ).Name
                                 , Expression = TransformExpr ( macc.Expression )
                                 , Operator   = macc.OperatorToken.ValueText
                                 , Name       = TransformExpr ( macc.Name )
                                   ,
                                    Tokens = macc.DescendantTokens().Select(token => token.ToString()),
                                    FullString = macc.ToFullString(),
                        } ;
                    
                    case LiteralExpressionSyntax lit :
                        return new
                               {
                                   lit.RawKind , Kind = lit.Kind ( ).ToString() , Literal = lit.Token.Value
                               } ;
                    case InterpolatedStringExpressionSyntax i :
                        return new
                               {
                                   i.RawKind
                                 , Kind     = i.Kind ( ).ToString()
                                 , Contents = i.Contents.Select ( TransformInterpolated ).ToList ( ),
                                   Tokens = i.ChildTokens().Select(token => token.ToString())
                        } ;
                    case ConditionalExpressionSyntax cx :
                        return new
                               {
                                   cx.RawKind
                                 , Kind      = cx.Kind ( ).ToString()
                                 , Condition = TransformExpr ( cx.Condition)
                                 , WhenTrue  = TransformExpr ( cx.WhenTrue )
                                 , WhenFalse = TransformExpr ( cx.WhenFalse )
                               } ;
                    case IsPatternExpressionSyntax isPattern :
                        return new
                               {
                                   isPattern.RawKind
                                 , Kind       = isPattern.Kind ( ).ToString()
                                 , Expression = TransformExpr ( isPattern.Expression )
                                 , Pattern    = TransformPatternSyntax ( isPattern.Pattern )
                               } ;
                    case IdentifierNameSyntax ident:
                        return new
                               {
                                   ident.RawKind
                                  ,
                                   Kind = ident.Kind().ToString()
                                  ,
                                   Identifier = ident.Identifier.ValueText
                                  ,
                                   Tokens = ident.ChildTokens().Select(token => token.ToString())
                               };
                    case SimpleNameSyntax simpleNameSyntax: break;
                    default :
                        break ;
                    case NameSyntax nameSyntax: break;
                    case TypeSyntax typeSyntax: break;

                }

                throw new UnsupportedExpressionTypeSyntax (
                                                           $"Unsupported mode type {expressionSyntaxNode?.GetType ( ).FullName} at line {expressionSyntaxNode.GetLocation().GetMappedLineSpan().StartLinePosition.Line +1} {expressionSyntaxNode.GetLocation().ToString (  )}"
                                                          ) ;
            }
            
            catch ( InvalidOperationException invOp )
            {
                return new { Exception = invOp , Argument = expressionSyntaxNode } ;
            }
        }

        private static object TransformPatternSyntax ( PatternSyntax isPatternPattern )
        {
            switch ( isPatternPattern )
            {
                case ConstantPatternSyntax constant :
                    return new
                           {
                               constant.RawKind
                             , Kind       = constant.Kind ( ).ToString()
                             , Expression = TransformExpr ( constant.Expression )
                           } ;
                case DeclarationPatternSyntax declarationPatternSyntax :
                    Debug.Assert ( declarationPatternSyntax.Type != null ) ;
                    return new
                           {
                               declarationPatternSyntax.RawKind
                             , Kind = declarationPatternSyntax.Kind ( ).ToString()
                             , Type = TransformTypeSyntax ( declarationPatternSyntax.Type )
                             , Designation =
                                   TransformVariableDesignation (
                                                                 declarationPatternSyntax
                                                                    .Designation
                                                                )
                           } ;
                case DiscardPatternSyntax discardPatternSyntax :
                    return new
                           {
                               discardPatternSyntax.RawKind , Kind = discardPatternSyntax.Kind ( ).ToString() ,
                           } ;
                case RecursivePatternSyntax recursivePatternSyntax :
                    return new
                           {
                               recursivePatternSyntax.RawKind
                             , Kind = recursivePatternSyntax.Kind ( ).ToString()
                             , Designation =
                                   TransformVariableDesignation (
                                                                 recursivePatternSyntax.Designation
                                                                )
                             , PositionalPatternClause =
                                   TransformPositionalPatternClauseSyntax (
                                                                           recursivePatternSyntax
                                                                              .PositionalPatternClause
                                                                          )
                             , PropertyPatternClause =
                                   TransformPropertyPatternClauseSyntax (
                                                                         recursivePatternSyntax
                                                                            .PropertyPatternClause
                                                                        )
                             , Type = TransformTypeSyntax ( recursivePatternSyntax.Type )
                           } ;
                case VarPatternSyntax varPatternSyntax :
                    return new
                           {
                               varPatternSyntax.RawKind
                             , Kind = varPatternSyntax.Kind ( ).ToString()
                             , Designation =
                                   TransformVariableDesignation ( varPatternSyntax.Designation )
                           } ;
            }

            return new UnsupportedExpressionTypeSyntax ( isPatternPattern.Kind ( ).ToString().ToString ( ) ) ;
        }

        private static object TransformPositionalPatternClauseSyntax (
            PositionalPatternClauseSyntax positionalPatternClause
        )
        {
            return new
                   {
                       positionalPatternClause.RawKind
                     , Kind = positionalPatternClause.Kind ( ).ToString()
                     , Subpatterns =
                           positionalPatternClause.Subpatterns.Select ( TransformSubpatternSyntax )
                   } ;
        }

        private static object TransformPropertyPatternClauseSyntax (
            PropertyPatternClauseSyntax propertyPatternClause
        )
        {
            return new
                   {
                       propertyPatternClause.RawKind
                     , Kind = propertyPatternClause.Kind ( ).ToString()
                     , Subpatterns =
                           propertyPatternClause.Subpatterns.Select ( TransformSubpatternSyntax )
                   } ;
        }

        private static object TransformSubpatternSyntax ( SubpatternSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind    = arg.Kind ( ).ToString()
                     , Pattern = TransformPatternSyntax ( arg.Pattern )
                   } ;
        }

        private static object TransformVariableDesignation ( VariableDesignationSyntax designation )
        {
            switch ( designation )
            {
                case DiscardDesignationSyntax discard :
                    return new { discard.RawKind , Kind = discard.Kind ( ).ToString() } ;
                case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax :
                    return new
                           {
                               parenthesizedVariableDesignationSyntax.RawKind
                             , Kind = parenthesizedVariableDesignationSyntax.Kind ( ).ToString()
                             , Variables =
                                   parenthesizedVariableDesignationSyntax.Variables.Select (
                                                                                            TransformVariableDesignation
                                                                                           )
                           } ;
                case SingleVariableDesignationSyntax singleVariableDesignationSyntax :
                    return new
                           {
                               singleVariableDesignationSyntax.RawKind
                             , Kind = singleVariableDesignationSyntax.Kind ( ).ToString()
                             , Identifier =
                                   TransformIdentifier (
                                                        singleVariableDesignationSyntax.Identifier
                                                       )
                           } ;
            }

            return new UnsupportedExpressionTypeSyntax ( designation.Kind ( ).ToString().ToString ( ) ) ;
        }

        /// <summary>
        /// Transform operator token.
        /// </summary>
        /// <param name="condOperatorToken"></param>
        /// <returns></returns>
        public static object TransformOperatorToken ( in SyntaxToken condOperatorToken )
        {
            return condOperatorToken.Kind ( ).ToString().ToString ( ) ;
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
                       arg.RawKind
                     , Kind       = arg.Kind ( ).ToString()
                     , Type       = TransformTypeSyntax ( arg.Type )
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
            return new
                   {
                       argIdentifier.RawKind , Kind = argIdentifier.Kind ( ).ToString() , argIdentifier.Value
                   } ;
        }

        /// <summary>
        /// Transform TypeSyntax
        /// </summary>
        /// <param name="argType"></param>
        /// <param name="dispatch"></param>
        /// <returns></returns>
        public static object TransformTypeSyntax (
            [ NotNull ] TypeSyntax argType
          , bool                   dispatch = true
        )
        {
            if ( argType == null )
            {
                return null ;
            }

            switch ( argType )
            {
                case ArrayTypeSyntax arrayTypeSyntax :                   break ;
                case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
                case QualifiedNameSyntax qualifiedNameSyntax :
                    return new
                           {
                               qualifiedNameSyntax.RawKind
                             , Kind  = qualifiedNameSyntax.Kind ( ).ToString()
                             , Left  = TransformNameSyntax ( qualifiedNameSyntax.Left )
                             , Right = TransformSimpleNameSyntax ( qualifiedNameSyntax.Right )
                           } ;
                case SimpleNameSyntax simpleNameSyntax :
                    return TransformSimpleNameSyntax ( simpleNameSyntax ) ;
                case NameSyntax name :
                    return TransformNameSyntax ( name ) ;
                case NullableTypeSyntax nullableTypeSyntax :               break ;
                case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax : break ;
                case PointerTypeSyntax pointerTypeSyntax :                 break ;
                case PredefinedTypeSyntax predefinedTypeSyntax :
                    return new
                           {
                               predefinedTypeSyntax.RawKind
                             , Kind    = predefinedTypeSyntax.Kind ( ).ToString().ToString ( )
                             , Keyword = TransformKeyword ( predefinedTypeSyntax.Keyword )
                           } ;
                
                case RefTypeSyntax refTypeSyntax :     break ;
                case TupleTypeSyntax tupleTypeSyntax : break ;
            }

            throw new UnsupportedExpressionTypeSyntax (
                                                       "Unsupported type "
                                                       + argType.GetType ( ).FullName
                                                      ) ;
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
        public static object TransformSimpleNameSyntax ( SimpleNameSyntax simple )
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
                       gen.RawKind
                     , Kind       = gen.Kind ( ).ToString()
                     , Identifier = TransformIdentifier ( gen.Identifier )
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
            return keyword.Kind ( ).ToString().ToString ( ) ;
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
            return new
                   {
                       method.MethodKind
                     , Parameters = method.Parameters.Select ( TransformMethodParameter ).ToList ( )
                   } ;
        }

        private static object TransformMethodParameter ( IParameterSymbol arg1 , int arg2 )
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

        public static object TransformTree ( SyntaxTree contextSyntaxTree )
        {
            var syntaxNode = contextSyntaxTree.GetRoot ( ) ;
            return TransformSyntaxNode ( syntaxNode ) ;
            
        }

        public static object TransformSyntaxNode ( SyntaxNode syntaxNode )
        {
            switch ( syntaxNode )
            {
                case CompilationUnitSyntax comp :
                    return new PojoCompilationUnit (
                                                    comp.Usings.Select ( TransformUsingDirectiveSyntax )
                                                        .ToList ( )
                                                  , comp.Externs.Select (
                                                                         TransformExternAliasDirectiveSyntax
                                                                        )
                                                        .ToList ( )
                                                  , comp.AttributeLists.Select (
                                                                                TransformAttributeListSybtax
                                                                               )
                                                        .ToList ( )
                                                  , comp.Members.Select ( TransformMemberDeclarationSyntax )
                                                        .ToList ( )
                                                   ) ;
                case ExpressionSyntax s: return TransformExpr ( s ) ;
                case UsingDirectiveSyntax u: return TransformUsingDirectiveSyntax ( u ) ;
                case ExpressionStatementSyntax x: return TransformExpressionStatementSyntax ( x ) ;
                case MemberDeclarationSyntax m: return TransformMemberDeclarationSyntax ( m ) ;
            }

            throw new UnsupportedExpressionTypeSyntax ( syntaxNode.Kind ( ).ToString ( ).ToString ( ) ) ;
        }

        private static object TransformExpressionStatementSyntax ( ExpressionStatementSyntax ex )
        {
            return new
                   {
                       ex.RawKind
                     , Kind       = ex.Kind ( ).ToString ( )
                     , Expression = TransformExpr ( ex.Expression )
                   } ;
        }

        private static object TransformMemberDeclarationSyntax ( MemberDeclarationSyntax arg )
        {
            switch ( arg )
            {
                // case EventFieldDeclarationSyntax eventFieldDeclarationSyntax :   break ;
                case FieldDeclarationSyntax fieldDeclarationSyntax :             return new { fieldDeclarationSyntax.RawKind };
                // case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax :     break ;
                // case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
                case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax :
                    break ;
                case DestructorDeclarationSyntax destructorDeclarationSyntax :     break ;
                // case MethodDeclarationSyntax methodDeclarationSyntax :             break ;
                case OperatorDeclarationSyntax operatorDeclarationSyntax :         break ;
                case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax :
                    return new
                           {
                               Statements =
                                   baseMethodDeclarationSyntax.Body.Statements.Select (
                                                                                       TransformStatement
                                                                                      )
                           } ;
                case EventDeclarationSyntax eventDeclarationSyntax :               break ;
                case IndexerDeclarationSyntax indexerDeclarationSyntax :           break ;
                case PropertyDeclarationSyntax propertyDeclarationSyntax :         break ;
                case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax : break ;
                case ClassDeclarationSyntax classDeclarationSyntax :
                    return TransformClassDeclarationSyntax ( classDeclarationSyntax ) ;
                case EnumDeclarationSyntax enumDeclarationSyntax :             break ;
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax :   break ;
                case StructDeclarationSyntax structDeclarationSyntax :         break ;
                case TypeDeclarationSyntax typeDeclarationSyntax :             break ;
                case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax :     break ;
                case DelegateDeclarationSyntax delegateDeclarationSyntax :     break ;
                case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax : break ;
                case GlobalStatementSyntax globalStatementSyntax :             break ;
                case IncompleteMemberSyntax incompleteMemberSyntax :           break ;
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax :
                    return new
                           {
                               namespaceDeclarationSyntax.RawKind
                             , Kind = namespaceDeclarationSyntax.Kind ( ).ToString()
                             , Members = namespaceDeclarationSyntax
                                        .Members.Select ( TransformMemberDeclarationSyntax )
                                        .ToList ( )
                           } ;
                default : break ;
            }

            throw new UnsupportedExpressionTypeSyntax ( arg.Kind ( ).ToString().ToString ( ) ) ;
            
        }

        private static object TransformClassDeclarationSyntax ( ClassDeclarationSyntax classDecl )
        {
            return new PojoClassDeclarationSyntax ( TransformSyntaxToken( classDecl.Identifier), classDecl.Members.Select(TransformMemberDeclarationSyntax).ToList()) ;
        }

        private static PocoSyntaxToken TransformSyntaxToken ( SyntaxToken token )
        {
            switch ( token.Kind() )
            {
                case SyntaxKind.None : break ;
                case SyntaxKind.List : break ;
                case SyntaxKind.TildeToken : break ;
                case SyntaxKind.ExclamationToken : break ;
                case SyntaxKind.DollarToken : break ;
                case SyntaxKind.PercentToken : break ;
                case SyntaxKind.CaretToken : break ;
                case SyntaxKind.AmpersandToken : break ;
                case SyntaxKind.AsteriskToken : break ;
                case SyntaxKind.OpenParenToken : break ;
                case SyntaxKind.CloseParenToken : break ;
                case SyntaxKind.MinusToken : break ;
                case SyntaxKind.PlusToken : break ;
                case SyntaxKind.EqualsToken : break ;
                case SyntaxKind.OpenBraceToken : break ;
                case SyntaxKind.CloseBraceToken : break ;
                case SyntaxKind.OpenBracketToken : break ;
                case SyntaxKind.CloseBracketToken : break ;
                case SyntaxKind.BarToken : break ;
                case SyntaxKind.BackslashToken : break ;
                case SyntaxKind.ColonToken : break ;
                case SyntaxKind.SemicolonToken : break ;
                case SyntaxKind.DoubleQuoteToken : break ;
                case SyntaxKind.SingleQuoteToken : break ;
                case SyntaxKind.LessThanToken : break ;
                case SyntaxKind.CommaToken : break ;
                case SyntaxKind.GreaterThanToken : break ;
                case SyntaxKind.DotToken : break ;
                case SyntaxKind.QuestionToken : break ;
                case SyntaxKind.HashToken : break ;
                case SyntaxKind.SlashToken : break ;
                case SyntaxKind.DotDotToken : break ;
                case SyntaxKind.SlashGreaterThanToken : break ;
                case SyntaxKind.LessThanSlashToken : break ;
                case SyntaxKind.XmlCommentStartToken : break ;
                case SyntaxKind.XmlCommentEndToken : break ;
                case SyntaxKind.XmlCDataStartToken : break ;
                case SyntaxKind.XmlCDataEndToken : break ;
                case SyntaxKind.XmlProcessingInstructionStartToken : break ;
                case SyntaxKind.XmlProcessingInstructionEndToken : break ;
                case SyntaxKind.BarBarToken : break ;
                case SyntaxKind.AmpersandAmpersandToken : break ;
                case SyntaxKind.MinusMinusToken : break ;
                case SyntaxKind.PlusPlusToken : break ;
                case SyntaxKind.ColonColonToken : break ;
                case SyntaxKind.QuestionQuestionToken : break ;
                case SyntaxKind.MinusGreaterThanToken : break ;
                case SyntaxKind.ExclamationEqualsToken : break ;
                case SyntaxKind.EqualsEqualsToken : break ;
                case SyntaxKind.EqualsGreaterThanToken : break ;
                case SyntaxKind.LessThanEqualsToken : break ;
                case SyntaxKind.LessThanLessThanToken : break ;
                case SyntaxKind.LessThanLessThanEqualsToken : break ;
                case SyntaxKind.GreaterThanEqualsToken : break ;
                case SyntaxKind.GreaterThanGreaterThanToken : break ;
                case SyntaxKind.GreaterThanGreaterThanEqualsToken : break ;
                case SyntaxKind.SlashEqualsToken : break ;
                case SyntaxKind.AsteriskEqualsToken : break ;
                case SyntaxKind.BarEqualsToken : break ;
                case SyntaxKind.AmpersandEqualsToken : break ;
                case SyntaxKind.PlusEqualsToken : break ;
                case SyntaxKind.MinusEqualsToken : break ;
                case SyntaxKind.CaretEqualsToken : break ;
                case SyntaxKind.PercentEqualsToken : break ;
                case SyntaxKind.QuestionQuestionEqualsToken : break ;
                case SyntaxKind.BoolKeyword : break ;
                case SyntaxKind.ByteKeyword : break ;
                case SyntaxKind.SByteKeyword : break ;
                case SyntaxKind.ShortKeyword : break ;
                case SyntaxKind.UShortKeyword : break ;
                case SyntaxKind.IntKeyword : break ;
                case SyntaxKind.UIntKeyword : break ;
                case SyntaxKind.LongKeyword : break ;
                case SyntaxKind.ULongKeyword : break ;
                case SyntaxKind.DoubleKeyword : break ;
                case SyntaxKind.FloatKeyword : break ;
                case SyntaxKind.DecimalKeyword : break ;
                case SyntaxKind.StringKeyword : break ;
                case SyntaxKind.CharKeyword : break ;
                case SyntaxKind.VoidKeyword : break ;
                case SyntaxKind.ObjectKeyword : break ;
                case SyntaxKind.TypeOfKeyword : break ;
                case SyntaxKind.SizeOfKeyword : break ;
                case SyntaxKind.NullKeyword : break ;
                case SyntaxKind.TrueKeyword : break ;
                case SyntaxKind.FalseKeyword : break ;
                case SyntaxKind.IfKeyword : break ;
                case SyntaxKind.ElseKeyword : break ;
                case SyntaxKind.WhileKeyword : break ;
                case SyntaxKind.ForKeyword : break ;
                case SyntaxKind.ForEachKeyword : break ;
                case SyntaxKind.DoKeyword : break ;
                case SyntaxKind.SwitchKeyword : break ;
                case SyntaxKind.CaseKeyword : break ;
                case SyntaxKind.DefaultKeyword : break ;
                case SyntaxKind.TryKeyword : break ;
                case SyntaxKind.CatchKeyword : break ;
                case SyntaxKind.FinallyKeyword : break ;
                case SyntaxKind.LockKeyword : break ;
                case SyntaxKind.GotoKeyword : break ;
                case SyntaxKind.BreakKeyword : break ;
                case SyntaxKind.ContinueKeyword : break ;
                case SyntaxKind.ReturnKeyword : break ;
                case SyntaxKind.ThrowKeyword : break ;
                case SyntaxKind.PublicKeyword : break ;
                case SyntaxKind.PrivateKeyword : break ;
                case SyntaxKind.InternalKeyword : break ;
                case SyntaxKind.ProtectedKeyword : break ;
                case SyntaxKind.StaticKeyword : break ;
                case SyntaxKind.ReadOnlyKeyword : break ;
                case SyntaxKind.SealedKeyword : break ;
                case SyntaxKind.ConstKeyword : break ;
                case SyntaxKind.FixedKeyword : break ;
                case SyntaxKind.StackAllocKeyword : break ;
                case SyntaxKind.VolatileKeyword : break ;
                case SyntaxKind.NewKeyword : break ;
                case SyntaxKind.OverrideKeyword : break ;
                case SyntaxKind.AbstractKeyword : break ;
                case SyntaxKind.VirtualKeyword : break ;
                case SyntaxKind.EventKeyword : break ;
                case SyntaxKind.ExternKeyword : break ;
                case SyntaxKind.RefKeyword : break ;
                case SyntaxKind.OutKeyword : break ;
                case SyntaxKind.InKeyword : break ;
                case SyntaxKind.IsKeyword : break ;
                case SyntaxKind.AsKeyword : break ;
                case SyntaxKind.ParamsKeyword : break ;
                case SyntaxKind.ArgListKeyword : break ;
                case SyntaxKind.MakeRefKeyword : break ;
                case SyntaxKind.RefTypeKeyword : break ;
                case SyntaxKind.RefValueKeyword : break ;
                case SyntaxKind.ThisKeyword : break ;
                case SyntaxKind.BaseKeyword : break ;
                case SyntaxKind.NamespaceKeyword : break ;
                case SyntaxKind.UsingKeyword : break ;
                case SyntaxKind.ClassKeyword : break ;
                case SyntaxKind.StructKeyword : break ;
                case SyntaxKind.InterfaceKeyword : break ;
                case SyntaxKind.EnumKeyword : break ;
                case SyntaxKind.DelegateKeyword : break ;
                case SyntaxKind.CheckedKeyword : break ;
                case SyntaxKind.UncheckedKeyword : break ;
                case SyntaxKind.UnsafeKeyword : break ;
                case SyntaxKind.OperatorKeyword : break ;
                case SyntaxKind.ExplicitKeyword : break ;
                case SyntaxKind.ImplicitKeyword : break ;
                case SyntaxKind.YieldKeyword : break ;
                case SyntaxKind.PartialKeyword : break ;
                case SyntaxKind.AliasKeyword : break ;
                case SyntaxKind.GlobalKeyword : break ;
                case SyntaxKind.AssemblyKeyword : break ;
                case SyntaxKind.ModuleKeyword : break ;
                case SyntaxKind.TypeKeyword : break ;
                case SyntaxKind.FieldKeyword : break ;
                case SyntaxKind.MethodKeyword : break ;
                case SyntaxKind.ParamKeyword : break ;
                case SyntaxKind.PropertyKeyword : break ;
                case SyntaxKind.TypeVarKeyword : break ;
                case SyntaxKind.GetKeyword : break ;
                case SyntaxKind.SetKeyword : break ;
                case SyntaxKind.AddKeyword : break ;
                case SyntaxKind.RemoveKeyword : break ;
                case SyntaxKind.WhereKeyword : break ;
                case SyntaxKind.FromKeyword : break ;
                case SyntaxKind.GroupKeyword : break ;
                case SyntaxKind.JoinKeyword : break ;
                case SyntaxKind.IntoKeyword : break ;
                case SyntaxKind.LetKeyword : break ;
                case SyntaxKind.ByKeyword : break ;
                case SyntaxKind.SelectKeyword : break ;
                case SyntaxKind.OrderByKeyword : break ;
                case SyntaxKind.OnKeyword : break ;
                case SyntaxKind.EqualsKeyword : break ;
                case SyntaxKind.AscendingKeyword : break ;
                case SyntaxKind.DescendingKeyword : break ;
                case SyntaxKind.NameOfKeyword : break ;
                case SyntaxKind.AsyncKeyword : break ;
                case SyntaxKind.AwaitKeyword : break ;
                case SyntaxKind.WhenKeyword : break ;
                case SyntaxKind.ElifKeyword : break ;
                case SyntaxKind.EndIfKeyword : break ;
                case SyntaxKind.RegionKeyword : break ;
                case SyntaxKind.EndRegionKeyword : break ;
                case SyntaxKind.DefineKeyword : break ;
                case SyntaxKind.UndefKeyword : break ;
                case SyntaxKind.WarningKeyword : break ;
                case SyntaxKind.ErrorKeyword : break ;
                case SyntaxKind.LineKeyword : break ;
                case SyntaxKind.PragmaKeyword : break ;
                case SyntaxKind.HiddenKeyword : break ;
                case SyntaxKind.ChecksumKeyword : break ;
                case SyntaxKind.DisableKeyword : break ;
                case SyntaxKind.RestoreKeyword : break ;
                case SyntaxKind.ReferenceKeyword : break ;
                case SyntaxKind.InterpolatedStringStartToken : break ;
                case SyntaxKind.InterpolatedStringEndToken : break ;
                case SyntaxKind.InterpolatedVerbatimStringStartToken : break ;
                case SyntaxKind.LoadKeyword : break ;
                case SyntaxKind.NullableKeyword : break ;
                case SyntaxKind.EnableKeyword : break ;
                case SyntaxKind.WarningsKeyword : break ;
                case SyntaxKind.AnnotationsKeyword : break ;
                case SyntaxKind.VarKeyword : break ;
                case SyntaxKind.UnderscoreToken : break ;
                case SyntaxKind.OmittedTypeArgumentToken : break ;
                case SyntaxKind.OmittedArraySizeExpressionToken : break ;
                case SyntaxKind.EndOfDirectiveToken : break ;
                case SyntaxKind.EndOfDocumentationCommentToken : break ;
                case SyntaxKind.EndOfFileToken : break ;
                case SyntaxKind.BadToken : break ;
                case SyntaxKind.IdentifierToken : return new PocoSyntaxToken ( token.Kind (  ).ToString(), token.RawKind, token.Value ) ;
                case SyntaxKind.NumericLiteralToken : break ;
                case SyntaxKind.CharacterLiteralToken : break ;
                case SyntaxKind.StringLiteralToken : break ;
                case SyntaxKind.XmlEntityLiteralToken : break ;
                case SyntaxKind.XmlTextLiteralToken : break ;
                case SyntaxKind.XmlTextLiteralNewLineToken : break ;
                case SyntaxKind.InterpolatedStringToken : break ;
                case SyntaxKind.InterpolatedStringTextToken : break ;
                case SyntaxKind.EndOfLineTrivia : break ;
                case SyntaxKind.WhitespaceTrivia : break ;
                case SyntaxKind.SingleLineCommentTrivia : break ;
                case SyntaxKind.MultiLineCommentTrivia : break ;
                case SyntaxKind.DocumentationCommentExteriorTrivia : break ;
                case SyntaxKind.SingleLineDocumentationCommentTrivia : break ;
                case SyntaxKind.MultiLineDocumentationCommentTrivia : break ;
                case SyntaxKind.DisabledTextTrivia : break ;
                case SyntaxKind.PreprocessingMessageTrivia : break ;
                case SyntaxKind.IfDirectiveTrivia : break ;
                case SyntaxKind.ElifDirectiveTrivia : break ;
                case SyntaxKind.ElseDirectiveTrivia : break ;
                case SyntaxKind.EndIfDirectiveTrivia : break ;
                case SyntaxKind.RegionDirectiveTrivia : break ;
                case SyntaxKind.EndRegionDirectiveTrivia : break ;
                case SyntaxKind.DefineDirectiveTrivia : break ;
                case SyntaxKind.UndefDirectiveTrivia : break ;
                case SyntaxKind.ErrorDirectiveTrivia : break ;
                case SyntaxKind.WarningDirectiveTrivia : break ;
                case SyntaxKind.LineDirectiveTrivia : break ;
                case SyntaxKind.PragmaWarningDirectiveTrivia : break ;
                case SyntaxKind.PragmaChecksumDirectiveTrivia : break ;
                case SyntaxKind.ReferenceDirectiveTrivia : break ;
                case SyntaxKind.BadDirectiveTrivia : break ;
                case SyntaxKind.SkippedTokensTrivia : break ;
                case SyntaxKind.ConflictMarkerTrivia : break ;
                case SyntaxKind.XmlElement : break ;
                case SyntaxKind.XmlElementStartTag : break ;
                case SyntaxKind.XmlElementEndTag : break ;
                case SyntaxKind.XmlEmptyElement : break ;
                case SyntaxKind.XmlTextAttribute : break ;
                case SyntaxKind.XmlCrefAttribute : break ;
                case SyntaxKind.XmlNameAttribute : break ;
                case SyntaxKind.XmlName : break ;
                case SyntaxKind.XmlPrefix : break ;
                case SyntaxKind.XmlText : break ;
                case SyntaxKind.XmlCDataSection : break ;
                case SyntaxKind.XmlComment : break ;
                case SyntaxKind.XmlProcessingInstruction : break ;
                case SyntaxKind.TypeCref : break ;
                case SyntaxKind.QualifiedCref : break ;
                case SyntaxKind.NameMemberCref : break ;
                case SyntaxKind.IndexerMemberCref : break ;
                case SyntaxKind.OperatorMemberCref : break ;
                case SyntaxKind.ConversionOperatorMemberCref : break ;
                case SyntaxKind.CrefParameterList : break ;
                case SyntaxKind.CrefBracketedParameterList : break ;
                case SyntaxKind.CrefParameter : break ;
                case SyntaxKind.IdentifierName : break ;
                case SyntaxKind.QualifiedName : break ;
                case SyntaxKind.GenericName : break ;
                case SyntaxKind.TypeArgumentList : break ;
                case SyntaxKind.AliasQualifiedName : break ;
                case SyntaxKind.PredefinedType : break ;
                case SyntaxKind.ArrayType : break ;
                case SyntaxKind.ArrayRankSpecifier : break ;
                case SyntaxKind.PointerType : break ;
                case SyntaxKind.NullableType : break ;
                case SyntaxKind.OmittedTypeArgument : break ;
                case SyntaxKind.ParenthesizedExpression : break ;
                case SyntaxKind.ConditionalExpression : break ;
                case SyntaxKind.InvocationExpression : break ;
                case SyntaxKind.ElementAccessExpression : break ;
                case SyntaxKind.ArgumentList : break ;
                case SyntaxKind.BracketedArgumentList : break ;
                case SyntaxKind.Argument : break ;
                case SyntaxKind.NameColon : break ;
                case SyntaxKind.CastExpression : break ;
                case SyntaxKind.AnonymousMethodExpression : break ;
                case SyntaxKind.SimpleLambdaExpression : break ;
                case SyntaxKind.ParenthesizedLambdaExpression : break ;
                case SyntaxKind.ObjectInitializerExpression : break ;
                case SyntaxKind.CollectionInitializerExpression : break ;
                case SyntaxKind.ArrayInitializerExpression : break ;
                case SyntaxKind.AnonymousObjectMemberDeclarator : break ;
                case SyntaxKind.ComplexElementInitializerExpression : break ;
                case SyntaxKind.ObjectCreationExpression : break ;
                case SyntaxKind.AnonymousObjectCreationExpression : break ;
                case SyntaxKind.ArrayCreationExpression : break ;
                case SyntaxKind.ImplicitArrayCreationExpression : break ;
                case SyntaxKind.StackAllocArrayCreationExpression : break ;
                case SyntaxKind.OmittedArraySizeExpression : break ;
                case SyntaxKind.InterpolatedStringExpression : break ;
                case SyntaxKind.ImplicitElementAccess : break ;
                case SyntaxKind.IsPatternExpression : break ;
                case SyntaxKind.RangeExpression : break ;
                case SyntaxKind.AddExpression : break ;
                case SyntaxKind.SubtractExpression : break ;
                case SyntaxKind.MultiplyExpression : break ;
                case SyntaxKind.DivideExpression : break ;
                case SyntaxKind.ModuloExpression : break ;
                case SyntaxKind.LeftShiftExpression : break ;
                case SyntaxKind.RightShiftExpression : break ;
                case SyntaxKind.LogicalOrExpression : break ;
                case SyntaxKind.LogicalAndExpression : break ;
                case SyntaxKind.BitwiseOrExpression : break ;
                case SyntaxKind.BitwiseAndExpression : break ;
                case SyntaxKind.ExclusiveOrExpression : break ;
                case SyntaxKind.EqualsExpression : break ;
                case SyntaxKind.NotEqualsExpression : break ;
                case SyntaxKind.LessThanExpression : break ;
                case SyntaxKind.LessThanOrEqualExpression : break ;
                case SyntaxKind.GreaterThanExpression : break ;
                case SyntaxKind.GreaterThanOrEqualExpression : break ;
                case SyntaxKind.IsExpression : break ;
                case SyntaxKind.AsExpression : break ;
                case SyntaxKind.CoalesceExpression : break ;
                case SyntaxKind.SimpleMemberAccessExpression : break ;
                case SyntaxKind.PointerMemberAccessExpression : break ;
                case SyntaxKind.ConditionalAccessExpression : break ;
                case SyntaxKind.MemberBindingExpression : break ;
                case SyntaxKind.ElementBindingExpression : break ;
                case SyntaxKind.SimpleAssignmentExpression : break ;
                case SyntaxKind.AddAssignmentExpression : break ;
                case SyntaxKind.SubtractAssignmentExpression : break ;
                case SyntaxKind.MultiplyAssignmentExpression : break ;
                case SyntaxKind.DivideAssignmentExpression : break ;
                case SyntaxKind.ModuloAssignmentExpression : break ;
                case SyntaxKind.AndAssignmentExpression : break ;
                case SyntaxKind.ExclusiveOrAssignmentExpression : break ;
                case SyntaxKind.OrAssignmentExpression : break ;
                case SyntaxKind.LeftShiftAssignmentExpression : break ;
                case SyntaxKind.RightShiftAssignmentExpression : break ;
                case SyntaxKind.CoalesceAssignmentExpression : break ;
                case SyntaxKind.UnaryPlusExpression : break ;
                case SyntaxKind.UnaryMinusExpression : break ;
                case SyntaxKind.BitwiseNotExpression : break ;
                case SyntaxKind.LogicalNotExpression : break ;
                case SyntaxKind.PreIncrementExpression : break ;
                case SyntaxKind.PreDecrementExpression : break ;
                case SyntaxKind.PointerIndirectionExpression : break ;
                case SyntaxKind.AddressOfExpression : break ;
                case SyntaxKind.PostIncrementExpression : break ;
                case SyntaxKind.PostDecrementExpression : break ;
                case SyntaxKind.AwaitExpression : break ;
                case SyntaxKind.IndexExpression : break ;
                case SyntaxKind.ThisExpression : break ;
                case SyntaxKind.BaseExpression : break ;
                case SyntaxKind.ArgListExpression : break ;
                case SyntaxKind.NumericLiteralExpression : break ;
                case SyntaxKind.StringLiteralExpression : break ;
                case SyntaxKind.CharacterLiteralExpression : break ;
                case SyntaxKind.TrueLiteralExpression : break ;
                case SyntaxKind.FalseLiteralExpression : break ;
                case SyntaxKind.NullLiteralExpression : break ;
                case SyntaxKind.DefaultLiteralExpression : break ;
                case SyntaxKind.TypeOfExpression : break ;
                case SyntaxKind.SizeOfExpression : break ;
                case SyntaxKind.CheckedExpression : break ;
                case SyntaxKind.UncheckedExpression : break ;
                case SyntaxKind.DefaultExpression : break ;
                case SyntaxKind.MakeRefExpression : break ;
                case SyntaxKind.RefValueExpression : break ;
                case SyntaxKind.RefTypeExpression : break ;
                case SyntaxKind.QueryExpression : break ;
                case SyntaxKind.QueryBody : break ;
                case SyntaxKind.FromClause : break ;
                case SyntaxKind.LetClause : break ;
                case SyntaxKind.JoinClause : break ;
                case SyntaxKind.JoinIntoClause : break ;
                case SyntaxKind.WhereClause : break ;
                case SyntaxKind.OrderByClause : break ;
                case SyntaxKind.AscendingOrdering : break ;
                case SyntaxKind.DescendingOrdering : break ;
                case SyntaxKind.SelectClause : break ;
                case SyntaxKind.GroupClause : break ;
                case SyntaxKind.QueryContinuation : break ;
                case SyntaxKind.Block : break ;
                case SyntaxKind.LocalDeclarationStatement : break ;
                case SyntaxKind.VariableDeclaration : break ;
                case SyntaxKind.VariableDeclarator : break ;
                case SyntaxKind.EqualsValueClause : break ;
                case SyntaxKind.ExpressionStatement : break ;
                case SyntaxKind.EmptyStatement : break ;
                case SyntaxKind.LabeledStatement : break ;
                case SyntaxKind.GotoStatement : break ;
                case SyntaxKind.GotoCaseStatement : break ;
                case SyntaxKind.GotoDefaultStatement : break ;
                case SyntaxKind.BreakStatement : break ;
                case SyntaxKind.ContinueStatement : break ;
                case SyntaxKind.ReturnStatement : break ;
                case SyntaxKind.YieldReturnStatement : break ;
                case SyntaxKind.YieldBreakStatement : break ;
                case SyntaxKind.ThrowStatement : break ;
                case SyntaxKind.WhileStatement : break ;
                case SyntaxKind.DoStatement : break ;
                case SyntaxKind.ForStatement : break ;
                case SyntaxKind.ForEachStatement : break ;
                case SyntaxKind.UsingStatement : break ;
                case SyntaxKind.FixedStatement : break ;
                case SyntaxKind.CheckedStatement : break ;
                case SyntaxKind.UncheckedStatement : break ;
                case SyntaxKind.UnsafeStatement : break ;
                case SyntaxKind.LockStatement : break ;
                case SyntaxKind.IfStatement : break ;
                case SyntaxKind.ElseClause : break ;
                case SyntaxKind.SwitchStatement : break ;
                case SyntaxKind.SwitchSection : break ;
                case SyntaxKind.CaseSwitchLabel : break ;
                case SyntaxKind.DefaultSwitchLabel : break ;
                case SyntaxKind.TryStatement : break ;
                case SyntaxKind.CatchClause : break ;
                case SyntaxKind.CatchDeclaration : break ;
                case SyntaxKind.CatchFilterClause : break ;
                case SyntaxKind.FinallyClause : break ;
                case SyntaxKind.LocalFunctionStatement : break ;
                case SyntaxKind.CompilationUnit : break ;
                case SyntaxKind.GlobalStatement : break ;
                case SyntaxKind.NamespaceDeclaration : break ;
                case SyntaxKind.UsingDirective : break ;
                case SyntaxKind.ExternAliasDirective : break ;
                case SyntaxKind.AttributeList : break ;
                case SyntaxKind.AttributeTargetSpecifier : break ;
                case SyntaxKind.Attribute : break ;
                case SyntaxKind.AttributeArgumentList : break ;
                case SyntaxKind.AttributeArgument : break ;
                case SyntaxKind.NameEquals : break ;
                case SyntaxKind.ClassDeclaration : break ;
                case SyntaxKind.StructDeclaration : break ;
                case SyntaxKind.InterfaceDeclaration : break ;
                case SyntaxKind.EnumDeclaration : break ;
                case SyntaxKind.DelegateDeclaration : break ;
                case SyntaxKind.BaseList : break ;
                case SyntaxKind.SimpleBaseType : break ;
                case SyntaxKind.TypeParameterConstraintClause : break ;
                case SyntaxKind.ConstructorConstraint : break ;
                case SyntaxKind.ClassConstraint : break ;
                case SyntaxKind.StructConstraint : break ;
                case SyntaxKind.TypeConstraint : break ;
                case SyntaxKind.ExplicitInterfaceSpecifier : break ;
                case SyntaxKind.EnumMemberDeclaration : break ;
                case SyntaxKind.FieldDeclaration : break ;
                case SyntaxKind.EventFieldDeclaration : break ;
                case SyntaxKind.MethodDeclaration : break ;
                case SyntaxKind.OperatorDeclaration : break ;
                case SyntaxKind.ConversionOperatorDeclaration : break ;
                case SyntaxKind.ConstructorDeclaration : break ;
                case SyntaxKind.BaseConstructorInitializer : break ;
                case SyntaxKind.ThisConstructorInitializer : break ;
                case SyntaxKind.DestructorDeclaration : break ;
                case SyntaxKind.PropertyDeclaration : break ;
                case SyntaxKind.EventDeclaration : break ;
                case SyntaxKind.IndexerDeclaration : break ;
                case SyntaxKind.AccessorList : break ;
                case SyntaxKind.GetAccessorDeclaration : break ;
                case SyntaxKind.SetAccessorDeclaration : break ;
                case SyntaxKind.AddAccessorDeclaration : break ;
                case SyntaxKind.RemoveAccessorDeclaration : break ;
                case SyntaxKind.UnknownAccessorDeclaration : break ;
                case SyntaxKind.ParameterList : break ;
                case SyntaxKind.BracketedParameterList : break ;
                case SyntaxKind.Parameter : break ;
                case SyntaxKind.TypeParameterList : break ;
                case SyntaxKind.TypeParameter : break ;
                case SyntaxKind.IncompleteMember : break ;
                case SyntaxKind.ArrowExpressionClause : break ;
                case SyntaxKind.Interpolation : break ;
                case SyntaxKind.InterpolatedStringText : break ;
                case SyntaxKind.InterpolationAlignmentClause : break ;
                case SyntaxKind.InterpolationFormatClause : break ;
                case SyntaxKind.ShebangDirectiveTrivia : break ;
                case SyntaxKind.LoadDirectiveTrivia : break ;
                case SyntaxKind.TupleType : break ;
                case SyntaxKind.TupleElement : break ;
                case SyntaxKind.TupleExpression : break ;
                case SyntaxKind.SingleVariableDesignation : break ;
                case SyntaxKind.ParenthesizedVariableDesignation : break ;
                case SyntaxKind.ForEachVariableStatement : break ;
                case SyntaxKind.DeclarationPattern : break ;
                case SyntaxKind.ConstantPattern : break ;
                case SyntaxKind.CasePatternSwitchLabel : break ;
                case SyntaxKind.WhenClause : break ;
                case SyntaxKind.DiscardDesignation : break ;
                case SyntaxKind.RecursivePattern : break ;
                case SyntaxKind.PropertyPatternClause : break ;
                case SyntaxKind.Subpattern : break ;
                case SyntaxKind.PositionalPatternClause : break ;
                case SyntaxKind.DiscardPattern : break ;
                case SyntaxKind.SwitchExpression : break ;
                case SyntaxKind.SwitchExpressionArm : break ;
                case SyntaxKind.VarPattern : break ;
                case SyntaxKind.DeclarationExpression : break ;
                case SyntaxKind.RefExpression : break ;
                case SyntaxKind.RefType : break ;
                case SyntaxKind.ThrowExpression : break ;
                case SyntaxKind.ImplicitStackAllocArrayCreationExpression : break ;
                case SyntaxKind.SuppressNullableWarningExpression : break ;
                case SyntaxKind.NullableDirectiveTrivia : break ;
                default : throw new ArgumentOutOfRangeException ( ) ;
            }
            throw new UnsupportedExpressionTypeSyntax(token.Kind (  ).ToString());
        }

        private static object TransformAttributeListSybtax ( AttributeListSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind       = arg.Kind ( ).ToString()
                     , Attributes = arg.Attributes.Select ( TransformAttributeSyntax )
                     , Target     = TransformAttributeTargetSpecifierSyntax ( arg.Target )
                   } ;
        }

        private static object TransformAttributeTargetSpecifierSyntax (
            AttributeTargetSpecifierSyntax argTarget
        )
        {
            return new
                   {
                       argTarget.RawKind
                     , Kind       = argTarget.Kind ( ).ToString()
                     , Identifier = TransformIdentifier ( argTarget.Identifier )
                   } ;
        }

        private static object TransformAttributeSyntax ( AttributeSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind = arg.Kind ( ).ToString()
                     , Name = TransformNameSyntax ( arg.Name )
                     , ArgumentList =
                           arg.ArgumentList.Arguments.Select ( TransformAttributeArgumentSyntax )
                   } ;
        }


        private static object TransformExternAliasDirectiveSyntax ( ExternAliasDirectiveSyntax arg )
        {
            return new { arg.RawKind , Kind = arg.Kind ( ).ToString() } ;
        }


        private static object TransformUsingDirectiveSyntax ( UsingDirectiveSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind  = arg.Kind ( ).ToString()
                     , Alias = arg.Alias != null ? TransformNameEqualsSyntax ( arg.Alias ) : null
                     , Name  = TransformNameSyntax ( arg.Name )
                      ,
                   } ;
        }

        private static object TransformNameEqualsSyntax ( [ NotNull ] NameEqualsSyntax argAlias )
        {
            if ( argAlias == null )
            {
                throw new ArgumentNullException ( nameof ( argAlias ) ) ;
            }

            return new
                   {
                       argAlias.RawKind
                     , Kind = argAlias.Kind ( ).ToString()
                     , Name = TransformIdentifierNameSyntax ( argAlias.Name )
                   } ;
        }


        private static object TransformAttributeArgumentSyntax ( AttributeArgumentSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind       = arg.Kind ( ).ToString()
                     , Expression = TransformExpr ( arg.Expression )
                   } ;
        }

        public class UnsupportedExpressionTypeSyntax : Exception
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
            public UnsupportedExpressionTypeSyntax ( ) { }

            /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
            /// <param name="message">The message that describes the error. </param>
            public UnsupportedExpressionTypeSyntax ( string message ) : base ( message ) { }

            /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
            /// <param name="message">The error message that explains the reason for the exception. </param>
            /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified. </param>
            public UnsupportedExpressionTypeSyntax ( string message , Exception innerException ) :
                base ( message , innerException )
            {
            }

            /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
            protected UnsupportedExpressionTypeSyntax (
                [ NotNull ] SerializationInfo info
              , StreamingContext              context
            ) : base ( info , context )
            {
            }
        }


        public class PojoClassDeclarationSyntax
        {
            public PocoSyntaxToken Identifier { get ; }

            public List < object > Members { get ; }

            public PojoClassDeclarationSyntax (
                in PocoSyntaxToken identifier
              , List < object >    members
            )
            {
                Identifier = identifier ;
                Members = members ;
            }
        }
    }

    public class PocoSyntaxToken
    {
        private string _kind;
        private int rawKind;
        private object value;

        public PocoSyntaxToken(string Kind, int rawKind, object value)
        {
            this.Kind = Kind;
            this.RawKind = rawKind;
            this.Value = value;
        }

        public string Kind { get => _kind ; set => _kind = value ; }

        public int RawKind { get => rawKind ; set => rawKind = value ; }

        public object Value { get => value ; set => this.value = value ; }
    }

    public class PojoCompilationUnit
    {
        public List < object > Usings { get ; }

        public List < object > ExternAliases { get ; }

        public List < object > AttributeLists { get ; }

        public List < object > Members { get ; }

        public PojoCompilationUnit (
            List < object > Usings
          , List < object > ExternAliases
          , List < object > AttributeLists
          , List < object > members
        )
        {
            this.Usings         = Usings ;
            this.ExternAliases  = ExternAliases ;
            this.AttributeLists = AttributeLists ;
            Members             = members ;
        }
    }
}