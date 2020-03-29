using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Configuration ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.ExceptionServices ;
using System.Runtime.Serialization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Integration.Mef ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Configuration ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.StackTrace ;
using Microsoft.Extensions.DependencyInjection ;
using NLog ;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// </summary>
    public sealed class ApplicationInstance : ApplicationInstanceBase , IDisposable
    {
        private readonly bool                    _disableLogging ;
        private readonly bool                    _disableServiceHost ;
        private readonly List < IModule >        _modules = new List < IModule > ( ) ;
        private          IContainer              _container ;
        private          ApplicationInstanceHost _host ;
        private          ILifetimeScope          _lifetimeScope ;

        /// <summary>
        /// </summary>
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

        /// <summary>
        /// </summary>
        public ILogger Logger { get ; }

        /// <summary>
        /// </summary>
        public override void Dispose ( )
        {
            _host?.Dispose ( ) ;
            _lifetimeScope?.Dispose ( ) ;
            _container?.Dispose ( ) ;
        }

        private static void CurrentDomain_FirstChanceException (
            object                                    sender
          , [ NotNull ] FirstChanceExceptionEventArgs e
        )
        {
            Utils.LogParsedExceptions ( e.Exception ) ;
        }

        #region Overrides of ApplicationInstanceBase
        /// <summary>
        /// </summary>
        public override void Initialize ( )
        {
            base.Initialize ( ) ;
            _container = BuildContainer ( ) ;
        }
        #endregion

        /// <summary>
        /// </summary>
        public override event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// </summary>
        /// <param name="appModule"></param>
        public override void AddModule ( IModule appModule ) { _modules.Add ( appModule ) ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
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
        [ NotNull ]
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


        #region IDisposable
        #endregion
    }

    /// <summary>
    ///     Fatal error building container. Wraps any autofac exceptions.
    /// </summary>
    [ Serializable ]
    public class ContainerBuildException : Exception
    {
        /// <summary>
        ///     Parameterless constructor.
        /// </summary>
        public ContainerBuildException ( ) { }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="message"></param>
        public ContainerBuildException ( string message ) : base ( message ) { }

        /// <summary>
        ///     Constructr
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ContainerBuildException ( string message , Exception innerException ) : base (
                                                                                             message
                                                                                           , innerException
                                                                                            )
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ContainerBuildException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }

    /// <summary>
    ///     New app module to replace crussty old app module. Work in progress.
    /// </summary>
    public sealed class NouveauAppModule : IocModule
    {
        #region Overrides of IocModule
        /// <summary>
        ///     Our fun custom load method that is public.
        /// </summary>
        /// <param name="builder"></param>
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
    /// </summary>
    public sealed class ParsedExceptions
    {
        private List < ParsedStackInfo > _parsedList = new List < ParsedStackInfo > ( ) ;

        /// <summary>
        /// </summary>
        public List < ParsedStackInfo > ParsedList
        {
            get { return _parsedList ; }
            set { _parsedList = value ; }
        }
    }

    /// <summary>
    /// </summary>
    public sealed class ParsedStackInfo
    {
        private readonly string _exMessage ;

        private readonly string                   _typeName ;
        private          List < StackTraceEntry > _stackTraceEntries ;

        /// <summary>
        /// </summary>
        public ParsedStackInfo ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="parsed"></param>
        /// <param name="typeName"></param>
        /// <param name="exMessage"></param>
        public ParsedStackInfo (
            [ NotNull ] IEnumerable < StackTraceEntry > parsed
          , string                                      typeName
          , string                                      exMessage
        )
        {
            _typeName         = typeName ;
            _exMessage        = exMessage ;
            StackTraceEntries = parsed.ToList ( ) ;
        }

        /// <summary>
        /// </summary>
        public List < StackTraceEntry > StackTraceEntries
        {
            get { return _stackTraceEntries ; }
            set { _stackTraceEntries = value ; }
        }

        /// <summary>
        /// </summary>
        public string TypeName { get { return _typeName ; } }
    }
}