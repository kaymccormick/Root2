using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;

namespace AnalysisAppLib.Dataflow
{
    public interface IDataflowTransformFuncProvider < in TSource , TDest >
    {
        Func < TSource , Task < IEnumerable < TDest > > > GetAsyncTransformFunction ( ) ;
        Func < TSource , IEnumerable < TDest > >          GetTransformFunction ( ) ;
    }

    public delegate TResult TransformFunc < in T , out TResult > ( T arg ) ;

    public class
        ConcreteDataflowTransformFuncProvider < TSource , TDest > : IDataflowTransformFuncProvider <
            TSource , TDest >
    {
        private readonly TransformFunc < TSource , Task < IEnumerable < TDest > > > _func ;

        public ConcreteDataflowTransformFuncProvider (
            TransformFunc < TSource , Task < IEnumerable < TDest > > > func
        )
        {
            _func = func ;
        }

        #region Implementation of IDataflowTransformFuncProvider<TSource,TDest>
        public Func < TSource , Task < IEnumerable < TDest > > > GetAsyncTransformFunction ( )
        {
            return x => _func ( x ) ;
        }

        public Func < TSource , IEnumerable < TDest > > GetTransformFunction ( ) { return null ; }
        #endregion
    }
}