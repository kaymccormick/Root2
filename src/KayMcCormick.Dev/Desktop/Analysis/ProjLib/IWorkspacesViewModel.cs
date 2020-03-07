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
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Collections.Specialized ;
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
        VisualStudioInstancesCollection VsCollection { get ; } //ObservableCollection<VsInstance> ;

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

        IProjectBrowserViewModoel ProjectBrowserViewModel { get ; }

        PipelineResult PipelineResult { get ; set ; }

        Task AnalyzeCommand (
           object                  viewCurrentItem
        ) ;
        string ApplicationMode { get ; }

        AdhocWorkspace Workspace { get ; set ; }

        ObservableCollection < LogEventInfo > EventInfos { get ; set ; }

        ObservableCollection<string> Events { get ; set ; }
    }

    public interface IBrowserNodeCollection : ICollection<IBrowserNode>, INotifyCollectionChanged, INotifyPropertyChanged
    {
    }

    class BrowserNodeCollection : ObservableCollection<IBrowserNode>, IBrowserNodeCollection
    {
    }

    public interface IBrowserNode
    {
        string Name { get ; }
    }

    public class BrowserNode : IBrowserNode
    {
        private string _name ;
        #region Implementation of IBrowserNode
        public string Name { get => _name ; set => _name = value ; }
        #endregion
    }

    public class ProjectBrowserNode : BrowserNode, IProjectBrowserNode, IBrowserNode
    {
        private string _solutionPath ;
        private string _platform ;

        public string RepositoryUrl { get ; set ; }

        public string Platform { get => _platform ; set => _platform = value ; }

        public string SolutionPath { get { return _solutionPath ; } set { _solutionPath = value ; } }
    }

    public interface IProjectBrowserNode : IBrowserNode
    {
        string RepositoryUrl { get ; }
        string Platform { get ; }
        string SolutionPath { get ; set ; }
    }
}