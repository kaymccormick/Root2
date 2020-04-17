using System ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
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
    ///     Interaction logic for WorkspaceControl.xaml
    /// </summary>
    [ TitleMetadata ( "Workspaces" ) ]
    public sealed partial class WorkspaceControl : UserControl
      , IView1
      , IView < WorkspaceViewModel >
      , IControlView
      , IViewWithTitle
    {
        private readonly WorkspaceViewModel _viewModel ;

        /// <inheritdoc />
        public WorkspaceControl ( WorkspaceViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        #region Implementation of IView<out WorkspaceViewModel>
        /// <inheritdoc />
        public WorkspaceViewModel ViewModel { get { return _viewModel ; } }
        #endregion

        #region Implementation of IViewWithTitle
        /// <inheritdoc />
        public string ViewTitle { get ; } = "workspaces" ;
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            _viewModel.CreateWorkspace ( ) ;
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            var dlg = new OpenFileDialog { Filter = "CSharp Files|*.cs" } ;
            var result = dlg.ShowDialog ( ) ;
            if ( result == true )
            {
                ViewModel.AddDocument ( dlg.FileName ) ;
            }
        }
    }


    /// <summary>
    /// </summary>
    public sealed class WorkspaceViewModel : IViewModel , INotifyPropertyChanged
    {
        private AdhocWorkspace _workspace ;

        /// <summary>
        /// </summary>
        public ICollectionView ProjectCollectionView
        {
            get
            {
                if ( Workspace?.CurrentSolution != null )
                {
                    return CollectionViewSource.GetDefaultView (
                                                                Workspace.CurrentSolution.Projects
                                                               ) ;
                }

                return null ;
            }
        }

        /// <summary>
        /// </summary>
        public AdhocWorkspace Workspace
        {
            get { return _workspace ; }
            set
            {
                if ( Equals ( value , _workspace ) )
                {
                    return ;
                }

                _workspace                  =  value ;
                _workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [ NotNull ] public ProjectId CurrentProjectId
        {
            get { return Workspace.CurrentSolution.Projects.First ( ).Id ; }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of ISerializable
        /// <inheritdoc />
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// </summary>
        public void CreateWorkspace ( )
        {
            var workspace = new AdhocWorkspace ( ) ;
            var projectId = ProjectId.CreateNewId ( ) ;
            // ReSharper disable once UnusedVariable
            var s = workspace.AddSolution (
                                           SolutionInfo.Create (
                                                                SolutionId.CreateNewId ( )
                                                              , VersionStamp.Create ( )
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
                                          )
                    ?? throw new ArgumentNullException ( ) ;

            Workspace = workspace ;
        }

        private void WorkspaceOnWorkspaceChanged (
            object                               sender
          , [ NotNull ] WorkspaceChangeEventArgs e
        )
        {
            DebugUtils.WriteLine ( e.Kind.ToString ( ) ) ;
            switch ( e.Kind )
            {
                case WorkspaceChangeKind.SolutionChanged :
                    OnPropertyChanged ( nameof ( ProjectCollectionView ) ) ;
                    break ;
                case WorkspaceChangeKind.SolutionAdded :    break ;
                case WorkspaceChangeKind.SolutionRemoved :  break ;
                case WorkspaceChangeKind.SolutionCleared :  break ;
                case WorkspaceChangeKind.SolutionReloaded : break ;
                case WorkspaceChangeKind.ProjectAdded :
                    OnPropertyChanged ( nameof ( ProjectCollectionView ) ) ;
                    break ;
                case WorkspaceChangeKind.ProjectRemoved :  break ;
                case WorkspaceChangeKind.ProjectChanged :  break ;
                case WorkspaceChangeKind.ProjectReloaded : break ;
                case WorkspaceChangeKind.DocumentAdded :
                    DocumentAdded ( e ) ;
                    break ;
                case WorkspaceChangeKind.DocumentRemoved :                break ;
                case WorkspaceChangeKind.DocumentReloaded :               break ;
                case WorkspaceChangeKind.DocumentChanged :                break ;
                case WorkspaceChangeKind.AdditionalDocumentAdded :        break ;
                case WorkspaceChangeKind.AdditionalDocumentRemoved :      break ;
                case WorkspaceChangeKind.AdditionalDocumentReloaded :     break ;
                case WorkspaceChangeKind.AdditionalDocumentChanged :      break ;
                case WorkspaceChangeKind.DocumentInfoChanged :            break ;
                case WorkspaceChangeKind.AnalyzerConfigDocumentAdded :    break ;
                case WorkspaceChangeKind.AnalyzerConfigDocumentRemoved :  break ;
                case WorkspaceChangeKind.AnalyzerConfigDocumentReloaded : break ;
                case WorkspaceChangeKind.AnalyzerConfigDocumentChanged :  break ;
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private void DocumentAdded ( WorkspaceChangeEventArgs workspaceChangeEventArgs ) { }

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDocument ( [ NotNull ] string fileName )
        {
            var docName = Path.GetFileNameWithoutExtension ( fileName ) ;
            var docInfo = DocumentInfo.Create (
                                               DocumentId.CreateNewId ( CurrentProjectId , docName )
                                             , docName
                                             , null
                                             , SourceCodeKind.Regular
                                             , new FileTextLoader ( fileName , Encoding.UTF8 )
                                             , fileName
                                              ) ;
            var solution = Workspace.CurrentSolution.AddDocument ( docInfo ) ;
            var result = Workspace.TryApplyChanges ( solution ) ;
            if ( result == false )
            {
                throw new InvalidOperationException ( ) ;
            }
        }
    }
}