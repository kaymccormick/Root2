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
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RegisterApplicationInstanceRequest
    {
        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public Url EndpointUrl { get ; set ; }
    }
}