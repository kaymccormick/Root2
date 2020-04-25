using System ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using Autofac.Core ;

namespace KayMcCormick.Dev.Metadata
{
    public class MyInfo2
    {
        public ConcurrentDictionary < IComponentLifetime , MyInfo3 > Lifetimes { get ; } =
            new ConcurrentDictionary < IComponentLifetime , MyInfo3 > ( ) ;

        private List<IComponentRegistration> _registrations = new List < IComponentRegistration > ();
        private Type                         _limitType ;
        public  List<IComponentRegistration> Registrations { get { return _registrations ; } set { _registrations = value ; } }

        public Type LimitType { get { return _limitType ; } set { _limitType = value ; } }
    }
}