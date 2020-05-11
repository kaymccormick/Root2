using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq;
using System.Text.Json;
using System.Windows ;
using System.Windows.Media ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Logging ;
using Microsoft.CodeAnalysis.CSharp;
using NLog ;
using NLog.Targets;
using Application = System.Windows.Application ;

#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif

namespace KayMcCormick.Lib.Wpf
{
#if COMMANDLINE
    public abstract class BaseOptions
    {
        [ Option ( 'q' ) ]
        public bool QuitOnError { get ; set ; }

        [ Option ( 't' ) ]
        public bool EnableTracing { get ; set ; }
    }
#endif
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public abstract class BaseApp : Application , IDisposable
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly bool _disableLogging ;

        private readonly ApplicationInstanceBase _applicationInstance ;
        private readonly ApplicationInstanceBase _createdAppInstance ;
        
        private MiscInstanceInfoProvider _miscInstanceInfoProvider ;

        /// <summary>
        /// </summary>
        protected BaseApp ( ) : this ( null ) { }

        /// <summary>
        /// </summary>
        /// <param name="applicationInstance"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        /// <param name="modules"></param>
        /// <param name="initAction"></param>
        protected BaseApp (
            [ CanBeNull ] ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
          , [ CanBeNull ] IModule[]               modules                     = null
            // ReSharper disable once UnusedParameter.Local
          , Action                  initAction                  = null
        )
        {
            _disableLogging = disableLogging ;

            if ( applicationInstance != null )
            {
                _applicationInstance = applicationInstance ;
            }
            else
            {
                _applicationInstance = _createdAppInstance = new ApplicationInstance (
                                                                                      new
                                                                                          ApplicationInstance.ApplicationInstanceConfiguration (
                                                                                                                                                message
                                                                                                                                                    => {
#if TRACEPROVIDER
                                                                                                                                                        PROVIDER_GUID
                                                                                                                                                       .EventWriteSETUP_LOGGING_EVENT (
                                                                                                                                                                                       message
                                                                                                                                                                                      ) ;
#endif
                                                                                                                                                }
                                                                                                                                                // ReSharper disable once VirtualMemberCallInConstructor
                                                                                                                                              , ApplicationGuid
                                                                                                                                              , null
                                                                                                                                              , disableLogging
                                                                                                                                              , disableRuntimeConfiguration
                                                                                                                                              , disableServiceHost
                                                                                                                                               )
                                                                                     ) ;
            }


            if ( modules != null )
            {
                foreach ( var module in modules )
                {
                    _applicationInstance.AddModule ( module ) ;
                }
            }

            // initAction?.Invoke ( ) ;
            _applicationInstance.Initialize ( ) ;
            _applicationInstance.Startup ( ) ;
            Scope = _applicationInstance.GetLifetimeScope ( ) ;
            var options = Scope.Resolve<JsonSerializerOptions>();
            options.Converters.Add(new JsonConverterLogEventInfo());
            TypeDescriptor.AddProvider(new InstanceInfoProvider(), typeof(InstanceInfo));

            var provider = Scope.Resolve<IControlsProvider>();
            foreach (var providerType in provider.Types)
            {
                if (providerType.Assembly.FullName == typeof(CSharpSyntaxNode).Assembly.FullName)
                {
                    DebugUtils.WriteLine(providerType.ToString());
                }
                TypeDescriptor.AddProvider(provider.Provider, providerType);
                
            }


            foreach ( var myJsonLayout in LogManager
            .Configuration.AllTargets.OfType < TargetWithLayout > ( )
            .Select ( t => t.Layout )
            .OfType < MyJsonLayout > ( ) )
            {
                myJsonLayout.Options = options;
             }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract Guid ApplicationGuid { get ; }

        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [ NotNull ]
        public virtual ILifetimeScope BeginLifetimeScope ( [ NotNull ] object tag )
        {
            return Scope.BeginLifetimeScope ( tag ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public virtual ILifetimeScope BeginLifetimeScope ( )
        {
            return Scope.BeginLifetimeScope ( ) ;
        }

        /// <summary>
        /// </summary>
        protected virtual ILifetimeScope Scope { get ; set ; }

        /// <summary>Gets a value indicating whether [do tracing].</summary>
        /// <value>
        ///     <see language="true" /> if [do tracing]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTracing
        protected virtual bool DoTracing { get ; } = false ;

        /// <summary>
        /// </summary>
        protected virtual void SetupTracing ( )
        {
            PresentationTraceSources.Refresh ( ) ;
            if ( ! DoTracing )
            {
                return ;
            }

            var nLogTraceListener = new NLogTraceListener ( ) ;
            var routedEventSource = PresentationTraceSources.RoutedEventSource ;
            nLogTraceListener.DefaultLogLevel = LogLevel.Debug ;
            nLogTraceListener.ForceLogLevel   = LogLevel.Warn ;
            //nLogTraceListener.LogFactory      = AppContainer.Resolve < LogFactory > ( ) ;
            nLogTraceListener.AutoLoggerName = false ;
            //nLogTraceListener.
            routedEventSource.Switch.Level = SourceLevels.All ;
            var foo = Scope.Resolve < IEnumerable < TraceListener > > ( ) ;
            foreach ( var tl in foo )
            {
                routedEventSource.Listeners.Add ( tl ) ;
            }

            //routedEventSource.Listeners.Add ( new AppTraceListener ( ) ) ;
            routedEventSource.Listeners.Add ( nLogTraceListener ) ;
        }

        /// <summary>
        /// </summary>
        protected virtual LogDelegates.LogMethod DebugLog { get ; set ; }

        /// <summary>Gets the configuration settings.</summary>
        /// <value>The configuration settings.</value>
        public virtual List < object > ConfigSettings { get ; } = new List < object > ( ) ;

        /// <summary>
        /// </summary>
        protected virtual ILogger Logger { get ; set ; }

        /// <summary>
        /// </summary>
        /// <param name="exitCode"></param>
        protected virtual void ErrorExit ( ExitCode exitCode = ExitCode.GeneralError )
        {
            var code = Convert.ChangeType ( ( object ) exitCode , ( TypeCode ) exitCode.GetTypeCode ( ) ) ;
            if ( code == null )
            {
                return ;
            }

            var intCode = ( int ) code ;

            if ( Current == null )
            {
                Process.GetCurrentProcess ( ).Kill ( ) ;
            }
            else
            {
                Current.Shutdown ( intCode ) ;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        protected virtual IEnumerable < IModule > GetModules ( )
        {
            return Array.Empty < IModule > ( ) ;
        }

#region Overrides of Application
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            _createdAppInstance?.Dispose ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
#if COMMANDLINE
            var optionTypes = OptionTypes ;
            var args = e.Args ;
            if ( e.Args.Length       == 0
                 || e.Args[ 0 ][ 0 ] == '-' )
            {
                args = args.Prepend ( "default" ).ToArray ( ) ;
            }
            ArgParseResult = Parser.Default.ParseArguments ( args , optionTypes ) ;
             ArgParseResult.WithNotParsed ( OnArgumentParseError ) ;
#endif
        }


#if COMMANDLINE
protected abstract void OnArgumentParseError ( IEnumerable < object > obj ) ;

        public ParserResult < object > ArgParseResult
        {
            get => _argParseResult ;
            set => _argParseResult = value ;
        }
        public virtual Type[] OptionTypes => _optionType ;
#endif
#endregion

#region IDisposable
        /// <summary>
        /// </summary>
        public virtual void Dispose ( ) { _applicationInstance?.Dispose ( ) ; }
#endregion
    }
}