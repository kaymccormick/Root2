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
using System.ComponentModel ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Threading ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged , IAppState
    {
        VisualStudioInstancesCollection VsCollection { get ; } //ObservableCollection<VsInstance> ;

        MyProjectLoadProgress CurrentProgress { get ; }

        ObservableCollection < LogInvocation > LogInvocations { get ; }

        IPipelineViewModel PipelineViewModel { get ; }

        Visibility BrowserVisibility { get ; set ; }

        Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
          , IMruItem               sender2SelectedItem
          , TaskFactory            factory
          , SynchronizationContext current
        ) ;

        Task < object > ProcessSolutionAsync (
            Dispatcher                      dispatcher
          , TaskFactory                     taskFactory
          , Func < object , FormattedCode > getFormattedCode
        ) ;
    }
}