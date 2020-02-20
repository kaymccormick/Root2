using Common.Logging ;
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text ;
using System.Threading ;
using System.Threading.Tasks ;
using NLog ;
using NLog.Config ;
using NLog.Fluent ;
using NLog.Targets ;
using Topshelf ;
using LogLevel = NLog.LogLevel ;
using LogManager = NLog.LogManager ;

namespace LogService.Service
{
    internal class WinService
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ILog Log { get ; private set ; }

        public WinService ( ILog logger )
        {
            // IocModule.cs needs to be updated in case new paramteres are added to this constructor

            if ( logger == null )
            {
                throw new ArgumentNullException ( nameof ( logger ) ) ;
            }

            Log = logger ;
        }

        public bool Start ( HostControl hostControl )
        {
            Log.Info ( $"{nameof ( WinService )} Start command received." ) ;

            var conf = new LoggingConfiguration() ;

            
            var  target = new LogReceiverWebServiceTarget("wcf") ;

            target.EndpointAddress = "http://serenity/LogService/ReceiveLogs.svc" ;//"http://localhost:27809/ReceiveLogs.svc";
            conf.AddTarget ( target ) ;
            conf.AddRuleForAllLevels(target);
            var console = new ConsoleTarget ("console" ) ;
            conf.AddTarget ( console ) ;
            conf.AddRuleForAllLevels(console);
            LogManager.Configuration = conf;
            var e = new EventLog("Application");
            e.EntryWritten += EOnEntryWritten;
            e.EnableRaisingEvents = true ;

            // MyThread = new Thread ( MainProc ) ;
            // MyThread.Start ( true ) ;
            //TODO: Implement your service start routine.
            return true ;
        }

        public Thread MyThread { get ; set ; }

        private void MainProc ( object obj )
        {
            Log.Info ( "in main proc" ) ;
            
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
            props[ "TimeGenerated" ] = e.Entry.TimeGenerated ;
            props[ "TimeWritten" ] = e.Entry.TimeWritten ;

            
            var l = new LogBuilder(Logger).LoggerName($"{e.Entry.MachineName}.{e.Entry.Source}").Level(level).Message(e.Entry.Message).Properties(props);
            l.Write(l.LogEventInfo.FormattedMessage);
        }

        public bool Stop ( HostControl hostControl )
        {
            Log.Trace ( $"{nameof ( WinService )} Stop command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }

        public bool Pause ( HostControl hostControl )
        {
            Log.Trace ( $"{nameof ( WinService )} Pause command received." ) ;

            //TODO: Implement your service start routine.
            return true ;
        }

        public bool Continue ( HostControl hostControl )
        {
            Log.Trace ( $"{nameof ( WinService )} Continue command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }

        public bool Shutdown ( HostControl hostControl )
        {
            Log.Trace ( $"{nameof ( WinService )} Shutdown command received." ) ;

            //TODO: Implement your service stop routine.
            return true ;
        }
    }
}
