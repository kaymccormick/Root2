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
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows.Threading ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged , IAppState
        //, ISupportInitialize
    {
        VisualStudioInstancesCollection
            VsCollection
        { get; } //ObservableCollection<VsInstance> ;

        MyProjectLoadProgress CurrentProgress { get ; }

        ObservableCollection < LogInvocation > LogInvocations { get ; }

        ITargetBlock < string > DataflowHead { get ; set ; }

        IPropagatorBlock < string , Workspace > ToWorkspaceTransformBlock { get ; }

        Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
          , IMruItem               sender2SelectedItem
          , TaskFactory            factory
          , SynchronizationContext current
        ) ;
        Task < object > ProcessSolutionAsync ( Dispatcher dispatcher , TaskFactory taskFactory,  Func <object, FormattedCode> getFormattedCode ) ;
    }
}