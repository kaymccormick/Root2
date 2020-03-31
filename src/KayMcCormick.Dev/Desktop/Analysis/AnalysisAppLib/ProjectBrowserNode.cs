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
using AnalysisAppLib.Projects ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectBrowserNode : BrowserNode , IProjectBrowserNode , IBrowserNode
    {
        private string _platform ;
        private string _solutionPath ;

        /// <summary>
        /// 
        /// </summary>
        public Uri RepositoryUrl { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public string Platform { get { return _platform ; } set { _platform = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string SolutionPath
        {
            get { return _solutionPath ; }
            set { _solutionPath = value ; }
        }
    }
}