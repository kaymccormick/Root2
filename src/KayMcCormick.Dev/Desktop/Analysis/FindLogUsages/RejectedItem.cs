#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// RejectedItem.cs
// 
// 2020-03-25-1:54 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace FindLogUsages
{
    public sealed class RejectedItem
    {
        private readonly SyntaxNode                               _statement ;
        // ReSharper disable once NotAccessedField.Local
        private readonly UnsupportedExpressionTypeSyntaxException _unsupported ;

        public RejectedItem (
            SyntaxNode                               statement
          , UnsupportedExpressionTypeSyntaxException unsupported = null
        )
        {
            _statement   = statement ;
            _unsupported = unsupported ;
        }

        public SyntaxNode Statement { get { return _statement ; } }
    }
}