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
using KayMcCormick.Dev ;
using KayMcCormick.Dev.StackTrace ;
using NLog ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Generic analyze command.
    /// </summary>
    public sealed class AnalyzeCommand : IAnalyzeCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly Pipeline                      _pipeline ;
        private readonly IAddRuntimeResource           _add ;
        private          ITargetBlock < RejectedItem > _rejectDestination ;
        private          ResourceNodeInfo              _resultsNode ;

        /// <summary>
        /// Constructor. Takes pipeline instance.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="add"></param>
        public AnalyzeCommand ( Pipeline pipeline , IAddRuntimeResource add )
        {
            _pipeline = pipeline ;
            _add      = add ;
        }

        #region Implementation of IAnalyzeCommand
        /// <summary>
        /// Async analyze routine.
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
            var resourceNodeInfo = ResourceNodeInfo.CreateInstance ( ) ;
            resourceNodeInfo.Key = "Analyze command" ;
            _add.AddResource ( resourceNodeInfo ) ;
            using ( MappedDiagnosticsLogicalContext.SetScoped ( "Command" , "AnalyzeCommand" ) )
            {
                Logger.Debug ( "run command" ) ;
                var pipeline = _pipeline ;
                if ( pipeline == null )
                {
                    throw new AnalyzeException ( "Pipeline is null" ) ;
                }

                pipeline.BuildPipeline ( ) ;
                var pInstance = pipeline.PipelineInstance ;
                var nodeInfo = ResourceNodeInfo.CreateInstance ( ) ;
                nodeInfo.Key  = "Pipeline" ;
                nodeInfo.Data =null ;
                
                resourceNodeInfo.AddChild ( nodeInfo ) ;
                
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
                                                , new DataflowLinkOptions
                                                  {
                                                      PropagateCompletion = true
                                                  }
                                                 ) ;
                }

                pInstance.LinkTo (
                                  actionBlock
                                , new DataflowLinkOptions { PropagateCompletion = true }
                                 ) ;
                var tcs = new CancellationTokenSource ( ) ;
                var cancellationToken = tcs.Token ;
                var t = Task.Run ( ( ) => Function ( ) , cancellationToken ) ;
                var instance = ResourceNodeInfo.CreateInstance ( ) ;
                instance.Key  = "Task " + t.Id ;
                instance.Data = t ;
                resourceNodeInfo.Children.Add ( instance ) ;
                _resultsNode     = ResourceNodeInfo.CreateInstance ( ) ;
                _resultsNode.Key = "Results" ;
                resourceNodeInfo.Children.Add ( _resultsNode ) ;

                var req = new AnalysisRequest { Info = projectNode } ;
                if ( ! pInstance.Post ( req ) )
                {
                    throw new AnalyzeException ( "Post failed" ) ;
                }

                DebugUtils.WriteLine ( "await pipeline" ) ;
                await HandlePipelineResultAsync ( actionBlock ) ;

                if ( cancellationToken.CanBeCanceled )
                {
                    tcs.Cancel ( ) ;
                }
            }
        }

        private int Function ( )
        {
            for ( ; ; )
            {
                var blockCount = _pipeline.Block.Count ;
                if ( blockCount != 0 )
                {
                    Logger.Info ( blockCount ) ;
                }

                Logger.Info ( $"{_pipeline.Input.Completion.IsCompleted}" ) ;

                var dataflowBlocks = _pipeline.Blocks.Where ( x => ! x.Completion.IsCompleted ) ;
                if ( dataflowBlocks.Any ( ) == false )
                {
                    return 1 ;
                }

                foreach ( var dataflowBlock in dataflowBlocks )
                {
                    Logger.Info ( dataflowBlock.ToString ) ;

                    continue ;
                    var gt = dataflowBlock.GetType ( ).GetGenericTypeDefinition ( ) ;
                    if ( gt == typeof ( TransformBlock < , > ) )
                    {
                        var ic = dataflowBlock.GetType ( ).GetProperty ( "InputCount" ) ;
                        var input = ( int ) ic.GetValue ( dataflowBlock ) ;
                        Logger.Info ( "input count is {input}" , input ) ;
                    }
                }

                Thread.Sleep ( 100 ) ;
            }
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
            var resourceNodeInfo = ResourceNodeInfo.CreateInstance ( ) ;
            resourceNodeInfo.Key  = invocation ;
            resourceNodeInfo.Data = invocation ;
            _resultsNode.Children.Add ( resourceNodeInfo ) ;
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
                DebugUtils.WriteLine ( "await completion" ) ;
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

            Logger.Info (
                         "{id} {result} {count}"
                       , Thread.CurrentThread.ManagedThreadId
                       , result.Status
                       , LogInvocations.Count
                        ) ;
        }
        #endregion
    }
}