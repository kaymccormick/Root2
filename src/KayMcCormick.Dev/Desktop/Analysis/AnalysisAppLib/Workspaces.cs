using System ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Buildalyzer ;
using Buildalyzer.Workspaces ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisAppLib
{
    public static class Workspaces
    {
        private static readonly Logger            Logger = LogManager.GetCurrentClassLogger ( ) ;
        

        public static TransformBlock<AnalysisRequest, Workspace> InitializeWorkspace2Block()
        {
            var makeWs =
                new TransformBlock<AnalysisRequest, Workspace>(MakeWorkspace2Async);
            return makeWs;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public static async Task < Workspace > MakeWorkspace2Async ( [ NotNull ] AnalysisRequest req )
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                AnalyzerManager manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                AdhocWorkspace workspace = new AdhocWorkspace ( ) ;
                foreach ( var keyValuePair in manager.Projects )
                {
                    Logger.Debug ( keyValuePair.Key ) ;
                    keyValuePair.Value.Build ( ) ;
                    keyValuePair.Value.AddToWorkspace ( workspace ) ;
                }

                return workspace ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , "here" ) ;
                throw ;
            }
        }


        public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock ( )
        {
            return new TransformManyBlock < Workspace , Document > ( workspace => ParallelEnumerable.Where < Project > (
                                                                                                                  workspace
                                                                                                                     .CurrentSolution
                                                                                                                     .Projects.AsParallel (  )
                                                                                                                , project
                                                                                                                      => {
                                                                                                                      Logger
                                                                                                                         .Warn (
                                                                                                                                "{project}"
                                                                                                                              , project
                                                                                                                                   .Name
                                                                                                                               ) ;
                                                                                                                      return
                                                                                                                          true ;
                                                                                                                  }
                                                                                                                 )
                                                                                                    .SelectMany (
                                                                                                                 project
                                                                                                                     => project
                                                                                                                        .Documents
                                                                                                                )
                                                                                                    .Where (
                                                                                                            document
                                                                                                                => {
                                                                                                                Logger
                                                                                                                   .Info (
                                                                                                                          "{document}"
                                                                                                                        , document
                                                                                                                             .Name
                                                                                                                         ) ;
                                                                                                                return
                                                                                                                    true ;
                                                                                                            }
                                                                                                           ) );

        }
    }
}