using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JetBrains.Annotations;
using KayMcCormick.Dev.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynCodeControls;

namespace AnalysisControls
{
    [TitleMetadata("Code Diagnostics Control")]
    public class CodeDiagnostics : SyntaxNodeControl, INotifyPropertyChanged, IControlWithViews
    {
        private ListView _regions;
        private ListView _lines;
        private DrawingGroup _dgroup;
        private Rectangle _rect;
        private EnhancedCodeControl _codeControl;
        private ObservableCollection<ViewSpec> _views = new ObservableCollection<ViewSpec>();
        private ViewSpec _currentView;
        private ViewSpec _codeView;
        private ViewSpec _documentView;
        private ViewSpec _modelView;
        private ViewSpec _diagView;
        private ViewSpec _sourceView;
        private Grid _viewContainer;
        private ViewSpec _structureView;
        private List<StructureNode> _structureRootNodes;

        static CodeDiagnostics()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeDiagnostics), new FrameworkPropertyMetadata(typeof(CodeDiagnostics)));
            // ModelProperty.OverrideMetadata(typeof(EnhancedCodeControl), new PropertyMetadata(SemanticModelChanged));
            // SyntaxTreeProperty.OverrideMetadata(typeof(EnhancedCodeControl), new FrameworkPropertyMetadata(SyntaxTreeChanged));
            SyntaxNodeProperty.OverrideMetadata(typeof(CodeDiagnostics), new PropertyMetadata(SyntaxNodeChanged));

        }

        public CodeDiagnostics()
        {
            _codeView = new ViewSpec() { ViewName = "Code", LargeImageSource = "pack://application:,,,/AnalysisControlsCore;component/Assets/CodeView.png" };
            _documentView = new ViewSpec() { ViewName = "Document"};
            _modelView = new ViewSpec() { ViewName = "Model", LargeImageSource = "pack://application:,,,/AnalysisControlsCore;component/Assets/ModelView.png" };
            _structureView = new ViewSpec() {ViewName = "Structure"};
            _diagView = new ViewSpec() { ViewName = "Diagnostics" };
            _views.Add(_codeView);
            _views.Add(_documentView);
            _views.Add(_structureView);
            _views.Add(_modelView);
            _views.Add(_diagView); 
            _currentView = _codeView;
        }

        private static void SyntaxNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (SyntaxNode)e.NewValue;
            if (node is CSharpSyntaxNode csn)
            {
                Walker w = new Walker();
                w.Visit(node);
                ((CodeDiagnostics)d).StructureRootNodes = w.CompilationUnitNode.Children;
            }
        }

        public List<StructureNode> StructureRootNodes
        {
            get { return _structureRootNodes; }
            set
            {
                if (Equals(value, _structureRootNodes)) return;
                _structureRootNodes = value;
                OnPropertyChanged();
            }
        }


        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _viewContainer = (Grid) GetTemplateChild("ViewContainer");
            _regions = (ListView)GetTemplateChild("regions");
            if (_regions != null) _regions.SelectionChanged += RegionsOnSelectionChanged;
            _lines = (ListView)GetTemplateChild("lines");
            _dgroup = (DrawingGroup) GetTemplateChild("dgroup");
            _rect = (Rectangle) GetTemplateChild("rect");
            CodeControl = (EnhancedCodeControl) GetTemplateChild("code");
            _lines.SelectionChanged += LinesOnSelectionChanged;
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            // if (CodeControl != null) Keyboard.Focus(CodeControl);
            // else Keyboard.Focus(this);
        }

        /// <inheritdoc />
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            // if (CodeControl != null) Keyboard.Focus(CodeControl);
        }

        public EnhancedCodeControl CodeControl
        {
            get { return _codeControl; }
            set
            {
                if (Equals(value, _codeControl)) return;
                _codeControl = value;
                if (HasEffectiveKeyboardFocus)
                {
                    Keyboard.Focus(_codeControl);
                }
                OnPropertyChanged();
            }
        }

        private void LinesOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineInfo line = (LineInfo)_lines.SelectedItem;
            _dgroup.Children.Clear();
            if (line == null)
                return;
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1), new RectangleGeometry(new Rect(line.Origin.X, line.Origin.Y, line.Size.Width, line.Size.Height))));
            _rect.InvalidateVisual();
        }

        private void RegionsOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_regions.SelectedItem == null)
            {
                return;
            }

            LineInfo line = (LineInfo) _lines.SelectedItem;
            RegionInfo r = (RegionInfo) _regions.SelectedItem;
            Rect newR = new Rect(r.BoundingRect.X, 0, r.BoundingRect.Width, r.BoundingRect.Height);
            _dgroup.Children.Clear();
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1),
                new RectangleGeometry(new Rect(0, 0, line.Size.Width, line.Size.Height))));
            _dgroup.Children.Add(new GeometryDrawing(null, null, new RectangleGeometry(new Rect(0, 0, 10, 10))));
            _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 1), new RectangleGeometry(newR)));
            foreach (var c in r.Characters)
            {
                _dgroup.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Red, 1),
                    new RectangleGeometry(new Rect(c.Bounds.X, c.Bounds.Y - r.BoundingRect.Y, c.Bounds.Width,
                        c.Bounds.Height))));
            }

           // SaveImage(r);

            _rect.InvalidateVisual();
            
        }

        private void SaveImage(RegionInfo r)
        {
            DrawingVisual v = new DrawingVisual();
            var dc = v.RenderOpen();
            dc.DrawRectangle((DrawingBrush) GetTemplateChild("DrawingBrush"), null, _dgroup.Bounds);
            dc.Close();
            RenderTargetBitmap rtb = new RenderTargetBitmap((int) _dgroup.Bounds.Width, (int) _dgroup.Bounds.Height, 96,
                96,
                PixelFormats.Pbgra32);
            rtb.Render(v);
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            var fname = $"img_{_lines.SelectedIndex}-{r.Offset}.png";
            using (var s = File.Create("C:\\temp\\" + fname))
            {
                png.Save(s);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ViewSpec> Views
        {
            get { return _views; }
            set
            {
                if (Equals(value, _views)) return;
                _views = value;
                OnPropertyChanged();
            }
        }

        public ViewSpec CurrentView
        {
            get { return _currentView; }
            set
            {
                if (Equals(value, _currentView)) return;
                _currentView = value;
                foreach (UIElement viewContainerChild in _viewContainer.Children)
                {
                    viewContainerChild.Visibility = ViewProperties.GetViewName(viewContainerChild)?.ToLowerInvariant() ==
                                                    _currentView.ViewName.ToLowerInvariant() ? Visibility.Visible : Visibility.Hidden;
                }
                OnPropertyChanged();
            }
        }
    }
}
