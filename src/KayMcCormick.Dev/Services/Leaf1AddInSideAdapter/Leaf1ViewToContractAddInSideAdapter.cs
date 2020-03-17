using System ;
using System.AddIn.Pipeline ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using KayMcCormick.Dev.Logging ;
using Leaf1Contract ;
using NLog ;
using ServiceAddIn1 ;

namespace Leaf1AddInSideAdapter
{
    [ AddInAdapter ]
    public class Leaf1ViewToContractAddInSideAdapter : ContractBase , IService1Contract
    {
        private readonly IService1 _contract;
#pragma warning disable 169
        private ILogger _logger ;
#pragma warning restore 169

        public Leaf1ViewToContractAddInSideAdapter ( IService1 leaf1 ) : base ( )
        {
            _contract = leaf1 ;
//            _logger = AppLoggingConfigHelper.EnsureLoggingConfigured ( ) ;
        }

        #region Implementation of IService1Contract
        public bool Start ( ) { return _contract.Start ( ) ; }

        public bool Stop ( ) { return _contract.Stop ( ) ; }

        public bool Pause ( ) { return _contract.Pause ( ) ; }

        public bool Continue ( ) { return _contract.Continue ( ) ; }

        public bool Shutdown ( ) { return _contract.Shutdown ( ) ; }

        public void PerformFunc1 ( )
        {
            _contract.PerformFunc1();
        }
        #endregion
    }
}