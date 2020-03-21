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

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///
    public class LogListener
    {
        private string[] levels =
            new[] { "TRACE" , "DEBUG" , "INFO" , "WARN" , "ERROR" , "FATAL" } ;
        private readonly int                   _port ;
        private readonly LogViewModel          _viewModel ;
        private          int                   udpPort = 4446 ;
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

        private void HandleJson ( JsonSerializerOptions options , string s )
        {
            var i = JsonSerializer.Deserialize < LogEventInstance > ( s , options ) ;
            i.SerializedForm = s ;
            _viewModel.AddEntry(i);
        }

        private void PacketReceived ( UdpReceiveResult resp )
        {
            var resultBuffer = resp.Buffer ;
            Debug.WriteLine ( "received" ) ;
            var s = Encoding.UTF8.GetString ( resultBuffer ) ;
            try
            {
                if ( s[ 0 ] == '{' )
                {
                    HandleJson ( _options , s ) ;
                    return ;
                }
                else if ( s[ 0 ] == '<' )
                {
                    try
                    {
                        HandleXml ( resultBuffer ) ;
                    }
                    catch ( XmlException xmlException )
                    {
                    }
                }
            }
            catch ( Exception ex )
            {
            }
        }

        private void HandleXml ( byte[] resultBuffer )
        {
            var xmlNameTable = new NameTable ( ) ;

            xmlNameTable.Add ( "log4j" ) ;
            var nameTable = new NameTable ( ) ;
            nameTable.Add ( "log4j" ) ;
            var xmlNamespaceManager = new XmlNamespaceManager ( xmlNameTable ) ;
            xmlNamespaceManager.AddNamespace ( "log4j" , "http://kaymccormick.com/xmlns/log4j" ) ;

            xmlNamespaceManager.AddNamespace ( "nlog" , "http://kaymccormick.com/xmlns/nlog" ) ;
            var xmlParserContext = new XmlParserContext (
                                                         xmlNameTable
                                                       , xmlNamespaceManager
                                                       , "en-US"
                                                       , XmlSpace.Preserve
                                                       , Encoding.UTF8
                                                        ) ;
            var reader = XmlReader.Create (
                                           new MemoryStream ( resultBuffer )
                                         , new XmlReaderSettings ( ) { NameTable = xmlNameTable }
                                         , xmlParserContext
                                          ) ;

            var document = new XmlDocument ( ) ;
            document.Load ( reader ) ;
            var instance = new LogEventInstance ( ) ;
            var elem = document.DocumentElement ;
            var logger = elem.GetAttribute ( "logger" ) ;
            var level = elem.GetAttribute ( "level" ) ;
            var levelOrdinal = levels.ToList ( ).IndexOf ( level ) ;
            var timestamp = elem.GetAttribute ( "timestamp" ) ;
            var dt = JavaTimeStampToDateTime ( long.Parse ( timestamp ) ) ;
            instance.LoggerName = logger ;
            instance.Level = levelOrdinal ;
            instance.TimeStamp  = dt ;
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
                        case "log4j:locationInfo" :        break ;
                        case "nlog:eventSequenceNumber" : 
                            if(long.TryParse(elem2.InnerText, out var seq))
                            {
                                instance.SequenceID = seq ;
                            }
                            break ;
                        case "log4j:properties":
                            /*
                             *     <log4j:data name="methodName" value="OnInitialized" />
    <log4j:data name="typeEvent" value="System.EventArgs" />
    <log4j:data name="log4japp" value="WpfClient1.exe(12712)" />
    <log4j:data name="log4jmachinename" value="EXOMAIL-87976" />

                             * */
                            
                            foreach (var xmlElement in elem2
                                                      .ChildNodes.OfType<XmlElement>()
                                                      .Where(
                                                             element => element.Name
                                                                        == "log4j:data"
                                                            ))
                            {
                                var name = xmlElement.GetAttribute("name");
                                var value = xmlElement.GetAttribute("value");
                                instance.Properties[name] = value;
                            }
                            break;
                        
                        case "nlog:properties":
                            foreach ( var xmlElement in elem2
                                                       .ChildNodes.OfType < XmlElement > ( )
                                                       .Where (
                                                               element => element.Name
                                                                          == "nlog:data"
                                                              ) )
                            {
                                var name =  xmlElement.GetAttribute ( "name" ) ;
                                var value = xmlElement.GetAttribute ( "value" ) ;
                                instance.Properties[ name ] = value ;
                            }
                            break;
                    }

                  
                }
            }

            instance.SerializedForm = resultBuffer ;
            _viewModel.AddEntry(instance);
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
    }
}