using System ;
using System.AddIn.Pipeline ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using Leaf1Contract ;
using ServiceAddIn1 ;

namespace Leaf1AddInSideAdapter
{
    [ AddInAdapter ]
    public class Leaf1ViewToContractAddInSideAdapter : ContractBase , IService1Contract
    {
        private readonly IService1 _leaf1 ;

        public Leaf1ViewToContractAddInSideAdapter ( IService1 leaf1 ) : base ( )
        {
            _leaf1 = leaf1 ;
        }

        #region Implementation of IService1Contract
        public void PerformFunc1 ( )
        {
            _leaf1.PerformFunc1();
        }
        #endregion
    }
}