using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using Autofac.Core.Registration ;
using Autofac.Core.Resolving ;
using Autofac.Extras.AttributeMetadata ;
using KayMcCormick.Dev.AppBuild ;
using KayMcCormick.Dev.Interfaces ;
using NLog ;
using Module = Autofac.Module ;

namespace KayMcCormick.Dev
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ContainerHelper
    public class AppBuildModule : Module
    {
        /// <summary>The assemblies for scanning property</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for AssembliesForScanningProperty
        public const string AssembliesForScanningProperty = "AssembliesForScanning" ;
#if ENABLE_BUILDERPROXY
        internal static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator ( ) ;
#endif

        private static readonly Logger Logger = LogManager.GetLogger ( "ContainerHelper" ) ;
        private static readonly Random Random = new Random ( ) ;

#pragma warning restore CA2211 // Non-constant fields should not be visible

#pragma warning restore 169

        /// <summary>Property name used to propagate the value of <see cref="DoInterception"/>.</summary>
        public const string InterceptProperty = "Intercept" ;

        /// <summary>Setups the container.</summary>
        /// <param name="container">The container.</param>
        /// <param name="containerHelperSettings"></param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for SetupContainer
        [ Obsolete ]
        public static ILifetimeScope SetupContainer (
            out IContainer          container
          , ContainerHelperSettings containerHelperSettings
        )
        {
            if ( containerHelperSettings != null )
            {
                Logger.Info ( "Applying container helper settings from app.config" ) ;
                containerHelperSettings.ApplySettings ( ) ;
            }
            else
            {
                Logger.Debug ( "No containerHelperSettings to apply." ) ;
            }

#if ENABLE_BUILDERPROXY
BuilderInterceptor builderInterceptor = null ;
            if ( DoProxyBuilder )
            {
                Logger.Info ( "Proxying container builder for debug purposes." ) ;
                var proxyGenerator = ProxyGenerator ;
                builderInterceptor = new BuilderInterceptor ( proxyGenerator ) ;
                var proxy =
                    proxyGenerator.CreateClassProxy < ContainerBuilder > ( builderInterceptor ) ;

                builder = proxy ;
            }
else
            {
                builder = new ContainerBuilder ( ) ;
            }
#else
            var builder = new ContainerBuilder ( ) ;
#endif



            #region Container Build
            var setupContainer = builder.Build ( ) ;
            container = setupContainer ;
            setupContainer.ChildLifetimeScopeBeginning +=
                SetupContainerOnChildLifetimeScopeBeginning ;
            #endregion
            // setupContainer.CurrentScopeEnding        += SetupContainerOnCurrentScopeEnding ;
            // setupContainer.ResolveOperationBeginning += SetupContainerOnResolveOperationBeginning ;


            var beginLifetimeScope = setupContainer.BeginLifetimeScope ( "initial scope" ) ;

            return beginLifetimeScope ;
        }

        #region Overrides of Module
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load ( ContainerBuilder builder )
        {
            ICollection < Assembly > assembliesToScan = null ;
            if ( assembliesToScan == null )
            {
                assembliesToScan = GetAssembliesForScanningViaTypes ( ) ;
            }

            AppBuild ( assembliesToScan , builder ) ;
        }
        #endregion

        private void AppBuild (
            IEnumerable < Assembly > assembliesToScan
          , ContainerBuilder         builder
        )
        {
            // Set the property in order to propagate the settings.
            builder.Properties[ InterceptProperty ] = DoInterception ;

            var toScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray ( ) ;

            builder.Properties[ AssembliesForScanningProperty ] = GetAssembliesForScanning ( ) ;
            #region Autofac Modules
            builder.RegisterModule < AttributedMetadataModule > ( ) ;
            builder.RegisterModule < AppBuildModule.IdGeneratorModule > ( ) ;
#if MENUS_ENABLE
            builder.RegisterModule < > ( ) ;
#endif
            #endregion

            var i = 0 ;

            void LogStuff ( ref int index )
            {
#if ENABLE_BUILDERPROXY
                builderInterceptor?.Invocations.ForEach (
                                                         invocation
                                                             => Logger.Debug (
                                                                              $"{index}]: {invocation.Method.Name} ({string.Join ( ", " , invocation.Arguments )}) => {invocation.OriginalReturnValue}"
                                                                             )
                                                        ) ;
#endif
                index ++ ;
            }

            LogStuff ( ref i ) ;

            #region Assembly scanning
            
            #endregion
            #region Interceptors
            if ( DoInterception )
            {
                builder.RegisterType < LoggingInterceptor > ( ).AsSelf ( ) ;
            }
            #endregion

            #region Logging
            builder.RegisterType < LoggerTracker > ( )
                   .As < ILoggerTracker > ( )
                   .InstancePerLifetimeScope ( ) ;

            builder.Register (
                              ( c , p ) => {
                                  var loggerName = "unset" ;
                                  try
                                  {
                                      loggerName = p.TypedAs < Type > ( ).FullName ;
                                  }
                                  catch ( Exception ex )
                                  {
                                      Console.WriteLine ( ex.ToString ( ) ) ;
                                  }

                                  var tracker = c.Resolve < ILoggerTracker > ( ) ;
                                  Logger.Trace ( $"creating logger loggerName = {loggerName}" ) ;
                                  var logger = LogManager.GetLogger ( loggerName ) ;
                                  tracker.TrackLogger ( loggerName , logger ) ;
                                  return logger ;
                              }
                             )
                   .As < ILogger > ( ) ;
            #endregion

            #region Callbacks
            builder.RegisterBuildCallback ( c => Logger.Info ( "Container built." ) ) ;
            builder.RegisterCallback (
                                      registry => {
                                          registry.Registered += ( sender , args ) => {
                                              Logger.Trace (
                                                            "Registered "
                                                            + args.ComponentRegistration.Activator
                                                                  .LimitType
                                                           ) ;
                                              args.ComponentRegistration.Activated +=
                                                  ( o , eventArgs ) => {
                                                      Logger.Trace (
                                                                    $"Activated {DescribeComponent ( eventArgs.Component )} (sender={o}, instance={eventArgs.Instance})"
                                                                   ) ;
                                                  } ;
                                          } ;
                                      }
                                     ) ;
            #endregion
        }

        private void SetupContainerOnResolveOperationBeginning (
            object                             sender
          , ResolveOperationBeginningEventArgs e
        )
        {
            e.ResolveOperation.CurrentOperationEnding += ResolveOperationOnCurrentOperationEnding ;
            e.ResolveOperation.InstanceLookupBeginning +=
                ResolveOperationOnInstanceLookupBeginning ;
            Logger.Info ( $"{nameof ( SetupContainerOnResolveOperationBeginning )} " ) ;
        }

        private void ResolveOperationOnInstanceLookupBeginning (
            object                           sender
          , InstanceLookupBeginningEventArgs e
        )
        {
            Logger.Info ( $"{nameof ( ResolveOperationOnInstanceLookupBeginning )}" ) ;
        }

        private void ResolveOperationOnCurrentOperationEnding (
            object                          sender
          , ResolveOperationEndingEventArgs e
        )
        {
            Logger.Info ( $"{nameof ( ResolveOperationOnCurrentOperationEnding )}" ) ;
        }

        /// <summary>Gets or sets a value indicating whether install interceptors for built objects.</summary>
        /// <value>
        ///   <see language="true"/> to perform interception; otherwise, <see language="false"/>.</value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool DoInterception { get ; set ; } = true ;

        /// <summary>
        ///     Gets or sets a value indicating whether to trace conditional
        ///     registration.
        /// </summary>
        /// <value>
        ///     <see language="true"/> to trace conditional registration; otherwise,
        ///     <see language="false"/>.
        /// </value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public static bool DoTraceConditionalRegistration { get ; set ; }

        /// <summary>Gets or sets a value indicating whether to proxy the ContainerBuilder.</summary>
        /// <value>
        ///     <see language="true"/> to proxy builder; otherwise, <see language="false"/>.
        /// </value>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable once UnusedMember.Global
        public bool DoProxyBuilder { get ; set ; } = true ;

        /// <summary>Gets the assemblies for scanning.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetAssembliesForScanning
        public ICollection < Assembly > GetAssembliesForScanning ( )
        {
            if ( GetAssembliesViaReferences )
            {
                return GetAssembliesForScanningByReferences ( ) ;
            }

            return GetAssembliesForScanningViaTypes ( ) ;
        }

        /// <summary>Gets or sets a value indicating whether [get assemblies via references].</summary>
        /// <value>
        ///   <see language="true"/> if [get assemblies via references]; otherwise, <see language="false"/>.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetAssembliesViaReferences
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool GetAssembliesViaReferences { get ; set ; }

        /// <summary>Gets the assemblies for scanning.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetAssembliesForScanning
        public static ICollection < Assembly > GetAssembliesForScanningByReferences ( )
        {
            throw new NotImplementedException ( ) ;
        }

        /// <summary>Gets the assemblies for scanning via types.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetAssembliesForScanningViaTypes
        public ICollection < Assembly > GetAssembliesForScanningViaTypes ( )
        {
            Logger.Debug (
                          "Getting assemblies to scan based on AssemblyContainerScan attribute."
                         ) ;

            Type[] ary = { typeof ( IHaveObjectId ) , typeof ( AppBuildModule ) } ;
            var forScanning = ary.Select ( ( type , i ) => type.Assembly ).ToList ( ) ;
            Logger.Info (
                         "Assemblies "
                         + string.Join (
                                        ", "
                                      , forScanning.Select (
                                                            ( assembly , i1 )
                                                                => assembly.GetName ( ).Name
                                                           )
                                       )
                        ) ;
            return forScanning ;
        }

        private void SetupContainerOnCurrentScopeEnding (
            object                       sender
          , LifetimeScopeEndingEventArgs e
        )
        {
            Logger.Info (
                         $"{nameof ( SetupContainerOnCurrentScopeEnding )} {e.LifetimeScope.Tag}"
                        ) ;
        }

        private static void SetupContainerOnChildLifetimeScopeBeginning (
            object                          sender
          , LifetimeScopeBeginningEventArgs e
        )
        {
            var n = Random.Next ( 1024 ) ;
            Logger.Info ( $"Child lifetime scope beginning {n}:  {e.LifetimeScope.Tag}" ) ;
            e.LifetimeScope.ChildLifetimeScopeBeginning +=
                SetupContainerOnChildLifetimeScopeBeginning ;
        }

        private static string DescribeComponent ( IComponentRegistration eventArgsComponent )
        {
            var debugDesc = "no description" ;
            const string key = "DebugDescription" ;
            if ( eventArgsComponent.Metadata.ContainsKey ( key ) )
            {
                debugDesc = eventArgsComponent.Metadata[ key ].ToString ( ) ;
            }

            return $" CompReg w({eventArgsComponent.Id}, {debugDesc})" ;
        }

        // ReSharper disable once UnusedMember.Global
        /// <summary>Dumps the specified component registry registration.</summary>
        /// <param name="componentRegistryRegistration">
        ///     The component registry
        ///     registration.
        /// </param>
        /// <param name="seenObjects">The seen objects.</param>
        /// <param name="outFunc">The out function.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Dump
        public static void Dump (
            IComponentRegistration componentRegistryRegistration
          , HashSet < object >     seenObjects
          , Action < string >      outFunc
        )
        {
            var activatorLimitType = componentRegistryRegistration.Activator.LimitType ;

            if ( seenObjects.Contains ( componentRegistryRegistration ) )
            {
                return ;
            }

            seenObjects.Add ( componentRegistryRegistration ) ;
            outFunc ( "Id = "             + componentRegistryRegistration.Id ) ;
            outFunc ( "Activator type = " + componentRegistryRegistration.Activator.GetType ( ) ) ;



            outFunc ( "LimitType = " + activatorLimitType ) ;


            foreach ( var service in componentRegistryRegistration.Services )
            {
                outFunc ( "Service is " + service.Description ) ;
            }

            if ( componentRegistryRegistration.Target == null )
            {
                outFunc ( "Target registration is null." ) ;
            }
            else if ( Equals (
                              componentRegistryRegistration
                            , componentRegistryRegistration.Target
                             ) )
            {
                outFunc ( "Target is same registration." ) ;
            }
            else
            {
                Dump ( componentRegistryRegistration.Target , seenObjects , outFunc ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class IdGeneratorModule : Module
        {
            private new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            /// <summary>Gets or sets the default object.</summary>
            /// <value>The default object.</value>
            /// <autogeneratedoc />
            /// TODO Edit XML Comment Template for DefaultObject
            public DefaultObjectIdProvider DefaultObject { get; set; }

            /// <summary>Gets or sets the generator.</summary>
            /// <value>The generator.</value>
            /// <autogeneratedoc />
            /// TODO Edit XML Comment Template for Generator
            public ObjectIDGenerator Generator { get; set; }

            /// <summary>Override to add registrations to the container.</summary>
            /// <remarks>
            ///     Note that the ContainerBuilder parameter is unique to this module.
            /// </remarks>
            /// <param name="builder">
            ///     The builder through which components can be
            ///     registered.
            /// </param>
            protected override void Load(ContainerBuilder builder)
            {
                //var obIdGenerator = new ObjectIDGenerator();
                Logger.Trace($"Load {nameof( AppBuildModule.IdGeneratorModule)}");


                Generator = new ObjectIDGenerator();
                //builder.RegisterInstance ( generator ).As < ObjectIDGenerator > ( ) ;
                //			builder.RegisterType < ObjectIDGenerator > ( ).InstancePerLifetimeScope ( ).AsSelf ( ) ;
                DefaultObject = new DefaultObjectIdProvider(Generator);
                builder.RegisterInstance(DefaultObject)
                       .As<IObjectIdProvider>()
                       .SingleInstance();
                // builder.RegisterType < DefaultObjectIdProvider > ( )
                //        .As < IObjectIdProvider > ( )
                //        .InstancePerLifetimeScope ( ) ;
            }


            /// <summary>
            ///     Override to attach module-specific functionality to a
            ///     component registration.
            /// </summary>
            /// <remarks>
            ///     This method will be called for all existing <i>and future</i> component
            ///     registrations - ordering is not important.
            /// </remarks>
            /// <param name="componentRegistry">The component registry.</param>
            /// <param name="registration">The registration to attach functionality to.</param>
            protected override void AttachToComponentRegistration(
                IComponentRegistryBuilder componentRegistry
              , IComponentRegistration    registration
            )
            {
                if (registration != null)
                {
                    registration.Preparing  += RegistrationOnPreparing;
                    registration.Activating += RegistrationOnActivating;
                }
            }

            private void RegistrationOnActivating(object sender, ActivatingEventArgs<object> e)
            {
                var inst = e.Instance;

                Logger.Trace(
                             $"{nameof(RegistrationOnActivating)} {e.Component.DebugFormat()}"
                            );
                if (e.Component.Services.Any(
                                             service =>
                                             {
                                                 var typedService = service as TypedService;
                                                 // Logger.Trace ( typedService ) ;
                                                 if (typedService == null)
                                                 {
                                                     return false;
                                                 }

                                                 var typedServiceServiceType =
                                                     typedService.ServiceType;
                                                 return typedServiceServiceType
                                                        == typeof(ObjectIDGenerator);
                                             }
                                            ))
                {
                    Logger.Debug($"Departing {nameof(RegistrationOnActivating)} early.");
                    return;
                }

                //var provider = e.Context.Resolve < IObjectIdProvider > ( ) ;
                var provideObjectInstanceIdentifier =
                    DefaultObject.ProvideObjectInstanceIdentifier(
                                                                  inst
                                                                , e.Component
                                                                , e.Parameters
                                                                 );
                if (inst is IHaveObjectId x)
                {
                    x.InstanceObjectId = provideObjectInstanceIdentifier;
                }
            }

            private void RegistrationOnPreparing(object sender, PreparingEventArgs e)
            {
            }
        }
    }
}