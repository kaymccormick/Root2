using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeAnalyseContext : ICompilationUnitRootContext , ISemanticModelContext, ICompilationContext
    {
    }

    public interface ICompilationContext
    {
        CSharpCompilation Compilation { get; }
    }
}