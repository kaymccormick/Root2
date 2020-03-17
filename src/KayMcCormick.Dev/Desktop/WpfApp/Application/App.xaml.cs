﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
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
using KayMcCormick.Dev.AppBuild ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using NLog.Fluent ;
using Vanara.Extensions.Reflection ;
using WpfApp.Core ;
using WpfApp.Core.Exceptions ;
using WpfApp.Core.Interfaces ;
using WpfApp.Debug ;

namespace WpfApp
{
    /// <summary>Root namespace for the WPF application infrastructure.</summary>
    [ CompilerGenerated ]
    [ UsedImplicitly ]
    [ SuppressMessage (
                          "Performance"
                        , "CA1812:Avoid uninstantiated internal classes"
                        , Justification = "<Pending>"
                      ) ]
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
    ///
    [ LoggingRule ( typeof ( ResourceTreeViewItemTemplateSelector ) , nameof ( LogLevel.Debug ) ) ]
    public sealed partial class App : BaseApp , IDisposable
    {
        private ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo _appNode ;

        /// <summary></summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">
        ///     The <see cref="DebugEventArgs" /> instance containing the
        ///     event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnDebugMessageRaised
        // ReSharper disable once UnusedType.Global
        public delegate void OnDebugMessageRaised ( object sender , DebugEventArgs args ) ;

        /// <summary>Initializes a new instance of the <see cref="App"/> class.</summary>
        /// <param name="debugEventHandler">The debug event handler.</param>
        public App ( EventHandler < DebugEventArgs > debugEventHandler = null )
        {
            //MappedDiagnosticsLogicalContext.
            if ( debugEventHandler != null )
            {
                DebugMessage += debugEventHandler ;
            }

            // DoLogMessage(
            // folderPath
            // );
            AppLoggingConfigHelper.EnsureLoggingConfigured (
                                                            message => OnDebugMessage (
                                                                                       new
                                                                                           DebugEventArgs (
                                                                                                           message
                                                                                                          )
                                                                                      )
                                                           ) ;

            Logger = LogManager.LogFactory.GetCurrentClassLogger < MyLogger > ( ) ;
            

            try
            {
                var s = new TaskCompletionSource < int > ( ) ;

                TCS = s ;
                var cd = AppDomain.CurrentDomain ;
                cd.AssemblyLoad += CurrentDomainOnAssemblyLoad ;
                //cd.TypeResolve += CdOnTypeResolve;
                cd.ProcessExit += ( sender , args ) => {
                    var argStr = args == null ? "Args is null" : args.ToString ( ) ;

                    DebugLog?.Invoke ( argStr) ;
                } ;
                cd.UnhandledException += OnAppDomainUnhandledException ;
                cd.ResourceResolve    += CdOnResourceResolve ;

                cd.FirstChanceException += CurrentDomainOnFirstChanceException ;
            }
            catch ( Exception ex )
            {
                DebugLog (ex.Message ) ;
            }
        }
        
        /// <summary>Gets the task completion source.</summary>
        /// <value>The TCS.</value>
        public TaskCompletionSource < int > TCS { get ; }


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
        public bool Initialized { get ; set ; }

        /// <summary>Gets or sets a value indicating whether [show main window].</summary>
        /// <value>
        ///     <see language="true"/> if [show main window]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ShowMainWindow
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool ShowMainWindow { get ; set ; } = true ;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>Gets or sets the menu item list collection view.</summary>
        /// <value>The menu item list collection view.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for MenuItemListCollectionView
        public ListCollectionView MenuItemListCollectionView { get ; set ; }

        /// <summary>Gets or sets the dispatcher op.</summary>
        /// <value>The dispatcher op.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DispatcherOp
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private DispatcherOperation DispatcherOp { get ; set ; }

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
        public bool ProcessArgs { get ; set ; } = false ;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose ( )
        {
            var cd = AppDomain.CurrentDomain ;
            cd.AssemblyLoad -= CurrentDomainOnAssemblyLoad ;
            //cd.TypeResolve += CdOnTypeResolve;

            cd.UnhandledException -= OnAppDomainUnhandledException ;
            cd.ResourceResolve    -= CdOnResourceResolve ;

            cd.FirstChanceException -= CurrentDomainOnFirstChanceException ;
        }

        /// <summary>Event for a debug event.</summary>
        public event EventHandler < DebugEventArgs > DebugMessage ;

        // ReSharper disable once UnusedMember.Local
        private void DoLogMessage (
            string                      message
          , [ CallerMemberName ] string callerMemberName = null
          , [ CallerFilePath ]   string callerFilePath   = ""
          , [ CallerLineNumber ] int    callerLineNumber = 0
        )
        {
            try
            {
                new LogBuilder ( Logger ).Level ( LogLevel.Debug )
                                         .Message ( message )
                                         .Property ( "callerLineNumber" , callerFilePath )
                                         .Property ( "callerMemberName" , callerMemberName )
                                         .Property ( "callerFilePath" ,   callerFilePath )
                                         .Write (
                                                 callerMemberName
                                                 // ReSharper disable once ExplicitCallerInfoArgument
                                               , callerFilePath
                                                 // ReSharper disable once ExplicitCallerInfoArgument
                                               , callerLineNumber
                                                ) ;
                System.Diagnostics.Debug.WriteLine ( message ) ;
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine (
                                                    $"Received exception trying to log {message}"
                                                    + ( callerMemberName.IsNullOrEmpty ( )
                                                            ? ""
                                                            : $" from {callerMemberName} at {callerFilePath}:{callerLineNumber}: {ex.Message}"
                                                      )
                                                   ) ;
                System.Diagnostics.Debug.WriteLine ( ex ) ;
            }
        }

        private Assembly CdOnResourceResolve ( object sender , ResolveEventArgs args )
        {
            return null ;
        }

        private void OnAppDomainUnhandledException ( object sender , UnhandledExceptionEventArgs e )
        {
            var message = e.ExceptionObject.GetPropertyValue < string > ( "Message" ) ;
            var err = new UnhandledException (
                                              "UnhandledException: " + message
                                            , e.ExceptionObject as Exception
                                             ) ;

            var str = $"{err.Message} Terminating={e.IsTerminating}" ;
            DebugLog ( str) ;
        }


        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private Assembly CdOnTypeResolve ( object sender , ResolveEventArgs args )
        {
            DebugLog($"{args.Name}");
            DebugLog($"Requesting assembly is {args.RequestingAssembly.FullName}");
            return null ;
        }

        private void CurrentDomainOnAssemblyLoad ( object sender , AssemblyLoadEventArgs args )
        {
            Logger?.Trace ( args.LoadedAssembly ) ;
        }

        private void CurrentDomainOnFirstChanceException (
            object                        sender
          , FirstChanceExceptionEventArgs e
        )
        {
            Utils.HandleInnerExceptions ( e.Exception ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private string RegOutput ( IComponentRegistration registration )
        {
            var registrationActivator = registration.Activator ;
            if ( registrationActivator != null )
            {
                var registrationActivatorLimitType = registrationActivator.LimitType ;
                if ( registrationActivatorLimitType != null )
                {
                    return registrationActivatorLimitType.FullName ;
                }
            }

            return "" ;
        }

        // ReSharper disable once UnusedMember.Local
        private void OpenWindowExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
        }

        private object DispatcherOperationCallback ( object arg )
        {

            AppInitialize ( ) ;

            WpfApp.Controls.Windows.MainWindow mainWindow ;
            try
            {
                mainWindow = Scope.Resolve < WpfApp.Controls.Windows.MainWindow > ( ) ;
            }
            catch ( Exception ex )
            {
                DebugLog (ex.Message ) ;
                // ReSharper disable once RedundantArgumentDefaultValue
                ErrorExit ( ExitCode.GeneralError ) ;
                return null ;
            }

            if ( ShowMainWindow )
            {
                try
                {
                    //mainWindow.WindowState = WindowState.Minimized ;
                    mainWindow.Show ( ) ;
                }
                catch ( Exception ex )
                {
                    DebugLog (ex.ToString() ) ; //?.Error ( ex , ex.Message ) ;
                }

                var mainWindow2 = new MainWindow ( ) ;
                mainWindow2.Show ( ) ;
            }

            Initialized = true ;

            return null ;
        }

        private void AppInitialize ( )
        {
            SetupTracing ( ) ;


            var loggerTracker = Scope.Resolve < ILoggerTracker > ( ) ;
            var myLoggerName = typeof ( App ).FullName ;
            loggerTracker.LoggerRegistered += ( sender , args ) => {
                if ( args.Logger.Name == myLoggerName )
                {
                    args.Logger.Trace (
                                       "Received logger for application in LoggerRegistered handler."
                                      ) ;
                }
                else
                {
                    if ( Logger == null )
                    {
                        System.Diagnostics.Debug.WriteLine (
                                                            "got a logger but i don't have one yet"
                                                           ) ;
                    }
                }
            } ;

            Logger = Scope.Resolve < ILogger > (
                                                new TypedParameter (
                                                                    typeof ( Type )
                                                                  , typeof ( App )
                                                                   )
                                               ) ;


            if ( Scope.IsRegistered < IMenuItemCollection > ( ) )
            {
                var menuItemList = Scope.Resolve < IMenuItemCollection > ( ) ;
                MenuItemListCollectionView    = new ListCollectionView ( menuItemList ) ;
                Resources[ "MyMenuItemList" ] = menuItemList ;
            }


            var handler = new RoutedEventHandler ( WindowLoaded ) ;

            EventManager.RegisterClassHandler (
                                               typeof ( Window )
                                             , FrameworkElement.LoadedEvent
                                             , handler
                                              ) ;



            // var converter = new RegistrationConverter ( Scope , objectIdProvider ) ;
            // Resources[ "RegistrationConverter" ] = converter ;
            TCS.SetResult ( 1 ) ;
        }


        private void WindowLoaded ( object o , RoutedEventArgs args )
        {
            if ( _appNode == null )
            {
                PopulateResourcesTree ( ) ;
            }

            HandleWindow ( o as Window ) ;
            var fe = ( FrameworkElement ) o ;
            DebugLog?.Invoke (fe.ToString() ) ;
            Props.SetMenuItemListCollectionView ( fe , MenuItemListCollectionView ) ;
            
            Props.SetAssemblyList (
                                   fe
                                 , new AssemblyList ( AppDomain.CurrentDomain.GetAssemblies ( ) )
                                  ) ;
        }

        private void PopulateResourcesTree ( )
        {
            try
            {
                var current = ( App ) Current ;
                _appNode = new ResourceNodeInfo { Key = "Application" , Data = current } ;
                var appResources = new ResourceNodeInfo
                                   {
                                       Key = "Resources" , Data = current.Resources
                                   } ;
                _appNode.Children.Add ( appResources ) ;
                AddResourceNodeInfos ( appResources ) ;
                AllResourcesCollection.Add ( _appNode ) ;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch ( Exception ex )
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }
        }

        public ObservableCollection < ResourceNodeInfo > AllResourcesCollection
            => _allResourcesCollection ;

        private void AddResourceNodeInfos ( ResourceNodeInfo appResources )
        {
            var res = ( ResourceDictionary ) appResources.Data ;
            appResources.SourceUri = res.Source ;

            foreach ( var md in res.MergedDictionaries )
            {
                var mdr = new ResourceNodeInfo { Key = md.Source , Data = md } ;
                AddResourceNodeInfos ( mdr ) ;
                appResources.Children.Add ( mdr ) ;
            }

            foreach ( DictionaryEntry haveResourcesResource in res )
            {
                if ( haveResourcesResource.Key      != null
                     && haveResourcesResource.Value != null )
                {
                    var resourceInfo = new ResourceNodeInfo
                                       {
                                           Key  = haveResourcesResource.Key
                                         , Data = haveResourcesResource.Value
                                       } ;
                    appResources.Children.Add ( resourceInfo ) ;
                }
            }
        }

        private void HandleWindow ( Window w )
        {
            var winNode = new ResourceNodeInfo
                          {
                              Key = w.GetType ( ) , Data = new ControlWrap < Window > ( w )
                          } ;
            _appNode.Children.Add ( winNode ) ;
            var winRes = new ResourceNodeInfo { Key = "Resources" , Data = w.Resources } ;
            winNode.Children.Add ( winRes ) ;
            AddResourceNodeInfos ( winRes ) ;
        }

        private void AddEventListeners ( )
        {
            try
            {
                EventManager.RegisterClassHandler (
                                                   typeof ( Window )
                                                 , UIElement.KeyDownEvent
                                                 , new KeyEventHandler ( OnKeyDown )
                                                  ) ;
            }
            catch ( Exception ex )
            {
                DebugLog (ex.ToString()) ;
            }
        }

        private void OnKeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.Key                         == Key.T
                 && e.KeyboardDevice.Modifiers == ( ModifierKeys.Control | ModifierKeys.Alt ) )
            {
                Process.Start (
                               new ProcessStartInfo (
                                                     @".\Demo.XamlDesigner.exe"
                                                   , @"..\WpfApp1\Windows\MainWindow.xaml"
                                                    ) { WorkingDirectory = @"..\..\..\tools" }
                              ) ;
            }
        }

        private void Application_DispatcherUnhandledException (
            object                                sender
          , DispatcherUnhandledExceptionEventArgs e
        )
        {
            var msg =
                $"{nameof ( Application_DispatcherUnhandledException )}: {e.Exception.Message}" ;
            if ( DebugLog == null )
            {
                DebugLog = message => System.Diagnostics.Debug.WriteLine ( message ) ;
            }
            DebugLog (msg ) ;
            var inner = e.Exception.InnerException ;
            var seen = new HashSet < object > ( ) ;
            while ( inner != null
                    && ! seen.Contains ( inner ) )
            {
                DebugLog (inner.Message ) ;
                seen.Add ( inner ) ;
                inner = inner.InnerException ;
            }
        }

        private void App_OnExit ( object sender , ExitEventArgs e )
        {
        }

        public override IEnumerable < IModule > GetModules ( )
        {
            var appBuildModule = new AppBuildModule ( ) ;
            var a = appBuildModule.GetAssembliesForScanning ( ).ToList ( ) ;
            a.Add ( typeof ( App ).Assembly ) ;
            yield return appBuildModule ;
            var wpfAppBuildModule = new WpfAppBuildModule ( ) ;
            wpfAppBuildModule.AssembliesForScanning.AddRange ( a ) ;
            yield return wpfAppBuildModule ;
        }

        /// <summary>
        ///     Raises the <see cref="System.Windows.Application.Startup"/>
        ///     event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="System.Windows.StartupEventArgs" /> that
        ///     contains the event data.
        /// </param>
        protected override void OnStartup ( StartupEventArgs e )
        {
            if ( e != null )
            {
                DoOnStartup ( e.Args ) ;

                base.OnStartup ( e ) ;
            }
        }
#if COMMANDLINE
        protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }
#endif

        /// <summary>Does the on startup.</summary>
        /// <param name="args">The arguments.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoOnStartup
        public void DoOnStartup ( string[] args )
        {
            AddEventListeners ( ) ;
            if ( args != null
                 && ProcessArgs
                 && args.Any ( ) )
            {
                var windowName = args[ 0 ] ;
                var xaml = "../Windows/" + windowName + ".xaml" ;
                var converter = TypeDescriptor.GetConverter ( typeof ( Uri ) ) ;
                if ( converter.CanConvertFrom ( typeof ( string ) ) )
                {
                    StartupUri = ( Uri ) converter.ConvertFrom ( xaml ) ;
                    Logger.Debug ( "Startup URI is {startupUri}" , StartupUri ) ;
                }
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                var dispatcherOperation = Dispatcher.BeginInvoke (
                                                                  DispatcherPriority.Send
                                                                , ( DispatcherOperationCallback )
                                                                  DispatcherOperationCallback
                                                                , null
                                                                 ) ;
                DispatcherOp = dispatcherOperation ;
            }
        }

        /// <summary>Raises the <see cref="DebugMessage"/> event.</summary>
        /// <param name="e">
        ///     The <see cref="DebugEventArgs" /> instance containing the
        ///     event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnDebugMessage
        private void OnDebugMessage ( DebugEventArgs e )
        {
            Logger?.Debug ( e.Message ) ;
            DebugMessage?.Invoke ( this , e ) ;
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MyLogger'
}