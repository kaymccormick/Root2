﻿using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Windows ;
using Autofac ;
using Autofac.Core ;
#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif
using KayMcCormick.Dev ;
using NLog ;
using static KayMcCormick.Dev.Logging.AppLoggingConfigHelper ;

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
    public abstract class BaseApp : Application, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private IComponentContext scope ;
        private ApplicationInstance appInst ;
#if COMMANDLINE
        private Type[]                  _optionType ;
        private ParserResult < object > _argParseResult ;
#endif
        protected BaseApp ( ) {
            appInst = new ApplicationInstance() ;
            EnsureLoggingConfigured();
        }

        /// <summary>Gets a value indicating whether [do tracing].</summary>
        /// <value>
        ///     <see language="true"/> if [do tracing]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTracing
        public bool DoTracing { get ; } = false ;

        protected void SetupTracing ( )
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

        public IComponentContext Scope { get => scope ; set => scope = value ; }

        protected void ErrorExit ( ExitCode exitCode = ExitCode.GeneralError )
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

        public virtual IEnumerable < IModule > GetModules ( ) { return Array.Empty < IModule > ( ) ; }
        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            appInst.Dispose();


        }

        protected override void OnStartup ( StartupEventArgs e )
        {
            foreach ( var module in GetModules ( ) )
            {
                Logger.Debug ( "Adding module {module}" , module ) ;
                appInst.AddModule(module);
            }
            appInst.Initialize ( ) ;
            appInst.Startup();
            Scope = appInst.GetLifetimeScope ( ) ;
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
        public void Dispose ( )
        {
            appInst?.Dispose ( ) ;
        }
        #endregion
    }
}
