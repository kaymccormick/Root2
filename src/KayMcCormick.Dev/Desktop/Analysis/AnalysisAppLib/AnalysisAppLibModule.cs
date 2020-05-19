
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Data ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Reactive.Subjects;
using System.Reflection ;
using System.Threading.Tasks ;
using AnalysisAppLib.Dataflow ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Features.AttributeFilters ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;
using Document = Microsoft.CodeAnalysis.Document;
// ReSharper disable UnusedParameter.Local

namespace AnalysisAppLib
{
    /// <summary>
    ///     Autofac module for the base Analysis App Lib.
    /// </summary>
    public sealed class AnalysisAppLibModule : IocModule
    {
        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool _registerExplorerTypes = false ;
        private readonly ReplaySubject<ActivationInfo> _activations = new ReplaySubject<ActivationInfo>();

        /// <summary>
        ///     Parameter-less constructor.
        /// </summary>
        public AnalysisAppLibModule ( ) { DebugUtils.WriteLine ( "here" ) ; }

        /// <summary>
        ///     Boolean indicating whether or not to register the "File explorer" types.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool RegisterExplorerTypes
        {
            get { return _registerExplorerTypes ; }
            set { _registerExplorerTypes = value ; }
        }

        /// <summary>
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <param name="registration"></param>
        protected override void AttachToComponentRegistration (
            IComponentRegistryBuilder componentRegistry
            // ReSharper disable once AnnotateNotNullParameter
          , IComponentRegistration registration
        )
        {
            // ReSharper disable once UnusedVariable
            var svc = String.Join ( "; " , registration.Services.Select ( s => s.ToString ( ) ) ) ;
            // DebugUtils.WriteLine (
                                  // $"{nameof ( AttachToComponentRegistration )}: {registration.Id} {registration.Lifetime} {svc}"
                                 // ) ;

            registration.Activated += (sender, args) =>
            {
                ActivationInfo x = new ActivationInfo()
                {
                    Instance = args.Instance,
                    Component = args.Component,
                    Parameters = args.Parameters,
                    Context = args.Context
                };
                _activations.OnNext(x);
            };
            registration.Activating += ( sender , args ) => {
                var inst = args.Instance ;
                // DebugUtils.WriteLine ( $"activating {inst} {registration.Lifetime}" ) ;
                if ( ! ( inst is IViewModel ) )
                {
                    return ;
                }

                switch ( inst )
                {
                    case ISupportInitializeNotification xx :
                    {
                        if ( ! xx.IsInitialized )
                        {
                            DebugUtils.WriteLine($"calling init on instance {xx}");
                            xx.BeginInit ( ) ;
                            xx.EndInit ( ) ;
                        }

                        break ;
                    }
                    case ISupportInitialize x :
                        DebugUtils.WriteLine($"calling init on instance {x}");
                        x.BeginInit ( ) ;
                        x.EndInit ( ) ;
                        break ;
                }
            } ;
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {

            builder.RegisterType<AppDbContext>().As<IAppDbContext1>().AsSelf().WithCallerMetadata().SingleInstance();
            builder.RegisterInstance(_activations);            
            builder.RegisterType<ResourceNodeInfo>().As<IHierarchicalNode>();
            builder.RegisterGeneric(typeof(ReplaySubject<>)).SingleInstance();
            
            builder.RegisterType < SyntaxTypesService > ( )
                   .As < ISyntaxTypesService > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < DocInterface > ( )
                   .As < IDocInterface > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterModule < LegacyAppBuildModule > ( ) ;
            if (RegisterModelResources)
            {
                builder.RegisterType<ModelResources>().WithCallerMetadata().SingleInstance();
            }
            
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
                   .WithAttributedMetadata ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType<AnalyzeCommandWrap>().AsImplementedInterfaces().WithCallerMetadata();
            builder.RegisterType < AnalyzeCommand > ( ).AsSelf()
                   .As < IAnalyzeCommand > ( ).AsImplementedInterfaces()
                   .WithCallerMetadata ( ) ;

            builder.RegisterGeneric ( typeof ( GenericAnalyzeCommand<>) )
                   .As ( typeof ( IAnalyzeCommand2<>) )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < Pipeline > ( ).AsSelf ( ).WithCallerMetadata ( ) ;

            builder.RegisterType < LogInvocation2 > ( )
                   .As < ILogInvocation > ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < FindLogInvocations > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < FindLogUsagesFuncProvider > ( )
                   .AsImplementedInterfaces ( )
                   .WithAttributeFiltering ( )
                   .AsSelf ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata ( ) ;

            builder.Register < Action<Document>> (
                                                                             ( c , p ) => doc => {
                                                                                 DebugUtils
                                                                                    .WriteLine (
                                                                                                doc
                                                                                                   .FilePath
                                                                                               ) ;
                                                                             }
                                                                            ) ;
            if ( true )
            {
                builder.RegisterGeneric ( typeof ( AnalysisBlockProvider<,,>) )
                       .As ( typeof ( IAnalysisBlockProvider<,,>) )
                       .WithAttributeFiltering ( )
                       .InstancePerLifetimeScope ( )
                       .WithCallerMetadata ( )
                       .WithMetadata ( "Purpose" , "Analysis" ) ;
            }

            if ( RegisterConcreteBlockProviders )
            {
                builder.RegisterGeneric ( typeof ( ConcreteAnalysisBlockProvider<,,>) )
                       .As ( typeof ( IAnalysisBlockProvider<,,>) )
                       .WithAttributeFiltering ( )
                       .InstancePerLifetimeScope ( )
                       .WithCallerMetadata ( )
                       .WithMetadata ( "Purpose" , "Analysis" ) ;
                builder.RegisterGeneric ( typeof ( ConcreteDataflowTransformFuncProvider<,>) )
                       .As ( typeof ( IDataflowTransformFuncProvider<,>) )
                       .WithAttributeFiltering ( )
                       .InstancePerLifetimeScope ( )
                       .WithMetadata ( "Purpose" , "Analysis" )
                       .WithCallerMetadata ( ) ;
            }

            #region MS LOGIN
            builder.Register ( MakePublicClientApplication )
                   .As < IPublicClientApplication > ( )
                   .WithCallerMetadata ( ) ;

            builder.Register (
                              ( ctx , p ) => {
                                  var bearerToken = p.TypedAs < string > ( ) ;
                                  return MakeGraphServiceClient ( bearerToken ) ;
                              }
                             )
                   .AsSelf ( )
                   .WithCallerMetadata ( ) ;
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public bool RegisterConcreteBlockProviders { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private bool RegisterModelResources { get; set; }

        [ NotNull ]
        // ReSharper disable once UnusedMember.Local
        private static DataTable DataAdapter (
            // ReSharper disable once UnusedParameter.Local
            IComponentContext         c
            // ReSharper disable once UnusedParameter.Local
            , IEnumerable<Parameter> p
          , [NotNull ] object        o
        )
        {
            var r = new DataTable ( o.GetType ( ).Name ) ;
            var values = new ArrayList ( ) ;
            foreach ( var p1 in o.GetType ( )
                                 .GetProperties ( BindingFlags.Instance | BindingFlags.Public ) )
            {
                object rr1 = null ;
                try { rr1 = p1.GetValue ( o ) ; }
                catch
                {
                    // ignored
                }

                if ( p1.GetMethod.GetParameters ( ).Any ( ) )
                {
                    continue ;
                }

                r.Columns.Add ( new DataColumn ( p1.Name , p1.PropertyType ) ) ;
                values.Add ( rr1 ) ;
            }

            r.LoadDataRow ( values.ToArray ( ) , LoadOption.OverwriteChanges ) ;
            return r ;
        }

        [ NotNull ]
        // ReSharper disable once UnusedMember.Local
        private Dictionary<string, object> DictAdapter (
            IComponentContext         c
          , IEnumerable<Parameter> p
          , [ NotNull ] object        o
        )
        {
            var r = new Dictionary<string, object>( ) ;
            foreach ( var p1 in o.GetType ( )
                                 .GetProperties ( BindingFlags.Instance | BindingFlags.Public ) )
            {
                object rr1 = null ;
                try { rr1 = p1.GetValue ( o ) ; }
                catch
                {
                    // ignored
                }

                r[ p1.Name ] = rr1 ;
            }

            return r ;
        }

        [ NotNull ]
        private static IPublicClientApplication MakePublicClientApplication (
            IComponentContext                     context
          , [ NotNull ] IEnumerable<Parameter> p
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
            //TokenCacheHelper.EnableSerialization ( a.UserTokenCache ) ;
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
}
