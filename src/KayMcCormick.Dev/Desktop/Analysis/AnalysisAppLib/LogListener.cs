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
using System ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Net.Sockets ;
using System.Text ;
using System.Text.Json ;
using System.Xml ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
#pragma warning disable DV2002 // Unmapped types
    public class LogListener : IDisposable
#pragma warning restore DV2002 // Unmapped types
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private const string log4jNsPrefix = "log4j" ;
        private const string nlogNsPrefix  = "nlog" ;
        private const string loggerAttributeName = "logger";
        private readonly string[] levels =
            new[] { "TRACE" , "DEBUG" , "INFO" , "WARN" , "ERROR" , "FATAL" } ;

        private readonly int                   _port ;
        private readonly LogViewModel          _viewModel ;
        private          UdpClient             _udpClient ;
        private          JsonSerializerOptions _options ;

        public LogListener ( int port , LogViewModel viewModel )
        {
            _port      = port ;
            _viewModel = viewModel ;
        }

        public void Start ( )
        {
            try
            {
                _udpClient = new UdpClient ( new IPEndPoint ( IPAddress.Any , _port ) ) ;
            }
            catch ( SocketException ex )
            {
                Debug.WriteLine ( ex.ToString ( ) ) ;
            }

            Debug.WriteLine ( "Listening on port " + _port ) ;
            Listen ( ) ;
        }

        private async void Listen ( )
        {
            var resp = await _udpClient.ReceiveAsync ( ).ConfigureAwait ( false ) ;

            PacketReceived ( resp ) ;
            Listen ( ) ;
        }

        private LogEventInstance HandleJsonMessage ( JsonSerializerOptions options , string s )
        {
            var instance = JsonSerializer.Deserialize < LogEventInstance > ( s , options ) ;
            if ( instance != null )
            {
                instance.SerializedForm = s ;
                return instance ;
            }

            throw new InvalidOperationException ( ) ;
        }

        private void PacketReceived ( UdpReceiveResult resp )
        {
            var resultBuffer = resp.Buffer ;
            Debug.WriteLine ( "received packet" ) ;
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
                        Debug.WriteLine ( xmlException.ToString ( ) ) ;
                    }
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex.ToString ( ) ) ;
            }
        }

        private void HandleLogInstance ( LogEventInstance instance )
        {
            _viewModel.AddEntry ( instance ) ;
        }

        private LogEventInstance HandleXml ( byte[] resultBuffer )
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
                                                 , new XmlReaderSettings ( )
                                                   {
                                                       NameTable = xmlNameTable
                                                   }
                                                 , xmlParserContext
                                                  ) )
            {
#pragma warning disable CA3075 // Insecure DTD processing in XML
                var document = new XmlDocument ( ) ;
#pragma warning restore CA3075 // Insecure DTD processing in XML
                document.Load ( reader ) ;

                instance = new LogEventInstance ( ) ;
                var elem = document.DocumentElement ;
                if ( elem != null ) {
                    var logger = elem.GetAttribute (loggerAttributeName) ;
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
                            Debug.WriteLine ( elem2.Name ) ;
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

        #region IDisposable
        public void Dispose ( ) { _udpClient?.Dispose ( ) ; }
        #endregion
    }
}