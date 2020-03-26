using JetBrains.Annotations;
using KayMcCormick.Dev.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]

    public interface IAppInfoService

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        AppInstanceInfoResponse GetAppInstanceInfo(AppInstanceInfoRequest request);
    }

    /// <summary>
    /// 
    /// </summary>
    #if NETFRAMEWORK
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
#endif
    public class AppInfoService : IAppInfoService
    {
        private readonly DateTime _startupTime;
        private readonly IObjectIdProvider _objectId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startupTime"></param>
        /// <param name="objectId"></param>
        public AppInfoService(DateTime startupTime, IObjectIdProvider objectId)
        {
            _startupTime = startupTime;
            _objectId = objectId;
        }

        #region Implementation of IAppInfoService
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AppInstanceInfoResponse GetAppInstanceInfo(AppInstanceInfoRequest request)
        {
            var appInstanceInfo = new AppInstanceInfo() { StartupTime = _startupTime };

            foreach (var target in LogManager.Configuration.AllTargets)
            {
                var logInfo = new LoggerInfo() { TargetName = target.Name };
                appInstanceInfo.LoggerInfos.Add(logInfo);
            }


            if (_objectId != null)
            {
                foreach (var rootNode in _objectId.GetRootNodes())
                {
                    WireComponentInfo info1 = new WireComponentInfo { Id = rootNode };
                    var info = _objectId.GetComponentInfo(rootNode);
                    foreach (var infoInstance in info.Instances)
                    {
                        info1.Instances.Add(
                                             new WireInstanceInfo
                                             {
                                                 Desc = infoInstance.Instance.ToString()
                                             }
                                            );
                        appInstanceInfo.ComponentInfos.Add(info1);
                    }
                }
            }

            return new AppInstanceInfoResponse()
            {
                Info = appInstanceInfo
            };
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class WireInstanceInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WireComponentInfo
    {
        private readonly List<WireInstanceInfo> _instances = new List<WireInstanceInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<WireInstanceInfo> Instances => _instances;

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfoResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public AppInstanceInfo Info { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string TargetName { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfo
    {
        private readonly List<WireComponentInfo> _componentInfos = new List<WireComponentInfo>();

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartupTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
         public IList<LoggerInfo> LoggerInfos { get; } = new List<LoggerInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<WireComponentInfo> ComponentInfos => _componentInfos;
    }

    /// <summary>
    /// 
    /// </summary>
    
    public class AppInstanceInfoRequest
    {
    }
}
