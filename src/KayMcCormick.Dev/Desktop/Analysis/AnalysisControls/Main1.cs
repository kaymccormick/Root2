using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalysisAppLib;
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
            "Anchorables", typeof(ObservableCollection<object>), typeof(Main1),
            new PropertyMetadata(default(ObservableCollection<object>)));

        public ObservableCollection<object> Anchorables
        {
            get { return (ObservableCollection<object>) GetValue(AnchorablesProperty); }
            set { SetValue(AnchorablesProperty, value); }
        }

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
            SetBinding(DocumentsProperty, new Binding("ViewModel.Documents") {Source = this});
            SetBinding(AnchorablesProperty, new Binding("ViewModel.Anchorables") {Source = this});
            //Documents.Add(new DocInfo { Description = "test", Content = Properties.Resources.Program_Parse});
        }

        public override void OnApplyTemplate()
        {
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

            _layoutDocumentPaneGroup =
                (LayoutDocumentPaneGroup) GetTemplateChild("LayoutDocumentPaneGroup");
        }

        private void OnSolutionItemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.OpenSolutionItem();
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

        public Main1Model ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = _viewModel;
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