using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace ProjLib
{
    internal static class DataflowBlocks
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static TransformBlock < string, Workspace > InitializeWorkspace ( )
        {
            var makeWs =
                new TransformBlock < string, Workspace > ( Workspaces.MakeWorkspaceAsync ) ;
            return makeWs ;
        }

        public static TransformManyBlock < Document , ILogInvocation > FindLogUsages ( )
        {
            var findLogUsagesBlock =
                new TransformManyBlock < Document , ILogInvocation > ( ProjLib.FindLogUsages.FindUsagesFunc ) ;
            return findLogUsagesBlock ;
        }

        public static TransformBlock < string , BuildResults > PackagesRestore ( )
        {
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => BuildTool.BuildRepository ( s )
                                                             ) ;
            return buildTransformBlock ;
        }

        public static TransformBlock < string , string > ClonseSource ( )
        {
            return new TransformBlock < string , string > ( VersionControl.CloneProjectAsync ) ;
        }
    }
}