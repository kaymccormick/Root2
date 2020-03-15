using System ;
using System.AddIn ;
using System.Collections.Generic ;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.SqlTypes ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Net.Sockets ;
using System.Runtime.CompilerServices ;
using System.Text ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Xml ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;
using NLog ;
using ServiceAddIn1 ;

namespace AddInService1V1
{
    [ AddIn (
                "Service1"
              , Description = "test imp"
              , Publisher   = "Kay McCormick"
              , Version     = "1.0"
            ) ]
    [ UsedImplicitly ]
    public class AddInService1V1 : IService1
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private Thread _thread ;
        #region Implementation of IService1
        public bool Start ( )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured (message => Console.WriteLine(message), new AppLoggingConfiguration(){ChainsawPort = 12333 } ) ;
            Logger.Debug("Here");
            Console.WriteLine ( "in start" ) ;
            _thread = new Thread ( ThreadProc ) ;
            _thread.Start();
            return true ;
        }

        private static async Task Run ( )
        {
            Logger.Warn ( "I am here" ) ;
            var @out = new StreamWriter ( "out.txt" ) ;
            var options = new JsonSerializerOptions ( ) ;
            options.Converters.Add ( new LogEventInfoConverter ( ) ) ;

            var b = new SqlConnectionStringBuilder
                    {
                        DataSource         = @".\sql2017"
                      , InitialCatalog     = "ProjLob"
                      , IntegratedSecurity = true
                    } ;
            var c = b.ConnectionString ;
            var conn = new SqlConnection ( c ) ;
            await conn.OpenAsync ( ) ;

            var ports = new[] { 5110 , 9995 } ;
            var clients = ports.Select ( CreateUdpClient ).ToArray ( ) ;

            var tasks = new Task < UdpReceiveResult >[ clients.Length ] ;
            var continued = new ConfiguredTaskAwaitable < object >[ clients.Length ] ;
            for ( var i1 = 0 ; i1 < clients.Length ; i1 ++ )
            {
                setupTask (
                           tasks
                         , i1
                         , clients
                         , continued
                 ,         @out
                 ,         options
                 ,         conn
                          ) ;
            }

            var waiting = new List < ConfiguredTaskAwaitable < object > > ( ) ;
            for ( ; ; )
            {
                var i2 = Task.WaitAny ( tasks ) ;

                var item = continued[ i2 ] ;
                waiting.Add ( item ) ;

                setupTask (
                           tasks
                         , i2
                         , clients
                         , continued
                 ,         @out
                 ,         options
                 ,         conn
                          ) ;
            }
        }

        private static void setupTask (
            Task < UdpReceiveResult >[]          tasks
          , int                                  i1
          , UdpClient[]                          clients
          , ConfiguredTaskAwaitable < object >[] continued
          , StreamWriter                         @out
          , JsonSerializerOptions                options
          , SqlConnection                        conn
        )
        {
            tasks[ i1 ] = clients[ i1 ].ReceiveAsync ( ) ;
            var continueWith = tasks[ i1 ]
                              .ContinueWith (
                                             task => {
                                                 HandleResult (
                                                               task.Result
                                                             , @out
                                                             , options
                                                             , conn
                                                              ) ;
                                                 return new object ( ) ;
                                             }
                                            )
                              .ConfigureAwait ( true ) ;
            continued[ i1 ] = continueWith ;
        }

        private static void HandleResult (
            UdpReceiveResult      result
          , StreamWriter          @out
          , JsonSerializerOptions options
          , SqlConnection         conn
        )
        {
            var resultBuffer = result.Buffer ;

            var s = Encoding.UTF8.GetString ( resultBuffer ) ;
            @out.WriteLine ( s ) ;
            @out.Flush ( ) ;
            Console.WriteLine ( "----\n" + s + "\n----\n" ) ;
            try
            {
                if ( s[ 0 ] == '{' )
                {
                    HandleJson ( options , conn , s , null ) ;

                    return ;
                }


                var xmlNameTable = new NameTable ( ) ;

                xmlNameTable.Add ( "log4j" ) ;
                var nameTable = new NameTable ( ) ;
                nameTable.Add ( "log4j" ) ;
                var xmlNamespaceManager = new XmlNamespaceManager ( xmlNameTable ) ;
                xmlNamespaceManager.AddNamespace (
                                                  "log4j"
                                                , "http://kaymccormick.com/xmlns/log4j"
                                                 ) ;

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
                                             , new XmlReaderSettings { NameTable = xmlNameTable }
                                             , xmlParserContext
                                              ) ;

                var tableName = "xmllog" ;
                var columnName = "data" ;
                var sql = "INSERT INTO " + tableName + " (" + columnName + ") VALUES (@xml)" ;
                var cmd = conn.CreateCommand ( ) ;
                cmd.CommandText = sql ;
                cmd.Parameters.Add (
                                    new SqlParameter ( "@xml" , SqlDbType.Xml )
                                    {
                                        Value = new SqlXml ( reader )
                                    }
                                   ) ;
                cmd.ExecuteNonQuery ( ) ;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( ex.ToString ( ) ) ;
            }
        }

        private static void HandleJson (
            JsonSerializerOptions options
          , SqlConnection         conn
          , string                s
          , XmlReader             reader
        )
        {
            var i = JsonSerializer.Deserialize < LogEventInstance > ( s , options ) ;
            var tableName = "jsonlog" ;
            var columnName = "jsondata" ;
            var sql = "INSERT INTO " + tableName + " (" + columnName + ") VALUES (@data)" ;
            var cmd = conn.CreateCommand ( ) ;
            cmd.CommandText = sql ;
            cmd.Parameters.Add ( new SqlParameter ( "@data" , SqlDbType.NVarChar ) { Value = s } ) ;
            cmd.ExecuteNonQuery ( ) ;
        }

        private static UdpClient CreateUdpClient ( int port )
        {
            var ipEndPoint = new IPEndPoint ( IPAddress.Any , port ) ;
            var udpClient = new UdpClient { ExclusiveAddressUse = false } ;
            udpClient.Client.SetSocketOption (
                                              SocketOptionLevel.Socket
                                            , SocketOptionName.ReuseAddress
                                            , true
                                             ) ;
            udpClient.Client.Bind ( ipEndPoint ) ;
            return udpClient ;
        }


        private void ThreadProc ( )
        {
            var spin = new SpinWait();

            var task = Run ( ) ;
            while (true)
            {
                if ( task.Status    == TaskStatus.Canceled
                     || task.Status == TaskStatus.Faulted
                     || task.Status == TaskStatus.RanToCompletion )
                {
                    task = Run ( ) ;
                }
                spin.SpinOnce();
            }
        }

        public bool Stop ( )
        {
            _thread?.Abort();
            _thread = null ;
            return true ;
        }

        public bool Pause ( ) { return false ; }

        public bool Continue ( ) { return false ; }

        public bool Shutdown ( )
        {
            _thread?.Abort();
            _thread = null;
            return true;
        }

        public void PerformFunc1 ( ) { Console.WriteLine ( "hello" ) ; }
    }
}


/// <summary>
/// </summary>
public class LogEventInfoConverter : JsonConverter < LogEventInstance >
{
    #region Overrides of JsonConverter<LogEventInfo>
    /// <summary>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public override LogEventInstance Read (
        ref Utf8JsonReader    reader
      , Type                  typeToConvert
      , JsonSerializerOptions options
    )
    {
        if ( reader.TokenType != JsonTokenType.StartObject )
        {
            throw new JsonException ( ) ;
        }

        //var builder = new LogBuilder(null);
        var info = new LogEventInstance ( ) ;
        while ( reader.Read ( ) )
        {
            if ( reader.TokenType == JsonTokenType.EndObject )
            {
                break ;
            }

            if ( reader.TokenType != JsonTokenType.PropertyName )
            {
                throw new JsonException ( ) ;
            }

            var field = reader.GetString ( ) ;
            switch ( field )
            {
                case "Level" :
                    reader.Read ( ) ;
                    if ( reader.TokenType != JsonTokenType.Number )
                    {
                        throw new JsonException ( ) ;
                    }

                    info.Level = reader.GetInt32 ( ) ;
                    break ;
                case "LoggerName" :
                    reader.Read ( ) ;
                    if ( reader.TokenType != JsonTokenType.String )
                    {
                        throw new JsonException ( ) ;
                    }

                    info.LoggerName = reader.GetString ( ) ;
                    break ;
                case "FormattedMessage" :
                    reader.Read ( ) ;
                    if ( reader.TokenType != JsonTokenType.String )
                    {
                        throw new JsonException ( ) ;
                    }

                    var msg = reader.GetString ( ) ;
                    info.FormattedMessage = msg ;
                    break ;
                case "SequenceID" :
                    reader.Read ( ) ;
                    if ( reader.TokenType != JsonTokenType.Number )
                    {
                        throw new JsonException ( ) ;
                    }

                    info.SequenceID = reader.GetInt32 ( ) ;
                    break ;
                case "CallerClassName" :
                    reader.Read ( ) ;
                    info.CallerClassName = reader.GetString ( ) ;
                    break ;
                case "Properties" :
                    reader.Read ( ) ;
                    while ( reader.Read ( ) )
                    {
                        if ( reader.TokenType == JsonTokenType.EndObject )
                        {
                            break ;
                        }

                        if ( reader.TokenType != JsonTokenType.PropertyName )
                        {
                            throw new JsonException ( ) ;
                        }

                        var curKey = reader.GetString ( ) ;
                        reader.Read ( ) ;
                        object value = null ;
                        if ( reader.TokenType == JsonTokenType.String )
                        {
                            value = reader.GetString ( ) ;
                        }
                        else if ( reader.TokenType == JsonTokenType.StartObject )
                        {
                            reader.Read ( ) ;
                            if ( reader.TokenType != JsonTokenType.PropertyName )
                            {
                                throw new JsonException ( ) ;
                            }

                            var key = reader.GetString ( ) ;
                            if ( key != "JsonConverter" )
                            {
                                var dict = new Dictionary < string , JsonElement > ( ) ;
                                var myVal =
                                    JsonSerializer.Deserialize < JsonElement > (
                                                                                ref reader
                                                                              , options
                                                                               ) ;
                                dict[ key ] = myVal ;
                                while ( reader.Read ( ) )
                                {
                                    if ( reader.TokenType == JsonTokenType.EndObject )
                                    {
                                        break ;
                                    }

                                    if ( reader.TokenType != JsonTokenType.PropertyName )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    var key1 = reader.GetString ( ) ;
                                    var myVal2 =
                                        JsonSerializer.Deserialize < JsonElement > (
                                                                                    ref reader
                                                                                  , options
                                                                                   ) ;
                                    dict[ key1 ] = myVal2 ;
                                }

                                info.Properties[ curKey ] = dict ;
                            }
                            else
                            {
                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.True )
                                {
                                    throw new JsonException ( ) ;
                                }

                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.PropertyName )
                                {
                                    throw new JsonException ( ) ;
                                }

                                var key2 = reader.GetString ( ) ;
                                if ( key2 != "Type" )
                                {
                                    throw new JsonException ( ) ;
                                }

                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.String )
                                {
                                    throw new JsonException ( ) ;
                                }

                                var typeName = reader.GetString ( ) ;
                                var type = Type.GetType ( typeName ) ;

                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.PropertyName )
                                {
                                    throw new JsonException ( ) ;
                                }

                                var key3 = reader.GetString ( ) ;
                                if ( key3 != "Value" )
                                {
                                    throw new JsonException ( ) ;
                                }

                                var o1 = JsonSerializer.Deserialize (
                                                                     ref reader
                                                                   , type
                                                                   , options
                                                                    ) ;

                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.EndObject )
                                {
                                    throw new JsonException ( ) ;
                                }

                                value = o1 ;
                            }
                        }

                        info.Properties[ curKey ] = value ;
                    }

                    break ;
                default :
                    var msg1 = $"Unknown field {field}" ;
                    Console.WriteLine ( msg1 ) ;
                    var elem = JsonSerializer.Deserialize < JsonElement > ( ref reader , options ) ;
                    info.AddUnknown ( field , elem ) ;
                    break ;
            }
        }

        return info ;
    }

    /// <summary>
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write (
        Utf8JsonWriter        writer
      , LogEventInstance      value
      , JsonSerializerOptions options
    )
    {
#if WRITE
            if (writer == null)
            {
                return;
            }

            writer.WriteStartObject();
            if (value == null) { }
            else
            {
                writer.WriteNumber("Level", value.Level.Ordinal);
                writer.WriteNumber("SequenceID", value.SequenceID);
                writer.WriteString("LoggerName", value.LoggerName);
                writer.WriteString("CallerClassName", value.CallerClassName);
                writer.WriteNumber("ProcessId", Process.GetCurrentProcess().Id);
                if (value.CallerFilePath != null)
                {
                    writer.WriteString("CallerFilePath", value.CallerFilePath);
                    writer.WriteNumber("CallerLineNumber", value.CallerLineNumber);
                }

                if (value.CallerMemberName != null)
                {
                    writer.WriteString("CallerMemberName", value.CallerMemberName);
                }

                if (value.Exception != null)
                {
                    writer.WriteString("ExceptionString", value.Exception.ToString());
                }
                writer.WriteNumber("ManagedThreadId", Thread.CurrentThread.ManagedThreadId);
                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = "Thread" + value.SequenceID;
                }

                writer.WriteString("ThreadName", Thread.CurrentThread.Name);
                if (Task.CurrentId.HasValue)
                {
                    writer.WriteNumber("CurrentTaskId", Task.CurrentId.Value);
                }

                writer.WriteString("Message", value.Message);
                writer.WriteString("TimeStamp", value.TimeStamp);
                writer.WriteString("FormattedMessage", value.FormattedMessage);
                if (value.HasProperties)
                {
                    writer.WriteStartObject("Properties");
                    foreach (var p in value.Properties)
                    {
                        writer.WritePropertyName(p.Key.ToString());
                        if (p.Value is Type t)
                        {
                            JsonSerializer.Serialize(writer, t, typeof(Type), options);
                        }
                        else
                        {
                            try
                            {
                                JsonSerializer.Serialize(writer, p.Value, options);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"{p.Key}: {p.Value}: {ex}");
                                throw;
                            }
                        }
                    }

                    writer.WriteEndObject();
                }
            }

            writer.WriteStartObject("GDC");
            foreach (var name in GlobalDiagnosticsContext.GetNames())
            {
                writer.WritePropertyName(name);
                var value1 = GlobalDiagnosticsContext.GetObject(name);
                if (value1 is Type t)
                {
                    JsonSerializer.Serialize(writer, t, typeof(Type), options);
                }
                else
                {
                    try
                    {
                        JsonSerializer.Serialize(writer, value1, options);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{name}: {value1}: {ex}");
                        throw;
                    }
                }
            }

            writer.WriteEndObject();

            writer.WriteStartObject("MDLC");

            foreach (var name in MappedDiagnosticsLogicalContext.GetNames())
            {
                writer.WritePropertyName(name);
                var value1 = GlobalDiagnosticsContext.GetObject(name);
                if (value1 is Type t)
                {
                    JsonSerializer.Serialize(writer, t, typeof(Type), options);
                }
                else
                {
                    try
                    {
                        JsonSerializer.Serialize(writer, value1, options);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{name}: {value1}: {ex}");
                        throw;
                    }
                }
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
#endif
    }
    #endregion
}

public class LogEventInstance
{
    private object _level ;
    private string _loggerName ;
    private string _formattedMessage ;
    public  int    SequenceID ;

    private readonly IDictionary < string , object > _properties =
        new Dictionary < string , object > ( ) ;

    private string _callerClassName ;

    public object Level { get => _level ; set => _level = value ; }

    public string LoggerName { get => _loggerName ; set => _loggerName = value ; }

    public string FormattedMessage { get => _formattedMessage ; set => _formattedMessage = value ; }

    public IDictionary < string , object > Properties => _properties ;

    public string CallerClassName { get => _callerClassName ; set => _callerClassName = value ; }

    public IDictionary < string , object > UnknownFields { get ; } =
        new Dictionary < string , object > ( ) ;

    public void AddUnknown ( string field , in JsonElement elem )
    {
        UnknownFields[ field ] = elem ;
    }

    public override string ToString ( )
    {
        return
            $"{nameof ( SequenceID )}: {SequenceID}, {nameof ( Level )}: {Level}, {nameof ( LoggerName )}: {LoggerName}, {nameof ( FormattedMessage )}: {FormattedMessage}, {nameof ( Properties )}: {Properties}, {nameof ( CallerClassName )}: {CallerClassName}, {nameof ( UnknownFields )}: {string.Join ( ", " , UnknownFields.Select ( pair => $"{pair.Key}: {pair.Value}" ) )}" ;
    }
    #endregion
}