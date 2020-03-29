using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;

namespace AnalysisAppLib.Dataflow
{
    public abstract class
        AnalysisBlockProvider < TSource , TDest , TBlock > : IAnalysisBlockProvider < TSource ,
            TDest , TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >
    {
        public abstract TBlock GetDataflowBlock ( ) ;
    }

    public delegate TBlock BlockFactory < TSource , TDest , out TBlock > (
        Func < TSource , Task < IEnumerable < TDest > > > transform
    )
        where TBlock : IPropagatorBlock < TSource , TDest > ;

    public class
        ConcreteAnalysisBlockProvider < TSource , TDest , TBlock > : AnalysisBlockProvider < TSource
          , TDest , TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >

    {
        private readonly IDataflowTransformFuncProvider < TSource , TDest > _funcProvider ;
        private readonly TBlock                                             _block ;

        public ConcreteAnalysisBlockProvider (
            BlockFactory < TSource , TDest , TBlock >          factory
          , IDataflowTransformFuncProvider < TSource , TDest > funcProvider
        )
        {
            _block        = factory ( _funcProvider.GetAsyncTransformFunction ( ) ) ;
            _funcProvider = funcProvider ;
        }

        #region Overrides of AnalysisBlockProvider<TSource,TDest,TransformManyBlock<TSource,TDest>>
        public override TBlock GetDataflowBlock ( ) { return _block ; }
        #endregion

        #region Overrides of AnalysisBlockProvider<TSource,TDest,TBlock>
        #endregion
    }
}