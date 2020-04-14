#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// SyntaxRewriter1.cs
// 
// 2020-04-14-10:02 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using AnalysisControls.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace ConsoleApp1
{
    internal sealed class SyntaxRewriter1 : CSharpSyntaxRewriter
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly IReadOnlyDictionary < string , object > _map ;
        // ReSharper disable once NotAccessedField.Local
        private readonly TypesViewModel _model1 ;
        #region Overrides of CSharpSyntaxRewriter
        // ReSharper disable once UnusedMember.Global
        public SyntaxRewriter1 (
            IReadOnlyDictionary < string , object > map
          , bool                                    visitIntoStructuredTrivia = false
        ) : base ( visitIntoStructuredTrivia )
        {
            _map = map ;
        }

        public SyntaxRewriter1 ( TypesViewModel model1 ) { _model1 = model1 ; }

        public override SyntaxNode VisitGenericName ( [ NotNull ] GenericNameSyntax node )
        {
            DebugUtils.WriteLine ( $"{node.Identifier} {node.TypeArgumentList}" ) ;
            if ( node.Arity                   == 1
                 && node.IsUnboundGenericName == false
                 && node.Identifier.ValueText == "SeparatedSyntaxList" )
            {
                var typeSyntax = node.TypeArgumentList.Arguments[ 0 ] ;
                if ( typeSyntax is SimpleNameSyntax sns )
                {
                    //return node.ReplaceNode ( node , node , _map[ sns.Identifier.ValueText ] ) ;
                }
            }

            return base.VisitGenericName ( node ) ;
        }

        [ CanBeNull ]
        public override SyntaxNode VisitPredefinedType ( PredefinedTypeSyntax node )
        {
            return base.VisitPredefinedType ( node ) ;
        }

        public override SyntaxList < TNode > VisitList < TNode > ( SyntaxList < TNode > list )
        {
            return base.VisitList ( list ) ;
        }

        public override TNode VisitListElement < TNode > ( TNode node )
        {
            return base.VisitListElement ( node ) ;
        }

        public override SeparatedSyntaxList < TNode > VisitList < TNode > (
            SeparatedSyntaxList < TNode > list
        )
        {
            return base.VisitList ( list ) ;
        }

        public override SyntaxToken VisitListSeparator ( SyntaxToken separator )
        {
            return base.VisitListSeparator ( separator ) ;
        }

        public override SyntaxTokenList VisitList ( SyntaxTokenList list )
        {
            return base.VisitList ( list ) ;
        }

        public override SyntaxTriviaList VisitList ( SyntaxTriviaList list )
        {
            return base.VisitList ( list ) ;
        }

        public override SyntaxTrivia VisitListElement ( SyntaxTrivia element )
        {
            return base.VisitListElement ( element ) ;
        }
        #endregion
    }
}