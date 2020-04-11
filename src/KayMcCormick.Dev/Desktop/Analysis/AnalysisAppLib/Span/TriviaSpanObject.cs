#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// TriviaSpanObject.cs
// 
// 2020-02-26-10:01 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Text ;

namespace AnalysisAppLib.XmlDoc.Span
{
    /// <summary>
    /// 
    /// </summary>
    public class TriviaSpanObject : SpanObject < SyntaxTrivia >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <param name="instance"></param>
        public TriviaSpanObject ( TextSpan span , SyntaxTrivia instance ) : base ( span , instance )
        {
        }
    }
}