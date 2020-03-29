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

namespace AnalysisAppLib
{
    public class TriviaSpanObject : SpanObject < SyntaxTrivia >
    {
        
        public TriviaSpanObject ( TextSpan span , SyntaxTrivia instance ) : base ( span , instance )
        {
        }

    }
}