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
using System.Diagnostics;
using Autofac;
using ProjLib;

namespace ProjInterface
{
    public static class Container
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
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<WorkspacesViewModel>().As<IWorkspacesViewModel>();
            builder.RegisterType<VsInstanceCollector>().As<IVsInstanceCollector>();
            Scope = builder.Build().BeginLifetimeScope();
            return Scope;
        }
    }
}