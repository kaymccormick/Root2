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

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISemanticModelContext
    {
        /// <summary>
        /// 
        /// </summary>
        SemanticModel CurrentModel { get ; set ; }
    }
}