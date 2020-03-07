#if VSSETTINGS
#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// VisualStudioInstancesCollection.cs
// 
// 2020-02-19-11:29 AM
// 
// ---
#endregion
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjLib
{
    public class VisualStudioInstancesCollection : ObservableCollection<IVsInstance>
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class.</summary>
        public VisualStudioInstancesCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified list.</summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="list" /> parameter cannot be <see langword="null" />.</exception>
        public VisualStudioInstancesCollection(List<IVsInstance> list) : base(list)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> parameter cannot be <see langword="null" />.</exception>
        public VisualStudioInstancesCollection(IEnumerable<IVsInstance> collection) : base(collection)
        {
        }
    }


}
#endif
