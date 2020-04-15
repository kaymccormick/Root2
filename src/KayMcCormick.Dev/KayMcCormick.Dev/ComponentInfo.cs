using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    /// <summary>Container for registered instances with the IOC container.</summary>
    public sealed class ComponentInfo
    {
        private IDictionary < string , object > _metadata ;

        /// <summary>Gets the instances.</summary>
        /// <value>The instances.</value>
        internal IList < InstanceInfo > Instances { get ; } = new List < InstanceInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary < string , object > Metadata { get { return _metadata ; } set { _metadata = value ; } }
    }
}