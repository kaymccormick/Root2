#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// SyntaxNodeSpanObject.cs
// 
// 2020-02-26-10:02 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public class SyntaxNodeSpanObject : SpanObject < SyntaxNode >
    {
        private SyntaxKind _kind ;

        
        public SyntaxNodeSpanObject ( TextSpan span , SyntaxNode instance ) : base (
                                                                                    span
                                                                                  , instance
                                                                                   )
        {
            _kind = instance.Kind ( ) ;
        }

        public SyntaxKind Kind { get => _kind ; set => _kind = value ; }
    }
}