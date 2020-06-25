using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

using Microsoft.CodeAnalysis.Text;

using NLog;
using Path = System.IO.Path;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Main1Mode2 : INotifyPropertyChanged
    {
        private readonly ReplaySubject<Workspace> _replay;
        private readonly IDocumentHost _docHost;
        private readonly IAnchorableHost _anchHost;
        private Workspace _workspace;
        private Main1 _view;
        private WorkspaceView _workspaceView;
        
        private ProjectLoadProgress _projectLoadProgress;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="replay"></param>
        public Main1Mode2(ReplaySubject<Workspace> replay, JsonSerializerOptions jsonSerializerOptions = null, IDocumentHost docHost = null, IAnchorableHost anchHost=null) : this()
        {
            JsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();
            _replay = replay;
            _docHost = docHost;
            _anchHost = anchHost;
        }

        /// <summary>
        /// 
        /// </summary>
        public Main1Mode2()
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
            // ReSharper disable once MemberCanBePrivate.Global
            set
            {
                if (Equals(value, _workspace)) return;
                _workspace = value;
                _workspaceView = new WorkspaceView() {Solutions = HierarchyRoot};
                if (_anchHost != null)
                    _anchHost.AddAnchorable(new AnchorableModel()
                    {
                        Content =
                            _workspaceView,
                        Title = "Workspace"
                    });
                else
                {
                    var dm = DocModel.CreateInstance("Workspace");
                    dm.Content = _workspaceView;
                    _docHost.AddDocument(dm);
                }

                _workspace.WorkspaceChanged += (sender, args) =>
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => WorkspaceOnWorkspaceChanged(sender, args)));
                };
                _workspace.DocumentOpened += WorkspaceOnDocumentOpened;
                _workspace.DocumentActiveContextChanged += WorkspaceOnDocumentActiveContextChanged;
                _workspace.DocumentClosed += WorkspaceOnDocumentClosed;
                var model = this;
                Diagnostics.Clear();
                _workspace.WorkspaceFailed += (sender, e) =>
                {
                    model.Diagnostics.Add(e.Diagnostic);
                        // ReSharper disable once UnusedVariable
                        var dispatcherOperation = Dispatcher.InvokeAsync(() =>
                            Messages.Messages.Add(new WorkspaceMessage
                            {
                                Source = e.Diagnostic, Message = e.Diagnostic.Message,
                                Severity = e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure
                                    ? WorkspaceMessageSeverity.Error
                                    : WorkspaceMessageSeverity.Warning
                            }));
                    
                };
                OnPropertyChanged();
            }
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IClientModel _clientViewModel;
        

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
        // ReSharper disable once CollectionNeverQueried.Global
        public List<WorkspaceDiagnostic> Diagnostics { get; set; } = new List<WorkspaceDiagnostic>();

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ProjectAddedEventArgs> ProjectedAddedEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DocumentAddedEventArgs> DocumentAddedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private async void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            DebugUtils.WriteLine(e.Kind.ToString());
            switch (e.Kind)
            {
                case WorkspaceChangeKind.SolutionRemoved:
                    HierarchyRoot.Remove(HierarchyRoot.First(s => s.Id == e.OldSolution.Id));
                    break;
                case WorkspaceChangeKind.SolutionChanged:
                    e.NewSolution.GetChanges(e.OldSolution);

                    break;
                case WorkspaceChangeKind.SolutionAdded:
                    var solutionModel = SolutionModelFromSolution(e.NewSolution);
                    HierarchyRoot.Add(solutionModel);
                    break;
                case WorkspaceChangeKind.SolutionCleared:
                    break;
                case WorkspaceChangeKind.SolutionReloaded:
                    break;
                case WorkspaceChangeKind.ProjectAdded:
                    var eNewSolution = e.NewSolution;
                    var m = GetSolution(eNewSolution.Id);
                    if (m == default(SolutionModel))
                    {
                        var solutionModel1 = SolutionModelFromSolution(e.NewSolution);
                        HierarchyRoot.Add(solutionModel1);
                        m = solutionModel1;
                    }

                    var projectModel = ProjectFromModel(m, e.NewSolution.GetProject(e.ProjectId));
                    m.Projects.Add(projectModel);
                    ProjectedAddedEvent?.Invoke(this, new ProjectAddedEventArgs() {Model = projectModel});
                    break;
                case WorkspaceChangeKind.ProjectRemoved:
                    break;
                case WorkspaceChangeKind.ProjectChanged:
                    break;
                case WorkspaceChangeKind.ProjectReloaded:
                    break;
                case WorkspaceChangeKind.DocumentAdded:
                    var p0 = GetProjectModel(e.ProjectId);
                    var d = new DocumentModel(p0, Workspace);
                    var doc = e.NewSolution.GetDocument(e.DocumentId);
                    if (doc != null)
                    {
                        d.Name = doc.Name;
                        d.FilePath = doc.FilePath;
                        d.Id = doc.Id.Id;
                        d.DocumentId = doc.Id;
                    }

                    Compilation compilation = null;
                    SemanticModel model = null;

                    if (d.Project.Project.SupportsCompilation)
                    {
                        if (doc != null) compilation = await doc.Project.GetCompilationAsync();
                        if (compilation != null)
                        {
                            var errs = compilation.GetDiagnostics()
                                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
                            var diagnostics = errs.ToList();
                            foreach (var diagnostic in diagnostics)
                            {
                                var docs = DiagnosticNodeModel.CreateInstance(PathModelKind.Diagnostic);
                                docs.Diagnostic = diagnostic;
                                d.Project.Diag.Add(docs);
                            }

                            DebugUtils.WriteLine(String.Join("\n", diagnostics));
                            if (diagnostics.Any())
                            {
                                //compilation = null;
                            }
                            else
                            {
                            }
                        }
                    }

                    model = await doc.GetSemanticModelAsync();


                    var tree = await doc.GetSyntaxTreeAsync();


                    p0.Documents.Add(d);
                    DocumentAddedEvent?.Invoke(this, new DocumentAddedEventArgs() {Document = d});

                    break;
                case WorkspaceChangeKind.DocumentRemoved:
                    break;
                case WorkspaceChangeKind.DocumentReloaded:
                    break;
                case WorkspaceChangeKind.DocumentChanged:
                    var ch = e.NewSolution.GetChanges(e.OldSolution);
                    foreach (var addedProject in ch.GetAddedProjects())
                        DebugUtils.WriteLine(addedProject.Id.Id.ToString());
                    foreach (var projectChanges in ch.GetProjectChanges())
                    {
                        foreach (var addedAdditionalDocument in projectChanges.GetAddedAdditionalDocuments())
                            DebugUtils.WriteLine(addedAdditionalDocument.Id.ToString());

                        foreach (var addedDocument in projectChanges.GetAddedDocuments())
                            DebugUtils.WriteLine(addedDocument.Id.ToString());
                        foreach (var changedDocument in projectChanges.GetChangedDocuments())
                            DebugUtils.WriteLine(changedDocument.Id.ToString());
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
            if (newSolutionId == null) newSolutionId = _workspace.CurrentSolution.Id;
            var m = HierarchyRoot.FirstOrDefault(m1 => m1.Id == newSolutionId);
            return m;
        }

        private ProjectModel GetProjectModel(ProjectId id)
        {
            return HierarchyRoot.SelectMany(s => s.Projects).First(p => p.Id == id);
        }

        // ReSharper disable once UnusedMember.Local
        private DocumentModel GetDocumentModel(DocumentId id)
        {
            return GetProjectModel(id.ProjectId).Documents.FirstOrDefault(z => z.Id == id.Id);
        }

        private static SolutionModel SolutionModelFromSolution(Solution s)
        {
            var m = new SolutionModel(s.Workspace) {Id = s.Id, FilePath = s.FilePath};
            foreach (var sProject in s.Projects) m.Projects.Add(ProjectFromModel(m, sProject));
            return m;
        }

        private static ProjectModel ProjectFromModel(SolutionModel s, Project sProject)
        {
            var p = new ProjectModel(s.Workspace)
            {
                Name = sProject.Name, Id = sProject.Id, Solution = s, FilePath = sProject.FilePath
            };
            //p.Project = sProject;
            foreach (var sProjectDocument in sProject.Documents)
            {
                var d = DocumentFromModel(p, sProjectDocument);
                p.Documents.Add(d);
            }

            return p;
        }

        private static DocumentModel DocumentFromModel(ProjectModel project, Document sProjectDocument)
        {
            var d = new DocumentModel(project, project.Workspace)
            {
                Name = sProjectDocument.Name,
                FilePath = sProjectDocument.FilePath,
                Id = sProjectDocument.Id.Id,
                DocumentId = sProjectDocument.Id
            };
            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SolutionModel> HierarchyRoot { get; set; } = new ObservableCollection<SolutionModel>();

       

        /// <summary>
        /// 
        /// </summary>
        public ProjectModel SelectedProject
        {
            get { return _workspaceView?.SelectedProject; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
                    // ReSharper disable once ExpressionIsAlwaysNull
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
                foreach (var servicesSupportedLanguage in ww.Services.SupportedLanguages)
                {
                    LogManager.GetCurrentClassLogger().Info(servicesSupportedLanguage);
                }

                if (!ww.Services.IsSupported(LanguageNames.CSharp))
                {
                    return null;
                }
                var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "unnamed project",
                    "unnamed assembly", LanguageNames.CSharp
                ).WithMetadataReferences(new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
                });

                var news = ww.CurrentSolution.AddProject(projectInfo);
                if (ww.TryApplyChanges(news)) return projectInfo;
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
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task LoadSolutionAsync(string file)
        {
            //TrySelectVsInstance();
            IDictionary<string, string> props = new Dictionary<string, string>();
            props["Platform"] = "x86";
            var msBuildWorkspace = MSBuildWorkspace.Create(props);
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() {Description = "load solution"};
            Solution r = null;
            try
            {
                r = await msBuildWorkspace.OpenSolutionAsync(file);
                    // new ProgressWithCompletion<ProjectLoadProgress>(Handler));
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }

            CurrentOperation = null;
            DebugUtils.WriteLine(r?.ToString() ?? "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task LoadProjectAsync(string file)
        {
            //TrySelectVsInstance();
            IDictionary<string, string> props = new Dictionary<string, string>();
            props["Platform"] = "x86";
            var msBuildWorkspace = MSBuildWorkspace.Create(props);
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() { Description = "load solution" };
            var r = await msBuildWorkspace.OpenProjectAsync(file);//ProgressWithCompletion<ProjectLoadProgress>(Handler));
            CurrentOperation = null;
            DebugUtils.WriteLine(r.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
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
        // ReSharper disable once MemberCanBePrivate.Global
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
            // ReSharper disable once UnusedMember.Global
            set
            {
                if (Equals(value, _workspaceView)) return;
                _workspaceView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public Dispatcher Dispatcher { get; set; }

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
                doc = p.Item as DocumentModel;
            else if (eParameter is DocumentModel dd) doc = dd;
            if (doc == null) doc = _workspaceView.SelectedDocument;
            if (doc != null)
            {
                CurrentOperation = new CurrentOperation() {Description = "Open document " + doc.Name};

                //Workspace.OpenDocument(doc.Document.Id, true);
                Compilation compilation = null;
                SemanticModel model = null;

                var docDocument = doc.Document;
                if (doc.Project.Project.SupportsCompilation)
                {
                    compilation = await doc.Project.Project.GetCompilationAsync();
                    if (compilation != null)
                    {
                        var errs = compilation.GetDiagnostics()
                            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
                        var diagnostics = errs.ToList();
                        DebugUtils.WriteLine(String.Join("\n", diagnostics));
                        if (diagnostics.Any())
                        {
                            compilation = null;
                        }
                        else
                        {
                        }
                    }

                    if (docDocument != null) model = await docDocument.GetSemanticModelAsync();
                }

                if (docDocument != null)
                {
                    var tree = await docDocument.GetSyntaxTreeAsync();
                    var doc2 = new CodeDocument(tree, compilation) {Title = doc.Name,Model=model};
                    _docHost.AddDocument(doc2);
                }//todo fixme

                CurrentOperation = null;
            }
            else if (SelectedProject != null)
            {
                var semanticControl1 = new SemanticControl1();
                var continueWith = SelectedProject.Project.GetCompilationAsync().ContinueWith(
                    task => { semanticControl1.Compilation = (CSharpCompilation) task.Result; }, taskScheduler);

                var anchorableModel = new AnchorableModel {Content = semanticControl1};
                _anchHost.AddAnchorable(anchorableModel);
            }
        }

        public CurrentOperation CurrentOperation { get; set; }

        public IList Documents { get; set; }

        /// 
        /// </summary>
        /// <param name="e"></param>
        // ReSharper disable once UnusedMember.Global
        private void OnDocumentAddedEvent(DocumentAddedEventArgs e)
        {
            DocumentAddedEvent?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextSyntaxTree"></param>
        /// <param name="cSharpCompilation"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        ///
        public delegate DocModel Del1(SyntaxTree contextSyntaxTree, Compilation cSharpCompilation, string file);

        public static async Task CodeDocAsync(Main1Mode2 main1Mode2, SyntaxTree contextSyntaxTree, Compilation cSharpCompilation, string file)
        { 
            await main1Mode2.Dispatcher.BeginInvoke(new Del1(main1Mode2.Method1), contextSyntaxTree, cSharpCompilation, file);
        }

        private DocModel Method1(SyntaxTree contextSyntaxTree, Compilation cSharpCompilation, string file)
        {
            var doc = new CodeDocument(contextSyntaxTree, cSharpCompilation)
            {
                Title = Path.GetFileNameWithoutExtension(file)
            };
                _docHost.AddDocument(doc);
            _docHost.SetActiveDocument(doc);

            return doc;
        }

        public void CreateDocument()
        {
            //DocumentInfo d = DocumentInfo.Create(DocumentId.CreateNewId(SelectedProject.Id),"test");
            var m = GetSolution();
            if (SelectedProject != null)
            {
                var n2 = Workspace.CurrentSolution.AddDocument(DocumentId.CreateNewId(SelectedProject.Id), "test", SourceText.From(""));
                Workspace.TryApplyChanges(n2);
            }
        }

        public void CreateClass()
        {
            //var q = QuickInfoService.GetService(SelectedProject.Documents.First().Document)
            //Workspace.Services.GetLanguageServices(LanguageNames.CSharp).GetServ;ice<ILanguageService>()
            //
        }
    }

    internal class CodeDocument : DocModel
    {
        /// <inheritdoc />
        public CodeDocument()
        {
            CodeControl = new FormattedTextControl3()
            {
                SourceText = ""
            };

        }

        /// <inheritdoc />
        public CodeDocument(SyntaxTree syntaxTree, Compilation compilation)
        {
            SyntaxTree = syntaxTree;
            Compilation = compilation;
            var model = Compilation?
                .GetSemanticModel(SyntaxTree);
            CodeControl  = new FormattedTextControl3()
            {
                SyntaxTree = syntaxTree,
                Compilation = compilation,
                Model = model
            };
            
        }

        /// <inheritdoc />
        public override IEnumerable ContextualTabGroupHeaders =>
            new[] {"Code Analysis"};

        /// <inheritdoc />
        public override object Content => CodeControl;

        public SyntaxNodeControl CodeControl { get; set; }

        public SyntaxTree SyntaxTree { get; set; }
        public Compilation Compilation { get; set; }
        public SemanticModel Model { get; set; }
    }
}