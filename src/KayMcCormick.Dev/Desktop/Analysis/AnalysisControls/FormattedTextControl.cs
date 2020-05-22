using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using AnalysisAppLib;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Lib.Wpf;
using Microsoft.CodeAnalysis;
using Border = System.Windows.Controls.Border;
using FontFamily = System.Windows.Media.FontFamily;
using TextAlignment = System.Windows.TextAlignment;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [TitleMetadata("Formatted Code Control")]
    public class FormattedTextControl : SyntaxNodeControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty InsertionPointProperty = DependencyProperty.Register(
            "InsertionPoint", typeof(int), typeof(FormattedTextControl),
            new PropertyMetadata(default(int), OnInsertionPointUpdated));

        /// <summary>
        /// 
        /// </summary>
        public int InsertionPoint
        {
            get { return (int) GetValue(InsertionPointProperty); }
            set { SetValue(InsertionPointProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceTextProperty = DependencyProperty.Register(
            "SourceText", typeof(string), typeof(FormattedTextControl), new PropertyMetadata("", OnSourceTextUpdated));

        private static void OnInsertionPointUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine($"Insertion Point Updated to {e.NewValue}", DebugCategory.TextFormatting);
        }

        private static void OnSourceTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (FormattedTextControl) d;

            var eNewValue = (string) e.NewValue;
            if (e.OldValue == null || c.SyntaxTree == null)
            {
                //    var tree = CSharpSyntaxTree.ParseText(eNewValue, new CSharpParseOptions(LanguageVersion.CSharp7_3));
                //    c.SyntaxTree = tree;
            }
            else
            {
                var newTree = c.SyntaxTree.WithChangedText(Microsoft.CodeAnalysis.Text.SourceText.From(eNewValue));
                c.SyntaxTree = newTree;
#if false
                foreach (var textChange in newTree.GetChanges(c.SyntaxTree))
                {
		DebugUtils.WriteLine($"{textChange.Span}", DebugCategory.TextFormatting);
                    var i = textChange.Span.Start;
                    LineInfo theLine = null;
                    for (var line = c.LineInfos.FirstOrDefault(); line != null; line = line.NextLine)
                    {
                        
                        if (line.Offset + line.Length >= i)
                        {
                            theLine = line;
                            break;
                        }
                    }

                    if (theLine != null)
                    {
                        var region = theLine.Regions[0];
                        while (region != null)
                        {
                            if (region.Offset + region.Length >= i)
                            {
                                break;
                            }
                            region = region.NextRegion;
                        }

                        if (region != null)
                        {
                            var chi = i - region.Offset;

                        }
                    }
                    
                }
#endif
                foreach (var changedSpan in c.SyntaxTree.GetChangedSpans(newTree))
                {
                }
            }


            if (!string.IsNullOrWhiteSpace(eNewValue))
            {
                var ctx = AnalysisService.Parse(eNewValue, "x");
                c.SyntaxTree = ctx.SyntaxTree;
                c.Model = ctx.CurrentModel;
                c.Compilation = ctx.Compilation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceText
        {
            get { return (string) GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value); }
        }

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
            Model = Compilation?.GetSemanticModel(SyntaxTree);
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
        protected FontRendering CurrentRendering
        {
            get
            {
                if (_currentRendering == null)
                    _currentRendering = FontRendering.CreateInstance(EmSize,
                        TextAlignment.Left,
                        null,
                        Brushes.Black,
                        Typeface);
                return _currentRendering;
            }
            private set { _currentRendering = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool UiLoaded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected CustomTextSource3 TextSource
        {
            get
            {
                if (_store == null) _store = CreateAndInitTextSource(PixelsPerDip, TypefaceManager);
                return _store;
            }
            set { _store = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        /// <param name="typefaceManager"></param>
        /// <returns></returns>
        protected CustomTextSource3 CreateAndInitTextSource(double pixelsPerDip, ITypefaceManager typefaceManager)
        {
            var source = new SyntaxNodeCustomTextSource(pixelsPerDip, typefaceManager)
            {
                EmSize = EmSize,
                Compilation = Compilation,
                Tree = SyntaxTree,
                Node = Node,
                Errors = Errors
            };
            source.Init();
            return source;
        }

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
        private void UpdateCompilation(Compilation compilation)
        {
            HandleDiagnostics(compilation.GetDiagnostics());
        }

        private void HandleDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                DebugUtils.WriteLine(diagnostic.ToString(), DebugCategory.TextFormatting);
                MarkLocation(diagnostic.Location);
                if (diagnostic.Severity == DiagnosticSeverity.Error) Errors.Add(new DiagnosticError(diagnostic));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<CompilationError> Errors { get; } = new List<CompilationError>();

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
            DebugUtils.WriteLine(constraint.ToString(), DebugCategory.TextFormatting);
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
            CommandBindings.Add(new CommandBinding(WpfAppCommands.SerializeContents, Executed));
            TypefaceManager = new DefaultTypefaceManager();
        }

        private async void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var text = await SyntaxTree.GetTextAsync();
            var root = await SyntaxTree.GetRootAsync();
            using (var s = new FileStream(@"C:\temp\serialize.bin", FileMode.Create))

            {
                root.SerializeTo(s);
            }
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
            {
                Compilation = null;
                DebugUtils.WriteLine("Compilation does not contain syntax tree.");
            }

            if (Node == null || SyntaxTree == null) return;
            if (ReferenceEquals(Node.SyntaxTree, SyntaxTree) == false)
                throw new AppInvalidOperationException("Node is not within syntax tree");
            DebugUtils.WriteLine("Creating new " + nameof(SyntaxNodeCustomTextSource), DebugCategory.TextFormatting);
            TextSource = CreateAndInitTextSource(PixelsPerDip, TypefaceManager);
            _errorTextSource = Errors.Any() ? new ErrorsTextSource(PixelsPerDip, Errors,TypefaceManager) : null;
            _baseProps = TextSource.BaseProps;
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
                // dpd.AddValueChanged(_rectangle, Handler);
                // dpd2.AddValueChanged(_rectangle, Handler2);
            }

            SetFontFamily();


            _grid = (Grid) GetTemplateChild("Grid");
            _canvas = (Canvas) GetTemplateChild("Canvas");
            _innerGrid = (Grid) GetTemplateChild("InnerGrid");
            var tryGetGlyphTypeface = Typeface.TryGetGlyphTypeface(out var gf);
            EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);

            _textCaret = new TextCaret(gf.Height * EmSize);
            _canvas.Children.Add(_textCaret);

            _border = (Border) GetTemplateChild("Border");
            _myDrawingBrush = (DrawingBrush) GetTemplateChild("DrawingBrush");

            _textDest = (DrawingGroup) GetTemplateChild("TextDest");
            _rect2 = (Rectangle) GetTemplateChild("Rect2");
            _dg2 = (DrawingGroup) GetTemplateChild("DG2");
            UiLoaded = true;

            UpdateTextSource();
            if (TextSource != null) UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        public LineInfo InsertionLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharacterCell InsertionCharacter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RegionInfo InsertionRegion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.Key)
            {
                case Key.Left:
                {
                    var ip = --InsertionPoint;
                    if (ip < 0) ip = 0;
                    DebugUtils.WriteLine($"{ip}");

                    var newc = InsertionCharacter.PreviousCell;
                    if (newc?.Region != InsertionRegion)
                    {
                        InsertionRegion = newc.Region;
                        if (newc.Region.Line != InsertionLine) InsertionLine = newc.Region.Line;
                    }

                    InsertionCharacter = newc;
                    var top = InsertionLine.Origin.Y;
                    DebugUtils.WriteLine("Setting top to " + top, DebugCategory.TextFormatting);

                    _textCaret.SetValue(Canvas.TopProperty, top);
                    if (InsertionCharacter != null)
                        _textCaret.SetValue(Canvas.LeftProperty, InsertionCharacter.Bounds.Left);
                }
                    break;
                case Key.Right:
                {
                    DebugUtils.WriteLine("incrementing insertion point", DebugCategory.TextFormatting);
                    var ip = ++InsertionPoint;
                    DebugUtils.WriteLine($"{ip}", DebugCategory.TextFormatting);

                    var newc = InsertionCharacter.NextCell;
                    if (newc.Region != InsertionRegion)
                    {
                        InsertionRegion = newc.Region;
                        if (newc.Region.Line != InsertionLine) InsertionLine = newc.Region.Line;
                    }

                    InsertionCharacter = newc;

                    var top = InsertionLine.Origin.Y;
                    DebugUtils.WriteLine("Setting top to " + top, DebugCategory.TextFormatting);

                    _textCaret.SetValue(Canvas.TopProperty, top);
                    _textCaret.SetValue(Canvas.LeftProperty, InsertionCharacter.Bounds.Left);

                    e.Handled = true;
                    break;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);


            var prev = SourceText.Substring(0, InsertionPoint);
            var next = SourceText.Substring(InsertionPoint);
            var code = prev + e.Text + next;
            if (InsertionLine != null)
            {
                var l = InsertionLine.Text.Substring(0, InsertionPoint - InsertionLine.Offset) + e.Text;
                var end = InsertionLine.Offset + InsertionLine.Length;
                if (end - InsertionPoint > 0)
                {
                    var start = InsertionPoint - InsertionLine.Offset;
                    var length = end - InsertionPoint;
                    if (start + length > InsertionLine.Text.Length)
                        length = length - (start + length - InsertionLine.Text.Length);
                    l += InsertionLine.Text.Substring(start, length);
                }
            }

            ChangingText = true;
            TextSource.TextInput(InsertionPoint, e.Text);
            var d = new DrawingGroup();

            var dc = d.Open();
            var lineNo = InsertionLine?.LineNumber ?? 0;
            LineContext lineCtx;
            using (var myTextLine = Formatter.FormatLine(
                TextSource,
                InsertionLine?.Offset ?? 0,
                OutputWidth,
                new GenericTextParagraphProperties(CurrentRendering, PixelsPerDip), null))
            {
                var y = InsertionLine?.Origin.Y ?? 0;
                var x = InsertionLine?.Origin.X ?? 0;

                lineCtx = new LineContext()
                {
                    LineNumber = lineNo,
                    CurCellRow = lineNo,
                    LineInfo = InsertionLine,
                    LineOriginPoint = new Point(x, y),
                    MyTextLine = myTextLine,
                    MaxX = MaxX,
                    MaxY = MaxY,
                    TextStorePosition = InsertionLine?.Offset ?? 0
                };

                myTextLine.Draw(dc, lineCtx.LineOriginPoint, InvertAxes.None);
                var regions = new List<RegionInfo>();
                FormattingHelper.HandleTextLine(regions, ref lineCtx, out var lineI, null);
                InsertionLine = lineI;
            }

            dc.Close();
            DebugUtils.WriteLine($"{_rect.Width}x{_rect.Height}", DebugCategory.TextFormatting);
            _textDest.Children.Add(d);
            // if (_textDest.Children.Count < lineNo + 1)
            // {
            // _textDest.Children.Add(d.Children[0]);
            // }
            // else
            // {
            // _textDest.Children[lineNo] = d.Children[0];
            // }

            _rectangle.Width = lineCtx.MaxX;
            _rectangle.Height = lineCtx.MaxY;
            _rect2.Width = lineCtx.MaxX;
            _rect2.Height = lineCtx.MaxY;
            InvalidateVisual();

            InsertionPoint = InsertionPoint + e.Text.Length;
            if (e.Text.Length == 1)
            {
                //_textCaret.SetValue(Canvas.LeftProperty, 0);
            }

            //AdvanceInsertionPoint(e.Text.Length);

            DebugUtils.WriteLine("About to update source text", DebugCategory.TextFormatting);
            SourceText = code;
            DebugUtils.WriteLine("Done updating source text", DebugCategory.TextFormatting);
            ChangingText = false;
            e.Handled = true;
        }

        private void AdvanceInsertionPoint(int textLength)
        {
            InsertionPoint += textLength;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ChangingText { get; set; }

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

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            DebugUtils.WriteLine($"{newTemplate}", DebugCategory.TextFormatting);
        }

        private void OnPropertyChangedz(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            DebugUtils.WriteLine($"{e.Property.Name}");
            if (e.Property.Name == "DesignerView1")
            {
                DebugUtils.WriteLine($"{e.Property.Name} {e.OldValue} = {e.NewValue}", DebugCategory.TextFormatting);
                foreach (var m in e.NewValue.GetType().GetMethods()) DebugUtils.WriteLine(m.ToString(), DebugCategory.TextFormatting);
                foreach (var ii in e.NewValue.GetType().GetInterfaces()) DebugUtils.WriteLine(ii.ToString(), DebugCategory.TextFormatting);
            }
            else if (e.Property.Name == "InstanceBuilderContext")
            {
                DebugUtils.WriteLine($"{e.Property.Name} {e.OldValue} = {e.NewValue}", DebugCategory.TextFormatting);
            }
        }

        private void Handler(object sender, EventArgs e)
        {
            EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            _baseProps.SetFontRenderingEmSize(EmSize);
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        protected TextFormatter Formatter { get; set; } = TextFormatter.Create();


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
                CurrentRendering = FontRendering.CreateInstance(EmSize,
                    TextAlignment.Left,
                    null,
                    Brushes.Black,
                    Typeface);
            }

            var textStorePosition = 0;
            var linePosition = new Point(0, 0);

            // Create a DrawingGroup object for storing formatted text.

            var dc = _textDest.Open();

            // Format each line of text from the text store and draw it.
            TextLineBreak prev = null;
            LineInfo prevLine = null;
            CharacterCell prevCell = null;
            RegionInfo prevRegion = null;
            var line = 0;
            while (textStorePosition < TextSource.Length)
            {
                using (var myTextLine = Formatter.FormatLine(
                    TextSource,
                    textStorePosition,
                    OutputWidth,
                    new GenericTextParagraphProperties(CurrentRendering, PixelsPerDip),
                    prev))
                {
                    var lineChars = new List<char>();

                    _chars.Add(lineChars);

                    var lineInfo = new LineInfo {Offset = textStorePosition, Length = myTextLine.Length};
                    lineInfo.PrevLine = prevLine;
                    lineInfo.LineNumber = line;

                    if (prevLine != null) prevLine.NextLine = lineInfo;

                    prevLine = lineInfo;
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
                        DebugUtils.WriteLine("no end of line", DebugCategory.TextFormatting);
                        foreach (var textRunSpan in myTextLine.GetTextRunSpans())
                            DebugUtils.WriteLine(textRunSpan.Value.ToString(), DebugCategory.TextFormatting);
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

                            if (rectGlyphRun.Characters.Count > rectGlyphRun.GlyphIndices.Count)
                            {
                                DebugUtils.WriteLine($"Character mismatch");
                            }
                            for (var i = 0; i < rectGlyphRun.GlyphIndices.Count; i++)
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
                                    //DebugUtils.WriteLine(glyphTypefaceBaseline.ToString(), DebugCategory.TextFormatting);
                                    //bounds.Offset(cell.X, cell.Y + glyphTypefaceBaseline);
                                    // dc.DrawRectangle(Brushes.White, null,  bounds);
                                    // dc.DrawText(
                                    // new FormattedText(cellColumn.ToString(), CultureInfo.CurrentCulture,
                                    // FlowDirection.LeftToRight, new Typeface("Arial"), _emSize * .66, Brushes.Aqua,
                                    // new NumberSubstitution(), _pixelsPerDip), new Point(bounds.Left, bounds.Top));
                                }

                                var char0 = new CharacterCell(bounds, new Point(cellColumn, _chars.Count - 1), c)
                                {
                                    PreviousCell = prevCell
                                };

                                if (prevCell != null)
                                    prevCell.NextCell = char0;
                                prevCell = char0;

                                cellBounds.Add(char0);
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
                                    Line = lineInfo,
                                    Offset = regionOffset,
                                    Length = textSpan.Length,
                                    SyntaxNode = node,
                                    SyntaxToken = token,
                                    Trivia = trivia,
                                    PrevRegion = prevRegion
                                };
                                foreach (var ch in tuple.Characters) ch.Region = tuple;
                                lineRegions.Add(tuple);
                                if (prevRegion != null) prevRegion.NextRegion = tuple;
                                prevRegion = tuple;
                                Infos.Add(tuple);
                            }

                            group++;
                            regionOffset = characterOffset;
                        }

                        lineInfo.Text = lineString;
                        lineInfo.Regions = lineRegions;
                        //                        DebugUtils.WriteLine(rect.ToString(), DebugCategory.TextFormatting);
                        //dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r1);
                    }


                    var ddBounds = dd.Bounds;
                    if (!ddBounds.IsEmpty)
                        ddBounds.Offset(0, linePosition.Y);
                    //DebugUtils.WriteLine(line.ToString() + ddBounds.ToString(), DebugCategory.TextFormatting);
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 1), ddBounds);

                    // Draw the formatted text into the drawing context.
                    myTextLine.Draw(dc, linePosition, InvertAxes.None);
                    // ReSharper disable once UnusedVariable
                    var p = new Point(linePosition.X + myTextLine.WidthIncludingTrailingWhitespace, linePosition.Y);
                    var textLineBreak = myTextLine.GetTextLineBreak();
                    if (textLineBreak != null) DebugUtils.WriteLine(textLineBreak.ToString(), DebugCategory.TextFormatting);
                    line++;

                    prev = textLineBreak;
                    if (prev != null) DebugUtils.WriteLine("Line break!", DebugCategory.TextFormatting);

                    // Update the index position in the text store.
                    textStorePosition += myTextLine.Length;
                    // Update the line position coordinate for the displayed line.
                    linePosition.Y += myTextLine.Height;
                    if (myTextLine.Width >= MaxX) MaxX = myTextLine.Width;
                }

                _pos = linePosition;
            }


            dc.Close();
            // Persist the drawn text content.

            _rectangle.Width = MaxX;
            _rectangle.Height = _pos.Y;

            UpdateCaretPosition();
            InvalidateVisual();
        }

        private void UpdateCaretPosition()
        {
            var insertionPoint = InsertionPoint;
            var l0 = LineInfos.FirstOrDefault(l => l.Offset + l.Length >= insertionPoint);
            if (l0 != null)
            {
                InsertionLine = l0;
                _textCaret.SetValue(Canvas.TopProperty, l0.Origin.Y);
                var rr = l0.Regions.FirstOrDefault(r => r.Offset + r.Length >= insertionPoint);
                InsertionRegion = rr;
                if (rr != null)
                {
                    var ch = rr.Characters[insertionPoint - rr.Offset];
                    InsertionCharacter = ch;
                    var x = ch.Bounds.Right - ch.Bounds.Width / 2;
                    _textCaret.SetValue(Canvas.LeftProperty, x);
                }
            }
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

        public ITypefaceManager TypefaceManager
        {
            get { return _typefaceManager; }
            set { _typefaceManager = value; }
        }

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
        private Rectangle _rect2;
        private DrawingGroup _dg2;
        private Grid _innerGrid;
        private TextCaret _textCaret;
        private Canvas _canvas;
        private FontRendering _currentRendering;
        private CustomTextSource3 _store;
        private ITypefaceManager _typefaceManager;
        private readonly DocumentPaginator _documentPaginator;

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(_rectangle);
            var zz = Infos.Where(x => x.BoundingRect.Contains(point)).ToList();
            if (zz.Count > 1)
                DebugUtils.WriteLine("Multiple regions matched", DebugCategory.TextFormatting);
            //    throw new AppInvalidOperationException();

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
                if (tuple.Trivia.HasValue) DebugUtils.WriteLine(tuple.ToString(), DebugCategory.TextFormatting);

                if (tuple.SyntaxNode != HoverSyntaxNode)
                {
                    if (ToolTip is ToolTip tt) tt.IsOpen = false;
                    HoverSyntaxNode = tuple.SyntaxNode;
                    if (tuple.SyntaxNode != null)
                    {
                        ISymbol sym = null;
                        IOperation operation = null;
                        if (Model != null)
                            try
                            {
                                sym = Model?.GetDeclaredSymbol(tuple.SyntaxNode);
                                operation = Model.GetOperation(tuple.SyntaxNode);
                            }
                            catch
                            {
                                // ignored
                            }

                        if (sym != null)
                        {
                            HoverSymbol = sym;
                            DebugUtils.WriteLine(sym.Kind.ToString(), DebugCategory.TextFormatting);
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
                        DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                    }
                    else
                    {
                        var chars = _chars[item2Y];
                        DebugUtils.WriteLine("y is " + item2Y, DebugCategory.TextFormatting);
                        var item2X = (int) item2.X;
                        if (item2X >= chars.Count)
                        {
                            //DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                        }
                        else
                        {
                            var ch = chars[item2X];
                            DebugUtils.WriteLine("Cell is " + item2 + " " + ch, DebugCategory.TextFormatting);
                            var newOffset = tuple.Offset + cellIndex;
                            HoverOffset = newOffset;
                            HoverColumn = (int) item2.X;
                            HoverRow = (int) item2.Y;
                            if (SelectionEnabled && IsSelecting)
                            {
                                if (_selectionGeometry != null) _textDest.Children.Remove(_selectionGeometry);
                                DebugUtils.WriteLine("Calculating selection", DebugCategory.TextFormatting);

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
                                        $"Region offset {regionInfo.Offset} : Length {regionInfo.Length}", DebugCategory.TextFormatting);
                                    if (regionInfo.Offset <= begin)
                                    {
                                        var takeNum = begin - regionInfo.Offset;
                                        DebugUtils.WriteLine("Taking " + takeNum, DebugCategory.TextFormatting);
                                        foreach (var tuple1 in regionInfo.Characters.Take(takeNum))
                                        {
                                            DebugUtils.WriteLine("Adding " + tuple1, DebugCategory.TextFormatting);
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

        /// <summary>
        /// 
        /// </summary>
        protected override void OnNodeUpdated()
        {
            base.OnNodeUpdated();
            if (!ChangingText)
            {
                DebugUtils.WriteLine("Node updated", DebugCategory.TextFormatting);
                UpdateTextSource();
                UpdateFormattedText();
            }
        }

       
    }
    #if false
    /// <summary>
    /// 
    /// </summary>
    public class CodeAutomationPeer : TextAutomationPeer
    {
        public CodeAutomationPeer(FormattedTextControl formattedTextControl): base(formattedTextControl)
        {
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Document;
        }

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Invoke:
                    break;
                case PatternInterface.Selection:
                    break;
                case PatternInterface.Value:
                    break;
                case PatternInterface.RangeValue:
                    break;
                case PatternInterface.Scroll:
                    break;
                case PatternInterface.ScrollItem:
                    break;
                case PatternInterface.ExpandCollapse:
                    break;
                case PatternInterface.Grid:
                    break;
                case PatternInterface.GridItem:
                    break;
                case PatternInterface.MultipleView:
                    break;
                case PatternInterface.Window:
                    break;
                case PatternInterface.SelectionItem:
                    break;
                case PatternInterface.Dock:
                    break;
                case PatternInterface.Table:
                    break;
                case PatternInterface.TableItem:
                    break;
                case PatternInterface.Toggle:
                    break;
                case PatternInterface.Transform:
                    break;
                case PatternInterface.Text:
                    break;
                case PatternInterface.ItemContainer:
                    break;
                case PatternInterface.VirtualizedItem:
                    break;
                case PatternInterface.SynchronizedInput:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(patternInterface), patternInterface, null);
            }
            return base.GetPattern(patternInterface);
        }
    }
#endif
}