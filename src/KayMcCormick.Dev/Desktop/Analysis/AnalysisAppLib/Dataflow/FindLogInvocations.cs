using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Threading.Tasks.Dataflow ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Dataflow
{
    internal class FindLogInvocations : AnalysisBlockProvider < Document , ILogInvocation ,
            TransformManyBlock < Document , ILogInvocation > >
      , IHaveRejectBlock
    {
        private readonly Func < ILogInvocation >                                      _factory ;
        private readonly Func < Document , IEnumerable < ILogInvocation > >           _func ;
        private readonly IDataflowTransformFuncProvider < Document , ILogInvocation > _provider ;

        public FindLogInvocations (
            [ NotNull ] IDataflowTransformFuncProvider < Document , ILogInvocation > provider
          , Func < ILogInvocation >                                                  factory
        )
        {
            Debug.WriteLine ( $"creating {nameof ( FindLogInvocations )}" ) ;
            _provider = provider ?? throw new ArgumentNullException ( nameof ( provider ) ) ;
            _factory  = factory ;
        }

        #region Implementation of IHaveRejectBlock
        public ISourceBlock < RejectedItem > GetRejectBlock ( )
        {
            return ( ( IHaveRejectBlock ) _provider ).GetRejectBlock ( ) ;
        }
        #endregion


        public override TransformManyBlock < Document , ILogInvocation > GetDataflowBlock ( )
        {
            Func < Document , IEnumerable < ILogInvocation > > func = document
                => Enumerable.Empty < ILogInvocation > ( ) ;
            return new TransformManyBlock < Document , ILogInvocation > (
                                                                         _provider
                                                                            .GetAsyncTransformFunction ( )
                                                                        ) ;
        }
    }

    public interface IHaveRejectBlock
    {
        ISourceBlock < RejectedItem > GetRejectBlock ( ) ;
    }
}