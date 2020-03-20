using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Configuration ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using Autofac ;
using Autofac.Core ;
using Autofac.Extras.AttributeMetadata ;
using Autofac.Features.Decorators ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Extensions.DependencyInjection ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    [ UsedImplicitly ]
    public class ApplicationInstance : ApplicationInstanceBase , IDisposable
    {
        private readonly bool                    _disableLogging ;
        private readonly bool                    _disableServiceHost ;
        private readonly List < IModule >        _modules = new List < IModule > ( ) ;
        private          IContainer              _container ;
        private          ApplicationInstanceHost _host ;
        private          ILifetimeScope          _lifetimeScope ;

        /// <summary>
        /// </summary>
        /// <param name="logMethod"></param>
        /// <param name="configs"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        public ApplicationInstance (
            LogDelegates.LogMethod logMethod
          , IEnumerable            configs                     = null
          , bool                   disableLogging              = false
          , bool                   disableRuntimeConfiguration = false
          , bool                   disableServiceHost          = false
        ) : base ( logMethod )

        {
            var serviceCollection = new ServiceCollection ( ) ;
            if ( ! disableRuntimeConfiguration )
            {
                var loadedConfigs = LoadConfiguration ( AppLoggingConfigHelper.ProtoLogDelegate ) ;
                configs = configs != null ? ((IEnumerable <object>)configs).Append ( loadedConfigs ) : loadedConfigs ;
            }

            if ( ! disableLogging )
            {
                serviceCollection.AddLogging ( ) ;
                var config = configs.OfType < ILoggingConfiguration > ( ).FirstOrDefault ( ) ;
                LogManager.EnableLogging ( ) ;
                if ( LogManager.IsLoggingEnabled ( ) )
                {
                    Debug.WriteLine ( "logging enableD" ) ;
                }

                foreach ( var configurationAllTarget in LogManager.Configuration.AllTargets )
                {
                    Debug.WriteLine ( configurationAllTarget ) ;
                }

                Logger = AppLoggingConfigHelper.EnsureLoggingConfigured ( logMethod , config ) ;
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
        public override void AddModule ( IModule appModule )
        {
            _modules.Add(appModule);
        }

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
            foreach ( var module in _modules )
            {
                Logger.Debug ( "Registering module {module}" , module.ToString ( ) ) ;
                builder.RegisterModule ( module ) ;
            }

            return builder.Build ( ) ;
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
}