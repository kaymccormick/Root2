using System ;
using System.AddIn.Pipeline ;
using KayMcCormick.Dev.Logging ;
using Leaf1Contract ;
using LeafHVA1 ;
using NLog ;
using NLog.Config ;
using NLog.Layouts ;
using NLog.Targets ;

namespace Leaf1HostSideAdapter
{
    [ HostAdapter ]
    public class Leaf1ContractToViewHostSideAdapter : IService1 , IDisposable
    {
        private static   Logger            Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly IService1Contract _service1Contract ;
        private readonly ContractHandle    _handle ;

        private readonly string
            loggerName = typeof ( Leaf1ContractToViewHostSideAdapter ).FullName ;

        public Leaf1ContractToViewHostSideAdapter ( IService1Contract service1Contract )
        {
            _service1Contract = service1Contract ;
            _handle           = new ContractHandle ( _service1Contract ) ;
        }


        #region IDisposable
        public void Dispose ( ) { _handle?.Dispose ( ) ; }
        #endregion

        #region Implementation of IService1
        public void PerformFunc1 ( ) { _service1Contract.PerformFunc1 ( ) ; }
        #endregion

        public bool Start ( )
        {
            var target = new ChainsawTarget ( "new" )
                         {
                             Address = new SimpleLayout ( "udp://10.25.0.102:4111" )
                         } ;
            AppLoggingConfigHelper.AddTarget ( target , LogLevel.Trace , false ) ;
            AppLoggingConfigHelper.AddRule (
                                            new LoggingRule ( loggerName , LogLevel.Trace , target )
                                           ) ;
            LogManager.ReconfigExistingLoggers ( ) ;
            // AppLoggingConfigHelper.EnsureLoggingConfigured ( message => {Console.WriteLine(message); }, new AppLoggingConfiguration() { MinLogLevel = LogLevel.Trace, NLogViewerPort = 12333, ChainsawPort = 4111 } ) ;
            Logger = LogManager.GetLogger ( loggerName ) ;
            Logger.Debug ( "start" ) ;
            return _service1Contract.Start ( ) ;
        }

        public bool Stop ( ) { return _service1Contract.Stop ( ) ; }

        public bool Pause ( ) { return _service1Contract.Pause ( ) ; }

        public bool Continue ( ) { return _service1Contract.Continue ( ) ; }

        public bool Shutdown ( ) { return _service1Contract.Shutdown ( ) ; }
    }
}