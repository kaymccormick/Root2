using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
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
using AnalysisAppLib;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using KmDevWpfControls;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using NLog;
using RibbonLib.Model;
using WpfApp4;
using TypeControl = KayMcCormick.Lib.Wpf.TypeControl;
using XamlReader = System.Windows.Markup.XamlReader;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Main1Model : INotifyPropertyChanged, IDocumentHost, IAnchorableHost, ISubjectWatcher
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
                DebugUtils.WriteLine("new active document is " + _activeContent);
                OnPropertyChanged(nameof(ActiveContent));
                if (ClientViewModel != null)
                    if (ClientViewModel.PrimaryRibbon != null)
                        ClientViewModel.PrimaryRibbon.ActiveContent = _activeContent;

                if (value is DocModel d)
                {
                    foreach (var primaryRibbonRibbonItem in ClientViewModel.PrimaryRibbon.RibbonItems)
                    foreach (var item in primaryRibbonRibbonItem.Items)
                        if (item is RibbonModelGroup g)
                            if (g.Header != null && g.Header.Equals("Context"))
                            {
                                g.Items.Clear();

                                foreach (var dRibbonItem in d.RibbonItems) g.Items.Add((RibbonModelItem) dRibbonItem);
                            }

                    if (old is DocModel dd)
                    {
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
        private readonly IActivationStream _ss;
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
        public ObservableCollection<object> Documents
        {
            get { return _documents; }
            set
            {
                if (Equals(value, _documents)) return;
                _documents = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public ObservableCollection<object> Anchorables { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replay"></param>
        public Main1Model(ReplaySubject<Workspace> replay, IActivationStream ss,
             JsonSerializerOptions jsonSerializerOptions = null) : this()
        {
            JsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();
            _replay = replay;
            _ss = ss;
            //_subs = subs;
            Subject(ss);
        }

        /// <summary>
        /// 
        /// </summary>
        public Main1Model()
        {
            Documents.CollectionChanged += (sender, args) =>
            {
                foreach (var argsNewItem in args.NewItems) Debug.WriteLine(argsNewItem);
            };
        }

        private void AddInitialDocuments()
        {
            //            AddModelDoc();
            AddRibbonModelViewDoc();
            AddRibbonModelViewDoc1();
            AddAssembliesDoc();
            //            AddPropertiesGridDoc();
            AddVisualTreeViewDoc();
            AddVisualTreeViewDoc1();
            AddTypeProvider();
            AddControlsDocq();
            AddPowerShell();
            AddPowerShell2();
        }

        private void AddPowerShell2()
        {
            TerminalUserControl0 terminal0  = new TerminalUserControl0();
            terminal0.Shell.Host.SetPrivateData(new PSObject(this));
            
            
            var doc = DocModel.CreateInstance();
            doc.Title = "PowerShell 0";
            doc.Content = terminal0;
            Documents.Add(doc);
        }

        private void AddVisualTreeViewDoc()
        {
            var c = new VisualTreeView();
            var doc = DocModel.CreateInstance();
            doc.Title = "Visual Tree View";
            doc.Content = c;
            Documents.Add(doc);
        }

        private void AddVisualTreeViewDoc1()
        {
            var c = new ContainerView();
            var doc = DocModel.CreateInstance();
            doc.Title = "Visual Tree View";
            doc.Content = c;
            Documents.Add(doc);
        }

        private void AddTypeProvider()
        {
            var c = new TypeProviderUserControl();
            var doc = DocModel.CreateInstance();
            doc.Title = "Type Provider";
            doc.Content = c;
            Documents.Add(doc);
        }

        private void AddRibbonModelViewDoc()
        {
            var c = new RibbonModelView();
            c.SetBinding(RibbonModelView.RibbonModelProperty,
                new Binding("ClientViewModel.PrimaryRibbon") {Source = this});
            var doc = DocModel.CreateInstance();
            doc.Title = "MyRibbon Model View";
            doc.Content = c;
            Documents.Add(doc);
        }

        private void AddRibbonModelViewDoc1()
        {
            var c = new DropControl();
            //c.SetBinding(RibbonModelView.RibbonModelProperty, new Binding("ClientViewModel.PrimaryRibbon") { Source = this });
            var doc = DocModel.CreateInstance();
            doc.Title = "MyRibbon Model View";
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

        private void AddControlsDocq()
        {
            var item = DocModel.CreateInstance();
            item.Title = "Trace Configuration";
            item.Content = new TraceView {ListenerTypes = new[] {typeof(XmlWriterTraceListener)}};

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
            Documents.Add(item);
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

            Documents.Add(assembliesDoc);
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
            Documents.Add(assembliesDoc);
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


        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IClientModel _clientViewModel;

        private ObservableCollection<object> _documents = new ObservableCollection<object>();

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
                AddInitialDocuments();

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

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public Dispatcher Dispatcher { get; set; }

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
            Documents.Add(doc);
        }

        /// <inheritdoc />
        public void SetActiveDocument(object doc)
        {
            ActiveContent = doc;
        }

        public void AddAnchorable(object anchorable)
        {
            Anchorables.Add(anchorable);
        }
        public static ResourceDictionary ControlsResources(string filename)
        {
            var assembly = typeof(AnalysisControlsModule).Assembly;
            var x = new ResourceManager(
                "AnalysisControlsCore.g"
                , assembly
            );

            var y = x.GetStream(filename);
            if (y == null)
            {
                throw new AppInvalidOperationException("Unable to get resource stream for " + filename);
            }
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader(y, new XamlReaderSettings());

            var oo = (ResourceDictionary)XamlReader.Load(b);
            return oo;
        }


        /// <inheritdoc />
        public void Subject(IMySubject x)
        {
            DocModel dm = DocModel.CreateInstance();
            ObservableCollection<object> c=new ObservableCollection<object>();
            dm.Content = AnalysisControlsModule.ReplayListView<object>(c, x.ObjectSubject, ControlsResources("templates.baml"), x.Type1);
            Documents.Add(dm);
        }
    }
    [CmdletProvider("Test", ProviderCapabilities.None)]
    internal class RibbonModelProvider : NavigationCmdletProvider
    {
        /// <inheritdoc />
        protected override string GetChildName(string path)
        {
            return base.GetChildName(path);
        }

        /// <inheritdoc />
        protected override void MoveItem(string path, string destination)
        {
            base.MoveItem(path, destination);
        }

        /// <inheritdoc />
        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return base.MoveItemDynamicParameters(path, destination);
        }

        /// <inheritdoc />
        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return base.NormalizeRelativePath(path, basePath);
        }

        /// <inheritdoc />
        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        /// <inheritdoc />
        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;

            return base.CopyItemDynamicParameters(path, destination, recurse);
        }

        /// <inheritdoc />
        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;
            foreach (var pdDocument in pd.Documents)
            {
                WriteItemObject(pd, path + "/" + pdDocument.ToString(), false);
            }
            return base.GetChildItemsDynamicParameters(path, recurse);
        }

        /// <inheritdoc />
        protected override object GetChildNamesDynamicParameters(string path)
        {
            return base.GetChildNamesDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override bool HasChildItems(string path)
        {
            return base.HasChildItems(path);
        }

        /// <inheritdoc />
        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return base.NewItemDynamicParameters(path, itemTypeName, newItemValue);
        }

        /// <inheritdoc />
        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return base.RemoveItemDynamicParameters(path, recurse);
        }

        /// <inheritdoc />
        protected override void RenameItem(string path, string newName)
        {
            base.RenameItem(path, newName);
        }

        /// <inheritdoc />
        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return base.RenameItemDynamicParameters(path, newName);
        }

        /// <inheritdoc />
        protected override void ClearItem(string path)
        {
            base.ClearItem(path);
        }

        /// <inheritdoc />
        protected override object ClearItemDynamicParameters(string path)
        {
            return base.ClearItemDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override string[] ExpandPath(string path)
        {
            return base.ExpandPath(path);
        }

        /// <inheritdoc />
        protected override object GetItemDynamicParameters(string path)
        {
            return base.GetItemDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override void InvokeDefaultAction(string path)
        {
            base.InvokeDefaultAction(path);
        }

        /// <inheritdoc />
        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return base.InvokeDefaultActionDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override bool ItemExists(string path)
        {
            return true;
            return base.ItemExists(path);
        }

        /// <inheritdoc />
        protected override object ItemExistsDynamicParameters(string path)
        {
            return base.ItemExistsDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override void SetItem(string path, object value)
        {
            base.SetItem(path, value);
        }

        /// <inheritdoc />
        protected override object SetItemDynamicParameters(string path, object value)
        {
            return base.SetItemDynamicParameters(path, value);
        }

        /// <inheritdoc />
        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            return base.RemoveDrive(drive);
        }

        /// <inheritdoc />
        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        /// <inheritdoc />
        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            return base.Start(providerInfo);
        }

        /// <inheritdoc />
        protected override object StartDynamicParameters()
        {
            return base.StartDynamicParameters();
        }

        /// <inheritdoc />
        protected override void Stop()
        {
            base.Stop();
        }

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        /// <inheritdoc />
        public override char AltItemSeparator { get; }

        /// <inheritdoc />
        public override char ItemSeparator { get; }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc />
        protected override string GetParentPath(string path, string root)
        {
            return base.GetParentPath(path, root);
        }

        /// <inheritdoc />
        protected override bool IsItemContainer(string path)
        {
            return true;
            return base.IsItemContainer(path);
        }

        /// <inheritdoc />
        protected override string MakePath(string parent, string child)
        {
            return base.MakePath(parent, child);
        }

        /// <inheritdoc />
        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            base.CopyItem(path, copyPath, recurse);
        }

        /// <inheritdoc />
        protected override void GetChildItems(string path, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;
            foreach (var pdDocument in pd.Documents)
            {
                if (pdDocument is DocModel dm)
                {
                    var p = MakePath(path, dm.Title);
                    WriteItemObject(new PSObject(dm),p, false);
                }
            }

            

        }

        /// <inheritdoc />
        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            base.GetChildItems(path, recurse, depth);
        }

        /// <inheritdoc />
        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            base.GetChildNames(path, returnContainers);
        }

        /// <inheritdoc />
        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            base.NewItem(path, itemTypeName, newItemValue);
        }

        /// <inheritdoc />
        protected override void RemoveItem(string path, bool recurse)
        {
            base.RemoveItem(path, recurse);
        }

        /// <inheritdoc />
        protected override void GetItem(string path)
        {

            base.GetItem(path);
        }

        /// <inheritdoc />
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            CustomDriveInfo info = new CustomDriveInfo(drive,Host.PrivateData);
            
            return base.NewDrive(drive);
        }

        /// <inheritdoc />
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var initializeDefaultDrives = new Collection<PSDriveInfo>();
            initializeDefaultDrives.Add(new CustomDriveInfo(
                new PSDriveInfo("Test1", ProviderInfo, @"/", "test1", PSCredential.Empty), Host.PrivateData));
            return initializeDefaultDrives;
        }

        /// <inheritdoc />
        protected override object NewDriveDynamicParameters()
        {
            return base.NewDriveDynamicParameter();
        }

        /// <inheritdoc />
        protected override bool IsValidPath(string path)
        {
            return true;
        }
    }

    internal class CustomDriveInfo : PSDriveInfo
    {
        /// <inheritdoc />
        public CustomDriveInfo(PSDriveInfo driveInfo, PSObject data) : base(driveInfo)
        {
        }

    }


    public interface IAnchorableHost
    {
        void AddAnchorable(object anchorable);
    }

    public interface IDocumentHost
    {
        void AddDocument(object doc);
        void SetActiveDocument(object doc);
    }
}