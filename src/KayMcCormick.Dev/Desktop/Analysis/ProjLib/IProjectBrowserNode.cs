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
namespace ProjLib
{
    public interface IProjectBrowserNode : IBrowserNode
    {
        string RepositoryUrl { get ; }
        string Platform      { get ; }
        string SolutionPath  { get ; set ; }
    }
}