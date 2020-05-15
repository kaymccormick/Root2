using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class FindLogUsagesFuncProvider : DataflowTransformFuncProvider <
            Document , ILogInvocation >
      , IHaveRejectBlock
    {
        private readonly Func < Document , Task < IEnumerable < ILogInvocation > > >
            _transformFunc ;

        private Microsoft.CodeAnalysis.Project _teamProject ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationFactory"></param>
        /// <param name="invocActions"></param>
        private FindLogUsagesFuncProvider (
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
        public override Func < Document , Task < IEnumerable < ILogInvocation > > >
            GetAsyncTransformFunction ( )
        {
            return _transformFunc ;
        }
    }
}