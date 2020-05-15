using System ;
using System.Linq ;
using System.ServiceModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;
using NLog ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
#if NETFRAMEWORK
    [ ServiceBehavior ( InstanceContextMode = InstanceContextMode.Single ) ]
#endif
    internal class AppInfoService : IAppInfoService
    {
        private readonly IObjectIdProvider _objectId ;
        private readonly DateTime          _startupTime ;

        /// <summary>
        /// </summary>
        /// <param name="startupTime"></param>
        /// <param name="objectId"></param>
        internal AppInfoService ( DateTime startupTime , IObjectIdProvider objectId )
        {
            _startupTime = startupTime ;
            _objectId    = objectId ;
        }

        #region Implementation of IAppInfoService
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ NotNull ]
        public AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request )
        {
            var appInstanceInfo = new AppInstanceInfo { StartupTime = _startupTime } ;

            foreach ( var logInfo in LogManager.Configuration.AllTargets.Select ( target => new LoggerInfo { TargetName = target.Name } ) )
            {
                appInstanceInfo.LoggerInfos.Add ( logInfo ) ;
            }


            if ( _objectId == null )
            {
                return new AppInstanceInfoResponse { Info = appInstanceInfo } ;
            }

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

            return new AppInstanceInfoResponse { Info = appInstanceInfo } ;
        }
        #endregion
    }
}