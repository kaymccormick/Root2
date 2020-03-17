using Autofac;
using Autofac.Core;
using Autofac.Features.Decorators;
using JetBrains.Annotations;
using KayMcCormick.Dev.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration ;
using System.Diagnostics ;
using System.Linq;
using System.Reflection ;
using Autofac.Extras.AttributeMetadata ;
using KayMcCormick.Dev.Attributes ;
using Microsoft.Extensions.DependencyInjection ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public sealed class ApplicationInstance : IDisposable
    {
        private readonly ILogger _logger;
        private ILifetimeScope lifetimeScope;
        private readonly List<IModule> _modules = new List<IModule>();
        private IContainer _container;
        private ApplicationInstanceHost _host;

        /// <summary>
        /// 
        /// </summary>
        public Guid InstanceRunGuid { get; }

        /// <summary>
        /// 
        /// </summary>
        public ILogger Logger => _logger ;

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event EventHandler<AppStartupEventArgs> AppStartup;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMethod"></param>
        /// <param name="configs"></param>
        public ApplicationInstance ( LogDelegates.LogMethod logMethod, IEnumerable <object> configs = null )
        {
            if ( configs == null )
            {
                configs = Array.Empty < object > ( ) ;
            }
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();

            var loadedConfigs = LoadConfiguration (AppLoggingConfigHelper.ProtoLogDelegate ) ;
            configs = configs.Append ( loadedConfigs ) ;

            InstanceRunGuid = Guid.NewGuid();
            ILoggingConfiguration config =
                configs.OfType < ILoggingConfiguration > ( ).FirstOrDefault ( ) ;

            LogManager.EnableLogging();
            if ( LogManager.IsLoggingEnabled ( ) )
            {
                Debug.WriteLine ( "logging enableD" ) ;
            }
            foreach ( var configurationAllTarget in LogManager.Configuration.AllTargets )
            {
                Debug.WriteLine ( configurationAllTarget ) ;
            }

            _logger = AppLoggingConfigHelper.EnsureLoggingConfigured(logMethod, config);
            GlobalDiagnosticsContext.Set(
                                         "ExecutionContext"
                                       , new ExecutionContextImpl
                                         (
                                          KayMcCormick.Dev.Logging.Application
                                                                       .MainApplication
                                         )
                                        );

            GlobalDiagnosticsContext.Set("RunId", InstanceRunGuid);
            _logger.Info ( "RunID: {runId}" , InstanceRunGuid ) ;

        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void Initialize() { _container = BuildContainer(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appModule"></param>
        // ReSharper disable once UnusedMember.Global
        public void AddModule(IModule appModule) => _modules.Add(appModule);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [UsedImplicitly]
        public ILifetimeScope GetLifetimeScope()
        {
            if (lifetimeScope != null)
            {
                return lifetimeScope;
            }

            if (_container == null)
            {
                _container = BuildContainer();
            }

            lifetimeScope = _container.BeginLifetimeScope();
            return _container.BeginLifetimeScope();
        }

        private IContainer BuildContainer()
        {
            var builder1 = new ContainerBuilder();
            builder1.RegisterModule < AttributedMetadataModule > ( ) ;
            foreach (var module in _modules)
            {
                _logger.Debug("Registering module {module}", module.ToString());
                builder1.RegisterModule(module);
            }

            var c
                = builder1.Build();
            //            DebugServices ( c ) ;
            return c;
        }

        // ReSharper disable once UnusedMember.Local
        private void DebugServices(IComponentContext c)
        {
            foreach (var componentRegistryRegistration in c.ComponentRegistry.Registrations)
            {
                _logger.Debug(
                                  "services: {services}"
                                , string.Join(
                                              ", "
                                            , componentRegistryRegistration.Services.Select(
                                                                                            service =>
                                                                                            {
                                                                                                switch (
                                                                                                    service)
                                                                                                {
                                                                                                    case
                                                                                                        KeyedService
                                                                                                        _
                                                                                                        :
                                                                                                        break;
                                                                                                    case
                                                                                                        TypedService
                                                                                                        typedService
                                                                                                        :
                                                                                                        return
                                                                                                            typedService
                                                                                                               .ServiceType
                                                                                                               .FullName;
                                                                                                    case
                                                                                                        UniqueService
                                                                                                        _
                                                                                                        :
                                                                                                        break;
                                                                                                    case
                                                                                                        DecoratorService
                                                                                                        _
                                                                                                        :
                                                                                                        break;
                                                                                                    default:
                                                                                                        throw
                                                                                                            new
                                                                                                                ArgumentOutOfRangeException(
                                                                                                                                            nameof
                                                                                                                                            (service
                                                                                                                                            )
                                                                                                                                           );
                                                                                                }

                                                                                                return service
                                                                                                   .Description;
                                                                                            }
                                                                                           )
                                             )
                                 );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public void Startup()
        {
            // if ( lifetimeScope == null )
            // {
            //     throw new ApplicationInstanceException ( "lifetime scope not initialized" ) ;
            // }
            _host = new ApplicationInstanceHost(_container);
            _host.HostOpen();
            OnAppStartup(new AppStartupEventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnAppStartup(AppStartupEventArgs e)
        {
            AppStartup?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        ///  todo call from wpf
        // ReSharper disable once UnusedMember.Global
        public void Shutdown()
        {
#if NETSTANDARD
            AppLoggingConfigHelper.ServiceTarget?.Dispose();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected List<object> LoadConfiguration( [ NotNull ] LogDelegates.LogMethod logMethod2)
        {
            if ( logMethod2 == null )
            {
                throw new ArgumentNullException ( nameof ( logMethod2 ) ) ;
            }

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            logMethod2?.Invoke($"Using {config.FilePath} for configuration");
            var type1 = typeof(ContainerHelperSection);

            try
            {
                var sections = config.Sections;
                foreach (ConfigurationSection configSection in sections)
                {
                    try
                    {
                        var type = configSection.SectionInformation.Type;
                        var sectionType = Type.GetType(type);
                        if (sectionType != null
                             && sectionType.Assembly == type1.Assembly)
                        {
                            logMethod2("Found section " + sectionType.Name);
                            var at = sectionType.GetCustomAttribute<ConfigTargetAttribute>();
                            var configTarget = Activator.CreateInstance(at.TargetType);
                            var infos = sectionType
                                       .GetMembers()
                                       .Select(
                                                info => Tuple.Create(
                                                                      info
                                                                    , info
                                                                         .GetCustomAttribute<
                                                                              ConfigurationPropertyAttribute
                                                                          >()
                                                                     )
                                               )
                                       .Where(tuple => tuple.Item2 != null)
                                       .ToArray();
                            foreach (var (item1, _) in infos)
                            {
                                if (item1.MemberType == MemberTypes.Property)
                                {
                                    var attr = at.TargetType.GetProperty(item1.Name);
                                    try
                                    {
                                        var configVal =
                                            ((PropertyInfo)item1).GetValue(configSection);
                                        if (attr != null)
                                        {
                                            attr.SetValue(configTarget, configVal);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logMethod2?.Invoke(
                                                            $"Unable to set property {item1.Name}: {ex.Message}"
                                                           );
                                    }
                                }
                            }


                            ConfigSettings.Add(configTarget);
                        }
                    }
                    catch (Exception ex1)
                    {
                        Logger.Error(ex1, ex1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                logMethod2(ex.Message);
            }

            return ConfigSettings;
        }

        private List<object> ConfigSettings { get; } = new List<object>();

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _host?.Dispose();
            lifetimeScope?.Dispose();
            _container?.Dispose();
        }
        #endregion
    }
}
