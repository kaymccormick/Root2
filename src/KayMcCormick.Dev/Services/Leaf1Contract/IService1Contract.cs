using System.AddIn.Contract ;
using System.AddIn.Pipeline ;

namespace Leaf1Contract
{
    [ AddInContract ]
    public interface IService1Contract : IContract
    {
        bool Start ( ) ;
        bool Stop ( ) ;
        bool Pause ( ) ;
        bool Continue ( ) ;
        bool Shutdown ( ) ;
        void PerformFunc1 ( ) ;
    }
}