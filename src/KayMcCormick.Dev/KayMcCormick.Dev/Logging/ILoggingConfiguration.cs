#region header
// Kay McCormick (mccor)
// 
// ConfigTest
// KayMcCormick.Dev
// ILoggingConfiguration.cs
// 
// 2020-03-09-7:58 PM
// 
// ---
#endregion
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public interface ILoggingConfiguration
    {
        /// <summary>
        /// </summary>
        bool ? IsEnabledConsoleTarget { get ; }

        /// <summary>
        /// </summary>
        bool ? IsEnabledEventLogTarget { get ; }

        /// <summary>
        /// </summary>
        bool ? IsEnabledCacheTarget { get ; set ; }

        /// <summary>
        /// </summary>
        [ CanBeNull ] LogLevel MinLogLevel { get ; set ; }

        /// <summary>
        /// </summary>
        bool ? IsEnabledDebuggerTarget { get ; }

        /// <summary>
        /// </summary>
        string DebuggerTargetName { get ; }

        /// <summary>
        ///     NLog viewer port
        /// </summary>
        int ? NLogViewerPort { get ; }

        /// <summary>
        ///     Chainsaw port
        /// </summary>
        int ? ChainsawPort { get ; }

        /// <summary>
        /// </summary>
        bool ? LogThrowExceptions { get ; }

        /// <summary>
        ///     Enable XML File target
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        bool IsEnabledXmlFileTarget { get ; set ; }
    }
}