#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ISyntaxTreeContext.cs
// 
// 2020-03-05-2:23 AM
// 
// ---
#endregion

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompilationUnitRootContext : ISyntaxTreeContext
    {
        /// <summary>
        /// 
        /// </summary>
        CompilationUnitSyntax CompilationUnit { get ; }

        SyntaxNode Node { get; }
    }
}