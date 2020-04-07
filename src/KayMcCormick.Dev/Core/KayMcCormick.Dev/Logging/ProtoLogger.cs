#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// ProtoLogger.cs
// 
// 2020-03-19-11:58 PM
// 
// ---
#endregion
using System ;
using System.Net ;
using System.Net.Sockets ;
using System.Text ;
using NLog ;
using NLog.LayoutRenderers ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    internal sealed class ProtoLogger
    {
        private readonly Func < LogEventInfo , byte[] > _getBytes ;
        private readonly IPEndPoint                     _ipEndPoint ;
        private readonly Layout                         _layout ;
        private readonly UdpClient                      _udpClient ;

        private static ProtoLogger _instance ;

        private readonly Log4JXmlEventLayoutRenderer _xmlEventLayoutRenderer ;
        

        public Layout XmlEventLayout { get ; }

        public static ProtoLogger Instance
        {
            get
            {
                if ( _instance == null )
                {
                    _instance = new ProtoLogger();
                }

                return _instance ;
            }
        }

        public ProtoLogger ( )
        {
            _xmlEventLayoutRenderer =
                new MyLog4JXmlEventLayoutRenderer();
            XmlEventLayout = new MyLayout(_xmlEventLayoutRenderer);
            _udpClient  = AppLoggingConfigHelper.UdpClient ;
            _ipEndPoint = AppLoggingConfigHelper.IpEndPoint ;
            _layout     = XmlEventLayout ;
            if ( _layout == null )
            {
                throw new InvalidOperationException ( "LAyout is null" ) ;
            }

            _getBytes = DefaultGetBytes ;
        }


        private ProtoLogger ( UdpClient udpClient , IPEndPoint ipEndPoint )
        {
            _udpClient  = udpClient ;
            _ipEndPoint = ipEndPoint ;
            _layout     = AppLoggingConfigHelper.XmlEventLayout ;
            _getBytes   = DefaultGetBytes ;
        }

        private byte[] DefaultGetBytes ( LogEventInfo arg )
        {
            var encoding = Encoding.UTF8 ;
            return encoding.GetBytes ( _layout.Render ( arg ) ) ;
        }

        public void LogAction ( LogEventInfo info )
        {
            var bytes = _getBytes ( info ) ;
            var nBytes = bytes.Length ;
            _udpClient.Send ( bytes , nBytes , _ipEndPoint ) ;
        }

        public static readonly Action < LogEventInfo > _protoLogAction = Instance.LogAction ;

        /// <summary>
        /// </summary>
        public static LogDelegates.LogMethod ProtoLogDelegate { get ; } = ProtoLogMessage ;

        private static void ProtoLogMessage ( string message )
        {
            _protoLogAction (
                             LogEventInfo.Create (
                                                  LogLevel.Warn
                                                , typeof ( AppLoggingConfigHelper ).FullName
                                                , message
                                                 )
                            ) ;
        }
    }
}