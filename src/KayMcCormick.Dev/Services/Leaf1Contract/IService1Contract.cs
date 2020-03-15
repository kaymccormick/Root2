using System.AddIn.Contract ;
using System.AddIn.Pipeline ;

namespace Leaf1Contract
{
    [AddInContract]
    public interface IService1Contract : IContract
    {
        void PerformFunc1 ( ) ;
    }
}
