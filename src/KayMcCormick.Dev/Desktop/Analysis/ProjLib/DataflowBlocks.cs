using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using AnalysisFramework.LogUsage ;
using AnalysisFramework.LogUsage.Interfaces ;
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
        
        public static TransformManyBlock < Document , ILogInvocation > FindLogUsages ( )
        {
            Logger.Trace ( "Constructing FindUsagesBlock" ) ;
            var findLogUsagesBlock =
                new TransformManyBlock < Document , ILogInvocation > ( ProjLib.FindLogUsages.FindUsagesFunc , new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4 } ) ;
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

        #if VERSIONCONTROL
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
