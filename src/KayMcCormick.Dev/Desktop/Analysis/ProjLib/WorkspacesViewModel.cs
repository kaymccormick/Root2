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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel ;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Runtime.CompilerServices;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using System.Windows.Threading ;
using CodeAnalysisApp1 ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.VisualStudio.Settings;
using VSShell = Microsoft.VisualStudio.Shell ;
using NLog ;
using ProjLib.Properties ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel, ISupportInitialize, INotifyPropertyChanged
    {
        public Dispatcher _d ;
        // private IList<IVsInstance> vsInstances;
        private readonly VisualStudioInstancesCollection _vsCollection = new VisualStudioInstancesCollection() ;
        private MyProjectLoadProgress _currentProgress ;
        private ProjectHandlerImpl _handler ;
        private bool _processing ;
        private string _currentProject ;
        private string _currentDocumentPath ;
        private readonly IVsInstanceCollector vsInstanceCollector;

        public MyProjectLoadProgress CurrentProgress
        {
            get => _currentProgress;
            set
            {
                _currentProgress = value;
                OnPropertyChanged ( nameof ( CurrentProgress ) ) ;
            }
        }

        public WorkspacesViewModel(IVsInstanceCollector collector  )
        {
            vsInstanceCollector = collector;
            BeginInit();
        }

        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit()
        {
            var vsInstances = vsInstanceCollector.CollectVsInstances();
            foreach (var vsInstance in vsInstances)
            {
                VsCollection.Add(vsInstance);
            }
            OnPropertyChanged(nameof(VsCollection));

        }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( )
        {

        }

        public VisualStudioInstancesCollection VsCollection
        {
            get
            {
                return _vsCollection;
            }
        }

        public async Task < object > LoadSolutionAsync (
            VsInstance             vsSelectedItem
          , IMruItem               sender2SelectedItem
          , TaskFactory            factory1
          , SynchronizationContext current
        )
        {
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances();
            var i = visualStudioInstances
               .Single(
                       instance => instance.VisualStudioRootPath
                                   == vsSelectedItem.InstallationPath
                      ) ;
            if ( sender2SelectedItem != null )
            {
                _handler = new ProjectHandlerImpl ( sender2SelectedItem.FilePath , i , current) ;
                _handler.ProcessProject +=
                    ( workspace , project ) => CurrentProject = project.Name ;
                _handler.ProcessDocument += document => {
                    CurrentDocumentPath = document.RelativePath ( ) ;
                } ;
                _handler.progressReporter = new MyProgress ( this ) ;
                await _handler.LoadAsync ( ) 
                    ;
                
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
            return new object();
        }

        private void DoSomething ( ) { }

        public delegate FormattedCode CreateFormattedCodeDelegate(
            Tuple<SyntaxTree, SemanticModel, CompilationUnitSyntax> tuple);

        public delegate FormattedCode CreateFormattedCodeDelegate2 ( ) ;


        public async Task < object > ProcessSolutionAsync (
            Dispatcher dispatcher,
            TaskFactory factory,
          Func<object, FormattedCode> func
        )
        {
            Processing = true;
            Func < Tuple < SyntaxTree , SemanticModel , CompilationUnitSyntax > ,
                CreateFormattedCodeDelegate2 > d = t
                => new CreateFormattedCodeDelegate2 ( ( ) => new FormattedCode ( ) );
            
            await _handler.ProcessAsync (
                                         invocation
                                             => dispatcher.Invoke (
                                                                   ( ) => LogInvocations.Add (
                                                                                              invocation
                                                                                             )
                                                                  )
                                       , new DispatcherSynchronizationContext(dispatcher), func
                                        )
                          .ConfigureAwait ( true ) ;
            Processing = false ;
            return new object ( ) ;
        }

        public bool Processing { get { return _processing ; }
            set
            {
                _processing = value ;
                OnPropertyChanged ( "Processing" ) ;
            }
        }

        public string CurrentProject
        {
            get => _currentProject ;
            set
            {
                _currentProject = value ;
                OnPropertyChanged("CurrentProject");
            }
        }

        public string CurrentDocumentPath
        {
            get => _currentDocumentPath ;
            set
            {
                _currentDocumentPath = value ;
                OnPropertyChanged ( "CurrentDocumentPath" ) ;
            }
        }

        public ObservableCollection<LogInvocation> LogInvocations{ get ; } = new ObservableCollection < LogInvocation > ();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CodeWindow : Window
    {
        private TabControl control = new TabControl ( ) ;

        public CodeWindow ( )
        {
            Content = control;
        }

        Dictionary < object , FormattedCode > dict = new Dictionary < object , FormattedCode > ( ) ;

        public delegate FormattedCode GetFormattedCodeDelegate (object o ) ;
        public Task < FormattedCode > GetFormattedCodeAsync ( object o )
        {
            return Task<FormattedCode>.FromResult(
                                   (FormattedCode)Dispatcher.Invoke (
                                                      DispatcherPriority.Send
                                                    , new GetFormattedCodeDelegate (
                                                                                    ( o1 )
                                                                                        => new
                                                                                            FormattedCode ( )
                                                                                   )
                                                    , o
                                                     )
                                  ) ;
        }


        public FormattedCode GetFormattedCode ( object o )
        {
            TaskFactory < FormattedCode > taskFactory = new TaskFactory < FormattedCode > ( ) ;
            if ( dict.TryGetValue ( o , out var item ) )
            {
                return item ;
            }

            dict[ o ] = new FormattedCode ( ) ;
            TabItem item2 = new TabItem ( ) { Header = "123" , Content = dict[ o ] } ;
            (( IAddChild ) control).AddChild ( item2 ) ;
            return dict[ o ] ;
        }
    }


    public class MyProjectLoadProgress
    {
        /// <summary>
        /// The project for which progress is being reported.
        /// </summary>
        public string FilePath { get; }

        public string FileName => Path.GetFileNameWithoutExtension ( FilePath ) ;

        /// <summary>
        /// The operation that has just completed.
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// The target framework of the project being built or resolved. This property is only valid for SDK-style projects
        /// during the <see cref="ProjectLoadOperation.Resolve"/> operation.
        /// </summary>
        public string TargetFramework { get; }

        /// <summary>
        /// The amount of time elapsed for this operation.
        /// </summary>
        public TimeSpan ElapsedTime { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MyProjectLoadProgress ( string filePath , string operation , string targetFramework , TimeSpan elapsedTime )
        {
            FilePath = filePath ;
            Operation = operation ;
            TargetFramework = targetFramework ;
            ElapsedTime = elapsedTime ;
        }
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
            _workspacesViewModel.CurrentProgress = new MyProjectLoadProgress(value.FilePath, value.Operation.ToString(), value.TargetFramework, value.ElapsedTime);
        }
    }
}