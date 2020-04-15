#region header
// Kay McCormick (mccor)
// 
// LogViewer1
// LogViewer1
// LogListener.cs
// 
// 2020-03-16-1:20 AM
// 
// ---
#endregion
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogListener : IDisposable


    {
        private const string log4jNsPrefix       = "log4j" ;
        private const string nlogNsPrefix        = "nlog" ;
        private const string loggerAttributeName = "logger" ;

        private readonly int          _port ;
        private readonly LogViewModel _viewModel ;

        private readonly string[] levels =
        {
            "TRACE" , "DEBUG" , "INFO" , "WARN" , "ERROR" , "FATAL"
        } ;

        private readonly JsonSerializerOptions _options ;
        private UdpClient             _udpClient ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="viewModel"></param>
        /// <param name="options"></param>
        // ReSharper disable once UnusedMember.Global
        public LogListener ( int port , LogViewModel viewModel , JsonSerializerOptions options )
        {
            _port      = port ;
            _viewModel = viewModel ;
            _options = options ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="logViewModel"></param>
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once UnusedParameter.Local
        public LogListener ( int port , LogViewModel logViewModel ) { }

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        public void Dispose ( ) { _udpClient?.Dispose ( ) ; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Start ( )
        {
            try
            {
                _udpClient = new UdpClient ( new IPEndPoint ( IPAddress.Any , _port ) ) ;
            }
            catch ( SocketException ex )
            {
                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
            }

            DebugUtils.WriteLine ( "Listening on port " + _port ) ;
            Listen ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once FunctionRecursiveOnAllPaths
        public async void Listen ( )
        {
            var resp = await _udpClient.ReceiveAsync ( ).ConfigureAwait ( false ) ;

            PacketReceived ( resp ) ;
            Listen ( ) ;
        }

        [ NotNull ]
        private LogEventInstance HandleJsonMessage ( JsonSerializerOptions options , string s )
        {
            var instance = JsonSerializer.Deserialize < LogEventInstance > ( s , options ) ;
            if ( instance == null )
            {
                throw new InvalidOperationException ( ) ;
            }

            instance.SerializedForm = s ;
            return instance ;

        }

        private void PacketReceived ( UdpReceiveResult resp )
        {
            var resultBuffer = resp.Buffer ;
            DebugUtils.WriteLine ( "received packet" ) ;
            var s = Encoding.UTF8.GetString ( resultBuffer ) ;
            try
            {
                if ( s[ 0 ] == '{' )
                {
                    var instance = HandleJsonMessage ( _options , s ) ;
                    HandleLogInstance ( instance ) ;
                }
                else if ( s[ 0 ] == '<' )
                {
                    try
                    {
                        var instance = HandleXml ( resultBuffer ) ;
                        HandleLogInstance ( instance ) ;
                    }
                    catch ( XmlException xmlException )
                    {
                        DebugUtils.WriteLine ( xmlException.ToString ( ) ) ;
                    }
                }
            }
            catch ( Exception ex )
            {
                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
            }
        }

        private void HandleLogInstance ( LogEventInstance instance )
        {
            _viewModel.AddEntry ( instance ) ;
        }

        [ NotNull ]
        private LogEventInstance HandleXml ( [ NotNull ] byte[] resultBuffer )
        {
            var xmlNameTable = new NameTable ( ) ;

            xmlNameTable.Add ( log4jNsPrefix ) ;
            var nameTable = new NameTable ( ) ;
            nameTable.Add ( log4jNsPrefix ) ;
            var xmlNamespaceManager = new XmlNamespaceManager ( xmlNameTable ) ;
            xmlNamespaceManager.AddNamespace (
                                              log4jNsPrefix
                                            , "http://kaymccormick.com/xmlns/log4j"
                                             ) ;

            xmlNamespaceManager.AddNamespace (
                                              nlogNsPrefix
                                            , "http://kaymccormick.com/xmlns/nlog"
                                             ) ;
            var xmlParserContext = new XmlParserContext (
                                                         xmlNameTable
                                                       , xmlNamespaceManager
                                                       , "en-US"
                                                       , XmlSpace.Preserve
                                                       , Encoding.UTF8
                                                        ) ;
            LogEventInstance instance ;
            using ( var reader = XmlReader.Create (
                                                   new MemoryStream ( resultBuffer )
                                                 , new XmlReaderSettings
                                                   {
                                                       NameTable = xmlNameTable
                                                   }
                                                 , xmlParserContext
                                                  ) )
            {
                var document = new XmlDocument ( ) ;

                document.Load ( reader ) ;

                instance = new LogEventInstance ( ) ;
                var elem = document.DocumentElement ;
                if ( elem != null )
                {
                    var logger = elem.GetAttribute ( loggerAttributeName ) ;
                    var level = elem.GetAttribute ( "level" ) ;
                    var levelOrdinal = levels.ToList ( ).IndexOf ( level ) ;
                    var timestamp = elem.GetAttribute ( "timestamp" ) ;
                    var dt = JavaTimeStampToDateTime ( long.Parse ( timestamp ) ) ;
                    instance.LoggerName = logger ;
                    instance.Level      = levelOrdinal ;
                    instance.TimeStamp  = dt ;
                }

                if ( elem != null )
                {
                    foreach ( var elemChildNode in elem.ChildNodes )
                    {
                        if ( elemChildNode is XmlElement elem2 )
                        {
                            DebugUtils.WriteLine ( elem2.Name ) ;
                            switch ( elem2.Name )
                            {
                                case "log4j:message" :

                                    instance.FormattedMessage = elem2.InnerText ;
                                    break ;
                                case "log4j:locationInfo" : break ;
                                case "nlog:eventSequenceNumber" :
                                    if ( long.TryParse ( elem2.InnerText , out var seq ) )
                                    {
                                        instance.SequenceID = seq ;
                                    }

                                    break ;
                                case "log4j:properties" :
                                    /*
                                 *     <log4j:data name="methodName" value="OnInitialized" />
        <log4j:data name="typeEvent" value="System.EventArgs" />
        <log4j:data name="log4japp" value="WpfClient1.exe(12712)" />
        <log4j:data name="log4jmachinename" value="EXOMAIL-87976" />
    
                                 * */

                                    foreach ( var xmlElement in elem2
                                                               .ChildNodes.OfType < XmlElement > ( )
                                                               .Where (
                                                                       element => element.Name
                                                                                  == "log4j:data"
                                                                      ) )
                                    {
                                        var name = xmlElement.GetAttribute ( "name" ) ;
                                        var value = xmlElement.GetAttribute ( "value" ) ;
                                        instance.Properties[ name ] = value ;
                                    }

                                    break ;

                                case "nlog:properties" :
                                    foreach ( var xmlElement in elem2
                                                               .ChildNodes.OfType < XmlElement > ( )
                                                               .Where (
                                                                       element => element.Name
                                                                                  == "nlog:data"
                                                                      ) )
                                    {
                                        var name = xmlElement.GetAttribute ( "name" ) ;
                                        var value = xmlElement.GetAttribute ( "value" ) ;
                                        instance.Properties[ name ] = value ;
                                    }

                                    break ;
                            }
                        }
                    }
                }
            }

            instance.SerializedForm = resultBuffer ;
            return instance ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="javaTimeStamp"></param>
        /// <returns></returns>
        public static DateTime JavaTimeStampToDateTime ( double javaTimeStamp )
        {
            // Java timestamp is milliseconds past epoch
            var dtDateTime = new DateTime (
                                           1970
                                         , 1
                                         , 1
                                         , 0
                                         , 0
                                         , 0
                                         , 0
                                         , DateTimeKind.Utc
                                          ) ;
            dtDateTime = dtDateTime.AddMilliseconds ( javaTimeStamp ).ToLocalTime ( ) ;
            return dtDateTime ;
        }
    }
}