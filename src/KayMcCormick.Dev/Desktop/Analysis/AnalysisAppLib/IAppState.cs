#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IAppState.cs
// 
// 2020-02-29-7:48 AM
// 
// ---
#endregion
namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppState
    {
        /// <summary>
        /// 
        /// </summary>
        bool Processing { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string CurrentProject { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string CurrentDocumentPath { get ; set ; }
    }
}