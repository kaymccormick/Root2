using System;
using System.Collections.Generic;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.Win32 ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for WorkspaceControl.xaml

    /// </summary>
    [TitleMetadata("Workspaces")]
    public partial class WorkspaceControl : UserControl, IView1, IView<WorkspaceViewModel>, IControlView, IViewWithTitle
    {
        private readonly WorkspaceViewModel _viewModel;

        /// <inheritdoc />
        public WorkspaceControl( WorkspaceViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent();
        }

        #region Implementation of IView<out WorkspaceViewModel>
        /// <inheritdoc />
        public WorkspaceViewModel ViewModel => _viewModel;
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            _viewModel.CreateWorkspace ( ) ;
        }

        #region Implementation of IViewWithTitle
        /// <inheritdoc />
        public string ViewTitle { get ; } = "workspaces" ;
        #endregion

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            var dlg = new OpenFileDialog ( ) ;
            dlg.Filter = "CSharp Files|*.cs" ;
            var result = dlg.ShowDialog ( ) ;
            if ( result == true )
            {
                ViewModel.AddDocument ( dlg.FileName ) ;
            }

        }
    }

    /// <inheritdoc />
    public sealed class WorkspaceViewModel : IViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public ICollectionView ProjectCollectionView
        {
            get
            {
                if ( Workspace                    != null
                     && Workspace.CurrentSolution != null )
                {
                    return CollectionViewSource.GetDefaultView (
                                                                Workspace.CurrentSolution.Projects
                                                               ) ;
                }

                return null ;
            }
        }

        private AdhocWorkspace _workspace ;
        #region Implementation of ISerializable
        /// <inheritdoc />
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void CreateWorkspace ( )
        {
            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId ( ) ;
            var s = workspace.AddSolution(
                              SolutionInfo.Create(
                                                   SolutionId.CreateNewId()
                                                 , VersionStamp.Create()
                                                 , null
                                                 , new[]
                                                   {
                                                                ProjectInfo.Create (
                                                                                    projectId
                                                                                  , VersionStamp
                                                                                       .Create ( )
                                                                                  , "test"
                                                                                  , "test"
                                                                                  , LanguageNames
                                                                                       .CSharp
                                                                                  , null
                                                                                  , null
                                                                                  , new
                                                                                        CSharpCompilationOptions (
                                                                                                                  OutputKind
                                                                                                                     .DynamicallyLinkedLibrary
                                                                                                                 )
                                                                                   )
                                                   }
                                                  )
                             );

            Workspace = workspace ;
        }

        /// <summary>
        /// 
        /// </summary>
        public AdhocWorkspace Workspace
        {
            get { return _workspace ; }
            set
            {
                if ( Equals ( value , _workspace ) ) return ;
                _workspace = value ;
                _workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
                OnPropertyChanged ( ) ;
            }
        }

        private void WorkspaceOnWorkspaceChanged ( object sender , WorkspaceChangeEventArgs e )
        {
            DebugUtils.WriteLine ( e.Kind ) ;
            switch (e.Kind)
            {
                case WorkspaceChangeKind.SolutionChanged:
                    OnPropertyChanged(nameof ( ProjectCollectionView ));
                    break;
                case WorkspaceChangeKind.SolutionAdded:
                    break;
                case WorkspaceChangeKind.SolutionRemoved:
                    break;
                case WorkspaceChangeKind.SolutionCleared:
                    break;
                case WorkspaceChangeKind.SolutionReloaded:
                    break;
                case WorkspaceChangeKind.ProjectAdded:
                    OnPropertyChanged(nameof(ProjectCollectionView));
                    break;
                case WorkspaceChangeKind.ProjectRemoved:
                    break;
                case WorkspaceChangeKind.ProjectChanged:
                    break;
                case WorkspaceChangeKind.ProjectReloaded:
                    break;
                case WorkspaceChangeKind.DocumentAdded:
                    DocumentAdded ( e ) ;
                    break;
                case WorkspaceChangeKind.DocumentRemoved:
                    break;
                case WorkspaceChangeKind.DocumentReloaded:
                    break;
                case WorkspaceChangeKind.DocumentChanged:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentAdded:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentRemoved:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentReloaded:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentChanged:
                    break;
                case WorkspaceChangeKind.DocumentInfoChanged:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentAdded:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentRemoved:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentReloaded:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentChanged:
                    break;
            }
        }

        private void DocumentAdded ( WorkspaceChangeEventArgs workspaceChangeEventArgs ) { }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDocument ( string fileName )
        {
            var docName = System.IO.Path.GetFileNameWithoutExtension ( fileName ) ;
            var docInfo = DocumentInfo.Create (
                                               DocumentId.CreateNewId ( CurrentProjectId , docName )
                                             , docName
                                             , null
                                             , SourceCodeKind.Regular
                                             , new FileTextLoader ( fileName, Encoding.UTF8 )
                                             , fileName
                                              ) ;
            var solution = Workspace.CurrentSolution.AddDocument ( docInfo ) ;
            var result = Workspace.TryApplyChanges ( solution ) ;
            if ( result == false )
            {
                throw new InvalidOperationException();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public ProjectId CurrentProjectId => Workspace.CurrentSolution.Projects.First ( ).Id ;
    }
}
