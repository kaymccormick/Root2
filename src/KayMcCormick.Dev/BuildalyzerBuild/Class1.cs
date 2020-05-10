using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisAppLib;
using AnalysisAppLib.Dataflow;
using Buildalyzer;
using Buildalyzer.Workspaces;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BuildalyzerBuild
{
    public class Class1 : IDataflowTransformFuncProvider<AnalysisRequest, Workspace>
    {
        public Class1()
        {
            var solutionPath = "";
            var m = new AnalyzerManager(solutionPath);
        }

        public async Task<Workspace> MakeWorkspace2Async(
#pragma warning restore 1998
            [NotNull] AnalysisRequest req
            , [NotNull] Action<string> outAct
            , [CanBeNull] IEnumerable<Action<IEventMisc>> misc
        )
        {
            try
            {
                var manager = new AnalyzerManager(req.Info.SolutionPath);
                Workspace workspace = null;
                try
                {
                    workspace = new AdhocWorkspace();
                }
                catch
                {
                    // ignored
                }

                //  manager.LoggerFactory = lo ;
                foreach (var keyValuePair in manager.Projects)
                {
                    // Logger.Debug ( keyValuePair.Key ) ;
                    var projectAnalyzer = keyValuePair.Value;
                    // var logger = new Log1 ( outAct , projectAnalyzer , misc ) ;
                    //projectAnalyzer.AddBuildLogger (  ) ;
                    var b = projectAnalyzer.Build();
                    foreach (var analyzerResult in b.Results)
                    {
                        // Logger.Info ( "{r}" , analyzerResult.ToString ( ) ) ;
                    }

                    if (!b.OverallSuccess)
                        outAct?.Invoke($"{keyValuePair.Key} failed");
                    else
                        projectAnalyzer.AddToWorkspace(workspace);
                }

                if (workspace != null) return workspace;

                return null;
            }
            catch (Exception ex)
            {
                // Logger.Error ( ex , "here" ) ;
                throw;
            }
        }

        public Func<AnalysisRequest, Task<IEnumerable<Workspace>>> GetAsyncTransformFunction()
        {
            return async (request) =>
            {
                var result = await MakeWorkspace2Async(request, null, null);
                return new [] {result};
            };
        }
    }
}