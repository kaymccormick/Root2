#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// SyntaxItem.cs
// 
// 2020-03-11-7:00 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisAppLib.Syntax
{
    public class SyntaxItem
    {
        private SyntaxToken ? _token ;

        public SyntaxKind SyntaxKind { get ; set ; }

        public SyntaxToken ? Token { get => _token ; set => _token = value ; }
    }
}