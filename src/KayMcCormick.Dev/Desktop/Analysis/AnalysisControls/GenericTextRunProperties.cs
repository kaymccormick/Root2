using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    public class AppTextRunTypographyProperties : TextRunTypographyProperties
    {
        public AppTextRunTypographyProperties(bool standardLigatures, bool contextualLigatures, bool discretionaryLigatures, bool historicalLigatures, bool contextualAlternates, bool historicalForms, bool kerning, bool capitalSpacing, bool caseSensitiveForms, bool stylisticSet1, bool stylisticSet2, bool stylisticSet3, bool stylisticSet4, bool stylisticSet5, bool stylisticSet6, bool stylisticSet7, bool stylisticSet8, bool stylisticSet9, bool stylisticSet10, bool stylisticSet11, bool stylisticSet12, bool stylisticSet13, bool stylisticSet14, bool stylisticSet15, bool stylisticSet16, bool stylisticSet17, bool stylisticSet18, bool stylisticSet19, bool stylisticSet20, bool slashedZero, bool mathematicalGreek, bool eastAsianExpertForms, FontVariants variants, FontCapitals capitals, FontFraction fraction, FontNumeralStyle numeralStyle, FontNumeralAlignment numeralAlignment, FontEastAsianWidths eastAsianWidths, FontEastAsianLanguage eastAsianLanguage, int standardSwashes, int contextualSwashes, int stylisticAlternates, int annotationAlternates)
        {
            StandardLigatures = standardLigatures;
            ContextualLigatures = contextualLigatures;
            DiscretionaryLigatures = discretionaryLigatures;
            HistoricalLigatures = historicalLigatures;
            ContextualAlternates = contextualAlternates;
            HistoricalForms = historicalForms;
            Kerning = kerning;
            CapitalSpacing = capitalSpacing;
            CaseSensitiveForms = caseSensitiveForms;
            StylisticSet1 = stylisticSet1;
            StylisticSet2 = stylisticSet2;
            StylisticSet3 = stylisticSet3;
            StylisticSet4 = stylisticSet4;
            StylisticSet5 = stylisticSet5;
            StylisticSet6 = stylisticSet6;
            StylisticSet7 = stylisticSet7;
            StylisticSet8 = stylisticSet8;
            StylisticSet9 = stylisticSet9;
            StylisticSet10 = stylisticSet10;
            StylisticSet11 = stylisticSet11;
            StylisticSet12 = stylisticSet12;
            StylisticSet13 = stylisticSet13;
            StylisticSet14 = stylisticSet14;
            StylisticSet15 = stylisticSet15;
            StylisticSet16 = stylisticSet16;
            StylisticSet17 = stylisticSet17;
            StylisticSet18 = stylisticSet18;
            StylisticSet19 = stylisticSet19;
            StylisticSet20 = stylisticSet20;
            SlashedZero = slashedZero;
            MathematicalGreek = mathematicalGreek;
            EastAsianExpertForms = eastAsianExpertForms;
            Variants = variants;
            Capitals = capitals;
            Fraction = fraction;
            NumeralStyle = numeralStyle;
            NumeralAlignment = numeralAlignment;
            EastAsianWidths = eastAsianWidths;
            EastAsianLanguage = eastAsianLanguage;
            StandardSwashes = standardSwashes;
            ContextualSwashes = contextualSwashes;
            StylisticAlternates = stylisticAlternates;
            AnnotationAlternates = annotationAlternates;
        }

        public override bool StandardLigatures { get; }
        public override bool ContextualLigatures { get; }
        public override bool DiscretionaryLigatures { get; }
        public override bool HistoricalLigatures { get; }
        public override bool ContextualAlternates { get; }
        public override bool HistoricalForms { get; }
        public override bool Kerning { get; }
        public override bool CapitalSpacing { get; }
        public override bool CaseSensitiveForms { get; }
        public override bool StylisticSet1 { get; }
        public override bool StylisticSet2 { get; }
        public override bool StylisticSet3 { get; }
        public override bool StylisticSet4 { get; }
        public override bool StylisticSet5 { get; }
        public override bool StylisticSet6 { get; }
        public override bool StylisticSet7 { get; }
        public override bool StylisticSet8 { get; }
        public override bool StylisticSet9 { get; }
        public override bool StylisticSet10 { get; }
        public override bool StylisticSet11 { get; }
        public override bool StylisticSet12 { get; }
        public override bool StylisticSet13 { get; }
        public override bool StylisticSet14 { get; }
        public override bool StylisticSet15 { get; }
        public override bool StylisticSet16 { get; }
        public override bool StylisticSet17 { get; }
        public override bool StylisticSet18 { get; }
        public override bool StylisticSet19 { get; }
        public override bool StylisticSet20 { get; }
        public override bool SlashedZero { get; }
        public override bool MathematicalGreek { get; }
        public override bool EastAsianExpertForms { get; }
        public override FontVariants Variants { get; }
        public override FontCapitals Capitals { get; }
        public override FontFraction Fraction { get; }
        public override FontNumeralStyle NumeralStyle { get; }
        public override FontNumeralAlignment NumeralAlignment { get; }
        public override FontEastAsianWidths EastAsianWidths { get; }
        public override FontEastAsianLanguage EastAsianLanguage { get; }
        public override int StandardSwashes { get; }
        public override int ContextualSwashes { get; }
        public override int StylisticAlternates { get; }
        public override int AnnotationAlternates { get; }
    }
    public class MyTextRunProperties : TextRunProperties
    {
        private TextRunProperties _baseProps;
        private Brush _backgroundBrush = null;
        private Brush _foregroundBrush;
        private FontStyle _fontStyle;
        private Typeface _typeface;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseProps"></param>
        public MyTextRunProperties(TextRunProperties baseProps)
        {
            _baseProps = baseProps;
        }

        /// <inheritdoc />
        public override Typeface Typeface
        {
            get { return _typeface ?? _baseProps.Typeface; }
        }

        public override double FontRenderingEmSize
        {
            get { return _baseProps.FontRenderingEmSize; }
        }

        public override double FontHintingEmSize
        {
            get { return _baseProps.FontHintingEmSize; }
        }

        public override TextDecorationCollection TextDecorations
        {
            get { return _baseProps.TextDecorations; }
        }

        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush ?? _baseProps.ForegroundBrush; }
        }

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
    /// <summary>
    /// Class used to implement TextRunProperties
    /// </summary>
    public class GenericTextRunProperties : TextRunProperties
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

        public void SetFondRenderingEmSize(double emSize)
        {
            _emSize = emSize;
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

        // public SymbolDisplayPart SymbolDisplaYPart { get; set; }
        // public ITypeSymbol TypeSymbol { get; set; }
        // public SyntaxToken SyntaxToken { get; set; }
        // public SyntaxTrivia SyntaxTrivia { get; set; }
        // public string Text { get; set; }

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