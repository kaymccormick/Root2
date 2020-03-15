using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafHVA1
{
    public interface IService1
    {
        bool Start();
        bool Stop();
        bool Pause();
        bool Continue();
        bool Shutdown();

        void PerformFunc1 ( ) ;
    }
}
