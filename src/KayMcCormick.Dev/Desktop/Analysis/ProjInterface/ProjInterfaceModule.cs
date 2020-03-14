﻿#region header
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

using NLog;
using ProjLib;
using ProjLib.Interfaces ;

namespace ProjInterface
{
#if MSBUILDWORKSPACE
    using Microsoft.CodeAnalysis.MSBuild ;
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
#else
    internal class StubWorkspaceManager : IWorkspaceManager
    {
        public Workspace CreateWorkspace(IDictionary<string, string> props)
        {
            return null;
        }
        public Task OpenSolutionAsync(Workspace workspace, string solutionPath)
        {
            return Task.CompletedTask;
        }
    }
#endif

    public class ProjInterfaceModule : Module
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            Logger.Trace("Load");
            builder.RegisterModule<ProjLibModule>();
#if MSBUILDWORKSPACE
            builder.RegisterType<MSBuildWorkspaceManager>().As<IWorkspaceManager>();
#else
            builder.RegisterType<StubWorkspaceManager>().As<IWorkspaceManager>();

#endif
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