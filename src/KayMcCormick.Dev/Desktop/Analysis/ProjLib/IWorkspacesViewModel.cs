#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// IWorkspacesViewModel.cs
// 
// 2020-02-19-11:29 AM
// 
// ---
#endregion
using System.ComponentModel;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged, ISupportInitialize
    {
        VisualStudioInstancesCollection
            VsCollection
        { get; } //ObservableCollection<VsInstance> ;
    }
}