#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// IHaveRejectBlock.cs
// 
// 2020-04-24-11:23 PM
// 
// ---
#endregion
#if FINDLOGUSAGES
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHaveRejectBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISourceBlock < RejectedItem > GetRejectBlock ( ) ;
    }
}
#endif