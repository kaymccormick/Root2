using System ;
using System.ServiceModel ;
using KayMcCormick.Dev.Interfaces ;
using NLog ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
#if NETFRAMEWORK
    [ ServiceBehavior ( InstanceContextMode = InstanceContextMode.Single ) ]
#endif
    public class AppInfoService : IAppInfoService
    {
        private readonly IObjectIdProvider _objectId ;
        private readonly DateTime          _startupTime ;

        /// <summary>
        /// </summary>
        /// <param name="startupTime"></param>
        /// <param name="objectId"></param>
        public AppInfoService ( DateTime startupTime , IObjectIdProvider objectId )
        {
            _startupTime = startupTime ;
            _objectId    = objectId ;
        }

        #region Implementation of IAppInfoService
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request )
        {
            var appInstanceInfo = new AppInstanceInfo { StartupTime = _startupTime } ;

            foreach ( var target in LogManager.Configuration.AllTargets )
            {
                var logInfo = new LoggerInfo { TargetName = target.Name } ;
                appInstanceInfo.LoggerInfos.Add ( logInfo ) ;
            }


            if ( _objectId != null )
            {
                foreach ( var rootNode in _objectId.GetRootNodes ( ) )
                {
                    var info1 = new WireComponentInfo { Id = rootNode } ;
                    var info = _objectId.GetComponentInfo ( rootNode ) ;
                    foreach ( var infoInstance in info.Instances )
                    {
                        info1.Instances.Add (
                                             new WireInstanceInfo
                                             {
                                                 Desc = infoInstance.Instance.ToString ( )
                                             }
                                            ) ;
                        appInstanceInfo.ComponentInfos.Add ( info1 ) ;
                    }
                }
            }

            return new AppInstanceInfoResponse { Info = appInstanceInfo } ;
        }
        #endregion
    }
}