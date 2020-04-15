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
using System.Linq ;
using System.Text.Json ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Project ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.StackTrace ;
using NLog ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalyzeCommand : IAnalyzeCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly Pipeline                      _pipeline ;
        private          ITargetBlock < RejectedItem > _rejectDestination ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public AnalyzeCommand ( Pipeline pipeline ) { _pipeline = pipeline ; }

        #region Implementation of IAnalyzeCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectNode"></param>
        /// <param name="rejectTarget"></param>
        /// <returns></returns>
        /// <exception cref="AnalyzeException"></exception>
        public async Task AnalyzeCommandAsync (
            IProjectBrowserNode           projectNode
          , ITargetBlock < RejectedItem > rejectTarget
        )
        {
            var pipeline = _pipeline ;
            if ( pipeline == null )
            {
                throw new AnalyzeException ( "Pipeline is null" ) ;
            }

            pipeline.BuildPipeline ( ) ;
            var pInstance = pipeline.PipelineInstance ;
            if ( pInstance == null )
            {
                throw new AnalyzeException ( "pipeline instance is null" ) ;
            }

            RejectDestination = rejectTarget ;
            var actionBlock = new ActionBlock < ILogInvocation > ( LogInvocationAction ) ;
            if ( RejectDestination != null )
            {
                _pipeline.RejectBlock.LinkTo (
                                              RejectDestination
                                            , new DataflowLinkOptions { PropagateCompletion = true }
                                             ) ;
            }

            pInstance.LinkTo (
                              actionBlock
                            , new DataflowLinkOptions { PropagateCompletion = true }
                             ) ;

            var req = new AnalysisRequest { Info = projectNode } ;
            if ( ! pInstance.Post ( req ) )
            {
                throw new AnalyzeException ( "Post failed" ) ;
            }

            await HandlePipelineResultAsync ( actionBlock ) ;
        }

        private void LogInvocationAction ( [ NotNull ] ILogInvocation invocation )
        {
            if ( invocation == null )
            {
                throw new ArgumentNullException ( nameof ( invocation ) ) ;
            }
#if !NETSTANDARD2_0
            Console.WriteLine ( JsonSerializer.Serialize ( invocation ) ) ;
#endif
            LogInvocations.Add ( invocation ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogInvocationCollection LogInvocations { get ; } = new LogInvocationCollection ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public ITargetBlock < RejectedItem > RejectDestination
        {
            get { return _rejectDestination ; }
            set { _rejectDestination = value ; }
        }


        private async Task HandlePipelineResultAsync ( ActionBlock < ILogInvocation > actionBlock )
        {
            PipelineResult result ;
            try
            {
                await actionBlock.Completion.ConfigureAwait ( true ) ;
                result = new PipelineResult ( ResultStatus.Success ) ;
            }
            catch ( AggregateException ex1 )
            {
                var exes = ex1.Flatten ( ).InnerExceptions ;
                Logger.Debug ( "actionTask completion threw exception" ) ;
                foreach ( var exception in exes )
                {
                    Logger.Debug ( exception , exception.Message ) ;
                }

                result = new PipelineResult ( ResultStatus.Failed , ex1 ) ;
            }

            if ( result.Status == ResultStatus.Failed )
            {
                Logger.Error (
                              result.TaskException
                            , "Failed: {}"
                            , string.Join (
                                           ", "
                                         , ( ( AggregateException ) result.TaskException )
                                          .Flatten ( )
                                          .InnerExceptions
                                          .Select (
                                                   exception => Tuple.Create (
                                                                              exception.Message
                                                                            , StackTraceParser
                                                                                 .Parse (
                                                                                         exception
                                                                                            .StackTrace
                                                                                       , (
                                                                                             s
                                                                                           , s1
                                                                                           , arg3
                                                                                           , arg4
                                                                                           , arg5
                                                                                           , arg6
                                                                                           , arg7
                                                                                         ) => Tuple
                                                                                            .Create (
                                                                                                     arg6
                                                                                                   , arg7
                                                                                                    )
                                                                                        )
                                                                             )
                                                  )
                                          .Select (
                                                   tuple => Tuple.Create (
                                                                          tuple.Item1
                                                                        , string.Join (
                                                                                       ", "
                                                                                     , tuple
                                                                                      .Item2
                                                                                      .Take ( 2 )
                                                                                      .Select (
                                                                                               tuple1
                                                                                                   => $"{tuple1.Item1}:{tuple1.Item2}"
                                                                                              )
                                                                                      )
                                                                         )
                                                  )
                                          .Select ( tuple => $"{tuple.Item1}: {tuple.Item2}" )
                                          )
                             ) ;
            }

            Logger.Info(
                          "{id} {result} {count}"
                        , Thread.CurrentThread.ManagedThreadId
                        , result.Status
                        , LogInvocations.Count
                         ) ;
        }
        #endregion
    }
}