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
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public sealed class AppLoggingConfiguration : ILoggingConfiguration
    {
        public void Trace()
        {
            MinLogLevel = LogLevel.Trace;
            
        }
        private static readonly ILoggingConfiguration _default =
            new AppLoggingConfiguration
            {
                IsEnabledConsoleTarget  = false
              , MinLogLevel             = LogLevel.Info
              , IsEnabledDebuggerTarget = true
            } ;

        private int ?  _chainsawPort ;
#pragma warning disable 649
        private string _debuggerTargetName ;
#pragma warning restore 649

#pragma warning disable 649
        private bool ? _isEnabledCacheTarget ;
#pragma warning restore 649
        private bool ? _isEnabledDebuggerTarget ;

        private bool ? _isEnabledEventLogTarget = false ;
        private bool ? _logThrowExceptions ;

        private LogLevel _minLogLevel ;
        private int ?    _nLogViewerPort ;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return
                $"{nameof ( IsEnabledConsoleTarget )}: {IsEnabledConsoleTarget}, {nameof ( IsEnabledEventLogTarget )}: {IsEnabledEventLogTarget}, {nameof ( IsEnabledCacheTarget )}: {IsEnabledCacheTarget}, {nameof ( IsEnabledDebuggerTarget )}: {IsEnabledDebuggerTarget}, {nameof ( DebuggerTargetName )}: {DebuggerTargetName}, {nameof ( NLogViewerPort )}: {NLogViewerPort}, {nameof ( ChainsawPort )}: {ChainsawPort}, {nameof ( MinLogLevel )}: {MinLogLevel}" ;
        }

        #region Implementation of ILoggingConfiguration
        /// <summary>
        /// </summary>
        public bool ? IsEnabledConsoleTarget { get ; set ; }

        /// <summary>
        /// </summary>
        public bool ? IsEnabledEventLogTarget
        {
            get { return _isEnabledEventLogTarget ; }
            set { _isEnabledEventLogTarget = value ; }
        }

        /// <summary>
        /// </summary>
        public bool ? IsEnabledCacheTarget
        {
            get { return _isEnabledCacheTarget ; }
            set { _isEnabledCacheTarget = value ; }
        }

        /// <summary>Gets or sets the is enabled debugger target.</summary>
        /// <value>The is enabled debugger target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for IsEnabledDebuggerTarget
        public bool ? IsEnabledDebuggerTarget
        {
            get { return _isEnabledDebuggerTarget ; }
            set { _isEnabledDebuggerTarget = value ; }
        }

        /// <summary>
        /// </summary>
        public string DebuggerTargetName { get { return _debuggerTargetName ; } }

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
        ///     Enavle the xml File target
        /// </summary>
        public bool IsEnabledXmlFileTarget { get ; set ; }

        /// <summary>
        /// </summary>
        public LogLevel MinLogLevel { get { return _minLogLevel ; } set { _minLogLevel = value ; } }

        /// <summary>
        /// </summary>
        public static ILoggingConfiguration Default { get { return _default ; } }
        #endregion
    }
}