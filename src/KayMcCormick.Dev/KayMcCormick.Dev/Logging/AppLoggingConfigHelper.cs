/* test 123 */

#if ENABLE_PROXYLOG
using Castle.DynamicProxy ;
#endif
#if ENABLE_WCF_TARGET
using System.ServiceModel.Discovery ;
#endif
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Diagnostics.CodeAnalysis ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Net.Sockets ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.InteropServices ;
using System.Security ;
using System.Security.Permissions ;
using System.Text ;
using System.Text.RegularExpressions ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Tracing ;
using NLog ;
using NLog.Common ;
using NLog.Config ;
using NLog.LayoutRenderers ;
using NLog.Layouts ;
using NLog.Targets ;
/* test 123 */
/* test 123 */
/* test 123 */
#if ENABLE_WCF_TARGET
using NLog.LogReceiverService ;
#endif

namespace KayMcCormick.Dev.Logging
{
#if NETFRAMEWORK && ENABLE_WCF_TARGET
    internal class Client
    {
        public static EndpointAddress serviceAddress ;

        // ** DISCOVERY ** //  
        public static bool FindService ( )
        {
            var discoveryClient = new DiscoveryClient ( new UdpDiscoveryEndpoint ( ) ) ;

            var logReceiverServices = ( Collection < EndpointDiscoveryMetadata > ) discoveryClient
                                                                                  .Find (
                                                                                         new
                                                                                             FindCriteria (
                                                                                                           typeof
                                                                                                           ( ILogReceiverServer
                                                                                                           )
                                                                                                          )
                                                                                        )
                                                                                  .Endpoints ;

            discoveryClient.Close ( ) ;

            if ( logReceiverServices.Count == 0 )
            {

                Console.WriteLine ( "\nNo services are found." ) ;

                return false ;
            }
            else
            {
                serviceAddress = logReceiverServices[ 0 ].Address ;
                return true ;
            }
        }
    }
#endif

    /// <summary>Static class containing logging configuration methods</summary>
    /// <seealso cref="NLog.Config.LoggingConfiguration" />
    public static class AppLoggingConfigHelper

    {
        private const           string JsonTargetName              = "json_out" ;
        private const           string DisableLogTargetsEnvVarName = "DISABLE_LOG_TARGETS" ;
        private const           string PublicFacingHostAddress     = "xx1.mynetgear.com:" ;
        private const           int    DefaultProtoLogUdpPort      = 4445 ;
        private const           string EnvVarName_MINLOGLEVEL      = "MINLOGLEVEL" ;
        private const           string DefaultEventLogTargetName   = "eventLogTarget" ;
        private const           string LogRootPath                 = @"c:\data\logs\" ;
        private static readonly int    _nLogViewerPort             = 9995 ;


        private static Logger Logger ;

        [ ThreadStatic ]
        private static int ? _numTimesConfigured ;

        private static          LogFactory                                _factory ;
        private static          Target                                    _serviceTarget ;
        private static readonly int                                       _chainsawPort = 4445 ;
        private static          MyCacheTarget                             _cacheTarget ;
        private static          LogDelegates.LogMethod                    _oldLogMethod ;
        private static          Dictionary < LogLevel , List < Target > > _dict ;
        private static          bool                                      _loggingConfigured ;
        private static          LogLevel                                  _minLogLevel ;
        private static          ChainsawTarget                            _chainsawTarget ;

        private static readonly int _protoLogPort = DefaultProtoLogUdpPort ;

        private static readonly IPAddress _protoLogIpAddress = IPAddress.Broadcast ;

        private static readonly IPEndPoint _ipEndPoint =
            new IPEndPoint ( _protoLogIpAddress , _protoLogPort ) ;

        private static readonly UdpClient _udpClient = CreateUdpClient ( ) ;

        private static readonly Log4JXmlEventLayoutRenderer _xmlEventLayoutRenderer =
            new MyLog4JXmlEventLayoutRenderer ( ) ;

        private static          NetworkTarget jsonNetworkTarget ;
        private static readonly ProtoLogger   _protoLogger = new ProtoLogger ( ) ;

        private static readonly Action < LogEventInfo > _protoLogAction = _protoLogger.LogAction ;

        //private static string _chainsawHost = PublicFacingHostAddress;
        private static readonly string _chainsawHost = "10.25.0.102" ;
        private static          bool   _performant ;

        /// <summary>The string writer</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for StringWriter
        public static StringWriter Writer { get ; set ; }


        /// <summary>
        /// </summary>

        public static Target ServiceTarget { get ; private set ; }

        /// <summary>Gets or sets a value indicating whether [logging is configured].</summary>
        /// <value>
        ///     <see language="true" /> if [logging is configured]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for LoggingIsConfigured
        public static bool ? LoggingIsConfigured { get ; set ; }

        /// <summary>
        ///     Gets or sets a value indicating whether [dump existing
        ///     configuration].
        /// </summary>
        /// <value>
        ///     <see language="true" /> if [dump existing configuration]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DumpExistingConfig
        public static bool DumpExistingConfig { get ; } = true ;


        /// <summary>Gets or sets a value indicating whether [force code configuration].</summary>
        /// <value>
        ///     <see language="true" /> if [force code configuration]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ForceCodeConfig
        public static bool ForceCodeConfig { get ; } = false ;

        /// <summary>
        /// </summary>
        public static string ConsoleTargetName { get ; private set ; }

        /// <summary>
        /// </summary>
        public static string EventLogTargetName { get ; private set ; }

        /// <summary>
        /// </summary>
        public static MyCacheTarget2 CacheTarget2 { get ; private set ; }

        /// <summary>
        /// </summary>
        public static UdpClient UdpClient { get { return _udpClient ; } }

        /// <summary>
        /// </summary>
        public static IPEndPoint IpEndPoint { get { return _ipEndPoint ; } }

        /// <summary>
        /// </summary>
        public static Layout XmlEventLayout { get ; } = new MyLayout ( _xmlEventLayoutRenderer ) ;

        /// <summary>
        /// </summary>
        public static LogDelegates.LogMethod ProtoLogDelegate { get ; } = ProtoLogMessage ;

        /// <summary>
        /// </summary>
        public static string DisableLoggingEnvVar { get ; set ; } = "DISABLE_LOGGING" ;

        /// <summary>
        /// </summary>
        public static bool Performant { get { return _performant ; } set { _performant = value ; } }

        private static UdpClient CreateUdpClient ( )
        {
            return new UdpClient
                   {
                       EnableBroadcast = true
                     , Client =
                           new Socket ( SocketType.Dgram , ProtocolType.Udp )
                           {
                               EnableBroadcast = true , DualMode = true
                           }
                   } ;
        }

        private static void DoLogMessage ( string message )
        {
            System.Diagnostics.Debug.WriteLine (
                                                nameof ( AppLoggingConfigHelper ) + ":" + message
                                               ) ;
            // System.Diagnostics.Debug.WriteLine ( nameof(AppLoggingConfigHelper) + ":" + message ) ;
        }


        internal static LogFactory ConfigureLogging (
            LogDelegates.LogMethod logMethod
          , bool                   proxyLogging = false
          , ILoggingConfiguration  config1      = null
        )
        {
            if ( Environment.GetEnvironmentVariable ( DisableLoggingEnvVar ) != null )
            {
                logMethod ( "Disabling logging completely" ) ;
                LogManager.Configuration = new CodeConfiguration ( ) ;
                return LogManager.LogFactory ;
            }

            if ( config1 == null )
            {
                config1 = AppLoggingConfiguration.Default ;
            }

            ApplyConfiguration ( config1 ) ;

            if ( ! Performant )
            {
                InternalLogging ( ) ;
            }


#if ENABLE_PROXYLOG
            LogFactory proxiedFactory = null ;
            if ( proxyLogging )
            {
                var proxyGenerator = new ProxyGenerator ( ) ;
                var loggerProxyHelper = new LoggerProxyHelper ( proxyGenerator , DoLogMessage ) ;
                var logFactory = new MyLogFactory ( DoLogMessage ) ;
                var lConfLogFactory = loggerProxyHelper.CreateLogFactory ( logFactory ) ;
                proxiedFactory = lConfLogFactory ;
            }
#endif
#if FIELD_ACCESS
            var fieldInfo = typeof ( LogManager ).GetField (
                                                            "factory"
                                              , BindingFlags.Static
                                                            | BindingFlags.NonPublic
                                                           ) ;
            if ( fieldInfo != null )
            {
                logMethod ( $"field info is {fieldInfo.DeclaringType} . {fieldInfo.Name}" ) ;
                var cur = fieldInfo.GetValue ( null ) ;
                logMethod ( $"cur is {cur}" ) ;
#if ENABLE_PROXYLOG
                if ( proxyLogging )
                {
                    fieldInfo.SetValue ( null , proxiedFactory ) ;
                    var newVal = fieldInfo.GetValue ( null ) ;
                    logMethod ( $"New Value = {newVal}" ) ;
                }
#endif
            }
#endif
#if ENABLE_PROXYLOG
            return PerformConfiguration ( logMethod , proxyLogging , proxiedFactory , config1 ) ;
#else
            return PerformConfiguration ( logMethod , false , null , config1 ) ;
#endif
        }


        private static void ApplyConfiguration ( ILoggingConfiguration config1 )
        {
            EventLogTargetName = DefaultEventLogTargetName ;
            ConsoleTargetName  = "consoleTarget" ;
        }

        private static LogFactory PerformConfiguration (
            LogDelegates.LogMethod logMethod
          , bool                   proxyLogging
          , LogFactory             proxiedFactory
          , ILoggingConfiguration  config1
        )
        {
            var oldTargets = LogManager.Configuration?.AllTargets?.ToList ( ) ;
            var oldRules = LogManager.Configuration?.LoggingRules?.ToList ( ) ;
            var usedNames = oldTargets?.Select ( target => target.Name )?.ToHashSet ( ) ;
            LogFactory useFactory ;
            if ( proxyLogging )
            {
                useFactory = proxiedFactory ;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine ( "Using stock logfactory" ) ;
                useFactory = LogManager.LogFactory ;
            }

            var lConf = new CodeConfiguration ( useFactory ) ;
            if ( config1.LogThrowExceptions.GetValueOrDefault ( ) )
            {
                logMethod ( "Setting throwExceptions to true" ) ;
                LogManager.ThrowExceptions = true ;
            }
            else
            {
                logMethod ( "Setting throwExceptions to false" ) ;
                LogManager.ThrowExceptions = false ;
            }

            //LogManager.ThrowExceptions = true ;
            _dict = LogLevel.AllLoggingLevels.ToDictionary (
                                                            level => level
                                                          , level => new List < Target > ( )
                                                           ) ;
            logMethod (
                       $"Log Levels\tName\tOrdinal:\n{string.Join ( ",\n" , _dict.Keys.Select ( level => $"\t\t{level.Name}\t({level.Ordinal})" ) )}"
                      ) ;
            _minLogLevel = config1.MinLogLevel ;

            var envVarName = EnvVarName_MINLOGLEVEL ;
            var env = Environment.GetEnvironmentVariable ( envVarName ) ;
            if ( env != null )
            {
                logMethod (
                           $"Detected environment variable {envVarName} with value {env}. Attempting to set minimum log level."
                          ) ;
                LogLevel level = null ;
                try
                {
                    level = env.Any ( c => ! char.IsDigit ( c ) )
                                ? LogLevel.FromString ( env )
                                : LogLevel.FromOrdinal ( int.Parse ( env ) ) ;
                }
                catch ( ArgumentException ex )
                {
                    logMethod ( $"Unable to determine specified log level: {ex.Message}" ) ;
                }

                if ( level != null )
                {
                    logMethod (
                               $"Setting minimum log level from environment variable {envVarName} to {level}"
                              ) ;
                    _minLogLevel = level ;
                }
            }

            if ( _minLogLevel != null )
            {
                logMethod (
                           $"Using supplied minimum log level of {_minLogLevel} from application configuration or environment variable source."
                          ) ;
                if ( _minLogLevel == LogLevel.Off )
                {
                    logMethod ( "Supplied value will suppress logging." ) ;
                }
            }

            if ( _minLogLevel == null )
            {
                _minLogLevel = LogLevel.Trace ;
            }


            var t = _dict[ _minLogLevel ] ;
            var errorTargets = _minLogLevel <= LogLevel.Error ? _dict[ LogLevel.Error ] : null ;

            var disabledLogTargets =
                Environment.GetEnvironmentVariable ( DisableLogTargetsEnvVarName ) ;
            HashSet < string > disabled ;
            if ( disabledLogTargets != null )
            {
                var targets = disabledLogTargets.Split ( ';' ) ;
                disabled = new HashSet < string > ( targets ) ;
                logMethod ( string.Join ( ", " , disabled ) ) ;
            }
            else
            {
                disabled = new HashSet < string > ( ) ;
            }

            if ( config1.IsEnabledEventLogTarget.GetValueOrDefault ( ) )
            {
                var x = EventLogTarget ( EventLogTargetName ) ;
                errorTargets?.Add ( x ) ;
            }

            var tracingTarget = new NLogTraceTarget ( ) ;
            t.Add ( tracingTarget ) ;

#if ENABLE_WCF_TARGET
            var endpointAddress = Environment.GetEnvironmentVariable("LOGGING_WEBSERVICE_ENDPOINT")
                                  ?? $"http://{PublicFacingHostAddress}/LogService/ReceiveLogs.svc";
            EndpointAddress receiverAddress = null;//new EndpointAddress(endpointAddress);
            if(Client.FindService())
            {
                receiverAddress = Client.serviceAddress ;
                logMethod($"Found service at endpoint {receiverAddress}");

            }
            receiverAddress =
 new EndpointAddress("http://exomail-87976:8737/discovery/scenarios/logreceiver/");
            // TODO make this address configurable
            if ( receiverAddress != null )
            {
                ServiceTarget = new LogReceiverWebServiceTarget ( "log" )
            {
                // EndpointAddress = Configuration.GetValue(LOGGING_WEBSERVICE_ENDPOINT)
                                    EndpointAddress = receiverAddress.ToString ( )
, ClientId = new SimpleLayout ( "${processName}" )
, EndpointConfigurationName =
                                        "WSDualHttpBinding_ILogReceiverServer"
, IncludeEventProperties = true
               ,
                                } ;
            }
                
            // var wrap = new AsyncTargetWrapper("wrap1", ServiceTarget);
            // "http://localhost:27809/ReceiveLogs.svc";
            // webServiceTarget.EndpointConfigurationName = "log";
            _dict[LogLevel.Debug].Add(ServiceTarget);
#endif


            if ( config1.IsEnabledConsoleTarget.HasValue
                 && config1.IsEnabledConsoleTarget.Value
                 && ! disabled.Contains ( ConsoleTargetName ) )
            {
                var consoleTarget = new ConsoleTarget ( ConsoleTargetName )
                                    {
                                        Error = true
                                      , Layout =
                                            new SimpleLayout ( "${level} ${message} ${logger}" )
                                    } ;
                _dict[ LogLevel.Warn ].Add ( consoleTarget ) ;
            }

            // ConfigurationItemFactory.Default = new ConfigurationItemFactory();
            // jsonNetworkTarget = new NetworkTarget ( "jsonNetworkTarget" )
            // {
            // Address = "udp://10.25.0.102:4477"
            // , Layout  = new MyJsonLayout ( )
            // } ;
            // t.Add ( jsonNetworkTarget ) ;
            if ( config1.IsEnabledXmlFileTarget )
            {
                var logRootDir = LogRootPath ;
                var xmlTarget = new AppFileTarget ( "xmlFile" )
                                {
                                    FileName =
                                        new SimpleLayout (
                                                          $@"{logRootDir}xmllog-${{processId}}.log"
                                                         )
                                  , Layout = XmlEventLayout
                                } ;
                t.Add ( xmlTarget ) ;
            }


            #region Cache Target
            if ( config1.IsEnabledCacheTarget.GetValueOrDefault ( ) )
            {
                _cacheTarget = new MyCacheTarget ( ) ;
                _dict[ LogLevel.Debug ].Add ( _cacheTarget ) ;
                CacheTarget2 = new MyCacheTarget2 { Layout = SetupJsonLayout ( ) } ;
                _dict[ LogLevel.Debug ].Add ( CacheTarget2 ) ;
            }
            #endregion
            #region NLogViewer Target
            var viewer = Viewer ( ) ;
            t.Add ( viewer ) ;
            #endregion
            #region Debugger Target
            if ( config1.IsEnabledDebuggerTarget.GetValueOrDefault ( )
                 && ! disabled.Contains ( config1.DebuggerTargetName ) )
            {
                var debuggerTarget =
                    new DebuggerTarget ( config1.DebuggerTargetName )
                    {
                        Layout = new SimpleLayout ( "${message}" )
                    } ;
                t.Add ( debuggerTarget ) ;
            }
            #endregion
            #region Chainsaw Target
            _chainsawTarget = CreateChainsawTarget ( ) ;
            var chainsawTarget = _chainsawTarget ;
            t.Add ( chainsawTarget ) ;
            #endregion
            t.Add ( MyFileTarget ( ) ) ;
            var jsonFileTarget = JsonFileTarget ( ) ;
            _dict[ LogLevel.Trace ].Add ( jsonFileTarget ) ;

            // var jsonNetworkTarget = new NetworkTarget ( "jsonNet" ) ;
            // jsonNetworkTarget.Layout = new MyJsonLayout ( ) ;
            // SetupNetworkTarget ( jsonNetworkTarget , "udp://127.0.0.1:5110" ) ;
            // t.Add ( jsonNetworkTarget ) ;

            var byType = new Dictionary < Type , int > ( ) ;
            var keys = _dict.Keys.ToList ( ) ;
            foreach ( var k in keys )
            {
                var v = _dict[ k ].Where ( target => target != null ) ;
                _dict[ k ] = v.Where ( ( target , i ) => ! disabled.Contains ( target.Name ) )
                              .ToList ( ) ;
            }

            foreach ( var target in _dict.SelectMany ( pair => pair.Value ) )
            {
                var type = target.GetType ( ) ;
                byType.TryGetValue ( type , out var count ) ;
                count          += 1 ;
                byType[ type ] =  count ;

                if ( target.Name == null
                     || usedNames != null && usedNames.Contains ( target.Name ) )
                {
                    target.Name = $"{Regex.Replace ( type.Name , "Target" , "" )}{count:D2}" ;
                }

                logMethod (
                           $"Adding log target {target.Name} of type {target.GetType ( ).AssemblyQualifiedName}"
                          ) ;
                LogAddTarget ( target ) ;
                lConf.AddTarget ( target ) ;
            }

            foreach ( var result in _dict.Select (
                                                  pair => LoggingRule (
                                                                       pair
                                                                     , _minLogLevel
                                                                     , config1
                                                                      )
                                                 ) )
            {
                foreach ( var loggingRule in result )
                {
                    logMethod ( $"Adding loging rule {loggingRule}" ) ;
                    lConf.LoggingRules.Add ( loggingRule ) ;
                }

                // ((List<LoggingRule>lConf.LoggingRules)).AddRange ( result ) ;
            }

            if ( oldTargets != null )
            {
                foreach ( var oldTarget in oldTargets )
                {
                    lConf.AddTarget ( oldTarget ) ;
                }
            }

            if ( oldRules != null )
            {
                foreach ( var oldRule in oldRules )
                {
                    lConf.LoggingRules.Add ( oldRule ) ;
                }
            }

            try
            {
                LogManager.Configuration = lConf ;
                Logger                   = LogManager.GetCurrentClassLogger ( ) ;
                _loggingConfigured       = true ;
                System.Diagnostics.Debug.WriteLine (
                                                    $"Logging configured. Logger is {Logger}. Configuration is {lConf}. Returning factory {useFactory}"
                                                   ) ;
                return useFactory ;
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine ( ex.ToString ( ) ) ;
                throw ;
            }
        }

        private static void LogAddTarget ( Target target )
        {
            PROVIDER_GUID.EventWriteLOGTARGET_ATTACHED_EVENT (
                                                              target.Name
                                                            , target.GetType ( ).FullName
                                                             ) ;
        }

        private static ChainsawTarget CreateChainsawTarget ( )
        {
            var chainsawTarget = new ChainsawTarget { Layout = XmlEventLayout } ;
            var s = _chainsawPort.ToString ( ) ;
            SetupNetworkTarget ( chainsawTarget , $"udp://{_chainsawHost}:{s}" ) ;
            return chainsawTarget ;
        }

        [ SuppressMessage (
                              "Style"
                            , "IDE0060:Remove unused parameter"
                            , Justification = "<Pending>"
                          ) ]
        private static IEnumerable < LoggingRule > LoggingRule (
            KeyValuePair < LogLevel , List < Target > > pair
          , LogLevel                                    config1MinLogLevel
          , ILoggingConfiguration                       config1
        )
        {
            return pair.Value.Select (
                                      target => new LoggingRule (
                                                                 "*"
                                                               , config1MinLogLevel <= pair.Key
                                                                     ? pair.Key
                                                                     : config1MinLogLevel
                                                               , target
                                                                )
                                     ) ;
        }

        private static EventLogTarget EventLogTarget ( string eventLogTargetName )
        {
            var x = new EventLogTarget ( eventLogTargetName ) { Source = "Application Error" } ;
            return x ;
        }

        // TODO never used??

        private static IEnumerable < LoggingRule > LoggingRule (
            KeyValuePair < LogLevel , List < Target > > arg
        )
        {
            return arg.Value.Select ( target => new LoggingRule ( "*" , arg.Key , target ) ) ;
        }


        private static LoggingRule DefaultLoggingRule ( Target target )
        {
            return new LoggingRule ( "*" , LogLevel.FromOrdinal ( 0 ) , target ) ;
        }

        private static void InternalLogging ( )
        {
            return ;
            InternalLogger.LogLevel = LogLevel.Debug ;

            var id = Process.GetCurrentProcess ( ).Id ;
            var logFile = $@"c:\temp\nlog-internal-{id}.txt" ;
            InternalLogger.LogFile = logFile ;

            //InternalLogger.LogToConsole      = true ;
            //InternalLogger.LogToConsoleError = true ;
            //InternalLogger.LogToTrace        = true ;

            Writer                   = new StringWriter ( ) ;
            InternalLogger.LogWriter = Writer ;
        }

        private static void SetupNetworkTarget ( NetworkTarget target , string address )
        {
            target.Address = new SimpleLayout ( address ) ;
        }

        private static NLogViewerTarget Viewer ( string name = null )
        {
            var s = _nLogViewerPort ;
            return new NLogViewerTarget ( name )
                   {
                       Address              = new SimpleLayout ( $"udp://10.25.0.102:{s}" )
                     , IncludeAllProperties = true
                     , IncludeCallSite      = true
                     , IncludeSourceInfo    = true
                     , IncludeNdlc          = true
                   } ;
        }

        /// <summary>JSON File Target</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for JsonFileTarget
        public static FileTarget JsonFileTarget ( )
        {
            var f = new AppFileTarget ( JsonTargetName )
                    {
                        FileName =
                            Layout.FromString ( @"c:\data\logs\${processName}\${processId}.json" )
                        // ,
                        // Layout = new MyJsonLayout()
                      , Layout = new MyJsonLayout ( )
                    } ;

            return f ;
        }

        /// <summary>My File Target.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for MyFileTarget
        public static FileTarget MyFileTarget ( )
        {
            var f = new AppFileTarget ( "text_log" )
                    {
                        FileName = Layout.FromString ( @"c:\data\logs\log-${processid}.txt" )
                      , Layout   = Layout.FromString ( "${message}" )
                    } ;

            return f ;
        }

        /// <summary>Removes the target.</summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RemoveTarget
        public static void RemoveTarget ( [ NotNull ] Target target )
        {
            if ( target == null )
            {
                throw new ArgumentNullException ( nameof ( target ) ) ;
            }

            LogManager.Configuration.RemoveTarget ( target.Name ) ;
            LogManager.LogFactory.ReconfigExistingLoggers ( ) ;
#if LOGREMOVAL
            Logger.Debug ( "Removing target " + target ) ;
            foreach ( var t in LogManager.Configuration.AllTargets )
            {
                Logger.Debug ( "Target " + t ) ;
            }
#endif
        }

        /// <summary>Ensures the logging configured.</summary>
        /// <param name="slogMethod"></param>
        /// <param name="config1"></param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <exception cref="Exception">no config loaded field found</exception>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for EnsureLoggingConfigured
        [ SuppressMessage (
                              "Microsoft.Naming"
                            , "CA2204:Literals should be spelled correctly"
                            , MessageId = "EnsureLoggingConfigured"
                          ) ]
        public static ILogger EnsureLoggingConfigured (
            LogDelegates.LogMethod    slogMethod     = null
          , ILoggingConfiguration     config1        = null
          , [ CallerFilePath ] string callerFilePath = null
        )
        {
            try
            {
                var logMethod = slogMethod != null
                                    ? slogMethod + ProtoLogDelegate
                                    : ProtoLogDelegate ;
                if ( ! _numTimesConfigured.HasValue )
                {
                    _numTimesConfigured = 1 ;
                }
                else
                {
                    _numTimesConfigured += 1 ;
                }

                if ( logMethod == null )
                {
                    logMethod = DoLogMessage ;
                }

                _oldLogMethod = logMethod ;
                logMethod     = message => _oldLogMethod ( message ) ;
                logMethod (
                           $"[time {_numTimesConfigured.Value}]\t{nameof ( EnsureLoggingConfigured )} called from {callerFilePath}"
                          ) ;


                var checkExistingConfig = false ;
                var isMyConfig = false ;
                var doConfig = true ;
                if ( checkExistingConfig )
                {
                    var configLoaded = GetConfigLoaded ( ) ;

                    isMyConfig = ! configLoaded.GetValueOrDefault ( )
                                 || LogManager.Configuration is CodeConfiguration ;

                    doConfig = ! LoggingIsConfigured.GetValueOrDefault ( )
                               || ForceCodeConfig && ! isMyConfig ;
                }

                logMethod (
                           $"{nameof ( DumpExistingConfig )} = {DumpExistingConfig}; {nameof ( doConfig )} = {doConfig}; {nameof ( LoggingIsConfigured )} = {LoggingIsConfigured}; {nameof ( ForceCodeConfig )} = {ForceCodeConfig}; {nameof ( isMyConfig )} = {isMyConfig});"
                          ) ;
                if ( ! Performant && DumpExistingConfig )
                {
                    void Collect ( string s ) { System.Diagnostics.Debug.WriteLine ( s ) ; }

                    var x = new StringWriter ( ) ;
                    Utils.PerformLogConfigDump ( x ) ;
                    Collect ( x.ToString ( ) ) ;
                }

                if ( doConfig )
                {
                    logMethod ( "About to configure logging" ) ;
                    var factory = ConfigureLogging ( logMethod , true , config1 ) ;
                    _factory = factory ;
                }


                // DumpPossibleConfig ( LogManager.Configuration ) ;
                return _factory.GetLogger ( "DefaultLogger" ) ;
            }
            catch ( SecurityException ex )
            {
                System.Diagnostics.Debug.WriteLine ( ex.ToString ( ) ) ;
                ProtoLogDelegate ( ex.ToString ( ) ) ;
            }

            return null ;
        }

        private static bool ? GetConfigLoaded ( )
        {
            return null ;
            var perm =
                new ReflectionPermission ( ReflectionPermissionFlag.RestrictedMemberAccess ) ;
            perm.Demand ( ) ;
            try
            {
                var fieldInfo2 = LogManager.LogFactory.GetType ( )
                                           .GetField (
                                                      "_config"
                                                    , BindingFlags.Instance | BindingFlags.NonPublic
                                                     ) ;

                if ( fieldInfo2 == null )
                {
                    System.Diagnostics.Debug.WriteLine (
                                                        "no field _configLoaded for "
                                                        + LogManager.LogFactory
                                                       ) ;
                    // throw new Exception ( Resources.AppLoggingConfigHelper_EnsureLoggingConfigured_no_config_loaded_field_found ) ;
                }

                bool ? configLoaded = null ;
                if ( fieldInfo2 != null )
                {
                    var config = fieldInfo2.GetValue ( LogManager.LogFactory ) ;

                    //LogManager.ThrowConfigExceptions = true;
                    //LogManager.ThrowExceptions = true;
                    var fieldInfo = LogManager.LogFactory.GetType ( )
                                              .GetField (
                                                         "_configLoaded"
                                                       , BindingFlags.Instance
                                                         | BindingFlags.NonPublic
                                                        ) ;


                    if ( fieldInfo == null )
                    {
                        configLoaded = config != null ;

                        System.Diagnostics.Debug.WriteLine (
                                                            "no field _configLoaded for "
                                                            + LogManager.LogFactory
                                                           ) ;
                        // throw new Exception ( "no config loaded field found" ) ;
                    }
                    else
                    {
                        configLoaded = ( bool ) fieldInfo.GetValue ( LogManager.LogFactory ) ;
                    }

                    LoggingIsConfigured = configLoaded ;
                }

                return configLoaded ;
            }
            catch ( SecurityException ex )
            {
                System.Diagnostics.Debug.WriteLine ( ex.ToString ( ) ) ;
            }

            return null ;
        }

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

        /// <summary>
        /// </summary>
        /// <param name="collect"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary < string , object > DoDumpConfig ( Action < string > collect )
        {
            var config = LogManager.Configuration ;
            if ( config == null )
            {
                return new Dictionary < string , object > ( ) ;
            }

            var configInfo = new Dictionary < string , object > ( ) ;
            var targets = new Dictionary < string , object > ( ) ;
            configInfo[ "Targets" ] = targets ;
            foreach ( var aTarget in config.AllTargets )
            {
                var target = new Dictionary < string , object > ( ) ;
                targets[ aTarget.Name ] = target ;
                collect ( aTarget.Name ) ;
                var targetType = aTarget.GetType ( ).ToString ( ) ;
                target[ "Type" ] = targetType ;
                collect ( targetType ) ;
                target[ "Rules" ] = config.LoggingRules
                                          .Where ( rule => rule.Targets.Contains ( aTarget ) )
                                          .Select ( ( rule , i ) => new { rule , i } )
                                          .ToDictionary ( arg => arg.i , arg => arg.rule ) ;

                foreach ( var propertyInfo in aTarget.GetType ( ).GetProperties ( ) )
                {
                    var p = propertyInfo.GetValue ( aTarget ) ;
                    target[ propertyInfo.Name ] = p ;
                }

                if ( aTarget is FileTarget f )
                {
                    target[ "FileName" ] = f.FileName ;
                }

                if ( aTarget is TargetWithLayout a )
                {
                    if ( a.Layout is JsonLayout jl )
                    {
                        string Selector ( JsonAttribute attribute , int i )
                        {
                            if ( attribute == null )
                            {
                                throw new ArgumentNullException ( nameof ( attribute ) ) ;
                            }

                            var b = new StringBuilder ( ) ;
                            var propertyInfos = attribute
                                               .GetType ( )
                                               .GetProperties (
                                                               BindingFlags.Public
                                                               | BindingFlags.Instance
                                                              ) ;
                            foreach ( var propertyInfo in propertyInfos )
                            {
                                var val2 = propertyInfo.GetValue ( attribute ) ;
                                b.Append ( $"{propertyInfo.Name} = {val2}; " ) ;
                            }

                            return b.ToString ( ) ;
                        }

                        var enumerable = jl.Attributes.Select ( Selector ) ;
                        collect ( string.Join ( "--" , enumerable ) ) ;
                    }
                }

                if ( aTarget is FileTarget gt )
                {
                    collect ( gt.FileName.ToString ( ) ) ;
                }
            }

            return configInfo ;
        }

        private static void DumpPossibleConfig ( LoggingConfiguration configuration )
        {
            var candidateConfigFilePaths = LogManager.LogFactory.GetCandidateConfigFilePaths ( ) ;
            foreach ( var q in candidateConfigFilePaths )
            {
                Debug ( $"{q}" ) ;
            }

            var fieldInfo = configuration.GetType ( )
                                         .GetField (
                                                    "_originalFileName"
                                                  , BindingFlags.NonPublic | BindingFlags.Instance
                                                   ) ;
            if ( fieldInfo != null )
            {
                if ( fieldInfo.GetValue ( configuration ) != null )
                {
                    {
                        Debug ( "Original NLog configuration filename" ) ;
                    }
                }
            }

            Debug ( $"{configuration}" ) ;
        }


        private static void Debug ( string s ) { }

        /// <summary>
        ///     Set up a <seealso cref="NLog.Layouts.JsonLayout" /> for json
        ///     loggers.
        /// </summary>
        /// <returns>Configured JSON layout</returns>
        public static JsonLayout SetupJsonLayout ( )
        {
            var atts = new[]
                       {
                           Tuple.Create ( "logger" ,    ( string ) null )
                         , Tuple.Create ( "level" ,     ( string ) null )
                         , Tuple.Create ( "@t" ,        "${longdate}" )
                         , Tuple.Create ( "logger" ,    ( string ) null )
                         , Tuple.Create ( "@mt" ,       "${message}" )
                         , Tuple.Create ( "exception" , "${exception}" )
                       } ;


            var l = new JsonLayout
                    {
                        IncludeGdc           = false
                      , IncludeMdlc          = false
                      , IncludeAllProperties = false
                      , MaxRecursionLimit    = 3
                    } ;
            ( ( List < JsonAttribute > ) l.Attributes ).AddRange (
                                                                  atts.Select (
                                                                               tuple
                                                                                   => new
                                                                                       JsonAttribute (
                                                                                                      tuple
                                                                                                         .Item1
                                                                                                    , Layout
                                                                                                         .FromString (
                                                                                                                      tuple
                                                                                                                         .Item2
                                                                                                                      ?? $"${{{tuple.Item1}}}"
                                                                                                                     )
                                                                                                     )
                                                                              )
                                                                 ) ;
            l.Attributes.Add (
                              new JsonAttribute (
                                                 "properties"
                                               , new JsonLayout
                                                 {
                                                     IncludeAllProperties = true
                                                   , MaxRecursionLimit    = 3
                                                 }
                                               , false
                                                )
                             ) ;
            l.Attributes.Add (
                              new JsonAttribute (
                                                 "gdc"
                                               , new JsonLayout
                                                 {
                                                     IncludeGdc = true , MaxRecursionLimit = 3
                                                 }
                                               , false
                                                )
                             ) ;
            l.Attributes.Add (
                              new JsonAttribute (
                                                 "mdlc"
                                               , new JsonLayout
                                                 {
                                                     IncludeMdlc = true , MaxRecursionLimit = 3
                                                 }
                                               , false
                                                )
                             ) ;
            return l ;
        }

        /// <summary>
        /// </summary>
        /// <param name="rule2"></param>
        public static void AddRule ( LoggingRule rule2 )
        {
            LogManager.Configuration.LoggingRules.Insert ( 0 , rule2 ) ;
        }

        /// <summary>
        /// </summary>
        public static void Shutdown ( )
        {
            if ( ! _loggingConfigured )
            {
                return ;
            }

            _numTimesConfigured = null ;
            if ( _dict != null )
            {
                foreach ( var target in _dict.Where ( pair => pair.Value != null )
                                             .SelectMany ( pair => pair.Value )
                                             .Where ( target => target != null ) )
                {
                    target.Dispose ( ) ;
                    LogManager.Configuration.RemoveTarget ( target.Name ) ;
                }
            }

            LogManager.Configuration.LoggingRules.Clear ( ) ;
            LogManager.ReconfigExistingLoggers ( ) ;
            LogManager.Shutdown ( ) ;
        }

        [ DllImport ( "kernel32.dll" ) ]
        private static extern void OutputDebugString ( string lpOutputString ) ;

        #region Target Methods
        /// <summary>Adds the supplied target to the current NLog configuration.</summary>
        /// <param name="target">The target.</param>
        /// <param name="minLevel"></param>
        /// <param name="addRules"></param>
        public static void AddTarget ( Target target , LogLevel minLevel , bool addRules = true )
        {
            LogAddTarget ( target ) ;
            if ( minLevel == null )
            {
                minLevel = LogLevel.Trace ;
            }


            LogManager.Configuration.AddTarget ( target ) ;

            if ( addRules )
            {
                LogManager.Configuration.AddRule ( minLevel , LogLevel.Fatal , target ) ;
                LogManager.LogFactory.ReconfigExistingLoggers ( ) ;
            }
        }

        /// <summary>Removes a target by name from the current NLog configuration.</summary>
        /// <param name="name">The name of the target to remove.</param>
        public static void RemoveTarget ( string name )
        {
            LogManager.Configuration.RemoveTarget ( name ) ;
            LogManager.Configuration.LogFactory.ReconfigExistingLoggers ( ) ;
        }
        #endregion
    }
}