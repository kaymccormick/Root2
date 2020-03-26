using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Configuration ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.ExceptionServices ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Xml.Serialization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Features.Decorators ;
using Autofac.Integration.Mef ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.Tracing ;
using Microsoft.Extensions.DependencyInjection ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    
    public class ApplicationInstance : ApplicationInstanceBase , IDisposable
    {
        private readonly        bool                    _disableLogging ;
        private readonly        bool                    _disableServiceHost ;
        private readonly        List < IModule >        _modules = new List < IModule > ( ) ;
        private                 IContainer              _container ;
        private                 ApplicationInstanceHost _host ;
        private                 ILifetimeScope          _lifetimeScope ;
        private static readonly BinaryFormatter         _binaryFormatter = new BinaryFormatter ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="logMethod"></param>
        /// <param name="configs"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        public ApplicationInstance (
            [ NotNull ] ApplicationInstanceConfiguration applicationInstanceConfiguration
        ) : base ( applicationInstanceConfiguration.LogMethod )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException ;
            _disableServiceHost =
                applicationInstanceConfiguration.DisableServiceHost ;
            var serviceCollection = new ServiceCollection ( ) ;
            if ( ! applicationInstanceConfiguration.DisableRuntimeConfiguration )
            {
                var loadedConfigs = LoadConfiguration ( AppLoggingConfigHelper.ProtoLogDelegate ) ;
                applicationInstanceConfiguration.Configs =
                    applicationInstanceConfiguration.Configs != null
                        ? ( ( IEnumerable < object > ) applicationInstanceConfiguration.Configs )
                       .Append ( loadedConfigs )
                        : loadedConfigs ;
            }

            if ( ! applicationInstanceConfiguration.DisableLogging )
            {
                serviceCollection.AddLogging ( ) ;
                var config = applicationInstanceConfiguration.Configs != null
                                 ? applicationInstanceConfiguration
                                  .Configs.OfType < ILoggingConfiguration > ( )
                                  .FirstOrDefault ( )
                                 : null ;
                // LogManager.EnableLogging ( ) ;
                if ( LogManager.IsLoggingEnabled ( ) )
                {
                    Debug.WriteLine ( "logging enableD" ) ;
                }

                if ( LogManager.Configuration != null )
                {
                    foreach ( var configurationAllTarget in LogManager.Configuration.AllTargets )
                    {
                        Debug.WriteLine ( configurationAllTarget ) ;
                    }
                }

                Logger = AppLoggingConfigHelper.EnsureLoggingConfigured (
                                                                         applicationInstanceConfiguration
                                                                            .LogMethod
                                                                       , config
                                                                        ) ;
                GlobalDiagnosticsContext.Set (
                                              "ExecutionContext"
                                            , new ExecutionContextImpl (
                                                                        Logging.Application
                                                                               .MainApplication
                                                                       )
                                             ) ;

                GlobalDiagnosticsContext.Set ( "RunId" , InstanceRunGuid ) ;
                Logger.Info ( "RunID: {runId}" , InstanceRunGuid ) ;
            }
            else
            {
                Logger = LogManager.CreateNullLogger ( ) ;
            }
        }

        private static void CurrentDomain_FirstChanceException (
            object                        sender
          , FirstChanceExceptionEventArgs e
        )
        {
            Utils.LogParsedExceptions ( e.Exception ) ;
        }

        /// <summary>
        /// </summary>
        public ILogger Logger { get ; }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose ( )
        {
            _host?.Dispose ( ) ;
            _lifetimeScope?.Dispose ( ) ;
            _container?.Dispose ( ) ;
        }

        #region Overrides of ApplicationInstanceBase
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize ( )
        {
            base.Initialize ( ) ;
            _container = BuildContainer ( ) ;
        }
        #endregion

        /// <summary>
        /// </summary>
        // ReSharper disable once EventNeverSubscribedTo.Global
        public override event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appModule"></param>
        public override void AddModule ( IModule appModule ) { _modules.Add ( appModule ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ILifetimeScope GetLifetimeScope ( )
        {
            if ( _lifetimeScope != null )
            {
                return _lifetimeScope ;
            }

            if ( _container == null )
            {
                _container = BuildContainer ( ) ;
            }

            _lifetimeScope = _container.BeginLifetimeScope ( ) ;
            return _container.BeginLifetimeScope ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected override IContainer BuildContainer ( )
        {
            var builder = new ContainerBuilder ( ) ;
            builder.RegisterModule < AttributedMetadataModule > ( ) ;
            builder.RegisterMetadataRegistrationSources ( ) ;
            builder.RegisterModule < NouveauAppModule > ( ) ;

            foreach ( var module in _modules )
            {
                Logger.Debug ( "Registering module {module}" , module.ToString ( ) ) ;
                builder.RegisterModule ( module ) ;
            }

            try
            {
                return builder.Build ( ) ;
            }
            catch ( ArgumentException argExp )
            {
                throw new ContainerBuildException (
                                                   "Unable to build container: " + argExp.Message
                                                 , argExp
                                                  ) ;
            }
        }

        /// <summary>
        /// </summary>
        public override void Startup ( )
        {
            if ( ! _disableServiceHost )
            {
                _host = new ApplicationInstanceHost ( _container ) ;
                _host.HostOpen ( ) ;
            }

            base.Startup ( ) ;
        }

        /// <summary>
        /// </summary>
        public override void Shutdown ( )
        {
            base.Shutdown ( ) ;
#if NETSTANDARD || NETFRAMEWORK
            AppLoggingConfigHelper.ServiceTarget?.Dispose ( ) ;
#endif
        }

        /// <summary>
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected override IEnumerable LoadConfiguration ( LogDelegates.LogMethod logMethod2 )
        {
            if ( logMethod2 == null )
            {
                throw new ArgumentNullException ( nameof ( logMethod2 ) ) ;
            }

            var config = ConfigurationManager.OpenExeConfiguration ( ConfigurationUserLevel.None ) ;

            logMethod2 ( $"Using {config.FilePath} for configuration" ) ;
            var type1 = typeof ( ContainerHelperSection ) ;

            try
            {
                var sections = config.Sections ;
                foreach ( ConfigurationSection configSection in sections )
                {
                    try
                    {
                        var type = configSection.SectionInformation.Type ;
                        var sectionType = Type.GetType ( type ) ;
                        if ( sectionType             != null
                             && sectionType.Assembly == type1.Assembly )
                        {
                            logMethod2 ( "Found section " + sectionType.Name ) ;
                            var at = sectionType.GetCustomAttribute < ConfigTargetAttribute > ( ) ;
                            var configTarget = Activator.CreateInstance ( at.TargetType ) ;
                            var infos = sectionType
                                       .GetMembers ( )
                                       .Select (
                                                info => Tuple.Create (
                                                                      info
                                                                    , info
                                                                         .GetCustomAttribute <
                                                                              ConfigurationPropertyAttribute
                                                                          > ( )
                                                                     )
                                               )
                                       .Where ( tuple => tuple.Item2 != null )
                                       .ToArray ( ) ;
                            foreach ( var (item1 , _) in infos )
                            {
                                if ( item1.MemberType == MemberTypes.Property )
                                {
                                    var attr = at.TargetType.GetProperty ( item1.Name ) ;
                                    try
                                    {
                                        var configVal =
                                            ( ( PropertyInfo ) item1 ).GetValue ( configSection ) ;
                                        if ( attr != null )
                                        {
                                            attr.SetValue ( configTarget , configVal ) ;
                                        }
                                    }
                                    catch ( Exception ex )
                                    {
                                        logMethod2 (
                                                    $"Unable to set property {item1.Name}: {ex.Message}"
                                                   ) ;
                                    }
                                }
                            }


                            ConfigSettings.Add ( configTarget ) ;
                        }
                    }
                    catch ( Exception ex1 )
                    {
                        Logger.Error ( ex1 , ex1.Message ) ;
                    }
                }
            }
            catch ( Exception ex )
            {
                logMethod2 ( ex.Message ) ;
            }

            return ConfigSettings ;
        }

        // ReSharper disable once UnusedMember.Local
        private void DebugServices ( IComponentContext c )
        {
            foreach ( var componentRegistryRegistration in c.ComponentRegistry.Registrations )
            {
                Logger.Debug (
                              "services: {services}"
                            , string.Join (
                                           ", "
                                         , componentRegistryRegistration.Services.Select (
                                                                                          service
                                                                                              => {
                                                                                              switch
                                                                                              ( service
                                                                                              )
                                                                                              {
                                                                                                  case
                                                                                                      KeyedService
                                                                                                      _ :
                                                                                                      break ;
                                                                                                  case
                                                                                                      TypedService
                                                                                                      typedService
                                                                                                      :
                                                                                                      return
                                                                                                          typedService
                                                                                                             .ServiceType
                                                                                                             .FullName ;
                                                                                                  case
                                                                                                      UniqueService
                                                                                                      _ :
                                                                                                      break ;
                                                                                                  case
                                                                                                      DecoratorService
                                                                                                      _ :
                                                                                                      break ;
                                                                                                  default
                                                                                                      :
                                                                                                      throw
                                                                                                          new
                                                                                                              ArgumentOutOfRangeException (
                                                                                                                                           nameof
                                                                                                                                           ( service
                                                                                                                                           )
                                                                                                                                          ) ;
                                                                                              }

                                                                                              return
                                                                                                  service
                                                                                                     .Description ;
                                                                                          }
                                                                                         )
                                          )
                             ) ;
            }
        }


        #region IDisposable
        #endregion
    }

    [ Serializable ]
    public class ContainerBuildException : Exception
    {
        public ContainerBuildException ( ) { }

        public ContainerBuildException ( string message ) : base ( message ) { }

        public ContainerBuildException ( string message , Exception innerException ) : base (
                                                                                             message
                                                                                           , innerException
                                                                                            )
        {
        }

        protected ContainerBuildException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }

    public class NouveauAppModule : IocModule
    {
        #region Overrides of IocModule
        public override void DoLoad ( ContainerBuilder builder )
        {
            if ( AppLoggingConfigHelper.CacheTarget2 != null )
            {
                builder.RegisterInstance ( AppLoggingConfigHelper.CacheTarget2 )
                       .WithMetadata ( "Description" , "Cache target" )
                       .SingleInstance ( ) ;
            }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParsedExceptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ParsedExceptions ( ) { }

        private List < ParsedStackInfo > _parsedList = new List < ParsedStackInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public List < ParsedStackInfo > ParsedList
        {
            get { return _parsedList ; }
            set { _parsedList = value ; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParsedStackInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ParsedStackInfo ( ) { }

        private readonly string                   _typeName ;
        private readonly string                   _exMessage ;
        private          List < StackTraceEntry > _stackTraceEntries ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parsed"></param>
        /// <param name="typeName"></param>
        /// <param name="exMessage"></param>
        public ParsedStackInfo (
            IEnumerable < StackTraceEntry > parsed
          , string                          typeName
          , string                          exMessage
        )
        {
            _typeName         = typeName ;
            _exMessage        = exMessage ;
            StackTraceEntries = parsed.ToList ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public List < StackTraceEntry > StackTraceEntries
        {
            get { return _stackTraceEntries ; }
            set { _stackTraceEntries = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get { return _exMessage ; } }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get { return _typeName ; } }
    }
}