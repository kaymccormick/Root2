/* test 123 */

#if NETFRAMEWORK
using Castle.DynamicProxy ;
using System.ServiceModel.Discovery ;
#endif
using JetBrains.Annotations ;
using NLog ;
using NLog.Common ;
using NLog.Config ;
using NLog.Layouts ;
using NLog.Targets ;
using NLog.Targets.Wrappers ;
using System ; /* test 123 */
/* test 123 */
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
/* test 123 */
using System.Diagnostics ;
using System.Diagnostics.CodeAnalysis ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.ServiceModel ;
using System.Text ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Text.RegularExpressions ;
#if NETFRAMEWORK
using NLog.LogReceiverService ;
#endif
using JsonAttribute = NLog.Layouts.JsonAttribute ;

namespace KayMcCormick.Dev.Logging
{
#if NETFRAMEWORK
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
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                Console.WriteLine ( "\nNo services are found." ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
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
        /// <summary>The string writer</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for StringWriter
        public static StringWriter Writer { get ; set ; }

        private const string JsonTargetName              = "json_out" ;
        private const string DisableLogTargetsEnvVarName = "DISABLE_LOG_TARGETS" ;
        private const string PublicFacingHostAddress     = "xx1.mynetgear.com:" ;

        // ReSharper disable once InconsistentNaming
        [ SuppressMessage ( "Microsoft.Performance" , "CA1823:AvoidUnusedPrivateFields" ) ]
        [ UsedImplicitly ] private static Logger Logger ;

        [ ThreadStatic ]
        private static int ? _numTimesConfigured ;

        private static LogFactory _factory ;

        /// <summary>
        /// 
        /// </summary>
#if ENABLE_WCF_TARGET
        public static LogReceiverWebServiceTarget ServiceTarget { get; private set; }
#endif


        /// <summary>Gets or sets a value indicating whether [debugger target enabled].</summary>
        /// <value>
        ///   <see language="true"/> if [debugger target enabled]; otherwise, <see language="false"/>.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebuggerTargetEnabled
        public static bool DebuggerTargetEnabled { get ; } = false ;


        /// <summary>Gets or sets a value indicating whether [logging is configured].</summary>
        /// <value>
        ///   <see language="true"/> if [logging is configured]; otherwise, <see language="false"/>.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for LoggingIsConfigured
        public static bool LoggingIsConfigured { get ; set ; }

        /// <summary>Gets or sets a value indicating whether [dump existing configuration].</summary>
        /// <value>
        ///   <see language="true"/> if [dump existing configuration]; otherwise, <see language="false"/>.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DumpExistingConfig
        public static bool DumpExistingConfig { get ; } = true ;


        /// <summary>Gets or sets a value indicating whether [force code configuration].</summary>
        /// <value>
        ///   <see language="true"/> if [force code configuration]; otherwise, <see language="false"/>.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ForceCodeConfig
        public static bool ForceCodeConfig { get ; } = false ;

        private static void DoLogMessage ( string message )
        {
            System.Diagnostics.Debug.WriteLine (
                                                nameof ( AppLoggingConfigHelper ) + ":" + message
                                               ) ;
            // System.Diagnostics.Debug.WriteLine ( nameof(AppLoggingConfigHelper) + ":" + message ) ;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        internal static LogFactory ConfigureLogging (
            LogDelegates.LogMethod logMethod
          , bool                   proxyLogging = false
          , ILoggingConfiguration  config1      = null
        )
        {
            if ( config1 == null )
            {
                config1 = AppLoggingConfiguration.Default ;
            }

            InternalLogging ( ) ;

            if ( Environment.GetEnvironmentVariable ( "DISABLE_LOGGING" ) != null )
            {
                LogManager.Configuration = new CodeConfiguration ( ) ;
                return LogManager.LogFactory ;
            }

#if ENABLE_PROXYLOG
            LogFactory proxiedFactory = null;
            if (proxyLogging)
            {
                var proxyGenerator = new ProxyGenerator();
                var loggerProxyHelper = new LoggerProxyHelper(proxyGenerator, DoLogMessage);
                var logFactory = new MyLogFactory(DoLogMessage);
                var lConfLogFactory = loggerProxyHelper.CreateLogFactory(logFactory);
                proxiedFactory = lConfLogFactory;
            }
#endif
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
                if (proxyLogging)
                {
                    fieldInfo.SetValue(null, proxiedFactory);
                    var newVal = fieldInfo.GetValue(null);
                    logMethod($"New Value = {newVal}");
                }
#endif
            }

#if ENABLE_PROXYLOG
            return PerformConfiguration(logMethod, proxyLogging, proxiedFactory);
#else
            return PerformConfiguration ( logMethod , false , null , config1) ;
#endif
        }

        private static LogFactory PerformConfiguration (
            LogDelegates.LogMethod logMethod
          , bool                   proxyLogging
          , LogFactory             proxiedFactory
          , ILoggingConfiguration  config1
        )
        {
            var useFactory = proxyLogging ? proxiedFactory : LogManager.LogFactory ;
            var lConf = new CodeConfiguration ( useFactory ) ;

            var dict = LogLevel.AllLoggingLevels.ToDictionary (
                                                               level => level
                                                             , level => new List < Target > ( )
                                                              ) ;
            // ReSharper disable once UnusedVariable
            var errorTargets = dict[ LogLevel.Error ] ;
            var t = dict[ LogLevel.Trace ] ;


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

            if ( config1.IsEnabledEventLogTarget )
            {
                var x = EventLogTarget ( ) ;
                errorTargets.Add ( x ) ;
            }

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
            dict[LogLevel.Debug].Add(ServiceTarget);
#endif


            if ( config1.IsEnabledConsoleTarget )
            {

                var consoleTarget = new ConsoleTarget ( "console" )
                                    {
                                        Error = true
                                      , Layout =
                                            new SimpleLayout ( "${level} ${message} ${logger}" )
                                    } ;
                dict[ LogLevel.Warn ].Add ( consoleTarget ) ;
            }


            #region Cache Target
            if ( config1.IsEnabledCacheTarget )
            {
                var cacheTarget = new MyCacheTarget ( ) ;
                dict[ LogLevel.Debug ].Add ( cacheTarget ) ;
                var cacheTarget2 = new MyCacheTarget2 ( ) { Layout = SetupJsonLayout ( ) } ;
                dict[ LogLevel.Debug ].Add ( cacheTarget2 ) ;
            }
            #endregion
            #region NLogViewer Target
            var viewer = Viewer ( ) ;
            t.Add ( viewer ) ;
            #endregion
            #region Debugger Target
            if ( DebuggerTargetEnabled )
            {
                // var debuggerTarget =
                // new DebuggerTarget { Layout = new SimpleLayout("${message}") };
                // t.Add(debuggerTarget);
            }
            #endregion
            #region Chainsaw Target
            var chainsawTarget = new ChainsawTarget ( ) ;
            var PublicHostAddress = PublicFacingHostAddress ;
            SetupNetworkTarget ( chainsawTarget , $"udp://{PublicHostAddress}4445" ) ;
            t.Add ( chainsawTarget ) ;
            #endregion
            t.Add ( MyFileTarget ( ) ) ;
            var jsonFileTarget = JsonFileTarget ( ) ;
            dict[ LogLevel.Debug ].Add ( jsonFileTarget ) ;
            var byType = new Dictionary < Type , int > ( ) ;
            var keys = dict.Keys.ToList ( ) ;
            foreach ( var k in keys )
            {
                var v = dict[ k ] ;
                dict[ k ] = v.Where ( ( target , i ) => ! disabled.Contains ( target.Name ) )
                             .ToList ( ) ;
            }

            foreach ( var target in dict.SelectMany ( pair => pair.Value ) )
            {
                var type = target.GetType ( ) ;
                byType.TryGetValue ( type , out var count ) ;
                count          += 1 ;
                byType[ type ] =  count ;

                if ( target.Name == null )
                {
                    target.Name = $"{Regex.Replace ( type.Name , "Target" , "" )}{count:D2}" ;
                }

                lConf.AddTarget ( target ) ;
            }

            foreach ( var result in dict.Select ( LoggingRule ) )
            {
                foreach ( var loggingRule in result )
                {
                    lConf.LoggingRules.Add ( loggingRule ) ;
                }

                // ((List<LoggingRule>lConf.LoggingRules)).AddRange ( result ) ;
            }

            LogManager.Configuration = lConf ;
            Logger                   = LogManager.GetCurrentClassLogger ( ) ;
            return useFactory ;
        }

        private static EventLogTarget EventLogTarget ( )
        {
            var x = new EventLogTarget ( "eventLog" ) { Source = "Application Error" } ;
            return x ;
        }

        private static IEnumerable < LoggingRule > LoggingRule (
            KeyValuePair < LogLevel , List < Target > > arg
        )
        {
            return arg.Value.Select ( target => new LoggingRule ( "*" , arg.Key , target ) ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private static LoggingRule DefaultLoggingRule ( Target target )
        {
            return new LoggingRule ( "*" , LogLevel.FromOrdinal ( 0 ) , target ) ;
        }

        private static void InternalLogging ( )
        {
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
            return new NLogViewerTarget ( name )
                   {
                       Address              = new SimpleLayout ( "udp://10.25.0.102:9995" )
                     , IncludeAllProperties = true
                     , IncludeCallSite      = true
                     , IncludeSourceInfo    = true
                   } ;
        }

        /// <summary>JSON File Target</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for JsonFileTarget
        public static FileTarget JsonFileTarget ( )
        {
            var f = new FileTarget ( JsonTargetName )
                    {
                        FileName = Layout.FromString ( @"c:\data\logs\${processName}.json" )
                      , Layout   = new MyJsonLayout ( )
                    } ;

            return f ;
        }

        /// <summary>My File Target.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for MyFileTarget
        public static FileTarget MyFileTarget ( )
        {
            var f = new FileTarget
                    {
                        Name     = "text_log"
                      , FileName = Layout.FromString ( @"c:\data\logs\log.txt" )
                      , Layout   = Layout.FromString ( "${message}" )
                    } ;

            return f ;
        }

        /// <summary>Removes the target.</summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RemoveTarget
        // ReSharper disable once UnusedMember.Global
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
        /// <param name="logMethod">The log method.</param>
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
        [ SuppressMessage ( "ReSharper" , "UnusedParameter.Global" ) ]
        public static ILogger EnsureLoggingConfigured (
            LogDelegates.LogMethod    logMethod      = null
          , ILoggingConfiguration     config1        = null
          , [ CallerFilePath ] string callerFilePath = null
        )
        {
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

            logMethod (
                       $"[time {_numTimesConfigured.Value}]\t{nameof ( EnsureLoggingConfigured )} called from {callerFilePath}"
                      ) ;


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

            if ( fieldInfo2 != null )
            {
                var config = fieldInfo2.GetValue ( LogManager.LogFactory ) ;

                //LogManager.ThrowConfigExceptions = true;
                //LogManager.ThrowExceptions = true;
                var fieldInfo = LogManager.LogFactory.GetType ( )
                                          .GetField (
                                                     "_configLoaded"
                                                   , BindingFlags.Instance | BindingFlags.NonPublic
                                                    ) ;

                bool configLoaded ;
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
                var isMyConfig =
                    ! configLoaded || LogManager.Configuration is CodeConfiguration ;
                var doConfig = ! LoggingIsConfigured || ForceCodeConfig && ! isMyConfig ;
                logMethod (
                           $"{nameof ( LoggingIsConfigured )} = {LoggingIsConfigured}; {nameof ( ForceCodeConfig )} = {ForceCodeConfig}; {nameof ( isMyConfig )} = {isMyConfig});"
                          ) ;
                if ( DumpExistingConfig )
                {
                    void Collect ( string s ) { System.Diagnostics.Debug.WriteLine ( s ) ; }

                    DoDumpConfig ( Collect ) ;
                }

                if ( doConfig )
                {
                    var factory = ConfigureLogging ( logMethod , false , config1 ) ;
                    _factory = factory ;
                }
            }

            DumpPossibleConfig ( LogManager.Configuration ) ;
            return _factory.GetLogger ( "DefaultLogger" ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collect"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary < string , object > DoDumpConfig ( Action < string > collect )
        {
            var config = LogManager.Configuration ;
            if ( config == null )
            {
                return null ;
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


        // ReSharper disable once UnusedParameter.Local
#pragma warning disable IDE0060 // Remove unused parameter
        private static void Debug ( string s ) { }
#pragma warning restore IDE0060 // Remove unused parameter
        #region Target Methods
        /// <summary>Adds the supplied target to the current NLog configuration.</summary>
        /// <param name="target">The target.</param>
        /// <param name="minLevel"></param>
        // ReSharper disable once RedundantNameQualifier
        // ReSharper disable once UnusedMember.Global
        public static void AddTarget ( NLog.Targets.Target target , LogLevel minLevel )
        {
            if ( minLevel == null )
            {
                minLevel = LogLevel.Trace ;
            }

            LogManager.Configuration.AddTarget ( target ) ;

            LogManager.Configuration.AddRule ( minLevel , LogLevel.Fatal , target ) ;

            LogManager.LogFactory.ReconfigExistingLoggers ( ) ;
        }

        /// <summary>Removes a target by name from the current NLog configuration.</summary>
        /// <param name="name">The name of the target to remove.</param>
        // ReSharper disable once UnusedMember.Global
        public static void RemoveTarget ( string name )
        {
            LogManager.Configuration.RemoveTarget ( name ) ;
            LogManager.Configuration.LogFactory.ReconfigExistingLoggers ( ) ;
        }
        #endregion

        /// <summary>Set up a <seealso cref="NLog.Layouts.JsonLayout"/> for json loggers.</summary>
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
                                               , new JsonLayout ( )
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
                                               , new JsonLayout ( )
                                                 {
                                                     IncludeGdc = true , MaxRecursionLimit = 3
                                                 }
                                               , false
                                                )
                             ) ;
            l.Attributes.Add (
                              new JsonAttribute (
                                                 "mdlc"
                                               , new JsonLayout ( )
                                                 {
                                                     IncludeMdlc = true , MaxRecursionLimit = 3
                                                 }
                                               , false
                                                )
                             ) ;
            return l ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule2"></param>
        public static void AddRule ( LoggingRule rule2 )
        {
            LogManager.Configuration.LoggingRules.Insert ( 0 , rule2 ) ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonTypeConverter : JsonConverter < Type >
    {
        #region Overrides of JsonConverter<Type>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Type Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            [ NotNull ] Utf8JsonWriter writer
          , [ NotNull ] Type           value
          , JsonSerializerOptions      options
        )
        {
            if ( writer == null )
            {
                throw new ArgumentNullException ( nameof ( writer ) ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStringValue ( value.FullName ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class DictConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeToConvert == typeof ( IDictionary < object , object > ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new Inner ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Inner : JsonConverter < IDictionary < object , object > >
        {
            #region Overrides of JsonConverter<IDictionary<object,object>>
            public override IDictionary < object , object > Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                IDictionary < object , object > dict = new Dictionary < object , object > ( ) ;
                while ( reader.Read ( ) )
                {
                    if ( reader.TokenType == JsonTokenType.EndObject )
                    {
                        return dict ;
                    }

                    if ( reader.TokenType != JsonTokenType.PropertyName )
                    {
                        throw new JsonException ( ) ;
                    }

                    var propertyName = reader.GetString ( ) ;
                    var value = JsonSerializer.Deserialize < object > ( ref reader , options ) ;
                    dict[ propertyName ] = value ;
                }

                return dict ;
            }

            public override void Write (
                Utf8JsonWriter                  writer
              , IDictionary < object , object > value
              , JsonSerializerOptions           options
            )
            {
                writer.WriteStartObject ( ) ;
                foreach ( var keyValuePair in value )
                {
                    writer.WritePropertyName ( keyValuePair.Key.ToString ( ) ) ;
                    JsonSerializer.Serialize (
                                              writer
                                            , keyValuePair.Value
                                            , keyValuePair.Value.GetType ( )
                                            , options
                                             ) ;
                }

                writer.WriteEndObject ( ) ;
            }
            #endregion
        }
        #endregion
    }
}