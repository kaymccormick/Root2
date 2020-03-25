﻿using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Windows ;
using Autofac ;
using Autofac.Core ;
#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.Tracing ;
using NLog ;
using NLog.Targets ;
using Application = System.Windows.Application ;

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
    /// Interaction logic for App.xaml
    /// </summary>
    public abstract class BaseApp : Application , IDisposable
    {
        private readonly bool _disableLogging ;

        private readonly ApplicationInstanceBase _applicationInstance ;
        private readonly EventLog                _eventLog ;
        private readonly ApplicationInstanceBase _createdAppInstance ;
        private          ILifetimeScope          _scope ;
#if COMMANDLINE
        private Type[]                  _optionType ;
        private ParserResult < object > _argParseResult ;
#endif
        protected BaseApp ( ) : this ( null , false , false , false ) { }

        protected BaseApp (
            ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
          , IModule[]               modules = null
        )
        {
            _disableLogging = disableLogging ;
            _eventLog       = new EventLog ( "Application" ) { Source = "Application" } ;
            //            var configs = ApplyConfiguration ( ) ;
            if ( applicationInstance != null )
            {
                _applicationInstance = applicationInstance ;
            }
            else
            {
                _applicationInstance = _createdAppInstance = new ApplicationInstance (
                                                                                      new ApplicationInstanceConfiguration (
                                                                                                                            message => {
                                                                                                                                if (
                                                                                                                                    _eventLog
                                                                                                                                    != null
                                                                                                                                )
                                                                                                                                {
                                                                                                                                    PROVIDER_GUID.EventWriteSETUP_LOGGING_EVENT( 
                                                                                                                                                                                message)
                                                                                                                                        ;
                                                                                                                                    _eventLog
                                                                                                                                       .WriteEntry (
                                                                                                                                                    message
                                                                                                                                                  , EventLogEntryType
                                                                                                                                                       .Information
                                                                                                                                                   ) ;
                                                                                                                                }
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

            _applicationInstance.Initialize ( ) ;
            _applicationInstance.Startup ( ) ;
            _scope = _applicationInstance.GetLifetimeScope ( ) ;

            foreach ( var myJsonLayout in LogManager
                                         .Configuration.AllTargets.OfType < TargetWithLayout > ( )
                                         .Select ( t => t.Layout )
                                         .OfType < MyJsonLayout > ( ) )
            {
                var options = myJsonLayout.CreateJsonSerializerOptions ( ) ;
                options.Converters.Add ( new DataTemplateKeyConverter ( ) ) ;
                myJsonLayout.Options = options ;
            }
        }

        public virtual ILifetimeScope BeginLifetimeScope(object tag)
        {
            return Scope.BeginLifetimeScope(tag);
        }

        public virtual ILifetimeScope BeginLifetimeScope()
        {
            return Scope.BeginLifetimeScope();
        }

        protected virtual ILifetimeScope Scope { get { return _scope ; } set { _scope = value ; } }

        /// <summary>Gets a value indicating whether [do tracing].</summary>
        /// <value>
        ///     <see language="true"/> if [do tracing]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTracing
        public virtual bool DoTracing { get ; } = false ;

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
                var foo = ResolutionExtensions.Resolve < IEnumerable < TraceListener > > ( Scope ) ;
                foreach ( var tl in foo )
                {
                    routedEventSource.Listeners.Add ( tl ) ;
                }

                //routedEventSource.Listeners.Add ( new AppTraceListener ( ) ) ;
                routedEventSource.Listeners.Add ( nLogTraceListener ) ;
            }
        }

        protected virtual LogDelegates.LogMethod DebugLog { get ; set ; }

        /// <summary>Gets the configuration settings.</summary>
        /// <value>The configuration settings.</value>
        public virtual List < object > ConfigSettings { get ; } = new List < object > ( ) ;

        protected virtual ILogger Logger { get ; set ; }

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

        protected virtual IEnumerable < IModule > GetModules ( )
        {
            return Array.Empty < IModule > ( ) ;
        }

        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            _createdAppInstance?.Dispose ( ) ;
            _eventLog?.Dispose ( ) ;
        }

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
        public virtual void Dispose ( ) { _applicationInstance?.Dispose ( ) ; }
        #endregion
    }
}