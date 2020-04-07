using System.Security.Policy ;
using System.ServiceModel ;

namespace KayMcCormick.Dev.Interfaces
{
    /// <summary>
    /// </summary>
    [ ServiceContract ]
    public interface ICentralService
    {
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ OperationContract ]
        RegisterApplicationInstanceResponse RegisterApplicationInstance (
            RegisterApplicationInstanceRequest request
        ) ;
    }

    /// <summary>
    /// </summary>
    public class RegisterApplicationInstanceRequest
    {
        /// <summary>
        /// </summary>
        public Url EndpointUrl { get ; set ; }
    }
}