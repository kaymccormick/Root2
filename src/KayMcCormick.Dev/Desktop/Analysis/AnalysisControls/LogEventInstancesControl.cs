﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEventInstancesControl : Control
    {

        public static readonly DependencyProperty EventsSourceProperty = DependencyProperty.Register(
            "EventsSource", typeof(IEnumerable<LogEventInstance>), typeof(LogEventInstancesControl), new PropertyMetadata(default(IEnumerable<LogEventInstance>), OnEventsSourceUpdated));

        private static void OnEventsSourceUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LogEventInstancesControl c = (LogEventInstancesControl) d;
            var view = CollectionViewSource.GetDefaultView(e.NewValue);
            if (view != null)
            {
                view.CollectionChanged += c.ViewOnCollectionChanged;
            }

        }


        private void ViewOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex + e.NewItems.Count == EventsSource.Count())
                    {
                        if (Store != null)
                        {
                            ((LogEventsSource) Store).AppendRange(e.NewItems);
                            UpdateFormattedTextPartial();
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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
                Store,
                pixelsPerDip, lineInfos, null, ref maxX, out var maxY, TextLineAction, _myDrawingBrush,
                new Rect(0, 0, maxX, MaxY), _textFormatter, _lineContext, ParaProps);
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
                DebugUtils.WriteLine("Setting tabs");
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

        public IEnumerable<LogEventInstance> EventsSource
        {
            get { return (IEnumerable<LogEventInstance>) GetValue(EventsSourceProperty); }
            set { SetValue(EventsSourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected FontRendering CurrentRendering
        {
            get
            {

                if (_currentRendering == null)
                {
                    _currentRendering = new FontRendering(
                        EmSize,
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
        public LogEventsSource Store { get; private set; }

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

        static LogEventInstancesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogEventInstancesControl),
                new FrameworkPropertyMetadata(typeof(LogEventInstancesControl)));
        }

        /// <summary>
        /// 
        /// </summary>
        public LogEventInstancesControl()

        {
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        }

        public static readonly DependencyProperty DisplaySymbolProperty = DependencyProperty.Register(
            "DisplaySymbol", typeof(ISymbol), typeof(LogEventInstancesControl), new PropertyMetadata(default(ISymbol)));

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

            Store = new LogEventsSource(PixelsPerDip)
            {
                EmSize = EmSize,
                EventsSource = EventsSource
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
            _baseProps.SetFondRenderingEmSize(EmSize);
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedTabs"></param>
        protected void UpdateFormattedText(bool updatedTabs = false)
        {
            _updatingTabs = updatedTabs;
            DebugUtils.WriteLine(nameof(UpdateFormattedText));
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            if (Typeface == null) Typeface = new Typeface("Courier New");
            double pixelsPerDip = PixelsPerDip;
            var genericTextParagraphPropertiesImpl = ParaProps;
            
            _textDest.GuidelineSet = new GuidelineSet(null, Tabs.Select(x => x.Location).ToArray()).Clone();

            _lineContext = FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface, _textDest,
                Store,
                pixelsPerDip, LineInfos, Infos, ref maxX, out var maxY, TextLineAction, genericTextParagraphPropertiesImpl);

            if (!updatedTabs && _updatedTabs)
            {
                UpdateFormattedText(_updatedTabs);
                _updatedTabs = false;
                return;
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

        // ReSharper disable once NotAccessedField.Local

        // ReSharper disable once NotAccessedField.Local
    }
}