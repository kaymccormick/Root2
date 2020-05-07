using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkspaceView : Control
    {
        public static readonly DependencyProperty SelectedDocumentProperty = DependencyProperty.Register(
            "SelectedDocument", typeof(DocumentModel), typeof(WorkspaceView),
            new PropertyMetadata(default(DocumentModel), new PropertyChangedCallback(OnSelectedDocumentChanged)));

        private static void OnSelectedDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorkspaceView v = (WorkspaceView) d;
            RoutedPropertyChangedEventArgs<DocumentModel> e2 = new RoutedPropertyChangedEventArgs<DocumentModel>(
                (DocumentModel)e.OldValue, (DocumentModel)e.NewValue, WorkspaceView.SelectedDocumentChangedEvent
                );
            v.OnSelectedDocumentChanged(e2);
        

        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent SelectedDocumentChangedEvent= EventManager.RegisterRoutedEvent(
            "SelectedDocumentChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DocumentModel>), typeof(WorkspaceView));
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent SelectedProjectChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedProjectChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<ProjectModel>), typeof(WorkspaceView));

        /// <summary>
        /// 
        /// </summary>
        public DocumentModel SelectedDocument
        {
            get { return (DocumentModel) GetValue(SelectedDocumentProperty); }
            set { SetValue(SelectedDocumentProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<DocumentModel> SelectedDocumentChanged
        {
            add { AddHandler(SelectedDocumentChangedEvent, value); }
            remove { RemoveHandler(SelectedDocumentChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<ProjectModel> SelectedProjectChanged
        {
            add { AddHandler(SelectedProjectChangedEvent, value); }
            remove { RemoveHandler(SelectedProjectChangedEvent, value); }
        }

        protected virtual void OnSelectedDocumentChanged(RoutedPropertyChangedEventArgs<DocumentModel> args)
        {
            RaiseEvent(args);
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SolutionsProperty = DependencyProperty.Register(
            "Solutions", typeof(IEnumerable), typeof(WorkspaceView), new PropertyMetadata(default(IEnumerable)));

        private TreeView _treeView;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Solutions
        {
            get { return (IEnumerable) GetValue(SolutionsProperty); }
            set { SetValue(SolutionsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            _treeView = (TreeView) GetTemplateChild("TreeView");
            _treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
        }

        public static readonly DependencyProperty SelectedSolutionProperty = DependencyProperty.Register(
            "SelectedSolution", typeof(SolutionModel), typeof(WorkspaceView),
            new PropertyMetadata(default(SolutionModel)));

        /// <summary>
        /// 
        /// </summary>
        public SolutionModel SelectedSolution
        {
            get { return (SolutionModel) GetValue(SelectedSolutionProperty); }
            set { SetValue(SelectedSolutionProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedProjectProperty = DependencyProperty.Register(
            "SelectedProject", typeof(ProjectModel), typeof(WorkspaceView),
            new PropertyMetadata(default(ProjectModel), new PropertyChangedCallback(OnSelectedProjectChanged)));

        private static void OnSelectedProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorkspaceView v = (WorkspaceView)d;
            RoutedPropertyChangedEventArgs<ProjectModel> e2 = new RoutedPropertyChangedEventArgs<ProjectModel>(
                (ProjectModel)e.OldValue, (ProjectModel)e.NewValue, WorkspaceView.SelectedProjectChangedEvent
            );
            v.OnSelectedProjectChanged(e2);

        }

        protected virtual void OnSelectedProjectChanged(RoutedPropertyChangedEventArgs<ProjectModel> e2)
        {
            RaiseEvent(e2);
        }

        public ProjectModel SelectedProject
        {
            get { return (ProjectModel) GetValue(SelectedProjectProperty); }
            set { SetValue(SelectedProjectProperty, value); }
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is ProjectModel p)
            {
                SelectedProject = p;
                SelectedDocument = null;
                SelectedSolution = p.Solution;
            }
            else if (e.NewValue is SolutionModel s)
            {
                SelectedDocument = null;
                SelectedProject = null;
                SelectedSolution = s;
            }
            else if (e.NewValue is DocumentModel d)
            {
                SelectedDocument = d;
                SelectedProject = d.Project;
                SelectedSolution = SelectedProject.Solution;
            } else if (e.NewValue is PathModel pp)
            {
                if(pp.Kind == PathModelKind.File)
                {
                    var dd = (DocumentModel) pp.Item;
                    SelectedDocument = dd;
                    SelectedProject = dd.Project;
                    SelectedSolution = dd.Project.Solution;
                }
            }
        }

        static WorkspaceView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkspaceView),
                new FrameworkPropertyMetadata(typeof(WorkspaceView)));
        }
    }
}