using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicTextRunProperties : TextRunProperties
    {
        private readonly TextRunProperties _baseProps;
        private Brush _backgroundBrush;
        private Brush _foregroundBrush;
        private FontStyle _fontStyle;
        private Typeface _typeface;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseProps"></param>
        public BasicTextRunProperties(TextRunProperties baseProps)
        {
            _baseProps = baseProps;
        }

        /// <inheritdoc />
        public override Typeface Typeface
        {
            get { return _typeface ?? _baseProps.Typeface; }
        }

        /// <inheritdoc />
        public override double FontRenderingEmSize
        {
            get { return _baseProps.FontRenderingEmSize; }
        }

        /// <inheritdoc />
        public override double FontHintingEmSize
        {
            get { return _baseProps.FontHintingEmSize; }
        }

        /// <inheritdoc />
        public override TextDecorationCollection TextDecorations
        {
            get { return _baseProps.TextDecorations; }
        }

        /// <inheritdoc />
        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush ?? _baseProps.ForegroundBrush; }
        }

        /// <inheritdoc />
        public override Brush BackgroundBrush
        {
            get { return _backgroundBrush ?? _baseProps.BackgroundBrush; }
        }

        public void SetBackgroundBrush(Brush backgroundBrush)
        {
            _backgroundBrush = backgroundBrush;
        }

        public void SetForegroundBrush(Brush foregroundBrush)
        {
            _foregroundBrush = foregroundBrush;
        }

        public override CultureInfo CultureInfo
        {
            get { return _baseProps.CultureInfo; }
        }

        public override TextEffectCollection TextEffects
        {
            get { return _baseProps.TextEffects; }
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
            _fontStyle = fontStyle;
            var family = _baseProps.Typeface.FontFamily;
            _typeface = new Typeface(family, _fontStyle, _baseProps.Typeface.Weight, _baseProps.Typeface.Stretch);

        }

        public TextRunProperties WithForegroundBrush(Brush fg)
        {
            _foregroundBrush = fg;
            return this;
        }
    }
}