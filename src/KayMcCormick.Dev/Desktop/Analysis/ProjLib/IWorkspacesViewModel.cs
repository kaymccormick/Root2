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
    }

    public struct DesignWorkspacesViewModel : IWorkspacesViewModel
    {
        private bool _processing ;
        private string _currentProject ;
        private string _currentDocumentPath ;
        private VisualStudioInstancesCollection _vsCollection ;
        private MyProjectLoadProgress _currentProgress ;
        private ObservableCollection < ILogInvocation > _logInvocations ;
        private IPipelineViewModel _pipelineViewModel ;
        private IProjectBrowserViewModoel _projectBrowserViewModel ;
        private PipelineResult _pipelineResult ;
        private string _applicationMode ;
        private AdhocWorkspace _workspace ;
        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged ;
        #endregion

        #region Implementation of IAppState
        public bool Processing { get => _processing ; set => _processing = value ; }

        public string CurrentProject { get => _currentProject ; set => _currentProject = value ; }

        public string CurrentDocumentPath { get => _currentDocumentPath ; set => _currentDocumentPath = value ; }
        #endregion

        #region Implementation of IWorkspacesViewModel
        public VisualStudioInstancesCollection VsCollection { get => _vsCollection ; set => _vsCollection = value ; }

        public MyProjectLoadProgress CurrentProgress { get => _currentProgress ; set => _currentProgress = value ; }

        public ObservableCollection < ILogInvocation > LogInvocations { get => _logInvocations ; set => _logInvocations = value ; }

        public IPipelineViewModel PipelineViewModel { get => _pipelineViewModel ; set => _pipelineViewModel = value ; }

        public IProjectBrowserViewModoel ProjectBrowserViewModel { get => _projectBrowserViewModel ; set => _projectBrowserViewModel = value ; }

        public PipelineResult PipelineResult { get => _pipelineResult ; set => _pipelineResult = value ; }

        public Task AnalyzeCommand ( object viewCurrentItem ) { return null ; }

        public string ApplicationMode => _applicationMode = "Design Mode" ;

        public AdhocWorkspace Workspace { get => _workspace ; set => _workspace = value ; }
        #endregion
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
        public string RepositoryUrl { get ; set ; }
    }

    public interface IProjectBrowserNode : IBrowserNode
    {
        string RepositoryUrl { get ; }
    }
}