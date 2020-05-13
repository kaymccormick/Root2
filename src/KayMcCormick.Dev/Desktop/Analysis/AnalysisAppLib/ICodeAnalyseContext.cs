using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeAnalyseContext : ICompilationUnitRootContext , ISemanticModelContext, ICompilationContext
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICompilationContext
    {
        /// <summary>
        /// 
        /// </summary>
        CSharpCompilation Compilation { get; }
    }
}