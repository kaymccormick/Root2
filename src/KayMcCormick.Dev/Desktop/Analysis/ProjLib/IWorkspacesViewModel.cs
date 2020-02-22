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
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged//, ISupportInitialize
    {
        VisualStudioInstancesCollection
            VsCollection
        { get; } //ObservableCollection<VsInstance> ;

        MyProjectLoadProgress CurrentProgress { get ; }
        Task LoadSolutionAsync ( VsInstance vsSelectedItem , IMruItem sender2SelectedItem ) ;
    }
}