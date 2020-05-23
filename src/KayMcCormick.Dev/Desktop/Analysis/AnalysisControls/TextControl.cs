using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using TextLine = System.Windows.Media.TextFormatting.TextLine;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TextControl : Control, ILineDrawer
    {

        
        private void UpdateFormattedTextPartial()
        {
            DebugUtils.WriteLine(nameof(UpdateFormattedText));
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            if (Typeface == null) Typeface = new Typeface("Courier New");
            DrawingGroup iterationDrawingGroup = new DrawingGroup();
            double pixelsPerDip = PixelsPerDip;
            IList<LineInfo> lineInfos = new List<LineInfo>();
            _lineContext = FormattingHelper.PartialUpdateFormattedText(OutputWidth, ref currentRendering, EmSize,
                Typeface, iterationDrawingGroup,
                (AppTextSource) Store,
                pixelsPerDip, lineInfos, null, ref maxX, out var maxY, TextLineAction, _myDrawingBrush,
                new Rect(0, 0, maxX, MaxY), _textFormatter, _lineContext, ParaProps, this);
            _textDest.Children.Add(iterationDrawingGroup);
            if (_updatedTabs)
            {
                UpdateFormattedText();
                _updatedTabs = false;
                return;
            }
            MaxX = maxX;
            MaxY = maxY;
            _rectangle.Width = maxX;
            _rectangle.Height = maxY;
            _innerGrid.Width = maxX;
            _innerGrid.Height = maxY;


            var Dg = ((DrawingGroup)GetTemplateChild("DG2"));
            var gs = new GuidelineSet();
            var dc = Dg.Open();
            dc.DrawRectangle(null, new Pen(Brushes.Green, 3), new Rect(0, 0, maxX, maxY));

            foreach (var textTabPropertiese in Tabs)
            {
                var solidColorBrush = new SolidColorBrush(Colors.BlueViolet) {Opacity = .66};
                var pen = new Pen(solidColorBrush, 4);
                dc.DrawLine(pen, new Point(textTabPropertiese.Location, 0), null, new Point(textTabPropertiese.Location, MaxY), null);
                //gs.GuidelinesX.Add(textTabPropertiese.Location);
            }
            dc.Close();


            _rectangle2.Width = maxX;
            _rectangle2.Height = maxY;

            //Dg.GuidelineSet = gs;

            InvalidateVisual();
        }

        private void TextLineAction(TextLine obj, LineContext context)
        {
            if(_updatingTabs)
            {
                return;
            }
            int i = 0;
            var g = obj.GetIndexedGlyphRuns().GetEnumerator();
            List<double> widths = new List<double>();
            List<double> offsets = new List<double>();
            double offset = 0;
            int charOffset = context.TextStorePosition;
            int glyphOffset = 0;
            bool moveIterator = true;
            foreach (var textRunSpan in obj.GetTextRunSpans())
            {
                var run = textRunSpan.Value;
                if (moveIterator && !g.MoveNext())
                {
                    DebugUtils.WriteLine($"{charOffset}");
                    break;
                }

                moveIterator = true;
                var gg = g.Current;
                DebugUtils.WriteLine($"{charOffset} - {gg.TextSourceCharacterIndex}");
                if (gg.TextSourceCharacterIndex != charOffset)
                {
                    DebugUtils.WriteLine($"Warning {gg.TextSourceCharacterIndex} does ntoo match {charOffset}");
                    moveIterator = false;
                    charOffset += textRunSpan.Length;
                    if (run is TabTextRun)
                    {
                        offset += 4 * EmSize;
                    }

                    continue;
                }
                double width = 0;
                foreach (var glyphRunAdvanceWidth in gg.GlyphRun.AdvanceWidths)
                {
                    width += glyphRunAdvanceWidth;
                }

                if (run is CustomTextCharacters c)
                {
                    widths.Add(width);
                    offsets.Add(offset);
                }

                glyphOffset += gg.TextSourceLength;
                offset = offset + width;
                g.MoveNext();
                charOffset += textRunSpan.Length;
            }

            if (ColumnWidths.Count < widths.Count - 1)
            {
                ColumnWidths.AddRange(Enumerable.Repeat(-1.0, widths.Count - ColumnWidths.Count - 1));
                Offsets.AddRange(Enumerable.Repeat(-1.0, widths.Count - Offsets.Count - 1));
                
            }

            bool updatedTabs = false;
            
            for (int j = 1; j < widths.Count; j++)
            {
                if(ColumnWidths[j - 1] < widths[j])
                {
                    ColumnWidths[j - 1] = widths[j];
                    Offsets[j - 1] = offsets[j];
                    updatedTabs = true;
                
                }
            }
            
            if (updatedTabs)
            {
                DebugUtils.WriteLine("Setting tabs", DebugCategory.TextFormatting);
                Tabs.Clear();
                Tabs.AddRange(Offsets.Select(o => new TextTabProperties(TextTabAlignment.Left, o, 0, 0)));
                _updatedTabs = true;
                _paraProps = new GenericTextParagraphPropertiesImpl(CurrentRendering, PixelsPerDip, Tabs);
                foreach (var textTabPropertiese in _paraProps.Tabs)
                {
                    DebugUtils.WriteLine($"{textTabPropertiese.Location}\t{textTabPropertiese}");
                }
            }
        }

        public List<double> Offsets { get; set; } = new List<double>();

        public List<TextTabProperties> Tabs { get; set; } = new List<TextTabProperties>();

        public List<double> ColumnWidths { get; set; } = new List<double>();


        /// <summary>
        /// 
        /// </summary>
        protected FontRendering CurrentRendering
        {
            get
            {

                if (_currentRendering == null)
                {
                    _currentRendering = FontRendering.CreateInstance(EmSize,
                        TextAlignment.Left,
                        null,
                        Brushes.Black,
                        Typeface);
                    
                }

                return _currentRendering;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool UiLoaded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadWriteTextSource Store { get; private set; }

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

        static TextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextControl),
                new FrameworkPropertyMetadata(typeof(TextControl)));
        }

        /// <summary>
        /// 
        /// </summary>
        public TextControl()

        {
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected bool UpdateTextSource()
        {
            if (!UiLoaded)
                return false;

            
            Store = (IReadWriteTextSource) new TextSource0();
            
            Store.EmSize = EmSize;
            
            Store.Init();
            _baseProps = Store.BaseProps;
            return UpdateFormattedText();
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
            if (SingleLineMode)
            {
                OutputWidth = 0;
            }
            else
            {
                if (_scrollViewer != null) OutputWidth = _scrollViewer.ActualWidth;
            }

            _rectangle = (Rectangle) GetTemplateChild("Rectangle");
            var dpd = DependencyPropertyDescriptor.FromProperty(TextElement.FontSizeProperty, typeof(Rectangle));
            var dpd2 = DependencyPropertyDescriptor.FromProperty(TextElement.FontFamilyProperty, typeof(Rectangle));

            _rectangle2 = (Rectangle) GetTemplateChild("Rect2");
            _innerGrid = (Grid) GetTemplateChild("InnerGrid");
            
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
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            var eText = e.Text;

            var code = HandleInput(eText);

            DebugUtils.WriteLine("About to update source text", DebugCategory.TextFormatting);
            Text = code;
            DebugUtils.WriteLine("Done updating source text", DebugCategory.TextFormatting);
            ChangingText = false;
            e.Handled = true;
        }

        public string HandleInput(string eText)
        {
            var prev = Text.Substring(0, InsertionPoint);
            var next = Text.Substring(InsertionPoint);

            var code = prev + eText + next;
            if (InsertionLine != null)
            {
                var l = InsertionLine.Text.Substring(0, InsertionPoint - InsertionLine.Offset) + eText;
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
            Store.TextInput(InsertionPoint, eText);
            var d = new DrawingGroup();

            var dc = d.Open();
            var lineNo = InsertionLine?.LineNumber ?? 0;
            LineContext lineCtx;
            using (var myTextLine = _textFormatter.FormatLine(
                (TextSource) Store,
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
                FormattingHelper.HandleTextLine(regions, ref lineCtx, out var lineI, this);
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
            InvalidateVisual();

            InsertionPoint = InsertionPoint + eText.Length;
            if (eText.Length == 1)
            {
                //_textCaret.SetValue(Canvas.LeftProperty, 0);
            }

            //AdvanceInsertionPoint(e.Text.Length);
            return code;
        }

        public LineInfo InsertionLine { get; set; }

        public bool ChangingText { get; set; }

        public int InsertionPoint { get; set; }

        public string Text { get; set; } = "";


        public bool SingleLineMode { get; set; } = false;

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
            _baseProps.SetFontRenderingEmSize(EmSize);
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedTabs"></param>
        protected bool UpdateFormattedText(bool updatedTabs = false)
        {
            _updatingTabs = updatedTabs;
            DebugUtils.WriteLine(nameof(UpdateFormattedText), DebugCategory.TextFormatting);
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            if (Typeface == null) Typeface = new Typeface("Courier New");
            double pixelsPerDip = PixelsPerDip;
            var genericTextParagraphPropertiesImpl = ParaProps;
            
            _textDest.GuidelineSet = new GuidelineSet(null, Tabs.Select(x => x.Location).ToArray()).Clone();

            _lineContext = FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface, _textDest,
                (AppTextSource) Store,
                pixelsPerDip, LineInfos, Infos, ref maxX, out var maxY, TextLineAction, genericTextParagraphPropertiesImpl, this);

            if (!updatedTabs && _updatedTabs)
            {
                UpdateFormattedText(_updatedTabs);
                _updatedTabs = false;
                return false;
            }
            _updatingTabs = false;

            MaxX = maxX;
            MaxY = maxY;
            _rectangle.Width = maxX;
            _rectangle.Height = maxY;
            _rectangle2.Width = maxX;
            _rectangle2.Height = maxY;
            _innerGrid.Width = maxX;
            _innerGrid.Height = maxY;
            InvalidateVisual();
            return true;
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

        public GenericTextParagraphPropertiesImpl ParaProps
        {
            get
            {
                if (_paraProps == null)
                {
                    _paraProps = new GenericTextParagraphPropertiesImpl(CurrentRendering, PixelsPerDip, null);
                }
                return _paraProps;
            }
            set { _paraProps = value; }
        }

        private FontFamily _fontFamily;
        private readonly FontStyle _fontStyle = FontStyles.Normal;
        private readonly FontWeight _fontWeight = FontWeights.Normal;
        private readonly FontStretch _fontStretch = FontStretches.Normal;
        private LineContext _lineContext = new LineContext();
        private TextFormatter _textFormatter = TextFormatter.Create();
        private GenericTextParagraphPropertiesImpl _paraProps;
        private bool _updatedTabs;
        private FontRendering _currentRendering;
        private bool _updatingTabs;
        private Rectangle _rectangle2
            ;

        private Grid _innerGrid;
        private DrawingContext _currentDrawingContext;

        // ReSharper disable once NotAccessedField.Local

        // ReSharper disable once NotAccessedField.Local

        public void PrepareDrawLines(LineContext lineContext, bool clear)
        {
            if (clear)
            {
                _textDest.Children.Clear();
            }
        }

        public void PrepareDrawLine(LineContext lineContext)
        {
        }

        /// <inheritdoc />
        public void DrawLine(LineContext lineContext)
        {
            if (_currentDrawingContext != null)
            {
                lineContext.MyTextLine.Draw(_currentDrawingContext, lineContext.LineOriginPoint, InvertAxes.None);
            }
            else
            {
                var dv = new DrawingGroup();
                var drawingContext = dv.Open();
                lineContext.MyTextLine.Draw(drawingContext, lineContext.LineOriginPoint, InvertAxes.None);
                drawingContext.Close();
                _textDest.Children.Add(dv);
                // _children.Add(dv);
            }
        }

        /// <inheritdoc />
        public void EndDrawLines(LineContext lineContext)
        {
            if (_currentDrawingContext != null)
            {
                
            }
        }
    }
}