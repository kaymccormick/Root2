using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    /// <summary>Container for registered instances with the IOC container.</summary>
    public sealed class ComponentInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key => "Error" ;
        private IDictionary < string , object > _metadata ;
        private IEnumerable < InstanceInfo > _instanceEnumeration ;
        private Guid _id ;

        /// <summary>Gets the instances.</summary>
        /// <value>The instances.</value>
        internal IList < InstanceInfo > InstancesList { get ; } = new List < InstanceInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < InstanceInfo> Instances => _instanceEnumeration ?? InstancesList ;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary < string , object > Metadata { get { return _metadata ; } set { _metadata = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < InstanceInfo > InstanceEnumeration { get { return _instanceEnumeration ; } set { _instanceEnumeration = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get { return _id ; } set { _id = value ; } }
    }
}