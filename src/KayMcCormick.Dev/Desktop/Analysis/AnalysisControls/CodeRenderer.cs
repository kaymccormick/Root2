using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using AnalysisAppLib;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CodeRenderer
    {
        public DrawingBrush _myDrawingBrush = new DrawingBrush();
        public DrawingGroup _textDest = new DrawingGroup();
        private Point _pos;
        private GeometryDrawing _geometryDrawing;
        private Rect _rect;
        public readonly List<List<char>> _chars = new List<List<char>>();
        public Rectangle Rectangle { get; set; }
        private GenericTextRunProperties _baseProps;
        private ITypefaceManager _typefaceManager;
        private FontFamily _fontFamily;
        private readonly FontStyle _fontStyle = FontStyles.Normal;
        private readonly FontWeight _fontWeight = FontWeights.Normal;
        private readonly FontStretch _fontStretch = FontStretches.Normal;
        private ErrorsTextSource _errorTextSource;
        public Rectangle _rect2;
        private DrawingGroup _dg2;
        private FontRendering _currentRendering;
        private ICustomTextSource _store;
        private DocumentPaginator _documentPaginator;
        public ICodeAnalyseContext AnalyseContext { get; }

        static CodeRenderer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public CodeRenderer()
        {
            PixelsPerDip = VisualTreeHelper.GetDpi(Visual).PixelsPerDip;
            TypefaceManager = new DefaultTypefaceManager();
        }

        /// <summary>
        /// 
        /// </summary>
        public Visual Visual { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FontRendering CurrentRendering
        {
            get
            {
                if (_currentRendering == null)
                    _currentRendering = new FontRendering(
                        EmSize,
                        TextAlignment.Left,
                        null,
                        Brushes.Black,
                        Typeface);
                return _currentRendering;
            }
            set { _currentRendering = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomTextSource TextSource
        {
            get { return _store ?? (_store = TextSourceService.CreateAndInitTextSource(ErrorsList, EmSize, PixelsPerDip, TypefaceManager, AnalyseContext)); }
            private set { _store = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double MaxY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TextFormatter Formatter { get; } = TextFormatter.Create();

        /// <summary>
        /// 
        /// </summary>
        public List<CompilationError> ErrorsList { get; } = new List<CompilationError>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CompilationError> Errors => ErrorsList;


        /// <summary>
        /// 
        /// </summary>
        public double PixelsPerDip { get; }

        /// <summary>
        /// 
        /// </summary>
        public double EmSize { get; set; }

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
        public List<RegionInfo> RegionInfoList { get; } = new List<RegionInfo>();

        /// <summary>
        /// 
        /// </summary>
        public Typeface Typeface { get; set; }

        private ITypefaceManager TypefaceManager
        {
            get { return _typefaceManager; }
            set { _typefaceManager = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdateTextSource()
        {
            if (AnalyseContext.Compilation != null && AnalyseContext.Compilation.SyntaxTrees.Contains(AnalyseContext.SyntaxTree) == false)
            {
                // _formattedTextControl.Compilation = null;
                DebugUtils.WriteLine("Compilation does not contain syntax tree.");
            }

            if (AnalyseContext.Node == null || AnalyseContext.SyntaxTree == null) return;
            if (ReferenceEquals(AnalyseContext.Node.SyntaxTree, AnalyseContext.SyntaxTree) == false)
                throw new InvalidOperationException("Node is not within syntax tree");
            DebugUtils.WriteLine("Creating new " + nameof(SyntaxNodeCustomTextSource));
            TextSource = TextSourceService.CreateAndInitTextSource(ErrorsList, EmSize, PixelsPerDip, TypefaceManager, AnalyseContext);
            _errorTextSource = ErrorsList.Any() ? new ErrorsTextSource(PixelsPerDip, ErrorsList,TypefaceManager) : null;
            _baseProps = TextSource.BaseProps;
            UpdateFormattedText_();
        }


        /// <summary>
        /// 
        /// </summary>
        public void UpdateFormattedText_()
        {
            if (CurrentRendering == null)
            {
                EmSize = (double) Rectangle.GetValue(TextElement.FontSizeProperty);
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
                    (TextSource) TextSource,
                    textStorePosition,
                    OutputWidth,
                    new GenericTextParagraphProperties(CurrentRendering, PixelsPerDip),
                    prev))
                {
                    var lineChars = new List<char>();

                    _chars.Add(lineChars);

                    var lineInfo = new LineInfo
                    {
                        Offset = textStorePosition,
                        Length = myTextLine.Length,
                        PrevLine = prevLine,
                        LineNumber = line
                    };

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
                                var textSpan = spans[@group];
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
                                RegionInfoList.Add(tuple);
                            }

                            @group++;
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

                    prev = textLineBreak;
                    if (prev != null) DebugUtils.WriteLine("Line break!");

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

            Rectangle.Width = MaxX;
            Rectangle.Height = _pos.Y;

            InvalidateVisual();
        }

        private void InvalidateVisual()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnNodeUpdated()
        {
            if (!SourceUpdateInProgress)
            {
                DebugUtils.WriteLine("Node updated");
                UpdateTextSource();
                UpdateFormattedText_();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SourceUpdateInProgress { get; set; }

        private void UpdateCompilation(Compilation compilation)
        {
            HandleDiagnostics(compilation.GetDiagnostics());
        }

        private void HandleDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                DebugUtils.WriteLine(diagnostic.ToString());
                MarkLocation(diagnostic.Location);
                if (diagnostic.Severity == DiagnosticSeverity.Error) ErrorsList.Add(new DiagnosticError(diagnostic));
            }
        }

        private void MarkLocation(Location diagnosticLocation)
        {
            switch (diagnosticLocation.Kind)
            {
                case LocationKind.SourceFile:
                    if (diagnosticLocation.SourceTree == AnalyseContext.SyntaxTree)
                    {
                        // ReSharper disable once UnusedVariable
                        var s = diagnosticLocation.SourceSpan.Start;
                    }

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emSize"></param>
        public void SetFontRenderingEmSize(double emSize)
        {
            
        }
    }
}