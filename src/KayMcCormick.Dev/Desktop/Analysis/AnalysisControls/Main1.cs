using AnalysisAppLib;
using Autofac.Features.Metadata;
using AvalonDock;
using AvalonDock.Layout;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using NLog.Fluent;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalysisControls.ViewModel;
using Autofac;
using Microsoft.Win32;
using SyntaxFactory = Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Main1 : AppControl, IView<Main1Model>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            // DebugUtils.WriteLine($"Constraint is {constraint}");
            // DebugUtils.WriteLine($"Size is {size}");
            return size;
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var outv = base.ArrangeOverride(arrangeBounds);
            // DebugUtils.WriteLine($"Arrange input {arrangeBounds} out {outv}");
            return outv;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            // if (e.Property.Name == "ActualWidth" || e.Property.Name == "ActualHeight")
                // DebugUtils.WriteLine($"Property update {e.Property.Name} from {e.OldValue} to {e.NewValue}");
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AnchorablesProperty = DependencyProperty.Register(
            "Anchorables", typeof(IEnumerable), typeof(Main1),
            new PropertyMetadata(default(IEnumerable)));

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public IEnumerable Anchorables
        {
            get { return (IEnumerable) GetValue(AnchorablesProperty); }
            set { SetValue(AnchorablesProperty, value); }
        }

        private readonly ObservableCollection<object> _anchorables = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DocumentsProperty = DependencyProperty.Register(
            "Documents", typeof(IEnumerable), typeof(Main1), new PropertyMetadata(default(IEnumerable)));

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public IEnumerable Documents
        {
            get { return (IEnumerable) GetValue(DocumentsProperty); }
            set { SetValue(DocumentsProperty, value); }
        }

        private Main1Model _viewModel;
        private DockingManager _dockingManager;
        private Main1Mode2 _viewModel2;
        private MyReplaySubject<string> r;

        /// <summary>
        /// 
        /// </summary>
        // public RibbonBuilder RibbonBuilder
        // {
        // get { return (RibbonBuilder) GetValue(RibbonBuilderProperty); }
        // set { SetValue(RibbonBuilderProperty, value); }
        // }
        static Main1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Main1), new FrameworkPropertyMetadata(typeof(Main1)));
            AttachedProperties.LifetimeScopeProperty.OverrideMetadata(typeof(Main1), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, null, CoerceLifetimeScope));

        }

        private static object CoerceLifetimeScope(DependencyObject d, object basevalue)
        {
            var vs = DependencyPropertyHelper.GetValueSource(d, AttachedProperties.LifetimeScopeProperty);
            var lifetimeScope = (ILifetimeScope)basevalue;
            if (lifetimeScope.Tag == "Main1")
            {
                return lifetimeScope;
            }
            var main1 = (Main1) d;
            var coerceLifetimeScope = lifetimeScope.BeginLifetimeScope($"Main1", main1.ConfigAction);
            if (main1.r == null)
            {
                main1.r = coerceLifetimeScope.Resolve<MyReplaySubject<string>>();
            }

            return coerceLifetimeScope;
        }

        private void ConfigAction(ContainerBuilder obj)
        {
            obj.RegisterInstance(this).AsSelf();
        }


        /// <summary>
        /// 
        /// </summary>
        public Main1()
        {
            
            Anchorables = _anchorables;
            SetBinding(DocumentsProperty, new Binding("ViewModel.Documents") {Source = this});
            SetBinding(AnchorablesProperty, new Binding("ViewModel.Anchorables") {Source = this});

            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateWorkspace, OnCreateWorkSpaceExecuted,
                CanExecute));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateSolution, OnCreateSolutionExecuted));

            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateDocument, OnCreateDocumentExecuted));            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateDocument, OnCreateDocumentExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateClass, OnCreateClass));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateProject, OnCreateProjectExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.OpenSolutionItem, OnSolutionItemExecutedAsync));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.LoadSolution, LoadSolutionExecutedAsync));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.BrowseSymbols, OnBrowseSymbolsExecutedAsync));
            
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ViewResources, OnViewResourcesExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, (sender, e) => OnOpenExecuted(ViewModel, ViewModel2, sender, e) , OnOpenCanExecute));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ConvertToJson, OnConvertToJsonExecuted));

            //Documents.Add(new DocInfo { Description = "test", Content = Properties.Resources.Program_Parse});
        }

        private void OnCreateClass(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel2.CreateClass();
        }

        private void OnCreateDocumentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel2.CreateDocument();
        }

        private void OnOpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter == null)
            {
                e.CanExecute = true;
            }
        }

        private void OnConvertToJsonExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter == null)
                return;
            if (e.Parameter is IDataObject o)
            {
                var t1 = o
                    .GetFormats().Select(f => Tuple.Create(f, (object) o.GetData(f)))
                    .FirstOrDefault(t => !(t.Item2 is string));
                if (t1 != null)
                {
                    try
                    {
                        var json = JsonSerializer.Serialize(t1.Item2,ViewModel.JsonSerializerOptions);
                        var d = DocModel.CreateInstance();
                        d.Title = "json";
                        var dp = new DockPanel();
                        dp.Children.Add(new TextBox {Text = json});
                        d.Content = dp;
                        ViewModel.Documents.Add(d);
                    } catch(Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }
                }

            }
        }

        private static async void OnOpenExecuted(Main1Model main1Model, Main1Mode2 main1Mode2, object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter)
            {
                case null:
                {
                    var  x = new OpenFileDialog();
                    if (!x.ShowDialog().GetValueOrDefault()) return;
                    var file = x.FileName;
                    if (file.ToLowerInvariant().EndsWith(".cs") || file.EndsWith(".vb"))
                    {
                        // if (ViewModel2.SelectedProject != null)
                        // {
                        // ViewModel2.AddDocument(ViewModel2.SelectedProject, file);
                        // return;
                        // }

                        Compilation compilation = null;
                        SyntaxTree tree;
                        if (file.ToLowerInvariant().EndsWith(".vb"))
                        {
                            var s = new StreamReader(file);
                            var code = await s.ReadToEndAsync();
                            tree = SyntaxFactory.ParseSyntaxTree(code,
                                new VisualBasicParseOptions(),
                                file);

                            compilation = VisualBasicCompilation.Create("x", new[] { tree });
                        }
                        else
                        {
                            var context = await AnalysisService.LoadAsync(file, "x", false).ConfigureAwait(true);
                            var cSharpCompilation = context.Compilation;
                            compilation = cSharpCompilation;
                            DebugUtils.WriteLine(string.Join("\n", cSharpCompilation.GetDiagnostics()));

                            tree = context.SyntaxTree;
                        }

                        await  Main1Mode2.CodeDocAsync(main1Mode2, tree, compilation, file);
                    }
                    else if (file.ToLowerInvariant().EndsWith(".sln"))
                    {
                        await main1Mode2.LoadSolutionAsync(file);
                    }
                    else if (file.ToLowerInvariant().EndsWith(".csproj") || file.EndsWith(".vbproj"))
                    {
                        await main1Mode2.LoadProjectAsync(file);
                    }

                    return;
                }
                case Meta<Lazy<IAppCustomControl>> controlItem:
                {
                    var control = controlItem.Value.Value;
                    var doc = DocModel.CreateInstance();
                    doc.Content = control;
                    main1Model.Documents.Add(doc);
                    main1Model.ActiveContent = doc;
                    break;
                }
            }
        }

        private void OnViewResourcesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var a = (Assembly) e.Parameter;
            var doc = DocModel.CreateInstance();
            doc.Title = "Resources";
            doc.Content = new AssemblyResourceTree() {Assembly = a};
            ViewModel.Documents.Add(doc);
            ViewModel.ActiveContent = doc;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var sourceName = (e.Source is IAppControl iap ? iap.ControlId.ToString( ): e.Source.ToString());
            //new LogBuilder(Logger).Message(nameof(OnPreviewMouseDown) + " " + e.ClickCount + sourceName).Write();
            base.OnPreviewMouseDown(e);
        }

   
        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DockingManagerLayoutRoot = (LayoutRoot) GetTemplateChild("LayoutRoot");
            // CommandManager.AddPreviewCanExecuteHandler(this, PreviewCanExecute);
            CommandManager.AddPreviewExecutedHandler(this, PreviewExecuted);
            _dockingManager = (DockingManager) GetTemplateChild("DockingManager");
            if (_dockingManager != null) _dockingManager.ActiveContentChanged += DockingManagerOnActiveContentChanged;
            
            AllowDrop = true;
            DragOver += OnDragOver;

            var listBox = new ListView();
            listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ViewModel.Messages") {Source = this});
            var listBoxView = new GridView();
            listBox.View = listBoxView;
            listBoxView.Columns.Add(new GridViewColumn() {DisplayMemberBinding = new Binding("Project")});


            if (ViewModel == null)
            {
                //.Add(anchorableModel);
            }
            else
            {
                
            }
        }

        /// <summary>
        /// Provides access to DockingManager's <see cref="LayoutRoot"/> model object.
        /// </summary>
        public LayoutRoot DockingManagerLayoutRoot { get; set; }

        private void PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var cmd = e.Command.ToString();
            if (e.Command is RoutedUICommand rui) cmd = rui.Text;

            DebugUtils.WriteLine($"{cmd} {e.Handled} {e.Handled}");
        }

        private void PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var cmd = e.Command.ToString();
            if (e.Command is RoutedUICommand rui) cmd = rui.Text;

            DebugUtils.WriteLine($"{cmd} {e.CanExecute} {e.Handled}");
        }

        private async void OnBrowseSymbolsExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                await ViewModel.BrowseSymbols(e.Parameter);
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }
        }

        private async void LoadSolutionExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            await ViewModel2.LoadSolutionAsync((string) e.Parameter);
        }

        private void DockingManagerOnActiveContentChanged(object sender, EventArgs e)
        {
            if (_dockingManager.ActiveContent != null) DebugUtils.WriteLine(_dockingManager.ActiveContent.ToString());
        }

        private async void OnSolutionItemExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            await ViewModel2.OpenSolutionItem(e.Parameter);
        }

        private void OnCreateProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel2.CreateProject();
        }

        public Main1Mode2 ViewModel2
        {
            get { return _viewModel2; }
            set
            {
                _viewModel2 = value;
                _viewModel2.Dispatcher = Dispatcher;
            }
        }

        private void OnCreateSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel2.CreateSolution();
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel2?.Workspace == null)
            {
                e.CanExecute = true;
                e.ContinueRouting = false;
            }
        }

        private void OnCreateWorkSpaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine(nameof(OnCreateProjectExecuted));
            ViewModel2.CreateWorkspace();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (!e.Handled)
            {
                e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            await ViewModel.ProcessDrop(e);
        }

        /// <inheritdoc />
        public Main1Model ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = _viewModel;
                if (_viewModel != null)
                {
                    _viewModel.Dispatcher = Dispatcher;
                    foreach (var anchorable in _anchorables) _viewModel.Anchorables.Add(anchorable);

                    _viewModel.View = this;
                }
            }
        }
    }
}