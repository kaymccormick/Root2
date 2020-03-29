using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text.Json ;
using System.Windows ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.Tracing ;
using NLog ;
using NLog.Targets ;
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
        private readonly bool _disableLogging ;

        private readonly ApplicationInstanceBase _applicationInstance ;
        private readonly ApplicationInstanceBase _createdAppInstance ;
        private          ILifetimeScope          _scope ;
#if COMMANDLINE
        private Type[]                  _optionType ;
        private ParserResult < object > _argParseResult ;
#endif
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
            ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
          , IModule[]               modules                     = null
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
                                                                                          ApplicationInstanceConfiguration (
                                                                                                                            message
                                                                                                                                => {
                                                                                                                                PROVIDER_GUID
                                                                                                                                   .EventWriteSETUP_LOGGING_EVENT (
                                                                                                                                                                   message
                                                                                                                                                                  ) ;
                                                                                                                            }
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

            initAction?.Invoke ( ) ;
            _applicationInstance.Initialize ( ) ;
            _applicationInstance.Startup ( ) ;
            _scope = _applicationInstance.GetLifetimeScope ( ) ;

            foreach ( var myJsonLayout in LogManager
                                         .Configuration.AllTargets.OfType < TargetWithLayout > ( )
                                         .Select ( t => t.Layout )
                                         .OfType < MyJsonLayout > ( ) )
            {
                var options = new JsonSerializerOptions ( ) ;
                foreach ( var optionsConverter in myJsonLayout.Options.Converters )
                {
                    options.Converters.Add ( optionsConverter ) ;
                }

                options.Converters.Add ( new DataTemplateKeyConverter ( ) ) ;
                myJsonLayout.Options = options ;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual ILifetimeScope BeginLifetimeScope ( object tag )
        {
            return Scope.BeginLifetimeScope ( tag ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual ILifetimeScope BeginLifetimeScope ( )
        {
            return Scope.BeginLifetimeScope ( ) ;
        }

        /// <summary>
        /// </summary>
        protected virtual ILifetimeScope Scope { get { return _scope ; } set { _scope = value ; } }

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
            if ( DoTracing )
            {
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
            var code = Convert.ChangeType ( exitCode , exitCode.GetTypeCode ( ) ) ;
            if ( code != null )
            {
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
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
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