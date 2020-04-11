using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.XmlDoc.Project ;
using FindLogUsages ;

namespace AnalysisAppLib.XmlDoc
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