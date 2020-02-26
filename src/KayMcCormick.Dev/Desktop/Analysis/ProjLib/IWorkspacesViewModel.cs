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
using System.Collections.ObjectModel ;
using System.ComponentModel;
using System.Threading.Tasks ;
using System.Windows.Threading ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged//, ISupportInitialize
    {
        VisualStudioInstancesCollection
            VsCollection
        { get; } //ObservableCollection<VsInstance> ;

        MyProjectLoadProgress CurrentProgress { get ; }

        ObservableCollection < LogInvocation > LogInvocations { get ; }

        bool Processing { get ; set ; }

        string CurrentProject { get ; set ; }

        string CurrentDocumentPath { get ; set ; }

        Task LoadSolutionAsync ( VsInstance vsSelectedItem , IMruItem sender2SelectedItem ) ;
        Task ProcessSolutionAsync (Dispatcher dispatcher ) ;
    }
}