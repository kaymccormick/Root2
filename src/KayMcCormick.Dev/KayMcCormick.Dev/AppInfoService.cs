using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.ServiceModel ;
using System.Text ;
using System.Threading.Tasks ;
using KayMcCormick.Dev.Interfaces ;
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
        private readonly IObjectIdProvider _objectId ;

        public AppInfoService ( DateTime startupTime, IObjectIdProvider objectId )
        {
            _startupTime = startupTime ;
            _objectId = objectId ;
        }

        #region Implementation of IAppInfoService
        public AppInstanceInfoResponse GetAppInstanceInfo ( AppInstanceInfoRequest request )
        {
            var appInstanceInfo = new AppInstanceInfo ( ) { StartupTime = _startupTime } ;

            foreach (var target in NLog.LogManager.Configuration.AllTargets)
            {
                var loginfo = new LoggerInfo ( ) { TargetName = target.Name } ;
                appInstanceInfo.LoggerInfos.Add ( loginfo ) ;
            }


            if ( _objectId != null )
            {
                foreach ( var rootNode in _objectId.GetRootNodes ( ) )
                {
                    WireComponentInfo info1 = new WireComponentInfo { Id = rootNode } ;
                    var info = _objectId.GetComponentInfo ( rootNode ) ;
                    foreach ( var infoInstance in info.Instances )
                    {
                        info1.Instances.Add (
                                             new WireInstanceInfo
                                             {
                                                 Desc = infoInstance.Instance.ToString ( )
                                             }
                                            ) ;
                        appInstanceInfo.ComponentInfos.Add ( @info1 ) ;
                    }
                }
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
    public class WireInstanceInfo
    {
        private string _desc ;
        /// <summary>
        /// 
        /// </summary>
        public string Desc { get { return _desc ; } set { _desc = value ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WireComponentInfo
    {
        private List<WireInstanceInfo> _instances = new List < WireInstanceInfo > ();
        private Guid _id ;

        /// <summary>
        /// 
        /// </summary>
        public WireComponentInfo ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        public List<WireInstanceInfo> Instances { get { return _instances ; } }

        public Guid Id { get { return _id ; } set { _id = value ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfoResponse
    {
        public AppInstanceInfo Info { get ; set ; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string TargetName { get ; set ; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfo
    {
        private List<WireComponentInfo> _componentInfos = new List < WireComponentInfo > ();

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartupTime { get ; set ; }
        public IList<LoggerInfo> LoggerInfos { get; set; } = new List<LoggerInfo>();

        public List<WireComponentInfo> ComponentInfos { get { return _componentInfos ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInstanceInfoRequest
    {
    }
}
