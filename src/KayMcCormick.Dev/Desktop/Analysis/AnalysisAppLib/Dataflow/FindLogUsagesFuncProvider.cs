using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FindLogUsagesFuncProvider : DataflowTransformFuncProvider <
            Document , ILogInvocation >
      , IHaveRejectBlock
    {
        private readonly Func < Document , Task < IEnumerable < ILogInvocation > > >
            _transformFunc ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationFactory"></param>
        public FindLogUsagesFuncProvider ( Func < ILogInvocation > invocationFactory )
        {
            var findusages = new FindLogUsagesMain( invocationFactory ) ;
            RejectBlock    = new BufferBlock < RejectedItem > ( ) ;
            _transformFunc = document => findusages.FindUsagesFuncAsync ( document , RejectBlock ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferBlock < RejectedItem > RejectBlock { get ; }

        #region Implementation of IHaveRejectBlock
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISourceBlock < RejectedItem > GetRejectBlock ( ) { return RejectBlock ; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [ NotNull ]
        public override Func < Document , IEnumerable < ILogInvocation > > GetTransformFunction ( )
        {
            return document => {
                var task = _transformFunc ( document ) ;
                task.Wait ( ) ;
                if ( task.IsFaulted )
                {
                    if ( task.Exception != null )
                    {
                        throw task.Exception ;
                    }

                    throw new InvalidOperationException ( "Faulted transform" ) ;
                }

                return task.Result ;
            } ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Func < Document , Task < IEnumerable < ILogInvocation > > >
            GetAsyncTransformFunction ( )
        {
            return _transformFunc ;
        }
    }
}