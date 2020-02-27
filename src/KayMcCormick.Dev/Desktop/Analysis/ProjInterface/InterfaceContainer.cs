#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjInterface
// Container.cs
// 
// 2020-02-19-2:20 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Diagnostics;
using System.Linq ;
using System.Windows.Documents ;
using Autofac;
using KayMcCormick.Dev ;
using ProjLib;

namespace ProjInterface
{
    public static class InterfaceContainer
    {
        public static ILifetimeScope Scope = null;
        public static ILifetimeScope GetContainer()
        {
            if (Scope != null)
            {
                return Scope;
            }

            var stackTrace = new System.Diagnostics.StackTrace();
            Debug.WriteLine(stackTrace.ToString());
            ContainerBuilder builder = new ContainerBuilder();
            
            builder.RegisterModule<IdGeneratorModule>();

            builder.RegisterType<ProjMainWindow>().AsSelf();
            builder.RegisterType<WorkspacesViewModel>().As<IWorkspacesViewModel>();
            builder.RegisterType<VsInstanceCollector>().As<IVsInstanceCollector>();
            builder.RegisterType < MruItemProvider > ( ).As < IMruItemProvider > ( ) ;

            
            #if adapter
            builder.RegisterAdapter < IVsInstance , IMruItems > (
                                                                 ( context , instance )
                                                                     => new
                                                                         MostRecentlyUsedAdapater (
                                                                                                   instance
                                                                                                  , context.Resolve<IMruItemProvider>())
                                                                ) ;
#endif
            builder.RegisterType < VsInstance > ( ).As < IVsInstance > ( ) ;
            Scope = builder.Build().BeginLifetimeScope();
            var mruItemses = Scope.Resolve < IEnumerable < IMruItems > > ( ) ;
            var viewModel = Scope.Resolve < IWorkspacesViewModel > ( ) ;
            // Debug.Assert ( mruItemses.Any ( ) ) ;
            return Scope;
        }
    }
}