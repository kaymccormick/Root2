using System ;
using System.Collections.Generic ;
using Autofac.Core ;

namespace KayMcCormick.Dev
{
    public class MyInfo3
    {
        private List<IComponentRegistration> _registrations = new List < IComponentRegistration > ();
        private ISet<Guid>                   _ids           = new HashSet < Guid > ();

        public Type               LimitType { get ; set ; }
        public IComponentLifetime Lifetime  { get ; set ; }

        public List<IComponentRegistration> Registrations { get { return _registrations ; } set { _registrations = value ; } }

        public ISet<Guid> Ids { get { return _ids ; } set { _ids = value ; } }
    }
}