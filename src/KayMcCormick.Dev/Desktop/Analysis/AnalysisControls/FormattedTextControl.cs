﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class FormattedTextControl : SyntaxNodeControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverOffsetProperty = DependencyProperty.Register(
            "HoverOffset", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverRegionInfoProperty = DependencyProperty.Register(
            "HoverRegionInfo", typeof(RegionInfo), typeof(FormattedTextControl),
            new PropertyMetadata(default(RegionInfo)));

        /// <summary>
        /// 
        /// </summary>
        public RegionInfo HoverRegionInfo
        {
            get { return (RegionInfo) GetValue(HoverRegionInfoProperty); }
            set { SetValue(HoverRegionInfoProperty, value); }
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            base.EndInit();
            Model = Compilation.GetSemanticModel(SyntaxTree);
        }

        /// <summary>
        /// 
        /// </summary>
        public int HoverOffset
        {
            get { return (int) GetValue(HoverOffsetProperty); }
            set { SetValue(HoverOffsetProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverTokenProperty = DependencyProperty.Register(
            "HoverToken", typeof(SyntaxToken?), typeof(FormattedTextControl),
            new PropertyMetadata(default(SyntaxToken?)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverSymbolProperty = DependencyProperty.Register(
            "HoverSymbol", typeof(ISymbol), typeof(FormattedTextControl), new PropertyMetadata(default(ISymbol)));

        /// <summary>
        /// 
        /// </summary>
        public ISymbol HoverSymbol
        {
            get { return (ISymbol) GetValue(HoverSymbolProperty); }
            set { SetValue(HoverSymbolProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxToken? HoverToken
        {
            get { return (SyntaxToken?) GetValue(HoverTokenProperty); }
            set { SetValue(HoverTokenProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverSyntaxNodeProperty = DependencyProperty.Register(
            "HoverSyntaxNode", typeof(SyntaxNode), typeof(FormattedTextControl),
            new PropertyMetadata(default(SyntaxNode)));

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode HoverSyntaxNode
        {
            get { return (SyntaxNode) GetValue(HoverSyntaxNodeProperty); }
            set { SetValue(HoverSyntaxNodeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverColumnProperty = DependencyProperty.Register(
            "HoverColumn", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public int HoverColumn
        {
            get { return (int) GetValue(HoverColumnProperty); }
            set { SetValue(HoverColumnProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverRowProperty = DependencyProperty.Register(
            "HoverRow", typeof(int), typeof(FormattedTextControl), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public int HoverRow
        {
            get { return (int) GetValue(HoverRowProperty); }
            set { SetValue(HoverRowProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected FontRendering CurrentRendering { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool UiLoaded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomTextSource3 Store { get; private set; }

        private DrawingBrush _myDrawingBrush = new DrawingBrush();
        private DrawingGroup _textDest = new DrawingGroup();
        private Point _pos;

        /// <summary>
        /// 
        /// </summary>
        protected double MaxX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected double MaxY { get; set; }


        // ReSharper disable once UnusedMember.Local
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
                if (diagnostic.Severity == DiagnosticSeverity.Error) Errors.Add(new DiagnosticError(diagnostic));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompilationError> Errors { get; } = new List<CompilationError>();

        private void MarkLocation(Location diagnosticLocation)
        {
            switch (diagnosticLocation.Kind)
            {
                case LocationKind.SourceFile:
                    if (diagnosticLocation.SourceTree == SyntaxTree)
                    {
                        // ReSharper disable once UnusedVariable
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

        static FormattedTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattedTextControl),
                new FrameworkPropertyMetadata(typeof(FormattedTextControl)));
        }

        /// <summary>
        /// 
        /// </summary>
        public FormattedTextControl()

        {
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void UpdateTextSource()
        {
            if (!UiLoaded)
                return;
            if (Compilation != null && Compilation.SyntaxTrees.Contains(SyntaxTree) == false)
                throw new InvalidOperationException("Compilation does not contain syntax tree.");

            if (ReferenceEquals(Node.SyntaxTree, SyntaxTree) == false)
                throw new InvalidOperationException("Node is not within syntax tree");
            Store = new SyntaxNodeCustomTextSource(PixelsPerDip)
            {
                EmSize = EmSize,
                Compilation = Compilation,
                Tree = SyntaxTree,
                Node = Node,
                Errors = Errors
            };
            Store.Init();
            _errorTextSource = Errors.Any() ? new ErrorsTextSource(PixelsPerDip, Errors) : null;
            _baseProps = Store.BaseProps;
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        public double PixelsPerDip { get; }

        private GeometryDrawing _geometryDrawing;
        private Rect _rect;

        /// <summary>
        /// 
        /// </summary>
        public double EmSize { get; set; }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");
            if (_scrollViewer != null) OutputWidth = _scrollViewer.ActualWidth;
            _rectangle = (Rectangle) GetTemplateChild("Rectangle");
            var dpd = DependencyPropertyDescriptor.FromProperty(TextElement.FontSizeProperty, typeof(Rectangle));
            var dpd2 = DependencyPropertyDescriptor.FromProperty(TextElement.FontFamilyProperty, typeof(Rectangle));

            if (_rectangle != null)
            {
                dpd.AddValueChanged(_rectangle, Handler);
                dpd2.AddValueChanged(_rectangle, Handler2);
            }

            SetFontFamily();

            _grid = (Grid) GetTemplateChild("Grid");
            _border = (Border) GetTemplateChild("Border");
            _myDrawingBrush = (DrawingBrush) GetTemplateChild("DrawingBrush");

            _textDest = (DrawingGroup) GetTemplateChild("TextDest");

            UiLoaded = true;
            EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            UpdateTextSource();
            UpdateFormattedText();
        }

        private void Handler2(object sender, EventArgs e)
        {
            SetFontFamily();
            UpdateTextSource();
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetFontFamily()
        {
            if (_rectangle != null) _fontFamily = (FontFamily) _rectangle.GetValue(TextElement.FontFamilyProperty);
            if (_fontFamily != null) Typeface = new Typeface(_fontFamily, _fontStyle, _fontWeight, _fontStretch);
        }

        private void Handler(object sender, EventArgs e)
        {
            EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            _baseProps.SetFondRenderingEmSize(EmSize);
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateFormattedText()
        {
            // Make sure all UI is loaded
            if (!UiLoaded)
                return;

            if (CurrentRendering == null)
            {
                EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
                CurrentRendering = new FontRendering(
                    EmSize,
                    TextAlignment.Left,
                    null,
                    Brushes.Black,
                    Typeface);
            }

            var textStorePosition = 0;
            var linePosition = new Point(0, 0);

            // Create a DrawingGroup object for storing formatted text.

            var dc0 = _textDest.Open();
            var d = new DrawingGroup();
            var dc = d.Open();

            var formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            TextLineBreak prev = null;
            while (textStorePosition < Store.Length)
            {
                var line = 0;
                using (var myTextLine = formatter.FormatLine(
                    Store,
                    textStorePosition,
                    OutputWidth,
                    new GenericTextParagraphProperties(CurrentRendering, PixelsPerDip),
                    prev))
                {
                    var lineChars = new List<char>();

                    _chars.Add(lineChars);

                    var lineInfo = new LineInfo {Offset = textStorePosition, Length = myTextLine.Length};

                    LineInfos.Add(lineInfo);
                    var dd = new DrawingGroup();
                    var dc1 = dd.Open();
                    myTextLine.Draw(dc1, new Point(0, 0), InvertAxes.None);
                    dc1.Close();
                    lineInfo.Size = new Size(myTextLine.WidthIncludingTrailingWhitespace, myTextLine.Height);
                    lineInfo.Origin = new Point(linePosition.X, linePosition.Y);

                    var location = linePosition;
                    var group = 0;

                    var textRunSpans = myTextLine.GetTextRunSpans();
                    var spans = textRunSpans;
                    var cell = linePosition;
                    var cellColumn = 0;
                    var characterOffset = textStorePosition;
                    var regionOffset = textStorePosition;
                    var eol = myTextLine.GetTextRunSpans().Select(xx => xx.Value).OfType<TextEndOfLine>();
                    if (eol.Any())
                    {
                        // dc.DrawRectangle(Brushes.Aqua, null,
                        // new Rect(linePosition.X + myTextLine.WidthIncludingTrailingWhitespace + 2,
                        // linePosition.Y + 2, 10, 10));
                    }
                    else
                    {
                        DebugUtils.WriteLine("no end of line");
                        foreach (var textRunSpan in myTextLine.GetTextRunSpans())
                            DebugUtils.WriteLine(textRunSpan.Value.ToString());
                    }

                    var lineRegions = new List<RegionInfo>();

                    var lineString = "";
                    foreach (var rect in myTextLine.GetIndexedGlyphRuns())
                    {
                        var rectGlyphRun = rect.GlyphRun;
                        if (rectGlyphRun != null)
                        {
                            var size = new Size(0, 0);
                            var cellBounds =
                                new List<CharacterCell>();
                            var emSize = rectGlyphRun.FontRenderingEmSize;

                            for (var i = 0; i < rectGlyphRun.Characters.Count; i++)
                            {
                                size.Width += rectGlyphRun.AdvanceWidths[i];
                                var gi = rectGlyphRun.GlyphIndices[i];
                                var c = rectGlyphRun.Characters[i];
                                lineChars.Add(c);
                                lineString += c;
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
                                    // ReSharper disable once UnusedVariable
                                    var glyphTypefaceBaseline = rectGlyphRun.GlyphTypeface.Baseline;
                                    //DebugUtils.WriteLine(glyphTypefaceBaseline.ToString());
                                    //bounds.Offset(cell.X, cell.Y + glyphTypefaceBaseline);
                                    // dc.DrawRectangle(Brushes.White, null,  bounds);
                                    // dc.DrawText(
                                    // new FormattedText(cellColumn.ToString(), CultureInfo.CurrentCulture,
                                    // FlowDirection.LeftToRight, new Typeface("Arial"), _emSize * .66, Brushes.Aqua,
                                    // new NumberSubstitution(), _pixelsPerDip), new Point(bounds.Left, bounds.Top));
                                }

                                cellBounds.Add(new CharacterCell(bounds, new Point(cellColumn, _chars.Count - 1), c));
                                cell.Offset(rectGlyphRun.AdvanceWidths[i], 0);

                                cellColumn++;
                                characterOffset++;
                                //                                _textDest.Children.Add(new GeometryDrawing(null, new Pen(Brushes.DarkOrange, 2), new RectangleGeometry(bounds)));
                            }

                            //var bb = rect.GlyphRun.BuildGeometry().Bounds;

                            size.Height += myTextLine.Height;
                            var r = new Rect(location, size);
                            location.Offset(size.Width, 0);
//                            dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r);
                            //rects.Add(r);
                            if (group < spans.Count)
                            {
                                var textSpan = spans[group];
                                var textSpanValue = textSpan.Value;
                                SyntaxNode node = null;
                                SyntaxToken? token = null;
                                SyntaxTrivia? trivia = null;
                                if (textSpanValue is SyntaxTokenTextCharacters stc)
                                {
                                    node = stc.Node;
                                    token = stc.Token;
                                }
                                else if (textSpanValue is SyntaxTriviaTextCharacters stc2)
                                {
                                    trivia = stc2.Trivia;
                                }

                                var tuple = new RegionInfo(textSpanValue, r, cellBounds)
                                {
                                    Offset = regionOffset,
                                    Length = textSpan.Length,
                                    SyntaxNode = node,
                                    SyntaxToken = token,
                                    Trivia = trivia
                                };
                                lineRegions.Add(tuple);
                                Infos.Add(tuple);
                            }

                            group++;
                            regionOffset = characterOffset;
                        }

                        lineInfo.Text = lineString;
                        lineInfo.Regions = lineRegions;
//                        DebugUtils.WriteLine(rect.ToString());
                        //dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r1);
                    }


                    var ddBounds = dd.Bounds;
                    if (!ddBounds.IsEmpty)
                        ddBounds.Offset(0, linePosition.Y);
                    //DebugUtils.WriteLine(line.ToString() + ddBounds.ToString());
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 1), ddBounds);

                    // Draw the formatted text into the drawing context.
                    myTextLine.Draw(dc, linePosition, InvertAxes.None);
                    // ReSharper disable once UnusedVariable
                    var p = new Point(linePosition.X + myTextLine.WidthIncludingTrailingWhitespace, linePosition.Y);
                    var textLineBreak = myTextLine.GetTextLineBreak();
                    if (textLineBreak != null) DebugUtils.WriteLine(textLineBreak.ToString());
                    line++;
                    if (line % 20 == 0)
                    {
                        dc.Close();
                        dc0.DrawDrawing(d);
                        d = new DrawingGroup();
                        dc = d.Open();
                    }

                    prev = textLineBreak;
                    if (prev != null) DebugUtils.WriteLine("Line break!");
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
                    if (myTextLine.Width >= MaxX) MaxX = myTextLine.Width;
                }

                _pos = linePosition;
            }


            dc.Close();
            dc0.DrawDrawing(d);
            // Persist the drawn text content.
            dc0.Close();

            _rectangle.Width = MaxX;
            _rectangle.Height = _pos.Y;

            InvalidateVisual();
        }

        /// <summary>
        /// 
        /// </summary>
        public double OutputWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LineInfo> LineInfos { get; } = new ObservableCollection<LineInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<RegionInfo> Infos { get; } = new List<RegionInfo>();
#if false
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            returnupda
            //var b = VisualTreeHelper.GetContentBounds(_dock);
            var _dock = _border;
            DrawingBrush br = new DrawingBrush(_textDest);
            br.SetValue(TileBrush.ViewboxProperty, _textDest.Bounds);
            br.Viewport = new Rect(0, _offset, _dock.ActualWidth, _dock.ActualHeight);
            drawingContext.DrawRectangle(br, null, new Rect(0, 0, _dock.ActualWidth, _dock.ActualHeight));
//            drawingContext.DrawDrawing(_textDest);
        }

#endif

        private readonly List<List<char>> _chars = new List<List<char>>();

        // ReSharper disable once NotAccessedField.Local
        private Border _border;

        // ReSharper disable once NotAccessedField.Local
        private Grid _grid;
        private Rectangle _rectangle;
        private ScrollViewer _scrollViewer;
        private GenericTextRunProperties _baseProps;

        /// <summary>
        /// 
        /// </summary>
        public Typeface Typeface { get; protected set; }

        private FontFamily _fontFamily;
        private readonly FontStyle _fontStyle = FontStyles.Normal;
        private readonly FontWeight _fontWeight = FontWeights.Normal;
        private readonly FontStretch _fontStretch = FontStretches.Normal;

        // ReSharper disable once NotAccessedField.Local
        private ErrorsTextSource _errorTextSource;

        // ReSharper disable once NotAccessedField.Local
        private int _startColumn;

        // ReSharper disable once NotAccessedField.Local
        private int _startRow;
        private int _startOffset;
        private DrawingGroup _selectionGeometry;

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(_rectangle);
            var zz = Infos.Where(x => x.BoundingRect.Contains(point)).ToList();
            if (zz.Count > 1)
                DebugUtils.WriteLine("Multiple regions matched");
            //    throw new InvalidOperationException();

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
                if (tuple.Trivia.HasValue) DebugUtils.WriteLine(tuple.ToString());

                if (tuple.SyntaxNode != HoverSyntaxNode)
                {
                    if (ToolTip is ToolTip tt) tt.IsOpen = false;
                    HoverSyntaxNode = tuple.SyntaxNode;
                    if (tuple.SyntaxNode != null)
                    {
                        ISymbol sym = null;
                        IOperation operation = null;
                        if (Model != null)
                        {
                            sym = Model?.GetDeclaredSymbol(tuple.SyntaxNode);
                            operation = Model.GetOperation(tuple.SyntaxNode);
                        }

                        if (sym != null)
                        {
                            HoverSymbol = sym;
                            DebugUtils.WriteLine(sym.Kind.ToString());
                        }

                        var node = tuple.SyntaxNode;
                        var nodes = new Stack<SyntaxNodeDepth>();
                        var depth = 0;
                        while (node != null)
                        {
                            node = node.Parent;
                            depth++;
                        }

                        depth--;
                        node = tuple.SyntaxNode;
                        while (node != null)
                        {
                            nodes.Push(new SyntaxNodeDepth {SyntaxNode = node, Depth = depth});
                            node = node.Parent;
                            depth--;
                        }


                        var content = new CodeToolTipContent()
                            {Symbol = sym, SyntaxNode = tuple.SyntaxNode, Nodes = nodes, Operation = operation};
                        var template = TryFindResource(new DataTemplateKey(typeof(CodeToolTipContent))) as DataTemplate;
                        var toolTip = new ToolTip {Content = content, ContentTemplate = template};
                        ToolTip = toolTip;
                        toolTip.IsOpen = true;
                    }
                }

                if (tuple.SyntaxNode != null)
                {
                }

                HoverToken = tuple.SyntaxToken;

                var cellIndex = tuple.Characters.FindIndex(zx => zx.Bounds.Contains(point));
                if (cellIndex != -1)
                {
                    var cell = tuple.Characters[cellIndex];

                    var first = cell;
                    var item2 = first.Point;

                    var item2Y = (int) item2.Y;
                    if (item2Y >= _chars.Count)
                    {
                        DebugUtils.WriteLine("out of bounds");
                    }
                    else
                    {
                        var chars = _chars[item2Y];
                        DebugUtils.WriteLine("y is " + item2Y);
                        var item2X = (int) item2.X;
                        if (item2X >= chars.Count)
                        {
                            //DebugUtils.WriteLine("out of bounds");
                        }
                        else
                        {
                            var ch = chars[item2X];
                            DebugUtils.WriteLine("Cell is " + item2 + " " + ch);
                            var newOffset = tuple.Offset + cellIndex;
                            HoverOffset = newOffset;
                            HoverColumn = (int) item2.X;
                            HoverRow = (int) item2.Y;
                            if (SelectionEnabled && IsSelecting)
                            {
                                if (_selectionGeometry != null) _textDest.Children.Remove(_selectionGeometry);
                                DebugUtils.WriteLine("Calculating selection");

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

                                var green = new SolidColorBrush(Colors.Green) {Opacity = .2};
                                var blue = new SolidColorBrush(Colors.Blue) {Opacity = .2};
                                var red = new SolidColorBrush(Colors.Red) {Opacity = .2};
                                foreach (var regionInfo in Infos.Where(info =>
                                    info.Offset <= begin && info.Offset + info.Length > begin ||
                                    info.Offset >= begin && info.Offset + info.Length <= end))
                                {
                                    DebugUtils.WriteLine(
                                        $"Region offset {regionInfo.Offset} : Length {regionInfo.Length}");
                                    if (regionInfo.Offset <= begin)
                                    {
                                        var takeNum = begin - regionInfo.Offset;
                                        DebugUtils.WriteLine("Taking " + takeNum);
                                        foreach (var tuple1 in regionInfo.Characters.Take(takeNum))
                                        {
                                            DebugUtils.WriteLine("Adding " + tuple1);
                                            group.Children.Add(new GeometryDrawing(red, null,
                                                new RectangleGeometry(tuple1.Bounds)));
                                        }

                                        continue;
                                    }

                                    if (regionInfo.Offset + regionInfo.Length > end)
                                    {
                                        foreach (var tuple1 in regionInfo.Characters.Take(end - regionInfo.Offset))
                                            group.Children.Add(new GeometryDrawing(blue, null,
                                                new RectangleGeometry(tuple1.Bounds)));

                                        continue;
                                    }

                                    var geo = new RectangleGeometry(regionInfo.BoundingRect);
                                    group.Children.Add(new GeometryDrawing(green, null, geo));
                                }


                                _selectionGeometry = group;
                                _textDest.Children.Add(_selectionGeometry);
                                _myDrawingBrush.Drawing = _textDest;
                                InvalidateVisual();
                            }
                        }
                    }
                }

                var textRunProperties = tuple.TextRun.Properties;
                if (!(textRunProperties is GenericTextRunProperties)) continue;
                if (_rect != tuple.BoundingRect)
                {
                    _rect = tuple.BoundingRect;
                    if (_geometryDrawing != null) _textDest.Children.Remove(_geometryDrawing);

                    var solidColorBrush = new SolidColorBrush(Colors.CadetBlue) {Opacity = .6};


                    _geometryDrawing =
                        new GeometryDrawing(solidColorBrush, null, new RectangleGeometry(tuple.BoundingRect));

                    _textDest.Children.Add(_geometryDrawing);
                    InvalidateVisual();
                }

                //DebugUtils.WriteLine(pp.Text);
            }
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            IsSelecting = false;
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (SelectionEnabled)
            {
                _startOffset = HoverOffset;
                _startRow = HoverRow;
                _startColumn = HoverColumn;
                IsSelecting = true;
            }
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            // _startOffset = HoverOffset;
            // _startRow = HoverRow;
            // _startColumn = HoverColumn;
            // _selecting = true;
        }
    }
}