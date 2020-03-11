#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ISemanticModelContext.cs
// 
// 2020-03-05-3:10 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace AnalysisFramework
{
    public interface ISemanticModelContext
    {
        SemanticModel CurrentModel { get ; set ; }
    }
}