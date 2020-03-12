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
        /// 
        /// </summary>
        bool ? IsEnabledEventLogTarget { get ; }

        /// <summary>
        /// 
        /// </summary>
        bool ? IsEnabledCacheTarget { get ; }

        /// <summary>
        /// 
        /// </summary>
        [ CanBeNull ] LogLevel MinLogLevel { get ; }

        /// <summary>
        /// 
        /// </summary>
        bool ? IsEnabledDebuggerTarget { get ; }

        /// <summary>
        /// </summary>
        string DebuggerTargetName { get ; }
    }
}