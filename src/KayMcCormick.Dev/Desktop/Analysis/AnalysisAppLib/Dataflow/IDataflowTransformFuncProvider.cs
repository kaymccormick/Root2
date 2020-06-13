using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    public interface IDataflowTransformFuncProvider < in TSource , TDest >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Func < TSource , Task < IEnumerable < TDest > > > GetAsyncTransformFunction ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="arg"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public delegate TResult TransformFunc<in T, out TResult>(T arg);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="arg"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public delegate IEnumerable<TResult> TransformManyFunc<in T, out TResult>(T arg);


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    public sealed class
        ConcreteDataflowTransformFuncProvider < TSource , TDest > : IDataflowTransformFuncProvider <
            TSource , TDest >
    {
        private readonly TransformFunc < TSource , Task < IEnumerable < TDest > > > _func ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public ConcreteDataflowTransformFuncProvider (
            TransformFunc < TSource , Task < IEnumerable < TDest > > > func
        )
        {
            _func = func ;
        }

        #region Implementation of IDataflowTransformFuncProvider<TSource,TDest>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public Func < TSource , Task < IEnumerable < TDest > > > GetAsyncTransformFunction ( )
        {
            return x => _func ( x ) ;
        }
        #endregion
    }
}