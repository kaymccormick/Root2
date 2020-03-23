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
    public class PipelineResult
    {
        public ResultStatus Status { get ; }

        public Exception TaskException { get ; }

        public PipelineResult ( ResultStatus status , Exception taskException = null )
        {
            Status        = status ;
            TaskException = taskException ;
        }
    }
}