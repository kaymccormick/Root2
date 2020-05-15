﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalysisAppLib;
using AnalysisControls.Properties;
using Autofac.Features.Metadata;
using AvalonDock;
using AvalonDock.Layout;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using NLog.Fluent;
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
            DebugUtils.WriteLine($"Constraint is {constraint}");
            DebugUtils.WriteLine($"Size is {size}");
            return size;
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var outv = base.ArrangeOverride(arrangeBounds);
            DebugUtils.WriteLine($"Arrange input {arrangeBounds} out {outv}");
            return outv;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "ActualWidth" || e.Property.Name == "ActualHeight")
                DebugUtils.WriteLine($"Property update {e.Property.Name} from {e.OldValue} to {e.NewValue}");
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
            CommandBindings.Add(new CommandBinding(WpfAppCommands.CreateProject, OnCreateProjectExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.OpenSolutionItem, OnSolutionItemExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.LoadSolution, LoadSolutionExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.BrowseSymbols, OnBrowseSymbolsExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ViewDetails, OnViewDetailsExecutedAsync));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ViewResources, OnViewResourcesExecuted));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OnOpenExecuted));

            //Documents.Add(new DocInfo { Description = "test", Content = Properties.Resources.Program_Parse});
        }

        private void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is Meta<Lazy<IAppCustomControl>> controlItem)
            {
                var control = controlItem.Value.Value;
                var doc = new DocModel() {Content = control};
                ViewModel.Documents.Add(doc);
                ViewModel.ActiveContent = doc;
            }
        }

        private void OnViewResourcesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var a = (Assembly) e.Parameter;
            var doc = new DocModel() {Title = "Resources", Content = new AssemblyResourceTree() {Assembly = a}};
            doc.ContextualTabGroupHeaders.Add("Resources");
            ViewModel.Documents.Add(doc);
            ViewModel.ActiveContent = doc;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var sourceName = (e.Source is IAppControl iap ? iap.ControlId.ToString( ): e.Source.ToString());
            new LogBuilder(Logger).Message(nameof(OnPreviewMouseDown) + " " + e.ClickCount + sourceName).Write();
            base.OnPreviewMouseDown(e);
        }

        private async void OnViewDetailsExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {
            var dg = new DataGrid();
            var desc = e.Parameter?.ToString() ?? "";
            if (e.Parameter is PathModel pm)
            {
                if (pm.Item is DocumentModel dm)
                    try
                    {
                        var root = await dm.Document.GetSyntaxRootAsync();
                        if (root != null)
                        {
                            var c = root.DescendantNodesAndTokensAndSelf().ToList();
                            dg.ItemsSource = c;
                        }

                        desc = dm.Document.Name;
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }
                else
                    dg.ItemsSource = pm.Children;
            }
            else if (e.Parameter is ProjectModel pm2)
            {
                var project = ViewModel.Workspace.CurrentSolution.GetProject(pm2.Id);
                if (project == null)
                    return;
                else
                    dg.ItemsSource = project.Documents;
            }
            else if (e.Parameter is Assembly a)
            {
                dg.ItemsSource = a.GetExportedTypes().ToList();
            }

            else
            {
                dg.ItemsSource = new[] {e.Parameter};
            }

            var detailsTitle = "Details for " + desc;
            var docModel = new DocModel {Title = detailsTitle, Content = dg};
            ViewModel.Documents.Add(docModel);
            ViewModel.ActiveContent = docModel;
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
                var anchorableModel = new DocModel() {Content = ViewModel?.Messages, Title = "Messages"};
                ViewModel.Documents.Add(anchorableModel);
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

        private async void OnBrowseSymbolsExecuted(object sender, ExecutedRoutedEventArgs e)
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

        private async void LoadSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await ViewModel.LoadSolutionAsync((string) e.Parameter);
        }

        private void DockingManagerOnActiveContentChanged(object sender, EventArgs e)
        {
            if (_dockingManager.ActiveContent != null) DebugUtils.WriteLine(_dockingManager.ActiveContent.ToString());
        }

        private async void OnSolutionItemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await ViewModel.OpenSolutionItem(e.Parameter);
        }

        private void OnCreateProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.CreateProject();
        }

        private void OnCreateSolutionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.CreateSolution();
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel?.Workspace == null)
            {
                e.CanExecute = true;
                e.ContinueRouting = false;
            }
        }

        private void OnCreateWorkSpaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine(nameof(OnCreateProjectExecuted));
            ViewModel.CreateWorkspace();
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

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var docPath = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (docPath == null) return;
            foreach (var file in docPath)
            {
                if (file.ToLowerInvariant().EndsWith(".cs") || file.EndsWith(".vb"))
                {
                    if (ViewModel.SelectedProject != null)
                    {
                        ViewModel.AddDocument(ViewModel.SelectedProject, file);
                        continue;
                    }

                    Compilation compilation = null;
                    SyntaxTree tree;
                    if (file.ToLowerInvariant().EndsWith(".vb"))
                    {
                        var s = new StreamReader(file);
                        string code = await s.ReadToEndAsync();
                        tree = SyntaxFactory.ParseSyntaxTree(code,
                            new VisualBasicParseOptions(),
                            file);

                        compilation = VisualBasicCompilation.Create("x", new[] {tree});
                    }
                    else
                    {
                        var context = await AnalysisService.LoadAsync(file, "x", false);
                        var cSharpCompilation = context.Compilation;
                        compilation = cSharpCompilation;
                        DebugUtils.WriteLine(string.Join("\n", cSharpCompilation.GetDiagnostics()));

                        tree = context.SyntaxTree;
                    }

                    var doc = Main1Model.CodeDoc(tree, compilation, file);
                    ViewModel.Documents.Add(doc);
                    ViewModel.ActiveContent = doc;
                }
                else if (file.ToLowerInvariant().EndsWith(".sln"))
                {
                    await ViewModel.LoadSolutionAsync(file);
                }
                else if (file.ToLowerInvariant().EndsWith(".csproj") || file.EndsWith(".vbproj"))
                {
                    await ViewModel.LoadProjectAsync(file);
                }

            }
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
                    foreach (var anchorable in _anchorables) _viewModel.Anchorables.Add(anchorable);

                    _viewModel.View = this;
                }
            }
        }
    }
}