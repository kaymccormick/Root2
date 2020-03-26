#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// LoggerEventArgs.cs
// 
// 2020-03-12-1:41 AM
// 
// ---
#endregion
using System ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <seealso cref="System.EventArgs" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerEventArgs
    public class LoggerEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.EventArgs" />
        ///     class.
        /// </summary>
        public LoggerEventArgs(ILogger logger) { Logger = logger; }

        /// <summary>Gets the logger.</summary>
        /// <value>The logger.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Logger
        
        public ILogger Logger { get; }
    }
}