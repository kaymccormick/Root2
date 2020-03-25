#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Dev
// ApplicationInstanceConfiguration.cs
// 
// 2020-03-24-9:27 PM
// 
// ---
#endregion
using System.Collections ;
using KayMcCormick.Dev.Logging ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationInstanceConfiguration
    {
        private readonly LogDelegates.LogMethod _logMethod ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMethod"></param>
        /// <param name="configs"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        public ApplicationInstanceConfiguration ( LogDelegates.LogMethod logMethod , IEnumerable configs = null , bool disableLogging = false , bool disableRuntimeConfiguration = false , bool disableServiceHost = false )
        {
            _logMethod                   = logMethod ;
            Configs                     = configs ;
            DisableLogging              = disableLogging ;
            DisableRuntimeConfiguration = disableRuntimeConfiguration ;
            DisableServiceHost          = disableServiceHost ;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogDelegates.LogMethod LogMethod { get { return _logMethod ; } }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Configs { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableLogging { get ; private set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableRuntimeConfiguration { get ; private set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableServiceHost { get ; private set ; }
    }
}