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
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public class AppLoggingConfiguration : ILoggingConfiguration
    {
        #region Implementation of ILoggingConfiguration
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabledConsoleTarget { get ; set ; }
        #endregion
    }
}