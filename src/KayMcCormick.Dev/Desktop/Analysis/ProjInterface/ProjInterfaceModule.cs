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
using Autofac ;
using NLog ;
using ProjLib ;

namespace ProjInterface
{
    public class ProjInterfaceModule : Module
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            Logger.Trace("Load");
            builder.RegisterModule<ProjLibModule>();
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