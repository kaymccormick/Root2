using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reactive.Subjects;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xaml;
using AnalysisControl;
using AnalysisControls.TypeDescriptors;
using AnalysisControlsCore;
using Castle.DynamicProxy;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using KmDevLib;
using KmDevWpfControls;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Identity.Client;
using NLog;
using RibbonLib.Model;

using Logger = NLog.Logger;
using TypeControl = KayMcCormick.Lib.Wpf.TypeControl;
using XamlReader = System.Windows.Markup.XamlReader;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Main1Model : DependencyObject, INotifyPropertyChanged,
        ICommandProvider,
        ISubjectWatcher, IMain1Model


    {
        //private IPublicClientApplication a\pp;

        public IDocumentHost DocHost

        {
            get { return _docHost; }
            set
            {
                if (Equals(value, _docHost)) return;
                _docHost = value;
                // if (_docHost != null)
                // {
                // var x = new CSharpEditorControl(_docHost);
                // x.ExecuteAsync(null);
                // }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Documents));
            }
        }

        public static readonly DependencyProperty CatchphraseProperty = DependencyProperty.Register(
            "Catchphrase", typeof(string), typeof(Main1Model), new PropertyMetadata(default(string)));


        public virtual string Catchphrase
        {
            get { return (string) GetValue(CatchphraseProperty); }
            set { SetValue(CatchphraseProperty, value); }
        }

        public static readonly DependencyProperty ActiveContentProperty = DependencyProperty.Register(
            "ActiveContent", typeof(object), typeof(Main1Model),
            new PropertyMetadata(default(object), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Main1Model) d).OnActiveContentChanged(e.OldValue, e.NewValue);
        }

        private void OnActiveContentChanged(object eOldValue, object eNewValue)
        {
            DebugUtils.WriteLine("new active document is " + eNewValue);
            if (eNewValue is DocModel dm00)
                if (dm00.Content is Control cc)
                    cc.Focus();

            if (ClientViewModel != null)
                if (ClientViewModel.PrimaryRibbon != null)
                    ClientViewModel.PrimaryRibbon.ActiveContent = _activeContent;

            if (eOldValue is DocModel dd0) dd0.IsActive = false;

            if (eNewValue is DocModel d)
            {
                d.IsActive = true;
                if (ClientViewModel == null)
                    return;
                foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                foreach (var item in primaryRibbonRibbonItem.Items)
                    if (item is RibbonModelGroup g)
                        if (g.Header != null && g.Header.Equals("Context"))
                        {
                            g.Items.Clear();

                            foreach (var dRibbonItem in d.RibbonItems) g.Items.Add((RibbonModelItem) dRibbonItem);
                        }

                if (eOldValue is DocModel dd)
                {
                    dd.IsActive = false;
                    var ddContextualTabGroupHeaders = dd.ContextualTabGroupHeaders.Cast<object>();
                    foreach (var xx in ddContextualTabGroupHeaders.Where(x =>
                        !ddContextualTabGroupHeaders.Contains(x)))
                    {
                        ContextualTabGroups.Remove(xx);
                        if (ClientViewModel?.PrimaryRibbon != null)
                        {
                            foreach (var primaryRibbonContextualTabGroup in ClientViewModel.PrimaryRibbon
                                .ContextualTabGroups)
                                if (Equals(primaryRibbonContextualTabGroup.Header, xx))
                                    primaryRibbonContextualTabGroup.Visibility = Visibility.Collapsed;

                            foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                                if (Equals(primaryRibbonRibbonItem.ContextualTabGroupHeader, xx))
                                    primaryRibbonRibbonItem.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                foreach (var header in d.ContextualTabGroupHeaders)
                {
                    if (ContextualTabGroups.Contains(header)) continue;

                    DebugUtils.WriteLine("Adding group " + header);
                    ContextualTabGroups.Add(header);
                    if (ClientViewModel?.PrimaryRibbon != null)
                    {
                        foreach (var primaryRibbonContextualTabGroup in ClientViewModel.PrimaryRibbon
                            .ContextualTabGroups)
                            if (Equals(primaryRibbonContextualTabGroup.Header, header))
                                primaryRibbonContextualTabGroup.Visibility = Visibility.Visible;

                        foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                            if (Equals(primaryRibbonRibbonItem.ContextualTabGroupHeader, header))
                            {
                                primaryRibbonRibbonItem.Visibility = Visibility.Visible;
                                primaryRibbonRibbonItem.OnContextualTabGroupActivated(this,
                                    new ContextualTabGroupActivatedHandlerArgs(d));
                            }
                    }
                }
            }
        }

        [Browsable(false)]
        public virtual object ActiveContent
        {
            get { return (object) GetValue(ActiveContentProperty); }
            set { SetValue(ActiveContentProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        ///
        /// 
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public virtual ObservableCollection<object> ContextualTabGroups
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
        private readonly IActivationStream _ss;
        private readonly MySubjectReplaySubject _impl2;

        public virtual IContentSelector ContentSelector
        {
            get { return _contentSelector; }
            set
            {
                if (Equals(value, _contentSelector)) return;
                _contentSelector = value;
                OnPropertyChanged();
            }
        }

        private readonly IEnumerable<IMySubject> _subs;
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
                    // new VisualStudioInstanceQueryOptions
                    // {
                    // DiscoveryTypes =
                    // DiscoveryType.VisualStudioSetup
                    // }
                );

            var visualStudioInstances = vsInstances as VisualStudioInstance[] ?? vsInstances.ToArray();
            foreach (var vsi in visualStudioInstances) DebugUtils.WriteLine($"{vsi.Name} {vsi.Version}");

            var versions = visualStudioInstances.Select(x => x.Version.Major).Distinct().OrderByDescending(i => i)
                .ToList();
            DebugUtils.WriteLine(string.Join(", ", versions));
            var inst = versions.FirstOrDefault();


            var visualStudioInstance1 = visualStudioInstances.Where(instance => instance.Version.Major == inst)
                .OrderByDescending(instance => instance.Version);
            if (visualStudioInstance1.Any())
            {
                var visualStudioInstance = visualStudioInstance1.First();
                DebugUtils.WriteLine($"Registering {visualStudioInstance}");
                MSBuildLocator.RegisterInstance(visualStudioInstance);
                VisualStudioInstance = visualStudioInstance;


                return true;
            }

            return false;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private static VisualStudioInstance VisualStudioInstance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public virtual ObservableCollection<object> DocumentsCollection
        {
            get { return _documents; }
            set
            {
                if (Equals(value, _documents)) return;
                _documents = value;
                _documents1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Documents));
            }
        }

        [Browsable(false)]
        public virtual IEnumerable Documents
        {
            get { return DocHost.Documents; }
        }

        public virtual AppSettingsViewModel AppSettingsViewModel { get; set; }
        public virtual IUserSettingsDbContext UserSettingsDbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        [Browsable(false)]
        public virtual ObservableCollection<object> Anchorables { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replay"></param>
        public Main1Model(ReplaySubject<Workspace> replay, IActivationStream ss, MySubjectReplaySubject impl2,
            IDocumentHost dochost, IContentSelector contentSelector, AppSettingsViewModel appSettingsViewModel,
            IUserSettingsDbContext userSettingsDbContext) : this()
        {
            DocHost = dochost;

            //JsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();
            _replay = replay;
            _ss = ss;
            _impl2 = impl2;
            ContentSelector = contentSelector;
            AppSettingsViewModel = appSettingsViewModel;
            UserSettingsDbContext = userSettingsDbContext;
            _impl2.Subject.Subscribe(subject => { Subject(subject); });
            //_subs = subs;
            Subject(ss);
            // Subject(s1);
        }

        /// <summary>
        /// 
        /// </summary>
        public Main1Model()
        {
            // this.app = app;

            BindingOperations.SetBinding(this, ActiveContentProperty,
                new Binding("ContentSelector.ActiveContent") {Source = this});
            DocumentsCollection = new ObservableCollection<object>();
            Anchorables = new ObservableCollection<object>();
            //_r = r;
            // _aReplaySubject = aReplaySubject;
            // _aReplaySubject.SubscribeOn(Scheduler.Default)
            // .ObserveOnDispatcher(DispatcherPriority.Background)
            // .Subscribe(x =>
            // {
            // if (x.Instance.GetType().IsGenericType &&
            // x.Instance.GetType().GetGenericTypeDefinition() == typeof(ReplaySubject<>))
            // {


            // var m = x.InstanceType.GetMethod("Subscribe");
            // Subject<object> subj = new Subject<object>();
            // Action<object> xxx = (o) => { subj.OnNext(o); };
            // m.Invoke(x.Instance, new object[] {xxx});

            // ObservableCollection<object> c = new ObservableCollection<object>();
            // var itemsControl = new ItemsControl { Resources = null, 
            // ItemsSource = c };
            // subj.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send)
            // .Subscribe(c.Add);


            // var dm = DocModel.CreateInstance();
            // dm.Content = itemsControl;
            // _documents.Add(dm);
            // }
            // });

            if (DocumentsCollection != null)
                DocumentsCollection.CollectionChanged += (sender, args) =>
                {
                    //foreach (var argsNewItem in args.NewItems) DebugUtils.WriteLine(argsNewItem);
                };
        }

        private void AddInitialDocuments()
        {
            AddtraceConfigurationVoew();

            //            AddModelDoc();
            // AddRibbonModelViewDoc();
            // AddRibbonModelViewDoc1();
            // AddAssembliesDoc();
            //            AddPropertiesGridDoc();
            //AddVisualTreeViewDoc();
            AddContainerView();
            // AddTypeProvider();

            // AddPowerShell();

            // ObservableCollection<CodeElementDocumentation> coll = new ObservableCollection<CodeElementDocumentation>();
            // DocModel dm = DocModel.CreateInstance();
            // dm.Content = new ScrollViewer
            // {
            // Content =
            // AnalysisControlsModule.ReplayItemsControl<CodeElementDocumentation>(coll, _r, ControlsResources("templates.baml" ))
            // };
            // Documents.Add(dm);
        }

        private void AddGenericInterface3Document()
        {
            var gi = new GenericInterface3();
            gi.Instance = ClientViewModel?.PrimaryRibbon;
            var doc = DocModel.CreateInstance("Generic");
            doc.Content = gi;
            AddDocument(doc);
        }

        public static ResourceDictionary ControlsResources(string filename)
        {
            var assembly = typeof(AnalysisControlsModule).Assembly;
            var x = new ResourceManager(
                "AnalysisControlsCore.g"
                , assembly
            );

            var y = x.GetStream(filename);
            if (y == null) throw new AppInvalidOperationException("Unable to get resource stream for " + filename);
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader(y, new XamlReaderSettings());

            var oo = (ResourceDictionary) XamlReader.Load(b);
            return oo;
        }

#if false
private void AddPowerShell2()
        {
            var terminal0 = new TerminalUserControl0();
            var proxy = ProxyGeneratorHelper.ProxyGenerator.CreateClassProxyWithTarget<Main1Model>(this,
                new DispatcherProxyInterceptor(Dispatcher));
            terminal0.Shell.Host.SetPrivateData(new PSObject(proxy));

            var doc = DocModel.CreateInstance();
            doc.Title = "PowerShell 0";
            doc.Content = terminal0;
            AddDocument(doc);
        }
#endif
        // private void AddVisualTreeViewDoc()
        // {
        // var c = new VisualTreeView();
        // var doc = DocModel.CreateInstance();
        // doc.Title = "Visual Tree View";
        // doc.Content = c;
        // Documents.Add(doc);
        // }

        private void AddContainerView()
        {
            var c = new ContainerView();
            var doc = DocModel.CreateInstance();
            doc.Title = "Container View";
            doc.Content = c;
            AddDocument(doc);
        }

        private void AddTypeProvider()
        {
            var c = new TypeProviderUserControl();
            var doc = DocModel.CreateInstance();
            doc.Title = "Type Provider";
            doc.Content = c;
            AddDocument(doc);
        }

        private void AddRibbonModelViewDoc()
        {
            var c = new RibbonModelView();
            c.SetBinding(RibbonModelView.RibbonModelProperty,
                new Binding("ClientViewModel.PrimaryRibbon") {Source = this});
            var doc = DocModel.CreateInstance();
            doc.Title = "MyRibbon Model View";
            doc.Content = c;
            AddDocument(doc);
        }

        private void AddRibbonModelViewDoc1()
        {
            var c = new DropControl();
            //c.SetBinding(RibbonModelView.RibbonModelProperty, new Binding("ClientViewModel.PrimaryRibbon") { Source = this });
            var doc = DocModel.CreateInstance();
            doc.Title = "Drop Debug";
            doc.Content = c;
            AddDocument(doc);
        }

        private void AddControlsDoc()
        {
            var item = DocModel.CreateInstance();
            item.Title = "Controls";
            item.Content = new ControlView();
            AddDocument(item);
        }

        private void AddtraceConfigurationVoew()
        {
            var item = DocModel.CreateInstance();
            item.Title = "Trace Configuration";
            item.Content = new TraceView {ListenerTypes = new[] {typeof(XmlWriterTraceListener)}};

            AddDocument(item);
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
                    o = d.GetData("ModelObject");
                else
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
            AddDocument(item);
        }

        private void AddPowerShell()
        {
            var assembliesDoc = DocModel.CreateInstance();
            assembliesDoc.Title = "PowerShell";
            var powershell = new PowerShellConsole() { };
            powershell.Loaded += (sender, args) => powershell.Shell?.Host?.SetPrivateData(new PSObject(this));
            var tryFindResource = (ControlTemplate) View.TryFindResource("PowerShellTemplate");
            if (tryFindResource != null) powershell.Template = tryFindResource;
            powershell.ApplyTemplate();

            assembliesDoc.Content = powershell;

            AddDocument(assembliesDoc);
        }

        private void AddAssembliesDoc()
        {
            var assembliesDoc = DocModel.CreateInstance();
            var x = (ObservableCollection<object>) assembliesDoc.RibbonItems;
            var ribbonModelItemMenuButton = new RibbonModelItemMenuButton() {Label = "Assemblies"};
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                ribbonModelItemMenuButton.ItemsCollection.Add(new RibbonModelMenuItem()
                    {Header = assembly.GetName().Name});

            x.Add(ribbonModelItemMenuButton);
            assembliesDoc.Title = "Assemblies";
            assembliesDoc.Content = new AssembliesControl {AssemblySource = AppDomain.CurrentDomain.GetAssemblies()};
            AddDocument(assembliesDoc);
        }

        private void AddModelDoc()
        {
            var t1 = new TablePanel();
            var dev = new TypeControl();
            dev.SetBinding(AttachedProperties.RenderedTypeProperty,
                new Binding("ActiveContent.Content") {Source = this, Converter = new GetTypeConverter()});
            var h1 = new TextBlock {Text = "Active Document"};
            t1.Children.Add(h1);
            t1.Children.Add(dev);
            var h2 = new TextBlock {Text = "Contextual Tab Group Headers"};
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

            //b.SetBinding(TextBlock.TextProperty, new Binding("ActiveContent.Content") {Source = this});

            var item = DocModel.CreateInstance();
            item.Title = "Model";
            item.Content = t1;
            AddDocument(item);
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


        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IClientModel _clientViewModel;

        private ObservableCollection<object> _documents;
        private ReplaySubject<CodeElementDocumentation> _r;
        private ReplaySubject<ActivationInfo> _aReplaySubject;
        private IEnumerable _documents1;
        private IDocumentHost _docHost;
        private IContentSelector _contentSelector;

        public virtual bool AllDocs { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public virtual Main1 View
        {
            get { return _view; }
            set
            {
                if (Equals(value, _view)) return;
                _view = value;

                AddInitialDocuments();


//                AddPowerShell2();
                AddInitialAnchorables();

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 
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

        /// </summary>
        [Browsable(false)]
        public virtual CurrentOperation CurrentOperation
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
        [Browsable(false)]
        public virtual IClientModel ClientViewModel
        {
            get { return _clientViewModel; }
            set
            {
                _clientViewModel = value;
                OnPropertyChanged();
                new UserControl1().propertyGrid1.SelectedObject = _clientViewModel.PrimaryRibbon;
            }
        }

        [Browsable(false)] public JsonSerializerOptions JsonSerializerOptions { get; set; }

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
                AddDocument(item);
            }
        }

        /// <param name="e"></param>
        public void OnExecutedPaste(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                var i = Clipboard.GetImage();
                var img = new Image {Source = i};
                var doc = DocModel.CreateInstance();
                doc.Content = img;
                AddDocument(doc);
                ActiveContent = doc;
            }

            // var  d =Clipboard.GetDataObject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task ProcessDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var docPath = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (docPath == null) return;
            foreach (var file in docPath)
            {
            }
        }

        public void AddDocument(object doc)
        {
            DocHost.AddDocument(doc);
            return;
            if (Dispatcher == null || Dispatcher.CheckAccess())
                DocumentsCollection.Add(doc);
            else
                throw new InvalidOperationException("wrong thread");
        }

        public void AddAnchorable(object anchorable)
        {
            Anchorables.Add(anchorable);
        }


        /// <inheritdoc />
        public void Subject(IMySubject x)
        {
            if (!AllDocs)
                return;
            var dm = DocModel.CreateInstance(x.Title);
            dm.GroupHeader = "Incoming";
            if (View != null) dm.LargeImageSource = View.TryFindResource("BlueArrowDrawingImage");
            // if(x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof(MyReplaySubject<>))
            // {
            BindingOperations.SetBinding(dm, DocModel.TitleProperty, new Binding("Title") {Source = x});
            // }

            dm.Content = new SubjectView2() {Observable = x.ObjectSubject, ItemType = x.Type};
            // ObservableCollection<object> c=new ObservableCollection<object>();
            // if (x.ListView)
            // {
            // dm.Content = AnalysisControlsModule.ReplayListView<object>(c, x.ObjectSubject,
            // ControlsResources("templates.baml"), x.Type);
            // }
            // else
            // {
            // dm.Content = AnalysisControlsModule.ReplayItemsControl<object>(c, x.ObjectSubject,
            // ControlsResources("templates.baml"), x.Type);

            // }

            // XamlServices.Save(dm.Content);

            AddDocument(dm);
        }

        public object InstanceObjectId { get; set; }

        /// <inheritdoc />
        public IEnumerable GetCommands()
        {
            return new[]
                {new LambdaAppCommand("Test", (command, o) => Task.FromResult(AppCommandResult.Success), null).Command};
        }
    }

    public interface ICommandProvider
    {
        IEnumerable GetCommands();
    }

    internal class CustomDriveInfo : PSDriveInfo
    {
        /// <inheritdoc />
        public CustomDriveInfo(PSDriveInfo driveInfo, PSObject data) : base(driveInfo)
        {
        }
    }

    public class DispatcherProxyInterceptor : IInterceptor
    {
        public ProxyGenerator ProxyGenerator { get; set; }
        public Dispatcher Dispatcher { get; }

        public DispatcherProxyInterceptor(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        /// <inheritdoc />
        public void Intercept(IInvocation invocation)
        {
            Dispatcher.Invoke(() => { invocation.Proceed(); });
            if (invocation.ReturnValue != null)
                if (invocation.ReturnValue.GetType().IsClass)
                    invocation.ReturnValue =
                        ProxyGeneratorHelper.ProxyGenerator.CreateClassProxyWithTarget(invocation.ReturnValue.GetType(),
                            invocation.ReturnValue, this);
        }
    }
}