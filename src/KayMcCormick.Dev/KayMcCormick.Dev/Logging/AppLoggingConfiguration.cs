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

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public class AppLoggingConfiguration : ILoggingConfiguration
    {
        private static ILoggingConfiguration _default =
            new AppLoggingConfiguration { IsEnabledConsoleTarget = false , } ;

        private bool _isEnabledEventLogTarget = false ;
        private bool _isEnabledCacheTarget ;

        #region Implementation of ILoggingConfiguration
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabledConsoleTarget { get; set; } 

        public bool IsEnabledEventLogTarget => _isEnabledEventLogTarget ;

        public bool IsEnabledCacheTarget => _isEnabledCacheTarget ;

        public static ILoggingConfiguration Default { get { return _default ; } }
        #endregion
    }
}