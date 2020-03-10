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

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoggingConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ] bool IsEnabledConsoleTarget { get ; }
    }
}