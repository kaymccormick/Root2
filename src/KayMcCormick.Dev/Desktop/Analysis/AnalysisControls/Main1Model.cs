using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Threading;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Main1Model : INotifyPropertyChanged
    {
        private readonly ReplaySubject<Workspace> _replay;
        private Workspace _workspace;
        private Main1 _view;
        private WorkspaceView _workspaceView;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SelectVsInstance()
        {
            if (TrySelectVsInstance())
                return;
            throw new InvalidOperationException("Cant register");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool TrySelectVsInstance ( )
        {
            if ( ! MSBuildLocator.CanRegister )
            {
                return false;
                
            }

            var vsInstances = MSBuildLocator
                .QueryVisualStudioInstances (
                    new VisualStudioInstanceQueryOptions
                    {
                        DiscoveryTypes =
                            DiscoveryType.VisualStudioSetup
                    }
                );

            foreach (var vsi in vsInstances)
            {
                DebugUtils.WriteLine($"{vsi.Name} {vsi.Version}");
            }
                
            var versions = vsInstances.Select(x => x.Version.Major).Distinct().OrderByDescending(i => i);
                DebugUtils.WriteLine(string.Join(", ", versions));
                    var inst = versions.FirstOrDefault();


var visualStudioInstance = vsInstances.Where(instance => instance.Version.Major == inst).OrderByDescending(instance => instance.Version).FirstOrDefault();
DebugUtils.WriteLine($"Registering {visualStudioInstance}");
            MSBuildLocator.RegisterInstance ( visualStudioInstance);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Documents { get; } = new ObservableCollection<object>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Anchorables { get; } = new ObservableCollection<object>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="replay"></param>
        public Main1Model(ReplaySubject<Workspace> replay)
        {
            _replay = replay;
        }

        public void CreateWorkspace()
        {
            Workspace = new AdhocWorkspace();
            _replay.OnNext(Workspace); 
        }

        /// <summary>
        /// 
        /// </summary>
        public Workspace Workspace
        {
            get { return _workspace; }
            set
            {
                if (Equals(value, _workspace)) return;
                _workspace = value;
                _workspaceView = new WorkspaceView() {Solutions = HierRoot};
                Anchorables.Add(new AnchorableModel() {Content = _workspaceView});
                _workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
                _workspace.WorkspaceFailed += (sender, e) =>
                {
                    DebugUtils.WriteLine($"{e.Diagnostic.Message}");
                };
                OnPropertyChanged();
            }
        } 

        private async void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            switch (e.Kind)
            {
                case WorkspaceChangeKind.SolutionRemoved:
                    HierRoot.Remove(HierRoot.First(s => s.Id == e.OldSolution.Id));
                    break;
                case WorkspaceChangeKind.SolutionChanged:
                    var ch = e.NewSolution.GetChanges(e.OldSolution);
                    
                    break;
                case WorkspaceChangeKind.SolutionAdded:
                    var solutionModel = SolutionModelFromSolution(e.NewSolution);
                    HierRoot.Add(solutionModel);
                    break;
                case WorkspaceChangeKind.SolutionCleared:
                    break;
                case WorkspaceChangeKind.SolutionReloaded:
                    break;
                case WorkspaceChangeKind.ProjectAdded:
                    var m = HierRoot.FirstOrDefault(m1 => m1.Id == e.NewSolution.Id);
                    if (m == default(SolutionModel))
                    {
                        var solutionModel1 = SolutionModelFromSolution(e.NewSolution);
                        HierRoot.Add(solutionModel1);
                        m = solutionModel1;
                    }

                    var projectModel = ProjectFromModel(m, e.NewSolution.GetProject(e.ProjectId));
                    m.Projects.Add(projectModel);
                    break;
                case WorkspaceChangeKind.ProjectRemoved:
                    break;
                case WorkspaceChangeKind.ProjectChanged:
                    break;
                case WorkspaceChangeKind.ProjectReloaded:
                    break;
                case WorkspaceChangeKind.DocumentAdded:
                    var d = new DocumentModel();
                    var doc = e.NewSolution.GetDocument(e.DocumentId);
                    if (doc != null)
                    {
                        d.Name = doc.Name;
                        d.FilePath = doc.FilePath;
                    }
                    var p0 = GetProjectModel(e.ProjectId);
                    p0.Documents.Add(d);
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ProjectModel GetProjectModel(ProjectId id)
        {
            return HierRoot.SelectMany(s => s.Projects).Where(p => p.Id == id).First();
        }

        private static SolutionModel SolutionModelFromSolution(Solution s)
        {
            var m = new SolutionModel();
            m.Id = s.Id;
            m.FilePath = s.FilePath;
            foreach (var sProject in s.Projects)
            {
                m.Projects.Add(ProjectFromModel(m, sProject));
            }
            return m;
        }

        private static ProjectModel ProjectFromModel(SolutionModel s, Project sProject)
        {
            var p = new ProjectModel();
            p.Name = sProject.Name;
            p.Id = sProject.Id;
            p.Solution = s;
            p.Project = sProject;
            p.FilePath = sProject.FilePath;
            foreach (var sProjectDocument in sProject.Documents)
            {
                var d = DocumentFromModel(p, sProjectDocument);
                p.Documents.Add(d);
            }

            return p;
        }

        private static DocumentModel DocumentFromModel(ProjectModel project, Document sProjectDocument)
        {
            var d = new DocumentModel();
            d.Document = sProjectDocument;
            d.Project = project;
            d.Name = sProjectDocument.Name;
            d.FilePath = sProjectDocument.FilePath;
            return d;
        }

        public ObservableCollection<SolutionModel> HierRoot { get; set; } = new ObservableCollection<SolutionModel>();

        public Main1 View
        {
            get { return _view; }
            set
            {
                if (Equals(value, _view)) return;
                _view = value;
                OnPropertyChanged();
            }
        }

        public ProjectModel SelectedProject => _workspaceView.SelectedProject;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateSolution()
        {
            string debugName = "debug1";
            string filePath = null;
            if (Workspace is AdhocWorkspace ww)
            {
                ww.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(debugName), VersionStamp.Create(),
                    filePath));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateProject()
        {
            // var s = _workspaceView.SelectedSolution;
            // if (Workspace.CurrentSolution.Id != s.Id)
            // {
                
            // }
            if(Workspace is AdhocWorkspace ww)
                ww.AddProject(ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "unnamed project",
                    "unnamed assembly", LanguageNames.CSharp
                ));

        }

        public void AddDocument(ProjectModel viewModelSelectedProject, string file)
        {
            var proj = Workspace.CurrentSolution.GetProject(viewModelSelectedProject.Id);
            var code = File.ReadAllText(file);
            if (proj != null)
            {
                var news = Workspace.CurrentSolution.AddDocument(DocumentInfo.Create(DocumentId.CreateNewId(proj.Id),
                    Path.GetFileNameWithoutExtension(file), null, SourceCodeKind.Regular,
                    TextLoader.From(TextAndVersion.Create(SourceText.From(code), VersionStamp.Create())), file));
                if (!Workspace.TryApplyChanges(news))
                {
                    DebugUtils.WriteLine("Failed");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CurrentOperation CurrentOperation { get; set; } = new CurrentOperation();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task LoadSolution(string file)
        {
            TrySelectVsInstance();
            var msBuildWorkspace = MSBuildWorkspace.Create();
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() { Description = "load solution" };
            var r = await msBuildWorkspace.OpenSolutionAsync(file, new ProgressWithCompletion<ProjectLoadProgress>(Handler));
            CurrentOperation = null;
            DebugUtils.WriteLine(r);
        }

        public ObservableCollection<object> Messages { get; }= new ObservableCollection<object>();
        private void Handler(ProjectLoadProgress obj)
        {
            ProjectLoadProgress = obj;

            var line = $"{obj.Operation} {obj.ElapsedTime} {obj.FilePath}";
            Messages.Add(line);
            DebugUtils.WriteLine(line);
        }

        public ProjectLoadProgress ProjectLoadProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkspaceView WorkspaceView
        {
            get { return _workspaceView; }
            set { _workspaceView = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task OpenSolutionItem()
        {
            var doc = _workspaceView.SelectedDocument;
            if (doc != null)
            {
                CurrentOperation = new CurrentOperation() {Description = "Open document " + doc.Name};

                var tree = await doc.Document.GetSyntaxTreeAsync();
                Compilation compilation = null;
                if (doc.Project.Project.SupportsCompilation)
                {
                    compilation = await doc.Project.Project.GetCompilationAsync();
                }

                FormattedTextControl c = new FormattedTextControl() {SyntaxTree = tree, Compilation = (CSharpCompilation) compilation};
                DocModel doc2 = new DocModel();
                doc2.Title = doc.Name;
                doc2.Content = c;
                Documents.Add(doc2);
                CurrentOperation = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CurrentOperation
    {
        public string Description { get; set; }
    }
}