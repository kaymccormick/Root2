#if VSSETTINGS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjLib
{
    public interface IVsInstanceCollector
    {
        IList < IVsInstance > CollectVsInstances ( );
    }
}

#endif