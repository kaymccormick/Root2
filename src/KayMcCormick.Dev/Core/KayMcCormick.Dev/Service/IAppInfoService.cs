using System.ServiceModel ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
    [ ServiceContract ]
    public interface IAppInfoService

    {
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ OperationContract ]
        AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request ) ;
    }
}