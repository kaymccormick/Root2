using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf;assembly=KayMcCormick.Lib.Wpf"
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
    ///     <MyNamespace:GradientEditorControl/>
    ///
    /// </summary>
    public class GradientEditorControl : Control, INotifyPropertyChanged
    {
        private GradientStopCollection _stopCol;

        public GradientEditorControl()
        {
            SetBinding(StartPointProperty, new Binding("LinearGradientBrush.StartPoint") { Source = this, Mode=BindingMode.TwoWay });
            SetBinding(EndPointProperty, new Binding("LinearGradientBrush.EndPoint") { Source = this, Mode=BindingMode.TwoWay });

        }

        public static readonly DependencyProperty StartPointXProperty = DependencyProperty.Register(
            "StartPointX", typeof(double), typeof(GradientEditorControl), new PropertyMetadata(default(double), OnStartPointXUpdated));

        private static void OnStartPointXUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GradientEditorControl c = (GradientEditorControl)d;
            c.OnStartPointXUpdated((double) e.OldValue, (double) e.NewValue);
        }

        private void OnStartPointXUpdated(double eOldValue, double eNewValue)
        {
            StartPoint = new Point(eNewValue, StartPoint.Y);
        }
        private static void OnStartPointYUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GradientEditorControl c = (GradientEditorControl)d;
            c.OnStartPointYUpdated((double)e.OldValue, (double)e.NewValue);
        }

        private void OnStartPointYUpdated(double eOldValue, double eNewValue)
        {
            StartPoint = new Point(StartPoint.X, eNewValue);
        }

        private static void OnEndPointXUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GradientEditorControl c = (GradientEditorControl)d;
            c.OnEndPointXUpdated((double)e.OldValue, (double)e.NewValue);
        }

        private void OnEndPointXUpdated(double eOldValue, double eNewValue)
        {
            EndPoint = new Point(eNewValue, EndPoint.Y);
        }

        private static void OnEndPointYUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GradientEditorControl c = (GradientEditorControl)d;
            c.OnEndPointYUpdated((double)e.OldValue, (double)e.NewValue);
        }

        private void OnEndPointYUpdated(double eOldValue, double eNewValue)
        {
            EndPoint = new Point(EndPoint.X, eNewValue);
        }

        public static readonly DependencyProperty EndPointXProperty = DependencyProperty.Register(
            "EndPointX", typeof(double), typeof(GradientEditorControl), new PropertyMetadata(default(double), OnEndPointXUpdated));

        public double EndPointX
        {
            get { return (double) GetValue(EndPointXProperty); }
            set { SetValue(EndPointXProperty, value); }
        }

        public static readonly DependencyProperty EndPointYProperty = DependencyProperty.Register(
            "EndPointY", typeof(double), typeof(GradientEditorControl), new PropertyMetadata(default(double), OnEndPointYUpdated));

        public double EndPointY
        {
            get { return (double) GetValue(EndPointYProperty); }
            set { SetValue(EndPointYProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(Point), typeof(GradientEditorControl), new PropertyMetadata(default(Point), OnEndPointUpdated));

        private static void OnEndPointUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (GradientEditorControl) d;
            c.OnEndPointUpdated((Point) e.OldValue, (Point) e.NewValue);
        }

        private void OnEndPointUpdated(Point eOldValue, Point eNewValue)
        {
            EndPointX = eNewValue.X;
            EndPointY = eNewValue.Y;
        }
        private static void OnStartPointUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (GradientEditorControl)d;
            c.OnStartPointUpdated((Point)e.OldValue, (Point)e.NewValue);
        }

        private void OnStartPointUpdated(Point eOldValue, Point eNewValue)
        {
            StartPointX = eNewValue.X;
            StartPointY = eNewValue.Y;
        }

        public Point EndPoint
        {
            get { return (Point) GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }
        public double StartPointX
        {
            get { return (double) GetValue(StartPointXProperty); }
            set { SetValue(StartPointXProperty, value); }
        }

        public static readonly DependencyProperty StartPointYProperty = DependencyProperty.Register(
            "StartPointY", typeof(double), typeof(GradientEditorControl), new PropertyMetadata(default(double), OnStartPointYUpdated));

        public double StartPointY
        {
            get { return (double) GetValue(StartPointYProperty); }
            set { SetValue(StartPointYProperty, value); }
        }

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
            "StartPoint", typeof(Point), typeof(GradientEditorControl), new PropertyMetadata(default(Point)));

        public Point StartPoint
        {
            get { return (Point) GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OnNewExecuted, CanNewExecute
            ));
        }

        private void CanNewExecute
            (object sender, CanExecuteRoutedEventArgs e)
        {
            if(_listView == null || _listView.ItemsSource == null)
                return;
            var view = CollectionViewSource.GetDefaultView(_listView.ItemsSource);
            if (view == null)
                return;
            switch (view)
            {
                case BindingListCollectionView bindingListCollectionView:
                    break;
                case ItemCollection itemCollection:
                    break;
                case ListCollectionView listCollectionView:
                    e.CanExecute = listCollectionView.CanAddNew;
                    break;
                case CollectionView collectionView:
                    break;
                default:
                    break;
            }
        }

        private void OnNewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (_listView == null)
                return;
            var view = CollectionViewSource.GetDefaultView(_listView.ItemsSource) as ListCollectionView;
            if (view == null)
                return;
            var newItem = view?.AddNew() as GradientStop;
        }

        public override void OnApplyTemplate()
        {
            _stopCol = (GradientStopCollection) GetTemplateChild("StopCol");
            _listView = (ListView) GetTemplateChild("ListView");
            _rectangle = (Rectangle) GetTemplateChild("Rectangle");
            _drawingGroup = (DrawingGroup) GetTemplateChild("DrawingGroup");
            _startPointGeometryDrawing = (GeometryDrawing) GetTemplateChild("StartPointGeometryDrawing");
            _endPointGeometryDrawing = (GeometryDrawing)GetTemplateChild("EndPointGeometryDrawing");
            _debugPanel = (Panel)GetTemplateChild("DebugPanel");
        }

        public static readonly DependencyProperty LinearGradientBrushProperty = DependencyProperty.Register(
            "LinearGradientBrush", typeof(LinearGradientBrush), typeof(GradientEditorControl), new PropertyMetadata(default(LinearGradientBrush), OnLinearGradientBrushUpdated));

        private static void OnLinearGradientBrushUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (GradientEditorControl) d;
            g.Translate = null;
        }

        private ListView _listView;
        private Rectangle _rectangle;
        private DrawingGroup _drawingGroup;
        private GeometryDrawing _startPointGeometryDrawing;
        private GeometryDrawing _dragging;
        private GeometryDrawing _endPointGeometryDrawing;
        private bool _mouseDown;
        private TranslateTransform _translate;
        private Point _startPos;
        private Panel _debugPanel;
        private double _mouseInRectX;
        private double _mouseInRectY;
        private bool _debugMode;
        private double _initialTranslateY;
        private double _initialTranslateX;

        public LinearGradientBrush LinearGradientBrush
        {
            get { return (LinearGradientBrush) GetValue(LinearGradientBrushProperty); }
            set { SetValue(LinearGradientBrushProperty, value); }
        }

        protected Point StartPos
        {
            get { return _startPos; }
            set { _startPos = value; }
        }

        protected TranslateTransform Translate
        {
            get { return _translate; }
            set { _translate = value; }
        }

        protected GeometryDrawing Dragging
        {
            get { return _dragging; }
            set { _dragging = value; }
        }

        static GradientEditorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GradientEditorControl), new FrameworkPropertyMetadata(typeof(GradientEditorControl)));
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            Dragging = null;
            IsMouseDown = false;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            DebugUtils.WriteLine(e.Key.ToString());
            if (e.Key == Key.D && e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt))
            {
                DebugMode = !DebugMode;
            }
        }

        public bool DebugMode
        {
            get { return _debugMode; }
            set
            {
                if (value == _debugMode) return;
                _debugMode = value;
                if (_debugMode)
                {
                    _debugPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    _debugPanel.Visibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!IsMouseDown && !_rectangle.IsMouseOver)
                return;
            
            var pos = e.GetPosition(_rectangle);
            var b = _startPointGeometryDrawing.Bounds;
            var s = _drawingGroup.Bounds.Size;
            var relX = pos.X / _rectangle.ActualWidth;
            var ratio = relX;
            var relY = pos.Y / _rectangle.ActualHeight;
            MouseInRectX = relX;
            MouseInRectY = relY;
            if (!IsMouseDown)
            {
                return;
            }
            var newPos = new Point(relX, relY);
            if (Dragging?.Geometry is EllipseGeometry rg)
            {
                rg.SetCurrentValue(EllipseGeometry.CenterProperty, newPos);
                return;
            }

            if (Dragging == null)
            {
                if (Translate != null)
                {
                    Translate.X = newPos.X - StartPos.X + _initialTranslateX;
                    Translate.Y = newPos.Y - StartPos.Y + _initialTranslateY;
                    DebugUtils.WriteLine(newPos.ToString());
                }
            }
        }

        public double MouseInRectY
        {
            get { return _mouseInRectY; }
            set
            {
                if (value.Equals(_mouseInRectY)) return;
                _mouseInRectY = value;
                OnPropertyChanged();
            }
        }

        public double MouseInRectX
        {
            get { return _mouseInRectX; }
            set
            {
                if (value.Equals(_mouseInRectX)) return;
                _mouseInRectX = value;
                OnPropertyChanged();
            }
        }

        public bool IsMouseDown
        {
            get { return _mouseDown; }
            set { _mouseDown = value; }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            DebugUtils.WriteLine(nameof(MouseDown));
            if (_rectangle.IsMouseOver)
            {
                IsMouseDown = true;
                var pos = e.GetPosition(_rectangle);
                var b = _startPointGeometryDrawing.Bounds;
                var s = _drawingGroup.Bounds.Size;
                var ratio = (pos.X / _rectangle.ActualWidth);
                var newPos = new Point(pos.X / _rectangle.ActualWidth, pos.Y / _rectangle.ActualHeight);

                if (_startPointGeometryDrawing.Bounds.Contains(newPos))
                {
                    Dragging = _startPointGeometryDrawing;
                }
                else if (_endPointGeometryDrawing.Bounds.Contains(newPos))
                {
                    Dragging = _endPointGeometryDrawing;
                }

                if (LinearGradientBrush == null)
                    return;
                
                IEnumerable<Transform> Flatten(Transform t, int curDepth = 0, int maxDepth = -1)
                {
                    if (curDepth > maxDepth)
                        return Enumerable.Empty<Transform>();
                    return t is TransformGroup tg
                        ? ((IEnumerable<Transform>) tg.Children).SelectMany(x => Flatten(x, curDepth + 1, maxDepth))
                        : Enumerable.Repeat<Transform>(t, 1);

                }

                StartPos = newPos;
                var tr = Translate ?? Flatten(LinearGradientBrush.RelativeTransform, 0, 1).OfType<TranslateTransform>()
                    .FirstOrDefault();
                if (tr == null)
                {
                    tr = new TranslateTransform();
                    var relativeTransform = LinearGradientBrush.RelativeTransform;
                    if (relativeTransform == null || !(relativeTransform is TransformGroup tg1))
                    {
                        var gr = new TransformGroup();
                        if (relativeTransform != null)
                            gr.Children.Add(relativeTransform);
                        gr.Children.Add(tr);
                        LinearGradientBrush.RelativeTransform = gr;
                    }
                    else if (tg1 != null)
                    {
                        tg1.Children.Add(tr);
                    }
                }

                Translate = tr;
                _initialTranslateX = tr.X;
                _initialTranslateY = tr.Y;
            }

            //fEnumDrawingGroup(_drawingGroup, pos);
        }

        // Determine if a geometry within the visual was hit.
        static public void HitTestGeometryInVisual(Visual visual, Point pt)
        {
            // Retrieve the group of drawings for the visual.
            DrawingGroup drawingGroup = VisualTreeHelper.GetDrawing(visual);
            EnumDrawingGroup(drawingGroup, pt);
        }

        // Enumerate the drawings in the DrawingGroup.
        static public void EnumDrawingGroup(DrawingGroup drawingGroup, Point pt)
        {
            DrawingCollection drawingCollection = drawingGroup.Children;

            // Enumerate the drawings in the DrawingCollection.
            foreach (Drawing drawing in drawingCollection)
            {
                // If the drawing is a DrawingGroup, call the function recursively.
                if (drawing.GetType() == typeof(DrawingGroup))
                {
                    EnumDrawingGroup((DrawingGroup)drawing, pt);
                }
                else if (drawing.GetType() == typeof(GeometryDrawing))
                {
                    // Determine whether the hit test point falls within the geometry.
                    if (((GeometryDrawing)drawing).Geometry.FillContains(pt))
                    {
                        // Perform action based on hit test on geometry.
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
