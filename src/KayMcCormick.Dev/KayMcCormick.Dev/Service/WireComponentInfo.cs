using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class WireComponentInfo
    {
        private readonly List<WireInstanceInfo> _instances = new List<WireInstanceInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<WireInstanceInfo> Instances => _instances;

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
    }
}