using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Threading;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Main1Model : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public object ActiveContent

        {
            get { return _activeContent; }
            set
            {
                if (Equals(value, _activeContent)) return;
                _activeContent = value;
                OnPropertyChanged();
            }
        }

        private readonly ReplaySubject<Workspace> _replay;
        private Workspace _workspace;
        private Main1 _view;
        private WorkspaceView _workspaceView;
        private object _activeContent;
        private ProjectLoadProgress _projectLoadProgress;
        private CurrentOperation _currentOperation = new CurrentOperation();

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
        public static bool TrySelectVsInstance()
        {
            if (!MSBuildLocator.CanRegister) return false;

            var vsInstances = MSBuildLocator
                .QueryVisualStudioInstances(
                    new VisualStudioInstanceQueryOptions
                    {
                        DiscoveryTypes =
                            DiscoveryType.VisualStudioSetup
                    }
                );

            foreach (var vsi in vsInstances) DebugUtils.WriteLine($"{vsi.Name} {vsi.Version}");

            var versions = vsInstances.Select(x => x.Version.Major).Distinct().OrderByDescending(i => i);
            DebugUtils.WriteLine(string.Join(", ", versions));
            var inst = versions.FirstOrDefault();


            var visualStudioInstance = vsInstances.Where(instance => instance.Version.Major == inst)
                .OrderByDescending(instance => instance.Version).FirstOrDefault();
            DebugUtils.WriteLine($"Registering {visualStudioInstance}");
            MSBuildLocator.RegisterInstance(visualStudioInstance);
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

        /// <summary>
        /// 
        /// </summary>
        public Main1Model()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateWorkspace()
        {
            Workspace = new AdhocWorkspace();
            _replay?.OnNext(Workspace);
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
                Anchorables.Add(new AnchorableModel() {Content =
                    _workspaceView, Title = "Workspace"});
                _workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
                _workspace.DocumentOpened += WorkspaceOnDocumentOpened;
                _workspace.DocumentActiveContextChanged += WorkspaceOnDocumentActiveContextChanged;
                _workspace.DocumentClosed += WorkspaceOnDocumentClosed;
                var model = this;
                Diagnostics.Clear();
                _workspace.WorkspaceFailed += (sender, e) =>
                {
                    model.Diagnostics.Add(e.Diagnostic);
                    if (View != null)
                    {
                        var dispatcherOperation = View.Dispatcher.InvokeAsync(() =>
                            Messages.Messages.Add(new WorkspaceMessage
                            {
                                Source = e.Diagnostic, Message = e.Diagnostic.Message,
                                Severity = e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure
                                    ? WorkspaceMessageSeverity.Error
                                    : WorkspaceMessageSeverity.Warning
                            }));
                    }
                };
                OnPropertyChanged();
            }
        }

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private void WorkspaceOnDocumentClosed(object sender, DocumentEventArgs e)
        {
            Logger.Info($"{e.Document.Id} closed");
        }

        private void WorkspaceOnDocumentActiveContextChanged(object sender, DocumentActiveContextChangedEventArgs e)
        {
            Logger.Info($"{e.NewActiveContextDocumentId.Id} context");
        }

        private void WorkspaceOnDocumentOpened(object sender, DocumentEventArgs e)
        {
            Logger.Info($"{e.Document.Id} opened"); 
        }

        /// <summary>
        /// 
        /// </summary>
        public List<WorkspaceDiagnostic> Diagnostics { get; set; } = new List<WorkspaceDiagnostic>();

        public event EventHandler<ProjectAddedventArgs> ProjectedAddedEvent;
        public event EventHandler<DocumentAddedventArgs> DocumentAddedEvent;
        public async void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            DebugUtils.WriteLine(e.Kind.ToString());
            switch (e.Kind)
            {
                case WorkspaceChangeKind.SolutionRemoved:
                    HierRoot.Remove(HierRoot.First(s => s.Id == e.OldSolution.Id));
                    break;
                case WorkspaceChangeKind.SolutionChanged:
                    var ch0 = e.NewSolution.GetChanges(e.OldSolution);

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
                    Solution eNewSolution = e.NewSolution;
                    var m = GetSolution(eNewSolution.Id);
                    if (m == default(SolutionModel))
                    {
                        var solutionModel1 = SolutionModelFromSolution(e.NewSolution);
                        HierRoot.Add(solutionModel1);
                        m = solutionModel1;
                    }

                    var projectModel = ProjectFromModel(m, e.NewSolution.GetProject(e.ProjectId));
                    m.Projects.Add(projectModel);
                    ProjectedAddedEvent?.Invoke(this, new ProjectAddedventArgs() {Model = projectModel});
                    break;
                case WorkspaceChangeKind.ProjectRemoved:
                    break;
                case WorkspaceChangeKind.ProjectChanged:
                    break;
                case WorkspaceChangeKind.ProjectReloaded:
                    break;
                case WorkspaceChangeKind.DocumentAdded:
                    var p0 = GetProjectModel(e.ProjectId);
                    var d = new DocumentModel(p0);
                    var doc = e.NewSolution.GetDocument(e.DocumentId);
                    if (doc != null)
                    {
                        d.Name = doc.Name;
                        d.FilePath = doc.FilePath;
                    }

                    d.Document = doc;

                
                    Compilation compilation = null;
                    SemanticModel model = null;

                    if (d.Project.Project.SupportsCompilation)
                    {
                        compilation = await doc.Project.GetCompilationAsync();
                        var errs = compilation.GetDiagnostics()
                            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
                        foreach (var diagnostic in compilation.GetDiagnostics())
                        {
                            PathModel diag = null;
                            diag.Add(new DiagnosticNodeModel(PathModelKind.Diagnostic) {Diagnostic = diagnostic});
                        }

                        DebugUtils.WriteLine(String.Join("\n", errs));
                        if (errs.Any())
                        {
                            compilation = null;
                        }
                        else
                        {

                        }
                    }

                    model = await doc.GetSemanticModelAsync();
                

                    var tree = await doc.GetSyntaxTreeAsync();
            

            p0.Documents.Add(d);
                    DocumentAddedEvent?.Invoke(this, new DocumentAddedventArgs(){Document =d});
                    
                    break;
                case WorkspaceChangeKind.DocumentRemoved:
                    break;
                case WorkspaceChangeKind.DocumentReloaded:
                    break;
                case WorkspaceChangeKind.DocumentChanged:
                    var ch = e.NewSolution.GetChanges(e.OldSolution);
                    foreach (var addedProject in ch.GetAddedProjects())
                    {
                        DebugUtils.WriteLine(addedProject.Id.Id.ToString());
                    }
                    foreach (var projectChangese in ch.GetProjectChanges())
                    {
                        foreach (var addedAdditionalDocument in projectChangese.GetAddedAdditionalDocuments())
                        {
                            DebugUtils.WriteLine(addedAdditionalDocument.Id.ToString());
                        }

                        foreach (var addedDocument in projectChangese.GetAddedDocuments())
                        {
                            DebugUtils.WriteLine(addedDocument.Id.ToString());
                        }
                        foreach (var changedDocument in projectChangese.GetChangedDocuments())
                        {
                            DebugUtils.WriteLine(changedDocument.Id.ToString());
                        }
                    }
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

        private SolutionModel GetSolution(SolutionId newSolutionId = null)
        {
            if (newSolutionId == null)
            {
                newSolutionId = _workspace.CurrentSolution.Id;
            }
            var m = HierRoot.FirstOrDefault(m1 =>
            {
                return m1.Id == newSolutionId;
            });
            return m;
        }

        private ProjectModel GetProjectModel(ProjectId id)
        {
            return HierRoot.SelectMany(s => s.Projects).Where(p => p.Id == id).First();
        }

        private DocumentModel GetDocumentModel(DocumentId id)
        {
            return GetProjectModel(id.ProjectId).Documents.FirstOrDefault(z => z.Id == id.Id);
        }

        private static SolutionModel SolutionModelFromSolution(Solution s)
        {
            var m = new SolutionModel();
            m.Id = s.Id;
            m.FilePath = s.FilePath;
            foreach (var sProject in s.Projects) m.Projects.Add(ProjectFromModel(m, sProject));
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
            var d = new DocumentModel(project);
            d.Document = sProjectDocument;
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

        public ProjectModel SelectedProject
        {
            get { return _workspaceView?.SelectedProject; }
        }

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
            var debugName = "debug1";
            string filePath = null;
            if (Workspace is AdhocWorkspace ww)
                ww.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(debugName), VersionStamp.Create(),
                    filePath));
        }

        /// <summary>
        /// 
        /// </summary>
        public ProjectInfo CreateProject()
        {
            // var s = _workspaceView.SelectedSolution;
            // if (Workspace.CurrentSolution.Id != s.Id)
            // {

            // }
            
            if (Workspace is AdhocWorkspace ww)
            {
                
                var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "unnamed project",
                    "unnamed assembly", LanguageNames.CSharp
                );
                var news = ww.CurrentSolution.AddProject(projectInfo);
                if (ww.TryApplyChanges(news))
                {
                    return projectInfo;
                }
                
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelSelectedProject"></param>
        /// <param name="file"></param>
        public void AddDocument(ProjectModel viewModelSelectedProject, string file)
        {
            var proj = Workspace.CurrentSolution.GetProject(viewModelSelectedProject.Id);
            var code = File.ReadAllText(file);
            if (proj != null)
            {
                var documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(proj.Id),
                    Path.GetFileNameWithoutExtension(file), null, SourceCodeKind.Regular,
                    TextLoader.From(TextAndVersion.Create(SourceText.From(code), VersionStamp.Create())), file);
                var news = Workspace.CurrentSolution.AddDocument(documentInfo);
                if (!Workspace.TryApplyChanges(news)) DebugUtils.WriteLine("Failed");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CurrentOperation CurrentOperation
        {
            get { return _currentOperation; }
            set
            {
                if (Equals(value, _currentOperation)) return;
                _currentOperation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task LoadSolution(string file)
        {
            TrySelectVsInstance();
            IDictionary<string, string> props = new Dictionary<string, string>();
            props["Platform"] = "x86";
            var msBuildWorkspace = MSBuildWorkspace.Create(props);
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() {Description = "load solution"};
            var r = await msBuildWorkspace.OpenSolutionAsync(file,
                new ProgressWithCompletion<ProjectLoadProgress>(Handler));
            CurrentOperation = null;
            DebugUtils.WriteLine(r);
        }

        public MessagesModel Messages { get; } = new MessagesModel();

        private void Handler(ProjectLoadProgress obj)
        {
            ProjectLoadProgress = obj;

            var line = $"{obj.Operation} {obj.FilePath}";
            Messages.Messages.Add(new WorkspaceMessage
                {Source = obj, Message = line, Severity = WorkspaceMessageSeverity.LoadProgress});
            DebugUtils.WriteLine(line);
        }

        /// <summary>
        /// 
        /// </summary>
        public ProjectLoadProgress ProjectLoadProgress
        {
            get { return _projectLoadProgress; }
            set
            {
                if (value.Equals(_projectLoadProgress)) return;
                _projectLoadProgress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WorkspaceView WorkspaceView
        {
            get { return _workspaceView; }
            set
            {
                if (Equals(value, _workspaceView)) return;
                _workspaceView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eParameter"></param>
        /// <returns></returns>
        public async Task OpenSolutionItem(object eParameter)
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            DocumentModel doc = null;
            if (eParameter is PathModel p)
            {
                doc = p.Item as DocumentModel;
            } else if (eParameter is DocumentModel dd)
            {
                doc = dd;
            }
            if (doc == null)
            {
                doc = _workspaceView.SelectedDocument;
            }
            if (doc != null)
            {
                CurrentOperation = new CurrentOperation() {Description = "Open document " + doc.Name};

                Workspace.OpenDocument( doc.Document.Id, true);
                Compilation compilation = null;
                SemanticModel model = null;
                
                if (doc.Project.Project.SupportsCompilation)
                {
                    compilation = await doc.Project.Project.GetCompilationAsync();
                    var errs = compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
                    DebugUtils.WriteLine(String.Join("\n", errs));
                    if (errs.Any())
                    {
                        compilation = null;
                    }
                    else
                    {

                    }

                    model = await doc.Document.GetSemanticModelAsync();
                }

                var tree = await doc.Document.GetSyntaxTreeAsync();
                var c = new FormattedTextControl()
                {
                    SyntaxTree = tree,
                    Compilation = (CSharpCompilation) compilation,
                    Model = model
                };
                var doc2 = new DocModel {Title = doc.Name, Content = c};
                Documents.Add(doc2);
                ActiveContent = doc2;
                CurrentOperation = null;
            }
            else if (SelectedProject != null)
            {
                var semanticControl1 = new SemanticControl1();
                SelectedProject.Project.GetCompilationAsync().ContinueWith(
                    task => { semanticControl1.Compilation = (CSharpCompilation) task.Result; }, taskScheduler);

                var anchorableModel = new AnchorableModel {Content = semanticControl1};
                Anchorables.Add(anchorableModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task BrowseSymbols(object parameter)
        {
            if (parameter is ProjectModel pm)
            {
                var listBox = new ListBox();
                listBox.ItemTemplate = (DataTemplate) View.TryFindResource(new DataTemplateKey(typeof(ISymbol)));
                var comp = await pm.Project.GetCompilationAsync();
                listBox.ItemsSource = comp.GetSymbolsWithName(x => true);
                Documents.Add(new DocModel()
                {
                    Title = "Symbols for " + pm.Name,
                    Content = listBox
                });
            }
        }
    }

    public class DiagnosticNodeModel : PathModel
    {
        public DiagnosticNodeModel(PathModelKind kind) : base(kind)
        {
        }

        public Diagnostic Diagnostic { get; set; }
    }

    public class DocumentAddedventArgs
    {
        public DocumentModel Document { get; set; }
    }

    public class ProjectAddedventArgs
    {
        private ProjectModel projectModel;

        public ProjectModel Model
        {
            get { return projectModel; }
            set { projectModel = value; }
        }
    }
}