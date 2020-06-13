#if FINDLOGUSAGES
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Project ;
using FindLogUsages ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAnalyzeCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectNode"></param>
        /// <param name="rejectTarget"></param>
        /// <returns></returns>
        Task AnalyzeCommandAsync (
            IProjectBrowserNode           projectNode
          , ITargetBlock < RejectedItem > rejectTarget
        ) ;
    }
}
#endif