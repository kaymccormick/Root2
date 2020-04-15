using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
    public sealed class AppInstanceInfo
    {
        private readonly List < WireComponentInfo > _componentInfos =
            new List < WireComponentInfo > ( ) ;

        /// <summary>
        /// </summary>
        public DateTime StartupTime { get ; set ; }

        /// <summary>
        /// </summary>
        public IList < LoggerInfo > LoggerInfos { get ; } = new List < LoggerInfo > ( ) ;

        /// <summary>
        /// </summary>
        public List < WireComponentInfo > ComponentInfos { get { return _componentInfos ; } }
    }
}