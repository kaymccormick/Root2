using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CodeRenderer : UIElement, ISupportInitialize, ILineDrawer
    {
        /// <summary>
        /// 
        /// </summary>
        public CodeRenderer()
        {
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            TypefaceManager = new DefaultTypefaceManager();

            AddHandler(CodeAnalysisProperties.CompilationChangedEvent,
                new RoutedPropertyChangedEventHandler<Compilation>(OnCompilationUpdated));
            AddHandler(CodeAnalysisProperties.SyntaxTreeChangedEvent,
                new RoutedPropertyChangedEventHandler<SyntaxTree>(OnSyntaxTreeUpdated));
        }

        private static void OnSyntaxTreeUpdated(object sender, RoutedPropertyChangedEventArgs<SyntaxTree> e)
        {
            ((CodeRenderer) sender).OnSyntaxTreeUpdated(e.OldValue, e.NewValue);
        }

        private static void OnCompilationUpdated(object sender, RoutedPropertyChangedEventArgs<Compilation> e)
        {
            ((CodeRenderer) sender).OnCompilationUpdated(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        // public static readonly DependencyProperty CompilationProperty =
        // CodeAnalysisProperties.CompilationProperty.AddOwner(typeof(CodeRenderer),
        // new PropertyMetadata(null, new PropertyChangedCallback(OnCompilationUpdated)));

        // public static readonly DependencyProperty SyntaxTreeProperty =
        // CodeAnalysisProperties.SyntaxTreeProperty.AddOwner(typeof(CodeRenderer),
        // new PropertyMetadata(null, new PropertyChangedCallback(OnSyntaxTreeUpdated)));
        private static void OnSyntaxTreeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CodeRenderer) d).OnSyntaxTreeUpdated((SyntaxTree) e.OldValue, (SyntaxTree) e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnSyntaxTreeUpdated(SyntaxTree oldValue, SyntaxTree newValue)
        {
            OnNodeUpdated();
        }

        private static void OnCompilationUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CodeRenderer) d).OnCompilationUpdated((Compilation) e.OldValue, (Compilation) e.NewValue);
        }

        private void OnCompilationUpdated(Compilation oldVal, Compilation newVal)
        {
        }

        /// <summary>
        /// 
        /// </summary>null, 
        public static readonly DependencyProperty FontFamilyProperty =
            TextElement.FontFamilyProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</returns>
        public static readonly DependencyProperty FontStyleProperty =
            TextElement.FontStyleProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</returns>
        public static readonly DependencyProperty FontWeightProperty =
            TextElement.FontWeightProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</returns>
        public static readonly DependencyProperty FontStretchProperty =
            TextElement.FontStretchProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</returns>
        public static readonly DependencyProperty FontSizeProperty =
            TextElement.FontSizeProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</returns>
        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</returns>
        public static readonly DependencyProperty BackgroundProperty =
            TextElement.BackgroundProperty.AddOwner(typeof(CodeRenderer),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property.</returns>
        public static readonly DependencyProperty TextDecorationsProperty =
            Inline.TextDecorationsProperty.AddOwner(typeof(CodeRenderer),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) new TextDecorationCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</returns>
        public static readonly DependencyProperty TextEffectsProperty =
            TextElement.TextEffectsProperty.AddOwner(typeof(CodeRenderer),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) new TextEffectCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property.</returns>
        public static readonly DependencyProperty LineHeightProperty =
            Block.LineHeightProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</returns>
        public static readonly DependencyProperty LineStackingStrategyProperty =
            Block.LineStackingStrategyProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property.</returns>
        public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(CodeRenderer),
            (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</returns>
        public static readonly DependencyProperty TextAlignmentProperty =
            Block.TextAlignmentProperty.AddOwner(typeof(CodeRenderer));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property. </summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property.</returns>
        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            nameof(TextTrimming), typeof(TextTrimming), typeof(CodeRenderer),
            (PropertyMetadata) new FrameworkPropertyMetadata((object) TextTrimming.None,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
            new ValidateValueCallback(IsValidTextTrimming));

        private static bool IsValidTextTrimming(object value)
        {
            var textTrimming = (TextTrimming) value;
            switch (textTrimming)
            {
                case TextTrimming.None:
                case TextTrimming.CharacterEllipsis:
                    return true;
                default:
                    return textTrimming == TextTrimming.WordEllipsis;
            }
        }

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextWrapping" /> dependency property. </summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextWrapping" /> dependency property.</returns>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            nameof(TextWrapping), typeof(TextWrapping), typeof(CodeRenderer),
            (PropertyMetadata) new FrameworkPropertyMetadata((object) TextWrapping.NoWrap,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
            new ValidateValueCallback(IsValidTextWrap));

        private static bool IsValidTextWrap(object value)
        {
            var textWrapping = (TextWrapping) value;
            switch (textWrapping)
            {
                case TextWrapping.NoWrap:
                case TextWrapping.Wrap:
                    return true;
                default:
                    return textWrapping == TextWrapping.WrapWithOverflow;
            }
        }

        [Localizability(LocalizationCategory.Font)]
        public FontFamily FontFamily
        {
            get { return (FontFamily) GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetFontFamily(DependencyObject element, FontFamily value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(FontFamilyProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="F:System.Windows.Controls.TextBlock.FontFamilyProperty" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontFamily" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static FontFamily GetFontFamily(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (FontFamily) element.GetValue(FontFamilyProperty);
        }

        /// <summary>Gets or sets the top-level font style for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>A member of the <see cref="T:System.Windows.FontStyles" /> class specifying the desired font style. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> value.</returns>
        public FontStyle FontStyle
        {
            get { return (FontStyle) GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetFontStyle(DependencyObject element, FontStyle value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(FontStyleProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static FontStyle GetFontStyle(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (FontStyle) element.GetValue(FontStyleProperty);
        }

        /// <summary>Gets or sets the top-level font weight for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>A member of the <see cref="T:System.Windows.FontWeights" /> class specifying the desired font weight. The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> value.</returns>
        public FontWeight FontWeight
        {
            get { return (FontWeight) GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetFontWeight(DependencyObject element, FontWeight value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(FontWeightProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static FontWeight GetFontWeight(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (FontWeight) element.GetValue(FontWeightProperty);
        }

        /// <summary>Gets or sets the top-level font-stretching characteristics for the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>A member of the <see cref="T:System.Windows.FontStretch" /> class specifying the desired font-stretching characteristics to use. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
        public FontStretch FontStretch
        {
            get { return (FontStretch) GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetFontStretch(DependencyObject element, FontStretch value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(FontStretchProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static FontStretch GetFontStretch(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (FontStretch) element.GetValue(FontStretchProperty);
        }

        /// <summary>Gets or sets the top-level font size for the <see cref="T:System.Windows.Controls.TextBlock" />.   </summary>
        /// <returns>The desired font size to use in device independent pixels). The default is determined by the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> value.</returns>
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSize
        {
            get { return (double) GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetFontSize(DependencyObject element, double value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(FontSizeProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        [TypeConverter(typeof(FontSizeConverter))]
        public static double GetFontSize(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (double) element.GetValue(FontSizeProperty);
        }

        /// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> to apply to the text contents of the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>The brush used to apply to the text contents. The default is <see cref="P:System.Windows.Media.Brushes.Black" />.</returns>
        public Brush Foreground
        {
            get { return (Brush) GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetForeground(DependencyObject element, Brush value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(ForegroundProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static Brush GetForeground(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (Brush) element.GetValue(ForegroundProperty);
        }

        /// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to fill the background of content area.  </summary>
        /// <returns>The brush used to fill the background of the content area, or <see langword="null" /> to not use a background brush. The default is <see langword="null" />.</returns>
        public Brush Background
        {
            get { return (Brush) GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, (object) value); }
        }

        /// <summary>Gets or sets a <see cref="T:System.Windows.TextDecorationCollection" /> that contains the effects to apply to the text of a <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>A <see cref="T:System.Windows.TextDecorationCollection" /> collection that contains text decorations to apply to this element. The default is <see langword="null" /> (no text decorations applied).</returns>
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection) GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, (object) value); }
        }

        /// <summary>Gets or sets the effects to apply to the text content in this element.  </summary>
        /// <returns>A <see cref="T:System.Windows.Media.TextEffectCollection" /> containing one or more <see cref="T:System.Windows.Media.TextEffect" /> objects that define effects to apply to the text of the <see cref="T:System.Windows.Controls.TextBlock" />. The default is <see langword="null" /> (no effects applied).</returns>
        public TextEffectCollection TextEffects
        {
            get { return (TextEffectCollection) GetValue(TextEffectsProperty); }
            set { SetValue(TextEffectsProperty, (object) value); }
        }

        /// <summary>Gets or sets the height of each line of content.  </summary>
        /// <returns>The height of line, in device independent pixels, in the range of 0.0034 to 160000. A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") indicates that the line height is determined automatically from the current font characteristics. The default is <see cref="F:System.Double.NaN" />.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> is set to a non-positive value.</exception>
        [TypeConverter(typeof(LengthConverter))]
        public double LineHeight
        {
            get { return (double) GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <see cref="P:System.Windows.Controls.TextBlock.LineHeight" />is set to a non-positive value.</exception>
        public static void SetLineHeight(DependencyObject element, double value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(LineHeightProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        [TypeConverter(typeof(LengthConverter))]
        public static double GetLineHeight(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (double) element.GetValue(LineHeightProperty);
        }

        /// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>The mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Controls.TextBlock" />. The default is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy) GetValue(LineStackingStrategyProperty); }
            set { SetValue(LineStackingStrategyProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(LineStackingStrategyProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static LineStackingStrategy GetLineStackingStrategy(
            DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (LineStackingStrategy) element.GetValue(LineStackingStrategyProperty);
        }

        /// <summary>Gets or sets a value that indicates the thickness of padding space between the boundaries of the content area, and the content displayed by a <see cref="T:System.Windows.Controls.TextBlock" />.  </summary>
        /// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of padding to apply, in device independent pixels. The default is <see cref="F:System.Double.NaN" />.</returns>
        public Thickness Padding
        {
            get { return (Thickness) GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, (object) value); }
        }

        /// <summary>Gets or sets a value that indicates the horizontal alignment of text content.  </summary>
        /// <returns>One of the <see cref="T:System.Windows.TextAlignment" /> values that specifies the desired alignment. The default is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment) GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, (object) value); }
        }

        /// <summary>Sets the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property on a specified dependency object.</summary>
        /// <param name="element">The dependency object on which to set the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> property.</param>
        /// <param name="value">The new value to set the property to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static void SetTextAlignment(DependencyObject element, TextAlignment value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.SetValue(TextAlignmentProperty, (object) value);
        }

        /// <summary>Returns the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property for a specified dependency object.</summary>
        /// <param name="element">The dependency object from which to retrieve the value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property.</param>
        /// <returns>The current value of the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> attached property on the specified dependency object.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element" /> is <see langword="null" />.</exception>
        public static TextAlignment GetTextAlignment(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            return (TextAlignment) element.GetValue(TextAlignmentProperty);
        }

        /// <summary>Gets or sets the text trimming behavior to employ when content overflows the content area.  </summary>
        /// <returns>One of the <see cref="T:System.Windows.TextTrimming" /> values that specifies the text trimming behavior to employ. The default is <see cref="F:System.Windows.TextTrimming.None" />.</returns>
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming) GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, (object) value); }
        }

        /// <summary>Gets or sets how the <see cref="T:System.Windows.Controls.TextBlock" /> should wrap text.  </summary>
        /// <returns>One of the <see cref="T:System.Windows.TextWrapping" /> values. The default is <see cref="F:System.Windows.TextWrapping.NoWrap" />.</returns>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping) GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, (object) value); }
        }

        public DrawingGroup TextualDestination { get; set; } = new DrawingGroup();
        private Point _pos;
        private GeometryDrawing _geometryDrawing;
        private Rect _rect;
        public readonly List<List<char>> _chars = new List<List<char>>();
        public GenericTextRunProperties BaseProps => _store.BaseProps;
        private ITypefaceManager _typefaceManager;
        private ErrorsTextSource _errorTextSource;
        public Rectangle _rect2;
        private DrawingGroup _dg2;
        private FontRendering _currentRendering;
        private ICustomTextSource _store;
        private DocumentPaginator _documentPaginator;
        private static Type type;
        private DrawingContext _dc;
        private DrawingContext _currentDrawingContext;


        private SyntaxTree SyntaxTree
        {
            get => CodeAnalysisProperties.GetSyntaxTree(this);
            set => CodeAnalysisProperties.SetSyntaxTree(this, value);
        }

        private Compilation Compilation
        {
            get => CodeAnalysisProperties.GetCompilation(this);
            set => CodeAnalysisProperties.SetCompilation(this, value);
        }
        /// <summary>
        /// 
        /// </summary>
        // public SyntaxTree SyntaxTree
        // {
        // get { return (SyntaxTree) GetValue(SyntaxTreeProperty); }
        // set { SetValue(SyntaxTreeProperty, value); }
        // }
        /// <summary>
        /// 
        /// </summary>
        private FontRendering CurrentRendering
        {
            get { return _currentRendering ?? (_currentRendering = SetupCurrentRendering()); }
            set { _currentRendering = value; }
        }

        private FontRendering SetupCurrentRendering()
        {
            return FontRendering.CreateInstance(FontSize,
                TextAlignment,
                TextDecorations,
                Foreground,
                Typeface);
        }

        /// <summary>
        /// 
        /// </summary>
        private ICustomTextSource TextSource
        {
            get { return _store ?? SetupTextSource(); }
            set { _store = value; }
        }

        private ICustomTextSource SetupTextSource()
        {
            return _store = TextSourceService.CreateAndInitTextSource(ErrorsList, FontSize, PixelsPerDip,
                TypefaceManager, SyntaxNode,
                CodeAnalysisProperties.GetSyntaxTree(this),
                (CSharpCompilation)CodeAnalysisProperties.GetCompilation(this));
        }

        public SyntaxNode SyntaxNode { get; set; }

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
        private List<CompilationError> ErrorsList { get; } = new List<CompilationError>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CompilationError> Errors
        {
            get { return ErrorsList; }
        }


        /// <summary>
        /// 
        /// </summary>
        private double PixelsPerDip { get; }

        /// <summary>
        /// 
        /// </summary>
        private double OutputWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<LineInfo> LineInfos { get; } = new ObservableCollection<LineInfo>();

        /// <summary>
        /// 
        /// </summary>
        private List<RegionInfo> RegionInfoList { get; } = new List<RegionInfo>();

        /// <summary>
        /// 
        /// </summary>
        private Typeface Typeface { get; set; }

        private ITypefaceManager TypefaceManager
        {
            get { return _typefaceManager; }
            set { _typefaceManager = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void UpdateTextSource()
        {
            if (Compilation != null &&
                Compilation.SyntaxTrees.Contains(SyntaxTree) == false)
            {
                Compilation = null;
                DebugUtils.WriteLine("Compilation does not contain syntax tree.");
            }

            if (SyntaxTree == null) return;
            DebugUtils.WriteLine("Creating new " + nameof(SyntaxNodeCustomTextSource));

            SetupTextSource();
            _errorTextSource =
                ErrorsList.Any() ? new ErrorsTextSource(PixelsPerDip, ErrorsList, TypefaceManager) : null;
            UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawingContext"></param>
        public void UpdateFormattedText(DrawingContext drawingContext = null)
        {
            DebugUtils.WriteLine(nameof(UpdateFormattedText));
            var currentRendering = CurrentRendering;
            var maxX = MaxX;
            _currentDrawingContext = drawingContext;

            var pixelsPerDip = PixelsPerDip;
            FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, FontSize, Typeface,
                TextualDestination,
                (AppTextSource) TextSource,
                pixelsPerDip, LineInfos, RegionInfoList, ref maxX, out var maxY, null,
                new GenericTextParagraphProperties(currentRendering, pixelsPerDip), this);
            MaxX = maxX;
            MaxY = maxY;

            var brush = new DrawingBrush(TextualDestination);

//           _rectangle.Width = maxX;
//           _rectangle.Height = maxY;
            //InvalidateVisual();
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
                UpdateFormattedText();
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
                    if (diagnosticLocation.SourceTree == SyntaxTree)
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

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            Typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch,
                new FontFamily("GlobalMonospace.CompositeFont"));
        }

        public void PrepareDrawLines(LineContext lineContext)
        {
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
                var dv = new DrawingVisual();
                var drawingContext = dv.RenderOpen();
                lineContext.MyTextLine.Draw(drawingContext, lineContext.LineOriginPoint, InvertAxes.None);
                drawingContext.Close();
                // _children.Add(dv);
            }
        }

        public void EndDrawLines(LineContext lineContext)
        {
            if (_currentDrawingContext != null)
            {
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateFormattedText(drawingContext);
            //drawingContext.Close();
        }
    }
}