using System ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;

namespace ProjLib
{
    public class FindLogUsagesBlock : ITargetBlock<Document>
    {
        private ITargetBlock < LogInvocation > _target ;

        public IReceivableSourceBlock < LogInvocation > Source => _source ;

        private readonly ActionBlock < Document >                 actionBlock ;
        private readonly ITargetBlock < Document > documentTarget ;
        private readonly IReceivableSourceBlock < LogInvocation > _source = new BufferBlock < LogInvocation > ( ) ;

        public FindLogUsagesBlock ( ITargetBlock < LogInvocation > target )
        {
            this._target = target ;
            actionBlock = new ActionBlock < Document > (
                                                        Action
                                                       ) ;
            documentTarget = actionBlock ;
        }

        private async Task Action ( Document d )
        {
            var tree = await d.GetSyntaxTreeAsync ( ) ;
            var model = await d.GetSemanticModelAsync ( ) ;
            LogUsages.FindLogUsages (
                                     new CodeSource ( d.FilePath )
                                   , tree.GetCompilationUnitRoot ( )
                                   , model
                                   , invocation => _target.Post ( invocation )
                                   , false
                                   , false
                                   , ( p ) => LogUsages.ProcessInvocation ( p )
                                   , tree
                                    ) ;
            _target.Complete();
        }

        public ITargetBlock < Document > Target { get { return actionBlock ; } }
        #region Implementation of IDataflowBlock
        public DataflowMessageStatus OfferMessage (
            DataflowMessageHeader messageHeader
          , Document              messageValue
          , ISourceBlock < Document > source
          , bool                  consumeToAccept
        )
        {
            return documentTarget.OfferMessage ( messageHeader , messageValue , source , consumeToAccept ) ;
        }

        public void Complete ( ) { actionBlock.Complete ( ) ; }

        public void Fault ( Exception exception ) { ( ( IDataflowBlock ) actionBlock ).Fault ( exception ) ; }

        public Task Completion
        {
            get => actionBlock.Completion ;
        }
        #endregion
    }
}