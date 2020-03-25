using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Text ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Auth ;
using AnalysisAppLib.Dataflow ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using Autofac.Features.AttributeFilters ;
using KayMcCormick.Dev ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib
{
    public sealed class AnalysisAppLibModule : IocModule
    {
        
        public override void DoLoad ( ContainerBuilder builder )
        {
            builder.RegisterModule < AppBuildModule > ( ) ;
            builder.RegisterType < DockWindowViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < LogUsageAnalysisViewModel > ( )
                   .As < ILogUsageAnalysisViewModel > ( ) ;
            builder.RegisterType < FileSystemExplorerItemProvider > ( )
                   .As < IExplorerItemProvider > ( ) ;
            builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ; 
            builder.RegisterType < AnalyzeCommand > ( ).As < IAnalyzeCommand > ( ) ;
            builder.RegisterType < LogInvocation2 > ( ).As < ILogInvocation > ( ) ;
            builder.RegisterType < ProjectBrowserViewModel > ( )
                   .As < IProjectBrowserViewModel > ( ) ;
            builder.RegisterType < Pipeline > ( ).AsSelf ( ) ;
            builder.RegisterType < CacheTargetViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < SyntaxPanelViewModel > ( ).As < ISyntaxPanelViewModel > ( ) ;
            
            builder.RegisterType < SyntaxTokenViewModel > ( )
                   .As < ISyntaxTokenViewModel> ( )
                   .SingleInstance ( ) ;

            
            builder.RegisterType < FindLogInvocations > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( ) ;
            builder.RegisterType < FindLogUsagesFuncProvider > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( ).AsSelf()
                   .InstancePerLifetimeScope ( ) ;
            // builder.RegisterGeneric ( typeof ( AnalysisBlockProvider < , , > ) )
                   // .As ( typeof ( IAnalysisBlockProvider < , , > ) )
                   // .WithAttributeFiltering ( )
                   // .InstancePerLifetimeScope ( ) ;
            // builder.RegisterGeneric ( typeof ( DataflowTransformFuncProvider < , > ) )
                   // .As ( typeof ( IDataflowTransformFuncProvider < , > ) )
                   // .WithAttributeFiltering ( )
                   // .InstancePerLifetimeScope ( ) ;
            // builder.RegisterGeneric(typeof(ConcreteAnalysisBlockProvider<,,>))
                   // .As(typeof(IAnalysisBlockProvider<,,>))
                   // .WithAttributeFiltering()
                   // .InstancePerLifetimeScope();
            // builder.RegisterGeneric(typeof(ConcreteDataflowTransformFuncProvider<,>))
                   // .As(typeof(IDataflowTransformFuncProvider<,>))
                   // .WithAttributeFiltering()
                   // .InstancePerLifetimeScope();

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
    }
}