#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ICodeAnalyseContext.cs
// 
// 2020-03-05-3:11 AM
// 
// ---
#endregion
namespace AnalysisFramework
{
    public interface ICodeAnalyseContext : ICompilationUnitRootContext , ISemanticModelContext
    {
    }
}