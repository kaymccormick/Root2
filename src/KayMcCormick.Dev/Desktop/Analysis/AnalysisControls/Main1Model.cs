using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AnalysisControls.Properties;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Threading;
using NLog;
using Path = System.IO.Path;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Main1Model : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public object ActiveContent

        {
            // ReSharper disable once UnusedMember.Global
            get { return _activeContent; }
            set
            {
                if (Equals(value, _activeContent)) return;
                var old = _activeContent;
                _activeContent = value;
                if (value is DocModel d)
                {
                    if (old is DocModel dd)
                    {
                        foreach (var xx in dd.ContextualTabGroupHeaders.Where(x =>
                            !d.ContextualTabGroupHeaders.Contains(x)))
                        {
                            ContextualTabGroups.Remove(xx);
                            foreach (var primaryRibbonContextualTabGroup in ClientViewModel.PrimaryRibbon.ContextualTabGroups)
                            {
                                if (Equals(primaryRibbonContextualTabGroup.Header, xx))
                                {
                                    primaryRibbonContextualTabGroup.Visibility = Visibility.Collapsed;
                                }
                            }
                            foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                            {
                                if (Object.Equals(primaryRibbonRibbonItem.ContextualTabGroupHeader, xx))
                                {
                                    primaryRibbonRibbonItem.Visibility = Visibility.Collapsed;
                                }
                            }


                        }
                    }

                    foreach (var header in d.ContextualTabGroupHeaders)
                    {
                        if (ContextualTabGroups.Contains(header))
                        {
                            continue;
                        }
                        DebugUtils.WriteLine("Adding group " + header);
                        ContextualTabGroups.Add(header);
                        foreach (var primaryRibbonContextualTabGroup in ClientViewModel.PrimaryRibbon.ContextualTabGroups)
                        {
                            if (Equals(primaryRibbonContextualTabGroup.Header, header))
                            {
                                primaryRibbonContextualTabGroup.Visibility = Visibility.Visible;
                            }
                        }
                        foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                        {
                            if (Object.Equals(primaryRibbonRibbonItem.ContextualTabGroupHeader, header))
                            {
                                primaryRibbonRibbonItem.Visibility = Visibility.Visible;
                            }
                        }
                        
                    }
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> ContextualTabGroups
        {
            get { return _contextualTabGroups; }
            set
            {
                if (Equals(value, _contextualTabGroups)) return;
                _contextualTabGroups = value;
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
        private ObservableCollection<object> _contextualTabGroups = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SelectVsInstance()
        {
            if (TrySelectVsInstance())
                return;
            throw new AppInvalidOperationException("Cant register");
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

            var visualStudioInstances = vsInstances as VisualStudioInstance[] ?? vsInstances.ToArray();
            foreach (var vsi in visualStudioInstances) DebugUtils.WriteLine($"{vsi.Name} {vsi.Version}");

            var versions = visualStudioInstances.Select(x => x.Version.Major).Distinct().OrderByDescending(i => i)
                .ToList();
            DebugUtils.WriteLine(String.Join(", ", versions));
            var inst = versions.FirstOrDefault();


            var visualStudioInstance = visualStudioInstances.Where(instance => instance.Version.Major == inst)
                .OrderByDescending(instance => instance.Version).FirstOrDefault();
            DebugUtils.WriteLine($"Registering {visualStudioInstance}");
            MSBuildLocator.RegisterInstance(visualStudioInstance);
            VisualStudioInstance = visualStudioInstance;
            return true;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private static VisualStudioInstance VisualStudioInstance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Documents { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public ObservableCollection<object> Anchorables { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replay"></param>
        public Main1Model(ReplaySubject<Workspace> replay) : this()
        {
            _replay = replay;
        }

        /// <summary>
        /// 
        /// </summary>
        public Main1Model()
        {
            AddInitialDocuments();

            AddInitialAnchorables();
        }

        private void AddInitialDocuments()
        {
            AddModelDoc();
            AddRibbonModelViewDoc();
            AddAssembliesDoc();
            AddPropertiesGridDoc();
            AddControlsDoc();
        }

        private void AddRibbonModelViewDoc()
        {
            var c = new RibbonModelView();
            c.SetBinding(RibbonModelView.RibbonModelProperty, new Binding("ClientViewModel.PrimaryRibbon") { Source = this });
            var doc = DocModel.CreateInstance();
            doc.Title = "Ribbon Model View";
            doc.Content = c;
            Documents.Add(doc);
        }

        private void AddControlsDoc()
        {
            var item = DocModel.CreateInstance();
            item.Title = "Controls";
            item.Content = new ControlView();
            Documents.Add(item);
        }

        private void AddPropertiesGridDoc()
        {
            var userControl1 = new UserControl1();
            var x = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch
            };
            x.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            x.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)});
            x.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});
            var rectangle = new Rectangle {Width = 100, Height = 50, Fill = Brushes.Red, AllowDrop = true};
            x.Children.Add(rectangle);
            rectangle.SetValue(Grid.RowProperty, 0);
            rectangle.SetValue(Grid.ColumnProperty, 0);
            rectangle.DragOver += (sender, e) =>
            {
                DebugUtils.WriteLine("Drag over");
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            };
            rectangle.Drop += (sender, args) =>
            {
                var d = args.Data;
                object o = null;
                if (d.GetDataPresent("ModelObject"))
                {
                    o = d.GetData("ModelObject");
                }
                else
                {
                    foreach (var format in d.GetFormats())
                    {
                        var oo = d.GetData(format);
                        if (oo.GetType().IsPrimitive)
                        {
                        }
                        else
                        {
                            o = oo;
                            break;
                        }
                    }
                }

                userControl1.propertyGrid1.SelectedObject = o;
                args.Effects = DragDropEffects.Copy;
                args.Handled = true;
            };

            var windowsFormsHost = new WindowsFormsHost() {Child = userControl1};
            windowsFormsHost.SetValue(Grid.RowProperty, 1);
            windowsFormsHost.SetValue(Grid.ColumnProperty, 0);

            x.Children.Add(windowsFormsHost);
            var item = DocModel.CreateInstance();
            item.Content = x;
            Documents.Add(item);
        }

        private void AddAssembliesDoc()
        {
            var assembliesDoc = DocModel.CreateInstance();
            assembliesDoc.Title = "Assemblies";
            assembliesDoc.Content = new AssembliesControl {AssemblySource = AppDomain.CurrentDomain.GetAssemblies()};
            assembliesDoc.ContextualTabGroupHeaders.Add("Assemblies");
            Documents.Add(assembliesDoc);
        }

        private void AddModelDoc()
        {
            TablePanel t1 = new TablePanel();
            TypeControl dev = new TypeControl();
            dev.SetBinding(AttachedProperties.RenderedTypeProperty,
                new Binding("ActiveContent.Content") {Source = this, Converter = new GetTypeConverter()});
            TextBlock h1 = new TextBlock {Text = "Active Document"};
            t1.Children.Add(h1);
            t1.Children.Add(dev);
            TextBlock h2 = new TextBlock {Text = "Contextual Tab Group Headers"};
            var lv1 = new ListBox();
            lv1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(ContextualTabGroups)) {Source = this});
            t1.Children.Add(h2);
            t1.Children.Add(lv1);
            var uiElement = new Button() { };
            uiElement.Click += (sender, args) =>
            {
                var prop = typeof(Ribbon).GetProperty("ContextualTabGroupItemsControl",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                var ctl = prop.GetValue(ClientViewModel.Ribbon);
                DebugUtils.WriteLine(ctl.ToString());
            };
            t1.Children.Add(uiElement);

            //b.SetBinding(TextBlock.TextProperty, new Binding("ActiveDocument.Content") {Source = this});

            var item = DocModel.CreateInstance();
            item.Title = "Model";
            item.Content = t1;
            Documents.Add(item);
        }

        private void AddInitialAnchorables()
        {
            var tv = new TreeView()
            {
                DisplayMemberPath = "Header"
            };
            tv.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding("ClientViewModel.PrimaryRibbon.RibbonItems")
                    {Source = this});
            Anchorables.Add(new AnchorableModel() {Title = "PrimaryRibbon Tabs", Content = tv});

            var lv = new ListBox();
            lv.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding("ClientViewModel.PrimaryRibbon.ContextualTabGroups")
                {
                    Source = this
                });
            Anchorables.Add(new AnchorableModel() {Title = "Contextual Tab Groups", Content = lv});
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
                Anchorables.Add(new AnchorableModel()
                {
                    Content =
                        _workspaceView,
                    Title = "Workspace"
                });
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
                        // ReSharper disable once UnusedVariable
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
                                compilation = null;
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
        public async Task LoadSolutionAsync(string file)
        {
            TrySelectVsInstance();
            IDictionary<string, string> props = new Dictionary<string, string>();
            props["Platform"] = "x86";
            var msBuildWorkspace = MSBuildWorkspace.Create(props);
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() {Description = "load solution"};
            Solution r = null;
            try
            {
                r = await msBuildWorkspace.OpenSolutionAsync(file,
                    new ProgressWithCompletion<ProjectLoadProgress>(Handler));
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
            TrySelectVsInstance();
            IDictionary<string, string> props = new Dictionary<string, string>();
            props["Platform"] = "x86";
            var msBuildWorkspace = MSBuildWorkspace.Create(props);
            Workspace = msBuildWorkspace;
            CurrentOperation = new CurrentOperation() { Description = "load solution" };
            var r = await msBuildWorkspace.OpenProjectAsync(file,
                new ProgressWithCompletion<ProjectLoadProgress>(Handler));
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

        /// <summary>
        /// 
        /// </summary>
        public IClientModel ClientViewModel
        {
            get { return _clientViewModel; }
            set
            {
                _clientViewModel = value;
                OnPropertyChanged();
                new UserControl1().propertyGrid1.SelectedObject = _clientViewModel.PrimaryRibbon;
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
                doc = p.Item as DocumentModel;
            else if (eParameter is DocumentModel dd) doc = dd;
            if (doc == null) doc = _workspaceView.SelectedDocument;
            if (doc != null)
            {
                CurrentOperation = new CurrentOperation() {Description = "Open document " + doc.Name};

                //Workspace.OpenDocument(doc.Document.Id, true);
                Compilation compilation = null;
                SemanticModel model = null;

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

                    model = await doc.Document.GetSemanticModelAsync();
                }

                var tree = await doc.Document.GetSyntaxTreeAsync();
                var c = new FormattedTextControl()
                {
                    SyntaxTree = tree,
                    Compilation = (CSharpCompilation) compilation,
                    Model = model
                };
                var doc2 = DocModel.CreateInstance();
                doc2.Title = doc.Name;
                doc2.Content = c;
                doc2.ContextualTabGroupHeaders.Add(RibbonResources.ContextualTabGroupHeader_CodeAnalysis);
                Documents.Add(doc2);
                ActiveContent = doc2;
                CurrentOperation = null;
            }
            else if (SelectedProject != null)
            {
                var semanticControl1 = new SemanticControl1();
                var continueWith = SelectedProject.Project.GetCompilationAsync().ContinueWith(
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
                var listBox = new ListBox
                {
                    ItemTemplate = (DataTemplate) View.TryFindResource(new DataTemplateKey(typeof(ISymbol)))
                };
                var comp = await pm.Project.GetCompilationAsync();
                if (comp != null)
                {
                    var errs = comp.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error);
                    var diagnostics = errs.ToList();
                    if (diagnostics.Any())
                    {
                        DebugUtils.WriteLine("ErrorsList");
                        foreach (var diagnostic in diagnostics) DebugUtils.WriteLine(diagnostic.ToString());
                    }
                }

                if (comp != null)
                {
                    var listBoxItemsSource = comp.GetSymbolsWithName(x => true).ToList();
                    DebugUtils.WriteLine($"{listBoxItemsSource.Count} symbols");
                    listBox.ItemsSource = listBoxItemsSource;
                }

                var item = DocModel.CreateInstance();
                item.Title = "Symbols for " + pm.Name;
                item.Content = listBox;
                Documents.Add(item);
            }
        }

        /// <summary>
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
        public static DocModel CodeDoc(SyntaxTree contextSyntaxTree, Compilation cSharpCompilation, string file)
        {
            var doc = DocModel.CreateInstance();
            doc.Content = new FormattedTextControl()
            {
                SyntaxTree = contextSyntaxTree,
                Compilation = cSharpCompilation
            };
            doc.Title = Path.GetFileNameWithoutExtension(file);
            doc.ContextualTabGroupHeaders.Add(RibbonResources.ContextualTabGroupHeader_CodeAnalysis);

            return doc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExecutedPaste(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var i = Clipboard.GetImage();
                var img = new Image {Source = i};
                var doc = DocModel.CreateInstance();
                doc.Content = img;
                Documents.Add(doc);
                ActiveContent = doc;
            }
            // var  d =Clipboard.GetDataObject();
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticNodeModel : PathModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DiagnosticNodeModel CreateInstance(PathModelKind kind)
        {
            return new DiagnosticNodeModel(kind);
        }

        private Diagnostic _diagnostic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        private DiagnosticNodeModel(PathModelKind kind) : base(kind)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docs"></param>
        public override void Add(PathModel docs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Diagnostic Diagnostic
        {
            get { return _diagnostic; }
            set
            {
                _diagnostic = value;
                Message = _diagnostic.GetMessage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}