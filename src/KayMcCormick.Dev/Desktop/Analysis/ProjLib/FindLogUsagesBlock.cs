using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AnalysisFramework;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NLog ;

namespace ProjLib
{
    public class FindLogUsagesBlock : ITargetBlock<Document>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ITargetBlock<LogInvocation> _target;

        public IReceivableSourceBlock<LogInvocation> Source => _source;

        private readonly ActionBlock<Document> actionBlock;
        private readonly ITargetBlock<Document> documentTarget;
        private readonly IReceivableSourceBlock<LogInvocation> _source = new BufferBlock<LogInvocation>();

        public FindLogUsagesBlock(ITargetBlock<LogInvocation> target)
        {
            this._target = target;
            actionBlock = new ActionBlock<Document>(
                                                        Action, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4 }
                                                       );
            documentTarget = actionBlock;
        }

        private async Task Action ( Document d )
        {
            var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait(true) ;
            var model = await d.GetSemanticModelAsync ( ) ;
            
            await Task.Run (
                            ( ) => LogUsages.FindLogUsages (
                                                            new CodeSource ( d.FilePath )
                                                          , tree.GetCompilationUnitRoot ( )
                                                          , model
                                                          , invocation
                                                                => {
                                                                Logger.Debug (
                                                                             "Posting invocation {invocation}"
                                                                           , invocation
                                                                            ) ;
                                                                _target.Post ( invocation ) ;
                                                            }
                                                          , false
                                                          , false
                                                          , ( p )
                                                                => LogUsages.ProcessInvocation ( p )
                                                          , tree
                                                           )
                           )
                      .ConfigureAwait ( false ) ;

        }

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