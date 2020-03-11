using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using NLog ;
#if NUGET
using NuGet.Commands ;
using NuGet.ProjectModel ;
#endif

namespace ProjLib
{
    internal static class DataflowBlocks
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        
        public static TransformBlock <AnalysisRequest, Workspace > InitializeWorkspace ( )
        {
            var makeWs =
                new TransformBlock <AnalysisRequest, Workspace > ( Workspaces.MakeWorkspaceAsync ) ;
            return makeWs ;
        }

        public static TransformManyBlock < Document , ILogInvocation > FindLogUsages ( )
        {
            Logger.Trace ( "Constructing FindUsagesBlock" ) ;
            var findLogUsagesBlock =
                new TransformManyBlock < Document , ILogInvocation > ( ProjLib.FindLogUsages.FindUsagesFunc , new ExecutionDataflowBlockOptions() ) ;
            return findLogUsagesBlock ;
        }
#if USEMSBUILD
        public static TransformBlock < string , BuildResults > PackagesRestore ( )
        {
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => BuildTool.BuildRepository ( s )
                                                             ) ;
            return buildTransformBlock ;
        }
#else
        #if NUGET
        public static TransformBlock<string, string> PackagesRestore()
        {
            var buildTransformBlock =
                new TransformBlock<string, string>(
                                                         s => NugetTool.RestorePackages(s)
                                                        );
            return buildTransformBlock;
        }
        #endif
#endif

        #if !NETSTANDARD2_0
        public static TransformBlock <AnalysisRequest, AnalysisRequest> CloneSource ( )
        {
            return new TransformBlock <AnalysisRequest, AnalysisRequest> ( VersionControl.CloneProjectAsync ) ;
        }
#endif
    }
#if NUGET
    internal static class NugetTool
    {
        public static string RestorePackages ( string s ) { return s ; }
    }
#endif
}