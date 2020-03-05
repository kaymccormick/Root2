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
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
{
    public interface ISyntaxTreeContext
    {
        SyntaxTree SyntaxTree { get ; }
    }

    public interface ICompilationUnitRootContext : ISyntaxTreeContext
    {
        CompilationUnitSyntax CompilationUnit { get ; }
    }
}