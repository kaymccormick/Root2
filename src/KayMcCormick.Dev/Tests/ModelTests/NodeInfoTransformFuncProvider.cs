#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ModelTests
// NodeInfoTransformFuncProvider.cs
// 
// 2020-04-24-11:29 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;

namespace ModelTests
{
    public sealed class NodeInfoTransformFuncProvider : DataflowTransformFuncProvider <
            Document , NodeInfo >
      , IHaveRejectBlock
    {
        private readonly TransformManyFunc < Document , NodeInfo > _func ;
        private BufferBlock < RejectedItem > _getRejectBlock = new BufferBlock<RejectedItem>(); 

        public NodeInfoTransformFuncProvider (
            TransformManyFunc<Document, NodeInfo> func
        )
        {
            _func = func ;
        }

        #region Overrides of DataflowTransformFuncProvider<Document,NodeInfo>
        public override Func < Document , Task < IEnumerable < NodeInfo > > >
            GetAsyncTransformFunction ( )
        {
            return async ( doc ) => _func ( doc ) ;
        }
        #endregion
        #region Implementation of IHaveRejectBlock
        [ NotNull ]
        public ISourceBlock < RejectedItem > GetRejectBlock ( )
        {
            return _getRejectBlock ;
        }
        #endregion
    }
}