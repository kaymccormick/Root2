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
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Input ;
using System.Windows.Threading ;
using AnalysisFramework ;

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
        #if false
        Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
          , IMruItem               sender2SelectedItem
          , TaskFactory            factory
          , SynchronizationContext current
        ) ;
#endif

        Task < object > ProcessSolutionAsync (
            Dispatcher                      dispatcher
          , TaskFactory                     taskFactory
          , Func < object , FormattedCode > getFormattedCode
        ) ;

        IProjectBrowserViewModoel ProjectBrowserViewModel { get ; }

        void AnalyzeCommand (
           object                  viewCurrentItem
        ) ;
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