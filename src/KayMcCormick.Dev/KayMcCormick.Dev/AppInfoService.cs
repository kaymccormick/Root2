using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.ServiceModel ;
using System.Text ;
using System.Threading.Tasks ;
using NLog ;
using NLog.Targets ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [ ServiceContract ]
    
    public interface IAppInfoService

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ OperationContract ]
        AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AppInfoService : IAppInfoService
    {
        private DateTime _startupTime ;

        public AppInfoService ( DateTime startupTime ) { _startupTime = startupTime ; }

        #region Implementation of IAppInfoService
        public AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request )
        {
            var appInstanceInfo = new AppInstanceInfo ( ) { StartupTime = _startupTime } ;

            foreach (var target in NLog.LogManager.Configuration.AllTargets)
            {
                var loginfo = new LoggerInfo ( ) { TargetName = target.Name } ;
                appInstanceInfo.LoggerInfos.Add ( loginfo ) ;
            }

            return new AppInstanceInfoResponse ( )
                   {
                       Info = appInstanceInfo
                   } ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfoResponse
    {
        public AppInstanceInfo Info { get ; set ; }
    }

    public class LoggerInfo
    {
        public string TargetName { get ; set ; }
        public FileTarget FileTarget { get ; set ; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfo
    {
        public DateTime StartupTime { get ; set ; }
        public IList<LoggerInfo> LoggerInfos { get; set; } = new List<LoggerInfo>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfoRequest
    {
    }
}
