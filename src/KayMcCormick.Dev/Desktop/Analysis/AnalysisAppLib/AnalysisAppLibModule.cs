using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Text ;
using System.Threading.Tasks ;
using AnalysisAppLib.Auth ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using KayMcCormick.Dev ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib
{
    public sealed class AnalysisAppLibModule : IocModule
    {
        #region Overrides of IocModule
        public override void DoLoad ( ContainerBuilder builder )
        {
            builder.RegisterModule < AppBuildModule > ( ) ;
            builder.RegisterType < DockWindowViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < LogUsageAnalysisViewModel > ( )
                   .As < ILogUsageAnalysisViewModel > ( ) ;
            builder.RegisterType < FileSystemExplorerItemProvider > ( )
                   .As < IExplorerItemProvider > ( ) ;
            builder.RegisterType < WorkspacesViewModel > ( )
                   .As < IWorkspacesViewModel > ( )
                   .InstancePerLifetimeScope ( ) ;
            builder.RegisterType < Workspaces > ( ).AsSelf ( ) ;
            builder.RegisterType < ProjectBrowserViewModel > ( )
                   .As < IProjectBrowserViewModel > ( ) ;
            builder.RegisterType < Pipeline > ( ).AsSelf ( ) ;
            builder.RegisterType < CacheTargetViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < SyntaxPanelViewModel > ( ).As < ISyntaxPanelViewModel > ( ) ;
            
            builder.RegisterType < ComponentViewModel > ( ).As < IComponentViewModel > ( ) ;
            builder.RegisterType < ApplicationViewModel > ( )
                   .As < IApplicationViewModel > ( )
                   .SingleInstance ( ) ;

#if MSBUILDWORKSPACE
            builder.RegisterType<MSBuildWorkspaceManager>().As<IWorkspaceManager>();
#else
            

#endif


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
                                                                                                         requestMessage
                                                                                                             => {
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
            builder.RegisterType < LogViewerAppViewModel > ( ).AsSelf ( ) ;
            builder.Register ( ( c , p ) => new LogViewerConfig ( p.TypedAs < ushort > ( ) ) )
                   .AsSelf ( ) ;
            builder.RegisterType < MicrosoftUserViewModel > ( ).AsSelf ( ) ;
        }
        #endregion
    }
}