#region header

// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// Client2Module.cs
// 
// 2020-03-08-7:55 PM
// 
// ---

#endregion

using System.Collections;

#if EXPLORER
using ExplorerCtrl ;
#endif

#if MIGRADOC
using MigraDoc.DocumentObjectModel.Internals ;
#endif

namespace Client2
{
#if MSBUILDWORKSPACE
    using Microsoft.CodeAnalysis.MSBuild ;
    internal class MSBuildWorkspaceManager : IWorkspaceManager
    {
        public Workspace CreateWorkspace(IDictionary<string, string> props)
        {
           return MSBuildWorkspace.Create(props);
        }
        public Task OpenSolutionAsync(Workspace workspace, string solutionPath) {
            return ((MSBuildWorkspace)workspace).OpenSolutionAsync(solutionPath);
        }
    }
#else
#endif
}