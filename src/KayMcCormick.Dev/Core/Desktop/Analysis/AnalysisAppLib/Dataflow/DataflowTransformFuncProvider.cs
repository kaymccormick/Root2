using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    public abstract class
        DataflowTransformFuncProvider < TSource , TDest > : IDataflowTransformFuncProvider < TSource
          , TDest >
    {
        #region Implementation of IDataflowTransformFuncProvider<TSource,TDest>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Func < TSource , Task < IEnumerable < TDest > > >
            GetAsyncTransformFunction ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Func < TSource , IEnumerable < TDest > > GetTransformFunction ( ) ;
        #endregion
    }
}