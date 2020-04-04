using System;

class CSharpSyntaxNode
{
}

class AccessorDeclarationSyntax : CSharpSyntaxNode
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Keyword
    {
        get;
        set;
    }
}

class AccessorListSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<AccessorDeclarationSyntax> Accessors
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class AliasQualifiedNameSyntax : NameSyntax
{
    public IdentifierNameSyntax Alias
    {
        get;
        set;
    }

    public SyntaxToken ColonColonToken
    {
        get;
        set;
    }

    public SimpleNameSyntax Name
    {
        get;
        set;
    }
}

class AnonymousFunctionExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken AsyncKeyword
    {
        get;
        set;
    }
}

class AnonymousMethodExpressionSyntax : AnonymousFunctionExpressionSyntax
{
    public SyntaxToken AsyncKeyword
    {
        get;
        set;
    }

    public SyntaxToken DelegateKeyword
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }

    public ExpressionSyntax ExpressionBody
    {
        get;
        set;
    }
}

class AnonymousObjectCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken NewKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<AnonymousObjectMemberDeclaratorSyntax> Initializers
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class AnonymousObjectMemberDeclaratorSyntax : CSharpSyntaxNode
{
    public NameEqualsSyntax NameEquals
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class ArgumentListSyntax : BaseArgumentListSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ArgumentSyntax> Arguments
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class ArgumentSyntax : CSharpSyntaxNode
{
    public NameColonSyntax NameColon
    {
        get;
        set;
    }

    public SyntaxToken RefKindKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class ArrayCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken NewKeyword
    {
        get;
        set;
    }

    public ArrayTypeSyntax Type
    {
        get;
        set;
    }

    public InitializerExpressionSyntax Initializer
    {
        get;
        set;
    }
}

class ArrayRankSpecifierSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ExpressionSyntax> Sizes
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }
}

class ArrayTypeSyntax : TypeSyntax
{
    public TypeSyntax ElementType
    {
        get;
        set;
    }

    public SyntaxList<ArrayRankSpecifierSyntax> RankSpecifiers
    {
        get;
        set;
    }
}

class ArrowExpressionClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken ArrowToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class AssignmentExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Left
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ExpressionSyntax Right
    {
        get;
        set;
    }
}

class AttributeArgumentListSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<AttributeArgumentSyntax> Arguments
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class AttributeArgumentSyntax : CSharpSyntaxNode
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class AttributeListSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public AttributeTargetSpecifierSyntax Target
    {
        get;
        set;
    }

    public SeparatedSyntaxList<AttributeSyntax> Attributes
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }
}

class AttributeSyntax : CSharpSyntaxNode
{
    public NameSyntax Name
    {
        get;
        set;
    }

    public AttributeArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class AttributeTargetSpecifierSyntax : CSharpSyntaxNode
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class AwaitExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class BadDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class BaseArgumentListSyntax : CSharpSyntaxNode
{
    public SeparatedSyntaxList<ArgumentSyntax> Arguments
    {
        get;
        set;
    }
}

class BaseCrefParameterListSyntax : CSharpSyntaxNode
{
    public SeparatedSyntaxList<CrefParameterSyntax> Parameters
    {
        get;
        set;
    }
}

class BaseExpressionSyntax : InstanceExpressionSyntax
{
    public SyntaxToken Token
    {
        get;
        set;
    }
}

class BaseFieldDeclarationSyntax : MemberDeclarationSyntax
{
    public VariableDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class BaseListSyntax : CSharpSyntaxNode
{
    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<BaseTypeSyntax> Types
    {
        get;
        set;
    }
}

class BaseMethodDeclarationSyntax : MemberDeclarationSyntax
{
    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }
}

class BaseParameterListSyntax : CSharpSyntaxNode
{
    public SeparatedSyntaxList<ParameterSyntax> Parameters
    {
        get;
        set;
    }
}

class BasePropertyDeclarationSyntax : MemberDeclarationSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
    {
        get;
        set;
    }

    public AccessorListSyntax AccessorList
    {
        get;
        set;
    }
}

class BaseTypeDeclarationSyntax : MemberDeclarationSyntax
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public BaseListSyntax BaseList
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class BaseTypeSyntax : CSharpSyntaxNode
{
    public TypeSyntax Type
    {
        get;
        set;
    }
}

class BinaryExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Left
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ExpressionSyntax Right
    {
        get;
        set;
    }
}

class BlockSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<StatementSyntax> Statements
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class BracketedArgumentListSyntax : BaseArgumentListSyntax
{
    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ArgumentSyntax> Arguments
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }
}

class BracketedParameterListSyntax : BaseParameterListSyntax
{
    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ParameterSyntax> Parameters
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }
}

class BranchingDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
}

class BreakStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken BreakKeyword
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class CasePatternSwitchLabelSyntax : SwitchLabelSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public PatternSyntax Pattern
    {
        get;
        set;
    }

    public WhenClauseSyntax WhenClause
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class CaseSwitchLabelSyntax : SwitchLabelSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public ExpressionSyntax Value
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class CastExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class CatchClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken CatchKeyword
    {
        get;
        set;
    }

    public CatchDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public CatchFilterClauseSyntax Filter
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }
}

class CatchDeclarationSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class CatchFilterClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken WhenKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax FilterExpression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class CheckedExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class CheckedStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }
}

class ClassDeclarationSyntax : TypeDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public BaseListSyntax BaseList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class ClassOrStructConstraintSyntax : TypeParameterConstraintSyntax
{
    public SyntaxToken ClassOrStructKeyword
    {
        get;
        set;
    }

    public SyntaxToken QuestionToken
    {
        get;
        set;
    }
}

class CommonForEachStatementSyntax : StatementSyntax
{
    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public SyntaxToken ForEachKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SyntaxToken InKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class CompilationUnitSyntax : CSharpSyntaxNode
{
    public SyntaxList<ExternAliasDirectiveSyntax> Externs
    {
        get;
        set;
    }

    public SyntaxList<UsingDirectiveSyntax> Usings
    {
        get;
        set;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken EndOfFileToken
    {
        get;
        set;
    }
}

class ConditionalAccessExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ExpressionSyntax WhenNotNull
    {
        get;
        set;
    }
}

class ConditionalDirectiveTriviaSyntax : BranchingDirectiveTriviaSyntax
{
    public ExpressionSyntax Condition
    {
        get;
        set;
    }
}

class ConditionalExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken QuestionToken
    {
        get;
        set;
    }

    public ExpressionSyntax WhenTrue
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public ExpressionSyntax WhenFalse
    {
        get;
        set;
    }
}

class ConstantPatternSyntax : PatternSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class ConstructorConstraintSyntax : TypeParameterConstraintSyntax
{
    public SyntaxToken NewKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class ConstructorDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public ConstructorInitializerSyntax Initializer
    {
        get;
        set;
    }
}

class ConstructorInitializerSyntax : CSharpSyntaxNode
{
    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public SyntaxToken ThisOrBaseKeyword
    {
        get;
        set;
    }

    public ArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class ContinueStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken ContinueKeyword
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class ConversionOperatorDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken ImplicitOrExplicitKeyword
    {
        get;
        set;
    }

    public SyntaxToken OperatorKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }
}

class ConversionOperatorMemberCrefSyntax : MemberCrefSyntax
{
    public SyntaxToken ImplicitOrExplicitKeyword
    {
        get;
        set;
    }

    public SyntaxToken OperatorKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public CrefParameterListSyntax Parameters
    {
        get;
        set;
    }
}

class CrefBracketedParameterListSyntax : BaseCrefParameterListSyntax
{
    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<CrefParameterSyntax> Parameters
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }
}

class CrefParameterListSyntax : BaseCrefParameterListSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<CrefParameterSyntax> Parameters
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class CrefParameterSyntax : CSharpSyntaxNode
{
    public SyntaxToken RefKindKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }
}

class CrefSyntax : CSharpSyntaxNode
{
}

class DeclarationExpressionSyntax : ExpressionSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public VariableDesignationSyntax Designation
    {
        get;
        set;
    }
}

class DeclarationPatternSyntax : PatternSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public VariableDesignationSyntax Designation
    {
        get;
        set;
    }
}

class DefaultExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class DefaultSwitchLabelSyntax : SwitchLabelSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class DefineDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken DefineKeyword
    {
        get;
        set;
    }

    public SyntaxToken Name
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class DelegateDeclarationSyntax : MemberDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken DelegateKeyword
    {
        get;
        set;
    }

    public TypeSyntax ReturnType
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class DestructorDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken TildeToken
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }
}

class DirectiveTriviaSyntax : StructuredTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class DiscardDesignationSyntax : VariableDesignationSyntax
{
    public SyntaxToken UnderscoreToken
    {
        get;
        set;
    }
}

class DiscardPatternSyntax : PatternSyntax
{
    public SyntaxToken UnderscoreToken
    {
        get;
        set;
    }
}

class DocumentationCommentTriviaSyntax : StructuredTriviaSyntax
{
    public SyntaxList<XmlNodeSyntax> Content
    {
        get;
        set;
    }

    public SyntaxToken EndOfComment
    {
        get;
        set;
    }
}

class DoStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken DoKeyword
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }

    public SyntaxToken WhileKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class ElementAccessExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public BracketedArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class ElementBindingExpressionSyntax : ExpressionSyntax
{
    public BracketedArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class ElifDirectiveTriviaSyntax : ConditionalDirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken ElifKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class ElseClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken ElseKeyword
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class ElseDirectiveTriviaSyntax : BranchingDirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken ElseKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class EmptyStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class EndIfDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken EndIfKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class EndRegionDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken EndRegionKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class EnumDeclarationSyntax : BaseTypeDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken EnumKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public BaseListSyntax BaseList
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<EnumMemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class EnumMemberDeclarationSyntax : MemberDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public EqualsValueClauseSyntax EqualsValue
    {
        get;
        set;
    }
}

class EqualsValueClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public ExpressionSyntax Value
    {
        get;
        set;
    }
}

class ErrorDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken ErrorKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class EventDeclarationSyntax : BasePropertyDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken EventKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class EventFieldDeclarationSyntax : BaseFieldDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken EventKeyword
    {
        get;
        set;
    }

    public VariableDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class ExplicitInterfaceSpecifierSyntax : CSharpSyntaxNode
{
    public NameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken DotToken
    {
        get;
        set;
    }
}

class ExpressionStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class ExpressionSyntax : CSharpSyntaxNode
{
}

class ExternAliasDirectiveSyntax : CSharpSyntaxNode
{
    public SyntaxToken ExternKeyword
    {
        get;
        set;
    }

    public SyntaxToken AliasKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class FieldDeclarationSyntax : BaseFieldDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public VariableDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class FinallyClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken FinallyKeyword
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }
}

class FixedStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken FixedKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public VariableDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class ForEachStatementSyntax : CommonForEachStatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public SyntaxToken ForEachKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken InKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class ForEachVariableStatementSyntax : CommonForEachStatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public SyntaxToken ForEachKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Variable
    {
        get;
        set;
    }

    public SyntaxToken InKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class ForStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken ForKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SyntaxToken FirstSemicolonToken
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken SecondSemicolonToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ExpressionSyntax> Incrementors
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class FromClauseSyntax : QueryClauseSyntax
{
    public SyntaxToken FromKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken InKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class GenericNameSyntax : SimpleNameSyntax
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeArgumentListSyntax TypeArgumentList
    {
        get;
        set;
    }
}

class GlobalStatementSyntax : MemberDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class GotoStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken GotoKeyword
    {
        get;
        set;
    }

    public SyntaxToken CaseOrDefaultKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class GroupClauseSyntax : SelectOrGroupClauseSyntax
{
    public SyntaxToken GroupKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax GroupExpression
    {
        get;
        set;
    }

    public SyntaxToken ByKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax ByExpression
    {
        get;
        set;
    }
}

class IdentifierNameSyntax : SimpleNameSyntax
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class IfDirectiveTriviaSyntax : ConditionalDirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken IfKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class IfStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken IfKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }

    public ElseClauseSyntax Else
    {
        get;
        set;
    }
}

class ImplicitArrayCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken NewKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SyntaxTokenList Commas
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }

    public InitializerExpressionSyntax Initializer
    {
        get;
        set;
    }
}

class ImplicitElementAccessSyntax : ExpressionSyntax
{
    public BracketedArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class ImplicitStackAllocArrayCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken StackAllocKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenBracketToken
    {
        get;
        set;
    }

    public SyntaxToken CloseBracketToken
    {
        get;
        set;
    }

    public InitializerExpressionSyntax Initializer
    {
        get;
        set;
    }
}

class IncompleteMemberSyntax : MemberDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }
}

class IndexerDeclarationSyntax : BasePropertyDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
    {
        get;
        set;
    }

    public SyntaxToken ThisKeyword
    {
        get;
        set;
    }

    public BracketedParameterListSyntax ParameterList
    {
        get;
        set;
    }
}

class IndexerMemberCrefSyntax : MemberCrefSyntax
{
    public SyntaxToken ThisKeyword
    {
        get;
        set;
    }

    public CrefBracketedParameterListSyntax Parameters
    {
        get;
        set;
    }
}

class InitializerExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ExpressionSyntax> Expressions
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class InstanceExpressionSyntax : ExpressionSyntax
{
}

class InterfaceDeclarationSyntax : TypeDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public BaseListSyntax BaseList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class InterpolatedStringContentSyntax : CSharpSyntaxNode
{
}

class InterpolatedStringExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken StringStartToken
    {
        get;
        set;
    }

    public SyntaxList<InterpolatedStringContentSyntax> Contents
    {
        get;
        set;
    }

    public SyntaxToken StringEndToken
    {
        get;
        set;
    }
}

class InterpolatedStringTextSyntax : InterpolatedStringContentSyntax
{
    public SyntaxToken TextToken
    {
        get;
        set;
    }
}

class InterpolationAlignmentClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken CommaToken
    {
        get;
        set;
    }

    public ExpressionSyntax Value
    {
        get;
        set;
    }
}

class InterpolationFormatClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public SyntaxToken FormatStringToken
    {
        get;
        set;
    }
}

class InterpolationSyntax : InterpolatedStringContentSyntax
{
    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public InterpolationAlignmentClauseSyntax AlignmentClause
    {
        get;
        set;
    }

    public InterpolationFormatClauseSyntax FormatClause
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class InvocationExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public ArgumentListSyntax ArgumentList
    {
        get;
        set;
    }
}

class IsPatternExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken IsKeyword
    {
        get;
        set;
    }

    public PatternSyntax Pattern
    {
        get;
        set;
    }
}

class JoinClauseSyntax : QueryClauseSyntax
{
    public SyntaxToken JoinKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken InKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax InExpression
    {
        get;
        set;
    }

    public SyntaxToken OnKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax LeftExpression
    {
        get;
        set;
    }

    public SyntaxToken EqualsKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax RightExpression
    {
        get;
        set;
    }

    public JoinIntoClauseSyntax Into
    {
        get;
        set;
    }
}

class JoinIntoClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken IntoKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class LabeledStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class LambdaExpressionSyntax : AnonymousFunctionExpressionSyntax
{
    public SyntaxToken ArrowToken
    {
        get;
        set;
    }
}

class LetClauseSyntax : QueryClauseSyntax
{
    public SyntaxToken LetKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class LineDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken LineKeyword
    {
        get;
        set;
    }

    public SyntaxToken Line
    {
        get;
        set;
    }

    public SyntaxToken File
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class LiteralExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Token
    {
        get;
        set;
    }
}

class LoadDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken LoadKeyword
    {
        get;
        set;
    }

    public SyntaxToken File
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class LocalDeclarationStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public SyntaxToken UsingKeyword
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public VariableDeclarationSyntax Declaration
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class LocalFunctionStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax ReturnType
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }
}

class LockStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken LockKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class MakeRefExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class MemberAccessExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public SimpleNameSyntax Name
    {
        get;
        set;
    }
}

class MemberBindingExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public SimpleNameSyntax Name
    {
        get;
        set;
    }
}

class MemberCrefSyntax : CrefSyntax
{
}

class MemberDeclarationSyntax : CSharpSyntaxNode
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }
}

class MethodDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax ReturnType
    {
        get;
        set;
    }

    public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }
}

class NameColonSyntax : CSharpSyntaxNode
{
    public IdentifierNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class NameEqualsSyntax : CSharpSyntaxNode
{
    public IdentifierNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }
}

class NameMemberCrefSyntax : MemberCrefSyntax
{
    public TypeSyntax Name
    {
        get;
        set;
    }

    public CrefParameterListSyntax Parameters
    {
        get;
        set;
    }
}

class NamespaceDeclarationSyntax : MemberDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken NamespaceKeyword
    {
        get;
        set;
    }

    public NameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<ExternAliasDirectiveSyntax> Externs
    {
        get;
        set;
    }

    public SyntaxList<UsingDirectiveSyntax> Usings
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class NameSyntax : TypeSyntax
{
}

class NullableDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken NullableKeyword
    {
        get;
        set;
    }

    public SyntaxToken SettingToken
    {
        get;
        set;
    }

    public SyntaxToken TargetToken
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class NullableTypeSyntax : TypeSyntax
{
    public TypeSyntax ElementType
    {
        get;
        set;
    }

    public SyntaxToken QuestionToken
    {
        get;
        set;
    }
}

class ObjectCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken NewKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public ArgumentListSyntax ArgumentList
    {
        get;
        set;
    }

    public InitializerExpressionSyntax Initializer
    {
        get;
        set;
    }
}

class OmittedArraySizeExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OmittedArraySizeExpressionToken
    {
        get;
        set;
    }
}

class OmittedTypeArgumentSyntax : TypeSyntax
{
    public SyntaxToken OmittedTypeArgumentToken
    {
        get;
        set;
    }
}

class OperatorDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax ReturnType
    {
        get;
        set;
    }

    public SyntaxToken OperatorKeyword
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }
}

class OperatorMemberCrefSyntax : MemberCrefSyntax
{
    public SyntaxToken OperatorKeyword
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public CrefParameterListSyntax Parameters
    {
        get;
        set;
    }
}

class OrderByClauseSyntax : QueryClauseSyntax
{
    public SyntaxToken OrderByKeyword
    {
        get;
        set;
    }

    public SeparatedSyntaxList<OrderingSyntax> Orderings
    {
        get;
        set;
    }
}

class OrderingSyntax : CSharpSyntaxNode
{
    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken AscendingOrDescendingKeyword
    {
        get;
        set;
    }
}

class ParameterListSyntax : BaseParameterListSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ParameterSyntax> Parameters
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class ParameterSyntax : CSharpSyntaxNode
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public EqualsValueClauseSyntax Default
    {
        get;
        set;
    }
}

class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class ParenthesizedLambdaExpressionSyntax : LambdaExpressionSyntax
{
    public SyntaxToken AsyncKeyword
    {
        get;
        set;
    }

    public ParameterListSyntax ParameterList
    {
        get;
        set;
    }

    public SyntaxToken ArrowToken
    {
        get;
        set;
    }
}

class ParenthesizedVariableDesignationSyntax : VariableDesignationSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<VariableDesignationSyntax> Variables
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class PatternSyntax : CSharpSyntaxNode
{
}

class PointerTypeSyntax : TypeSyntax
{
    public TypeSyntax ElementType
    {
        get;
        set;
    }

    public SyntaxToken AsteriskToken
    {
        get;
        set;
    }
}

class PositionalPatternClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<SubpatternSyntax> Subpatterns
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class PostfixUnaryExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Operand
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }
}

class PragmaChecksumDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken PragmaKeyword
    {
        get;
        set;
    }

    public SyntaxToken ChecksumKeyword
    {
        get;
        set;
    }

    public SyntaxToken File
    {
        get;
        set;
    }

    public SyntaxToken Guid
    {
        get;
        set;
    }

    public SyntaxToken Bytes
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class PragmaWarningDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken PragmaKeyword
    {
        get;
        set;
    }

    public SyntaxToken WarningKeyword
    {
        get;
        set;
    }

    public SyntaxToken DisableOrRestoreKeyword
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ExpressionSyntax> ErrorCodes
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class PredefinedTypeSyntax : TypeSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }
}

class PrefixUnaryExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ExpressionSyntax Operand
    {
        get;
        set;
    }
}

class PropertyDeclarationSyntax : BasePropertyDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class PropertyPatternClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<SubpatternSyntax> Subpatterns
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class QualifiedCrefSyntax : CrefSyntax
{
    public TypeSyntax Container
    {
        get;
        set;
    }

    public SyntaxToken DotToken
    {
        get;
        set;
    }

    public MemberCrefSyntax Member
    {
        get;
        set;
    }
}

class QualifiedNameSyntax : NameSyntax
{
    public NameSyntax Left
    {
        get;
        set;
    }

    public SyntaxToken DotToken
    {
        get;
        set;
    }

    public SimpleNameSyntax Right
    {
        get;
        set;
    }
}

class QueryBodySyntax : CSharpSyntaxNode
{
    public SyntaxList<QueryClauseSyntax> Clauses
    {
        get;
        set;
    }

    public SelectOrGroupClauseSyntax SelectOrGroup
    {
        get;
        set;
    }

    public QueryContinuationSyntax Continuation
    {
        get;
        set;
    }
}

class QueryClauseSyntax : CSharpSyntaxNode
{
}

class QueryContinuationSyntax : CSharpSyntaxNode
{
    public SyntaxToken IntoKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public QueryBodySyntax Body
    {
        get;
        set;
    }
}

class QueryExpressionSyntax : ExpressionSyntax
{
    public FromClauseSyntax FromClause
    {
        get;
        set;
    }

    public QueryBodySyntax Body
    {
        get;
        set;
    }
}

class RangeExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax LeftOperand
    {
        get;
        set;
    }

    public SyntaxToken OperatorToken
    {
        get;
        set;
    }

    public ExpressionSyntax RightOperand
    {
        get;
        set;
    }
}

class RecursivePatternSyntax : PatternSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public PositionalPatternClauseSyntax PositionalPatternClause
    {
        get;
        set;
    }

    public PropertyPatternClauseSyntax PropertyPatternClause
    {
        get;
        set;
    }

    public VariableDesignationSyntax Designation
    {
        get;
        set;
    }
}

class ReferenceDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken ReferenceKeyword
    {
        get;
        set;
    }

    public SyntaxToken File
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class RefExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken RefKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class RefTypeExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class RefTypeSyntax : TypeSyntax
{
    public SyntaxToken RefKeyword
    {
        get;
        set;
    }

    public SyntaxToken ReadOnlyKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }
}

class RefValueExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken Comma
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class RegionDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken RegionKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class ReturnStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken ReturnKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class SelectClauseSyntax : SelectOrGroupClauseSyntax
{
    public SyntaxToken SelectKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class SelectOrGroupClauseSyntax : CSharpSyntaxNode
{
}

class ShebangDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken ExclamationToken
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class SimpleBaseTypeSyntax : BaseTypeSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }
}

class SimpleLambdaExpressionSyntax : LambdaExpressionSyntax
{
    public SyntaxToken AsyncKeyword
    {
        get;
        set;
    }

    public ParameterSyntax Parameter
    {
        get;
        set;
    }

    public SyntaxToken ArrowToken
    {
        get;
        set;
    }
}

class SimpleNameSyntax : NameSyntax
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class SingleVariableDesignationSyntax : VariableDesignationSyntax
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class SizeOfExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class SkippedTokensTriviaSyntax : StructuredTriviaSyntax
{
    public SyntaxTokenList Tokens
    {
        get;
        set;
    }
}

class StackAllocArrayCreationExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken StackAllocKeyword
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public InitializerExpressionSyntax Initializer
    {
        get;
        set;
    }
}

class StatementSyntax : CSharpSyntaxNode
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }
}

class StructDeclarationSyntax : TypeDeclarationSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxTokenList Modifiers
    {
        get;
        set;
    }

    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public BaseListSyntax BaseList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class StructuredTriviaSyntax : CSharpSyntaxNode
{
}

class SubpatternSyntax : CSharpSyntaxNode
{
    public NameColonSyntax NameColon
    {
        get;
        set;
    }

    public PatternSyntax Pattern
    {
        get;
        set;
    }
}

class SwitchExpressionArmSyntax : CSharpSyntaxNode
{
    public PatternSyntax Pattern
    {
        get;
        set;
    }

    public WhenClauseSyntax WhenClause
    {
        get;
        set;
    }

    public SyntaxToken EqualsGreaterThanToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class SwitchExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax GoverningExpression
    {
        get;
        set;
    }

    public SyntaxToken SwitchKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<SwitchExpressionArmSyntax> Arms
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class SwitchLabelSyntax : CSharpSyntaxNode
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class SwitchSectionSyntax : CSharpSyntaxNode
{
    public SyntaxList<SwitchLabelSyntax> Labels
    {
        get;
        set;
    }

    public SyntaxList<StatementSyntax> Statements
    {
        get;
        set;
    }
}

class SwitchStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken SwitchKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public SyntaxToken OpenBraceToken
    {
        get;
        set;
    }

    public SyntaxList<SwitchSectionSyntax> Sections
    {
        get;
        set;
    }

    public SyntaxToken CloseBraceToken
    {
        get;
        set;
    }
}

class ThisExpressionSyntax : InstanceExpressionSyntax
{
    public SyntaxToken Token
    {
        get;
        set;
    }
}

class ThrowExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken ThrowKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }
}

class ThrowStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken ThrowKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class TryStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken TryKeyword
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }

    public SyntaxList<CatchClauseSyntax> Catches
    {
        get;
        set;
    }

    public FinallyClauseSyntax Finally
    {
        get;
        set;
    }
}

class TupleElementSyntax : CSharpSyntaxNode
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class TupleExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<ArgumentSyntax> Arguments
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class TupleTypeSyntax : TypeSyntax
{
    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<TupleElementSyntax> Elements
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class TypeArgumentListSyntax : CSharpSyntaxNode
{
    public SyntaxToken LessThanToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<TypeSyntax> Arguments
    {
        get;
        set;
    }

    public SyntaxToken GreaterThanToken
    {
        get;
        set;
    }
}

class TypeConstraintSyntax : TypeParameterConstraintSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }
}

class TypeCrefSyntax : CrefSyntax
{
    public TypeSyntax Type
    {
        get;
        set;
    }
}

class TypeDeclarationSyntax : BaseTypeDeclarationSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public TypeParameterListSyntax TypeParameterList
    {
        get;
        set;
    }

    public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
    {
        get;
        set;
    }

    public SyntaxList<MemberDeclarationSyntax> Members
    {
        get;
        set;
    }
}

class TypeOfExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken Keyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public TypeSyntax Type
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }
}

class TypeParameterConstraintClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken WhereKeyword
    {
        get;
        set;
    }

    public IdentifierNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<TypeParameterConstraintSyntax> Constraints
    {
        get;
        set;
    }
}

class TypeParameterConstraintSyntax : CSharpSyntaxNode
{
}

class TypeParameterListSyntax : CSharpSyntaxNode
{
    public SyntaxToken LessThanToken
    {
        get;
        set;
    }

    public SeparatedSyntaxList<TypeParameterSyntax> Parameters
    {
        get;
        set;
    }

    public SyntaxToken GreaterThanToken
    {
        get;
        set;
    }
}

class TypeParameterSyntax : CSharpSyntaxNode
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken VarianceKeyword
    {
        get;
        set;
    }

    public SyntaxToken Identifier
    {
        get;
        set;
    }
}

class TypeSyntax : ExpressionSyntax
{
}

class UndefDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken UndefKeyword
    {
        get;
        set;
    }

    public SyntaxToken Name
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class UnsafeStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken UnsafeKeyword
    {
        get;
        set;
    }

    public BlockSyntax Block
    {
        get;
        set;
    }
}

class UsingDirectiveSyntax : CSharpSyntaxNode
{
    public SyntaxToken UsingKeyword
    {
        get;
        set;
    }

    public NameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}

class UsingStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken AwaitKeyword
    {
        get;
        set;
    }

    public SyntaxToken UsingKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class VariableDeclarationSyntax : CSharpSyntaxNode
{
    public TypeSyntax Type
    {
        get;
        set;
    }

    public SeparatedSyntaxList<VariableDeclaratorSyntax> Variables
    {
        get;
        set;
    }
}

class VariableDeclaratorSyntax : CSharpSyntaxNode
{
    public SyntaxToken Identifier
    {
        get;
        set;
    }

    public BracketedArgumentListSyntax ArgumentList
    {
        get;
        set;
    }

    public EqualsValueClauseSyntax Initializer
    {
        get;
        set;
    }
}

class VariableDesignationSyntax : CSharpSyntaxNode
{
}

class VarPatternSyntax : PatternSyntax
{
    public SyntaxToken VarKeyword
    {
        get;
        set;
    }

    public VariableDesignationSyntax Designation
    {
        get;
        set;
    }
}

class WarningDirectiveTriviaSyntax : DirectiveTriviaSyntax
{
    public SyntaxToken HashToken
    {
        get;
        set;
    }

    public SyntaxToken WarningKeyword
    {
        get;
        set;
    }

    public SyntaxToken EndOfDirectiveToken
    {
        get;
        set;
    }
}

class WhenClauseSyntax : CSharpSyntaxNode
{
    public SyntaxToken WhenKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }
}

class WhereClauseSyntax : QueryClauseSyntax
{
    public SyntaxToken WhereKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }
}

class WhileStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken WhileKeyword
    {
        get;
        set;
    }

    public SyntaxToken OpenParenToken
    {
        get;
        set;
    }

    public ExpressionSyntax Condition
    {
        get;
        set;
    }

    public SyntaxToken CloseParenToken
    {
        get;
        set;
    }

    public StatementSyntax Statement
    {
        get;
        set;
    }
}

class XmlAttributeSyntax : CSharpSyntaxNode
{
    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public SyntaxToken StartQuoteToken
    {
        get;
        set;
    }

    public SyntaxToken EndQuoteToken
    {
        get;
        set;
    }
}

class XmlCDataSectionSyntax : XmlNodeSyntax
{
    public SyntaxToken StartCDataToken
    {
        get;
        set;
    }

    public SyntaxTokenList TextTokens
    {
        get;
        set;
    }

    public SyntaxToken EndCDataToken
    {
        get;
        set;
    }
}

class XmlCommentSyntax : XmlNodeSyntax
{
    public SyntaxToken LessThanExclamationMinusMinusToken
    {
        get;
        set;
    }

    public SyntaxTokenList TextTokens
    {
        get;
        set;
    }

    public SyntaxToken MinusMinusGreaterThanToken
    {
        get;
        set;
    }
}

class XmlCrefAttributeSyntax : XmlAttributeSyntax
{
    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public SyntaxToken StartQuoteToken
    {
        get;
        set;
    }

    public CrefSyntax Cref
    {
        get;
        set;
    }

    public SyntaxToken EndQuoteToken
    {
        get;
        set;
    }
}

class XmlElementEndTagSyntax : CSharpSyntaxNode
{
    public SyntaxToken LessThanSlashToken
    {
        get;
        set;
    }

    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken GreaterThanToken
    {
        get;
        set;
    }
}

class XmlElementStartTagSyntax : CSharpSyntaxNode
{
    public SyntaxToken LessThanToken
    {
        get;
        set;
    }

    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxList<XmlAttributeSyntax> Attributes
    {
        get;
        set;
    }

    public SyntaxToken GreaterThanToken
    {
        get;
        set;
    }
}

class XmlElementSyntax : XmlNodeSyntax
{
    public XmlElementStartTagSyntax StartTag
    {
        get;
        set;
    }

    public SyntaxList<XmlNodeSyntax> Content
    {
        get;
        set;
    }

    public XmlElementEndTagSyntax EndTag
    {
        get;
        set;
    }
}

class XmlEmptyElementSyntax : XmlNodeSyntax
{
    public SyntaxToken LessThanToken
    {
        get;
        set;
    }

    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxList<XmlAttributeSyntax> Attributes
    {
        get;
        set;
    }

    public SyntaxToken SlashGreaterThanToken
    {
        get;
        set;
    }
}

class XmlNameAttributeSyntax : XmlAttributeSyntax
{
    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public SyntaxToken StartQuoteToken
    {
        get;
        set;
    }

    public IdentifierNameSyntax Identifier
    {
        get;
        set;
    }

    public SyntaxToken EndQuoteToken
    {
        get;
        set;
    }
}

class XmlNameSyntax : CSharpSyntaxNode
{
    public XmlPrefixSyntax Prefix
    {
        get;
        set;
    }

    public SyntaxToken LocalName
    {
        get;
        set;
    }
}

class XmlNodeSyntax : CSharpSyntaxNode
{
}

class XmlPrefixSyntax : CSharpSyntaxNode
{
    public SyntaxToken Prefix
    {
        get;
        set;
    }

    public SyntaxToken ColonToken
    {
        get;
        set;
    }
}

class XmlProcessingInstructionSyntax : XmlNodeSyntax
{
    public SyntaxToken StartProcessingInstructionToken
    {
        get;
        set;
    }

    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxTokenList TextTokens
    {
        get;
        set;
    }

    public SyntaxToken EndProcessingInstructionToken
    {
        get;
        set;
    }
}

class XmlTextAttributeSyntax : XmlAttributeSyntax
{
    public XmlNameSyntax Name
    {
        get;
        set;
    }

    public SyntaxToken EqualsToken
    {
        get;
        set;
    }

    public SyntaxToken StartQuoteToken
    {
        get;
        set;
    }

    public SyntaxTokenList TextTokens
    {
        get;
        set;
    }

    public SyntaxToken EndQuoteToken
    {
        get;
        set;
    }
}

class XmlTextSyntax : XmlNodeSyntax
{
    public SyntaxTokenList TextTokens
    {
        get;
        set;
    }
}

class YieldStatementSyntax : StatementSyntax
{
    public SyntaxList<AttributeListSyntax> AttributeLists
    {
        get;
        set;
    }

    public SyntaxToken YieldKeyword
    {
        get;
        set;
    }

    public SyntaxToken ReturnOrBreakKeyword
    {
        get;
        set;
    }

    public ExpressionSyntax Expression
    {
        get;
        set;
    }

    public SyntaxToken SemicolonToken
    {
        get;
        set;
    }
}
