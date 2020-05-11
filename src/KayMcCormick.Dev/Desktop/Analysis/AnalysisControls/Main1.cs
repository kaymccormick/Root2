using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalysisAppLib;
using AvalonDock;
using AvalonDock.Layout;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    public class Main1 : Control, IView<Main1Model>
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            DebugUtils.WriteLine($"Constraint is {constraint}");
            DebugUtils.WriteLine($"Size is {size}");
            return size;
        }

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
            {
                DebugUtils.WriteLine($"Property update {e.Property.Name} from {e.OldValue} to {e.NewValue}");
            }
        }

        public static readonly DependencyProperty AnchorablesProperty = DependencyProperty.Register(
            "Anchorables", typeof(IEnumerable), typeof(Main1),
            new PropertyMetadata(default(IEnumerable)));

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Anchorables
        {
            get { return (IEnumerable) GetValue(AnchorablesProperty); }
            set { SetValue(AnchorablesProperty, value); }
        }

        private ObservableCollection<object> _anchorables = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DocumentsProperty = DependencyProperty.Register(
            "Documents", typeof(IEnumerable), typeof(Main1), new PropertyMetadata(default(IEnumerable)));

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Documents
        {
            get { return (IEnumerable) GetValue(DocumentsProperty); }
            set { SetValue(DocumentsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        // public static readonly DependencyProperty RibbonBuilderProperty = DependencyProperty.Register(
            // "RibbonBuilder", typeof(RibbonBuilder), typeof(Main1), new PropertyMetadata(default(RibbonBuilder)));

        private Grid _grid;
        private Main1Model _viewModel;
        private LayoutDocumentPaneGroup _layoutDocumentPaneGroup;
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

            var b = new CommandBinding(WpfAppCommands.CreateWorkspace, OncreateWorkSpaceExecuted, CanExecute);
            CommandBindings.Add(b);
            var b2 = new CommandBinding(WpfAppCommands.CreateSolution, OnCreateSolutionExecuted);
            CommandBindings.Add(b2);
            var b3 = new CommandBinding(WpfAppCommands.CreateProject, OnCreateProjectExecuted);
            CommandBindings.Add(b3);
            CommandBindings.Add(new CommandBinding(WpfAppCommands.OpenSolutionItem, OnSolutionItemExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.LoadSolution, LoadSolutionExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.BrowseSymbols, OnBrowseSymbolsExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.ViewDetails, OnViewDetailsExecutedAsync));

            //Documents.Add(new DocInfo { Description = "test", Content = Properties.Resources.Program_Parse});
        }

        private async void OnViewDetailsExecutedAsync(object sender, ExecutedRoutedEventArgs e)
        {

            DataGrid dg = new DataGrid();
            var desc = e.Parameter?.ToString() ?? "";
            if (e.Parameter is PathModel pm)
            {
                if (pm.Item is DocumentModel dm)
                {
                    try
                    {
                        var root = await dm.Document.GetSyntaxRootAsync();
                        var c = root.DescendantNodesAndTokensAndSelf().ToList();
                        dg.ItemsSource = c;
                        desc = dm.Document.Name;
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    dg.ItemsSource = pm.Children;
                }
            } else if (e.Parameter is ProjectModel pm2)
            {
                var project = ViewModel.Workspace.CurrentSolution.GetProject(pm2.Id);
                if (project == null)
                {
                    return;
                }
                else
                    dg.ItemsSource = project.Documents;
            } else if (e.Parameter is Assembly a)
            {
                dg.ItemsSource = a.GetExportedTypes().ToList();
            }

            else
            {
                dg.ItemsSource = new[]{e.Parameter};
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
            CommandManager.AddPreviewCanExecuteHandler(this, PreviewCanExecute);
            CommandManager.AddPreviewExecutedHandler(this, PrepviewExecuted);
            _dockingManager = (DockingManager)GetTemplateChild("DockingManager");
            _dockingManager.ActiveContentChanged += DockingManagerOnActiveContentChanged;
            _grid = (Grid) GetTemplateChild("Grid");
            AllowDrop = true;
            DragOver += OnDragOver;
            _layoutDocumentPaneGroup =
                (LayoutDocumentPaneGroup) GetTemplateChild("LayoutDocumentPaneGroup");

            var listBox = new ListView();
            listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ViewModel.Messages") {Source = this});
            var listBoxView = new GridView();
            listBox.View = listBoxView;
            listBoxView.Columns.Add(new GridViewColumn() { DisplayMemberBinding = new Binding("Project")});
            
            
            if (ViewModel == null)
            {
                
                //.Add(anchorableModel);
            }
            else
            {
                var anchorableModel = new DocModel() { Content = ViewModel?.Messages, Title = "Messages" };
                ViewModel.Documents.Add(anchorableModel);
            }
        }

        private void PrepviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string cmd = e.Command.ToString();
            if (e.Command is RoutedUICommand rui)
            {
                cmd = rui.Text;
            }

            DebugUtils.WriteLine($"{cmd} {e.Handled} {e.Handled}");

        }

        private void PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            string cmd = e.Command.ToString();
            if (e.Command is RoutedUICommand rui)
            {
                cmd = rui.Text;
            }

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
            await ViewModel.LoadSolution((string) e.Parameter);
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

        private void OncreateWorkSpaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine(nameof(OnCreateProjectExecuted));
            ViewModel.CreateWorkspace();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.All;
            else
                e.Effects = DragDropEffects.None;
        }

        protected override async void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var docPath = (string[]) e.Data.GetData(DataFormats.FileDrop);
                var file = docPath[0];
                if (file.EndsWith(".cs"))
                {
                    if (ViewModel.SelectedProject != null)
                    {
                        ViewModel.AddDocument(ViewModel.SelectedProject, file);
                        return;
                    }

                    var context = AnalysisService.Load(file, "x", false);
                    DebugUtils.WriteLine(string.Join("\n", context.Compilation.GetDiagnostics()));
                    var doc = new DocModel()
                    {
                        // Content = "Beep",
                        Content = new FormattedTextControl()
                        {
                            SyntaxTree = context.SyntaxTree,
                            Compilation = context.Compilation
                        },
                        Title = System.IO.Path.GetFileNameWithoutExtension(file)
                    };
                    ViewModel.Documents.Add(doc);
                }
                else if (file.EndsWith(".sln"))
                {
                    await ViewModel.LoadSolution(file);
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
                    foreach (var anchorable in _anchorables)
                    {
                        _viewModel.Anchorables.Add(anchorable);
                    }

                    _viewModel.View = this;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LayoutDocumentPaneGroup LayoutDocumentPaneGroup
        {
            get { return _layoutDocumentPaneGroup; }
            set { _layoutDocumentPaneGroup = value; }
        }
    }
}