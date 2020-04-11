using System.Threading.Tasks.Dataflow ;

namespace AnalysisAppLib.XmlDoc.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public interface IAnalysisBlockProvider < TSource , TDest , out TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TBlock GetDataflowBlock ( ) ;
    }
}