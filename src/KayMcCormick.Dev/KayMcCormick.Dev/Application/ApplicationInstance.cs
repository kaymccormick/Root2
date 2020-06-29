using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;
using Autofac.Extras.AttributeMetadata;
using Autofac.Integration.Mef;
using JetBrains.Annotations;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Configuration;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Dev.Serialization;
using KmDevLib;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using IContainer = Autofac.IContainer;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// Static class containing application GUIds
    /// </summary>
    public static class ApplicationInstanceIds
    {
        /// <summary>
        /// GUID for console analysis application
        /// </summary>
        public static Guid ConsoleAnalysisProgramGuid { get; } =
            new Guid("49A60392-BCC5-468B-8F09-76E0C04CD27C");

        /// <summary>
        /// Application GUID for the Leaf node windows service.
        /// </summary>
        public static Guid LeafService { get; } =
            new Guid("{E66F158B-37EB-4FA7-8A2E-2387D5ADF2A7}");

        /// <summary>
        /// Application GUID for a basic command-line configuration test application.
        /// </summary>
        // ReSharper disable once UnusedMember.Global"
        // ReSharper disable once UnusedMember.Global
        public static Guid ConfigTest { get; } =
            new Guid("{28CE37FB-A675-4483-BD6F-79FC9C68D973}");

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static Guid ClassLibTests { get; set; } =
            new Guid("{177EF37C-8D28-4CBE-A3D7-703E51AEE246}");

        /// <summary>
        /// Basic win forms app
        /// </summary>
        public static Guid BasicWinForms { get; } = new Guid("c9c74fca-0769-4990-9967-2ac8c06b4630");

        public static Guid ProjTests { get; set; } = new Guid("{EEC2E4DC-A0BE-4472-A936-50CA7419B530}");

        public static Guid Client2 { get ; } =
            new Guid ( "9919c0fb-916c-4804-81de-f272a1b585f7" ) ;

        public static Dictionary<Guid, string> GuidMap { get; } =
            new Dictionary<Guid, string>() {[ProjTests] = "ProjTests", [Client2] = "Client2" };
    }

    /// <summary>
    /// </  summary>
    public class ApplicationInstance : ApplicationInstanceBase, IDisposable
    {
#pragma warning disable 169
        private readonly bool _disableLogging;
#pragma warning restore 169

        private readonly bool _disableServiceHost;
        private readonly List<IModule> _modules = new List<IModule>();
        private IContainer _container;
        private ApplicationInstanceHost _host;
        protected ILifetimeScope LifetimeScope { get; private set; }
        private ILogger _logger;
        private MyReplaySubject<AppLogMessage> _subject1;
        private ILifetimeScope _root;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMethod"></param>
        /// <param name="appGuid"></param>
        /// <param name="configs"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        /// <returns></returns>
        [NotNull]
        public static ApplicationInstanceConfiguration CreateConfiguration(
            LogMethodDelegate logMethod
            , Guid appGuid
            , IEnumerable configs = null
            , bool disableLogging = false
            , bool disableRuntimeConfiguration = false
            , bool disableServiceHost = false
        )
        {
            return new ApplicationInstanceConfiguration(
                logMethod
                , appGuid
                , configs
                , disableLogging
                , disableRuntimeConfiguration
                , disableServiceHost
            );
        }

        public object Resolve(Type type)
        {
            return LifetimeScope.Resolve(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public delegate void LogMethodDelegate(string message);

        /// <summary>
        /// 
        /// </summary>
        public sealed class ApplicationInstanceConfiguration
        {
            private Action<ContainerBuilder> _builderAction;

            /// <summary>
            /// 
            /// </summary>
            private readonly LogMethodDelegate _logMethod;

            /// <summary>
            ///     Initialize a <see cref="ApplicationInstanceConfiguration" /> instance
            ///     with the provided values.
            /// </summary>
            /// <param name="logMethod">
            ///     Delegate to provide logging capability prior to
            ///     initialization of logging infrastructure.
            /// </param>
            /// <param name="appGuid">Pre-assigned app GUID.</param>
            /// <param name="configs">Set of configuration objects to apply</param>
            /// <param name="disableLogging">
            ///     Boolean flag indicating whether or not the
            ///     logging system should be disabled.
            /// </param>
            /// <param name="disableRuntimeConfiguration">
            ///     Boolean indicating whether or not
            ///     to load runtime configuration using the System.Configuration mechanism.
            /// </param>
            /// <param name="disableServiceHost">Boolean indicating whether or not to run a service host inside the application.</param>
            public ApplicationInstanceConfiguration(
                LogMethodDelegate logMethod
                , Guid appGuid
                , IEnumerable configs = null
                , bool disableLogging = false
                , bool disableRuntimeConfiguration = false
                , bool disableServiceHost = false
            )
            {
                _logMethod = logMethod;
                AppGuid = appGuid;
                Configs = configs;
                DisableLogging = disableLogging;
                DisableRuntimeConfiguration = disableRuntimeConfiguration;
                DisableServiceHost = disableServiceHost;
            }

            /// <summary>
            /// </summary>
            public LogMethodDelegate LogMethod
            {
                get { return _logMethod; }
            }

            /// <summary>
            /// 
            /// </summary>
            public Guid AppGuid { get; }

            /// <summary>
            /// </summary>
            public IEnumerable Configs { get; set; }

            /// <summary>
            /// </summary>
            public bool DisableLogging { get; }

            /// <summary>
            /// </summary>
            public bool DisableRuntimeConfiguration { get; }

            /// <summary>
            /// </summary>
            public bool DisableServiceHost { get; }

            /// <summary>
            /// 
            /// </summary>
            public Action<ContainerBuilder> BuilderAction
            {
                get { return _builderAction; }
                set { _builderAction = value; }
            }
        }


        /// <summary>
        /// </summary>
        public ApplicationInstance(
            [NotNull] ApplicationInstanceConfiguration applicationInstanceConfiguration
        )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            Action<string> log = s =>
            {
                //WSubject1.OnNext(new AppLogMessage(s));
                applicationInstanceConfiguration.LogMethod(s);
            };


            _disableServiceHost = applicationInstanceConfiguration.DisableServiceHost;
//applicationInstanceConfiguration.AppGuid

            var serviceCollection = new ServiceCollection();

            // ReSharper disable once UnusedVariable
            var protoLogger = ProtoLogger.Instance;
            if (!applicationInstanceConfiguration.DisableRuntimeConfiguration)
            {
                var loadedConfigs = LoadConfiguration(log);
                applicationInstanceConfiguration.Configs =
                    applicationInstanceConfiguration.Configs != null
                        ? ((IEnumerable<object>) applicationInstanceConfiguration.Configs)
                        .Append(loadedConfigs)
                        : loadedConfigs;
            }

            if (!applicationInstanceConfiguration.DisableLogging)
            {
                serviceCollection.AddLogging();
                // ReSharper disable once UnusedVariable
                var config = applicationInstanceConfiguration.Configs != null
                    ? applicationInstanceConfiguration
                        .Configs.OfType<ILoggingConfiguration>()
                        .FirstOrDefault()
                    : null;
                // LogManager.EnableLogging ( ) ;
                if (LogManager.IsLoggingEnabled()) DebugUtils.WriteLine("logging enableD");

                if (LogManager.Configuration == null) return;

                foreach (var configurationAllTarget in LogManager.Configuration.AllTargets)
                    DebugUtils.WriteLine(configurationAllTarget.ToString());

                // Logger = AppLoggingConfigHelper.EnsureLoggingConfigured (
                // new LogDelegates.
                // LogMethod (
                // applicationInstanceConfiguration
                // .LogMethod
                // )
                // , config
                // ) ;
                // GlobalDiagnosticsContext.Set (
                // "ExecutionContext"
                // , new ExecutionContextImpl (
                // Logging.Application
                // .MainApplication
                // )
                // ) ;

                // GlobalDiagnosticsContext.Set ( "RunId" , InstanceRunGuid ) ;
                // Logger.Info ( "RunID: {runId}" , InstanceRunGuid ) ;
            }
            else
            {
                Logger = LogManager.CreateNullLogger();
            }
        }

        /// <summary>
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value;
                //Subject.Subscribe(message => _logger.Info($"XX: {message}"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }


        /// <summary>
        /// </summary>
        public override void Dispose()
        {
            _host?.Dispose();
            LifetimeScope?.Dispose();
            Container?.Dispose();
        }

        private static void CurrentDomain_FirstChanceException(
            object sender
            , [NotNull] FirstChanceExceptionEventArgs e
        )
        {
            // Utils.LogParsedExceptions ( e.Exception ) ;
        }

        #region Overrides of ApplicationInstanceBase

        /// <summary>
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Container = BuildContainer();
            // Container.ChildLifetimeScopeBeginning += OnChildLifetimeScopeBeginning;
            // Container.CurrentScopeEnding += OnCurrentScopeEnding;
        }

        private void OnChildLifetimeScopeBeginning(object sender, LifetimeScopeBeginningEventArgs e)
        {
            Debug.WriteLine("Scope beginning " + e.LifetimeScope.Tag);
            e.LifetimeScope.ChildLifetimeScopeBeginning += OnChildLifetimeScopeBeginning;

            e.LifetimeScope.CurrentScopeEnding += OnCurrentScopeEnding;
        }

        private void OnCurrentScopeEnding(object sender, LifetimeScopeEndingEventArgs e)
        {
            Debug.WriteLine("Scope ending " + e.LifetimeScope.Tag);
        }

        #endregion

        /// <summary>
        /// </summary>
        public override event EventHandler<AppStartupEventArgs> AppStartup;

        /// <summary>
        /// </summary>
        /// <param name="appModule"></param>
        public override void AddModule(IModule appModule)
        {
            _modules.Add(appModule);
        }

        public IComponentContext ComponentContext
        {
            get { return GetLifetimeScope(); }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public override ILifetimeScope GetLifetimeScope()
        {
            if (LifetimeScope != null) return LifetimeScope;

            if (Container == null) Container = BuildContainer();

            LifetimeScope = _root.BeginLifetimeScope("Primary");
            return LifetimeScope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [NotNull]
        public override ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> action)
        {
            if (LifetimeScope != null)
                // if (action != null)
                // {
                // throw new AppInvalidOperationException();
                // }
                return LifetimeScope;

            Container ??= BuildContainer();

            LifetimeScope = Container.BeginLifetimeScope("initial scope", action);
            return LifetimeScope;
        }

        /// <inheritdoc />
        protected override IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            var b = builder.Build();
            _root = b.BeginLifetimeScope(BuildContainer2);
            
            return b;
        }
        class myPaginayor : nDyamicDocumentPaginator
        {

        }
        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <returns></returns>
        [NotNull]
        protected void BuildContainer2(ContainerBuilder builder)
        {

            builder.RegisterModule<AttributedMetadataModule>();
            builder.RegisterMetadataRegistrationSources();
            builder.RegisterModule<LegacyAppBuildModule>();
            builder.RegisterModule<NouveauAppModule>();

            // foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            // {
            // DebugUtils.WriteLine(assembly.GetName().ToString());
            // }

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                foreach (var assembly in entryAssembly.GetReferencedAssemblies())
                    //DebugUtils.WriteLine("ref:" + assembly.Name);
                    if (assembly.Name == "AnalysisAppLib")
                        Assembly.Load(assembly);

            var yy = AppDomain.CurrentDomain.GetAssemblies()
                .Where(
                    assembly =>
                    {
                        if (assembly.GetName().Name == "WpfLib" || assembly.GetName().Name == "AnalysisAppLib")
                            return true;

                        return false;
                    }
                ).ToArray();
            builder.RegisterAssemblyTypes(yy)
                .AssignableTo<JsonConverter>().PublicOnly()
                .AsImplementedInterfaces()
                .As<JsonConverter>()
                .AsSelf().WithCallerMetadata();

            var jsonSerializerOptions = new JsonSerializerOptions();
            JsonConverters.AddJsonConverters(jsonSerializerOptions);
            foreach (var jsonConverter in jsonSerializerOptions.Converters)
                builder.RegisterInstance(jsonConverter).AsSelf().As<JsonConverter>().AsImplementedInterfaces()
                    .WithCallerMetadata();

            //builder.RegisterInstance ( jsonSerializerOptions ).As < JsonSerializerOptions > ( ) ;
            builder.Register(
                    (context, parameters) =>
                    {
                        var o = new JsonSerializerOptions();
                        foreach (var cc in context
                            .Resolve<IEnumerable<JsonConverter>>())
                            o.Converters.Add(cc);

                        return o;
                    }
                )
                .As<JsonSerializerOptions>().WithCallerMetadata();

            foreach (var module in _modules)
            {
                LogDebug($"Registering module {module}");
                builder.RegisterModule(module);
            }

            builder.RegisterBuildCallback(scope =>
            {
                scope.ChildLifetimeScopeBeginning +=
                    OnChildLifetimeScopeBeginning;
                scope.CurrentScopeEnding += OnCurrentScopeEnding;
                scope.ResolveOperationBeginning += OnResolveOperationBeginning;
            });

            // scope => scope.ChildLifetimeScopeBeginning += (sender, args) =>
            // {

            // if (args.LifetimeScope.Tag.GetType() == typeof(object))
            // {
            // throw new AppInvalidOperationException();
            // }

            // });

         
        }

        private void OnResolveOperationBeginning(object sender, ResolveOperationBeginningEventArgs e)
        {
            e.ResolveOperation.InstanceLookupBeginning += ResolveOperationOnInstanceLookupBeginning;
        }

        private void ResolveOperationOnInstanceLookupBeginning(object sender, InstanceLookupBeginningEventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var keyValuePair in e.InstanceLookup.ComponentRegistration.Metadata)
                sb.Append($"{keyValuePair.Key}={keyValuePair.Value}; ");

            DebugUtils.WriteLine(sb.ToString());
            DebugUtils.WriteLine(e.InstanceLookup.ActivationScope.Tag);
        }

        private void LogDebug(string message)
        {
            //Subject.OnNext(new AppLogMessage(message));
        }

        /// <summary>
        /// </summary>
        public override void Startup()
        {
            if (!_disableServiceHost)
            {
                _host = new ApplicationInstanceHost(Container);
                _host.HostOpen();
            }

            base.Startup();
        }

        /// <summary>
        /// </summary>
        protected override void Shutdown()
        {
            base.Shutdown();
#if NETSTANDARD || NETFRAMEWORK
            AppLoggingConfigHelper.ServiceTarget?.Dispose();
#endif
        }

        /// <summary>
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        // ReSharper disable once FunctionComplexityOverflow
        protected override IEnumerable LoadConfiguration(Action<string> logMethod2)
        {
            if (logMethod2 == null) throw new ArgumentNullException(nameof(logMethod2));

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            logMethod2($"Using {config.FilePath} for configuration");
            var type1 = typeof(ContainerHelperSection);

            try
            {
                var sections = config.Sections;
                foreach (ConfigurationSection configSection in sections)
                    try
                    {
                        var type = configSection.SectionInformation.Type;
                        var sectionType = Type.GetType(type);
                        if (sectionType == null
                            || sectionType.Assembly != type1.Assembly)
                            continue;

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
                            if (item1.MemberType != MemberTypes.Property) continue;

                            var attr = at.TargetType.GetProperty(item1.Name);
                            try
                            {
                                var configVal =
                                    ((PropertyInfo) item1).GetValue(configSection);
                                if (attr != null) attr.SetValue(configTarget, configVal);
                            }
                            catch (Exception ex)
                            {
                                logMethod2(
                                    $"Unable to set property {item1.Name}: {ex.Message}"
                                );
                            }
                        }


                        ConfigSettings.Add(configTarget);
                    }
                    catch (Exception ex1)
                    {
                        Logger.Error(ex1, ex1.Message);
                    }
            }
            catch (Exception ex)
            {
                logMethod2(ex.Message);
            }

            return ConfigSettings;
        }


        #region IDisposable

        #endregion
    }

    internal class nDyamicDocumentPaginator
    {
    }
}