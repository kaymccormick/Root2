using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public abstract class
        AnalysisBlockProvider < TSource , TDest , TBlock > : IAnalysisBlockProvider < TSource ,
            TDest , TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract TBlock GetDataflowBlock ( ) ;

        #region Implementation of IAnalysisBlockProvider1
        /// <inheritdoc />
        public abstract IDataflowBlock GetDataflowBlockObj ( ) ;
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public delegate TBlock BlockFactory < TSource , TDest , out TBlock > (
        Func < TSource , Task < IEnumerable < TDest > > > transform
    )
        where TBlock : IPropagatorBlock < TSource , TDest > ;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public class
        ConcreteAnalysisBlockProvider < TSource , TDest , TBlock > : AnalysisBlockProvider < TSource
          , TDest , TBlock >
        where TBlock : IPropagatorBlock < TSource , TDest >

    {
        private readonly IDataflowTransformFuncProvider < TSource , TDest > _funcProvider ;
        private readonly TBlock                                             _block ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="funcProvider"></param>
        public ConcreteAnalysisBlockProvider (
            [ NotNull ] BlockFactory < TSource , TDest , TBlock >          factory
          , IDataflowTransformFuncProvider < TSource , TDest > funcProvider
        )
        {
            _block        = factory ( _funcProvider.GetAsyncTransformFunction ( ) ) ;
            _funcProvider = funcProvider ;
        }

        #region Overrides of AnalysisBlockProvider<TSource,TDest,TransformManyBlock<TSource,TDest>>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TBlock GetDataflowBlock ( ) { return _block ; }

        /// <inheritdoc />
        public override IDataflowBlock GetDataflowBlockObj ( ) { return _block ; }
        #endregion

        #region Overrides of AnalysisBlockProvider<TSource,TDest,TBlock>
        #endregion
    }
}