using System ;
using System.Collections.Generic ;
using Autofac.Core ;

namespace ProjInterface
{
    internal class RegSource : IRegistrationSource
    {
        private bool _isAdapterForIndividualComponents ;
        #region Implementation of IRegistrationSource
        public IEnumerable < IComponentRegistration > RegistrationsFor (
            Service                                                   service
          , Func < Service , IEnumerable < IComponentRegistration > > registrationAccessor
        )
        {
            var swt = service as IServiceWithType ;
            return null ;
        }

        public bool IsAdapterForIndividualComponents
        {
            get { return _isAdapterForIndividualComponents ; }
            set { _isAdapterForIndividualComponents = value ; }
        }
        #endregion
    }
}