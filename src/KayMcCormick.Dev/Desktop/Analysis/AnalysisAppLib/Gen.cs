using System ;
using System.Collections.Generic ;
using System.Text.Json.Serialization ;
using System.ComponentModel ;

namespace PocoSyntax
{
    public class PocoSyntaxToken
    {
        public int RawKind { get ; set ; }

        public string Kind { get ; set ; }

        [ JsonIgnore ]
        public object Value { get ; set ; }

        public string ValueText { get ; set ; }
    }


    public class PocoCSharpSyntaxNode
    {
    }

    public class PocoAccessorDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoAccessorListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAccessorDeclarationSyntax> Accessors
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoAliasQualifiedNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Alias
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoAnonymousFunctionExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoAnonymousMethodExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DelegateKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoAnonymousObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAnonymousObjectMemberDeclaratorSyntax> Initializers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoAnonymousObjectMemberDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax NameEquals
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoArgumentSyntax> Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoArgumentSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKindKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArrayTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoArrayRankSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExpressionSyntax> Sizes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoArrayTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoArrayRankSpecifierSyntax> RankSpecifiers
        {
            get;
            set;
        }
    }

    public class PocoArrowExpressionClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoAssignmentExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoAttributeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeArgumentSyntax> Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoAttributeArgumentSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax NameEquals
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }
    }

    public class PocoAttributeListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeTargetSpecifierSyntax Target
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeSyntax> Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoAttributeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoAttributeTargetSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoAwaitExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoBadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoBaseArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoArgumentSyntax> Arguments
        {
            get;
            set;
        }
    }

    public class PocoBaseCrefParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoCrefParameterSyntax> Parameters
        {
            get;
            set;
        }
    }

    public class PocoBaseExpressionSyntax : PocoInstanceExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoBaseFieldDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoBaseListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoBaseTypeSyntax> Types
        {
            get;
            set;
        }
    }

    public class PocoBaseMethodDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoBaseParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoParameterSyntax> Parameters
        {
            get;
            set;
        }
    }

    public class PocoBasePropertyDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoBaseTypeDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoBaseTypeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoBinaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoBlockSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoStatementSyntax> Statements
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoBracketedArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoArgumentSyntax> Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoBracketedParameterListSyntax : PocoBaseParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoParameterSyntax> Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoBranchingDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool BranchTaken
        {
            get;
            set;
        }
    }

    public class PocoBreakStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken BreakKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoCasePatternSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoWhenClauseSyntax WhenClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoCaseSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoCastExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoCatchClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CatchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCatchDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCatchFilterClauseSyntax Filter
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoCatchDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCatchFilterClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhenKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax FilterExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCheckedExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCheckedStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoClassDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoClassOrStructConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ClassOrStructKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }
    }

    public class PocoCommonForEachStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoCompilationUnitSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExternAliasDirectiveSyntax> Externs
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoUsingDirectiveSyntax> Usings
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfFileToken
        {
            get;
            set;
        }
    }

    public class PocoConditionalAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenNotNull
        {
            get;
            set;
        }
    }

    public class PocoConditionalDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoConditionalExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenTrue
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenFalse
        {
            get;
            set;
        }
    }

    public class PocoConstantPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoConstructorConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoConstructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoConstructorInitializerSyntax Initializer
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoConstructorInitializerSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisOrBaseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoContinueStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ContinueKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoConversionOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoConversionOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoCrefBracketedParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoCrefParameterSyntax> Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoCrefParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoCrefParameterSyntax> Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCrefParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKindKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoCrefSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoDeclarationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoDeclarationPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoDefaultExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoDefaultSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoDefineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DefineKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoDelegateDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DelegateKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoDestructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TildeToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoDirectiveTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }
    }

    public class PocoDiscardDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnderscoreToken
        {
            get;
            set;
        }
    }

    public class PocoDiscardPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnderscoreToken
        {
            get;
            set;
        }
    }

    public class PocoDocumentationCommentTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoXmlNodeSyntax> Content
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfComment
        {
            get;
            set;
        }
    }

    public class PocoDoStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhileKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoElementAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoElementBindingExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoElifDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElifKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoElseClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoElseDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }
    }

    public class PocoEmptyStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEndIfDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndIfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEndRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndRegionKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEnumDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EnumKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoEnumMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEnumMemberDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax EqualsValue
        {
            get;
            set;
        }
    }

    public class PocoEqualsValueClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }
    }

    public class PocoErrorDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ErrorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEventDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EventKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEventFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EventKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoExplicitInterfaceSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }
    }

    public class PocoExpressionStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoExpressionSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoExternAliasDirectiveSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ExternKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AliasKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoFinallyClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FinallyKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoFixedStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FixedKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForEachStatementSyntax : PocoCommonForEachStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForEachVariableStatementSyntax : PocoCommonForEachStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Variable
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ForKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FirstSemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SecondSemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExpressionSyntax> Incrementors
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExpressionSyntax> Initializers
        {
            get;
            set;
        }
    }

    public class PocoFromClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FromKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoGenericNameSyntax : PocoSimpleNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeArgumentListSyntax TypeArgumentList
        {
            get;
            set;
        }
    }

    public class PocoGlobalStatementSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoGotoStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GotoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CaseOrDefaultKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoGroupClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GroupKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax GroupExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ByKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax ByExpression
        {
            get;
            set;
        }
    }

    public class PocoIdentifierNameSyntax : PocoSimpleNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoIfDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoIfStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoElseClauseSyntax Else
        {
            get;
            set;
        }
    }

    public class PocoImplicitArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Commas
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoImplicitElementAccessSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoImplicitStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StackAllocKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoIncompleteMemberSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoIndexerDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoIndexerMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefBracketedParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoInitializerExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExpressionSyntax> Expressions
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoInstanceExpressionSyntax : PocoExpressionSyntax
    {
    }

    public class PocoInterfaceDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolatedStringContentSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoInterpolatedStringExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StringStartToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoInterpolatedStringContentSyntax> Contents
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StringEndToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolatedStringTextSyntax : PocoInterpolatedStringContentSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TextToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolationAlignmentClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CommaToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }
    }

    public class PocoInterpolationFormatClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FormatStringToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolationSyntax : PocoInterpolatedStringContentSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInterpolationAlignmentClauseSyntax AlignmentClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInterpolationFormatClauseSyntax FormatClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoInvocationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoIsPatternExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IsKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }
    }

    public class PocoJoinClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken JoinKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax InExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OnKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax LeftExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax RightExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoJoinIntoClauseSyntax Into
        {
            get;
            set;
        }
    }

    public class PocoJoinIntoClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IntoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoLabeledStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoLambdaExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }
    }

    public class PocoLetClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LetKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoLineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LineKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Line
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoLiteralExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoLoadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LoadKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoLocalDeclarationStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoLocalFunctionStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoLockStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LockKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoMakeRefExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoMemberAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoMemberBindingExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoMemberCrefSyntax : PocoCrefSyntax
    {
    }

    public class PocoMemberDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }
    }

    public class PocoMethodDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoNameColonSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoNameEqualsSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }
    }

    public class PocoNameMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoNamespaceDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NamespaceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExternAliasDirectiveSyntax> Externs
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoUsingDirectiveSyntax> Usings
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoNameSyntax : PocoTypeSyntax
    {
    }

    public class PocoNullableDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NullableKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SettingToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TargetToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoNullableTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }
    }

    public class PocoObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoOmittedArraySizeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OmittedArraySizeExpressionToken
        {
            get;
            set;
        }
    }

    public class PocoOmittedTypeArgumentSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OmittedTypeArgumentToken
        {
            get;
            set;
        }
    }

    public class PocoOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoOrderByClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OrderByKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoOrderingSyntax> Orderings
        {
            get;
            set;
        }
    }

    public class PocoOrderingSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AscendingOrDescendingKeyword
        {
            get;
            set;
        }
    }

    public class PocoParameterListSyntax : PocoBaseParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoParameterSyntax> Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax Default
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoVariableDesignationSyntax> Variables
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoPatternSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoPointerTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AsteriskToken
        {
            get;
            set;
        }
    }

    public class PocoPositionalPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSubpatternSyntax> Subpatterns
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoPostfixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Operand
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }
    }

    public class PocoPragmaChecksumDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken PragmaKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ChecksumKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Guid
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Bytes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoPragmaWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken PragmaKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WarningKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DisableOrRestoreKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoExpressionSyntax> ErrorCodes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoPredefinedTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }
    }

    public class PocoPrefixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Operand
        {
            get;
            set;
        }
    }

    public class PocoPropertyDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoPropertyPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSubpatternSyntax> Subpatterns
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoQualifiedCrefSyntax : PocoCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Container
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoMemberCrefSyntax Member
        {
            get;
            set;
        }
    }

    public class PocoQualifiedNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoQueryBodySyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoQueryClauseSyntax> Clauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSelectOrGroupClauseSyntax SelectOrGroup
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryContinuationSyntax Continuation
        {
            get;
            set;
        }
    }

    public class PocoQueryClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoQueryContinuationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IntoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryBodySyntax Body
        {
            get;
            set;
        }
    }

    public class PocoQueryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoFromClauseSyntax FromClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryBodySyntax Body
        {
            get;
            set;
        }
    }

    public class PocoRangeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax LeftOperand
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax RightOperand
        {
            get;
            set;
        }
    }

    public class PocoRecursivePatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPositionalPatternClauseSyntax PositionalPatternClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPropertyPatternClauseSyntax PropertyPatternClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoReferenceDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReferenceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoRefExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoRefTypeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoRefTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReadOnlyKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoRefValueExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Comma
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RegionKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoReturnStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReturnKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoSelectClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SelectKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoSelectOrGroupClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoShebangDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ExclamationToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoSimpleBaseTypeSyntax : PocoBaseTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoSimpleLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterSyntax Parameter
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoSimpleNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoSingleVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoSizeOfExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoSkippedTokensTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> Tokens
        {
            get;
            set;
        }
    }

    public class PocoStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StackAllocKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoStatementSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }
    }

    public class PocoStructDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoSyntaxToken> Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoStructuredTriviaSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoSubpatternSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }
    }

    public class PocoSwitchExpressionArmSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoWhenClauseSyntax WhenClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsGreaterThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoSwitchExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax GoverningExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SwitchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSwitchExpressionArmSyntax> Arms
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoSwitchLabelSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoSwitchSectionSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSwitchLabelSyntax> Labels
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoStatementSyntax> Statements
        {
            get;
            set;
        }
    }

    public class PocoSwitchStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SwitchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSwitchSectionSyntax> Sections
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoThisExpressionSyntax : PocoInstanceExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoThrowExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThrowKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoThrowStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThrowKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoTryStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TryKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoCatchClauseSyntax> Catches
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoFinallyClauseSyntax Finally
        {
            get;
            set;
        }
    }

    public class PocoTupleElementSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoTupleExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoArgumentSyntax> Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTupleTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTupleElementSyntax> Elements
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTypeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeSyntax> Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoTypeConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoTypeCrefSyntax : PocoCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoTypeDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoMemberDeclarationSyntax> Members
        {
            get;
            set;
        }
    }

    public class PocoTypeOfExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterConstraintClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhereKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterConstraintSyntax> Constraints
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterConstraintSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoTypeParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoTypeParameterSyntax> Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken VarianceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoTypeSyntax : PocoExpressionSyntax
    {
    }

    public class PocoUndefDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UndefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoUnsafeStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnsafeKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoUsingDirectiveSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StaticKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax Alias
        {
            get;
            set;
        }
    }

    public class PocoUsingStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoVariableDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoVariableDeclaratorSyntax> Variables
        {
            get;
            set;
        }
    }

    public class PocoVariableDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoVariableDesignationSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoVarPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken VarKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WarningKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoWhenClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhenKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }
    }

    public class PocoWhereClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhereKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }
    }

    public class PocoWhileStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhileKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoXmlAttributeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCDataSectionSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartCDataToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndCDataToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCommentSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanExclamationMinusMinusToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken MinusMinusGreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCrefAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefSyntax Cref
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementEndTagSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanSlashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementStartTagSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoXmlAttributeSyntax> Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlElementStartTagSyntax StartTag
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoXmlNodeSyntax> Content
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlElementEndTagSyntax EndTag
        {
            get;
            set;
        }
    }

    public class PocoXmlEmptyElementSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoXmlAttributeSyntax> Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SlashGreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlNameAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlNameSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlPrefixSyntax Prefix
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LocalName
        {
            get;
            set;
        }
    }

    public class PocoXmlNodeSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoXmlPrefixSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Prefix
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoXmlProcessingInstructionSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartProcessingInstructionToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndProcessingInstructionToken
        {
            get;
            set;
        }
    }

    public class PocoXmlTextAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlTextSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual List<PocoSyntaxToken> TextTokens
        {
            get;
            set;
        }
    }

    public class PocoYieldStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override List<PocoAttributeListSyntax> AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken YieldKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReturnOrBreakKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }
}