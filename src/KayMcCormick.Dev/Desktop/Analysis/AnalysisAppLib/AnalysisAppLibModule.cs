using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Reflection ;
using System.Threading.Tasks ;
using AnalysisAppLib.Auth ;
using AnalysisAppLib.Dataflow ;
using AnalysisAppLib.Project ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Features.AttributeFilters ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;
using NLog ;
using Logger = NLog.Logger ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AnalysisAppLibModule : IocModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        #region Overrides of Module
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <param name="registration"></param>
        protected override void AttachToComponentRegistration (
            IComponentRegistryBuilder componentRegistry
          , IComponentRegistration    registration
        )
        {
            registration.Activated += ( sender , args ) => {
                Debug.WriteLine ( "activated" ) ;
                var inst = args.Instance ;
                if ( inst is IViewModel
                     && inst is ISupportInitialize x )
                {
                    Debug.WriteLine ( "calling init on instance" ) ;
                    x.BeginInit ( ) ;
                    x.EndInit ( ) ;
                }
            } ;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            builder.RegisterModule < LegacyAppBuildModule > ( ) ;
            builder.RegisterAssemblyTypes ( Assembly.GetExecutingAssembly() )
                   .Where (
                           type => {
                               var b = typeof ( IViewModel ).IsAssignableFrom ( type ) ;
                               Debug.WriteLine($"{type.FullName} - {b}");
                               return b ;
                           }
                           // || typeof ( IView1 ).IsAssignableFrom ( type )
                          )
                   .AsImplementedInterfaces ( )
                   .AsSelf ( )
                   .WithAttributedMetadata ( ) ;

            //builder.RegisterType<DockWindowViewModel>().AsSelf();
#if false
            builder.RegisterType < ModelResources > ( ) ;
            
            builder.RegisterType < LogUsageAnalysisViewModel > ( )
                   .As < ILogUsageAnalysisViewModel > ( ) ;
            builder.RegisterType < FileSystemExplorerItemProvider > ( )
                   .As < IExplorerItemProvider > ( ) ;
            builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ;
#endif
            builder.RegisterType < AnalyzeCommand > ( ).As < IAnalyzeCommand > ( ) ;

            builder.RegisterGeneric ( typeof ( GenericAnalyzeCommand <> ) )
                   .As ( typeof ( IAnalyzeCommand2 <> ) ) ;

#if false
            builder.RegisterType < ProjectBrowserViewModel > ( )
                   .As < IProjectBrowserViewModel > ( ) ;
            builder.RegisterType < Pipeline > ( ).AsSelf ( ) ;

            /* Register the "Cache target view model. */

            builder.RegisterType < CacheTargetViewModel > ( ).AsSelf ( ) ;

            builder.RegisterType < SyntaxPanelViewModel > ( ).As < ISyntaxPanelViewModel > ( ) ;

            builder.RegisterType < SyntaxTokenViewModel > ( )
                   .As < ISyntaxTokenViewModel > ( )
                   .SingleInstance ( ) ;
#endif
            builder.RegisterType < LogInvocation2 > ( ).As < ILogInvocation > ( ) ;
            // builder.RegisterType < FindLogUsagesAnalysisDefinition > ( )
            // .As < IAnalysisDefinition > ( ) ;
            builder.RegisterType < FindLogUsagesFuncProvider > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( )
                   .AsSelf ( )
                   .InstancePerLifetimeScope ( ) ;


            //builder.Register()
            //builder.Register ( typeof ( IAnalysisDefinition <> ) )
            //.As ( typeof ( IAnalyzeCommand2 <> ) ).As<IAnalyzeCommand3> (  ) ;
            //  (
            //      context
            //    , parameters
            //    , arg3
            //  ) => {
            //      return new
            //          GenericAnalyzeCommandImpl (
            //                                     context
            //                                        .Resolve
            //                                         < ITargetBlock
            //                                             < AnalysisRequest
            //                                             > > ( )
            //                                   , context
            //                                        .Resolve
            //                                         < ISourceBlock
            //                                             < object
            //                                             > > ( )
            //                                    ) ;
            //  }
            // ) ;

            builder.RegisterAssemblyTypes ( Assembly.GetExecutingAssembly()  )
                   .Where (
                           t => t.FindInterfaces (
                                                  ( type , criteria ) => {
                                                      if ( ! type.IsGenericType )
                                                      {
                                                          return false ;
                                                      }

                                                      var assemblyName =
                                                          type.GetGenericTypeDefinition ( )
                                                              .Assembly.GetName ( ) ;
                                                      // Logger.Warn ( $"assembly is {assemblyName}" ) ;
                                                      if ( assemblyName
                                                           != Assembly
                                                             .GetCallingAssembly ( )
                                                             .GetName ( ) )
                                                      {
                                                          return false ;
                                                      }

                                                      // Logger.Warn(
                                                      // $"{t.FullName} {type.FullName} {criteria}"
                                                      // );

                                                      if ( type.GetGenericTypeDefinition ( )
                                                           == typeof ( IAnalysisBlockProvider < , ,
                                                           > ) )
                                                      {
                                                          Logger.Warn (
                                                                       "Discovered class {type}"
                                                                      ) ;
                                                          return true ;
                                                      }

                                                      return false ;
                                                  }
                                                , t
                                                 )
                                 .Any ( )
                          )
                   .AsImplementedInterfaces ( ) ;


            //        .AsImplementedInterfaces ( )
            //        .WithAttributeFiltering ( )
            //        .InstancePerLifetimeScope ( ) ;

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


#region MS LOGIN
            builder.Register ( MakePublicClientApplication ).As < IPublicClientApplication > ( ) ;

            builder.Register (
                              ( ctx , p ) => {
                                  var bearerToken = p.TypedAs < string > ( ) ;
                                  return MakeGraphServiceClient ( bearerToken ) ;
                              }
                             )
                   .AsSelf ( ) ;
            #endregion
#if false
            builder.RegisterType < LogViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < LogViewerAppViewModel > ( ).AsSelf ( ) ;
            builder.Register ( ( c , p ) => new LogViewerConfig ( p.TypedAs < ushort > ( ) ) )
                   .AsSelf ( ) ;
            builder.RegisterType < MicrosoftUserViewModel > ( ).AsSelf ( ) ;
#endif
        }

        private IPublicClientApplication MakePublicClientApplication (
            IComponentContext         context
          , IEnumerable < Parameter > p
        )
        {
            var typedAs = p.TypedAs < Guid > ( ) ;

            var a = PublicClientApplicationBuilder
                   .CreateWithApplicationOptions (
                                                  new PublicClientApplicationOptions
                                                  {
                                                      ClientId    = typedAs.ToString ( )
                                                    , RedirectUri = "myapp://auth"
                                                  }
                                                 )
                   .WithAuthority ( AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount )
                   .Build ( ) ;
            TokenCacheHelper.EnableSerialization ( a.UserTokenCache ) ;
            return a ;
        }

        private static GraphServiceClient MakeGraphServiceClient ( string bearerToken )
        {
            var parameter = bearerToken ;
            var auth = new DelegateAuthenticationProvider (
                                                           AuthenticateRequestAsyncDelegate (
                                                                                             parameter
                                                                                            )
                                                          ) ;
            return new GraphServiceClient ( auth ) ;
        }

        private static AuthenticateRequestAsyncDelegate AuthenticateRequestAsyncDelegate (
            string parameter
        )
        {
            return requestMessage => {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue ( "Bearer" , parameter ) ;
                return Task.FromResult ( 0 ) ;
            } ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ TitleMetadata ( "Find and analyze usages of NLog logging." ) ]
    public sealed class FindLogUsagesAnalysisDefinition : IAnalysisDefinition < ILogInvocation >
    {
        private Type _dataflowOutputType = typeof ( ILogInvocation ) ;

        /// <summary>
        /// 
        /// </summary>
        public Type DataflowOutputType
        {
            get { return _dataflowOutputType ; }
            set { _dataflowOutputType = value ; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IAnalysisDefinition < TOutput >
    {
        /// <summary>
        /// 
        /// </summary>
        Type DataflowOutputType { get ; set ; }
    }
}