#region header
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
using Microsoft.CodeAnalysis ;
using NLog ;
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
#if !NETSTANDARD2_0
using System.Text.Json ;
#endif
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework.Interfaces ;
using KayMcCormick.Dev ;
using ProjLib.Interfaces ;
using JetBrains.Annotations ;

namespace ProjLib
{
    public sealed class WorkspacesViewModel : IWorkspacesViewModel
#if VSSETTINGS
, ISupportInitialize
#endif
      , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private          string                   _currentDocumentPath ;
        private          string                   _currentProject ;
        private          bool                     _processing ;
        private          IProjectBrowserViewModel _projectBrowserViewModel ;
        private readonly Pipeline                 _pipeline ;
        private          PipelineResult           _pipelineResult ;
        private          string                   _applicationMode = "Runtime mode" ;
        private          AdhocWorkspace           _workspace ;

        public WorkspacesViewModel (
            IProjectBrowserViewModel projectBrowserViewModel
          , Pipeline                 pipeline
        )
        {
            _projectBrowserViewModel = projectBrowserViewModel ;
            _pipeline                = pipeline ;
        }

        public IProjectBrowserViewModel ProjectBrowserViewModel
        {
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }

        public async Task AnalyzeCommand ( object viewCurrentItem )
        {
            PipelineResult = new PipelineResult ( ResultStatus.Pending ) ;
            var actionBlock = new ActionBlock < ILogInvocation > ( LogInvocationAction ) ;

            var pipeline = _pipeline ;
            if ( pipeline == null )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "Pipeline is null" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            pipeline.BuildPipeline ( ) ;
            var pInstance = pipeline.PipelineInstance ;
            if ( pInstance == null )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "pipeline instance is null" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            pInstance.LinkTo (
                              actionBlock
                            , new DataflowLinkOptions ( ) { PropagateCompletion = true }
                             ) ;
            var projectBrowserNode = ( IProjectBrowserNode ) viewCurrentItem ;
            var req = new AnalysisRequest { Info = projectBrowserNode } ;
            Logger.Trace ( "About to post on pipeline" ) ;
            if ( ! pInstance.Post ( req ) )
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new AnalyzeException ( "Post failed" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            await HandlePipelineResultAsync ( actionBlock ) ;
        }

        private void LogInvocationAction ( [ NotNull ] ILogInvocation invocation )
        {
            if ( invocation == null )
            {
                throw new ArgumentNullException ( nameof ( invocation ) ) ;
            }
#if !NETSTANDARD2_0
            Console.WriteLine ( JsonSerializer.Serialize ( invocation ) ) ;
#endif
            LogInvocations.Add ( invocation ) ;
            Logger.Debug ( "{invocation}" , invocation ) ;
        }

        private async Task HandlePipelineResultAsync ( ActionBlock < ILogInvocation > actionBlock )
        {
            PipelineResult result ;
            try
            {
                await actionBlock.Completion.ConfigureAwait ( true ) ;
                result = new PipelineResult ( ResultStatus.Success ) ;
            }
            catch ( AggregateException ex1 )
            {
                var exes = ex1.Flatten ( ).InnerExceptions ;
                Logger.Debug ( $"actionTask completion threw exception" ) ;
                foreach ( var exception in exes )
                {
                    Logger.Debug ( exception , exception.Message ) ;
                }

                result = new PipelineResult ( ResultStatus.Failed , ex1 ) ;
            }

            if ( result.Status == ResultStatus.Failed )
            {
                Logger.Error (
                              result.TaskException
                            , "Failed: {}"
                            , result.TaskException.Message
                             ) ;
            }

            Logger.Debug (
                          "{id} {result} {count}"
                        , Thread.CurrentThread.ManagedThreadId
                        , result.Status
                        , LogInvocations.Count
                         ) ;
        }

        private ObservableCollection < LogEventInstance > _events =
            new ObservableCollection < LogEventInstance > ( ) ;

        public string ApplicationMode { get { return _applicationMode ; } }

        public AdhocWorkspace Workspace { get { return _workspace ; } set { _workspace = value ; } }


        public PipelineResult PipelineResult
        {
            get { return _pipelineResult ; }
            set
            {
                _pipelineResult = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public bool Processing
        {
            get { return _processing ; }
            set
            {
                _processing = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentProject
        {
            get { return _currentProject ; }
            set
            {
                _currentProject = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public string CurrentDocumentPath
        {
            get { return _currentDocumentPath ; }
            set
            {
                _currentDocumentPath = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public LogInvocationCollection LogInvocations { get ; } = new LogInvocationCollection ( ) ;

        public ObservableCollection < LogEventInfo > EventInfos { get ; } =
            new ObservableCollection < LogEventInfo > ( ) ;

        public ObservableCollection < LogEventInstance > Events { get { return _events ; } }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}