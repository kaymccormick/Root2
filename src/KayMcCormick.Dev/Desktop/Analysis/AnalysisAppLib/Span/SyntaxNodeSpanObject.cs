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

namespace AnalysisAppLib.XmlDoc.Span
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxNodeSpanObject : SpanObject < SyntaxNode >
    {
        private SyntaxKind _kind ;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <param name="instance"></param>
        public SyntaxNodeSpanObject ( TextSpan span , SyntaxNode instance ) : base (
                                                                                    span
                                                                                  , instance
                                                                                   )
        {
            _kind = instance.Kind ( ) ;
        }

        /// <summary>
        ///
        /// 
        /// </summary>
        public SyntaxKind Kind { get { return _kind ; } set { _kind = value ; } }
    }
}