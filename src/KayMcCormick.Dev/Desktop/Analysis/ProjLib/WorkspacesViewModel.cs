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
using NLog ;
using ProjLib.Properties ;
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Data.SqlClient ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Microsoft.TeamFoundation.TestManagement.WebApi.Legacy ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel
#if VSSETTINGS
      , ISupportInitialize
        #endif
      , INotifyPropertyChanged
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate (
            Tuple < SyntaxTree , SemanticModel , CompilationUnitSyntax > tuple
        ) ;

        public delegate IFormattedCode CreateFormattedCodeDelegate2 ( ) ;

        // private IList<IVsInstance> vsInstances;
#if VSSETTINGS
                public VisualStudioInstancesCollection VsCollection { get ; } =
            new VisualStudioInstancesCollection ( ) ;

        private readonly IVsInstanceCollector vsInstanceCollector ;
#endif
        public IPipelineViewModel PipelineViewModel { get ; }

        private string                _currentDocumentPath ;
        private MyProjectLoadProgress _currentProgress ;
        private string                _currentProject ;

        private bool                      _processing ;
        private IProjectBrowserViewModoel _projectBrowserViewModel ;
        private readonly SqlConnection _sqlConn ;
        private PipelineResult            _pipelineResult ;
        private string _applicationMode = "Runtime mode" ;
        private AdhocWorkspace _workspace ;

        public WorkspacesViewModel (
            #if VSSETTINGS
            IVsInstanceCollector      collector
          , 
            
            #endif
            IPipelineViewModel        pipelineViewModel
          , IProjectBrowserViewModoel projectBrowserViewModel
            , SqlConnection sqlConn
        )
        {
            #if VSSETTINGS
            vsInstanceCollector      = collector ;
#endif
            _projectBrowserViewModel = projectBrowserViewModel ;
            _sqlConn = sqlConn ;
            PipelineViewModel        = pipelineViewModel ;
        }

#if VSSETTINGS
        /// <summary>Signals the object that initialization is starting.</summary>
        /// 
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
#endif
        public MyProjectLoadProgress CurrentProgress
        {
            get => _currentProgress ;
            set
            {
                _currentProgress = value ;
                OnPropertyChanged ( nameof ( CurrentProgress ) ) ;
            }
        }

    
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
                                                                      Logger.Warn(
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
            AnalysisRequest req = new AnalysisRequest();
            req.Info = projectBrowserNode ;
            Logger.Trace("About to await sendasync on pipeline");
            await PipelineViewModel.Pipeline.PipelineInstance
                                   .SendAsync ( req )
                                   .ConfigureAwait ( true ) ;
            Logger.Trace("back from await sendasync on pipeline");
            PipelineResult result;
            try
            {
                await actionBlock.Completion.ConfigureAwait(true);
                result =            new                                          
                    PipelineResult(
                                   ResultStatus
                                      .Success
                                  );

            }
            catch ( Exception ex )
            {
                Logger.Debug ( ex , $"actionTask completion threw exception {ex.Message}" ) ;
                result = new
                    PipelineResult(
                                   ResultStatus
                                      .Failed, ex);
            }
        
        if ( result.Status == ResultStatus.Failed )
            {
                Logger.Error (
                              result.TaskException
                            , "Failed: {}"
                            , result.TaskException.Message
                             ) ;
            }
            Logger.Debug("{id} {result}", Thread.CurrentThread.ManagedThreadId, result.Status);
        }

        private ObservableCollection < LogEventInfo > eventInfos  = new ObservableCollection < LogEventInfo > ();
        private ObservableCollection < string > _events  = new ObservableCollection < string > ();

        public string ApplicationMode => _applicationMode ;

        public AdhocWorkspace Workspace { get => _workspace ; set => _workspace = value ; }


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

        public ObservableCollection < LogEventInfo > EventInfos
        {
            get => eventInfos ;
            set => eventInfos = value ;
        }

        public ObservableCollection < string > Events { get => _events ; set => _events = value ; }

        public event PropertyChangedEventHandler PropertyChanged ;

        private void DoSomething ( ) { }

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}