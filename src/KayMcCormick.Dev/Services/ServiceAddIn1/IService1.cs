using System;
using System.AddIn.Pipeline ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAddIn1
{
 
    [AddInBase]
    public interface IService1
    {
        bool Start();
        bool Stop();
        bool Pause();
        bool Continue();
        bool Shutdown();
        void PerformFunc1();
    }

    
}
