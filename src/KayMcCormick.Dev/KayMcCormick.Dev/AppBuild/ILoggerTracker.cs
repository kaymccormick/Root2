#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// ILoggerTracker.cs
// 
// 2020-03-12-1:41 AM
// 
// ---
#endregion
using KayMcCormick.Dev.Logging ;
using NLog ;

namespace KayMcCormick.Dev.AppBuild
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ILoggerTracker
    public interface ILoggerTracker
    {
        /// <summary>Tracks the logger.</summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="logger">The logger.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TrackLogger
        void TrackLogger ( string loggerName , ILogger logger ) ;

        /// <summary>Occurs when [logger registered].</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for LoggerRegistered
        // ReSharper disable once EventNeverSubscribedTo.Global
        event LoggerRegisteredEventHandler LoggerRegistered ;
    }
}