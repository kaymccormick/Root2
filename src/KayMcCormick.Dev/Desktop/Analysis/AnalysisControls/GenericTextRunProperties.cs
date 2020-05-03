using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// Class used to implement TextRunProperties
    /// </summary>
    class GenericTextRunProperties : TextRunProperties
    {
        #region Constructors

        
        public GenericTextRunProperties(
            Typeface typeface,
            double pixelsPerDip,
            double size,
            double hintingSize,
            TextDecorationCollection textDecorations,
            Brush forgroundBrush,
            Brush backgroundBrush,
            BaselineAlignment baselineAlignment,
            CultureInfo culture)
        {
            if (typeface == null)
                throw new ArgumentNullException("typeface");

            ValidateCulture(culture);

            PixelsPerDip = pixelsPerDip;
            _typeface = typeface;
            _emSize = size;
            _emHintingSize = hintingSize;
            _textDecorations = textDecorations;
            _foregroundBrush = forgroundBrush;
            _backgroundBrush = backgroundBrush;
            _baselineAlignment = baselineAlignment;
            _culture = culture;
        }

        public GenericTextRunProperties(FontRendering newRender, double pixelsPerDip)
        {
            _typeface = newRender.Typeface;
            _emSize = newRender.FontSize;
            _emHintingSize = newRender.FontSize;
            _textDecorations = newRender.TextDecorations;
            _foregroundBrush = newRender.TextColor;
            _backgroundBrush = null;
            _baselineAlignment = BaselineAlignment.Baseline;
            _culture = CultureInfo.CurrentUICulture;
            PixelsPerDip = pixelsPerDip;
        }

        #endregion

        #region Private Methods

        private static void ValidateCulture(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (culture.IsNeutralCulture || culture.Equals(CultureInfo.InvariantCulture))
                throw new ArgumentException("Specific Culture Required", "culture");
        }

        private static void ValidateFontSize(double emSize)
        {
            if (emSize <= 0)
                throw new ArgumentOutOfRangeException("emSize", "Parameter Must Be Greater Than Zero.");
            //if (emSize > MaxFontEmSize)
            //   throw new ArgumentOutOfRangeException("emSize", "Parameter Is Too Large.");
            if (double.IsNaN(emSize))
                throw new ArgumentOutOfRangeException("emSize", "Parameter Cannot Be NaN.");
        }

        #endregion

        #region Properties

        public override Typeface Typeface
        {
            get { return _typeface; }
        }

        public override double FontRenderingEmSize
        {
            get { return _emSize; }
        }

        public override double FontHintingEmSize
        {
            get { return _emHintingSize; }
        }

        public override TextDecorationCollection TextDecorations
        {
            get { return _textDecorations; }
        }

        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush; }
        }

        public override Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }

        public override BaselineAlignment BaselineAlignment
        {
            get { return _baselineAlignment; }
        }

        public override CultureInfo CultureInfo
        {
            get { return _culture; }
        }

        public override TextRunTypographyProperties TypographyProperties
        {
            get { return null; }
        }

        public override TextEffectCollection TextEffects
        {
            get { return null; }
        }

        public override NumberSubstitution NumberSubstitution
        {
            get { return null; }
        }

        public SymbolDisplayPart SymbolDisplaYPart { get; set; }
        public ITypeSymbol TypeSymbol { get; set; }
        public SyntaxToken SyntaxToken { get; set; }
        public SyntaxTrivia SyntaxTrivia { get; set; }
        public string Text { get; set; }

        #endregion

        #region Private Fields

        private Typeface _typeface;
        private double _emSize;
        private double _emHintingSize;
        private TextDecorationCollection _textDecorations;
        private Brush _foregroundBrush;
        private Brush _backgroundBrush;
        private BaselineAlignment _baselineAlignment;
        private CultureInfo _culture;

        #endregion
    }
}