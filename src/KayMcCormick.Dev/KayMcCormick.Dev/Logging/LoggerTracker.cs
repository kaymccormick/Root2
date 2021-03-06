using System.Collections.Concurrent ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.AppBuild ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <seealso cref="KayMcCormick.Dev.AppBuild.ILoggerTracker" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerTracker
    public sealed class LoggerTracker : ILoggerTracker
    {
        /// <summary>The logger</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for _Logger
        private static readonly ILogger _Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>The loggers</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for loggers
        private readonly ConcurrentDictionary < string , ILogger > _loggers =
            new ConcurrentDictionary < string , ILogger > ( ) ;

        /// <summary>Gets the logger.</summary>
        /// <value>The logger.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Logger
        public static ILogger Logger { get { return _Logger ; } }

        /// <summary>Tracks the logger.</summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="logger">The logger.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TrackLogger
        public void TrackLogger ( [ JetBrains.Annotations.NotNull ] string loggerName , ILogger logger )
        {
            // race condition
            if ( _loggers.TryGetValue ( loggerName , out _ ) )
            {
                Logger.Debug ( $"logger {loggerName} exists already." ) ;
            }
            else
            {
                _loggers.TryAdd ( loggerName , logger ) ;
                OnLoggerRegistered ( new LoggerEventArgs ( logger ) ) ;
            }
        }

        /// <summary>Occurs when [logger registered].</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for LoggerRegistered
        public event LoggerRegisteredEventHandler LoggerRegistered ;

        /// <summary>Raises the <see cref="LoggerRegistered" /> event.</summary>
        /// <param name="args">
        ///     The <see cref="LoggerEventArgs" /> instance containing
        ///     the event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnLoggerRegistered
        private void OnLoggerRegistered ( LoggerEventArgs args )
        {
            var handler = LoggerRegistered ;
            handler?.Invoke ( this , args ) ;
        }
    }

    /// <summary></summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">
    ///     The <see cref="LoggerEventArgs" /> instance containing
    ///     the event data.
    /// </param>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerRegisteredEventHandler
    public delegate void LoggerRegisteredEventHandler ( object sender , LoggerEventArgs args ) ;
}