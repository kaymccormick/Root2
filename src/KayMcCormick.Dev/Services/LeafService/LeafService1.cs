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
using System.Threading ;
using KayMcCormick.Dev.Interfaces ;
using NLog ;
using NLog.Config ;
using NLog.Fluent ;
using NLog.Targets ;
using Topshelf ;
using LogLevel = NLog.LogLevel ;
using LogManager = NLog.LogManager ;

namespace LeafService
{
    internal class LeafService1
    {
        private static Logger Logger        = LogManager.GetLogger ( "RelayLogger" ) ;
        private static Logger ServiceLogger = LogManager.GetCurrentClassLogger ( ) ;

        public LeafService1 ( ) { }

        public bool Start ( HostControl hostControl )
        {
            Log.Info ( $"{nameof ( LeafService1 )} Start command received." ) ;

            var service = new CentralService ( ) ;
            var svcHost = new ServiceHost ( service ) ;
            svcHost.Open ( ) ;

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
            var e = new EventLog ( "Application" ) ;
            e.EntryWritten        += EOnEntryWritten ;
            e.EnableRaisingEvents =  true ;

            // MyThread = new Thread ( MainProc ) ;
            // MyThread.Start ( true ) ;
            //TODO: Implement your service start routine.
            return true ;
        }

        public Thread MyThread { get ; set ; }

        private void MainProc ( object obj ) { Log.Info ( "in main proc" ) ; }

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

            //TODO: Implement your service start routine.
            return true ;
        }

        public bool Continue ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Continue command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }

        public bool Shutdown ( HostControl hostControl )
        {
            ServiceLogger.Trace ( $"{nameof ( LeafService1 )} Shutdown command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }
    }
}