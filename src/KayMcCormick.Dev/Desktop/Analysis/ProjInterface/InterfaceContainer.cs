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
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows.Documents ;
using Autofac ;
using CodeAnalysisApp1 ;
using KayMcCormick.Dev ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using ProjLib ;

namespace ProjInterface
{
    public static class InterfaceContainer
    {
        public static ILifetimeScope Scope = null ;

        public static ILifetimeScope GetContainer ( )
        {
            if ( Scope != null )
            {
                return Scope ;
            }

            var stackTrace = new StackTrace ( ) ;
            Debug.WriteLine ( stackTrace.ToString ( ) ) ;
            var builder = new ContainerBuilder ( ) ;

            builder.RegisterModule < IdGeneratorModule > ( ) ;

            builder.RegisterType < ProjMainWindow > ( ).AsSelf ( ) ;
            builder.RegisterType < WorkspacesViewModel > ( )
                   .As < IWorkspacesViewModel > ( )
                   .InstancePerLifetimeScope ( ) ;
            builder.Register (
                              ( context , parameters ) => {
                                  // var inst = context.Resolve < VisualStudioInstance > ( ) ;
                                  return new TransformBlock < string , Workspace > (
                                                                                    s => ProjLibUtils
                                                                                       .LoadSolutionInstanceAsync (
                                                                                                                   null
                                                                                                                 , s
                                                                                                                 , null
                                                                                                                  )
                                                                                   ) ;
                              }
                             ) ;
                             


            builder.RegisterType < VsInstanceCollector > ( ).As < IVsInstanceCollector > ( ) ;
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
            Scope = builder.Build ( ).BeginLifetimeScope ( ) ;
            var mruItemses = Scope.Resolve < IEnumerable < IMruItems > > ( ) ;
            var viewModel = Scope.Resolve < IWorkspacesViewModel > ( ) ;
            // Debug.Assert ( mruItemses.Any ( ) ) ;
            return Scope ;
        }
    }

    public class FindLogUsagesBlock
    {
        private ITargetBlock < LogInvocation > _target ;

        public IReceivableSourceBlock < LogInvocation > Source => _source ;

        private readonly ActionBlock < Document > actionBlock ;
        private readonly IReceivableSourceBlock < LogInvocation > _source = new BufferBlock < LogInvocation > ( ) ;

        public FindLogUsagesBlock ( ITargetBlock < LogInvocation > target )
        {
            this._target = target ;
            actionBlock = new ActionBlock < Document > (
                                                        Action
                                                       ) ;
        }

        private async Task Action ( Document d )
        {
            var tree = await d.GetSyntaxTreeAsync ( ) ;
            var model = await d.GetSemanticModelAsync ( ) ;
            LogUsages.FindLogUsages (
                                     new CodeSource ( d.FilePath )
                                   , tree.GetCompilationUnitRoot ( )
                                   , model
                                   , invocation => _target.Post ( invocation )
                                   , false
                                   , false
                                   , ( p ) => LogUsages.ProcessInvocation ( p )
                                   , tree
                                    ) ;
            _target.Complete();
        }

        public ITargetBlock < Document > Target { get { return actionBlock ; } }
    }


}