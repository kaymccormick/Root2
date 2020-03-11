#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// ProjInterfaceModule.cs
// 
// 2020-03-08-7:55 PM
// 
// ---
#endregion
using System.Collections.Generic;
using System.Threading.Tasks ;
using Autofac;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog;
using ProjLib;

namespace ProjInterface
{

    internal class MSBuildWorkspaceManager : IWorkspaceManager
    {
        public Workspace CreateWorkspace(IDictionary<string, string> props)
        {
           return MSBuildWorkspace.Create(props);
        }
        public Task OpenSolutionAsync(Workspace workspace, string solutionPath) {
            return ((MSBuildWorkspace)workspace).OpenSolutionAsync(solutionPath);
        }
    }

    public class ProjInterfaceModule : Module
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            Logger.Trace("Load");
            builder.RegisterModule<ProjLibModule>();
            builder.RegisterType<MSBuildWorkspaceManager>().As<IWorkspaceManager>();
            builder.Register (
                              ( context , parameters )
                                  => new ProjMainWindow (
                                                         context
                                                            .Resolve < IWorkspacesViewModel > ( )
                                                       , context.Resolve < ILifetimeScope > ( )
                                                        )
                             )
                   .AsSelf ( ) ;
        }
        #endregion
    }
}
