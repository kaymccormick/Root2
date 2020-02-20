#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// VsInstanceCollector.cs
// 
// 2020-02-19-7:02 AM
// 
// ---
#endregion
using System.Collections.Generic;

namespace ProjLib
{
    public class VsInstanceCollector : IVsInstanceCollector
    {
        private IVsInstanceCollector vsInstanceCollectorImplementation = new VsCollector();
        public IList<IVsInstance> CollectVsInstances() { return vsInstanceCollectorImplementation.CollectVsInstances(); }
    }
}