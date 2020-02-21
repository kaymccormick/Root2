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
using System ;
using System.Collections.Generic;

namespace ProjLib
{
    public class VsInstanceCollector : IVsInstanceCollector
    {
        private readonly Func < IVsInstance > _insFunc ;

        private IVsInstanceCollector vsInstanceCollectorImplementation ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public VsInstanceCollector ( Func < IVsInstance > insFunc )
        {
            _insFunc = insFunc ;
                vsInstanceCollectorImplementation = new VsCollector(insFunc);
        }

        public IList<IVsInstance> CollectVsInstances() { return vsInstanceCollectorImplementation.CollectVsInstances(); }
    }
}