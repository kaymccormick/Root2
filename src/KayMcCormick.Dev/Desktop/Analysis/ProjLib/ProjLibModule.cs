#region header
// Kay McCormick (mccor)
// 
// AnalyzeConsole
// ProjLib
// ProjLibModule.cs
// 
// 2020-03-09-10:06 PM
// 
// ---
#endregion
using Autofac ;
using NLog ;

namespace ProjLib
{
    public class ProjLibModule : Module
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            Logger.Trace ( "{methodName}" , nameof ( Load ) ) ;
            builder.RegisterType<WorkspacesViewModel>()
                   .As<IWorkspacesViewModel>()
                   .InstancePerLifetimeScope();
            builder.RegisterType < ProjectBrowserViewModel > ( )
                   .As < IProjectBrowserViewModel > ( ) ;

        }
        #endregion
    }
}