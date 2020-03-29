using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Dataflow
{
    public sealed class FindLogUsagesFuncProvider : DataflowTransformFuncProvider <
            Document , ILogInvocation >
      , IHaveRejectBlock
    {
        private readonly Func < Document , Task < IEnumerable < ILogInvocation > > >
            _transformFunc ;

        public FindLogUsagesFuncProvider ( Func < ILogInvocation > invocationFactory )
        {
            var findusages = new FindLogUsages ( invocationFactory ) ;
            RejectBlock    = new BufferBlock < RejectedItem > ( ) ;
            _transformFunc = document => findusages.FindUsagesFuncAsync ( document , RejectBlock ) ;
        }

        public BufferBlock < RejectedItem > RejectBlock { get ; }

        #region Implementation of IHaveRejectBlock
        public ISourceBlock < RejectedItem > GetRejectBlock ( ) { return RejectBlock ; }
        #endregion

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

        public override Func < Document , Task < IEnumerable < ILogInvocation > > >
            GetAsyncTransformFunction ( )
        {
            return _transformFunc ;
        }
    }
}