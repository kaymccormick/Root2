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
using System.Windows ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public class SyntaxNodeSpanObject : SpanObject < SyntaxNode >
    {
        private SyntaxKind _kind ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SyntaxNodeSpanObject ( TextSpan span , SyntaxNode instance ) : base (
                                                                                    span
                                                                                  , instance
                                                                                   )
        {
            _kind = instance.Kind ( ) ;
        }

        public SyntaxKind Kind { get => _kind ; set => _kind = value ; }

        public override UIElement GetToolTipContent ( )
        {
            var kind = Kind.ToString ( ) ;
            return new TextBlock ( ) { Text = kind } ;
        }
    }
}