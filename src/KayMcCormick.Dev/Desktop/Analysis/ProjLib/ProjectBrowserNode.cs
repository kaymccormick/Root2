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
namespace ProjLib
{
    public class ProjectBrowserNode : BrowserNode, IProjectBrowserNode, IBrowserNode
    {
        private string _solutionPath ;
        private string _platform ;

        public string RepositoryUrl { get ; set ; }

        public string Platform { get => _platform ; set => _platform = value ; }

        public string SolutionPath { get { return _solutionPath ; } set { _solutionPath = value ; } }
    }
}