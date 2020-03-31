using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    /// <summary>Container for registered instances with the IOC container.</summary>
    internal sealed class ComponentInfo
    {
        /// <summary>Gets the instances.</summary>
        /// <value>The instances.</value>
        internal IList < InstanceInfo > Instances { get ; } = new List < InstanceInfo > ( ) ;
    }
}