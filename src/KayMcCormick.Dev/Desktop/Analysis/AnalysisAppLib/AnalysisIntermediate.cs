#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// AnalysisIntermediate.cs
// 
// 2020-04-23-8:21 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalysisIntermediate
    {
        private Workspace _workspace ;
        /// <summary>
        /// 
        /// </summary>
        public  Workspace Workspace { get { return _workspace ; } set { _workspace = value ; } }
    }
}