using System ;
using System.Collections ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;


namespace AnalysisControls.Converters
{
    public class HierarchyConverter : IValueConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            Logger.Debug ( "{type} {type2}" , value?.GetType ( ).FullName , targetType.FullName ) ;
            if ( value is SyntaxNode s )
            {
                if ( targetType == typeof ( string ) )
                {
                    var cs = ( CSharpSyntaxNode ) s ;
                    switch ( cs )
                    {
                        case FieldDeclarationSyntax fieldDeclarationSyntax :
                            return string.Join (
                                                ", "
                                              , fieldDeclarationSyntax.Declaration.Variables
                                               ) ;
                        case GenericNameSyntax genericNameSyntax :
                            return genericNameSyntax.Identifier.ValueText ;
                        case LiteralExpressionSyntax literalExpressionSyntax :
                            return literalExpressionSyntax.Token.ValueText ;
                        case MethodDeclarationSyntax methodDeclarationSyntax :
                            return methodDeclarationSyntax.Identifier.ValueText ;
                        case AccessorDeclarationSyntax accessorDeclarationSyntax :
                            return accessorDeclarationSyntax.Keyword ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AccessorListSyntax accessorListSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
                        case AnonymousObjectCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            anonymousObjectCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
                        case AnonymousObjectMemberDeclaratorSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            anonymousObjectMemberDeclaratorSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArgumentListSyntax argumentListSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArgumentSyntax argumentSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayTypeSyntax arrayTypeSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AssignmentExpressionSyntax assignmentExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeArgumentListSyntax attributeArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeArgumentSyntax attributeArgumentSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeListSyntax attributeListSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeSyntax attributeSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AwaitExpressionSyntax awaitExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseExpressionSyntax baseExpressionSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseListSyntax baseListSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BinaryExpressionSyntax binaryExpressionSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BlockSyntax blockSyntax :                                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BracketedArgumentListSyntax bracketedArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BracketedParameterListSyntax bracketedParameterListSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BreakStatementSyntax breakStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CaseSwitchLabelSyntax caseSwitchLabelSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CastExpressionSyntax castExpressionSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchClauseSyntax catchClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchDeclarationSyntax catchDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchFilterClauseSyntax catchFilterClauseSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CheckedExpressionSyntax checkedExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CheckedStatementSyntax checkedStatementSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ClassDeclarationSyntax classDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ClassOrStructConstraintSyntax classOrStructConstraintSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CompilationUnitSyntax compilationUnitSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalExpressionSyntax conditionalExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstantPatternSyntax constantPatternSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorConstraintSyntax constructorConstraintSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorInitializerSyntax constructorInitializerSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ContinueStatementSyntax continueStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefParameterListSyntax crefParameterListSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefParameterSyntax crefParameterSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DeclarationExpressionSyntax declarationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DeclarationPatternSyntax declarationPatternSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefaultExpressionSyntax defaultExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DelegateDeclarationSyntax delegateDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DiscardDesignationSyntax discardDesignationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DiscardPatternSyntax discardPatternSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DoStatementSyntax doStatementSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElementAccessExpressionSyntax elementAccessExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElementBindingExpressionSyntax elementBindingExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElseClauseSyntax elseClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EmptyStatementSyntax emptyStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EnumDeclarationSyntax enumDeclarationSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EqualsValueClauseSyntax equalsValueClauseSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EventDeclarationSyntax eventDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EventFieldDeclarationSyntax eventFieldDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExpressionStatementSyntax expressionStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExternAliasDirectiveSyntax externAliasDirectiveSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FinallyClauseSyntax finallyClauseSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FixedStatementSyntax fixedStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForEachStatementSyntax forEachStatementSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForEachVariableStatementSyntax forEachVariableStatementSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForStatementSyntax forStatementSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FromClauseSyntax fromClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GlobalStatementSyntax globalStatementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GotoStatementSyntax gotoStatementSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GroupClauseSyntax groupClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case IdentifierNameSyntax identifierNameSyntax :
                            return identifierNameSyntax.Identifier.ValueText ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IfStatementSyntax ifStatementSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case ImplicitArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            implicitArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ImplicitElementAccessSyntax implicitElementAccessSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case ImplicitStackAllocArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            implicitStackAllocArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IncompleteMemberSyntax incompleteMemberSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IndexerDeclarationSyntax indexerDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IndexerMemberCrefSyntax indexerMemberCrefSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InitializerExpressionSyntax initializerExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterfaceDeclarationSyntax interfaceDeclarationSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringTextSyntax interpolatedStringTextSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationSyntax interpolationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InvocationExpressionSyntax invocationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IsPatternExpressionSyntax isPatternExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case JoinClauseSyntax joinClauseSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case JoinIntoClauseSyntax joinIntoClauseSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LabeledStatementSyntax labeledStatementSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LetClauseSyntax letClauseSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LocalDeclarationStatementSyntax localDeclarationStatementSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LocalFunctionStatementSyntax localFunctionStatementSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LockStatementSyntax lockStatementSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MakeRefExpressionSyntax makeRefExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberAccessExpressionSyntax memberAccessExpressionSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberBindingExpressionSyntax memberBindingExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameColonSyntax nameColonSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameEqualsSyntax nameEqualsSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameMemberCrefSyntax nameMemberCrefSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NamespaceDeclarationSyntax namespaceDeclarationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NullableTypeSyntax nullableTypeSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ObjectCreationExpressionSyntax objectCreationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OperatorDeclarationSyntax operatorDeclarationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OperatorMemberCrefSyntax operatorMemberCrefSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OrderByClauseSyntax orderByClauseSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OrderingSyntax orderingSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParameterListSyntax parameterListSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParameterSyntax parameterSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
                        case ParenthesizedVariableDesignationSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            parenthesizedVariableDesignationSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PointerTypeSyntax pointerTypeSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PositionalPatternClauseSyntax positionalPatternClauseSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PredefinedTypeSyntax predefinedTypeSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PropertyDeclarationSyntax propertyDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PropertyPatternClauseSyntax propertyPatternClauseSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QualifiedCrefSyntax qualifiedCrefSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case QualifiedNameSyntax qualifiedNameSyntax :
                            return qualifiedNameSyntax.ToString ( ) ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryBodySyntax queryBodySyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryContinuationSyntax queryContinuationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryExpressionSyntax queryExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RangeExpressionSyntax rangeExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RecursivePatternSyntax recursivePatternSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefExpressionSyntax refExpressionSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefTypeExpressionSyntax refTypeExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefTypeSyntax refTypeSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefValueExpressionSyntax refValueExpressionSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ReturnStatementSyntax returnStatementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SelectClauseSyntax selectClauseSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SingleVariableDesignationSyntax singleVariableDesignationSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SizeOfExpressionSyntax sizeOfExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case StackAllocArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            stackAllocArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StructDeclarationSyntax structDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SubpatternSyntax subpatternSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchExpressionArmSyntax switchExpressionArmSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchExpressionSyntax switchExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchSectionSyntax switchSectionSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchStatementSyntax switchStatementSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThisExpressionSyntax thisExpressionSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThrowExpressionSyntax throwExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThrowStatementSyntax throwStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TryStatementSyntax tryStatementSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleElementSyntax tupleElementSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleExpressionSyntax tupleExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleTypeSyntax tupleTypeSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeArgumentListSyntax typeArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeConstraintSyntax typeConstraintSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeCrefSyntax typeCrefSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeOfExpressionSyntax typeOfExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterListSyntax typeParameterListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterSyntax typeParameterSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UnsafeStatementSyntax unsafeStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UsingDirectiveSyntax usingDirectiveSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UsingStatementSyntax usingStatementSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case VariableDeclarationSyntax variableDeclarationSyntax :
                            return variableDeclarationSyntax.Variables.Count == 1
                                       ? variableDeclarationSyntax
                                        .Variables[ 0 ]
                                        .Identifier.ValueText
                                       : variableDeclarationSyntax.Type.ToString ( ) ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VariableDeclaratorSyntax variableDeclaratorSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VarPatternSyntax varPatternSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhenClauseSyntax whenClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhereClauseSyntax whereClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhileStatementSyntax whileStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCDataSectionSyntax xmlCDataSectionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCommentSyntax xmlCommentSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCrefAttributeSyntax xmlCrefAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementEndTagSyntax xmlElementEndTagSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementStartTagSyntax xmlElementStartTagSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementSyntax xmlElementSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlEmptyElementSyntax xmlEmptyElementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNameAttributeSyntax xmlNameAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNameSyntax xmlNameSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlPrefixSyntax xmlPrefixSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlTextAttributeSyntax xmlTextAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlTextSyntax xmlTextSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case YieldStatementSyntax yieldStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case SimpleNameSyntax simpleNameSyntax :
                            return simpleNameSyntax.Identifier.ValueText ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseArgumentListSyntax baseArgumentListSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseCrefParameterListSyntax baseCrefParameterListSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseParameterListSyntax baseParameterListSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseTypeSyntax baseTypeSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CommonForEachStatementSyntax commonForEachStatementSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InstanceExpressionSyntax instanceExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringContentSyntax interpolatedStringContentSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LambdaExpressionSyntax lambdaExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberCrefSyntax memberCrefSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PatternSyntax patternSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryClauseSyntax queryClauseSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SelectOrGroupClauseSyntax selectOrGroupClauseSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchLabelSyntax switchLabelSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeDeclarationSyntax typeDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterConstraintSyntax typeParameterConstraintSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VariableDesignationSyntax variableDesignationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlAttributeSyntax xmlAttributeSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNodeSyntax xmlNodeSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AnonymousFunctionExpressionSyntax anonymousFunctionExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BranchingDirectiveTriviaSyntax branchingDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefSyntax crefSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameSyntax nameSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StatementSyntax statementSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DirectiveTriviaSyntax directiveTriviaSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberDeclarationSyntax memberDeclarationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeSyntax typeSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExpressionSyntax expressionSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StructuredTriviaSyntax structuredTriviaSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                    }

                    return value.GetType ( ).Name + " (" + cs.Kind ( ) + ")" ;
                }

                LogManager.GetCurrentClassLogger ( ).Debug ( targetType.FullName ) ;
                if ( targetType == typeof ( IEnumerable ) )
                {
                    var cs = s as CSharpSyntaxNode ;
                    switch ( cs )
                    {
                        case AccessorDeclarationSyntax accessorDeclarationSyntax :
                            return accessorDeclarationSyntax.Body != null
                                       ?
                                       ( IEnumerable ) accessorDeclarationSyntax.Body.Statements
                                       : accessorDeclarationSyntax.ExpressionBody != null
                                           ? new[] { accessorDeclarationSyntax.ExpressionBody }
                                           : Array.Empty < SyntaxNode > ( ) ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AccessorListSyntax accessorListSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
                        case AnonymousObjectCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            anonymousObjectCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
                        case AnonymousObjectMemberDeclaratorSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            anonymousObjectMemberDeclaratorSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArgumentListSyntax argumentListSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArgumentSyntax argumentSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrayTypeSyntax arrayTypeSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AssignmentExpressionSyntax assignmentExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeArgumentListSyntax attributeArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeArgumentSyntax attributeArgumentSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeListSyntax attributeListSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeSyntax attributeSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AwaitExpressionSyntax awaitExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseExpressionSyntax baseExpressionSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseListSyntax baseListSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BinaryExpressionSyntax binaryExpressionSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BlockSyntax blockSyntax :                                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BracketedArgumentListSyntax bracketedArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BracketedParameterListSyntax bracketedParameterListSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BreakStatementSyntax breakStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CaseSwitchLabelSyntax caseSwitchLabelSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CastExpressionSyntax castExpressionSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchClauseSyntax catchClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchDeclarationSyntax catchDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CatchFilterClauseSyntax catchFilterClauseSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CheckedExpressionSyntax checkedExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CheckedStatementSyntax checkedStatementSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ClassDeclarationSyntax classDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ClassOrStructConstraintSyntax classOrStructConstraintSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CompilationUnitSyntax compilationUnitSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalExpressionSyntax conditionalExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstantPatternSyntax constantPatternSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorConstraintSyntax constructorConstraintSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConstructorInitializerSyntax constructorInitializerSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ContinueStatementSyntax continueStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefParameterListSyntax crefParameterListSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefParameterSyntax crefParameterSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DeclarationExpressionSyntax declarationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DeclarationPatternSyntax declarationPatternSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefaultExpressionSyntax defaultExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DelegateDeclarationSyntax delegateDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DiscardDesignationSyntax discardDesignationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DiscardPatternSyntax discardPatternSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DoStatementSyntax doStatementSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElementAccessExpressionSyntax elementAccessExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElementBindingExpressionSyntax elementBindingExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElseClauseSyntax elseClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EmptyStatementSyntax emptyStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EnumDeclarationSyntax enumDeclarationSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EqualsValueClauseSyntax equalsValueClauseSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EventDeclarationSyntax eventDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case EventFieldDeclarationSyntax eventFieldDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExpressionStatementSyntax expressionStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExternAliasDirectiveSyntax externAliasDirectiveSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FieldDeclarationSyntax fieldDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FinallyClauseSyntax finallyClauseSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FixedStatementSyntax fixedStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForEachStatementSyntax forEachStatementSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForEachVariableStatementSyntax forEachVariableStatementSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ForStatementSyntax forStatementSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case FromClauseSyntax fromClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GenericNameSyntax genericNameSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GlobalStatementSyntax globalStatementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GotoStatementSyntax gotoStatementSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case GroupClauseSyntax groupClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IdentifierNameSyntax identifierNameSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IfStatementSyntax ifStatementSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case ImplicitArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            implicitArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ImplicitElementAccessSyntax implicitElementAccessSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case ImplicitStackAllocArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            implicitStackAllocArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IncompleteMemberSyntax incompleteMemberSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IndexerDeclarationSyntax indexerDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IndexerMemberCrefSyntax indexerMemberCrefSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InitializerExpressionSyntax initializerExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterfaceDeclarationSyntax interfaceDeclarationSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringTextSyntax interpolatedStringTextSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolationSyntax interpolationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InvocationExpressionSyntax invocationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case IsPatternExpressionSyntax isPatternExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case JoinClauseSyntax joinClauseSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case JoinIntoClauseSyntax joinIntoClauseSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LabeledStatementSyntax labeledStatementSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LetClauseSyntax letClauseSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LiteralExpressionSyntax literalExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LocalDeclarationStatementSyntax localDeclarationStatementSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LocalFunctionStatementSyntax localFunctionStatementSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LockStatementSyntax lockStatementSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MakeRefExpressionSyntax makeRefExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberAccessExpressionSyntax memberAccessExpressionSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberBindingExpressionSyntax memberBindingExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MethodDeclarationSyntax methodDeclarationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameColonSyntax nameColonSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameEqualsSyntax nameEqualsSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameMemberCrefSyntax nameMemberCrefSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NamespaceDeclarationSyntax namespaceDeclarationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NullableTypeSyntax nullableTypeSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ObjectCreationExpressionSyntax objectCreationExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OperatorDeclarationSyntax operatorDeclarationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OperatorMemberCrefSyntax operatorMemberCrefSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OrderByClauseSyntax orderByClauseSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case OrderingSyntax orderingSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParameterListSyntax parameterListSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParameterSyntax parameterSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
                        case ParenthesizedVariableDesignationSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            parenthesizedVariableDesignationSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PointerTypeSyntax pointerTypeSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PositionalPatternClauseSyntax positionalPatternClauseSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PredefinedTypeSyntax predefinedTypeSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PropertyDeclarationSyntax propertyDeclarationSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PropertyPatternClauseSyntax propertyPatternClauseSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QualifiedCrefSyntax qualifiedCrefSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QualifiedNameSyntax qualifiedNameSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryBodySyntax queryBodySyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryContinuationSyntax queryContinuationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryExpressionSyntax queryExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RangeExpressionSyntax rangeExpressionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RecursivePatternSyntax recursivePatternSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefExpressionSyntax refExpressionSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefTypeExpressionSyntax refTypeExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefTypeSyntax refTypeSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RefValueExpressionSyntax refValueExpressionSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ReturnStatementSyntax returnStatementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SelectClauseSyntax selectClauseSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SingleVariableDesignationSyntax singleVariableDesignationSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SizeOfExpressionSyntax sizeOfExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                        case StackAllocArrayCreationExpressionSyntax
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                            stackAllocArrayCreationExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StructDeclarationSyntax structDeclarationSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SubpatternSyntax subpatternSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchExpressionArmSyntax switchExpressionArmSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchExpressionSyntax switchExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchSectionSyntax switchSectionSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchStatementSyntax switchStatementSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThisExpressionSyntax thisExpressionSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThrowExpressionSyntax throwExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ThrowStatementSyntax throwStatementSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TryStatementSyntax tryStatementSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleElementSyntax tupleElementSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleExpressionSyntax tupleExpressionSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TupleTypeSyntax tupleTypeSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeArgumentListSyntax typeArgumentListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeConstraintSyntax typeConstraintSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeCrefSyntax typeCrefSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeOfExpressionSyntax typeOfExpressionSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            :
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterListSyntax typeParameterListSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterSyntax typeParameterSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UnsafeStatementSyntax unsafeStatementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UsingDirectiveSyntax usingDirectiveSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case UsingStatementSyntax usingStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VariableDeclarationSyntax variableDeclarationSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VariableDeclaratorSyntax variableDeclaratorSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VarPatternSyntax varPatternSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhenClauseSyntax whenClauseSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhereClauseSyntax whereClauseSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case WhileStatementSyntax whileStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCDataSectionSyntax xmlCDataSectionSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCommentSyntax xmlCommentSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlCrefAttributeSyntax xmlCrefAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementEndTagSyntax xmlElementEndTagSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementStartTagSyntax xmlElementStartTagSyntax :             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlElementSyntax xmlElementSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlEmptyElementSyntax xmlEmptyElementSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNameAttributeSyntax xmlNameAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNameSyntax xmlNameSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlPrefixSyntax xmlPrefixSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlTextAttributeSyntax xmlTextAttributeSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlTextSyntax xmlTextSyntax :                                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case YieldStatementSyntax yieldStatementSyntax :                     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseArgumentListSyntax baseArgumentListSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseCrefParameterListSyntax baseCrefParameterListSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax :       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseParameterListSyntax baseParameterListSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax :   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseTypeSyntax baseTypeSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CommonForEachStatementSyntax commonForEachStatementSyntax :     break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InstanceExpressionSyntax instanceExpressionSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case InterpolatedStringContentSyntax interpolatedStringContentSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case LambdaExpressionSyntax lambdaExpressionSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberCrefSyntax memberCrefSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case PatternSyntax patternSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case QueryClauseSyntax queryClauseSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SelectOrGroupClauseSyntax selectOrGroupClauseSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SimpleNameSyntax simpleNameSyntax :                           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case SwitchLabelSyntax switchLabelSyntax :                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeDeclarationSyntax typeDeclarationSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeParameterConstraintSyntax typeParameterConstraintSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case VariableDesignationSyntax variableDesignationSyntax :         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlAttributeSyntax xmlAttributeSyntax :                       break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case XmlNodeSyntax xmlNodeSyntax :                                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case AnonymousFunctionExpressionSyntax anonymousFunctionExpressionSyntax :
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                            break ;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax :           break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case BranchingDirectiveTriviaSyntax branchingDirectiveTriviaSyntax : break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case CrefSyntax crefSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case NameSyntax nameSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StatementSyntax statementSyntax :                               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case DirectiveTriviaSyntax directiveTriviaSyntax :                   break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case MemberDeclarationSyntax memberDeclarationSyntax :               break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case TypeSyntax typeSyntax :                                         break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case ExpressionSyntax expressionSyntax :                             break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        case StructuredTriviaSyntax structuredTriviaSyntax :                 break ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                    }

                    return s.ChildNodes ( ) ;
                }

                Logger.Debug ( "returning null for {s} {t}" , s , targetType.FullName ) ;
                return null ;
            }

            Logger.Debug ( "returning null for {s} {t}" , value , targetType.FullName ) ;
            return null ;
        }

        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}