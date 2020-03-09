using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy ;
using System.ServiceModel ;
using System.Text;
using System.Threading.Tasks;

namespace KayMcCormick.Dev.Interfaces
{
    [ServiceContract]
    public interface ICentralService
    {
        [ OperationContract ]
        RegisterApplicationInstanceResponse RegisterApplicationInstance (
            RegisterApplicationInstanceRequest request
        ) ;
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CentralService : ICentralService
    {
        #region Implementation of ICentralService
        public RegisterApplicationInstanceResponse RegisterApplicationInstance (
            RegisterApplicationInstanceRequest request
        )
        {
            return null ;
        }
        #endregion
    }

    public class RegisterApplicationInstanceRequest
    {
        public Url EndpointUrl { get ; set ; }

        public RegisterApplicationInstanceRequest ( )
        {
        }
    }

    public class RegisterApplicationInstanceResponse
    {
    }
}
