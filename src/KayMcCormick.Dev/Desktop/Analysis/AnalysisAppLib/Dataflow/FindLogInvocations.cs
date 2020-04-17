using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Dataflow
{
    internal sealed class FindLogInvocations : AnalysisBlockProvider < Document , ILogInvocation ,
            TransformManyBlock < Document , ILogInvocation > >
      , IHaveRejectBlock
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly Func < ILogInvocation >                                      _factory ;
#pragma warning disable 169
        private readonly Func < Document , IEnumerable < ILogInvocation > >           _func ;
#pragma warning restore 169
        private readonly IDataflowTransformFuncProvider < Document , ILogInvocation > _provider ;

        public FindLogInvocations (
            [ NotNull ] IDataflowTransformFuncProvider < Document , ILogInvocation > provider
          , Func < ILogInvocation >                                                  factory
        )
        {
            DebugUtils.WriteLine ( $"creating {nameof ( FindLogInvocations )}" ) ;
            _provider = provider ?? throw new ArgumentNullException ( nameof ( provider ) ) ;
            _factory  = factory ;
        }

        #region Implementation of IHaveRejectBlock
        public ISourceBlock < RejectedItem > GetRejectBlock ( )
        {
            return ( ( IHaveRejectBlock ) _provider ).GetRejectBlock ( ) ;
        }
        #endregion


        [ NotNull ]
        public override TransformManyBlock < Document , ILogInvocation > GetDataflowBlock ( )
        {
            // ReSharper disable once UnusedVariable
            Func < Document , IEnumerable < ILogInvocation > > func = document
                => Enumerable.Empty < ILogInvocation > ( ) ;
            return new TransformManyBlock < Document , ILogInvocation > (
                                                                         _provider
                                                                            .GetAsyncTransformFunction ( )
                                                                        ) ;
        }

        [ NotNull ] public override IDataflowBlock GetDataflowBlockObj ( ) { return GetDataflowBlock() ; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IHaveRejectBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISourceBlock < RejectedItem > GetRejectBlock ( ) ;
    }
}