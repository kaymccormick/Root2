using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
    public sealed class WireComponentInfo
    {
        private readonly List < WireInstanceInfo > _instances = new List < WireInstanceInfo > ( ) ;

        /// <summary>
        /// </summary>
        public List < WireInstanceInfo > Instances { get { return _instances ; } }

        /// <summary>
        /// </summary>
        public Guid Id { get ; set ; }
    }
}