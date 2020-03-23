using System.Collections.Generic ;
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    public class StubWorkspaceManager : IWorkspaceManager
    {
        public Workspace CreateWorkspace ( IDictionary < string , string > props ) { return null ; }

        public Task OpenSolutionAsync ( Workspace workspace , string solutionPath )
        {
            return Task.CompletedTask ;
        }
    }
}