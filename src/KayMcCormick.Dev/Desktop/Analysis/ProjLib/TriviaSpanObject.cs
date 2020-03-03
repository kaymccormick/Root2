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

namespace ProjLib
{
    public class TriviaSpanObject : SpanObject < SyntaxTrivia >
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TriviaSpanObject ( TextSpan span , SyntaxTrivia instance ) : base ( span , instance )
        {
        }

    }
}