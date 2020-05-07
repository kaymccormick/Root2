using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeControl2 : FormattedTextControl
    {
        static CodeControl2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeControl2),
                new FrameworkPropertyMetadata(typeof(CodeControl2)));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateFormattedText()
        {
            DebugUtils.WriteLine(nameof(UpdateFormattedText));
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            if (Typeface == null) Typeface = new Typeface("Courier New");
            FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface, _textDest,
                Store,
                PixelsPerDip, LineInfos, Infos, ref maxX, out var maxY, null);
            MaxX = maxX;
            MaxY = maxY;
            _rectangle.Width = maxX;
            _rectangle.Height = maxY;
            InvalidateVisual();
            
        }

        private System.Windows.Shapes.Rectangle _rectangle;
        private ScrollViewer _scrollViewer;
        private DrawingBrush _myDrawingBrush;
        private DrawingGroup _textDest;

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            _scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");
            _rectangle = (System.Windows.Shapes.Rectangle) GetTemplateChild("Rectangle");
            OutputWidth = _rectangle.ActualWidth;
#if false
            var dpd = DependencyPropertyDescriptor.FromProperty(TextElement.FontSizeProperty, typeof(Rectangle));
            var dpd2 = DependencyPropertyDescriptor.FromProperty(TextElement.FontFamilyProperty, typeof(Rectangle));

            if (_rectangle != null)
            {
                dpd.AddValueChanged(_rectangle, Handler);
                dpd2.AddValueChanged(_rectangle, Handler2);
            }
#endif

            SetFontFamily();

            _myDrawingBrush = (DrawingBrush) GetTemplateChild("DrawingBrush");
            _textDest = (DrawingGroup) GetTemplateChild("TextDest");

            UiLoaded = true;
            EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            UpdateTextSource();
            UpdateFormattedText();
        }
    }
}