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
using System.Data.SqlClient ;
using System.Diagnostics ;
using System.Threading.Tasks.Dataflow ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    public static class InterfaceContainer
    {
        public static ILifetimeScope Scope = null ;

        public static ILifetimeScope GetContainer (params IModule[] modules)
        {
            if ( Scope != null )
            {
                return Scope ;
            }

            var stackTrace = new StackTrace ( ) ;
            Debug.WriteLine ( stackTrace.ToString ( ) ) ;
            var builder = new ContainerBuilder ( ) ;

            builder.Register (
                              ( context , parameters )
                                  => {
                                  // SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
                                  // b.InitialCatalog = 
                                  return new SqlConnection (
                                                            @"Data Source=.\sql2017; Initial Catalog=xaml"
                                                           ) ;
                              }
                             )
                   .As < SqlConnection > ( )
                   .InstancePerLifetimeScope ( ) ;

            var a = new[]
                    {
                        typeof ( InterfaceContainer ).Assembly , typeof ( IViewModel ).Assembly
                    } ;
            builder.RegisterAssemblyTypes ( a )
                   .Where ( type => typeof ( IViewModel ).IsAssignableFrom ( type ) )
                   .AsImplementedInterfaces( ) ;
            builder.RegisterModule < IdGeneratorModule > ( ) ;

            builder.RegisterType < WorkspacesViewModel > ( )
                   .As < IWorkspacesViewModel > ( )
                   .InstancePerLifetimeScope ( ) ;

            builder.RegisterType < VsInstanceCollector > ( ).As < IVsInstanceCollector > ( ) ;
            builder.RegisterType<Pipeline>().AsSelf();
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
            
            // var mruItemses = Scope.Resolve < IEnumerable < IMruItems > > ( ) ;
            // var viewModel = Scope.Resolve < IWorkspacesViewModel > ( ) ;

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
            Scope = builder.Build().BeginLifetimeScope();
            // Debug.Assert ( mruItemses.Any ( ) ) ;
            return Scope ;
        }
    }
}
