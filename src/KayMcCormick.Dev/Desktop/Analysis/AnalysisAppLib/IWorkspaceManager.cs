using System.Collections.Generic ;
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    public interface IWorkspaceManager
    {
        Workspace CreateWorkspace(IDictionary<string, string> props);
        Task      OpenSolutionAsync(Workspace                 workspace, string solutionPath);
    }
}