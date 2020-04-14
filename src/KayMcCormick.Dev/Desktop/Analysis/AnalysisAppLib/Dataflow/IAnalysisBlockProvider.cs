using System.Threading.Tasks.Dataflow ;

namespace AnalysisAppLib.XmlDoc.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public interface IAnalysisBlockProvider < TSource , TDest , out TBlock > : IAnalysisBlockProvider1
        where TBlock : IPropagatorBlock < TSource , TDest >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TBlock GetDataflowBlock ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAnalysisBlockProvider1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDataflowBlock GetDataflowBlockObj ( ) ;
    }
}