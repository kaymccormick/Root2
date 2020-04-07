#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// PipelineResult.cs
// 
// 2020-03-06-5:14 PM
// 
// ---
#endregion
using System ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class PipelineResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="taskException"></param>
        public PipelineResult ( ResultStatus status , Exception taskException = null )
        {
            Status        = status ;
            TaskException = taskException ;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResultStatus Status { get ; }

        /// <summary>
        /// 
        /// </summary>
        public Exception TaskException { get ; }
    }
}