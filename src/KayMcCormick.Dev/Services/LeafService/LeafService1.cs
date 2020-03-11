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
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.ServiceModel ;
using System.ServiceModel.Channels ;
using System.ServiceModel.Discovery ;
using System.Threading ;
using System.Timers ;
using Common.Logging ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Logging ;
using NLog ;
using NLog.Fluent ;
using NLog.LogReceiverService ;
using Topshelf ;
using LogLevel = NLog.LogLevel ;
using LogManager = NLog.LogManager ;
using Timer = System.Timers.Timer ;

namespace LeafService
{
    internal class LeafService1 : IDisposable
    {
        private readonly ILog _commonLogger ;
        private static Logger Logger        = LogManager.GetLogger ( "RelayLogger" ) ;
        private static Logger ServiceLogger = LogManager.GetCurrentClassLogger ( ) ;
#pragma warning disable CS0649 // Field 'LeafService1._svcHost' is never assigned to, and will always have its default value null
        private ServiceHost _svcHost ;
#pragma warning restore CS0649 // Field 'LeafService1._svcHost' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'LeafService1._centralService' is never assigned to, and will always have its default value null
        private CentralService _centralService ;
#pragma warning restore CS0649 // Field 'LeafService1._centralService' is never assigned to, and will always have its default value null
        private Timer _timer ;
        private ServiceHost _svcReceiver ;

        public LeafService1 (ILog commonLogger ) { _commonLogger = commonLogger ; }

        public bool Start ( HostControl hostControl )
        {
            var f = new LogFactory < MyLogger > ( ) ;
            Trace.Refresh();
            Trace.Listeners.Add ( new NLogTraceListener ( )
                                  {
                                      AutoLoggerName = false, Filter = new LeafServiceTraceFilter (  ),
                                      Name = "test1", TraceOutputOptions = TraceOptions.Callstack | TraceOptions.LogicalOperationStack,
                                      LogFactory = f
                                  } ) ;
            var svcTarget = AppLoggingConfigHelper.ServiceTarget ;
            if(svcTarget != null) {
                Logger.Debug("Existing service target endpoing is {endpoingAddress}", svcTarget.EndpointAddress);
                AppLoggingConfigHelper.RemoveTarget(svcTarget);
            }
            Log.Info ( $"{nameof ( LeafService1 )} Start command received." ) ;

            Uri baseAddress = new Uri($"http://{System.Net.Dns.GetHostName()}:8737/discovery/scenarios/logreceiver/");
            Logger.Info ( baseAddress ) ;

            //Uri baseAddress = new Uri ( $"http://10.25.0.102:8737/leafService/" ) ;
            //Uri receiverUri = new Uri ( baseAddress , "receiver" ) ;

            var svcReceiver = new ServiceHost ( typeof ( LogReceiver ) , baseAddress) ;
            // Add calculator endpoint
            // svcReceiver.AddServiceEndpoint(typeof(ILogReceiverServer), new WSHttpBinding(), String.Empty);
            svcReceiver.Faulted += SvcReceiverOnFaulted;
            svcReceiver.UnknownMessageReceived += ( sender , args ) => {
                _commonLogger.Warn(nameof(svcReceiver.UnknownMessageReceived));
                _commonLogger.Info($"From: {args.Message.Headers.From}");
                _commonLogger.Info($"To: {args.Message.Headers.To}");
                _commonLogger.Info ( $"Via: {args.Message.Properties.Via}" ) ;
                _commonLogger.Info($"Action: {args.Message.Headers.Action}");
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
            svcReceiver.Open();
            _svcReceiver = svcReceiver ;

#if false
            _centralService = new CentralService ( ) ;
            var serviceHost = new ServiceHost ( _centralService , new Uri ( "http://localhost:8737/CentralSvc1" ) );

            // Add calculator endpoint
            serviceHost.AddServiceEndpoint(typeof(ICentralService), new WSHttpBinding(), string.Empty);

            // ** DISCOVERY ** //
            // Make the service discoverable by adding the discovery behavior
            var discoveryBehavior1 = new ServiceDiscoveryBehavior() ;
            serviceHost.Description.Behaviors.Add(discoveryBehavior);
            discoveryBehavior1.AnnouncementEndpoints.Add(
                                                        new UdpAnnouncementEndpoint());
            // ** DISCOVERY ** //
            // Add the discovery endpoint that specifies where to publish the services
            serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());

           // Open the ServiceHost to create listeners and start listening for messages.
            serviceHost.Open();


            _svcHost = serviceHost;
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
            _timer = new Timer ( 60 * 1000 ) { AutoReset = true } ;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start ( ) ;

            var e = new EventLog ( "Application" ) ;
            e.EntryWritten        += EOnEntryWritten ;
            e.EnableRaisingEvents =  true ;

            // MyThread = new Thread ( MainProc ) ;
            // MyThread.Start ( true ) ;
            //TODO: Implement your service start routine.
            return true ;
        }

        private void SvcReceiverOnFaulted ( object sender , EventArgs e )
        {
            _commonLogger.Error ( "faulted" ) ;
        }

        private void TimerOnElapsed ( object sender , ElapsedEventArgs e )
        {
            Logger.Debug ( "Timer elapsed" ) ;
        }

        public Thread MyThread { get ; set ; }

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

            var props = new Dictionary < string , object > ( ) ;
            props[ "Category" ]           = e.Entry.Category ;
            props[ "CategoryNumber" ]     = e.Entry.CategoryNumber ;
            props[ "EncodedData" ]        = Convert.ToBase64String ( e.Entry.Data ) ;
            props[ "MachineName" ]        = e.Entry.MachineName ;
            props[ "Index" ]              = e.Entry.Index ;
            props[ "EntryType" ]          = e.Entry.EntryType ;
            props[ "ReplacementStrings" ] = e.Entry.ReplacementStrings ;
            props[ "Source" ]             = e.Entry.Source ;
            props[ "InstanceId" ]         = e.Entry.InstanceId ;
            props[ "UserName" ]           = e.Entry.UserName ;
            props[ "TimeGenerated" ]      = e.Entry.TimeGenerated ;
            props[ "TimeWritten" ]        = e.Entry.TimeWritten ;


            var l = new LogBuilder ( Logger )
                   .LoggerName ( $"{e.Entry.MachineName}.{e.Entry.Source}" )
                   .Level ( level )
                   .Message ( e.Entry.Message )
                   .Properties ( props ) ;
            l.Write ( l.LogEventInfo.FormattedMessage ) ;
        }

        public bool Stop ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Stop command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }

        public bool Pause ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Pause command received." ) ;
            _timer.Stop();
            //TODO: Implement your service start routine.
            return true ;
        }

        public bool Continue ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Continue command received." ) ;

            _timer.Start();
            return true ;
        }

        public bool Shutdown ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Shutdown command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }

        #region IDisposable
        public void Dispose ( )
        {
            _timer?.Dispose();
            ( ( IDisposable ) _svcHost )?.Dispose ( ) ;
            _centralService?.Dispose ( ) ;
        }
        #endregion
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

    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    internal class LogReceiver : ILogReceiverServer
    {
        private static ILog CLogger =
            Common.Logging.LogManager.GetLogger ( typeof ( LeafService1 ) ) ;
        public LogReceiver () { CLogger.Warn ( "In constructor" ) ; }

        #region Implementation of ILogReceiverServer
        public void ProcessLogMessages ( NLogEvents nevents )
        {
            CLogger.Warn ( $"Received {nevents.Events.Length} events" ) ;
            var context = OperationContext.Current;
            var properties = context.IncomingMessageProperties;
            var endpoint =
                properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            var address = string.Empty;
            //http://www.simosh.com/article/ddbggghj-get-client-ip-address-using-wcf-4-5-remoteendpointmessageproperty-in-load-balanc.html
            if (properties.Keys.Contains(HttpRequestMessageProperty.Name))
            {
                if (properties[HttpRequestMessageProperty.Name] is HttpRequestMessageProperty
                        endpointLoadBalancer
                    && endpointLoadBalancer.Headers["X-Forwarded-For"] != null)
                {
                    address = endpointLoadBalancer.Headers["X-Forwarded-For"];
                }
            }

            if (string.IsNullOrEmpty(address))
            {
                address = endpoint.Address;
            }

            var events = nevents.ToEventInfo("Client." + address?.ToString() + ".");
            Debug.WriteLine("in: {0} {1}", nevents.Events.Length, events.Count);

            foreach (var ev in events)
            {
                var logger = LogManager.GetLogger(ev.LoggerName);
                logger.Log(ev);
            }
        }
        #endregion
    }
}
