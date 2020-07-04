using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SymbolTextControl : Control, ILineDrawer
    {


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
        public AppTextSource Store { get; private set; }

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

        static SymbolTextControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SymbolTextControl),
                new FrameworkPropertyMetadata(typeof(SymbolTextControl)));
        }

        /// <summary>
        /// 
        /// </summary>
        public SymbolTextControl()

        {
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        }

        public static readonly DependencyProperty DisplaySymbolProperty = DependencyProperty.Register(
            "DisplaySymbol", typeof(ISymbol), typeof(SymbolTextControl), new PropertyMetadata(default(ISymbol)));

        public ISymbol DisplaySymbol
        {
            get { return (ISymbol) GetValue(DisplaySymbolProperty); }
            set { SetValue(DisplaySymbolProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void UpdateTextSource()
        {
            if (!UiLoaded)
                return;

            Store = new SymbolTextSource(PixelsPerDip)
            {
                EmSize = EmSize,
                Symbol = DisplaySymbol
            };
            Store.Init();
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

        public bool SingleLineMode { get; set; } = true;

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

        protected void UpdateFormattedText()
        {
            DebugUtils.WriteLine(nameof(UpdateFormattedText));
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            if (Typeface == null) Typeface = new Typeface("Courier New");
            double pixelsPerDip = PixelsPerDip;
            FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface, _textDest,
                Store,
                pixelsPerDip, LineInfos, Infos, ref maxX, out var maxY, null, new GenericTextParagraphProperties(currentRendering, pixelsPerDip), this);
            MaxX = maxX;
            MaxY = maxY;
            _rectangle.Width = maxX;
            _rectangle.Height = maxY;
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
        private FontRendering _currentRendering;

        // ReSharper disable once NotAccessedField.Local

        // ReSharper disable once NotAccessedField.Local
        /// <inheritdoc />
        public void PrepareDrawLines(LineContext lineContext, bool clear)
        {
        }

        /// <inheritdoc />
        public void PrepareDrawLine(LineContext lineContext)
        {
        }

        /// <inheritdoc />
        public void DrawLine(LineContext lineContext)
        {
        }

        /// <inheritdoc />
        public void EndDrawLines(LineContext lineContext)
        {
        }
    }
}