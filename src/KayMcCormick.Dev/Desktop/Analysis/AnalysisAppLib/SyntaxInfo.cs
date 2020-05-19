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

using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public SyntaxKind Kind { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get ; set ; }
    }
}