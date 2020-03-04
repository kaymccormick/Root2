﻿using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Windows ;
using Autofac ;
using KayMcCormick.Dev ;
using NLog ;
using static KayMcCormick.Dev.Logging.AppLoggingConfigHelper ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class BaseApp : Application
    {
        private IComponentContext scope ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.</summary>
        /// <exception cref="System.InvalidOperationException">More than one instance of the <see cref="System.Windows.Application" /> class is created per <see cref="System.AppDomain" />.</exception>
        public BaseApp ( )
        {
            EnsureLoggingConfigured ( ) ;
        }

        /// <summary>Gets a value indicating whether [do tracing].</summary>
        /// <value>
        ///     <see language="true"/> if [do tracing]; otherwise, <see language="false"/>.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTracing
        public bool DoTracing { get; } = false;

        protected void SetupTracing()
        {
            PresentationTraceSources.Refresh();
            if (DoTracing)
            {
                var nLogTraceListener = new NLogTraceListener();
                var routedEventSource = PresentationTraceSources.RoutedEventSource;
                nLogTraceListener.DefaultLogLevel = LogLevel.Debug;
                nLogTraceListener.ForceLogLevel   = LogLevel.Warn;
                //nLogTraceListener.LogFactory      = AppContainer.Resolve < LogFactory > ( ) ;
                nLogTraceListener.AutoLoggerName = false;
                //nLogTraceListener.
                routedEventSource.Switch.Level = SourceLevels.All;
                var foo = ResolutionExtensions.Resolve <IEnumerable<TraceListener>>( Scope );
                foreach (var tl in foo)
                {
                    routedEventSource.Listeners.Add(tl);
                }

                //routedEventSource.Listeners.Add ( new AppTraceListener ( ) ) ;
                routedEventSource.Listeners.Add(nLogTraceListener);
            }
        }

        public IComponentContext Scope { get { return scope ; } }

        protected void ErrorExit(ExitCode exitCode = ExitCode.GeneralError)
        {
            var code = Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            if (code != null)
            {
                var intCode = (int)code;

                if (Current == null)
                {
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    Current.Shutdown(intCode);
                }
            }
        }
    }
}
