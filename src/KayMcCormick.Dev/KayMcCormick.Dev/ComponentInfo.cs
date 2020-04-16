using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    /// <summary>Container for registered instances with the IOC container.</summary>
    public sealed class ComponentInfo
    {
        private IDictionary < string , object > _metadata ;
        private IEnumerable < InstanceInfo > _instanceEnumeration ;
        private Guid _id ;

        /// <summary>Gets the instances.</summary>
        /// <value>The instances.</value>
        internal IList < InstanceInfo > Instances { get ; } = new List < InstanceInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary < string , object > Metadata { get { return _metadata ; } set { _metadata = value ; } }

        public IEnumerable < InstanceInfo > InstanceEnumeration { get { return _instanceEnumeration ; } set { _instanceEnumeration = value ; } }

        public Guid Id { get { return _id ; } set { _id = value ; } }
    }
}