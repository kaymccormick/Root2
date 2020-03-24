using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;

namespace AnalysisAppLib
{
    public interface IAnalyzeCommand
    {
        Task AnalyzeCommandAsync (
            IProjectBrowserNode           projectNode
          , ITargetBlock < RejectedItem > rejectTarget
        ) ;
    }
}