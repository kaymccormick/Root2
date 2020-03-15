using System;
using System.AddIn.Pipeline ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf1Contract ;
using LeafHVA1 ;

namespace Leaf1HostSideAdapter
{
    [HostAdapter]
    public class Leaf1ContractToViewHostSideAdapter : IService1
    {
        private readonly IService1Contract _service1Contract ;
        private ContractHandle _handle ;

        public Leaf1ContractToViewHostSideAdapter (IService1Contract service1Contract )
        {
            _service1Contract = service1Contract ;
            _handle = new ContractHandle(_service1Contract);
        }

        #region Implementation of IService1
        public void PerformFunc1 ( )
        {
            _service1Contract.PerformFunc1();
        }
        #endregion
    }
}
