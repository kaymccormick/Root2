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
using System.Diagnostics.CodeAnalysis ;
using System.Net ;
using System.Net.Sockets ;
using System.Text ;
using NLog ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    internal class ProtoLogger
    {
        private readonly Func < LogEventInfo , byte[] > _getBytes ;
        private readonly UdpClient                      _udpClient ;
        private readonly IPEndPoint                     _ipEndPoint ;
        private readonly Layout                         _layout ;

        public ProtoLogger ( )
        {
            _udpClient  = AppLoggingConfigHelper.UdpClient ;
            _ipEndPoint = AppLoggingConfigHelper.IpEndPoint ;
            _layout     = AppLoggingConfigHelper.XmlEventLayout ;
            _getBytes   = DefaultGetBytes ;
        }

        private byte[] DefaultGetBytes ( LogEventInfo arg )
        {
            var encoding = Encoding.UTF8 ;
            return encoding.GetBytes ( _layout.Render ( arg ) ) ;
        }

        
        public ProtoLogger ( UdpClient udpClient , IPEndPoint ipEndPoint )
        {
            _udpClient  = udpClient ;
            _ipEndPoint = ipEndPoint ;
            _layout     = AppLoggingConfigHelper.XmlEventLayout ;
            _getBytes   = DefaultGetBytes ;
        }

        public void LogAction ( LogEventInfo info )
        {
            var bytes = _getBytes ( info ) ;
            var nBytes = bytes.Length ;
            _udpClient.Send ( bytes , nBytes , _ipEndPoint ) ;
        }
    }
}