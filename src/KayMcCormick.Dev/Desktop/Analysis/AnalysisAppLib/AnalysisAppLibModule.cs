using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Net.Http.Headers ;
using System.Reflection ;
using System.Threading.Tasks ;
using AnalysisAppLib.Auth ;
using AnalysisAppLib.Command ;
using AnalysisAppLib.Dataflow ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Features.AttributeFilters ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Autofac module for the base Analysis App Lib
    /// </summary>
    public sealed class AnalysisAppLibModule : IocModule
    {

        private bool _registerExplorerTypes = false ;
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public AnalysisAppLibModule ( ) { DebugUtils.WriteLine ( "here" ) ; }

        /// <summary>
        /// 
        /// </summary>
        public bool RegisterExplorerTypes
        {
            get { return _registerExplorerTypes ; }
            set { _registerExplorerTypes = value ; }
        }

        #region Overrides of Module
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <param name="registration"></param>
        protected override void AttachToComponentRegistration (
            IComponentRegistryBuilder componentRegistry
            // ReSharper disable once AnnotateNotNullParameter
          , IComponentRegistration registration
        )
        {
            registration.Activating += ( sender , args ) => {
                var inst = args.Instance ;
                DebugUtils.WriteLine ( $"activating {inst} {registration.Lifetime}" ) ;
                if ( ! ( inst is IViewModel ) )
                {
                    return ;
                }

                switch ( inst )
                {
                    case ISupportInitializeNotification xx :
                    {
                        DebugUtils.WriteLine ( "calling init on instance" ) ;
                        if ( ! xx.IsInitialized )
                        {
                            xx.BeginInit ( ) ;
                            xx.EndInit ( ) ;
                        }

                        break ;
                    }
                    case ISupportInitialize x :
                        DebugUtils.WriteLine ( "calling init on instance" ) ;
                        x.BeginInit ( ) ;
                        x.EndInit ( ) ;
                        break ;
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

            
            builder.RegisterType < SyntaxTypesService > ( ).As < ISyntaxTypesService > ( ).WithCallerMetadata() ;
            builder.RegisterType < DocInterface > ( ).As < IDocInterface > ( ).WithCallerMetadata();
            builder.RegisterModule < LegacyAppBuildModule > ( ) ;
            builder.RegisterType < ModelResources > ( ).SingleInstance ( ).WithCallerMetadata();
            builder.RegisterType < CodeGenCommand > ( ).AsSelf ( ).AsImplementedInterfaces ( ).WithCallerMetadata();
            builder.RegisterAssemblyTypes ( Assembly.GetExecutingAssembly ( ) )
                   .Where (
                           type => {
                               if ( builder.ComponentRegistryBuilder.IsRegistered (
                                                                                   new
                                                                                       TypedService (
                                                                                                     type
                                                                                                    )
                                                                                  ) )
                               {
                                   return false ;
                               }

                               var b = typeof ( IViewModel ).IsAssignableFrom ( type ) ;
                               return b ;
                           }
                          )
                   .AsImplementedInterfaces ( )
                   .AsSelf ( )
                   .WithAttributedMetadata ( ).WithCallerMetadata();

#if false
            builder.RegisterType < LogUsageAnalysisViewModel > ( )
                   .As < ILogUsageAnalysisViewModel > ( ) ;
            builder.RegisterType < FileSystemExplorerItemProvider > ( )
                   .As < IExplorerItemProvider > ( ) ;
            builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ;
#endif
            builder.RegisterType < AnalyzeCommand > ( ).As < IAnalyzeCommand > ( ).WithCallerMetadata(); 

            builder.RegisterGeneric ( typeof ( GenericAnalyzeCommand <> ) )
                   .As ( typeof ( IAnalyzeCommand2 <> ) ).WithCallerMetadata();
            builder.RegisterType < Pipeline > ( ).AsSelf ( ).WithCallerMetadata();


#if false
            builder.RegisterType < ProjectBrowserViewModel > ( )
                   .As < IProjectBrowserViewModel > ( ) ;
            
            /* Register the "Cache target view model. */

            builder.RegisterType < CacheTargetViewModel > ( ).AsSelf ( ) ;

            builder.RegisterType < SyntaxPanelViewModel > ( ).As < ISyntaxPanelViewModel > ( ) ;

            builder.RegisterType < SyntaxTokenViewModel > ( )
                   .As < ISyntaxTokenViewModel > ( )
                   .SingleInstance ( ) ;
#endif
            builder.RegisterType < LogInvocation2 > ( ).As < ILogInvocation > ( ).WithCallerMetadata();
            // builder.RegisterType < FindLogUsagesAnalysisDefinition > ( )
            // .As < IAnalysisDefinition > ( ) ;

            builder.RegisterType < FindLogInvocations > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( ).WithCallerMetadata();

            builder.RegisterType < FindLogUsagesFuncProvider > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( )
                   .AsSelf ( )
                   .InstancePerLifetimeScope ( ).WithCallerMetadata();



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
#if false
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
#endif


            builder.RegisterGeneric ( typeof ( AnalysisBlockProvider < , , > ) )
                   .As ( typeof ( IAnalysisBlockProvider < , , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata()
                   .WithMetadata ( "Purpose" , "Analysis" ) ;

            builder.RegisterGeneric ( typeof ( DataflowTransformFuncProvider < , > ) )
                   .As ( typeof ( IDataflowTransformFuncProvider < , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata()
                   .WithMetadata ( "Purpose" , "Analysis" ) ;
            
            builder.RegisterType < Example1TransformFuncProvider > ( ).AsSelf().AsImplementedInterfaces().WithCallerMetadata(); ;

            builder.RegisterGeneric ( typeof ( ConcreteAnalysisBlockProvider < , , > ) )
                   .As ( typeof ( IAnalysisBlockProvider < , , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata()
                   .WithMetadata ( "Purpose" , "Analysis" ) ;


            builder.RegisterGeneric ( typeof ( ConcreteDataflowTransformFuncProvider < , > ) )
                   .As ( typeof ( IDataflowTransformFuncProvider < , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithMetadata ( "Purpose" , "Analysis" ).WithCallerMetadata();


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

        [ NotNull ]
        private static IPublicClientApplication MakePublicClientApplication (
            IComponentContext                     context
          , [ NotNull ] IEnumerable < Parameter > p
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

        [ NotNull ]
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

        [ NotNull ]
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
    // ReSharper disable once UnusedType.Global
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
    // ReSharper disable once UnusedTypeParameter
    public interface IAnalysisDefinition < TOutput >
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Type DataflowOutputType { get ; set ; }
    }
}