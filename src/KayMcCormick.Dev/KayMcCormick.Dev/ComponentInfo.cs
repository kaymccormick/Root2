using System ;
using System.Collections.Generic ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>Container for registered instances with the IOC container.</summary>

    public sealed class ComponentInfo
    {
        public ComponentInfo()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        [ NotNull ] public string Key
        {
            get { return "Error" ; }
        }

        private IDictionary < string , object > _metadata ;
        private IEnumerable < InstanceInfo > _instanceEnumeration ;
        private Guid _id ;

        /// <summary>Gets the instances.</summary>
        /// <value>The instances.</value>
        internal IList < InstanceInfo > InstancesList { get ; } = new List < InstanceInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < InstanceInfo> Instances
        {
            get { return _instanceEnumeration ?? InstancesList ; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
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