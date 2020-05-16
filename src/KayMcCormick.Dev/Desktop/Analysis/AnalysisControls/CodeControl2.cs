using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [TitleMetadata("Code Control 2")]
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
            if (Typeface == null) Typeface = new Typeface("Courier New");
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            
            var pixelsPerDip = PixelsPerDip;
            FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface, _textDest,
                TextSource,
                pixelsPerDip, LineInfos, RegionInfoList, ref maxX, out var maxY, null, new GenericTextParagraphProperties(currentRendering, pixelsPerDip));
            MaxX = maxX;
            MaxY = maxY;
            _rectangle.Width = maxX;
            _rectangle.Height = maxY;
            InvalidateVisual();
            
        }

        /// <summary>
        /// 
        /// </summary>
        public List<RegionInfo> RegionInfoList { get; set; }

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>

        private System.Windows.Shapes.Rectangle _rectangle;
        private DrawingGroup _textDest;

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            _rectangle = (System.Windows.Shapes.Rectangle) GetTemplateChild("Rectangle");
            if (_rectangle != null)
            {
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

                _textDest = (DrawingGroup) GetTemplateChild("TextDest");

                UiLoaded = true;
                EmSize = (double) _rectangle.GetValue(TextElement.FontSizeProperty);
            }

            UpdateTextSource();
            UpdateFormattedText();
        }
    }
}