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
    public class ProjectBrowserNode : BrowserNode , IProjectBrowserNode , IBrowserNode
    {
        private string _platform ;
        private string _solutionPath ;

        public Uri RepositoryUrl { get ; set ; }

        public string Platform { get { return _platform ; } set { _platform = value ; } }

        public string SolutionPath
        {
            get { return _solutionPath ; }
            set { _solutionPath = value ; }
        }
    }
}