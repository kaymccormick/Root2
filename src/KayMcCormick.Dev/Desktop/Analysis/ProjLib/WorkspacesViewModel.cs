﻿#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// WorkspacesViewModel.cs
// 
// 2020-02-19-7:26 AM
// 
// ---
#endregion
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog ;
using ProjLib.Properties ;
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Microsoft.TeamFoundation.TestManagement.WebApi.Legacy ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel
      , ISupportInitialize
      , INotifyPropertyChanged
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate (
            Tuple < SyntaxTree , SemanticModel , CompilationUnitSyntax > tuple
        ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate2 ( ) ;

        // private IList<IVsInstance> vsInstances;
        private readonly IVsInstanceCollector vsInstanceCollector ;

        public IPipelineViewModel PipelineViewModel { get ; }

        private string                _currentDocumentPath ;
        private MyProjectLoadProgress _currentProgress ;
        private string                _currentProject ;

        private ProjectHandlerImpl        _handler ;
        private bool                      _processing ;
        private IProjectBrowserViewModoel _projectBrowserViewModel ;
        private PipelineResult            _pipelineResult ;
        private string _applicationMode = "Runtime mode" ;

        public WorkspacesViewModel (
            IVsInstanceCollector      collector
          , IPipelineViewModel        pipelineViewModel
          , IProjectBrowserViewModoel projectBrowserViewModel
        )
        {
            vsInstanceCollector      = collector ;
            _projectBrowserViewModel = projectBrowserViewModel ;
            PipelineViewModel        = pipelineViewModel ;
        }

        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit ( )
        {
            var vsInstances = vsInstanceCollector.CollectVsInstances ( ) ;
            foreach ( var vsInstance in vsInstances )
            {
                VsCollection.Add ( vsInstance ) ;
            }

            OnPropertyChanged ( nameof ( VsCollection ) ) ;
        }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( ) { }

        public MyProjectLoadProgress CurrentProgress
        {
            get => _currentProgress ;
            set
            {
                _currentProgress = value ;
                OnPropertyChanged ( nameof ( CurrentProgress ) ) ;
            }
        }

        public VisualStudioInstancesCollection VsCollection { get ; } =
            new VisualStudioInstancesCollection ( ) ;

#if false
        public async Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
      , IMruItem               sender2SelectedItem
      , TaskFactory            factory1
      , SynchronizationContext current
        )
        {
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances ( ) ;
            var i = visualStudioInstances.Single (
                                                  instance => instance.VisualStudioRootPath
                                                              == vsSelectedItem.InstallationPath
                                                 ) ;
            if ( sender2SelectedItem != null )
            {
                _handler = new ProjectHandlerImpl ( sender2SelectedItem.FilePath , i , current ) ;
                _handler.ProcessProject +=
                    ( workspace , project ) => CurrentProject = project.Name ;
                _handler.ProcessDocument += document => {
                    CurrentDocumentPath = document.RelativePath ( ) ;
                } ;
                _handler.progressReporter = new MyProgress ( this ) ;
                await _handler.LoadAsync ( ) ;

                foreach ( var currentSolutionProject in _handler.Workspace.CurrentSolution.Projects
                )
                {
                    LogManager.GetCurrentClassLogger ( )
                              .Info ( "Current {project}" , currentSolutionProject.Name ) ;

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
                    await factory1.StartNew (
                                             ( ) => sender2SelectedItem.ProjectCollection.Add (
                                                                                               new
                                                                                                   AppProjectInfo (
                                                                                                                   currentSolutionProject
                                                                                                                      .Name
                                                                                                             , currentSolutionProject
                                                                                                                      .FilePath
                                                                                                             , currentSolutionProject
                                                                                                                  .Documents
                                                                                                                  .Count ( )
                                                                                                                  )
                                                                                              )
                                            )
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
                                  .ConfigureAwait ( false ) ;
                }
            }

            return new object ( ) ;
        }

#endif

        public IProjectBrowserViewModoel ProjectBrowserViewModel
        {
            get => _projectBrowserViewModel ;
            set => _projectBrowserViewModel = value ;
        }

        public async Task AnalyzeCommand ( object viewCurrentItem )
        {
            PipelineResult = new PipelineResult(ResultStatus.Pending);
            var actionBlock = new ActionBlock < ILogInvocation > (
                                                                  invocation => {
                                                                      Logger.Debug (
                                                                                    "{invocation}"
                                                                                  , invocation
                                                                                   ) ;
                                                                  }
                                                                 ) ;
            PipelineViewModel.Pipeline.PipelineInstance.LinkTo (
                                                                actionBlock
                                                              , new DataflowLinkOptions ( )
                                                                {
                                                                    PropagateCompletion = true
                                                                }
                                                               ) ;
            var projectBrowserNode = ( IProjectBrowserNode ) viewCurrentItem ;
            await PipelineViewModel.Pipeline.PipelineInstance
                                   .SendAsync ( projectBrowserNode.RepositoryUrl )
                                   .ConfigureAwait ( true ) ;
            var result = await actionBlock.Completion.ContinueWith (
                                                                    task => {
                                                                        Logger.Info (
                                                                                     "received task {fault}"
                                                                                   , task.IsFaulted
                                                                                    ) ;
                                                                        if ( task.IsFaulted )
                                                                        {
                                                                            return new
                                                                                PipelineResult (
                                                                                                ResultStatus
                                                                                                   .Failed
                                                                                              , task
                                                                                                   .Exception
                                                                                               ) ;
                                                                        }
                                                                        else
                                                                        {
                                                                            return new
                                                                                PipelineResult (
                                                                                                ResultStatus
                                                                                                   .Success
                                                                                               ) ;
                                                                        }
                                                                    }
                                                                  , CancellationToken.None
                                                                  , TaskContinuationOptions.None
                                                                  , TaskScheduler.Current)
                                                                  
                                          .ConfigureAwait ( true ) ;
            PipelineResult = result ;
            Logger.Info("{id} {result}", Thread.CurrentThread.ManagedThreadId, result.Status);
        }

        public string ApplicationMode => _applicationMode ;


        public PipelineResult PipelineResult
        {
            get => _pipelineResult ;
            set
            {
                _pipelineResult = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public bool Processing
        {
            get => _processing ;
            set
            {
                _processing = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentProject
        {
            get => _currentProject ;
            set
            {
                _currentProject = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentDocumentPath
        {
            get => _currentDocumentPath ;
            set
            {
                _currentDocumentPath = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ObservableCollection < ILogInvocation > LogInvocations { get ; } =
            new ObservableCollection < ILogInvocation > ( ) ;

        public event PropertyChangedEventHandler PropertyChanged ;

        private void DoSomething ( ) { }

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    public class PipelineResult
    {
        public ResultStatus Status { get ; }

        public Exception TaskException { get ; }

        public PipelineResult ( ResultStatus status , Exception taskException = null )
        {
            Status        = status ;
            TaskException = taskException ;
        }
    }

    public enum ResultStatus { Failed , Success, Pending , None }

    public class MyProjectLoadProgress
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public MyProjectLoadProgress (
            string   filePath
          , string   operation
          , string   targetFramework
          , TimeSpan elapsedTime
        )
        {
            FilePath        = filePath ;
            Operation       = operation ;
            TargetFramework = targetFramework ;
            ElapsedTime     = elapsedTime ;
        }

        /// <summary>
        ///     The project for which progress is being reported.
        /// </summary>
        public string FilePath { get ; }

        public string FileName => Path.GetFileNameWithoutExtension ( FilePath ) ;

        /// <summary>
        ///     The operation that has just completed.
        /// </summary>
        public string Operation { get ; }

        /// <summary>
        ///     The target framework of the project being built or resolved. This
        ///     property is only valid for SDK-style projects
        ///     during the <see cref="ProjectLoadOperation.Resolve" /> operation.
        /// </summary>
        public string TargetFramework { get ; }

        /// <summary>
        ///     The amount of time elapsed for this operation.
        /// </summary>
        public TimeSpan ElapsedTime { get ; }
    }

    public class MyProgress : IProgress < ProjectLoadProgress >
    {
        private readonly WorkspacesViewModel _workspacesViewModel ;

        public MyProgress ( WorkspacesViewModel workspacesViewModel )
        {
            _workspacesViewModel = workspacesViewModel ;
        }

        /// <summary>Reports a progress update.</summary>
        /// <param name="value">The value of the updated progress.</param>
        public void Report ( ProjectLoadProgress value )
        {
            _workspacesViewModel.CurrentProgress = new MyProjectLoadProgress (
                                                                              value.FilePath
                                                                            , value
                                                                             .Operation.ToString ( )
                                                                            , value.TargetFramework
                                                                            , value.ElapsedTime
                                                                             ) ;
        }
    }
}