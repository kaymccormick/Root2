#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// GenericAnalyzeCommand.cs
// 
// 2020-03-25-1:57 PM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using KayMcCormick.Dev.StackTrace ;
using NLog ;

namespace AnalysisAppLib
{
    public class GenericAnalyzeCommand < TOutput > : IAnalyzeCommand2 < TOutput >
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly ITargetBlock < AnalysisRequest > _acceptorBlock ;
        private readonly ISourceBlock < TOutput >         _outputBlock ;
        private          ActionBlock < TOutput >          _actionBlock ;
        private          IDataflowBlock                   _finalBlock ;


        private ITargetBlock < RejectedItem > _rejectDestination ;

        public GenericAnalyzeCommand (
            ITargetBlock < AnalysisRequest > acceptorBlock
          , ISourceBlock < TOutput >         outputBlock
        )
        {
            _acceptorBlock = acceptorBlock ;
            _outputBlock   = outputBlock ;
        }

        #region Implementation of IAnalyzeCommand
        public async Task AnalyzeCommandAsync ( IProjectBrowserNode projectNode )
        {
            var pipeline = _acceptorBlock ;
            if ( pipeline == null )
            {
                throw new AnalyzeException ( "Pipeline is null" ) ;
            }

            // RejectDestination = rejectTarget ;
            // if ( RejectDestination != null )
            // {
            // _outputBlock.LinkTo (
            // RejectDestination
            // , new DataflowLinkOptions { PropagateCompletion = true }
            // ) ;
            // }

            if ( _actionBlock != null )
            {
                _outputBlock.LinkTo (
                                     _actionBlock
                                   , new DataflowLinkOptions { PropagateCompletion = true }
                                    ) ;
                _finalBlock = _actionBlock ;
            }
            else
            {
                _finalBlock = _outputBlock ;
            }

            var req = new AnalysisRequest { Info = projectNode } ;
            if ( ! pipeline.Post ( req ) )
            {
                throw new AnalyzeException ( "Post failed" ) ;
            }

            await HandlePipelineResultAsync ( _finalBlock ) ;
        }

        public ITargetBlock < RejectedItem > RejectDestination
        {
            get { return _rejectDestination ; }
            set { _rejectDestination = value ; }
        }


        private async Task HandlePipelineResultAsync ( IDataflowBlock finalblock )
        {
            PipelineResult result ;
            try
            {
                await finalblock.Completion.ConfigureAwait ( true ) ;
                result = new PipelineResult ( ResultStatus.Success ) ;
            }
            catch ( AggregateException ex1 )
            {
                var exes = ex1.Flatten ( ).InnerExceptions ;
                Logger.Debug ( "final block completion threw exception" ) ;
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
        }
        #endregion
    }

    internal class GenericAnalyzeCommandImpl : GenericAnalyzeCommand < object >
    {
        public GenericAnalyzeCommandImpl (
            ITargetBlock < AnalysisRequest > acceptorBlock
          , ISourceBlock < object >          outputBlock
        ) : base ( acceptorBlock , outputBlock )
        {
        }
    }
}