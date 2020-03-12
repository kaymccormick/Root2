#region header
// Kay McCormick (mccor)
// 
// ConfigTest
// KayMcCormick.Dev
// AppLoggingConfiguration.cs
// 
// 2020-03-09-7:59 PM
// 
// ---
#endregion
using JetBrains.Annotations;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public class AppLoggingConfiguration : ILoggingConfiguration
    {
        private static readonly ILoggingConfiguration _default =
            new AppLoggingConfiguration { IsEnabledConsoleTarget = false , MinLogLevel = LogLevel.Info } ;

        private bool? _isEnabledEventLogTarget = false ;
        private bool? _isEnabledCacheTarget ;
        private LogLevel _minLogLevel ;
        private string _debuggerTargetName ;

        #region Implementation of ILoggingConfiguration
        /// <summary>
        /// 
        /// </summary>
        public bool? IsEnabledConsoleTarget { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public bool? IsEnabledEventLogTarget { get => _isEnabledEventLogTarget ;
            set => _isEnabledEventLogTarget = value ;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsEnabledCacheTarget => _isEnabledCacheTarget;
        public bool? IsEnabledDebuggerTarget { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public string DebuggerTargetName => _debuggerTargetName ;

        /// <summary>
        /// 
        /// </summary>
        [ CanBeNull ] public LogLevel MinLogLevel { get => _minLogLevel ; set => _minLogLevel = value ; }

        /// <summary>
        /// 
        /// </summary>
        public static ILoggingConfiguration Default => _default ;
        #endregion
    }
}