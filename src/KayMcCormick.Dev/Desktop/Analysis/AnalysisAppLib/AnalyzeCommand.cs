#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// AnalyzeCommand.cs
// 
// 2020-03-23-11:01 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using JetBrains.Annotations ;
using NLog ;

namespace AnalysisAppLib
{
    public class AnalyzeCommand : IAnalyzeCommand
    {
        private readonly Pipeline _pipeline ;

        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public AnalyzeCommand (Pipeline pipeline ) { _pipeline = pipeline ; }

        #region Implementation of IAnalyzeCommand
        public async Task AnalyzeCommandAsync ( IProjectBrowserNode projectNode )
        {
            PipelineResult result = new PipelineResult ( ResultStatus.Pending ) ;
            var actionBlock = new ActionBlock < ILogInvocation > ( LogInvocationAction ) ;

            var pipeline = _pipeline ;
            if ( pipeline == null )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "Pipeline is null" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            pipeline.BuildPipeline ( ) ;
            var pInstance = pipeline.PipelineInstance ;
            if ( pInstance == null )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "pipeline instance is null" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            pInstance.LinkTo (
                              actionBlock
                            , new DataflowLinkOptions ( ) { PropagateCompletion = true }
                             ) ;

            var req = new AnalysisRequest { Info = projectNode } ;
            if ( ! pInstance.Post ( req ) )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "Post failed" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            await HandlePipelineResultAsync ( actionBlock ) ;
        }

        private void LogInvocationAction([NotNull] ILogInvocation invocation)
        {
            if (invocation == null)
            {
                throw new ArgumentNullException(nameof(invocation));
            }
#if !NETSTANDARD2_0
            Console.WriteLine(JsonSerializer.Serialize(invocation));
#endif
            LogInvocations.Add(invocation);
        }
        public LogInvocationCollection LogInvocations { get; } = new LogInvocationCollection();


        private async Task HandlePipelineResultAsync(ActionBlock<ILogInvocation> actionBlock)
        {
            PipelineResult result;
            try
            {
                await actionBlock.Completion.ConfigureAwait(true);
                result = new PipelineResult(ResultStatus.Success);
            }
            catch (AggregateException ex1)
            {
                var exes = ex1.Flatten().InnerExceptions;
                Logger.Debug($"actionTask completion threw exception");
                foreach (var exception in exes)
                {
                    Logger.Debug(exception, exception.Message);
                }

                result = new PipelineResult(ResultStatus.Failed, ex1);
            }

            if (result.Status == ResultStatus.Failed)
            {
                Logger.Error(
                             result.TaskException
                           , "Failed: {}"
                           , result.TaskException.Message
                            );
            }

            Logger.Debug(
                         "{id} {result} {count}"
                       , Thread.CurrentThread.ManagedThreadId
                       , result.Status
                       , LogInvocations.Count
                        );
        }

        #endregion
    }
}