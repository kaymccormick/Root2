#region header
// Kay McCormick (mccor)
// 
// Deployment
// LogService
// LeafService1.cs
// 
// 2020-03-10-3:24 AM
// 
// ---
#endregion
using Common.Logging ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.ServiceImpl ;
using LeafHVA1 ;
using NLog ;
using NLog.Fluent ;
using NLog.Targets ;
using System ;
using System.AddIn.Hosting ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Runtime.Serialization ;
using System.ServiceModel ;
using System.ServiceModel.Channels ;
using System.Threading ;
using System.Timers ;
using JetBrains.Annotations ;
using NLog.LogReceiverService ;
using Topshelf ;
using LogLevel = NLog.LogLevel ;
using LogManager = NLog.LogManager ;
using Timer = System.Timers.Timer ;

namespace LeafService
{
    internal class LeafService1 : IDisposable
    {
        private static readonly Logger Logger        = LogManager.GetLogger ( "RelayLogger" ) ;
        private static readonly Logger ServiceLogger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        ILog   _commonLogger ;
#pragma warning disable CS0649 // Field 'LeafService1._centralService' is never assigned to, and will always have its default value null
        private CentralService _centralService ;
#pragma warning restore CS0649 // Field 'LeafService1._centralService' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'LeafService1._svcHost' is never assigned to, and will always have its default value null
        private ServiceHost _svcHost ;
#pragma warning restore CS0649 // Field 'LeafService1._svcHost' is never assigned to, and will always have its default value null
        private ServiceHost        _svcReceiver ;
        private Timer              _timer ;
        private List < IService1 > _services = new List < IService1 > ( ) ;
        private AddInProcess _addInProcess ;

        public LeafService1 ( ILog commonLogger ) { _commonLogger = commonLogger ; }

        public Thread MyThread { get ; set ; }

        #region IDisposable
        public void Dispose ( )
        {
            _timer?.Dispose ( ) ;
            ( ( IDisposable ) _svcHost )?.Dispose ( ) ;
            ( ( IDisposable ) _svcReceiver )?.Dispose ( ) ;
            _centralService?.Dispose ( ) ;
        }
        #endregion

        // ReSharper disable once UnusedParameter.Global
        public bool Start ( HostControl hostControl )
        {
            InitializeAddin ( ) ;


            var f = new LogFactory < MyLogger > ( ) ;
            Trace.Refresh ( ) ;
            Trace.Listeners.Add (
                                 new NLogTraceListener
                                 {
                                     AutoLoggerName = false
                                   , Filter         = new LeafServiceTraceFilter ( )
                                   , Name           = "test1"
                                   , TraceOutputOptions =
                                         TraceOptions.Callstack | TraceOptions.LogicalOperationStack
                                   , LogFactory = f
                                 }
                                ) ;


            var svcTarget = AppLoggingConfigHelper.ServiceTarget ;
            if ( svcTarget != null )
            {
                Logger.Debug (
                              "Existing service target endpoing is {endpoingAddress}"
                            , ( svcTarget as LogReceiverWebServiceTarget )?.EndpointAddress
                             ) ;
                AppLoggingConfigHelper.RemoveTarget ( svcTarget ) ;
            }

            Log.Info ( $"{nameof ( LeafService1 )} Start command received." ) ;

            InitializeWfcServices ( ) ;

            InitializeTimer ( ) ;
            InitializeEventLogListener ( ) ;
            return true ;
        }

        private void InitializeEventLogListener ( )
        {
            var e = new EventLog ( "Application" ) ;
            e.EntryWritten        += EOnEntryWritten ;
            e.EnableRaisingEvents =  true ;
        }

        private void InitializeTimer ( )
        {
            _timer         =  new Timer ( 60 * 1000 ) { AutoReset = true } ;
            _timer.Elapsed += TimerOnElapsed ;
            _timer.Start ( ) ;
        }

        private void InitializeWfcServices ( )
        {
            var baseAddress = new Uri (
                                       $"http://{Dns.GetHostName ( )}:8737/discovery/scenarios/logreceiver/"
                                      ) ;
            Logger.Info ( baseAddress ) ;

            //Uri baseAddress = new Uri ( $"http://10.25.0.102:8737/leafService/" ) ;
            //Uri receiverUri = new Uri ( baseAddress , "receiver" ) ;

            var svcReceiver = new ServiceHost ( typeof ( LogReceiver ) , baseAddress ) ;
            // Add calculator endpoint
            // svcReceiver.AddServiceEndpoint(typeof(ILogReceiverServer), new WSHttpBinding(), String.Empty);
            svcReceiver.Faulted += SvcReceiverOnFaulted ;
            svcReceiver.UnknownMessageReceived += ( sender , args ) => {
                _commonLogger.Warn ( nameof ( svcReceiver.UnknownMessageReceived ) ) ;
                _commonLogger.Info ( $"From: {args.Message.Headers.From}" ) ;
                _commonLogger.Info ( $"To: {args.Message.Headers.To}" ) ;
                _commonLogger.Info ( $"Via: {args.Message.Properties.Via}" ) ;
                _commonLogger.Info ( $"Action: {args.Message.Headers.Action}" ) ;
            } ;
            svcReceiver.Opened += ( sender , args )
                => _commonLogger.Info ( nameof ( svcReceiver.Opened ) ) ;
            // ** DISCOVERY ** //
            // Make the service discoverable by adding the discovery behavior
            // var discoveryBehavior = new ServiceDiscoveryBehavior();
            // svcReceiver.Description.Behaviors.Add(discoveryBehavior);
            // discoveryBehavior.AnnouncementEndpoints.Add(
            // new UdpAnnouncementEndpoint());
            // ** DISCOVERY ** //
            // Add the discovery endpoint that specifies where to publish the services
            // svcReceiver.AddServiceEndpoint(new UdpDiscoveryEndpoint());
            svcReceiver.Open ( ) ;
            _svcReceiver = svcReceiver ;

#if true
            _centralService = new CentralService ( ) ;
            var serviceHost = new ServiceHost (
                                               _centralService
                                             , new Uri ( "http://localhost:8737/CentralSvc1" )
                                              ) ;

            // Add calculator endpoint
            serviceHost.AddServiceEndpoint (
                                            typeof ( ICentralService )
                                          , new BasicHttpBinding ( )
                                          , string.Empty
                                           ) ;

            // ** DISCOVERY ** //
            // Make the service discoverable by adding the discovery behavior
#if CENTRALDISC
            var discoveryBehavior1 = new ServiceDiscoveryBehavior() ;
            serviceHost.Description.Behaviors.Add(discoveryBehavior);
            discoveryBehavior1.AnnouncementEndpoints.Add(
                                                        new UdpAnnouncementEndpoint());
            // ** DISCOVERY ** //
            // Add the discovery endpoint that specifies where to publish the services
            serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());
#endif
            // Open the ServiceHost to create listeners and start listening for messages.
            serviceHost.Open ( ) ;
            serviceHost.Opened += ( sender , args ) => Logger.Info ( "central open" ) ;


            _svcHost = serviceHost ;
#endif


#if USEOWNCONFIG
            var conf = new LoggingConfiguration() ;
            var target = new LogReceiverWebServiceTarget ( "wcf" )
                         {
                             EndpointAddress = "http://serenity/LogService/ReceiveLogs.svc"
                         } ;

            //"http://localhost:27809/ReceiveLogs.svc";
            conf.AddTarget ( target ) ;
            conf.AddRuleForAllLevels(target);
            var console = new ConsoleTarget ("console" ) ;
            conf.AddTarget ( console ) ;
            conf.AddRuleForAllLevels(console);
            LogManager.Configuration = conf;
#endif
        }

        private void InitializeAddin ( )
        {
            var baseDir = Environment.CurrentDirectory ;
            var pipelineDir = "Pipeline" ;
            var root = Path.Combine ( baseDir , pipelineDir ) ;
            var warnings = AddInStore.Update ( root ) ;
            Logger.Info ( "pipeline directory is {dir}" , root ) ;
            foreach ( var warning in warnings )
            {
                Logger.Warn ( warning ) ;
            }

            // if ( warnings.Any())
            // {

            // var message = string.Join("\n", warnings) ;
            // Logger.Fatal ( message ) ;
            // _commonLogger.Fatal(message);
            // throw new AddInException (message);
            // }

            var tokens = AddInStore.FindAddIns ( typeof ( IService1 ) , root ) ;
            _commonLogger.Debug ( $"add ins count {tokens.Count}" ) ;
            if ( ! tokens.Any ( ) )
            {
                throw new AddInException ( "No addins found" ) ;
            }

            _addInProcess = new AddInProcess ( ) ;
            _addInProcess.ShuttingDown += ( sender , args )
                => _commonLogger.Warn ( "Shutting down adin process" ) ;

#if false
            PermissionSet set = new PermissionSet(PermissionState.None);
            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            set.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
            set.AddPermission ( new SocketPermission (PermissionState.Unrestricted ) ) ;
            set.AddPermission ( new EnvironmentPermission 
                                    ( PermissionState.Unrestricted ) ) ;
            set.AddPermission ( new SqlClientPermission ( PermissionState.Unrestricted ) ) ;
            var p1 =
 @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\addin2\build\service\Debug\x86\Pipeline\AddIns\Service1V1";

            set.AddPermission (
                               new FileIOPermission (
                                                     FileIOPermissionAccess.PathDiscovery | FileIOPermissionAccess.Read 
                                               , AccessControlActions.None
                                               , p1
                                                    )
                               
                              ) ;
            set.AddPermission (
                               new FileIOPermission (
                                                     FileIOPermissionAccess.AllAccess
                                               , AccessControlActions.None
                                               , @"C:\data"
                                                    )
                              ) ;
            Console.WriteLine(set);
            byte[] key = new byte[]
                         {
                             0 , 36 , 0 , 0 , 4 , 128
                       , 0 , 0 , 148 , 0 , 0 , 0
                       , 6 , 2 , 0 , 0 , 0 , 36
                       , 0 , 0 , 82 , 83 , 65 , 49
                       , 0 , 4 , 0 , 0 , 1 , 0
                       , 1 , 0 , 119 , 35 , 145 , 230
                       , 60 , 16 , 71 , 40 , 173 , 207
                       , 24 , 226 , 57 , 4 , 116 , 38
                       , 37 , 89 , 250 , 127 , 52 , 164
                       , 33 , 88 , 72 , 244 , 50 , 136
                       , 205 , 232 , 117 , 220 , 201 , 42
                       , 6 , 34 , 46 , 155 , 224 , 89
                       , 43 , 33 , 31 , 247 , 74 , 219
                       , 181 , 210 , 26 , 122 , 171 , 85
                       , 34 , 181 , 64 , 177 , 115 , 95
                       , 47 , 3 , 39 , 146 , 33 , 5
                       , 111 , 237 , 190 , 126 , 83 , 64
                       , 115 , 218 , 190 , 233 , 219 , 72
                       , 248 , 236 , 235 , 207 , 29 , 201
                       , 138 , 149 , 87 , 110 , 69 , 203
                       , 239 , 245 , 254 , 124 , 72 , 66
                       , 133 , 148 , 81 , 171 , 45 , 174
                       , 122 , 131 , 112 , 241 , 178 , 247
                       , 165 , 41 , 210 , 202 , 33 , 14
                       , 62 , 132 , 77 , 151 , 53 , 35
                       , 215 , 61 , 25 , 61 , 246 , 193
                       , 127 , 19 , 20 , 166
                         } ;
            var a1 = typeof ( AppLoggingConfigHelper ).Assembly.GetName ( ) ;
            var key1 = a1.GetPublicKey ( ) ;
            var name1 = a1.Name ;
            var version1 = a1.Version ;
            var sn1 = new StrongName ( new StrongNamePublicKeyBlob(key1), name1, version1);
            var assembly = typeof ( Logger ).Assembly ;
            var name = assembly.GetName ( ).Name ;
            var version = assembly.GetName ( ).Version ;
            Console.WriteLine(assembly.FullName);
            StrongName xx = new StrongName (
                                            new StrongNamePublicKeyBlob ( key )
                                      , name
                                      , version
                                           ) ;
#endif
            foreach ( var addInToken in tokens )
            {
                // var code = addInToken.AssemblyName.CodeBase;
                // Console.WriteLine(code);
#if false
                ActivationContext ac = AppDomain.CurrentDomain.ActivationContext;
                ApplicationIdentity ai = ac?.Identity;
                Console.WriteLine(ai?.FullName);
                var applicationBase =
 @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\addin2\build\service\Debug\x86\Pipeline\AddIns\Service1V1";
                var applicationIdentity = new ApplicationIdentity("test") ;
                var appDomainSetup = new AppDomainSetup ( ) ;
                appDomainSetup.ApplicationBase = applicationBase ;
                Console.WriteLine(xx);
                Console.WriteLine(sn1);
                // appDomainSetup.ApplicationBase = applicationBase;
                AppDomain x = AppDomain.CreateDomain (
                "add in domain"
            , null
            , appDomainSetup
            , set
              ,xx
                                                     ) ;
                
                // x.FirstChanceException += ( sender , args )
                 // => _commonLogger.Error ( args.Exception.ToString ( ) ) ;
                 AppDomain.CurrentDomain.FirstChanceException += ( sender , args )
                     => _commonLogger.Error ( args.Exception.ToString ( ) ) ;
#endif
                var service1 =
                    addInToken.Activate < IService1 > ( _addInProcess , AddInSecurityLevel.FullTrust ) ;
                var addInController = AddInController.GetAddInController ( service1 ) ;
                Console.WriteLine(_addInProcess.IsCurrentProcess);
                Console.WriteLine(_addInProcess.ProcessId);
                //Console.WriteLine(addInController.AppDomain.BaseDirectory);
                _services.Add ( service1 ) ;
                bool success ;
                try
                {
                    success = service1.Start ( ) ;
                }
                catch ( Exception ex )
                {
                    throw new AddInException ( "Addin failed to start" + ex.ToString ( ) ) ;
                }

                if ( ! success )
                {
                    throw new AddInException ( "Addin failed to start" ) ;
                }
            }
        }

        private void Initiate ( IService1 service1 ) { service1.PerformFunc1 ( ) ; }

        private void SvcReceiverOnFaulted ( object sender , EventArgs e )
        {
            _commonLogger.Error ( "faulted" ) ;
        }

        private void TimerOnElapsed ( object sender , ElapsedEventArgs e )
        {
            Logger.Debug ( "Timer elapsed" ) ;
            var proc = Process.GetProcessById ( _addInProcess.ProcessId ) ;
            Logger.Warn ( "proc is {proc}", proc ) ;
        }

        private void EOnEntryWritten ( object sender , EntryWrittenEventArgs e )
        {
            var typ = e.Entry.EntryType ;
            var level = LogLevel.Info ;
            switch ( typ )
            {
                case EventLogEntryType.Error :
                    level = LogLevel.Error ;
                    break ;
                case EventLogEntryType.FailureAudit :
                    level = LogLevel.Error ;
                    break ;
                case EventLogEntryType.SuccessAudit :
                    level = LogLevel.Info ;
                    break ;
                case EventLogEntryType.Warning :
                    level = LogLevel.Warn ;
                    break ;
                case EventLogEntryType.Information :
                    level = LogLevel.Info ;
                    break ;
            }

            var props = new Dictionary < string , object >
                        {
                            [ "Category" ]           = e.Entry.Category
                          , [ "CategoryNumber" ]     = e.Entry.CategoryNumber
                          , [ "EncodedData" ]        = Convert.ToBase64String ( e.Entry.Data )
                          , [ "MachineName" ]        = e.Entry.MachineName
                          , [ "Index" ]              = e.Entry.Index
                          , [ "EntryType" ]          = e.Entry.EntryType
                          , [ "ReplacementStrings" ] = e.Entry.ReplacementStrings
                          , [ "Source" ]             = e.Entry.Source
                          , [ "InstanceId" ]         = e.Entry.InstanceId
                          , [ "UserName" ]           = e.Entry.UserName
                          , [ "TimeGenerated" ]      = e.Entry.TimeGenerated
                          , [ "TimeWritten" ]        = e.Entry.TimeWritten
                        } ;


            var l = new LogBuilder ( Logger )
                   .LoggerName ( $"{e.Entry.MachineName}.{e.Entry.Source}" )
                   .Level ( level )
                   .Message ( e.Entry.Message )
                   .Properties ( props ) ;
            l.Write ( l.LogEventInfo.FormattedMessage ) ;
        }

        // ReSharper disable once UnusedParameter.Global
        public bool Stop ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Stop command received." ) ;

            foreach ( var service1 in _services )
            {
                service1.Stop ( ) ;
            }

            //TODO: Implement your service stop routine.
            return true ;
        }

        // ReSharper disable once UnusedParameter.Global
        public bool Pause ( HostControl hostControl )
        {
            foreach ( var service1 in _services )
            {
                service1.Pause ( ) ;
            }

            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Pause command received." ) ;
            _timer.Stop ( ) ;
            //TODO: Implement your service start routine.
            return true ;
        }

        // ReSharper disable once UnusedParameter.Global
        public bool Continue ( HostControl hostControl )
        {
            foreach ( var service1 in _services )
            {
                service1.Continue ( ) ;
            }

            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Continue command received." ) ;

            _timer.Start ( ) ;
            return true ;
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        // ReSharper disable once UnusedParameter.Global
        public bool Shutdown ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Shutdown command received." ) ;
            foreach ( var service1 in _services )
            {
                service1.Shutdown ( ) ;
                ( service1 as IDisposable )?.Dispose ( ) ;
            }

            //TODO: Implement your service stop routine.
            return true ;
        }
    }

    public class AddInException : Exception
    {
        public AddInException ( ) { }

        public AddInException ( string message ) : base ( message ) { }

        public AddInException ( string message , Exception innerException ) : base (
                                                                                    message
                                                                                  , innerException
                                                                                   )
        {
        }

        protected AddInException ( [ NotNull ] SerializationInfo info , StreamingContext context ) :
            base ( info , context )
        {
        }
    }

    internal class LeafServiceTraceFilter : TraceFilter
    {
        #region Overrides of TraceFilter
        public override bool ShouldTrace (
            TraceEventCache cache
          , string          source
          , TraceEventType  eventType
          , int             id
          , string          formatOrMessage
          , object[]        args
          , object          data1
          , object[]        data
        )
        {
            return true ;
        }
        #endregion
    }

    [ ServiceBehavior ( AddressFilterMode = AddressFilterMode.Any ) ]
    internal class LogReceiver : ILogReceiverServer
    {
        private static readonly ILog CLogger =
            Common.Logging.LogManager.GetLogger ( typeof ( LeafService1 ) ) ;

        public LogReceiver ( ) { CLogger.Warn ( "In constructor" ) ; }

        #region Implementation of ILogReceiverServer
        public void ProcessLogMessages ( NLogEvents nEvents )
        {
            CLogger.Warn ( $"Received {nEvents.Events.Length} events" ) ;
            var context = OperationContext.Current ;
            var properties = context.IncomingMessageProperties ;
            var endpoint =
                properties[ RemoteEndpointMessageProperty.Name ] as RemoteEndpointMessageProperty ;
            var address = string.Empty ;
            //http://www.simosh.com/article/ddbggghj-get-client-ip-address-using-wcf-4-5-remoteendpointmessageproperty-in-load-balanc.html
            if ( properties.Keys.Contains ( HttpRequestMessageProperty.Name ) )
            {
                if ( properties[ HttpRequestMessageProperty.Name ] is HttpRequestMessageProperty
                         endpointLoadBalancer
                     && endpointLoadBalancer.Headers[ "X-Forwarded-For" ] != null )
                {
                    address = endpointLoadBalancer.Headers[ "X-Forwarded-For" ] ;
                }
            }

            if ( string.IsNullOrEmpty ( address ) )
            {
                if ( endpoint != null )
                {
                    address = endpoint.Address ;
                }
            }

            var events = nEvents.ToEventInfo ( "Client." + address + "." ) ;
            Debug.WriteLine ( "in: {0} {1}" , nEvents.Events.Length , events.Count ) ;

            foreach ( var ev in events )
            {
                var logger = LogManager.GetLogger ( ev.LoggerName ) ;
                logger.Log ( ev ) ;
            }
        }
        #endregion
    }
}
