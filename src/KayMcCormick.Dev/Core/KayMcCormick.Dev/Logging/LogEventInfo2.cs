#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Dev
// LogEventInfo2.cs
// 
// 2020-03-17-12:05 AM
// 
// ---
#endregion
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public class LogEventInfo2 : LogEventInfo
    {
        /// <summary>
        /// </summary>
        public new int SequenceID { get ; set ; }
    }
}