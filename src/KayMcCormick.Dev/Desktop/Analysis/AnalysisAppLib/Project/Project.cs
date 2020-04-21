#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// ProjectBrowserNode.cs
// 
// 2020-03-08-8:15 PM
// 
// ---
#endregion
using System ;

namespace AnalysisAppLib.Project
{
    /// <summary>
    /// Project Browser node
    /// </summary>
    public sealed class ProjectBrowserNode : BrowserNode , IProjectBrowserNode , IBrowserNode
    {
        private string _platform ;
        private string _solutionPath ;

        /// <summary>
        /// Repository URL for project
        /// </summary>
        public Uri RepositoryUrl { get ; set ; }

        /// <summary>
        /// Platform configuration setting for project.
        /// </summary>
        public string Platform { get { return _platform ; } set { _platform = value ; } }

        /// <summary>
        /// Path to solution file.
        /// </summary>
        public string SolutionPath
        {
            get { return _solutionPath ; }
            set { _solutionPath = value ; }
        }
    }
}