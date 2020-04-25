using System ;
using System.Collections.Generic ;
using Autofac.Core ;

namespace KayMcCormick.Dev.Metadata
{
    public class MyIonfo
    {
        private List < IComponentRegistration > _registrations =
            new List < IComponentRegistration > ( ) ;

        private Guid _id ;

        public List < IComponentRegistration > Registrations
        {
            get { return _registrations ; }
            set { _registrations = value ; }
        }

        public Guid Id { get { return _id ; } set { _id = value ; } }
    }
}