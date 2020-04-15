#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// IProjectBrowserNode.cs
// 
// 2020-03-08-8:15 PM
// 
// ---
#endregion
using System ;

namespace AnalysisAppLib.Project
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectBrowserNode : IBrowserNode
    {
        /// <summary>
        /// 
        /// </summary>
        Uri RepositoryUrl { get ; }

        /// <summary>
        /// 
        /// </summary>
        string Platform { get ; }

        /// <summary>
        /// 
        /// </summary>
        string SolutionPath { get ; set ; }
    }
}