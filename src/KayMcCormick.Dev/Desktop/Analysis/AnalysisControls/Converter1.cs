using System ;
using System.Collections ;
using System.ComponentModel ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.VisualBasic ;
using NLog ;
using AnonymousObjectCreationExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax ;
using ArgumentListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax ;
using ArgumentSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax ;
using ArrayCreationExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax ;
using ArrayRankSpecifierSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax ;
using ArrayTypeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax ;
using AttributeListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax ;
using AttributeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax ;
using AwaitExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax ;
using BadDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.BadDirectiveTriviaSyntax ;
using BinaryExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax ;
using CastExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax ;
using CatchFilterClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax ;
using CompilationUnitSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax ;
using ConditionalAccessExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax ;
using ContinueStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ContinueStatementSyntax ;
using DocumentationCommentTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax ;
using DoStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.DoStatementSyntax ;
using ElseDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ElseDirectiveTriviaSyntax ;
using EmptyStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.EmptyStatementSyntax ;
using EndIfDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.EndIfDirectiveTriviaSyntax ;
using EndRegionDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.EndRegionDirectiveTriviaSyntax ;
using EnumMemberDeclarationSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax ;
using ExpressionStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax ;
using ExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax ;
using FieldDeclarationSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax ;
using ForEachStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax ;
using ForStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax ;
using FromClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.FromClauseSyntax ;
using GenericNameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax ;
using IdentifierNameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax ;
using IfDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.IfDirectiveTriviaSyntax ;
using IfStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.IfStatementSyntax ;
using IncompleteMemberSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax ;
using InstanceExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax ;
using InterpolatedStringContentSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax ;
using InterpolatedStringExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax ;
using InterpolatedStringTextSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringTextSyntax ;
using InterpolationAlignmentClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax ;
using InterpolationFormatClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax ;
using InterpolationSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationSyntax ;
using InvocationExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax ;
using JoinClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.JoinClauseSyntax ;
using LambdaExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.LambdaExpressionSyntax ;
using LetClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.LetClauseSyntax ;
using LiteralExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax ;
using LocalDeclarationStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax ;
using MemberAccessExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax ;
using NameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax ;
using NullableTypeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax ;
using ObjectCreationExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ObjectCreationExpressionSyntax ;
using OrderByClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.OrderByClauseSyntax ;
using OrderingSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax ;
using ParameterListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ;
using ParameterSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax ;
using ParenthesizedExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax ;
using PredefinedTypeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax ;
using QualifiedNameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax ;
using QueryClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax ;
using QueryExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax ;
using ReferenceDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ReferenceDirectiveTriviaSyntax ;
using RegionDirectiveTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.RegionDirectiveTriviaSyntax ;
using ReturnStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ReturnStatementSyntax ;
using SelectClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.SelectClauseSyntax ;
using SimpleNameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax ;
using SkippedTokensTriviaSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.SkippedTokensTriviaSyntax ;
using StatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax ;
using ThrowStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ThrowStatementSyntax ;
using TryStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TryStatementSyntax ;
using TupleElementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax ;
using TupleExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax ;
using TupleTypeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax ;
using TypeArgumentListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax ;
using TypeConstraintSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax ;
using TypeOfExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax ;
using TypeParameterConstraintClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ;
using TypeParameterListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax ;
using TypeParameterSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax ;
using TypeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ;
using UsingStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.UsingStatementSyntax ;
using VariableDeclaratorSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax ;
using WhereClauseSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.WhereClauseSyntax ;
using WhileStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.WhileStatementSyntax ;
using XmlCDataSectionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlCDataSectionSyntax ;
using XmlCommentSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlCommentSyntax ;
using XmlCrefAttributeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlCrefAttributeSyntax ;
using XmlElementEndTagSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax ;
using XmlElementStartTagSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax ;
using XmlElementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementSyntax ;
using XmlEmptyElementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlEmptyElementSyntax ;
using XmlNameAttributeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameAttributeSyntax ;
using XmlNameSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax ;
using XmlNodeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax ;
using XmlPrefixSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax ;
using XmlProcessingInstructionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlProcessingInstructionSyntax ;
using XmlTextSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.XmlTextSyntax ;
using YieldStatementSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.YieldStatementSyntax ;
namespace AnalysisControls
{

    public class Converter2 : TypeConverter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of TypeConverter
        public override bool CanConvertFrom ( ITypeDescriptorContext context , Type sourceType )
        {
            Logger.Debug ( "{context}, {sourceType}" , context , sourceType ) ;
            return base.CanConvertFrom ( context , sourceType ) ;
        }

        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType ) { return base.CanConvertTo ( context , destinationType ) ; }

        public override object ConvertFrom ( ITypeDescriptorContext context , CultureInfo culture , object value ) { return base.ConvertFrom ( context , culture , value ) ; }

        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }
        #endregion
    }

    public enum Converter1Param
    {
        Location,
        ChildNodesAndTokens,
        ChildNodes,
        ChildTokens,
        DescendantNodes,
        DescendantNodesAndSelf,
        DescendantNodesAndTokens,
        DescendantNodesAndTokensAndSelf,
        DescendantTokens,
        DescendantTrivia,
        Diagnostics,
    }
    public class Converter1 : IValueConverter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            Logger.Debug (
                          "{type} {type2}"
                        , value?.GetType ( ).FullName
                        , targetType.FullName
                         ) ;
            if ( value is SyntaxNode s )
            {
                if ( targetType == typeof ( string ) )
                {
                    var cs = s as CSharpSyntaxNode ;
                    switch ( cs)
                    {
                        case FieldDeclarationSyntax fieldDeclarationSyntax :
                            return string.Join (
                                                ", "
                                              , fieldDeclarationSyntax.Declaration.Variables
                                               ) ;
                        case GenericNameSyntax genericNameSyntax :             
                            return genericNameSyntax.Identifier.GetIdentifierText();
                        case LiteralExpressionSyntax literalExpressionSyntax : return literalExpressionSyntax.Token.ValueText ;
                        case MethodDeclarationSyntax methodDeclarationSyntax : return methodDeclarationSyntax.Identifier.GetIdentifierText();
                        case AccessorDeclarationSyntax accessorDeclarationSyntax : return  accessorDeclarationSyntax.Keyword ;
                        case AccessorListSyntax accessorListSyntax : break ;
                        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
                        case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax : break ;
                        case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax : break ;
                        case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax : break ;
                        case ArgumentListSyntax argumentListSyntax : break ;
                        case ArgumentSyntax argumentSyntax : break ;
                        case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax : break ;
                        case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax : break ;
                        case ArrayTypeSyntax arrayTypeSyntax : break ;
                        case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax : break ;
                        case AssignmentExpressionSyntax assignmentExpressionSyntax : break ;
                        case AttributeArgumentListSyntax attributeArgumentListSyntax : break ;
                        case AttributeArgumentSyntax attributeArgumentSyntax : break ;
                        case AttributeListSyntax attributeListSyntax : break ;
                        case AttributeSyntax attributeSyntax : break ;
                        case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax : break ;
                        case AwaitExpressionSyntax awaitExpressionSyntax : break ;
                        case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax : break ;
                        case BaseExpressionSyntax baseExpressionSyntax : break ;
                        case BaseListSyntax baseListSyntax : break ;
                        case BinaryExpressionSyntax binaryExpressionSyntax : break ;
                        case BlockSyntax blockSyntax : break ;
                        case BracketedArgumentListSyntax bracketedArgumentListSyntax : break ;
                        case BracketedParameterListSyntax bracketedParameterListSyntax : break ;
                        case BreakStatementSyntax breakStatementSyntax : break ;
                        case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax : break ;
                        case CaseSwitchLabelSyntax caseSwitchLabelSyntax : break ;
                        case CastExpressionSyntax castExpressionSyntax : break ;
                        case CatchClauseSyntax catchClauseSyntax : break ;
                        case CatchDeclarationSyntax catchDeclarationSyntax : break ;
                        case CatchFilterClauseSyntax catchFilterClauseSyntax : break ;
                        case CheckedExpressionSyntax checkedExpressionSyntax : break ;
                        case CheckedStatementSyntax checkedStatementSyntax : break ;
                        case ClassDeclarationSyntax classDeclarationSyntax : break ;
                        case ClassOrStructConstraintSyntax classOrStructConstraintSyntax : break ;
                        case CompilationUnitSyntax compilationUnitSyntax : break ;
                        case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax : break ;
                        case ConditionalExpressionSyntax conditionalExpressionSyntax : break ;
                        case ConstantPatternSyntax constantPatternSyntax : break ;
                        case ConstructorConstraintSyntax constructorConstraintSyntax : break ;
                        case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
                        case ConstructorInitializerSyntax constructorInitializerSyntax : break ;
                        case ContinueStatementSyntax continueStatementSyntax : break ;
                        case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax : break ;
                        case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax : break ;
                        case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax : break ;
                        case CrefParameterListSyntax crefParameterListSyntax : break ;
                        case CrefParameterSyntax crefParameterSyntax : break ;
                        case DeclarationExpressionSyntax declarationExpressionSyntax : break ;
                        case DeclarationPatternSyntax declarationPatternSyntax : break ;
                        case DefaultExpressionSyntax defaultExpressionSyntax : break ;
                        case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax : break ;
                        case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax : break ;
                        case DelegateDeclarationSyntax delegateDeclarationSyntax : break ;
                        case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
                        case DiscardDesignationSyntax discardDesignationSyntax : break ;
                        case DiscardPatternSyntax discardPatternSyntax : break ;
                        case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax : break ;
                        case DoStatementSyntax doStatementSyntax : break ;
                        case ElementAccessExpressionSyntax elementAccessExpressionSyntax : break ;
                        case ElementBindingExpressionSyntax elementBindingExpressionSyntax : break ;
                        case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax : break ;
                        case ElseClauseSyntax elseClauseSyntax : break ;
                        case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax : break ;
                        case EmptyStatementSyntax emptyStatementSyntax : break ;
                        case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax : break ;
                        case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax : break ;
                        case EnumDeclarationSyntax enumDeclarationSyntax : break ;
                        case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax : break ;
                        case EqualsValueClauseSyntax equalsValueClauseSyntax : break ;
                        case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax : break ;
                        case EventDeclarationSyntax eventDeclarationSyntax : break ;
                        case EventFieldDeclarationSyntax eventFieldDeclarationSyntax : break ;
                        case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax : break ;
                        case ExpressionStatementSyntax expressionStatementSyntax : break ;
                        case ExternAliasDirectiveSyntax externAliasDirectiveSyntax : break ;
                        case FinallyClauseSyntax finallyClauseSyntax : break ;
                        case FixedStatementSyntax fixedStatementSyntax : break ;
                        case ForEachStatementSyntax forEachStatementSyntax : break ;
                        case ForEachVariableStatementSyntax forEachVariableStatementSyntax : break ;
                        case ForStatementSyntax forStatementSyntax : break ;
                        case FromClauseSyntax fromClauseSyntax : break ;
                        case GlobalStatementSyntax globalStatementSyntax : break ;
                        case GotoStatementSyntax gotoStatementSyntax : break ;
                        case GroupClauseSyntax groupClauseSyntax : break ;
                        case IdentifierNameSyntax identifierNameSyntax : return identifierNameSyntax.Identifier.GetIdentifierText();
                        case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax : break ;
                        case IfStatementSyntax ifStatementSyntax : break ;
                        case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax : break ;
                        case ImplicitElementAccessSyntax implicitElementAccessSyntax : break ;
                        case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax : break ;
                        case IncompleteMemberSyntax incompleteMemberSyntax : break ;
                        case IndexerDeclarationSyntax indexerDeclarationSyntax : break ;
                        case IndexerMemberCrefSyntax indexerMemberCrefSyntax : break ;
                        case InitializerExpressionSyntax initializerExpressionSyntax : break ;
                        case InterfaceDeclarationSyntax interfaceDeclarationSyntax : break ;
                        case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax : break ;
                        case InterpolatedStringTextSyntax interpolatedStringTextSyntax : break ;
                        case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax : break ;
                        case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax : break ;
                        case InterpolationSyntax interpolationSyntax : break ;
                        case InvocationExpressionSyntax invocationExpressionSyntax : break ;
                        case IsPatternExpressionSyntax isPatternExpressionSyntax : break ;
                        case JoinClauseSyntax joinClauseSyntax : break ;
                        case JoinIntoClauseSyntax joinIntoClauseSyntax : break ;
                        case LabeledStatementSyntax labeledStatementSyntax : break ;
                        case LetClauseSyntax letClauseSyntax : break ;
                        case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax : break ;
                        case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax : break ;
                        case LocalDeclarationStatementSyntax localDeclarationStatementSyntax : break ;
                        case LocalFunctionStatementSyntax localFunctionStatementSyntax : break ;
                        case LockStatementSyntax lockStatementSyntax : break ;
                        case MakeRefExpressionSyntax makeRefExpressionSyntax : break ;
                        case MemberAccessExpressionSyntax memberAccessExpressionSyntax : break ;
                        case MemberBindingExpressionSyntax memberBindingExpressionSyntax : break ;
                        case NameColonSyntax nameColonSyntax : break ;
                        case NameEqualsSyntax nameEqualsSyntax : break ;
                        case NameMemberCrefSyntax nameMemberCrefSyntax : break ;
                        case NamespaceDeclarationSyntax namespaceDeclarationSyntax : break ;
                        case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax : break ;
                        case NullableTypeSyntax nullableTypeSyntax : break ;
                        case ObjectCreationExpressionSyntax objectCreationExpressionSyntax : break ;
                        case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax : break ;
                        case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax : break ;
                        case OperatorDeclarationSyntax operatorDeclarationSyntax : break ;
                        case OperatorMemberCrefSyntax operatorMemberCrefSyntax : break ;
                        case OrderByClauseSyntax orderByClauseSyntax : break ;
                        case OrderingSyntax orderingSyntax : break ;
                        case ParameterListSyntax parameterListSyntax : break ;
                        case ParameterSyntax parameterSyntax : break ;
                        case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax : break ;
                        case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax : break ;
                        case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax : break ;
                        case PointerTypeSyntax pointerTypeSyntax : break ;
                        case PositionalPatternClauseSyntax positionalPatternClauseSyntax : break ;
                        case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax : break ;
                        case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax : break ;
                        case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax : break ;
                        case PredefinedTypeSyntax predefinedTypeSyntax : break ;
                        case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax : break ;
                        case PropertyDeclarationSyntax propertyDeclarationSyntax : break ;
                        case PropertyPatternClauseSyntax propertyPatternClauseSyntax : break ;
                        case QualifiedCrefSyntax qualifiedCrefSyntax : break ;
                        case QualifiedNameSyntax qualifiedNameSyntax :
                            return qualifiedNameSyntax.ToString ( ) ;
                        case QueryBodySyntax queryBodySyntax : break ;
                        case QueryContinuationSyntax queryContinuationSyntax : break ;
                        case QueryExpressionSyntax queryExpressionSyntax : break ;
                        case RangeExpressionSyntax rangeExpressionSyntax : break ;
                        case RecursivePatternSyntax recursivePatternSyntax : break ;
                        case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax : break ;
                        case RefExpressionSyntax refExpressionSyntax : break ;
                        case RefTypeExpressionSyntax refTypeExpressionSyntax : break ;
                        case RefTypeSyntax refTypeSyntax : break ;
                        case RefValueExpressionSyntax refValueExpressionSyntax : break ;
                        case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax : break ;
                        case ReturnStatementSyntax returnStatementSyntax : break ;
                        case SelectClauseSyntax selectClauseSyntax : break ;
                        case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax : break ;
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax : break ;
                        case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax : break ;
                        case SingleVariableDesignationSyntax singleVariableDesignationSyntax : break ;
                        case SizeOfExpressionSyntax sizeOfExpressionSyntax : break ;
                        case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax : break ;
                        case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax : break ;
                        case StructDeclarationSyntax structDeclarationSyntax : break ;
                        case SubpatternSyntax subpatternSyntax : break ;
                        case SwitchExpressionArmSyntax switchExpressionArmSyntax : break ;
                        case SwitchExpressionSyntax switchExpressionSyntax : break ;
                        case SwitchSectionSyntax switchSectionSyntax : break ;
                        case SwitchStatementSyntax switchStatementSyntax : break ;
                        case ThisExpressionSyntax thisExpressionSyntax : break ;
                        case ThrowExpressionSyntax throwExpressionSyntax : break ;
                        case ThrowStatementSyntax throwStatementSyntax : break ;
                        case TryStatementSyntax tryStatementSyntax : break ;
                        case TupleElementSyntax tupleElementSyntax : break ;
                        case TupleExpressionSyntax tupleExpressionSyntax : break ;
                        case TupleTypeSyntax tupleTypeSyntax : break ;
                        case TypeArgumentListSyntax typeArgumentListSyntax : break ;
                        case TypeConstraintSyntax typeConstraintSyntax : break ;
                        case TypeCrefSyntax typeCrefSyntax : break ;
                        case TypeOfExpressionSyntax typeOfExpressionSyntax : break ;
                        case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax : break ;
                        case TypeParameterListSyntax typeParameterListSyntax : break ;
                        case TypeParameterSyntax typeParameterSyntax : break ;
                        case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax : break ;
                        case UnsafeStatementSyntax unsafeStatementSyntax : break ;
                        case UsingDirectiveSyntax usingDirectiveSyntax : break ;
                        case UsingStatementSyntax usingStatementSyntax : break ;
                        case VariableDeclarationSyntax variableDeclarationSyntax : return variableDeclarationSyntax.Variables.Count == 1 ? variableDeclarationSyntax.Variables[0].Identifier.ValueText : variableDeclarationSyntax.Type.ToString();
                        case VariableDeclaratorSyntax variableDeclaratorSyntax : break ;
                        case VarPatternSyntax varPatternSyntax : break ;
                        case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax : break ;
                        case WhenClauseSyntax whenClauseSyntax : break ;
                        case WhereClauseSyntax whereClauseSyntax : break ;
                        case WhileStatementSyntax whileStatementSyntax : break ;
                        case XmlCDataSectionSyntax xmlCDataSectionSyntax : break ;
                        case XmlCommentSyntax xmlCommentSyntax : break ;
                        case XmlCrefAttributeSyntax xmlCrefAttributeSyntax : break ;
                        case XmlElementEndTagSyntax xmlElementEndTagSyntax : break ;
                        case XmlElementStartTagSyntax xmlElementStartTagSyntax : break ;
                        case XmlElementSyntax xmlElementSyntax : break ;
                        case XmlEmptyElementSyntax xmlEmptyElementSyntax : break ;
                        case XmlNameAttributeSyntax xmlNameAttributeSyntax : break ;
                        case XmlNameSyntax xmlNameSyntax : break ;
                        case XmlPrefixSyntax xmlPrefixSyntax : break ;
                        case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax : break ;
                        case XmlTextAttributeSyntax xmlTextAttributeSyntax : break ;
                        case XmlTextSyntax xmlTextSyntax : break ;
                        case YieldStatementSyntax yieldStatementSyntax : break ;
                        case SimpleNameSyntax simpleNameSyntax :               return simpleNameSyntax.Identifier.GetIdentifierText();
                        case BaseArgumentListSyntax baseArgumentListSyntax : break ;
                        case BaseCrefParameterListSyntax baseCrefParameterListSyntax : break ;
                        case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax : break ;
                        case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax : break ;
                        case BaseParameterListSyntax baseParameterListSyntax : break ;
                        case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax : break ;
                        case BaseTypeSyntax baseTypeSyntax : break ;
                        case CommonForEachStatementSyntax commonForEachStatementSyntax : break ;
                        case ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax : break ;
                        case InstanceExpressionSyntax instanceExpressionSyntax : break ;
                        case InterpolatedStringContentSyntax interpolatedStringContentSyntax : break ;
                        case LambdaExpressionSyntax lambdaExpressionSyntax : break ;
                        case MemberCrefSyntax memberCrefSyntax : break ;
                        case PatternSyntax patternSyntax : break ;
                        case QueryClauseSyntax queryClauseSyntax : break ;
                        case SelectOrGroupClauseSyntax selectOrGroupClauseSyntax : break ;
                        case SwitchLabelSyntax switchLabelSyntax : break ;
                        case TypeDeclarationSyntax typeDeclarationSyntax : break ;
                        case TypeParameterConstraintSyntax typeParameterConstraintSyntax : break ;
                        case VariableDesignationSyntax variableDesignationSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax xmlAttributeSyntax : break ;
                        case XmlNodeSyntax xmlNodeSyntax : break ;
                        case AnonymousFunctionExpressionSyntax anonymousFunctionExpressionSyntax : break ;
                        case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax : break ;
                        case BranchingDirectiveTriviaSyntax branchingDirectiveTriviaSyntax : break ;
                        case CrefSyntax crefSyntax : break ;
                        case NameSyntax nameSyntax : break ;
                        case StatementSyntax statementSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.DirectiveTriviaSyntax directiveTriviaSyntax : break ;
                        case MemberDeclarationSyntax memberDeclarationSyntax : break ;
                        case TypeSyntax typeSyntax : break ;
                        case ExpressionSyntax expressionSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax structuredTriviaSyntax : break ;
                    }
                    return value.GetType().Name +
                           " (" + cs.Kind()            + ")";

                }

                LogManager.GetCurrentClassLogger ( ).Debug ( targetType.FullName ) ;
                if ( targetType == typeof ( IEnumerable ) )
                {
                    var cs = s as CSharpSyntaxNode ;
                    switch (cs)
                    {
                        case AccessorDeclarationSyntax accessorDeclarationSyntax :
                            return accessorDeclarationSyntax.Body != null ? (IEnumerable)accessorDeclarationSyntax.Body.Statements : accessorDeclarationSyntax.ExpressionBody != null ? new [] { accessorDeclarationSyntax.ExpressionBody } : Array.Empty<SyntaxNode> (  );
                        case AccessorListSyntax accessorListSyntax : break ;
                        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax : break ;
                        case AnonymousMethodExpressionSyntax anonymousMethodExpressionSyntax : break ;
                        case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax : break ;
                        case AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclaratorSyntax : break ;
                        case ArgumentListSyntax argumentListSyntax : break ;
                        case ArgumentSyntax argumentSyntax : break ;
                        case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax : break ;
                        case ArrayRankSpecifierSyntax arrayRankSpecifierSyntax : break ;
                        case ArrayTypeSyntax arrayTypeSyntax : break ;
                        case ArrowExpressionClauseSyntax arrowExpressionClauseSyntax : break ;
                        case AssignmentExpressionSyntax assignmentExpressionSyntax : break ;
                        case AttributeArgumentListSyntax attributeArgumentListSyntax : break ;
                        case AttributeArgumentSyntax attributeArgumentSyntax : break ;
                        case AttributeListSyntax attributeListSyntax : break ;
                        case AttributeSyntax attributeSyntax : break ;
                        case AttributeTargetSpecifierSyntax attributeTargetSpecifierSyntax : break ;
                        case AwaitExpressionSyntax awaitExpressionSyntax : break ;
                        case BadDirectiveTriviaSyntax badDirectiveTriviaSyntax : break ;
                        case BaseExpressionSyntax baseExpressionSyntax : break ;
                        case BaseListSyntax baseListSyntax : break ;
                        case BinaryExpressionSyntax binaryExpressionSyntax : break ;
                        case BlockSyntax blockSyntax : break ;
                        case BracketedArgumentListSyntax bracketedArgumentListSyntax : break ;
                        case BracketedParameterListSyntax bracketedParameterListSyntax : break ;
                        case BreakStatementSyntax breakStatementSyntax : break ;
                        case CasePatternSwitchLabelSyntax casePatternSwitchLabelSyntax : break ;
                        case CaseSwitchLabelSyntax caseSwitchLabelSyntax : break ;
                        case CastExpressionSyntax castExpressionSyntax : break ;
                        case CatchClauseSyntax catchClauseSyntax : break ;
                        case CatchDeclarationSyntax catchDeclarationSyntax : break ;
                        case CatchFilterClauseSyntax catchFilterClauseSyntax : break ;
                        case CheckedExpressionSyntax checkedExpressionSyntax : break ;
                        case CheckedStatementSyntax checkedStatementSyntax : break ;
                        case ClassDeclarationSyntax classDeclarationSyntax : break ;
                        case ClassOrStructConstraintSyntax classOrStructConstraintSyntax : break ;
                        case CompilationUnitSyntax compilationUnitSyntax : break ;
                        case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax : break ;
                        case ConditionalExpressionSyntax conditionalExpressionSyntax : break ;
                        case ConstantPatternSyntax constantPatternSyntax : break ;
                        case ConstructorConstraintSyntax constructorConstraintSyntax : break ;
                        case ConstructorDeclarationSyntax constructorDeclarationSyntax : break ;
                        case ConstructorInitializerSyntax constructorInitializerSyntax : break ;
                        case ContinueStatementSyntax continueStatementSyntax : break ;
                        case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax : break ;
                        case ConversionOperatorMemberCrefSyntax conversionOperatorMemberCrefSyntax : break ;
                        case CrefBracketedParameterListSyntax crefBracketedParameterListSyntax : break ;
                        case CrefParameterListSyntax crefParameterListSyntax : break ;
                        case CrefParameterSyntax crefParameterSyntax : break ;
                        case DeclarationExpressionSyntax declarationExpressionSyntax : break ;
                        case DeclarationPatternSyntax declarationPatternSyntax : break ;
                        case DefaultExpressionSyntax defaultExpressionSyntax : break ;
                        case DefaultSwitchLabelSyntax defaultSwitchLabelSyntax : break ;
                        case DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax : break ;
                        case DelegateDeclarationSyntax delegateDeclarationSyntax : break ;
                        case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
                        case DiscardDesignationSyntax discardDesignationSyntax : break ;
                        case DiscardPatternSyntax discardPatternSyntax : break ;
                        case DocumentationCommentTriviaSyntax documentationCommentTriviaSyntax : break ;
                        case DoStatementSyntax doStatementSyntax : break ;
                        case ElementAccessExpressionSyntax elementAccessExpressionSyntax : break ;
                        case ElementBindingExpressionSyntax elementBindingExpressionSyntax : break ;
                        case ElifDirectiveTriviaSyntax elifDirectiveTriviaSyntax : break ;
                        case ElseClauseSyntax elseClauseSyntax : break ;
                        case ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax : break ;
                        case EmptyStatementSyntax emptyStatementSyntax : break ;
                        case EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax : break ;
                        case EndRegionDirectiveTriviaSyntax endRegionDirectiveTriviaSyntax : break ;
                        case EnumDeclarationSyntax enumDeclarationSyntax : break ;
                        case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax : break ;
                        case EqualsValueClauseSyntax equalsValueClauseSyntax : break ;
                        case ErrorDirectiveTriviaSyntax errorDirectiveTriviaSyntax : break ;
                        case EventDeclarationSyntax eventDeclarationSyntax : break ;
                        case EventFieldDeclarationSyntax eventFieldDeclarationSyntax : break ;
                        case ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax : break ;
                        case ExpressionStatementSyntax expressionStatementSyntax : break ;
                        case ExternAliasDirectiveSyntax externAliasDirectiveSyntax : break ;
                        case FieldDeclarationSyntax fieldDeclarationSyntax : break ;
                        case FinallyClauseSyntax finallyClauseSyntax : break ;
                        case FixedStatementSyntax fixedStatementSyntax : break ;
                        case ForEachStatementSyntax forEachStatementSyntax : break ;
                        case ForEachVariableStatementSyntax forEachVariableStatementSyntax : break ;
                        case ForStatementSyntax forStatementSyntax : break ;
                        case FromClauseSyntax fromClauseSyntax : break ;
                        case GenericNameSyntax genericNameSyntax : break ;
                        case GlobalStatementSyntax globalStatementSyntax : break ;
                        case GotoStatementSyntax gotoStatementSyntax : break ;
                        case GroupClauseSyntax groupClauseSyntax : break ;
                        case IdentifierNameSyntax identifierNameSyntax : break ;
                        case IfDirectiveTriviaSyntax ifDirectiveTriviaSyntax : break ;
                        case IfStatementSyntax ifStatementSyntax : break ;
                        case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax : break ;
                        case ImplicitElementAccessSyntax implicitElementAccessSyntax : break ;
                        case ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpressionSyntax : break ;
                        case IncompleteMemberSyntax incompleteMemberSyntax : break ;
                        case IndexerDeclarationSyntax indexerDeclarationSyntax : break ;
                        case IndexerMemberCrefSyntax indexerMemberCrefSyntax : break ;
                        case InitializerExpressionSyntax initializerExpressionSyntax : break ;
                        case InterfaceDeclarationSyntax interfaceDeclarationSyntax : break ;
                        case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax : break ;
                        case InterpolatedStringTextSyntax interpolatedStringTextSyntax : break ;
                        case InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax : break ;
                        case InterpolationFormatClauseSyntax interpolationFormatClauseSyntax : break ;
                        case InterpolationSyntax interpolationSyntax : break ;
                        case InvocationExpressionSyntax invocationExpressionSyntax : break ;
                        case IsPatternExpressionSyntax isPatternExpressionSyntax : break ;
                        case JoinClauseSyntax joinClauseSyntax : break ;
                        case JoinIntoClauseSyntax joinIntoClauseSyntax : break ;
                        case LabeledStatementSyntax labeledStatementSyntax : break ;
                        case LetClauseSyntax letClauseSyntax : break ;
                        case LineDirectiveTriviaSyntax lineDirectiveTriviaSyntax : break ;
                        case LiteralExpressionSyntax literalExpressionSyntax : break ;
                        case LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax : break ;
                        case LocalDeclarationStatementSyntax localDeclarationStatementSyntax : break ;
                        case LocalFunctionStatementSyntax localFunctionStatementSyntax : break ;
                        case LockStatementSyntax lockStatementSyntax : break ;
                        case MakeRefExpressionSyntax makeRefExpressionSyntax : break ;
                        case MemberAccessExpressionSyntax memberAccessExpressionSyntax : break ;
                        case MemberBindingExpressionSyntax memberBindingExpressionSyntax : break ;
                        case MethodDeclarationSyntax methodDeclarationSyntax : break ;
                        case NameColonSyntax nameColonSyntax : break ;
                        case NameEqualsSyntax nameEqualsSyntax : break ;
                        case NameMemberCrefSyntax nameMemberCrefSyntax : break ;
                        case NamespaceDeclarationSyntax namespaceDeclarationSyntax : break ;
                        case NullableDirectiveTriviaSyntax nullableDirectiveTriviaSyntax : break ;
                        case NullableTypeSyntax nullableTypeSyntax : break ;
                        case ObjectCreationExpressionSyntax objectCreationExpressionSyntax : break ;
                        case OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax : break ;
                        case OmittedTypeArgumentSyntax omittedTypeArgumentSyntax : break ;
                        case OperatorDeclarationSyntax operatorDeclarationSyntax : break ;
                        case OperatorMemberCrefSyntax operatorMemberCrefSyntax : break ;
                        case OrderByClauseSyntax orderByClauseSyntax : break ;
                        case OrderingSyntax orderingSyntax : break ;
                        case ParameterListSyntax parameterListSyntax : break ;
                        case ParameterSyntax parameterSyntax : break ;
                        case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax : break ;
                        case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax : break ;
                        case ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax : break ;
                        case PointerTypeSyntax pointerTypeSyntax : break ;
                        case PositionalPatternClauseSyntax positionalPatternClauseSyntax : break ;
                        case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax : break ;
                        case PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax : break ;
                        case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax : break ;
                        case PredefinedTypeSyntax predefinedTypeSyntax : break ;
                        case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax : break ;
                        case PropertyDeclarationSyntax propertyDeclarationSyntax : break ;
                        case PropertyPatternClauseSyntax propertyPatternClauseSyntax : break ;
                        case QualifiedCrefSyntax qualifiedCrefSyntax : break ;
                        case QualifiedNameSyntax qualifiedNameSyntax : break ;
                        case QueryBodySyntax queryBodySyntax : break ;
                        case QueryContinuationSyntax queryContinuationSyntax : break ;
                        case QueryExpressionSyntax queryExpressionSyntax : break ;
                        case RangeExpressionSyntax rangeExpressionSyntax : break ;
                        case RecursivePatternSyntax recursivePatternSyntax : break ;
                        case ReferenceDirectiveTriviaSyntax referenceDirectiveTriviaSyntax : break ;
                        case RefExpressionSyntax refExpressionSyntax : break ;
                        case RefTypeExpressionSyntax refTypeExpressionSyntax : break ;
                        case RefTypeSyntax refTypeSyntax : break ;
                        case RefValueExpressionSyntax refValueExpressionSyntax : break ;
                        case RegionDirectiveTriviaSyntax regionDirectiveTriviaSyntax : break ;
                        case ReturnStatementSyntax returnStatementSyntax : break ;
                        case SelectClauseSyntax selectClauseSyntax : break ;
                        case ShebangDirectiveTriviaSyntax shebangDirectiveTriviaSyntax : break ;
                        case SimpleBaseTypeSyntax simpleBaseTypeSyntax : break ;
                        case SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax : break ;
                        case SingleVariableDesignationSyntax singleVariableDesignationSyntax : break ;
                        case SizeOfExpressionSyntax sizeOfExpressionSyntax : break ;
                        case SkippedTokensTriviaSyntax skippedTokensTriviaSyntax : break ;
                        case StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpressionSyntax : break ;
                        case StructDeclarationSyntax structDeclarationSyntax : break ;
                        case SubpatternSyntax subpatternSyntax : break ;
                        case SwitchExpressionArmSyntax switchExpressionArmSyntax : break ;
                        case SwitchExpressionSyntax switchExpressionSyntax : break ;
                        case SwitchSectionSyntax switchSectionSyntax : break ;
                        case SwitchStatementSyntax switchStatementSyntax : break ;
                        case ThisExpressionSyntax thisExpressionSyntax : break ;
                        case ThrowExpressionSyntax throwExpressionSyntax : break ;
                        case ThrowStatementSyntax throwStatementSyntax : break ;
                        case TryStatementSyntax tryStatementSyntax : break ;
                        case TupleElementSyntax tupleElementSyntax : break ;
                        case TupleExpressionSyntax tupleExpressionSyntax : break ;
                        case TupleTypeSyntax tupleTypeSyntax : break ;
                        case TypeArgumentListSyntax typeArgumentListSyntax : break ;
                        case TypeConstraintSyntax typeConstraintSyntax : break ;
                        case TypeCrefSyntax typeCrefSyntax : break ;
                        case TypeOfExpressionSyntax typeOfExpressionSyntax : break ;
                        case TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax : break ;
                        case TypeParameterListSyntax typeParameterListSyntax : break ;
                        case TypeParameterSyntax typeParameterSyntax : break ;
                        case UndefDirectiveTriviaSyntax undefDirectiveTriviaSyntax : break ;
                        case UnsafeStatementSyntax unsafeStatementSyntax : break ;
                        case UsingDirectiveSyntax usingDirectiveSyntax : break ;
                        case UsingStatementSyntax usingStatementSyntax : break ;
                        case VariableDeclarationSyntax variableDeclarationSyntax : break ;
                        case VariableDeclaratorSyntax variableDeclaratorSyntax : break ;
                        case VarPatternSyntax varPatternSyntax : break ;
                        case WarningDirectiveTriviaSyntax warningDirectiveTriviaSyntax : break ;
                        case WhenClauseSyntax whenClauseSyntax : break ;
                        case WhereClauseSyntax whereClauseSyntax : break ;
                        case WhileStatementSyntax whileStatementSyntax : break ;
                        case XmlCDataSectionSyntax xmlCDataSectionSyntax : break ;
                        case XmlCommentSyntax xmlCommentSyntax : break ;
                        case XmlCrefAttributeSyntax xmlCrefAttributeSyntax : break ;
                        case XmlElementEndTagSyntax xmlElementEndTagSyntax : break ;
                        case XmlElementStartTagSyntax xmlElementStartTagSyntax : break ;
                        case XmlElementSyntax xmlElementSyntax : break ;
                        case XmlEmptyElementSyntax xmlEmptyElementSyntax : break ;
                        case XmlNameAttributeSyntax xmlNameAttributeSyntax : break ;
                        case XmlNameSyntax xmlNameSyntax : break ;
                        case XmlPrefixSyntax xmlPrefixSyntax : break ;
                        case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax : break ;
                        case XmlTextAttributeSyntax xmlTextAttributeSyntax : break ;
                        case XmlTextSyntax xmlTextSyntax : break ;
                        case YieldStatementSyntax yieldStatementSyntax : break ;
                        case BaseArgumentListSyntax baseArgumentListSyntax : break ;
                        case BaseCrefParameterListSyntax baseCrefParameterListSyntax : break ;
                        case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax : break ;
                        case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax : break ;
                        case BaseParameterListSyntax baseParameterListSyntax : break ;
                        case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax : break ;
                        case BaseTypeSyntax baseTypeSyntax : break ;
                        case CommonForEachStatementSyntax commonForEachStatementSyntax : break ;
                        case ConditionalDirectiveTriviaSyntax conditionalDirectiveTriviaSyntax : break ;
                        case InstanceExpressionSyntax instanceExpressionSyntax : break ;
                        case InterpolatedStringContentSyntax interpolatedStringContentSyntax : break ;
                        case LambdaExpressionSyntax lambdaExpressionSyntax : break ;
                        case MemberCrefSyntax memberCrefSyntax : break ;
                        case PatternSyntax patternSyntax : break ;
                        case QueryClauseSyntax queryClauseSyntax : break ;
                        case SelectOrGroupClauseSyntax selectOrGroupClauseSyntax : break ;
                        case SimpleNameSyntax simpleNameSyntax : break ;
                        case SwitchLabelSyntax switchLabelSyntax : break ;
                        case TypeDeclarationSyntax typeDeclarationSyntax : break ;
                        case TypeParameterConstraintSyntax typeParameterConstraintSyntax : break ;
                        case VariableDesignationSyntax variableDesignationSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax xmlAttributeSyntax : break ;
                        case XmlNodeSyntax xmlNodeSyntax : break ;
                        case AnonymousFunctionExpressionSyntax anonymousFunctionExpressionSyntax : break ;
                        case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax : break ;
                        case BranchingDirectiveTriviaSyntax branchingDirectiveTriviaSyntax : break ;
                        case CrefSyntax crefSyntax : break ;
                        case NameSyntax nameSyntax : break ;
                        case StatementSyntax statementSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.DirectiveTriviaSyntax directiveTriviaSyntax : break ;
                        case MemberDeclarationSyntax memberDeclarationSyntax : break ;
                        case TypeSyntax typeSyntax : break ;
                        case ExpressionSyntax expressionSyntax : break ;
                        case Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax structuredTriviaSyntax : break ;
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