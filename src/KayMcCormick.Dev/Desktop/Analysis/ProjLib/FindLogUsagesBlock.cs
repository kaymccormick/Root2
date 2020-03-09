using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AnalysisFramework;

using Microsoft.CodeAnalysis;
using NLog ;

namespace ProjLib
{
    public class FindLogUsagesBlock : ITargetBlock<Document>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ITargetBlock<ILogInvocation> _target;

        public IReceivableSourceBlock<ILogInvocation> Source => _source;

        private readonly ActionBlock<Document> actionBlock;
        private readonly ITargetBlock<Document> documentTarget;
        private readonly IReceivableSourceBlock<ILogInvocation> _source = new BufferBlock<ILogInvocation>();

        public ITargetBlock<Document> Target { get { return actionBlock; } }
        #region Implementation of IDataflowBlock
        public DataflowMessageStatus OfferMessage(
            DataflowMessageHeader messageHeader
          , Document messageValue
          , ISourceBlock<Document> source
          , bool consumeToAccept
        )
        {
            return documentTarget.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        public void Complete() { actionBlock.Complete(); }

        public void Fault(Exception exception) { ((IDataflowBlock)actionBlock).Fault(exception); }

        public Task Completion
        {
            get => actionBlock.Completion;
        }
        #endregion
    }
}