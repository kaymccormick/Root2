using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
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
        /// <param name="invocActions"></param>
        public FindLogUsagesFuncProvider (
            Func < ILogInvocation >                   invocationFactory
          , IEnumerable < Action < ILogInvocation > > invocActions
        )
        {

            var findusages = new FindLogUsagesMain( invocationFactory ) ;
            RejectBlock    = new BufferBlock < RejectedItem > ( ) ;
            _transformFunc = document => findusages.FindUsagesFuncAsync ( document , RejectBlock , invocActions) ;
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
                if ( ! task.IsFaulted )
                {
                    return task.Result ;
                }

                if ( task.Exception != null )
                {

                    DebugUtils.WriteLine($"{task.Exception.ToString()}");
                    throw task.Exception ;
                }

                DebugUtils.WriteLine($"faulted");
                throw new InvalidOperationException ( "Faulted transform" ) ;

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