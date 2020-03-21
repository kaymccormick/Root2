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
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Threading.Tasks ;
using System.Windows.Controls ;
using Autofac ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;
using NLog ;
using ProjLib ;
using ProjLib.Interfaces ;
using Logger = NLog.Logger ;

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
        public Workspace CreateWorkspace ( IDictionary < string , string > props ) { return null ; }

        public Task OpenSolutionAsync ( Workspace workspace , string solutionPath )
        {
            return Task.CompletedTask ;
        }
    }
#endif

    public class ProjInterfaceModule : IocModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder ) { DoLoad ( builder ) ; }
        #endregion

        public override void DoLoad ( ContainerBuilder builder )
        {
            Logger.Trace ( "Load" ) ;
            Logger.Warn (
                         $"Loading module {typeof ( ProjInterfaceModule ).AssemblyQualifiedName}"
                        ) ;
            builder.RegisterModule < ProjLibModule > ( ) ;
#if MSBUILDWORKSPACE
            builder.RegisterType<MSBuildWorkspaceManager>().As<IWorkspaceManager>();
#else
            builder.RegisterType < StubWorkspaceManager > ( ).As < IWorkspaceManager > ( ) ;

#endif
            LogRegistration ( typeof ( Window1 ) , "AsSelf" ) ;
            builder.RegisterType < Window1 > ( ).AsSelf ( ) ;
            LogRegistration ( typeof ( ProjMainWindow ) , "AsSelf" ) ;
            builder.Register (
                              ( context , parameters ) => new ProjMainWindow (
                                                                              context
                                                                                 .Resolve <
                                                                                      IWorkspacesViewModel
                                                                                  > ( )
                                                                            , context
                                                                                 .Resolve <
                                                                                      ILifetimeScope
                                                                                  > ( )
                                                                             )
                             )
                   .AsSelf ( ) ;
            LogRegistration ( typeof ( DockWindowViewModel ) , "AsSelf" ) ;
            builder.RegisterType < DockWindowViewModel > ( ).AsSelf ( ) ;
            LogRegistration ( typeof ( ProjMainWindow ) , "AsSelf" , typeof ( IView1 ) ) ;
            builder.RegisterType < ProjMainWindow > ( ).AsSelf ( ).As < IView1 > ( ) ;
            LogRegistration (
                             typeof ( AllResourcesTree )
                           , typeof ( UserControl )
                           , "AsSelf"
                           , typeof ( IView1 )
                            ) ;
            builder.RegisterType < AllResourcesTree > ( )
                   .As < UserControl > ( )
                   .AsSelf ( )
                   .As < IView1 > ( ) ;
            LogRegistration ( typeof ( AllResourcesTreeViewModel ) , "AsSelf" ) ;
            builder.RegisterType < AllResourcesTreeViewModel > ( ).AsSelf ( ) ;
            LogRegistration (
                             typeof ( LogUsageAnalysisViewModel )
                           , typeof ( ILogUsageAnalysisViewModel )
                            ) ;
            builder.RegisterType < LogUsageAnalysisViewModel > ( )
                   .As < ILogUsageAnalysisViewModel > ( ) ;
            LogRegistration ( typeof ( IconsSource ) , typeof ( IIconsSource ) ) ;
            builder.RegisterType < IconsSource > ( ).As < IIconsSource > ( ) ;
            //   builder.RegisterType < ShellExplorerItemProvider > ( ).As < IExplorerItemProvider> ( ) ;
            builder.RegisterType < FileSystemExplorerItemProvider > ( )
                   .As < IExplorerItemProvider > ( ) ;

            builder.Register (
                              ( context , p ) => {
                                  var a = PublicClientApplicationBuilder
                                         .CreateWithApplicationOptions (
                                                                        new
                                                                        PublicClientApplicationOptions
                                                                        {
                                                                            ClientId =
                                                                                p.TypedAs < Guid
                                                                                  > ( )
                                                                                 .ToString ( )
                                                                          , RedirectUri =
                                                                                "myapp://auth"
                                                                        }
                                                                       )
                                         .WithAuthority (
                                                         AadAuthorityAudience
                                                            .AzureAdAndPersonalMicrosoftAccount
                                                        )
                                         .Build ( ) ;
                                  TokenCacheHelper.EnableSerialization ( a.UserTokenCache ) ;
                                  return a ;
                              }
                             )
                   .As < IPublicClientApplication > ( ) ;
            builder.Register (
                              ( ctx , p ) => new GraphServiceClient (
                                                                     new
                                                                         DelegateAuthenticationProvider (
                                                                                                         (
                                                                                                             requestMessage
                                                                                                         ) => {
                                                                                                             requestMessage
                                                                                                                    .Headers
                                                                                                                    .Authorization
                                                                                                                 = new
                                                                                                                     AuthenticationHeaderValue (
                                                                                                                                                "Bearer"
                                                                                                                                              , p
                                                                                                                                                   .TypedAs
                                                                                                                                                    < string
                                                                                                                                                    > ( )
                                                                                                                                               ) ;
                                                                                                             return
                                                                                                                 Task
                                                                                                                    .FromResult (
                                                                                                                                 0
                                                                                                                                ) ;
                                                                                                         }
                                                                                                        )
                                                                    )
                             )
                   .AsSelf ( ) ;
            builder.RegisterType < LogViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < LogViewerWindow > ( ).AsSelf ( ) ;
            builder.RegisterType < LogViewerControl > ( ).AsSelf ( ) ;
        }
    }
}