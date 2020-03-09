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
using System.ComponentModel ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog ;

namespace ProjLib
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged , IAppState
    {
        #if VSSETTINGS
        VisualStudioInstancesCollection VsCollection { get ; } //ObservableCollection<VsInstance> ;
        #endif
        MyProjectLoadProgress CurrentProgress { get ; }

        ObservableCollection < ILogInvocation > LogInvocations { get ; }

        IPipelineViewModel PipelineViewModel { get ; }

#if false
        Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
          , IMruItem               sender2SelectedItem
          , TaskFactory            factory
          , SynchronizationContext current
        ) ;
#endif

        IProjectBrowserViewModel ProjectBrowserViewModel { get ; }

        PipelineResult PipelineResult { get ; set ; }

        Task AnalyzeCommand (
           object                  viewCurrentItem
        ) ;
        string ApplicationMode { get ; }

        AdhocWorkspace Workspace { get ; set ; }

        ObservableCollection < LogEventInfo > EventInfos { get ;  }

        ObservableCollection<LogEventInstance> Events { get ; set ; }
    }
}