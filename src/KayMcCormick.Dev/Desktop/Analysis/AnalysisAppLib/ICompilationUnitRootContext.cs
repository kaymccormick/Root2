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
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib
{
    public interface ICompilationUnitRootContext : ISyntaxTreeContext
    {
        CompilationUnitSyntax CompilationUnit { get ; }
    }
}