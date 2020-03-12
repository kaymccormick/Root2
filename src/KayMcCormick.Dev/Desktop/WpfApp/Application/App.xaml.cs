﻿using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Configuration ;
using System.Diagnostics ;
using System.Diagnostics.CodeAnalysis ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.ExceptionServices ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Input ;
using System.Windows.Threading ;
using Autofac ;
using Autofac.Core ;
using Castle.Core.Internal ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using NLog.Fluent ;
using Vanara.Extensions.Reflection ;
using WpfApp.Config ;
using WpfApp.Core ;
using WpfApp.Core.Container ;
using WpfApp.Core.Exceptions ;
using WpfApp.Core.Interfaces ;
using WpfApp.Debug ;
using IContainer = Autofac.IContainer ;

namespace WpfApp
{
    /// <summary>Root namespace for the WPF application infrastructure.</summary>
    [CompilerGenerated]
    [UsedImplicitly]
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "<Pending>")]
    internal class NamespaceDoc
    {
    }
}

//TODO move namespaces to WpfApp
namespace WpfApp.Application
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : BaseApp, IDisposable
    {
        /// <summary></summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">
        ///     The <see cref="DebugEventArgs" /> instance containing the
        ///     event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnDebugMessageRaised
        // ReSharper disable once UnusedType.Global
        public delegate void OnDebugMessageRaised(object sender, DebugEventArgs args);

        private IContainer _container;
        private readonly ContainerHelperSettings _containerHelperSettings = null;

        /// <summary>Initializes a new instance of the <see cref="App"/> class.</summary>
        /// <param name="debugEventHandler">The debug event handler.</param>
        public App(EventHandler<DebugEventArgs> debugEventHandler = null)
        {
            //MappedDiagnosticsLogicalContext.
            if (debugEventHandler != null)
            {
                DebugMessage += debugEventHandler;
            }

            DebugLog = DoLogMessage;
            
            // DoLogMessage(
            // folderPath
            // );
            AppLoggingConfigHelper.EnsureLoggingConfigured(
                                                            message => OnDebugMessage(
                                                                                       new
                                                                                           DebugEventArgs(
                                                                                                           message
                                                                                                          )
                                                                                      )
                                                           );
            GlobalDiagnosticsContext.Set(
                                          "ExecutionContext"
                                        , new ExecutionContextImpl
                                        {
                                            Application = KayMcCormick.Dev.Logging.Application
                                                                      .MainApplication
                                        }
                                         );

            Logger = LogManager.LogFactory.GetCurrentClassLogger<MyLogger>();
            ApplyConfiguration();

            try
            {
                var s = new TaskCompletionSource<int>();

                TCS = s;
                var cd = AppDomain.CurrentDomain;
                cd.AssemblyLoad += CurrentDomainOnAssemblyLoad;
                //cd.TypeResolve += CdOnTypeResolve;
                cd.ProcessExit += (sender, args) =>
                {
                    var argStr = args == null ? "Args is null" : args.ToString();

                    DebugLog($"Exiting. args is {argStr}");
                };
                cd.UnhandledException += OnAppDomainUnhandledException;
                cd.ResourceResolve += CdOnResourceResolve;

                cd.FirstChanceException += CurrentDomainOnFirstChanceException;
            }
            catch (Exception ex)
            {
                DebugLog(ex + "exception in constructor");
            }
        }

        /// <summary>Gets the configuration settings.</summary>
        /// <value>The configuration settings.</value>
        public List<object> ConfigSettings { get; } = new List<object>();

        private ILogger Logger { get; set; }

        private LogDelegates.LogMethod2 DebugLog { get; }


        /// <summary>Gets the task completion source.</summary>
        /// <value>The TCS.</value>
        public TaskCompletionSource<int> TCS { get; }

        private ILifetimeScope AppContainer { get; set; }

        /// <summary>Gets or sets a value indicating whether [stage2 complete].</summary>
        /// <value>
        ///     <see language="true"/> if [stage2 complete]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Stage2Complete
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool Stage2Complete { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="App" /> is
        ///     initialized.
        /// </summary>
        /// <value>
        ///     <see language="true"/> if initialized; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Initialized
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool Initialized { get; set; }

        /// <summary>Gets or sets a value indicating whether [show main window].</summary>
        /// <value>
        ///     <see language="true"/> if [show main window]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ShowMainWindow
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool ShowMainWindow { get; set; } = true;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Gets or sets the menu item list collection view.</summary>
        /// <value>The menu item list collection view.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for MenuItemListCollectionView
        public ListCollectionView MenuItemListCollectionView { get; set; }

        /// <summary>Gets or sets the dispatcher op.</summary>
        /// <value>The dispatcher op.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DispatcherOp
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DispatcherOperation DispatcherOp { get; set; }

        /// <summary>
        ///     Whether the application should process command line arguments. used
        ///     for testing.
        /// </summary>
        /// <value>
        ///     <see language="true"/> if [process arguments]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ProcessArgs
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool ProcessArgs { get; set; } = false;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {

            var cd = AppDomain.CurrentDomain;
            cd.AssemblyLoad -= CurrentDomainOnAssemblyLoad;
            //cd.TypeResolve += CdOnTypeResolve;

            cd.UnhandledException -= OnAppDomainUnhandledException;
            cd.ResourceResolve -= CdOnResourceResolve;

            cd.FirstChanceException -= CurrentDomainOnFirstChanceException;
            _container?.Dispose();
            AppContainer?.Dispose();
        }

        /// <summary>Event for a debug event.</summary>
        public event EventHandler<DebugEventArgs> DebugMessage;

        private void ApplyConfiguration()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DebugLog(config.FilePath);
            var type1 = typeof(ContainerHelperSection);

            try
            {
                var sections = config.Sections;
                foreach (ConfigurationSection configSection in sections)
                {
                    try
                    {
                        var type = configSection.SectionInformation.Type;
                        // DoLogMethod ( $"Type is {type}" ) ;
                        var sectionType = Type.GetType(type);
                        if (sectionType != null && sectionType.Assembly == type1.Assembly)
                        {
                            DebugLog("Found section " + sectionType.Name);
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
                                    foreach (var memberInfo in infos)
                                    {
                                        var attr = at.TargetType.GetProperty(item1.Name);
                                        try
                                        {
                                            var configVal =
                                                ((PropertyInfo)memberInfo.Item1).GetValue(
                                                                                                configSection
                                                                                               );
                                            if (attr != null)
                                            {
                                                attr.SetValue(configTarget, configVal);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DebugLog?.Invoke(
                                                              $"Unable to set property {attr.Name}: {ex.Message}"
                                                             );
                                        }
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
                DebugLog(ex.Message);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void DoLogMessage(
            string message
          , [CallerMemberName] string callerMemberName = null
          , [CallerFilePath]   string callerFilePath = ""
          , [CallerLineNumber] int callerLineNumber = 0
        )
        {
            try
            {
                new LogBuilder(Logger).Level(LogLevel.Debug)
                                         .Message(message)
                                         .Property("callerLineNumber", callerFilePath)
                                         .Property("callerMemberName", callerMemberName)
                                         .Property("callerFilePath", callerFilePath)
                                         .Write(
                                                 callerMemberName
                                                 // ReSharper disable once ExplicitCallerInfoArgument
                                               , callerFilePath
                                                 // ReSharper disable once ExplicitCallerInfoArgument
                                               , callerLineNumber
                                                );
                System.Diagnostics.Debug.WriteLine(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                                                   $"Received exception trying to log {message}"
                                                   + (callerMemberName.IsNullOrEmpty()
                                                          ? ""
                                                          : $" from {callerMemberName} at {callerFilePath}:{callerLineNumber}: {ex.Message}"
                                                     )
                                                  );
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private Assembly CdOnResourceResolve(object sender, ResolveEventArgs args)
        {
            DebugLog($"nameof(CdOnResourceResolve): {args.Name}");
            return null;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = e.ExceptionObject.GetPropertyValue<string>("Message");
            var err = new UnhandledException(
                                              "UnhandledException: " + message
                                            , e.ExceptionObject as Exception
                                             );

            var str = $"{err.Message} Terminating={e.IsTerminating}";
            DebugLog(str);
        }


        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private Assembly CdOnTypeResolve(object sender, ResolveEventArgs args)
        {
            DebugLog($"{args.Name}");
            DebugLog($"Requesting assembly is {args.RequestingAssembly.FullName}");
            return null;
        }

        private void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Logger?.Trace(args.LoadedAssembly);
        }

        private void CurrentDomainOnFirstChanceException(
            object sender
          , FirstChanceExceptionEventArgs e
        )
        {
            Utils.HandleInnerExceptions ( e.Exception) ;
        }

        private void HandleInnerExceptions ( FirstChanceExceptionEventArgs e )
        {
            try
            {
                var msg = $"{e.Exception.Message}" ;
                DebugLog ( msg ) ;
                System.Diagnostics.Debug.WriteLine ( "Exception: " + e.Exception ) ;
                var inner = e.Exception.InnerException ;
                var seen = new HashSet < object > ( ) ;
                while ( inner != null
                        && ! seen.Contains ( inner ) )
                {
                    DebugLog ( inner.Message ) ;
                    seen.Add ( inner ) ;
                    inner = inner.InnerException ;
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine ( "Exception: " + ex ) ;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private string RegOutput(IComponentRegistration registration)
        {
            var registrationActivator = registration.Activator;
            if (registrationActivator != null)
            {
                var registrationActivatorLimitType = registrationActivator.LimitType;
                if (registrationActivatorLimitType != null)
                {
                    return registrationActivatorLimitType.FullName;
                }
            }

            return "";
        }

        // ReSharper disable once UnusedMember.Local
        private void OpenWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugLog($"{sender} {e.Parameter}");
        }

        [SuppressMessage(
                              "Usage"
                            , "VSTHRD001:Avoid legacy thread switching APIs"
                            , Justification = "<Pending>"
                          )]
        private object DispatcherOperationCallback(object arg)
        {
            DebugLog(nameof(DispatcherOperationCallback));

            AppInitialize();

            WpfApp.Controls.Windows.MainWindow mainWindow;
            try
            {
                mainWindow = AppContainer.Resolve<WpfApp.Controls.Windows.MainWindow>();
                DebugLog($"Received {mainWindow} ");
            }
            catch (Exception ex)
            {
                DebugLog("Cant resolve main Window: " + ex.Message);
                // ReSharper disable once RedundantArgumentDefaultValue
                ErrorExit(ExitCode.GeneralError);
                return null;
            }

            if (ShowMainWindow)
            {
                try
                {
                    //mainWindow.WindowState = WindowState.Minimized ;
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    DebugLog(ex.Message); //?.Error ( ex , ex.Message ) ;
                }

                var mainWindow2 = new MainWindow();
                mainWindow2.Show();

            }

            Initialized = true;

            return null;
        }

        private void AppInitialize()
        {
            AppContainer = ContainerHelper.SetupContainer(
                                                           out var container
                                                         , null
                                                         , _containerHelperSettings
                                                          );
            _container = container;

            SetupTracing();


            var loggerTracker = AppContainer.Resolve<ILoggerTracker>();
            var myLoggerName = typeof(App).FullName;
            loggerTracker.LoggerRegistered += (sender, args) =>
            {
                if (args.Logger.Name == myLoggerName)
                {
                    args.Logger.Trace(
                                       "Received logger for application in LoggerRegistered handler."
                                      );
                }
                else
                {
                    if (Logger == null)
                    {
                        System.Diagnostics.Debug.WriteLine(
                                                           "got a logger but i don't have one yet"
                                                          );
                    }
                }
            };

            Logger = AppContainer.Resolve<ILogger>(
                                                       new TypedParameter(
                                                                           typeof(Type)
                                                                         , typeof(App)
                                                                          )
                                                      );


            if (AppContainer.IsRegistered<IMenuItemCollection>())
            {
                var menuItemList = AppContainer.Resolve<IMenuItemCollection>();
                MenuItemListCollectionView = new ListCollectionView(menuItemList);
                Resources["MyMenuItemList"] = menuItemList;
            }


            var handler = new RoutedEventHandler(MainWindowLoaded);

            EventManager.RegisterClassHandler(
                                               typeof(Window)
                                             , FrameworkElement.LoadedEvent
                                             , handler
                                              );



            // var converter = new RegistrationConverter ( AppContainer , objectIdProvider ) ;
            // Resources[ "RegistrationConverter" ] = converter ;
            Stage2Complete = true;
            TCS.SetResult(1);
        }


        private void MainWindowLoaded(object o, RoutedEventArgs args)
        {
            var fe = ( FrameworkElement ) o;
            DebugLog(nameof(MainWindowLoaded));
            Props.SetMenuItemListCollectionView(fe, MenuItemListCollectionView);
            DebugLog($"Setting LifetimeScope DependencyProperty to {AppContainer}");
            Props.SetAssemblyList(
                                   fe
                                 , new AssemblyList(AppDomain.CurrentDomain.GetAssemblies())
                                  );
            Props.SetContainer(fe, _container);
            AttachedProperties.SetLifetimeScope(fe, AppContainer);
        }

        private void AddEventListeners()
        {
            try
            {
                EventManager.RegisterClassHandler(
                                                   typeof(Window)
                                                 , UIElement.KeyDownEvent
                                                 , new KeyEventHandler(OnKeyDown)
                                                  );
            }
            catch (Exception ex)
            {
                DebugLog(ex.Message);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T
                 && e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt))
            {
                Process.Start(
                               new ProcessStartInfo(
                                                     @".\Demo.XamlDesigner.exe"
                                                   , @"..\WpfApp1\Windows\MainWindow.xaml"
                                                    )
                               { WorkingDirectory = @"..\..\..\tools" }
                              );
            }
        }

        private void Application_DispatcherUnhandledException(
            object sender
          , DispatcherUnhandledExceptionEventArgs e
        )
        {
            var msg =
                $"{nameof(Application_DispatcherUnhandledException)}: {e.Exception.Message}";
            DebugLog(msg);
            var inner = e.Exception.InnerException;
            var seen = new HashSet<object>();
            while (inner != null
                    && !seen.Contains(inner))
            {
                DebugLog(inner.Message);
                seen.Add(inner);
                inner = inner.InnerException;
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            DebugLog($"Application exiting.  Exit code is {e.ApplicationExitCode}");
        }

        /// <summary>
        ///     Raises the <see cref="System.Windows.Application.Startup"/>
        ///     event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="System.Windows.StartupEventArgs" /> that
        ///     contains the event data.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e != null)
            {
                DoOnStartup(e.Args);

                base.OnStartup(e);
            }
        }
        #if COMMANDLINE
        protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }
#endif

        /// <summary>Does the on startup.</summary>
        /// <param name="args">The arguments.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoOnStartup
        public void DoOnStartup(string[] args)
        {
            AddEventListeners();
            if (args != null && (ProcessArgs && args.Any()))
            {
                var windowName = args[0];
                var xaml = "../Windows/" + windowName + ".xaml";
                var converter = TypeDescriptor.GetConverter(typeof(Uri));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    StartupUri = (Uri)converter.ConvertFrom(xaml);
                    Logger.Debug("Startup URI is {startupUri}", StartupUri);
                }
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                var dispatcherOperation = Dispatcher.BeginInvoke(
                                                                  DispatcherPriority.Send
                                                                , (DispatcherOperationCallback)
                                                                  DispatcherOperationCallback
                                                                , null
                                                                 );
                DispatcherOp = dispatcherOperation;
            }
        }

        /// <summary>Raises the <see cref="DebugMessage"/> event.</summary>
        /// <param name="e">
        ///     The <see cref="DebugEventArgs" /> instance containing the
        ///     event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnDebugMessage
        private void OnDebugMessage(DebugEventArgs e)
        {
            Logger?.Debug(e.Message);
            DebugMessage?.Invoke(this, e);
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MyLogger'
}