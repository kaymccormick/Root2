using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Windows.Size;

namespace AnalysisControls
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
    ///     <MyNamespace:DevTypeControl/>
    ///
    /// </summary>
    public class FormattedTextControl : Control
    {
        public static readonly DependencyProperty HoverColumnProperty = DependencyProperty.Register(
            "HoverColumn", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        public int HoverColumn
        {
            get { return (int) GetValue(HoverColumnProperty); }
            set { SetValue(HoverColumnProperty, value); }
        }

        public static readonly DependencyProperty HoverRowProperty = DependencyProperty.Register(
            "HoverRow", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        public int HoverRow
        {
            get { return (int) GetValue(HoverRowProperty); }
            set { SetValue(HoverRowProperty, value); }
        }
        private FontRendering _currentRendering;
        private bool _UILoaded = false;
        private CustomTextSource3 _textStore;
        private DrawingBrush myDrawingBrush = new DrawingBrush();
        private DrawingBrush _drawing= new DrawingBrush();
        private DrawingGroup _textDest = new DrawingGroup();
        private Point _pos;
        private double max_x;
        private SyntaxNode _node;
        public TextLine TheLine { get; private set; }

        public CSharpCompilation Compilation
        {
            get { return _compilation; }
            set
            {
                _compilation = value;
                UpdateCompilation(_compilation);
            }
        }

        private void UpdateCompilation(CSharpCompilation compilation)
        {
            HandleDiagnostics(compilation.GetDiagnostics());
        }

        private void HandleDiagnostics(ImmutableArray<Diagnostic> getDiagnostics)
        {
            foreach (var diagnostic in getDiagnostics)
            {
                MarkLocation(diagnostic.Location);

            }
        }

        private void MarkLocation(Location diagnosticLocation)
        {
            switch (diagnosticLocation.Kind)
            {
                case LocationKind.SourceFile:
                    if (diagnosticLocation.SourceTree == Tree)
                    {
                        var s = diagnosticLocation.SourceSpan.Start;

                    }

                    break;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _grid.Measure(constraint);
            var gridDesiredSize = _grid.DesiredSize;
            DebugUtils.WriteLine(gridDesiredSize.ToString());
            return gridDesiredSize;
            DebugUtils.WriteLine(constraint.ToString());
            return base.MeasureOverride(constraint);
            return new Size(max_x, _pos.Y);
        }

        public static readonly DependencyProperty SyntaxNodeProperty = DependencyProperty.Register(
            "SyntaxNode", typeof(SyntaxNode), typeof(FormattedTextControl), new PropertyMetadata(default(SyntaxNode)));

        public SyntaxNode SyntaxNode
        {
            get { return (SyntaxNode) GetValue(SyntaxNodeProperty); }
            set { SetValue(SyntaxNodeProperty, value); }
        }

        static FormattedTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattedTextControl), new FrameworkPropertyMetadata(typeof(FormattedTextControl)));
        }

        public FormattedTextControl()

        {
            _pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            
        }


        public static string MyTypeName(Type myType)
        {

            var myTypeName = myType.Name;
            if (myType == typeof(Boolean))
            {
                myTypeName = "bool";
            }
            else if (myType == typeof(Byte))
            {
                myTypeName = "byte";
            }
            else if (myType == typeof(SByte))
            {
                myTypeName = "sbyte";
            }
            else if (myType == typeof(Char))
            {
                myTypeName = "char";
            }
            else if (myType == typeof(Decimal))
            {
                myTypeName = "decimal";
            }
            else if (myType == typeof(Double))
            {
                myTypeName = "double";
            }
            else if (myType == typeof(Single))
            {
                myTypeName = "float";
            }
            else if (myType == typeof(Int32))
            {
                myTypeName = "int";
            }
            else if (myType == typeof(UInt32))
            {
                myTypeName = "uint";
            }
            else if (myType == typeof(Int64))
            {
                myTypeName = "long";
            }
            else if (myType == typeof(UInt64))
            {
                myTypeName = "ulong";
            }
            else if (myType == typeof(Int16))
            {
                myTypeName = "short";
            }
            else if (myType == typeof(UInt16))
            {
                myTypeName = "ushort";
            }
            else if (myType == typeof(Object))
            {
                myTypeName = "object";
            }
            else if (myType == typeof(String))
            {
                myTypeName = "string";
            }
            else if (myType == typeof(void))
            {
                myTypeName = "void";
            }

            return myTypeName;
        }

        public bool MakeHyperlink { get; set; }

        [NotNull]
        private static object ToolTipContent([NotNull] Type myType, StackPanel pp = null)
        {
            var provider = new CSharpCodeProvider();
            var codeTypeReference = new CodeTypeReference(myType);
            var q = codeTypeReference;
            var toolTipContent = new TextBlock
            {
                Text = provider.GetTypeOutput(q),
                FontSize = 20
                //, Margin = new Thickness ( 15 )
            };
            if (pp == null)
            {
                pp = new StackPanel { Orientation = Orientation.Vertical };
            }

            pp.Children.Insert(0, toolTipContent);
            var @base = myType.BaseType;
            if (@base != null)
            {
                ToolTipContent(@base, pp);
            }

            return pp;
        }


        private void PopulateControl ( [ CanBeNull ] Type myType )
        {
            if ( myType == null )
            {
                return ;
            }

            var p = new Span ( ) ;
            
            //GenerateControlsForType ( myType , Inlines , true ) ;
            // foreach (object o in m0)
            // {
            // if (o is Inline il)
            // {
            // p.Inlines.Add(il);
            // } else
            // {
            // throw new InvalidOperationException();
            // }
            // }
            // addChild.Inlines.Add(p);
        }

        public bool Detailed { get; set; }

        public SyntaxNode 
            Node
        {
            get { return _node; }
            set
            {
                _node = value;
                UpdateFormattedText(_pixelsPerDip, null);
            }
        }

        public SyntaxTree Tree { get; set; }

        double _pixelsPerDip;
        private GeometryDrawing _geometryDrawing;
        private Rect _rect;
        private CSharpCompilation _compilation;
        private double _emSize;

        public override void OnApplyTemplate()

        {
            base.OnApplyTemplate();
            _scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");
            _rectangle = (System.Windows.Shapes.Rectangle) GetTemplateChild("Rectangle");
            _grid = (Grid) GetTemplateChild("Grid");
            _border = (Border) GetTemplateChild("Border");
            //_dock = (DockPanel) GetTemplateChild("DockPanel");
            _drawing = (DrawingBrush) GetTemplateChild("DrawingBrush");

            _scrollbar = (ScrollBar) GetTemplateChild("scrollbar");
            _scrollbar.Scroll += ScrollbarOnScroll;
            _textDest = (DrawingGroup)GetTemplateChild("TextDest");
            _canvas = (DrawingVisual) GetTemplateChild("Canvas");
            //
            _UILoaded = true;
            UpdateFormattedText(_pixelsPerDip, null);
        }

        private void ScrollbarOnScroll(object sender, ScrollEventArgs e)
        {
            switch(e.ScrollEventType)
            {
                case ScrollEventType.EndScroll:
                    _offset = e.NewValue* _totalHeight;
                    InvalidateVisual();
                    break;
                case ScrollEventType.First:
                    break;
                case ScrollEventType.LargeDecrement:
                    break;
                case ScrollEventType.LargeIncrement:
                    break;
                case ScrollEventType.Last:
                    break;
                case ScrollEventType.SmallDecrement:
                    break;
                case ScrollEventType.SmallIncrement:
                    break;
                case ScrollEventType.ThumbPosition:
                    break;
                case ScrollEventType.ThumbTrack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void UpdateFormattedText(
            double pixelsPerDip, DrawingContext drawingContext)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            if (_currentRendering == null)
            {
                var fontSize = (double)GetValue(TextElement.FontSizeProperty);
                _emSize = fontSize;
                _currentRendering = new FontRendering(
                    (double)_emSize,//fontSizeCB.SelectedItem,
                    TextAlignment.Left,
                    null,
                    System.Windows.Media.Brushes.Black,
                    new Typeface("Arial"));
            }

            // Initialize the text store.
            _textStore = new CustomTextSource3(_pixelsPerDip);
	    _textStore.Compilation = Compilation;
        _textStore.Tree = Tree;
        _textStore.Node = Node;


            int textStorePosition = 0;                //Index into the text of the textsource
            System.Windows.Point linePosition = new System.Windows.Point(0, 0);     //current line

            // Create a DrawingGroup object for storing formatted text.
            
            DrawingContext dc0 = _textDest.Open();
            var d = new DrawingGroup();
            var dc = d.Open();

            // Update the text store.
            _textStore.FontRendering = _currentRendering;

            // Create a TextFormatter object.
            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            TextLineBreak prev = null;
            while (textStorePosition < _textStore.length)
            {
                // Create a textline from the text store using the TextFormatter object.
             
                //DebugUtils.WriteLine(textStorePosition.ToString());
                int line = 0;
                using (TextLine myTextLine = formatter.FormatLine(
                    _textStore,
                    textStorePosition,
                    96 * 6,
                    new GenericTextParagraphProperties(_currentRendering, _pixelsPerDip),
                    prev))
                {
                    var lineChars = new List<char>();
                    Chars.Add(lineChars);

                    var textRunSpans = myTextLine.GetTextRunSpans();
                    foreach (var result in textRunSpans.Select(s => s.Value.Properties).OfType<GenericTextRunProperties>()
                        .Select(p => p.Text))
                    {
                        //DebugUtils.WriteLine(result);
                    }

                    var textLength = myTextLine.Length;
                    //DebugUtils.WriteLine(textLength);

                    var dd = new DrawingGroup();
                    var dc1 = dd.Open();
                    myTextLine.Draw(dc1, new Point(0, 0), InvertAxes.None);
                    dc1.Close();

                    var mycur = linePosition;
                    var groupi = 0;
                    List<Rect> rects = new List<Rect>();
                    var spans = textRunSpans.ToList();
                    var cell = linePosition;
                    var cellColumn = 0;
                    foreach (var rect in myTextLine.GetIndexedGlyphRuns()) {
                        var r1 = Rect.Empty;
                        if (rect.GlyphRun != null)
                        {
                            var size = new Size(0, 0);
                            List<System.Tuple<Rect, Point>> cellBounds = new List<Tuple<Rect, Point>>();
                            for (int i = 0; i < rect.GlyphRun.Characters.Count; i++)
                            {
                                size.Width += rect.GlyphRun.AdvanceWidths[i];
                                var gi = rect.GlyphRun.GlyphIndices[i];
                                var b = rect.GlyphRun.BaselineOrigin;
                                var c = rect.GlyphRun.Characters[i];
                                lineChars.Add(c);
                                var mapi = rect.GlyphRun.GlyphTypeface.CharacterToGlyphMap[c];
                                var glyphTypefaceAdvanceWidth = rect.GlyphRun.GlyphTypeface.AdvanceWidths[gi];
                                var glyphTypefaceAdvanceHeight = rect.GlyphRun.GlyphTypeface.AdvanceHeights[gi];
                                var s = new Size(glyphTypefaceAdvanceWidth * rect.GlyphRun.FontRenderingEmSize,
                                    (glyphTypefaceAdvanceHeight
                                    + rect.GlyphRun.GlyphTypeface.BottomSideBearings[gi])
                                    * rect.GlyphRun.FontRenderingEmSize);
                                
                                
                                var bounds = new Rect(new Point(cell.X, cell.Y + rect.GlyphRun.GlyphTypeface.TopSideBearings[gi]), s);
                                if (!bounds.IsEmpty)
                                {

                                    var glyphTypefaceBaseline = rect.GlyphRun.GlyphTypeface.Baseline;
                                    //DebugUtils.WriteLine(glyphTypefaceBaseline.ToString());
                                    //bounds.Offset(cell.X, cell.Y + glyphTypefaceBaseline);
                                    // dc.DrawRectangle(Brushes.White, null,  bounds);
                                    // dc.DrawText(
                                        // new FormattedText(cellColumn.ToString(), CultureInfo.CurrentCulture,
                                            // FlowDirection.LeftToRight, new Typeface("Arial"), _emSize * .66, Brushes.Aqua,
                                            // new NumberSubstitution(), _pixelsPerDip), new Point(bounds.Left, bounds.Top));

                                    
                                }

                                cellBounds.Add(Tuple.Create(bounds, new Point(cellColumn, Chars.Count - 1)));
                                cell.Offset(rect.GlyphRun.AdvanceWidths[i], 0);

                                cellColumn++;
                                //                                _textDest.Children.Add(new GeometryDrawing(null, new Pen(Brushes.DarkOrange, 2), new RectangleGeometry(bounds)));
                            }

                            //var bb = rect.GlyphRun.BuildGeometry().Bounds;

                            size.Height += myTextLine.Height;
                            Rect r = new Rect(mycur, size);
                            mycur.Offset(size.Width, 0);
//                            dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r);
                            //rects.Add(r);
                            if (groupi < spans.Count)
                            {
                                Infos.Add(Tuple.Create(spans[groupi].Value, r, cellBounds));
                            }

                            groupi++;

                            
                        }
                        
//                        DebugUtils.WriteLine(rect.ToString());
                        //dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r1);
                        
                        
                    }
                    var ddBounds = dd.Bounds;
                    if (!ddBounds.IsEmpty)
                    {
                        ddBounds.Offset(0, linePosition.Y);
                        //DebugUtils.WriteLine(line.ToString() + ddBounds.ToString());
                        //dc.DrawRectangle(null, new Pen(Brushes.Red, 1), ddBounds);
                    }

                    // Draw the formatted text into the drawing context.
                    myTextLine.Draw(dc, linePosition, InvertAxes.None);
                    line++;
                    if (line % 20 == 0)
                    {
                        dc.Close();
                        dc0.DrawDrawing(d);
                        d = new DrawingGroup();
                        dc = d.Open();

                    }
                    prev = myTextLine.GetTextLineBreak();
                    //DebugUtils.WriteLine(linePosition.Y.ToString());
                    // TheLine = myTextLine;
                    // var p2 = new Point(linePosition.X, linePosition.Y);
                    // foreach (var indexedGlyphRun in myTextLine.GetIndexedGlyphRuns())

                    // {
                        // var box = indexedGlyphRun.GlyphRun.BuildGeometry().Bounds;
                        // DebugUtils.WriteLine(box.ToString());
                        // var last = indexedGlyphRun.GlyphRun.AdvanceWidths.Last();
                        // p2.Offset(last, 0);
                        // dc.DrawRectangle(null, new Pen(Brushes.Pink,2), box);
                        
                        
                            
                        // DebugUtils.WriteLine(box.ToString());
                        // DebugUtils.WriteLine(indexedGlyphRun.GlyphRun.BuildGeometry().Bounds);
                        // p2.Offset(box.Width, 0);    
                    // }
                    
                    // Update the index position in the text store.
                    textStorePosition += myTextLine.Length;
                    // Update the line position coordinate for the displayed line.
                    linePosition.Y += myTextLine.Height;
                    if (myTextLine.Width >= max_x)
                    {
                        max_x = myTextLine.Width;
                    }
                    
                }

                _pos = linePosition;
            }


            dc.Close();
            dc0.DrawDrawing(d);
            // Persist the drawn text content.
            dc0.Close();

            _rectangle.Width = max_x;
            _rectangle.Height = _pos.Y;

            _drawing.Drawing = _textDest;
            InvalidateVisual();
            InvalidateMeasure();

            // Display the formatted text in the DrawingGroup object.
            //_drawing.Drawing = _textDest;
            //drawingContext?.DrawDrawing(_textDest);
            //InvalidateMeasure();
            //InvalidateVisual();

        }

        public List<Tuple<TextRun, Rect, List<Tuple<Rect, Point>>>> Infos { get; }= new List<Tuple<TextRun, Rect, List<Tuple<Rect, Point>>>>();
#if false
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            return;
            //var b = VisualTreeHelper.GetContentBounds(_dock);
            var _dock = _border;
            DrawingBrush br = new DrawingBrush(_textDest);
            br.SetValue(TileBrush.ViewboxProperty, _textDest.Bounds);
            br.Viewport = new Rect(0, _offset, _dock.ActualWidth, _dock.ActualHeight);
            drawingContext.DrawRectangle(br, null, new Rect(0, 0, _dock.ActualWidth, _dock.ActualHeight));
//            drawingContext.DrawDrawing(_textDest);
        }
        
#endif

        // protected override void OnPreviewMouseMove(MouseEventArgs e)
        // {
        // base.OnPreviewMouseMove(e);
        // var zz = Infos.Where(x => x.Item2.Contains(e.GetPosition(this))).ToList();
        // foreach (var tuple in zz)
        // {
        // var textRunProperties = tuple.Item1.Properties;
        // if (textRunProperties is GenericTextRunProperties pp)
        // {
        // DebugUtils.WriteLine(pp.Text);
        // }
        // }

        // }

        private List<List<char>> Chars = new List<List<char>>();
        private DrawingVisual _canvas;
        private ScrollBar _scrollbar;
        private double _totalHeight;
        private double _offset;
        private Border _border;
        private Grid _grid;
        private Rectangle _rectangle;
        private ScrollViewer _scrollViewer;

        protected override void OnMouseMove(MouseEventArgs e)

        {
            var zz = Infos.Where(x => x.Item2.Contains(e.GetPosition(this))).ToList();
            foreach (var tuple in zz)
            {
                var cell = tuple.Item3.Where(zx =>zx.Item1.Contains(e.GetPosition(this)));
                if (cell.Any())
                {
                    var item2 = cell.First().Item2;

                    var item2Y = (int) item2.Y;
                    if (item2Y >= Chars.Count)
                    {
                        DebugUtils.WriteLine("out of bounds");
                    }
                    else
                    {
                        var chars = Chars[item2Y];
                        DebugUtils.WriteLine("y is " + item2Y);
                        var item2X = (int) item2.X;
                        if (item2X >= chars.Count)
                        {
                            //DebugUtils.WriteLine("out of bounds");
                        }
                        else
                        {
                            var ch = chars[item2X];
                            DebugUtils.WriteLine("Cell is " + item2 + " " + ch.ToString());
                            HoverColumn = (int) item2.X;
                            HoverRow = (int) item2.Y;
                        }
                    }
                }
                var textRunProperties = tuple.Item1.Properties;
                if (textRunProperties is GenericTextRunProperties pp)
                {
                    
                    if (_rect != tuple.Item2)
                    {
                        _rect = tuple.Item2;
                        var g = (tuple.Item1.Properties as GenericTextRunProperties);
                        if (g.SyntaxToken != default)
                        {
                            SyntaxNode = g.SyntaxToken.Parent;
                        }
                        else
                        {
                            //SyntaxTrivia = g.SyntaxTrivia.
                        }
                        if (_geometryDrawing != null)
                        {
                            _textDest.Children.Remove(_geometryDrawing);
                        }

                        var solidColorBrush = new SolidColorBrush(Colors.CadetBlue) {Opacity = .6};

                        
                        _geometryDrawing = new GeometryDrawing(solidColorBrush, null, new RectangleGeometry(tuple.Item2));
                        
                        _textDest.Children.Add(_geometryDrawing);
                        InvalidateVisual();
                    }

                    //DebugUtils.WriteLine(pp.Text);
                }
            }

            return;
            DebugUtils.WriteLine(e.GetPosition(this).ToString());
            var gr
                = TheLine.GetIndexedGlyphRuns().Where(x =>
            {
                var computeAlignmentBox = x.GlyphRun.BuildGeometry().Bounds;
                DebugUtils.WriteLine(computeAlignmentBox.ToString());
                if (computeAlignmentBox.IsEmpty)
                {
                    return false;
                }
                computeAlignmentBox.Offset(x.GlyphRun.BaselineOrigin.X, x.GlyphRun.BaselineOrigin.Y);
                DebugUtils.WriteLine(computeAlignmentBox.ToString());
                var position = e.GetPosition(this);
                var alignmentBox = new Rect(new Point(0, 0),
                    new Size(position.X - computeAlignmentBox.X,
                        position.Y - computeAlignmentBox.Y));

                DebugUtils.WriteLine(alignmentBox.ToString());
                return computeAlignmentBox.Contains(position);
            }).ToList();
            if (gr.Any())
            {
                if (gr.Count() > 1)
                {
                    throw new InvalidOperationException();
                }

                DebugUtils.WriteLine(gr.First().GlyphRun.Characters.ToString());
            }

        }

        private HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            //DebugUtils.WriteLine(result.VisualHit.ToString());
            return HitTestResultBehavior.Continue;
        }

        private HitTestFilterBehavior FilterCallback(DependencyObject potentialhittesttarget)
        {
            
            //DebugUtils.WriteLine(potentialhittesttarget.ToString());
            return HitTestFilterBehavior.Continue;
        }

        public HitTestResultBehavior MyCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(System.Windows.Media.DrawingVisual))
            {
                ((System.Windows.Media.DrawingVisual)result.VisualHit).Opacity =
                    ((System.Windows.Media.DrawingVisual)result.VisualHit).Opacity == 1.0 ? 0.4 : 1.0;
            }

            // Stop the hit test enumeration of objects in the visual tree.
            return HitTestResultBehavior.Stop;
        }

    }
}