using System.Threading.Tasks.Dataflow ;

namespace AnalysisAppLib.Dataflow
{
    public interface IAnalysisBlockProvider < TSource , TDest , TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >
    {
        TBlock GetDataflowBlock ( ) ;
    }
}