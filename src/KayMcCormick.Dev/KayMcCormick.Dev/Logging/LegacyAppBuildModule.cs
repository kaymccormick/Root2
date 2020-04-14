using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using Autofac.Core.Registration ;
using Autofac.Core.Resolving ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.AppBuild ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Tracing ;
using NLog ;
using Module = Autofac.Module ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ContainerHelper
    [ UsedImplicitly ]
    public sealed class LegacyAppBuildModule : Module
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


        /// <summary>
        ///     Property name used to propagate the value of
        ///     <see cref="DoInterception" />.
        /// </summary>
        public const string InterceptProperty = "Intercept" ;


        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load ( [ NotNull ] ContainerBuilder builder )
        {
            AppBuild ( builder ) ;
        }

        private void AppBuild (
            [ NotNull ] ContainerBuilder builder
        )
        {
            // Set the property in order to propagate the settings.
            builder.Properties[ InterceptProperty ] = DoInterception ;

            builder.Properties[ AssembliesForScanningProperty ] = GetAssembliesForScanning ( ) ;
            #region Autofac Modules
            // builder.RegisterModule < AttributedMetadataModule > ( ) ;
            builder.RegisterModule < IdGeneratorModule > ( ) ;
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


            #region Interceptors
            if ( DoInterception )
            {

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

            //builder.RegisterBuildCallback ( c => Logger.Info ( "Container built." ) ) ;
            builder.RegisterCallback ( ConfigurationCallback ) ;
            #endregion
        }

        private void ConfigurationCallback ( [ NotNull ] IComponentRegistryBuilder registry )
        {
            if ( registry == null )
            {
                throw new ArgumentNullException ( nameof ( registry ) ) ;
            }

            registry.Registered += OnRegistryOnRegistered ;
        }

        private void OnRegistryOnRegistered (
            object                                   sender
          , [ NotNull ] ComponentRegisteredEventArgs args
        )
        {
            if ( args == null )
            {
                throw new ArgumentNullException ( nameof ( args ) ) ;
            }

            var reg = args.ComponentRegistration ;
            reg.Metadata[ "RegisteredDatetime" ] = DateTime.Now ;
            var activatorLimitType = reg.Activator.LimitType ;
            Logger.Trace ( "Registered {limitType} {activator}", activatorLimitType , args.ComponentRegistration.Activator) ;
#if TRACEPROVIDER
            PROVIDER_GUID.EventWriteEVENT_COMPONENT_REGISTERED (
                                                                activatorLimitType
                                                                   .AssemblyQualifiedName
                                                              , reg.Id

                                                               ) ;
#endif

            reg.Activated += ( o , eventArgs ) => {
                var instanceDesc = eventArgs.Instance ;
                var type = eventArgs.Instance.GetType ( ) ;
                if ( type.IsArray
                     && typeof ( Delegate ).IsAssignableFrom ( type.GetElementType ( ) ) )
                {
                    instanceDesc = eventArgs.Instance.ToString ( ) ;
                }
                else if ( type.IsGenericType
                          && type.GetGenericTypeDefinition ( ) == typeof ( Meta <> ) )
                {
                    var x = type.GetGenericArguments ( )[ 0 ] ;
                    if ( typeof ( Delegate ).IsAssignableFrom ( x ) )
                    {
                        instanceDesc = eventArgs.Instance.ToString ( ) ;
                    }
                }
                else if ( eventArgs.Instance.GetType ( ).Name == "BuildCallbackService" )
                {
                    instanceDesc = eventArgs.Instance.ToString();
                }
                else if ( eventArgs.Instance is Delegate )
                {
                    instanceDesc = eventArgs.Instance.ToString ( ) ;
                }

                Logger.Trace (
                              "Activated {desc} (sender={sender}, instance={instance})"
                            , DescribeComponent ( eventArgs.Component )
                            , o.ToString()
                            , instanceDesc.ToString()
                             ) ;
            } ;
        }

        private void SetupContainerOnResolveOperationBeginning (
            object                                         sender
          , [ NotNull ] ResolveOperationBeginningEventArgs e
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

        /// <summary>
        ///     Gets or sets a value indicating whether install interceptors for
        ///     built objects.
        /// </summary>
        /// <value>
        ///     <see language="true" /> to perform interception; otherwise,
        ///     <see language="false" />.
        /// </value>

        public bool DoInterception { get ; } = true ;

        /// <summary>
        ///     Gets or sets a value indicating whether to trace conditional
        ///     registration.
        /// </summary>
        /// <value>
        ///     <see language="true" /> to trace conditional registration; otherwise,
        ///     <see language="false" />.
        /// </value>

        public static bool DoTraceConditionalRegistration { get ; set ; }

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

        /// <summary>
        ///     Gets or sets a value indicating whether [get assemblies via
        ///     references].
        /// </summary>
        /// <value>
        ///     <see language="true" /> if [get assemblies via references]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetAssembliesViaReferences

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
        [ NotNull ]
        public ICollection < Assembly > GetAssembliesForScanningViaTypes ( )
        {
            Logger.Debug (
                          "Getting assemblies to scan based on AssemblyContainerScan attribute."
                         ) ;

            Type[] ary = { typeof ( IHaveObjectId ) , typeof ( LegacyAppBuildModule ) } ;
            var forScanning = ary.Select ( ( type , i ) => type.Assembly ).ToList ( ) ;
            // Logger.Info (
            // "Assemblies "
            // + string.Join (
            // ", "
            // , forScanning.Select (
            // ( assembly , i1 )
            // => assembly.GetName ( ).Name
            // )
            // )
            // ) ;
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

        [ NotNull ]
        private static string DescribeComponent ( [ NotNull ] IComponentRegistration eventArgsComponent )
        {
            var debugDesc = "no description" ;
            const string key = "DebugDescription" ;
            if ( eventArgsComponent.Metadata.ContainsKey ( key ) )
            {
                debugDesc = eventArgsComponent.Metadata[ key ].ToString ( ) ;
            }

            return $" CompReg w({eventArgsComponent.Id}, {debugDesc})" ;
        }


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

            if ( Equals (
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
        /// </summary>
        public sealed class IdGeneratorModule : Module
        {
            private new static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

            /// <summary>Gets or sets the default object.</summary>
            /// <value>The default object.</value>
            /// <autogeneratedoc />
            /// TODO Edit XML Comment Template for DefaultObject
            private DefaultObjectIdProvider DefaultObject { get ; set ; }

            /// <summary>Gets or sets the generator.</summary>
            /// <value>The generator.</value>
            /// <autogeneratedoc />
            /// TODO Edit XML Comment Template for Generator
            public ObjectIDGenerator Generator { get ; set ; }

            /// <summary>Override to add registrations to the container.</summary>
            /// <remarks>
            ///     Note that the ContainerBuilder parameter is unique to this module.
            /// </remarks>
            /// <param name="builder">
            ///     The builder through which components can be
            ///     registered.
            /// </param>
            protected override void Load ( ContainerBuilder builder )
            {
                //var obIdGenerator = new ObjectIDGenerator();
                Logger.Trace ( $"Load {nameof ( IdGeneratorModule )}" ) ;


                Generator = new ObjectIDGenerator ( ) ;
                //builder.RegisterInstance ( generator ).As < ObjectIDGenerator > ( ) ;
                //			builder.RegisterType < ObjectIDGenerator > ( ).InstancePerLifetimeScope ( ).AsSelf ( ) ;
                DefaultObject = new DefaultObjectIdProvider ( Generator ) ;
                builder.RegisterInstance ( DefaultObject )
                       .As < IObjectIdProvider > ( )
                       .SingleInstance ( ) ;
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
            protected override void AttachToComponentRegistration (
                IComponentRegistryBuilder            componentRegistry
              , [ CanBeNull ] IComponentRegistration registration
            )
            {
                if ( registration != null )
                {
                    registration.Preparing  += RegistrationOnPreparing ;
                    registration.Activating += RegistrationOnActivating ;
                }
            }

            private void RegistrationOnActivating (
                object                         sender
              , [ NotNull ] ActivatingEventArgs < object > e
            )
            {
                var inst = e.Instance ;

                Logger.Trace (
                              $"{nameof ( RegistrationOnActivating )} {e.Component.DebugFormat ( )}"
                             ) ;
                if ( e.Component.Services.Any (
                                               service => {
                                                   var typedService = service as TypedService ;
                                                   // Logger.Trace ( typedService ) ;
                                                   if ( typedService == null )
                                                   {
                                                       return false ;
                                                   }

                                                   var typedServiceServiceType =
                                                       typedService.ServiceType ;
                                                   return typedServiceServiceType
                                                          == typeof ( ObjectIDGenerator ) ;
                                               }
                                              ) )
                {
                    Logger.Debug ( $"Departing {nameof ( RegistrationOnActivating )} early." ) ;
                    return ;
                }

                //var provider = e.Context.Resolve < IObjectIdProvider > ( ) ;
                try
                {
                    var provideObjectInstanceIdentifier =
                        DefaultObject.ProvideObjectInstanceIdentifier (
                                                                       inst
                                                                     , e.Component
                                                                     , e.Parameters
                                                                      ) ;
                    if ( inst is IHaveObjectId x )
                    {
                        x.InstanceObjectId = provideObjectInstanceIdentifier ;
                    }
                }
                catch ( Exception eX )
                {
                    DebugUtils.WriteLine ( eX.ToString ( ) ) ;
                }
            }

            private void RegistrationOnPreparing ( object sender , PreparingEventArgs e ) { }
        }
    }
}