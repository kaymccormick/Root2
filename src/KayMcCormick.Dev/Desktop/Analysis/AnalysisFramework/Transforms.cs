using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
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
                    case ThisExpressionSyntax thise : return new { thise.RawKind , This = true } ;
                    case ArrayCreationExpressionSyntax ac :
                        return new
                               {
                                   ac.RawKind
                                 , Kind     = ac.Kind ( )
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
                                 , Kind = binding.Kind ( )
                                 , Name = TransformSimpleNameSyntax ( binding.Name )
                                 , Op   = TransformOperatorToken ( binding.OperatorToken )
                               } ;
                    case ConditionalAccessExpressionSyntax cond :
                        return new
                               {
                                   cond.RawKind
                                 , Kind        = cond.Kind ( )
                                 , Op          = TransformOperatorToken ( cond.OperatorToken )
                                 , Expression  = TransformExpr ( cond.Expression )
                                 , WhenNotNull = TransformExpr ( cond.WhenNotNull )
                               } ;
                    case LambdaExpressionSyntax l :
                        return new
                               {
                                   l.RawKind
                                 , Kind = l.Kind ( )
                                 , Parameters =
                                       ( l as ParenthesizedLambdaExpressionSyntax )
                                     ?.ParameterList.Parameters.Select ( TransformParameter )
                                      .ToList ( )
                                 , Block = l.Block?.Statements.Select ( TransformStatement )
                                            .ToList ( )
                                 , ExpressionBody = TransformExpr ( l.ExpressionBody )
                               } ;
                    case PredefinedTypeSyntax preDef :
                        return new
                               {
                                   preDef.RawKind
                                 , Kind                 = preDef.Kind ( )
                                 , PredefinedTypeSyntax = TransformKeyword ( preDef.Keyword )
                               } ;
                    case InvocationExpressionSyntax invoc :
                        return new
                               {
                                   invoc.RawKind
                                 , Kind       = invoc.Kind ( )
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
                                 , Kind  = bin.Kind ( )
                                 , Left  = TransformExpr ( bin.Left )
                                 , Op    = bin.OperatorToken.ValueText
                                 , Right = TransformExpr ( bin.Right )
                               } ;
                    case MemberAccessExpressionSyntax macc :
                        return new
                               {
                                   macc.RawKind
                                 , Kind       = macc.Kind ( )
                                 , Expression = TransformExpr ( macc.Expression )
                                 , Operator   = macc.OperatorToken.ValueText
                                 , Name       = TransformExpr ( macc.Name )
                               } ;
                    case IdentifierNameSyntax ident :
                        return new
                               {
                                   ident.RawKind
                                 , Kind       = ident.Kind ( )
                                 , Identifier = ident.Identifier.ValueText
                               } ;
                    case LiteralExpressionSyntax lit :
                        return new
                               {
                                   lit.RawKind , Kind = lit.Kind ( ) , Literal = lit.Token.Value
                               } ;
                    case InterpolatedStringExpressionSyntax i :
                        return new
                               {
                                   i.RawKind
                                 , Kind     = i.Kind ( )
                                 , Contents = i.Contents.Select ( TransformInterpolated ).ToList ( )
                               } ;
                    case ConditionalExpressionSyntax cx :
                        return new
                               {
                                   cx.RawKind
                                 , Kind      = cx.Kind ( )
                                 , Condition = TransformExpr ( cx.Condition )
                                 , WhenTrue  = TransformExpr ( cx.WhenTrue )
                                 , WhenFalse = TransformExpr ( cx.WhenFalse )
                               } ;
                    case IsPatternExpressionSyntax isPattern :
                        return new
                               {
                                   isPattern.RawKind
                                 , Kind       = isPattern.Kind ( )
                                 , Expression = TransformExpr ( isPattern.Expression )
                                 , Pattern    = TransformPatternSyntax ( isPattern.Pattern )
                               } ;
                    default :
                        throw new UnsupportedExpressionTypeSyntax (
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

        private static object TransformPatternSyntax ( PatternSyntax isPatternPattern )
        {
            switch ( isPatternPattern )
            {
                case ConstantPatternSyntax constant :
                    return new
                           {
                               constant.RawKind
                             , Kind       = constant.Kind ( )
                             , Expression = TransformExpr ( constant.Expression )
                           } ;
                case DeclarationPatternSyntax declarationPatternSyntax :
                    return new
                           {
                               declarationPatternSyntax.RawKind
                             , Kind = declarationPatternSyntax.Kind ( )
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
                               discardPatternSyntax.RawKind , Kind = discardPatternSyntax.Kind ( ) ,
                           } ;
                case RecursivePatternSyntax recursivePatternSyntax :
                    return new
                           {
                               recursivePatternSyntax.RawKind
                             , Kind = recursivePatternSyntax.Kind ( )
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
                             , Kind = varPatternSyntax.Kind ( )
                             , Designation =
                                   TransformVariableDesignation ( varPatternSyntax.Designation )
                           } ;
            }

            return new UnsupportedExpressionTypeSyntax ( isPatternPattern.Kind ( ).ToString ( ) ) ;
        }

        private static object TransformPositionalPatternClauseSyntax (
            PositionalPatternClauseSyntax positionalPatternClause
        )
        {
            return new
                   {
                       positionalPatternClause.RawKind
                     , Kind = positionalPatternClause.Kind ( )
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
                     , Kind = propertyPatternClause.Kind ( )
                     , Subpatterns =
                           propertyPatternClause.Subpatterns.Select ( TransformSubpatternSyntax )
                   } ;
        }

        private static object TransformSubpatternSyntax ( SubpatternSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind    = arg.Kind ( )
                     , Pattern = TransformPatternSyntax ( arg.Pattern )
                   } ;
        }

        private static object TransformVariableDesignation ( VariableDesignationSyntax designation )
        {
            switch ( designation )
            {
                case DiscardDesignationSyntax discard :
                    return new { discard.RawKind , Kind = discard.Kind ( ) } ;
                case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax :
                    return new
                           {
                               parenthesizedVariableDesignationSyntax.RawKind
                             , Kind = parenthesizedVariableDesignationSyntax.Kind ( )
                             , Variables =
                                   parenthesizedVariableDesignationSyntax.Variables.Select (
                                                                                            TransformVariableDesignation
                                                                                           )
                           } ;
                case SingleVariableDesignationSyntax singleVariableDesignationSyntax :
                    return new
                           {
                               singleVariableDesignationSyntax.RawKind
                             , Kind = singleVariableDesignationSyntax.Kind ( )
                             , Identifier =
                                   TransformIdentifier (
                                                        singleVariableDesignationSyntax.Identifier
                                                       )
                           } ;
            }

            return new UnsupportedExpressionTypeSyntax ( designation.Kind ( ).ToString ( ) ) ;
        }

        /// <summary>
        /// Transform operator token.
        /// </summary>
        /// <param name="condOperatorToken"></param>
        /// <returns></returns>
        public static object TransformOperatorToken ( in SyntaxToken condOperatorToken )
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
                       arg.RawKind
                     , Kind       = arg.Kind ( )
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
                       argIdentifier.RawKind , Kind = argIdentifier.Kind ( ) , argIdentifier.Value
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
                throw new ArgumentNullException ( nameof ( argType ) ) ;
            }

            switch ( argType )
            {
                case ArrayTypeSyntax arrayTypeSyntax :                   break ;
                case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
                case QualifiedNameSyntax qualifiedNameSyntax :
                    return new
                           {
                               qualifiedNameSyntax.RawKind
                             , Kind  = qualifiedNameSyntax.Kind ( )
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
                case PredefinedTypeSyntax predefinedTypeSyntax :           break ;
                case RefTypeSyntax refTypeSyntax :                         break ;
                case TupleTypeSyntax tupleTypeSyntax :                     break ;
                default :                                                  break ;
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
                     , Kind       = gen.Kind ( )
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

        private static object TransformTypeSymbol( ITypeSymbol arg1Type )
        {
            return new { arg1Type.Name } ;
        }

        public static object TransformTree ( SyntaxTree contextSyntaxTree )
        {
            var syntaxNode = contextSyntaxTree.GetRoot ( ) ;
            switch ( syntaxNode )
            {
                case CompilationUnitSyntax comp :
                    return new PojoCompilationUnit (
                                                    comp.Usings.Select (
                                                                        TransformUsingDirectiveSyntax
                                                                       )
                                                        .ToList ( )
                                                  , comp.Externs.Select (
                                                                         TransformExternAliasDirectiveSyntax
                                                                        )
                                                        .ToList ( )
                                                  , comp.AttributeLists.Select (
                                                                                TransformAttributeListSybtax
                                                                               ).ToList()
                                                  , comp.Members.Select (
                                                                         TransformMemberDeclarationSyntax
                                                                        ).ToList()
                                                   ) ;
            }
            throw new UnsupportedExpressionTypeSyntax(syntaxNode.Kind().ToString()); ;

        }

        private static object TransformMemberDeclarationSyntax ( MemberDeclarationSyntax arg )
        {
            switch ( arg )
            {
                case EventFieldDeclarationSyntax eventFieldDeclarationSyntax :   break ;
                case FieldDeclarationSyntax fieldDeclarationSyntax :             break ;
                case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax :     break ;
                case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
                case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax :
                    break ;
                case DestructorDeclarationSyntax destructorDeclarationSyntax :     break ;
                case MethodDeclarationSyntax methodDeclarationSyntax :             break ;
                case OperatorDeclarationSyntax operatorDeclarationSyntax :         break ;
                case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax :     break ;
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
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax :  return new { namespaceDeclarationSyntax.RawKind, Kind = namespaceDeclarationSyntax.Kind (  ), Members = namespaceDeclarationSyntax.Members.Select(TransformMemberDeclarationSyntax).ToList() } ;
                default :                                                      break ;
            }

                                    throw new UnsupportedExpressionTypeSyntax (arg.Kind ().ToString()); ;
        }

        private static object TransformClassDeclarationSyntax ( ClassDeclarationSyntax classDecl )
        {
            return new PojoClassDeclarationSyntax ( classDecl.Identifier ) ;
        }

        private static object TransformAttributeListSybtax ( AttributeListSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind       = arg.Kind ( )
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
                     , Kind       = argTarget.Kind ( )
                     , Identifier = TransformIdentifier ( argTarget.Identifier )
                   } ;
        }

        private static object TransformAttributeSyntax ( AttributeSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind = arg.Kind ( )
                     , Name = TransformNameSyntax ( arg.Name )
                     , ArgumentList =
                           arg.ArgumentList.Arguments.Select ( TransformAttributeArgumentSyntax )
                   } ;
        }


        private static object  TransformExternAliasDirectiveSyntax  (
            ExternAliasDirectiveSyntax arg
        )
        {
            return new { arg.RawKind , Kind = arg.Kind ( ) } ;
        }


        private static object TransformUsingDirectiveSyntax ( UsingDirectiveSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind  = arg.Kind ( )
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
                     , Kind = argAlias.Kind ( )
                     , Name = TransformIdentifierNameSyntax ( argAlias.Name )
                   } ;
        }


        private static object TransformAttributeArgumentSyntax ( AttributeArgumentSyntax arg )
        {
            return new
                   {
                       arg.RawKind
                     , Kind       = arg.Kind ( )
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
            public PojoClassDeclarationSyntax ( in SyntaxToken classDeclIdentifier ) { }
        }
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
          , List< object >  members
        )
        {
            this.Usings         = Usings ;
            this.ExternAliases  = ExternAliases ;
            this.AttributeLists = AttributeLists ;
            Members             = members ;
        }
    }
}