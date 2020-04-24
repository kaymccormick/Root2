﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Data ;
using System.Linq ;
using System.Net.Http.Headers ;
using System.Reflection ;
using System.Threading.Tasks ;
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
using Microsoft.EntityFrameworkCore ;
using Microsoft.Extensions.Logging ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib
{
    /// <summary>
    ///     Autofac module for the base Analysis App Lib.
    /// </summary>
    public sealed class AnalysisAppLibModule : IocModule
    {
        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool _registerExplorerTypes = false ;

        /// <summary>
        ///     Parameterless constructor.
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
            var svc = string.Join ( "; " , registration.Services.Select ( s => s.ToString ( ) ) ) ;
            DebugUtils.WriteLine (
                                  $"{nameof ( AttachToComponentRegistration )}: {registration.Id} {registration.Lifetime} {svc}"
                                 ) ;
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

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            builder.Register (
                              ( c , p ) => {
                                  var loggerProviders =
                                      c.Resolve < IEnumerable < ILoggerProvider > > ( ) ;
                                  return new LoggerFactory ( loggerProviders ) ;
                              }
                             )
                   .As < ILoggerFactory > ( ) ;
            builder.RegisterType < Myw > ( ).AsImplementedInterfaces ( ).AsSelf ( ) ;
            builder.Register<Func<object, DataTable>> ( ( c , p ) => ( object o ) => DataAdapter ( c , p , o ) ).As<Func <object, DataTable> > (  ) ;
            builder.RegisterAdapter<object, DataTable>(
                                                       DataAdapter);
            builder.RegisterAdapter<object, IDictionary>(
                                                       DictAdapter);
            builder.RegisterType<AppDbContext>().As<DbContext>();
            builder.RegisterType < SyntaxTypesService > ( )
                   .As < ISyntaxTypesService > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < DocInterface > ( )
                   .As < IDocInterface > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterModule < LegacyAppBuildModule > ( ) ;
            builder.RegisterType < ModelResources > ( ).WithCallerMetadata ( ).SingleInstance ( ) ;
            builder.RegisterType < CodeGenCommand > ( )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( )
                   .WithCallerMetadata ( ) ;
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

            builder.RegisterType < AnalyzeCommand > ( )
                   .As < IAnalyzeCommand > ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterGeneric ( typeof ( GenericAnalyzeCommand <> ) )
                   .As ( typeof ( IAnalyzeCommand2 <> ) )
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

            builder.Register < Action <Microsoft.CodeAnalysis.Document> > (
                                                                                   (c, p) => (Microsoft.CodeAnalysis.Document doc) => {
                                                                                       DebugUtils.WriteLine ( doc.FilePath ) ;
                                                                                       
                                                                                   }
                                                                                  ) ;

            builder.RegisterGeneric ( typeof ( AnalysisBlockProvider < , , > ) )
                   .As ( typeof ( IAnalysisBlockProvider < , , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata ( )
                   .WithMetadata ( "Purpose" , "Analysis" ) ;

            builder.RegisterGeneric ( typeof ( DataflowTransformFuncProvider < , > ) )
                   .As ( typeof ( IDataflowTransformFuncProvider < , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata ( )
                   .WithMetadata ( "Purpose" , "Analysis" ) ;

            builder.RegisterType < Example1TransformFuncProvider > ( )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterGeneric ( typeof ( ConcreteAnalysisBlockProvider < , , > ) )
                   .As ( typeof ( IAnalysisBlockProvider < , , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata ( )
                   .WithMetadata ( "Purpose" , "Analysis" ) ;


            builder.RegisterGeneric ( typeof ( ConcreteDataflowTransformFuncProvider < , > ) )
                   .As ( typeof ( IDataflowTransformFuncProvider < , > ) )
                   .WithAttributeFiltering ( )
                   .InstancePerLifetimeScope ( )
                   .WithMetadata ( "Purpose" , "Analysis" )
                   .WithCallerMetadata ( ) ;

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
#if false
            builder.RegisterType < LogViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < LogViewerAppViewModel > ( ).AsSelf ( ) ;
            builder.Register ( ( c , p ) => new LogViewerConfig ( p.TypedAs < ushort > ( ) ) )
                   .AsSelf ( ) ;
            builder.RegisterType < MicrosoftUserViewModel > ( ).AsSelf ( ) ;
#endif
        }

        private DataTable DataAdapter ( IComponentContext c , IEnumerable < Parameter > p , object o )
        {
            var r = new DataTable ( o.GetType ( ).Name ) ;
            foreach ( var p1 in o.GetType ( ).GetProperties ( BindingFlags.Instance | BindingFlags.Public ) )
            {
                Prop px = new Prop { Name = p1.Name } ;
                object rr1 = null ;
                try { rr1 = p1.GetValue ( o ) ; }
                catch { }

                r.Columns.Add ( new DataColumn ( p1.Name , p1.PropertyType ) ) ;
            }

            return r ;
        }

        private Dictionary < string , object > DictAdapter(IComponentContext c, IEnumerable<Parameter> p, object o)
        {
            var r = new Dictionary < string , object > ( ) ;
            foreach (var p1 in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                
                Prop px = new Prop { Name = p1.Name };
                object rr1 = null;
                try { rr1 = p1.GetValue(o); }
                catch { }

                r[ p1.Name ] = rr1 ;
            }

            return r;
        }
        [NotNull ]
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

    internal class Prop
    {
        private string _name ;
        public string Name { get { return _name ; } set { _name = value ; } }
    }

    /// <summary>
    /// </summary>
    [ TitleMetadata ( "Find and analyze usages of NLog logging." ) ]
    // ReSharper disable once UnusedType.Global
    public sealed class FindLogUsagesAnalysisDefinition : IAnalysisDefinition < ILogInvocation >
    {
        private Type _dataflowOutputType = typeof ( ILogInvocation ) ;

        /// <summary>
        /// </summary>
        public Type DataflowOutputType
        {
            get { return _dataflowOutputType ; }
            set { _dataflowOutputType = value ; }
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IAnalysisDefinition < TOutput >
    {
        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Type DataflowOutputType { get ; set ; }
    }
}
