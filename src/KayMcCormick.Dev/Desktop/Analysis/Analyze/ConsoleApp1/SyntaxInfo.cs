#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// SyntaxInfo.cs
// 
// 2020-04-08-5:31 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis.CSharp ;

namespace ConsoleApp1
{
    internal sealed class SyntaxInfo
    {
        public SyntaxKind Kind { get ; set ; }

        public int Count { get ; set ; }
    }
}