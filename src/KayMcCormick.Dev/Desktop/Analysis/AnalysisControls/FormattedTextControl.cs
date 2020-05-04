using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;
using InvalidOperationException = System.InvalidOperationException;
using Pen = System.Windows.Media.Pen;
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
        public static readonly DependencyProperty HoverOffsetProperty = DependencyProperty.Register(
            "HoverOffset", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty HoverRegionInfoProperty = DependencyProperty.Register(
            "HoverRegionInfo", typeof(RegionInfo), typeof(FormattedTextControl), new PropertyMetadata(default(RegionInfo)));

        public RegionInfo HoverRegionInfo
        {
            get { return (RegionInfo) GetValue(HoverRegionInfoProperty); }
            set { SetValue(HoverRegionInfoProperty, value); }
        }
        public override void EndInit()
        {
            base.EndInit();
            Model = Compilation.GetSemanticModel(Tree);
        }

        public int HoverOffset
        {
            get { return (int) GetValue(HoverOffsetProperty); }
            set { SetValue(HoverOffsetProperty, value); }
        }
        public static readonly DependencyProperty HoverTokenProperty = DependencyProperty.Register(
            "HoverToken", typeof(SyntaxToken?), typeof(FormattedTextControl), new PropertyMetadata(default(SyntaxToken?)));

        public static readonly DependencyProperty HoverSymbolProperty = DependencyProperty.Register(
            "HoverSymbol", typeof(ISymbol), typeof(FormattedTextControl), new PropertyMetadata(default(ISymbol)));

        public ISymbol HoverSymbol
        {
            get { return (ISymbol) GetValue(HoverSymbolProperty); }
            set { SetValue(HoverSymbolProperty, value); }
        }
        public SyntaxToken? HoverToken
        {
            get { return (SyntaxToken?) GetValue(HoverTokenProperty); }
            set { SetValue(HoverTokenProperty, value); }
        }
        public static readonly DependencyProperty HoverSyntaxNodeProperty = DependencyProperty.Register(
            "HoverSyntaxNode", typeof(SyntaxNode), typeof(FormattedTextControl), new PropertyMetadata(default(SyntaxNode)));

        public SyntaxNode HoverSyntaxNode
        {
            get { return (SyntaxNode) GetValue(HoverSyntaxNodeProperty); }
            set { SetValue(HoverSyntaxNodeProperty, value); }
        }
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

        private void HandleDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                DebugUtils.WriteLine(diagnostic.ToString());
                MarkLocation(diagnostic.Location);
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                {
                    Errors.Add(new DiagnosticError(diagnostic));
                }
            }
        }

        public List<CompilationError> Errors { get; } = new List<CompilationError>();

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

#if false
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
#endif

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


        public SyntaxNode 
            Node
        {
            get { return _node; }
            set
            {
                _node = value;
                UpdateTextSource();
                UpdateFormattedText(_pixelsPerDip, _scrollViewer?.ViewportWidth ?? 800);
            }
        }

        private void UpdateTextSource()
        {
            if (!_UILoaded)
                return;
            // Initialize the text store
            //
            if (Compilation != null && Compilation.SyntaxTrees.Contains(Tree) == false)
            {
                throw new InvalidOperationException("Compilation does not contain syntax tree.");
            }

            if (object.ReferenceEquals(Node.SyntaxTree, Tree) == false)
            {
                throw new InvalidOperationException("Node is not within syntax tree");
            }
            _textStore = new SyntaxNodeCustomTextSource(_pixelsPerDip)
            {
                EmSize = _emSize,
                Compilation = Compilation,
                Tree = Tree,
                Node = Node,
                Errors = Errors
            };
            _textStore.Init();
            if (Errors.Any())
            {
                _errorTextSource = new ErrorsTextSource(_pixelsPerDip, Errors);
            }
            else
            {
                _errorTextSource = null;
            }
            _baseProps = _textStore.BaseProps;
            UpdateFormattedText(_pixelsPerDip, _scrollViewer.ViewportWidth);
        }

        public SyntaxTree Tree
        {
            get { return _tree; }
            set
            {
                _tree = value;
                Node = _tree.GetRoot();
            }
        }

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
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(TextElement.FontSizeProperty, typeof(Rectangle));
            DependencyPropertyDescriptor dpd2 = DependencyPropertyDescriptor.FromProperty(TextElement.FontFamilyProperty, typeof(Rectangle));
            
            dpd.AddValueChanged(_rectangle, Handler);
            dpd2.AddValueChanged(_rectangle, Handler2);

            SetFontFamily();
            
            _grid = (Grid) GetTemplateChild("Grid");
            _border = (Border) GetTemplateChild("Border");
            //_dock = (DockPanel) GetTemplateChild("DockPanel");
            _drawing = (DrawingBrush) GetTemplateChild("DrawingBrush");
            
            _scrollbar = (ScrollBar) GetTemplateChild("scrollbar");
            //_scrollbar.LayoutUpdated += (sender, args) => UpdateFormattedText(_pixelsPerDip);
            _textDest = (DrawingGroup)GetTemplateChild("TextDest");
            _canvas = (DrawingVisual) GetTemplateChild("Canvas");
            //
            _UILoaded = true;
            _emSize = (double)_rectangle.GetValue(TextElement.FontSizeProperty);
            UpdateTextSource();
            UpdateFormattedText(_pixelsPerDip, _scrollViewer.ViewportWidth);
        }

        private void Handler2(object sender, EventArgs e)
        {
            SetFontFamily();
            UpdateTextSource();
            UpdateFormattedText(_pixelsPerDip, _scrollViewer.ViewportWidth);
        }

        private void SetFontFamily()
        {
            _FontFamily = (FontFamily) _rectangle.GetValue(TextElement.FontFamilyProperty);
            foreach (var typeface in _FontFamily.FamilyTypefaces)
            {
                DebugUtils.WriteLine(typeface.Weight + " " + typeface.Style + " " + typeface.Stretch);
            }

            _typeface = new Typeface(_FontFamily, _fontStyle, _fontWeight, _fontStretch);
            
        }

        private void Handler(object sender, EventArgs e)
        {
            _emSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            _baseProps.SetFondRenderingEmSize(_emSize);
            UpdateFormattedText(_pixelsPerDip, _scrollViewer.ViewportWidth);
        }


        private void UpdateFormattedText(
            double pixelsPerDip, double width)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            if (_currentRendering == null)
            {
                _emSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
                _currentRendering = new FontRendering(
                    (double) _emSize,
                    TextAlignment.Left,
                    null,
                    System.Windows.Media.Brushes.Black,
                    _typeface);
            }

            int textStorePosition = 0;                //Index into the text of the textsource
            System.Windows.Point linePosition = new System.Windows.Point(0, 0);     //current line

            // Create a DrawingGroup object for storing formatted text.
            
            DrawingContext dc0 = _textDest.Open();
            var d = new DrawingGroup();
            var dc = d.Open();

            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            TextLineBreak prev = null;
            while (textStorePosition < _textStore.length)
            {
                int line = 0;
                using (TextLine myTextLine = formatter.FormatLine(
                    _textStore,
                    textStorePosition,
                    width,
                    new GenericTextParagraphProperties(_currentRendering, _pixelsPerDip),
                    prev))
                {
                    var lineChars = new List<char>();
                    Chars.Add(lineChars);

                    
                    var dd = new DrawingGroup();
                    var dc1 = dd.Open();
                    myTextLine.Draw(dc1, new Point(0, 0), InvertAxes.None);
                    dc1.Close();

                     var  location = linePosition;
                    var groupi = 0;
                    List<Rect> rects = new List<Rect>();

                    var textRunSpans = myTextLine.GetTextRunSpans();
                    var spans = textRunSpans;
                    var cell = linePosition;
                    var cellColumn = 0;
                    var characterOffset = textStorePosition;
                    var regionOffset = textStorePosition;
                    foreach (var rect in myTextLine.GetIndexedGlyphRuns()) {
                        var r1 = Rect.Empty;
                        var rectGlyphRun = rect.GlyphRun;
                        if (rectGlyphRun != null)
                        {
                            var size = new Size(0, 0);
                            List<CharacterCell> cellBounds = 
                                new List<CharacterCell>();
                            var emSize = rectGlyphRun.FontRenderingEmSize;

                            for (int i = 0; i < rectGlyphRun.Characters.Count; i++)
                            {
                                size.Width += rectGlyphRun.AdvanceWidths[i];
                                var gi = rectGlyphRun.GlyphIndices[i];
                                var c = rectGlyphRun.Characters[i];
                                lineChars.Add(c);
                                var advWidth = rectGlyphRun.GlyphTypeface.AdvanceWidths[gi];
                                var advHeight = rectGlyphRun.GlyphTypeface.AdvanceHeights[gi];
                                
                                var s = new Size(advWidth * emSize,
                                    (advHeight
                                    + rectGlyphRun.GlyphTypeface.BottomSideBearings[gi])
                                    * emSize);

                                var topSide = rectGlyphRun.GlyphTypeface.TopSideBearings[gi];
                                var bounds = new Rect(new Point(cell.X, cell.Y + topSide), s);
                                if (!bounds.IsEmpty)
                                {

                                    var glyphTypefaceBaseline = rectGlyphRun.GlyphTypeface.Baseline;
                                    //DebugUtils.WriteLine(glyphTypefaceBaseline.ToString());
                                    //bounds.Offset(cell.X, cell.Y + glyphTypefaceBaseline);
                                    // dc.DrawRectangle(Brushes.White, null,  bounds);
                                    // dc.DrawText(
                                        // new FormattedText(cellColumn.ToString(), CultureInfo.CurrentCulture,
                                            // FlowDirection.LeftToRight, new Typeface("Arial"), _emSize * .66, Brushes.Aqua,
                                            // new NumberSubstitution(), _pixelsPerDip), new Point(bounds.Left, bounds.Top));

                                    
                                }

                                cellBounds.Add(new CharacterCell(bounds, new Point(cellColumn, Chars.Count - 1), c));
                                cell.Offset(rectGlyphRun.AdvanceWidths[i], 0);

                                cellColumn++;
                                characterOffset++;
                                //                                _textDest.Children.Add(new GeometryDrawing(null, new Pen(Brushes.DarkOrange, 2), new RectangleGeometry(bounds)));
                            }

                            //var bb = rect.GlyphRun.BuildGeometry().Bounds;

                            size.Height += myTextLine.Height;
                            Rect r = new Rect(location, size);
                            location.Offset(size.Width, 0);
//                            dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r);
                            //rects.Add(r);
                            if (groupi < spans.Count)
                            {
                                var textSpan = spans[groupi];
                                var textSpanValue = textSpan.Value;
                                SyntaxNode node = null;
                                SyntaxToken? token = null;
                                SyntaxTrivia? trivia = null;
                                if (textSpanValue is SyntaxTokenTextCharacters stc)
                                {
                                    node = stc.Node;
                                    token = stc.Token;
                                } else if (textSpanValue is SyntaxTriviaTextCharacters stc2)
                                {
                                    trivia = stc2.Trivia;
                                }
                                var tuple = new RegionInfo(textSpanValue, r, cellBounds);
                                tuple.Offset = regionOffset;
                                tuple.Length = textSpan.Length;
                                tuple.SyntaxNode = node;
                                tuple.SyntaxToken = token;
                                tuple.Trivia = trivia;
                                Infos.Add(tuple);
                            }

                            groupi++;
                            regionOffset = characterOffset;

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
                    if (prev != null)
                    {
                        DebugUtils.WriteLine("Line break!");
                    }
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

            //_drawing.Drawing = _textDest;

            // Display the formatted text in the DrawingGroup object.
            //_drawing.Drawing = _textDest;
            //drawingContext?.DrawDrawing(_textDest);
            //InvalidateMeasure();
            InvalidateVisual();
        }

        public List<RegionInfo> Infos { get; }= new List<RegionInfo>();
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
        private SyntaxTree _tree;
        private GenericTextRunProperties _baseProps;
        private Typeface _typeface;
        private FontFamily _FontFamily;
        private FontStyle _fontStyle = FontStyles.Normal;
        private FontWeight _fontWeight  =FontWeights.Normal;
        private FontStretch _fontStretch= FontStretches.Normal;
        private ErrorsTextSource _errorTextSource;
        private int _startColumn;
        private int _startRow;
        private int _startOffset;
        private bool _selecting;
        private DrawingGroup _selectionGeometry;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(_rectangle);
            var zz = Infos.Where(x => x.BoundingRect.Contains(point)).ToList();
            if (zz.Count() > 1)
            {
                DebugUtils.WriteLine("Multiple regions matched");
            //    throw new InvalidOperationException();
            }

            if (!zz.Any())
            {
                HoverColumn = 0;
                HoverSyntaxNode = null;
                HoverOffset = 0;
                HoverRegionInfo = null;
                HoverRow = 0;
                HoverSymbol = null;
                HoverToken = null;
            }
            foreach (var tuple in zz)
            {
                HoverRegionInfo = tuple;
                if (tuple.Trivia.HasValue)
                {
                    DebugUtils.WriteLine(tuple.ToString());
                }

                if (tuple.SyntaxNode != HoverSyntaxNode)
                {
                    HoverSyntaxNode = tuple.SyntaxNode;
                }
                if (tuple.SyntaxNode != null)
                {
                    

                    ISymbol sym
                        = Model.GetDeclaredSymbol(tuple.SyntaxNode);
                    if (sym != null)
                    {
                        HoverSymbol = sym;
                        DebugUtils.WriteLine(sym.Kind.ToString());
                    }
                }

                HoverToken = tuple.SyntaxToken;
                
                var cellindex = tuple.Characters.FindIndex(zx =>zx.Bounds.Contains(point));
                if (cellindex != -1)
                {
                    var cell = tuple.Characters[cellindex];
                    
                        var first = cell;
                        var item2 = first.Point;

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

                            var newOffset = tuple.Offset + cellindex;

                            
                            HoverOffset = newOffset;
                            HoverColumn = (int) item2.X;
                            HoverRow = (int) item2.Y;
                            if (_selecting)
                            {
                                if (_selectionGeometry != null)
                                {
                                    _textDest.Children.Remove(_selectionGeometry);
                                }
                                DebugUtils.WriteLine("Calcu=lating selection");

                                var group = new DrawingGroup(); 
                                
                                int begin;
                                int end;
                                if (_startOffset < newOffset)
                                {
                                    begin = _startOffset;
                                    end = newOffset;
                                }
                                else
                                {
                                    begin = newOffset;
                                    end = _startOffset;
                                }

                                var green = new SolidColorBrush(Colors.Green) { Opacity = .66 };
                                var blue = new SolidColorBrush(Colors.Blue) { Opacity = .4 };
                                var red = new SolidColorBrush(Colors.Red) { Opacity = .3 };
                                foreach (var regionInfo in Infos.Where(info => info.Offset <= begin && (info.Offset + info.Length) > begin || info.Offset >= begin && (info.Offset + info.Length) <= end))
                                {
                                    DebugUtils.WriteLine($"Region offset {regionInfo.Offset} : length {regionInfo.Length}");
                                    if (regionInfo.Offset <= begin)
                                    {
                                        var takenum = begin - regionInfo.Offset;
                                        DebugUtils.WriteLine("Taking " + takenum);
                                        foreach (var tuple1 in regionInfo.Characters.Take(takenum))
                                        {
                                            DebugUtils.WriteLine("Adding " + tuple1.ToString());
                                            group.Children.Add(new GeometryDrawing(red, null, new RectangleGeometry(tuple1.Bounds)));
                                        }

                                        continue;
                                    }
                                    if (regionInfo.Offset + regionInfo.Length > end)
                                    {
                                        foreach (var tuple1 in regionInfo.Characters.Take(end - regionInfo.Offset))
                                        {
                                            group.Children.Add(new GeometryDrawing(blue, null, new RectangleGeometry(tuple1.Bounds)));
                                        }

                                        continue;
                                    }
                                    var geo = new RectangleGeometry(regionInfo.BoundingRect);
                                    group.Children.Add(new GeometryDrawing(green, null, geo));
                                }


                                _selectionGeometry = group;
                                _textDest.Children.Add(_selectionGeometry);
                                myDrawingBrush.Drawing = _textDest;
                                InvalidateVisual();
                            }
                        }
                    }
                }
                var textRunProperties = tuple.TextRun.Properties;
                if (textRunProperties is GenericTextRunProperties pp)
                {
                    
                    if (_rect != tuple.BoundingRect)
                    {
                        _rect = tuple.BoundingRect;
                        var g = (tuple.TextRun.Properties as GenericTextRunProperties);
                        // if (g.SyntaxToken != default)
                        // {
                            // SyntaxNode = g.SyntaxToken.Parent;
                        // }
                        // else
                        // {
                            // SyntaxTrivia = g.SyntaxTrivia.
                        // }
                        if (_geometryDrawing != null)
                        {
                            _textDest.Children.Remove(_geometryDrawing);
                        }

                        var solidColorBrush = new SolidColorBrush(Colors.CadetBlue) {Opacity = .6};

                        
                        _geometryDrawing = new GeometryDrawing(solidColorBrush, null, new RectangleGeometry(tuple.BoundingRect));
                        
                        _textDest.Children.Add(_geometryDrawing);
                        InvalidateVisual();
                    }

                    //DebugUtils.WriteLine(pp.Text);
                }
            }

            return;
            DebugUtils.WriteLine(point.ToString());
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
                var position = point;
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

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _selecting = false;
        }

        public SemanticModel Model { get; set; }

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _startOffset = HoverOffset;
            _startRow = HoverRow;
            _startColumn = HoverColumn;
            _selecting = true;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            _startOffset = HoverOffset;
            _startRow = HoverRow;
            _startColumn = HoverColumn;
            _selecting = true;
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

        public static double[] CommonFontSizes => new double[] {
            3.0d, 4.0d, 5.0d, 6.0d, 6.5d, 7.0d, 7.5d, 8.0d, 8.5d, 9.0d,
            9.5d, 10.0d, 10.5d, 11.0d, 11.5d, 12.0d, 12.5d, 13.0d, 13.5d, 14.0d,
            15.0d, 16.0d, 17.0d, 18.0d, 19.0d, 20.0d, 22.0d, 24.0d, 26.0d, 28.0d,
            30.0d, 32.0d, 34.0d, 36.0d, 38.0d, 40.0d, 44.0d, 48.0d, 52.0d, 56.0d,
            60.0d, 64.0d, 68.0d, 72.0d, 76.0d, 80.0d, 88.0d, 96.0d, 104.0d, 112.0d,
            120.0d, 128.0d, 136.0d, 144.0d, 152.0d, 160.0d,  176.0d,  192.0d,  208.0d,
            224.0d, 240.0d, 256.0d, 272.0d, 288.0d, 304.0d, 320.0d, 352.0d, 384.0d,
            416.0d, 448.0d, 480.0d, 512.0d, 544.0d, 576.0d, 608.0d, 640.0d};
    }

    public class CharacterCell
    {
        public override string ToString()
        {
            return $"({Row}, {Column}) {Char}";
        }

        public Rect Bounds { get; }
        public Point Point { get; }
        public char Char { get; }

        public CharacterCell(Rect bounds, Point point, char c)
        {
            Bounds = bounds;
            Point = point;
            Column = (int) point.X;
            Row = (int) point.Y;
            Char = c;
        }

        public int Column { get; set; }

        public int Row { get; set; }
    }

    public class DiagnosticError : CompilationError
    {
        private readonly Diagnostic _diagnostic;

        public DiagnosticError(Diagnostic diagnostic)
        {
            _diagnostic = diagnostic;
            foreach (var kv in _diagnostic.Properties)
            {
                    DebugUtils.WriteLine($"{_diagnostic.Id}: {kv.Key}: {kv.Value}");
            }
            Message = _diagnostic.GetMessage();
        }
    }

    public class CompilationError
    {
        public string Message { get; set; }
    }
}