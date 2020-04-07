using System ;
using System.Collections.Generic ;

namespace FindLogUsages
{
    public class PocoSyntaxToken
    {
        public int RawKind { get ; set ; }

        public string Kind { get ; set ; }

        public object Value { get ; set ; }

        public string ValueText { get ; set ; }
    }


    public class PocoCSharpSyntaxNode
    {
    }

    public class PocoAccessorDeclarationSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoAccessorListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoAccessorDeclarationSyntax > Accessors { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoAliasQualifiedNameSyntax : PocoNameSyntax
    {
        public virtual PocoIdentifierNameSyntax Alias { get ; set ; }

        public virtual PocoSyntaxToken ColonColonToken { get ; set ; }

        public virtual PocoSimpleNameSyntax Name { get ; set ; }
    }

    public class PocoAnonymousFunctionExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken AsyncKeyword { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }

        public virtual PocoExpressionSyntax ExpressionBody { get ; set ; }
    }

    public class PocoAnonymousMethodExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        public override PocoSyntaxToken AsyncKeyword { get ; set ; }

        public virtual PocoSyntaxToken DelegateKeyword { get ; set ; }

        public virtual PocoParameterListSyntax ParameterList { get ; set ; }

        public override PocoBlockSyntax Block { get ; set ; }

        public override PocoExpressionSyntax ExpressionBody { get ; set ; }
    }

    public class PocoAnonymousObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken NewKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoAnonymousObjectMemberDeclaratorSyntax > Initializers
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoAnonymousObjectMemberDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoNameEqualsSyntax NameEquals { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public override List < PocoArgumentSyntax > Arguments { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoArgumentSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoNameColonSyntax NameColon { get ; set ; }

        public virtual PocoSyntaxToken RefKindKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken NewKeyword { get ; set ; }

        public virtual PocoArrayTypeSyntax Type { get ; set ; }

        public virtual PocoInitializerExpressionSyntax Initializer { get ; set ; }
    }

    public class PocoArrayRankSpecifierSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public virtual List < PocoExpressionSyntax > Sizes { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }
    }

    public class PocoArrayTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoTypeSyntax ElementType { get ; set ; }

        public virtual List < PocoArrayRankSpecifierSyntax > RankSpecifiers { get ; set ; }
    }

    public class PocoArrowExpressionClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ArrowToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoAssignmentExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Left { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoExpressionSyntax Right { get ; set ; }
    }

    public class PocoAttributeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual List < PocoAttributeArgumentSyntax > Arguments { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoAttributeArgumentSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoNameEqualsSyntax NameEquals { get ; set ; }

        public virtual PocoNameColonSyntax NameColon { get ; set ; }
    }

    public class PocoAttributeListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public virtual PocoAttributeTargetSpecifierSyntax Target { get ; set ; }

        public virtual List < PocoAttributeSyntax > Attributes { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }
    }

    public class PocoAttributeSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoNameSyntax Name { get ; set ; }

        public virtual PocoAttributeArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoAttributeTargetSpecifierSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoAwaitExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken AwaitKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoBadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoBaseArgumentListSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoArgumentSyntax > Arguments { get ; set ; }
    }

    public class PocoBaseCrefParameterListSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoCrefParameterSyntax > Parameters { get ; set ; }
    }

    public class PocoBaseExpressionSyntax : PocoInstanceExpressionSyntax
    {
        public virtual PocoSyntaxToken Token { get ; set ; }
    }

    public class PocoBaseFieldDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public virtual PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoBaseListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual List < PocoBaseTypeSyntax > Types { get ; set ; }
    }

    public class PocoBaseMethodDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public virtual PocoParameterListSyntax ParameterList { get ; set ; }

        public virtual PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoBaseParameterListSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoParameterSyntax > Parameters { get ; set ; }
    }

    public class PocoBasePropertyDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get ;
            set ;
        }

        public virtual PocoAccessorListSyntax AccessorList { get ; set ; }
    }

    public class PocoBaseTypeDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoBaseListSyntax BaseList { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoBaseTypeSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoBinaryExpressionSyntax : PocoExpressionSyntax
    {

        public PocoBinaryExpressionSyntax ( ) { }

        public PocoExpressionSyntax Left { get ; set ; }

        public PocoSyntaxToken OperatorToken { get ; set ; }

        public PocoExpressionSyntax Right { get ; set ; }
    }

    public class PocoBlockSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoStatementSyntax > Statements { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoBracketedArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public override List < PocoArgumentSyntax > Arguments { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }
    }

    public class PocoBracketedParameterListSyntax : PocoBaseParameterListSyntax
    {
        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public override List < PocoParameterSyntax > Parameters { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }
    }

    public class PocoBranchingDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public virtual bool BranchTaken { get ; set ; }
    }

    public class PocoBreakStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken BreakKeyword { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoCasePatternSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        public override PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoPatternSyntax Pattern { get ; set ; }

        public virtual PocoWhenClauseSyntax WhenClause { get ; set ; }

        public override PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoCaseSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        public override PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoExpressionSyntax Value { get ; set ; }

        public override PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoCastExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoCatchClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken CatchKeyword { get ; set ; }

        public virtual PocoCatchDeclarationSyntax Declaration { get ; set ; }

        public virtual PocoCatchFilterClauseSyntax Filter { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }
    }

    public class PocoCatchDeclarationSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoCatchFilterClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken WhenKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax FilterExpression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoCheckedExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoCheckedStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }
    }

    public class PocoClassDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoSyntaxToken Keyword { get ; set ; }

        public override PocoSyntaxToken Identifier { get ; set ; }

        public override PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public override PocoBaseListSyntax BaseList { get ; set ; }

        public override List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public override PocoSyntaxToken OpenBraceToken { get ; set ; }

        public override List < PocoMemberDeclarationSyntax > Members { get ; set ; }

        public override PocoSyntaxToken CloseBraceToken { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoClassOrStructConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        public virtual PocoSyntaxToken ClassOrStructKeyword { get ; set ; }

        public virtual PocoSyntaxToken QuestionToken { get ; set ; }
    }

    public class PocoCommonForEachStatementSyntax : PocoStatementSyntax
    {
        public virtual PocoSyntaxToken AwaitKeyword { get ; set ; }

        public virtual PocoSyntaxToken ForEachKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoSyntaxToken InKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoCompilationUnitSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoExternAliasDirectiveSyntax > Externs { get ; set ; }

        public virtual List < PocoUsingDirectiveSyntax > Usings { get ; set ; }

        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual List < PocoMemberDeclarationSyntax > Members { get ; set ; }

        public virtual PocoSyntaxToken EndOfFileToken { get ; set ; }
    }

    public class PocoConditionalAccessExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoExpressionSyntax WhenNotNull { get ; set ; }
    }

    public class PocoConditionalDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual bool ConditionValue { get ; set ; }
    }

    public class PocoConditionalExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual PocoSyntaxToken QuestionToken { get ; set ; }

        public virtual PocoExpressionSyntax WhenTrue { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual PocoExpressionSyntax WhenFalse { get ; set ; }
    }

    public class PocoConstantPatternSyntax : PocoPatternSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoConstructorConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        public virtual PocoSyntaxToken NewKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoConstructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoParameterListSyntax ParameterList { get ; set ; }

        public virtual PocoConstructorInitializerSyntax Initializer { get ; set ; }

        public override PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoConstructorInitializerSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual PocoSyntaxToken ThisOrBaseKeyword { get ; set ; }

        public virtual PocoArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoContinueStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken ContinueKeyword { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoConversionOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword { get ; set ; }

        public virtual PocoSyntaxToken OperatorKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public override PocoParameterListSyntax ParameterList { get ; set ; }

        public override PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoConversionOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword { get ; set ; }

        public virtual PocoSyntaxToken OperatorKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoCrefParameterListSyntax Parameters { get ; set ; }
    }

    public class PocoCrefBracketedParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public override List < PocoCrefParameterSyntax > Parameters { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }
    }

    public class PocoCrefParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public override List < PocoCrefParameterSyntax > Parameters { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoCrefParameterSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken RefKindKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoCrefSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoDeclarationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoVariableDesignationSyntax Designation { get ; set ; }
    }

    public class PocoDeclarationPatternSyntax : PocoPatternSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoVariableDesignationSyntax Designation { get ; set ; }
    }

    public class PocoDefaultExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoDefaultSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        public override PocoSyntaxToken Keyword { get ; set ; }

        public override PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoDefineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken DefineKeyword { get ; set ; }

        public virtual PocoSyntaxToken Name { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoDelegateDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken DelegateKeyword { get ; set ; }

        public virtual PocoTypeSyntax ReturnType { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public virtual PocoParameterListSyntax ParameterList { get ; set ; }

        public virtual List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoDestructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken TildeToken { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoParameterListSyntax ParameterList { get ; set ; }

        public override PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoDirectiveTriviaSyntax : PocoStructuredTriviaSyntax
    {
        public virtual bool IsActive { get ; set ; }

        public virtual PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken EndOfDirectiveToken { get ; set ; }
    }

    public class PocoDiscardDesignationSyntax : PocoVariableDesignationSyntax
    {
        public virtual PocoSyntaxToken UnderscoreToken { get ; set ; }
    }

    public class PocoDiscardPatternSyntax : PocoPatternSyntax
    {
        public virtual PocoSyntaxToken UnderscoreToken { get ; set ; }
    }

    public class PocoDocumentationCommentTriviaSyntax : PocoStructuredTriviaSyntax
    {
        public virtual List < PocoXmlNodeSyntax > Content { get ; set ; }

        public virtual PocoSyntaxToken EndOfComment { get ; set ; }
    }

    public class PocoDoStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken DoKeyword { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }

        public virtual PocoSyntaxToken WhileKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoElementAccessExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoBracketedArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoElementBindingExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoBracketedArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoElifDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken ElifKeyword { get ; set ; }

        public override PocoExpressionSyntax Condition { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }

        public override bool BranchTaken { get ; set ; }

        public override bool ConditionValue { get ; set ; }
    }

    public class PocoElseClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ElseKeyword { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoElseDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken ElseKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }

        public override bool BranchTaken { get ; set ; }
    }

    public class PocoEmptyStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoEndIfDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken EndIfKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoEndRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken EndRegionKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoEnumDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken EnumKeyword { get ; set ; }

        public override PocoSyntaxToken Identifier { get ; set ; }

        public override PocoBaseListSyntax BaseList { get ; set ; }

        public override PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoEnumMemberDeclarationSyntax > Members { get ; set ; }

        public override PocoSyntaxToken CloseBraceToken { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoEnumMemberDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoEqualsValueClauseSyntax EqualsValue { get ; set ; }
    }

    public class PocoEqualsValueClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken EqualsToken { get ; set ; }

        public virtual PocoExpressionSyntax Value { get ; set ; }
    }

    public class PocoErrorDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken ErrorKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoEventDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken EventKeyword { get ; set ; }

        public override PocoTypeSyntax Type { get ; set ; }

        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoAccessorListSyntax AccessorList { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoEventFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken EventKeyword { get ; set ; }

        public override PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoExplicitInterfaceSpecifierSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken DotToken { get ; set ; }
    }

    public class PocoExpressionStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoExpressionSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoExternAliasDirectiveSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ExternKeyword { get ; set ; }

        public virtual PocoSyntaxToken AliasKeyword { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoFinallyClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken FinallyKeyword { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }
    }

    public class PocoFixedStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken FixedKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoForEachStatementSyntax : PocoCommonForEachStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override PocoSyntaxToken AwaitKeyword { get ; set ; }

        public override PocoSyntaxToken ForEachKeyword { get ; set ; }

        public override PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoSyntaxToken InKeyword { get ; set ; }

        public override PocoExpressionSyntax Expression { get ; set ; }

        public override PocoSyntaxToken CloseParenToken { get ; set ; }

        public override PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoForEachVariableStatementSyntax : PocoCommonForEachStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override PocoSyntaxToken AwaitKeyword { get ; set ; }

        public override PocoSyntaxToken ForEachKeyword { get ; set ; }

        public override PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Variable { get ; set ; }

        public override PocoSyntaxToken InKeyword { get ; set ; }

        public override PocoExpressionSyntax Expression { get ; set ; }

        public override PocoSyntaxToken CloseParenToken { get ; set ; }

        public override PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoForStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken ForKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoSyntaxToken FirstSemicolonToken { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual PocoSyntaxToken SecondSemicolonToken { get ; set ; }

        public virtual List < PocoExpressionSyntax > Incrementors { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }

        public virtual PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public virtual List < PocoExpressionSyntax > Initializers { get ; set ; }
    }

    public class PocoFromClauseSyntax : PocoQueryClauseSyntax
    {
        public virtual PocoSyntaxToken FromKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken InKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoGenericNameSyntax : PocoSimpleNameSyntax
    {
        public override PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoTypeArgumentListSyntax TypeArgumentList { get ; set ; }
    }

    public class PocoGlobalStatementSyntax : PocoMemberDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoGotoStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken GotoKeyword { get ; set ; }

        public virtual PocoSyntaxToken CaseOrDefaultKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoGroupClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        public virtual PocoSyntaxToken GroupKeyword { get ; set ; }

        public virtual PocoExpressionSyntax GroupExpression { get ; set ; }

        public virtual PocoSyntaxToken ByKeyword { get ; set ; }

        public virtual PocoExpressionSyntax ByExpression { get ; set ; }
    }

    public class PocoIdentifierNameSyntax : PocoSimpleNameSyntax
    {
        public override PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoIfDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken IfKeyword { get ; set ; }

        public override PocoExpressionSyntax Condition { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }

        public override bool BranchTaken { get ; set ; }

        public override bool ConditionValue { get ; set ; }
    }

    public class PocoIfStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken IfKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }

        public virtual PocoElseClauseSyntax Else { get ; set ; }
    }

    public class PocoImplicitArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken NewKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public virtual List < PocoSyntaxToken > Commas { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }

        public virtual PocoInitializerExpressionSyntax Initializer { get ; set ; }
    }

    public class PocoImplicitElementAccessSyntax : PocoExpressionSyntax
    {
        public virtual PocoBracketedArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoImplicitStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken StackAllocKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenBracketToken { get ; set ; }

        public virtual PocoSyntaxToken CloseBracketToken { get ; set ; }

        public virtual PocoInitializerExpressionSyntax Initializer { get ; set ; }
    }

    public class PocoIncompleteMemberSyntax : PocoMemberDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoIndexerDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoTypeSyntax Type { get ; set ; }

        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken ThisKeyword { get ; set ; }

        public virtual PocoBracketedParameterListSyntax ParameterList { get ; set ; }

        public override PocoAccessorListSyntax AccessorList { get ; set ; }
    }

    public class PocoIndexerMemberCrefSyntax : PocoMemberCrefSyntax
    {
        public virtual PocoSyntaxToken ThisKeyword { get ; set ; }

        public virtual PocoCrefBracketedParameterListSyntax Parameters { get ; set ; }
    }

    public class PocoInitializerExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoExpressionSyntax > Expressions { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoInstanceExpressionSyntax : PocoExpressionSyntax
    {
    }

    public class PocoInterfaceDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoSyntaxToken Keyword { get ; set ; }

        public override PocoSyntaxToken Identifier { get ; set ; }

        public override PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public override PocoBaseListSyntax BaseList { get ; set ; }

        public override List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public override PocoSyntaxToken OpenBraceToken { get ; set ; }

        public override List < PocoMemberDeclarationSyntax > Members { get ; set ; }

        public override PocoSyntaxToken CloseBraceToken { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoInterpolatedStringContentSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoInterpolatedStringExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken StringStartToken { get ; set ; }

        public virtual List < PocoInterpolatedStringContentSyntax > Contents { get ; set ; }

        public virtual PocoSyntaxToken StringEndToken { get ; set ; }
    }

    public class PocoInterpolatedStringTextSyntax : PocoInterpolatedStringContentSyntax
    {
        public virtual PocoSyntaxToken TextToken { get ; set ; }
    }

    public class PocoInterpolationAlignmentClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken CommaToken { get ; set ; }

        public virtual PocoExpressionSyntax Value { get ; set ; }
    }

    public class PocoInterpolationFormatClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual PocoSyntaxToken FormatStringToken { get ; set ; }
    }

    public class PocoInterpolationSyntax : PocoInterpolatedStringContentSyntax
    {
        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoInterpolationAlignmentClauseSyntax AlignmentClause { get ; set ; }

        public virtual PocoInterpolationFormatClauseSyntax FormatClause { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoInvocationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoArgumentListSyntax ArgumentList { get ; set ; }
    }

    public class PocoIsPatternExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken IsKeyword { get ; set ; }

        public virtual PocoPatternSyntax Pattern { get ; set ; }
    }

    public class PocoJoinClauseSyntax : PocoQueryClauseSyntax
    {
        public virtual PocoSyntaxToken JoinKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken InKeyword { get ; set ; }

        public virtual PocoExpressionSyntax InExpression { get ; set ; }

        public virtual PocoSyntaxToken OnKeyword { get ; set ; }

        public virtual PocoExpressionSyntax LeftExpression { get ; set ; }

        public virtual PocoSyntaxToken EqualsKeyword { get ; set ; }

        public virtual PocoExpressionSyntax RightExpression { get ; set ; }

        public virtual PocoJoinIntoClauseSyntax Into { get ; set ; }
    }

    public class PocoJoinIntoClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken IntoKeyword { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoLabeledStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoLambdaExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        public virtual PocoSyntaxToken ArrowToken { get ; set ; }
    }

    public class PocoLetClauseSyntax : PocoQueryClauseSyntax
    {
        public virtual PocoSyntaxToken LetKeyword { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoSyntaxToken EqualsToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoLineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken LineKeyword { get ; set ; }

        public virtual PocoSyntaxToken Line { get ; set ; }

        public virtual PocoSyntaxToken File { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoLiteralExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Token { get ; set ; }
    }

    public class PocoLoadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken LoadKeyword { get ; set ; }

        public virtual PocoSyntaxToken File { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoLocalDeclarationStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken AwaitKeyword { get ; set ; }

        public virtual PocoSyntaxToken UsingKeyword { get ; set ; }

        public virtual List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoLocalFunctionStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoTypeSyntax ReturnType { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public virtual PocoParameterListSyntax ParameterList { get ; set ; }

        public virtual List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public virtual PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoLockStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken LockKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoMakeRefExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoMemberAccessExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoSimpleNameSyntax Name { get ; set ; }
    }

    public class PocoMemberBindingExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoSimpleNameSyntax Name { get ; set ; }
    }

    public class PocoMemberCrefSyntax : PocoCrefSyntax
    {
    }

    public class PocoMemberDeclarationSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual List < PocoSyntaxToken > Modifiers { get ; set ; }
    }

    public class PocoMethodDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoTypeSyntax ReturnType { get ; set ; }

        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public override PocoParameterListSyntax ParameterList { get ; set ; }

        public virtual List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public override PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoNameColonSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoIdentifierNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoNameEqualsSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoIdentifierNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken EqualsToken { get ; set ; }
    }

    public class PocoNameMemberCrefSyntax : PocoMemberCrefSyntax
    {
        public virtual PocoTypeSyntax Name { get ; set ; }

        public virtual PocoCrefParameterListSyntax Parameters { get ; set ; }
    }

    public class PocoNamespaceDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoSyntaxToken NamespaceKeyword { get ; set ; }

        public virtual PocoNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoExternAliasDirectiveSyntax > Externs { get ; set ; }

        public virtual List < PocoUsingDirectiveSyntax > Usings { get ; set ; }

        public virtual List < PocoMemberDeclarationSyntax > Members { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoNameSyntax : PocoTypeSyntax
    {
    }

    public class PocoNullableDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken NullableKeyword { get ; set ; }

        public virtual PocoSyntaxToken SettingToken { get ; set ; }

        public virtual PocoSyntaxToken TargetToken { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoNullableTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoTypeSyntax ElementType { get ; set ; }

        public virtual PocoSyntaxToken QuestionToken { get ; set ; }
    }

    public class PocoObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken NewKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoArgumentListSyntax ArgumentList { get ; set ; }

        public virtual PocoInitializerExpressionSyntax Initializer { get ; set ; }
    }

    public class PocoOmittedArraySizeExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OmittedArraySizeExpressionToken { get ; set ; }
    }

    public class PocoOmittedTypeArgumentSyntax : PocoTypeSyntax
    {
        public virtual PocoSyntaxToken OmittedTypeArgumentToken { get ; set ; }
    }

    public class PocoOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoTypeSyntax ReturnType { get ; set ; }

        public virtual PocoSyntaxToken OperatorKeyword { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public override PocoParameterListSyntax ParameterList { get ; set ; }

        public override PocoBlockSyntax Body { get ; set ; }
    }

    public class PocoOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        public virtual PocoSyntaxToken OperatorKeyword { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoCrefParameterListSyntax Parameters { get ; set ; }
    }

    public class PocoOrderByClauseSyntax : PocoQueryClauseSyntax
    {
        public virtual PocoSyntaxToken OrderByKeyword { get ; set ; }

        public virtual List < PocoOrderingSyntax > Orderings { get ; set ; }
    }

    public class PocoOrderingSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken AscendingOrDescendingKeyword { get ; set ; }
    }

    public class PocoParameterListSyntax : PocoBaseParameterListSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public override List < PocoParameterSyntax > Parameters { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoParameterSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual List < PocoSyntaxToken > Modifiers { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoEqualsValueClauseSyntax Default { get ; set ; }
    }

    public class PocoParenthesizedExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoParenthesizedLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        public override PocoSyntaxToken AsyncKeyword { get ; set ; }

        public virtual PocoParameterListSyntax ParameterList { get ; set ; }

        public override PocoSyntaxToken ArrowToken { get ; set ; }

        public override PocoBlockSyntax Block { get ; set ; }

        public override PocoExpressionSyntax ExpressionBody { get ; set ; }
    }

    public class PocoParenthesizedVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual List < PocoVariableDesignationSyntax > Variables { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoPatternSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoPointerTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoTypeSyntax ElementType { get ; set ; }

        public virtual PocoSyntaxToken AsteriskToken { get ; set ; }
    }

    public class PocoPositionalPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual List < PocoSubpatternSyntax > Subpatterns { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoPostfixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax Operand { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }
    }

    public class PocoPragmaChecksumDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken PragmaKeyword { get ; set ; }

        public virtual PocoSyntaxToken ChecksumKeyword { get ; set ; }

        public virtual PocoSyntaxToken File { get ; set ; }

        public virtual PocoSyntaxToken Guid { get ; set ; }

        public virtual PocoSyntaxToken Bytes { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoPragmaWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken PragmaKeyword { get ; set ; }

        public virtual PocoSyntaxToken WarningKeyword { get ; set ; }

        public virtual PocoSyntaxToken DisableOrRestoreKeyword { get ; set ; }

        public virtual List < PocoExpressionSyntax > ErrorCodes { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoPredefinedTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }
    }

    public class PocoPrefixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoExpressionSyntax Operand { get ; set ; }
    }

    public class PocoPropertyDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoTypeSyntax Type { get ; set ; }

        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get ;
            set ;
        }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public override PocoAccessorListSyntax AccessorList { get ; set ; }
    }

    public class PocoPropertyPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoSubpatternSyntax > Subpatterns { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoQualifiedCrefSyntax : PocoCrefSyntax
    {
        public virtual PocoTypeSyntax Container { get ; set ; }

        public virtual PocoSyntaxToken DotToken { get ; set ; }

        public virtual PocoMemberCrefSyntax Member { get ; set ; }
    }

    public class PocoQualifiedNameSyntax : PocoNameSyntax
    {
        public virtual PocoNameSyntax Left { get ; set ; }

        public virtual PocoSyntaxToken DotToken { get ; set ; }

        public virtual PocoSimpleNameSyntax Right { get ; set ; }
    }

    public class PocoQueryBodySyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoQueryClauseSyntax > Clauses { get ; set ; }

        public virtual PocoSelectOrGroupClauseSyntax SelectOrGroup { get ; set ; }

        public virtual PocoQueryContinuationSyntax Continuation { get ; set ; }
    }

    public class PocoQueryClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoQueryContinuationSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken IntoKeyword { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoQueryBodySyntax Body { get ; set ; }
    }

    public class PocoQueryExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoFromClauseSyntax FromClause { get ; set ; }

        public virtual PocoQueryBodySyntax Body { get ; set ; }
    }

    public class PocoRangeExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax LeftOperand { get ; set ; }

        public virtual PocoSyntaxToken OperatorToken { get ; set ; }

        public virtual PocoExpressionSyntax RightOperand { get ; set ; }
    }

    public class PocoRecursivePatternSyntax : PocoPatternSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoPositionalPatternClauseSyntax PositionalPatternClause { get ; set ; }

        public virtual PocoPropertyPatternClauseSyntax PropertyPatternClause { get ; set ; }

        public virtual PocoVariableDesignationSyntax Designation { get ; set ; }
    }

    public class PocoReferenceDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken ReferenceKeyword { get ; set ; }

        public virtual PocoSyntaxToken File { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoRefExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken RefKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoRefTypeExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoRefTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoSyntaxToken RefKeyword { get ; set ; }

        public virtual PocoSyntaxToken ReadOnlyKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoRefValueExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken Comma { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken RegionKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoReturnStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken ReturnKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoSelectClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        public virtual PocoSyntaxToken SelectKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoSelectOrGroupClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoShebangDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken ExclamationToken { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoSimpleBaseTypeSyntax : PocoBaseTypeSyntax
    {
        public override PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoSimpleLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        public override PocoSyntaxToken AsyncKeyword { get ; set ; }

        public virtual PocoParameterSyntax Parameter { get ; set ; }

        public override PocoSyntaxToken ArrowToken { get ; set ; }

        public override PocoBlockSyntax Block { get ; set ; }

        public override PocoExpressionSyntax ExpressionBody { get ; set ; }
    }

    public class PocoSimpleNameSyntax : PocoNameSyntax
    {
        public virtual PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoSingleVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        public virtual PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoSizeOfExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoSkippedTokensTriviaSyntax : PocoStructuredTriviaSyntax
    {
        public virtual List < PocoSyntaxToken > Tokens { get ; set ; }
    }

    public class PocoStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken StackAllocKeyword { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoInitializerExpressionSyntax Initializer { get ; set ; }
    }

    public class PocoStatementSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }
    }

    public class PocoStructDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public override List < PocoSyntaxToken > Modifiers { get ; set ; }

        public override PocoSyntaxToken Keyword { get ; set ; }

        public override PocoSyntaxToken Identifier { get ; set ; }

        public override PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public override PocoBaseListSyntax BaseList { get ; set ; }

        public override List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public override PocoSyntaxToken OpenBraceToken { get ; set ; }

        public override List < PocoMemberDeclarationSyntax > Members { get ; set ; }

        public override PocoSyntaxToken CloseBraceToken { get ; set ; }

        public override PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoStructuredTriviaSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoSubpatternSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoNameColonSyntax NameColon { get ; set ; }

        public virtual PocoPatternSyntax Pattern { get ; set ; }
    }

    public class PocoSwitchExpressionArmSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoPatternSyntax Pattern { get ; set ; }

        public virtual PocoWhenClauseSyntax WhenClause { get ; set ; }

        public virtual PocoSyntaxToken EqualsGreaterThanToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoSwitchExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoExpressionSyntax GoverningExpression { get ; set ; }

        public virtual PocoSyntaxToken SwitchKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoSwitchExpressionArmSyntax > Arms { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoSwitchLabelSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoSwitchSectionSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoSwitchLabelSyntax > Labels { get ; set ; }

        public virtual List < PocoStatementSyntax > Statements { get ; set ; }
    }

    public class PocoSwitchStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken SwitchKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoSyntaxToken OpenBraceToken { get ; set ; }

        public virtual List < PocoSwitchSectionSyntax > Sections { get ; set ; }

        public virtual PocoSyntaxToken CloseBraceToken { get ; set ; }
    }

    public class PocoThisExpressionSyntax : PocoInstanceExpressionSyntax
    {
        public virtual PocoSyntaxToken Token { get ; set ; }
    }

    public class PocoThrowExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken ThrowKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoThrowStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken ThrowKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }

    public class PocoTryStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken TryKeyword { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }

        public virtual List < PocoCatchClauseSyntax > Catches { get ; set ; }

        public virtual PocoFinallyClauseSyntax Finally { get ; set ; }
    }

    public class PocoTupleElementSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoTupleExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual List < PocoArgumentSyntax > Arguments { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoTupleTypeSyntax : PocoTypeSyntax
    {
        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual List < PocoTupleElementSyntax > Elements { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoTypeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken LessThanToken { get ; set ; }

        public virtual List < PocoTypeSyntax > Arguments { get ; set ; }

        public virtual PocoSyntaxToken GreaterThanToken { get ; set ; }
    }

    public class PocoTypeConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoTypeCrefSyntax : PocoCrefSyntax
    {
        public virtual PocoTypeSyntax Type { get ; set ; }
    }

    public class PocoTypeDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoTypeParameterListSyntax TypeParameterList { get ; set ; }

        public virtual List < PocoTypeParameterConstraintClauseSyntax > ConstraintClauses
        {
            get ;
            set ;
        }

        public virtual List < PocoMemberDeclarationSyntax > Members { get ; set ; }
    }

    public class PocoTypeOfExpressionSyntax : PocoExpressionSyntax
    {
        public virtual PocoSyntaxToken Keyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }
    }

    public class PocoTypeParameterConstraintClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken WhereKeyword { get ; set ; }

        public virtual PocoIdentifierNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }

        public virtual List < PocoTypeParameterConstraintSyntax > Constraints { get ; set ; }
    }

    public class PocoTypeParameterConstraintSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoTypeParameterListSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken LessThanToken { get ; set ; }

        public virtual List < PocoTypeParameterSyntax > Parameters { get ; set ; }

        public virtual PocoSyntaxToken GreaterThanToken { get ; set ; }
    }

    public class PocoTypeParameterSyntax : PocoCSharpSyntaxNode
    {
        public virtual List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken VarianceKeyword { get ; set ; }

        public virtual PocoSyntaxToken Identifier { get ; set ; }
    }

    public class PocoTypeSyntax : PocoExpressionSyntax
    {
    }

    public class PocoUndefDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken UndefKeyword { get ; set ; }

        public virtual PocoSyntaxToken Name { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoUnsafeStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken UnsafeKeyword { get ; set ; }

        public virtual PocoBlockSyntax Block { get ; set ; }
    }

    public class PocoUsingDirectiveSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken UsingKeyword { get ; set ; }

        public virtual PocoNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }

        public virtual PocoSyntaxToken StaticKeyword { get ; set ; }

        public virtual PocoNameEqualsSyntax Alias { get ; set ; }
    }

    public class PocoUsingStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken AwaitKeyword { get ; set ; }

        public virtual PocoSyntaxToken UsingKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }

        public virtual PocoVariableDeclarationSyntax Declaration { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }
    }

    public class PocoVariableDeclarationSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoTypeSyntax Type { get ; set ; }

        public virtual List < PocoVariableDeclaratorSyntax > Variables { get ; set ; }
    }

    public class PocoVariableDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken Identifier { get ; set ; }

        public virtual PocoBracketedArgumentListSyntax ArgumentList { get ; set ; }

        public virtual PocoEqualsValueClauseSyntax Initializer { get ; set ; }
    }

    public class PocoVariableDesignationSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoVarPatternSyntax : PocoPatternSyntax
    {
        public virtual PocoSyntaxToken VarKeyword { get ; set ; }

        public virtual PocoVariableDesignationSyntax Designation { get ; set ; }
    }

    public class PocoWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        public override PocoSyntaxToken HashToken { get ; set ; }

        public virtual PocoSyntaxToken WarningKeyword { get ; set ; }

        public override PocoSyntaxToken EndOfDirectiveToken { get ; set ; }

        public override bool IsActive { get ; set ; }
    }

    public class PocoWhenClauseSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken WhenKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }
    }

    public class PocoWhereClauseSyntax : PocoQueryClauseSyntax
    {
        public virtual PocoSyntaxToken WhereKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }
    }

    public class PocoWhileStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken WhileKeyword { get ; set ; }

        public virtual PocoSyntaxToken OpenParenToken { get ; set ; }

        public virtual PocoExpressionSyntax Condition { get ; set ; }

        public virtual PocoSyntaxToken CloseParenToken { get ; set ; }

        public virtual PocoStatementSyntax Statement { get ; set ; }
    }

    public class PocoXmlAttributeSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoXmlNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken EqualsToken { get ; set ; }

        public virtual PocoSyntaxToken StartQuoteToken { get ; set ; }

        public virtual PocoSyntaxToken EndQuoteToken { get ; set ; }
    }

    public class PocoXmlCDataSectionSyntax : PocoXmlNodeSyntax
    {
        public virtual PocoSyntaxToken StartCDataToken { get ; set ; }

        public virtual List < PocoSyntaxToken > TextTokens { get ; set ; }

        public virtual PocoSyntaxToken EndCDataToken { get ; set ; }
    }

    public class PocoXmlCommentSyntax : PocoXmlNodeSyntax
    {
        public virtual PocoSyntaxToken LessThanExclamationMinusMinusToken { get ; set ; }

        public virtual List < PocoSyntaxToken > TextTokens { get ; set ; }

        public virtual PocoSyntaxToken MinusMinusGreaterThanToken { get ; set ; }
    }

    public class PocoXmlCrefAttributeSyntax : PocoXmlAttributeSyntax
    {
        public override PocoXmlNameSyntax Name { get ; set ; }

        public override PocoSyntaxToken EqualsToken { get ; set ; }

        public override PocoSyntaxToken StartQuoteToken { get ; set ; }

        public virtual PocoCrefSyntax Cref { get ; set ; }

        public override PocoSyntaxToken EndQuoteToken { get ; set ; }
    }

    public class PocoXmlElementEndTagSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken LessThanSlashToken { get ; set ; }

        public virtual PocoXmlNameSyntax Name { get ; set ; }

        public virtual PocoSyntaxToken GreaterThanToken { get ; set ; }
    }

    public class PocoXmlElementStartTagSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken LessThanToken { get ; set ; }

        public virtual PocoXmlNameSyntax Name { get ; set ; }

        public virtual List < PocoXmlAttributeSyntax > Attributes { get ; set ; }

        public virtual PocoSyntaxToken GreaterThanToken { get ; set ; }
    }

    public class PocoXmlElementSyntax : PocoXmlNodeSyntax
    {
        public virtual PocoXmlElementStartTagSyntax StartTag { get ; set ; }

        public virtual List < PocoXmlNodeSyntax > Content { get ; set ; }

        public virtual PocoXmlElementEndTagSyntax EndTag { get ; set ; }
    }

    public class PocoXmlEmptyElementSyntax : PocoXmlNodeSyntax
    {
        public virtual PocoSyntaxToken LessThanToken { get ; set ; }

        public virtual PocoXmlNameSyntax Name { get ; set ; }

        public virtual List < PocoXmlAttributeSyntax > Attributes { get ; set ; }

        public virtual PocoSyntaxToken SlashGreaterThanToken { get ; set ; }
    }

    public class PocoXmlNameAttributeSyntax : PocoXmlAttributeSyntax
    {
        public override PocoXmlNameSyntax Name { get ; set ; }

        public override PocoSyntaxToken EqualsToken { get ; set ; }

        public override PocoSyntaxToken StartQuoteToken { get ; set ; }

        public virtual PocoIdentifierNameSyntax Identifier { get ; set ; }

        public override PocoSyntaxToken EndQuoteToken { get ; set ; }
    }

    public class PocoXmlNameSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoXmlPrefixSyntax Prefix { get ; set ; }

        public virtual PocoSyntaxToken LocalName { get ; set ; }
    }

    public class PocoXmlNodeSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoXmlPrefixSyntax : PocoCSharpSyntaxNode
    {
        public virtual PocoSyntaxToken Prefix { get ; set ; }

        public virtual PocoSyntaxToken ColonToken { get ; set ; }
    }

    public class PocoXmlProcessingInstructionSyntax : PocoXmlNodeSyntax
    {
        public virtual PocoSyntaxToken StartProcessingInstructionToken { get ; set ; }

        public virtual PocoXmlNameSyntax Name { get ; set ; }

        public virtual List < PocoSyntaxToken > TextTokens { get ; set ; }

        public virtual PocoSyntaxToken EndProcessingInstructionToken { get ; set ; }
    }

    public class PocoXmlTextAttributeSyntax : PocoXmlAttributeSyntax
    {
        public override PocoXmlNameSyntax Name { get ; set ; }

        public override PocoSyntaxToken EqualsToken { get ; set ; }

        public override PocoSyntaxToken StartQuoteToken { get ; set ; }

        public virtual List < PocoSyntaxToken > TextTokens { get ; set ; }

        public override PocoSyntaxToken EndQuoteToken { get ; set ; }
    }

    public class PocoXmlTextSyntax : PocoXmlNodeSyntax
    {
        public virtual List < PocoSyntaxToken > TextTokens { get ; set ; }
    }

    public class PocoYieldStatementSyntax : PocoStatementSyntax
    {
        public override List < PocoAttributeListSyntax > AttributeLists { get ; set ; }

        public virtual PocoSyntaxToken YieldKeyword { get ; set ; }

        public virtual PocoSyntaxToken ReturnOrBreakKeyword { get ; set ; }

        public virtual PocoExpressionSyntax Expression { get ; set ; }

        public virtual PocoSyntaxToken SemicolonToken { get ; set ; }
    }
}
