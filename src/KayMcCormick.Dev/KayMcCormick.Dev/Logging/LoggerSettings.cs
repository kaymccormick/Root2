﻿#region header
// Kay McCormick (mccor)
// 
// WpfApp
// WpfApp
// LoggingSection.cs
// 
// 2020-02-07-2:57 PM
// 
// ---
#endregion
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerSettings
    public class LoggerSettings : ILoggingConfiguration
    {
        private int ?    _chainsawPort ;
        private string   _debuggerTargetName ;
        private bool ?   _isEnabledCacheTarget ;
        private bool ?   _isEnabledConsoleTarget ;
        private bool ?   _isEnabledDebuggerTarget ;
        private bool ?   _isEnabledEventLogTarget ;
        private bool ?   _logThrowExceptions ;
        private LogLevel _minLogLevel ;
        private int ?    _nLogViewerPort ;
        #region Implementation of ILoggingConfiguration
        /// <summary>Gets or sets the is enabled console target.</summary>
        /// <value>The is enabled console target.</value>
        public bool ? IsEnabledConsoleTarget
        {
            get { return _isEnabledConsoleTarget ; }
            set { _isEnabledConsoleTarget = value ; }
        }

        /// <summary>Gets or sets the is enabled event log target.</summary>
        /// <value>The is enabled event log target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for IsEnabledEventLogTarget
        public bool ? IsEnabledEventLogTarget
        {
            get { return _isEnabledEventLogTarget ; }
            set { _isEnabledEventLogTarget = value ; }
        }

        /// <summary>Gets or sets the is enabled cache target.</summary>
        /// <value>The is enabled cache target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for IsEnabledCacheTarget
        public bool ? IsEnabledCacheTarget
        {
            get { return _isEnabledCacheTarget ; }
            set { _isEnabledCacheTarget = value ; }
        }

        /// <summary>Gets or sets the minimum log level.</summary>
        /// <value>The minimum log level.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for MinLogLevel
        public LogLevel MinLogLevel { get { return _minLogLevel ; } set { _minLogLevel = value ; } }

        /// <summary>Gets or sets the is enabled debugger target.</summary>
        /// <value>The is enabled debugger target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for IsEnabledDebuggerTarget
        public bool ? IsEnabledDebuggerTarget
        {
            get { return _isEnabledDebuggerTarget ; }
            set { _isEnabledDebuggerTarget = value ; }
        }

        /// <summary>Gets or sets the name of the debugger target.</summary>
        /// <value>The name of the debugger target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebuggerTargetName
        public string DebuggerTargetName
        {
            get { return _debuggerTargetName ; }
            set { _debuggerTargetName = value ; }
        }

        /// <summary>
        /// </summary>
        public int ? NLogViewerPort
        {
            get { return _nLogViewerPort ; }
            set { _nLogViewerPort = value ; }
        }

        /// <summary>
        /// </summary>
        public int ? ChainsawPort { get { return _chainsawPort ; } set { _chainsawPort = value ; } }

        /// <summary>
        /// </summary>
        public bool ? LogThrowExceptions
        {
            get { return _logThrowExceptions ; }
            set { _logThrowExceptions = value ; }
        }

        /// <summary>
        ///     Config variable to enavble the XML File target.
        /// </summary>
        public bool IsEnabledXmlFileTarget { get ; set ; }
        #endregion
    }
}