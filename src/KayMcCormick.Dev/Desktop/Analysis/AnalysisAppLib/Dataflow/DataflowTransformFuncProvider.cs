using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;

namespace AnalysisAppLib.Dataflow
{
    public abstract class
        DataflowTransformFuncProvider < TSource , TDest > : IDataflowTransformFuncProvider < TSource
          , TDest >
    {
        #region Implementation of IDataflowTransformFuncProvider<TSource,TDest>
        public abstract Func < TSource , Task < IEnumerable < TDest > > >
            GetAsyncTransformFunction ( ) ;

        public abstract Func < TSource , IEnumerable < TDest > > GetTransformFunction ( ) ;
        #endregion
    }
}