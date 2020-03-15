using System;
using System.AddIn.Pipeline ;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels ;
using System.Text;
using System.Threading.Tasks;
using KayMcCormick.Dev.Logging ;
using Leaf1Contract ;
using LeafHVA1 ;
using NLog ;

namespace Leaf1HostSideAdapter
{
    [HostAdapter]
    public class Leaf1ContractToViewHostSideAdapter : IService1
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly IService1Contract _service1Contract;
        private          ContractHandle    _handle;

        public Leaf1ContractToViewHostSideAdapter(IService1Contract service1Contract)
        {
            _service1Contract = service1Contract;
            _handle           = new ContractHandle(_service1Contract);
        }

        #region Implementation of IService1
        public void PerformFunc1()
        {
            _service1Contract.PerformFunc1();
        }
        #endregion

        public bool Start ( )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured ( message => { }, new AppLoggingConfiguration() { MinLogLevel = LogLevel.Trace, NLogViewerPort = 1233, ChainsawPort = 4111 } ) ;
            Logger.Debug ( "start" ) ;
            return _service1Contract.Start ( ) ;
        }

        public bool Stop ( ) { return _service1Contract.Stop ( ) ; }

        public bool Pause ( ) { return _service1Contract.Pause ( ) ; }

        public bool Continue ( ) { return _service1Contract.Continue ( ) ; }

        public bool Shutdown ( ) { return _service1Contract.Shutdown ( ) ; }


    }
}
