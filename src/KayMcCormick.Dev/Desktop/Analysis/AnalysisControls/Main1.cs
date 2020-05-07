using System;
using System.Collections;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Main1/>
    ///
    /// </summary>
    public class Main1 : Control, IView<Main1Model>
    {
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
        public static readonly DependencyProperty RibbonBuilderProperty = DependencyProperty.Register(
            "RibbonBuilder", typeof(RibbonBuilder), typeof(Main1), new PropertyMetadata(default(RibbonBuilder)));

        private Grid _grid;
        private Main1Model _viewModel;
        private LayoutDocumentPaneGroup _layoutDocumentPaneGroup;
        private DockingManager _dockingManager;

        /// <summary>
        /// 
        /// </summary>
        public RibbonBuilder RibbonBuilder
        {
            get { return (RibbonBuilder) GetValue(RibbonBuilderProperty); }
            set { SetValue(RibbonBuilderProperty, value); }
        }

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
            //Documents.Add(new DocInfo { Description = "test", Content = Properties.Resources.Program_Parse});
        }

        public override void OnApplyTemplate()
        {
            _dockingManager = (DockingManager)GetTemplateChild("DockingManager");
            _dockingManager.ActiveContentChanged += DockingManagerOnActiveContentChanged;
            _grid = (Grid) GetTemplateChild("Grid");
            AllowDrop = true;
            DragOver += OnDragOver;
            var b = new CommandBinding(WpfAppCommands.CreateWorkspace, OncreateWorkSpaceExecuted, CanExecute);
            CommandBindings.Add(b);
            var b2 = new CommandBinding(WpfAppCommands.CreateSolution, OnCreateSolutionExecuted);
            CommandBindings.Add(b2);
            var b3 = new CommandBinding(WpfAppCommands.CreateProject, OnCreateProjectExecuted);
            CommandBindings.Add(b3);
            CommandBindings.Add(new CommandBinding(WpfAppCommands.OpenSolutionItem, OnSolutionItemExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.LoadSolution, LoadSolutionExecuted));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.BrowseSymbols, OnBrowseSymbolsExecuted));
            _layoutDocumentPaneGroup =
                (LayoutDocumentPaneGroup) GetTemplateChild("LayoutDocumentPaneGroup");

            var listBox = new ListView();
            listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ViewModel.Messages") {Source = this});
            var listBoxView = new GridView();
            listBox.View = listBoxView;
            listBoxView.Columns.Add(new GridViewColumn() { DisplayMemberBinding = new Binding("Project")});
            
            var anchorableModel = new DocModel() { Content = ViewModel.Messages, Title="Messages" };
            if (ViewModel == null)
            {
                
                //.Add(anchorableModel);
            }
            else
            {
                ViewModel.Documents.Add(anchorableModel);
            }
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

        private void OnSolutionItemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.OpenSolutionItem(e.Parameter);
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
            if (ViewModel.Workspace == null) e.CanExecute = true;
        }

        private void OncreateWorkSpaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
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
                foreach (var anchorable in _anchorables)
                {
                    _viewModel.Anchorables.Add(anchorable);
                }
                _viewModel.View = this;
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