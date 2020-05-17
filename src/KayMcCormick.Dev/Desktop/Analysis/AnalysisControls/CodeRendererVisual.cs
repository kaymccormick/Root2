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
    public class CodeRendererVisual : UIElement, ISupportInitialize, ILineDrawer
    {
        public CodeRendererVisual()
        {
            _children = new VisualCollection(this);
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            TypefaceManager = new DefaultTypefaceManager();

            AddHandler(CodeAnalysisProperties.CompilationChangedEvent, new RoutedPropertyChangedEventHandler<Compilation>(OnCompilationUpdated));
            AddHandler(CodeAnalysisProperties.SyntaxTreeChangedEvent, new RoutedPropertyChangedEventHandler<SyntaxTree>(OnSyntaxTreeUpdated));
        }

        private static void OnSyntaxTreeUpdated(object sender, RoutedPropertyChangedEventArgs<SyntaxTree> e)
        {
            ((CodeRendererVisual) sender).OnSyntaxTreeUpdated(e.OldValue, e.NewValue);
        }

        private static void OnCompilationUpdated(object sender, RoutedPropertyChangedEventArgs<Compilation> e)
        {
            ((CodeRendererVisual)sender).OnCompilationUpdated(e.OldValue, e.NewValue);
        }

        private VisualCollection _children;
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
            ((CodeRendererVisual) d).OnSyntaxTreeUpdated((SyntaxTree) e.OldValue, (SyntaxTree) e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSyntaxTreeUpdated(SyntaxTree oldValue, SyntaxTree newValue)
        {
            OnNodeUpdated();
        }

        private static void OnCompilationUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CodeRendererVisual) d).OnCompilationUpdated((Compilation) e.OldValue, (Compilation) e.NewValue);
        }

        private void OnCompilationUpdated(Compilation oldVal, Compilation newVal)
        {
        }

        /// <summary>
        /// 
        /// </summary>null, 
        public static readonly DependencyProperty FontFamilyProperty =
            TextElement.FontFamilyProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStyle" /> dependency property.</returns>
        public static readonly DependencyProperty FontStyleProperty =
            TextElement.FontStyleProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontWeight" /> dependency property.</returns>
        public static readonly DependencyProperty FontWeightProperty =
            TextElement.FontWeightProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontStretch" /> dependency property.</returns>
        public static readonly DependencyProperty FontStretchProperty =
            TextElement.FontStretchProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.FontSize" /> dependency property.</returns>
        public static readonly DependencyProperty FontSizeProperty =
            TextElement.FontSizeProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Foreground" /> dependency property.</returns>
        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Background" /> dependency property.</returns>
        public static readonly DependencyProperty BackgroundProperty =
            TextElement.BackgroundProperty.AddOwner(typeof(CodeRendererVisual),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextDecorations" /> dependency property.</returns>
        public static readonly DependencyProperty TextDecorationsProperty =
            Inline.TextDecorationsProperty.AddOwner(typeof(CodeRendererVisual),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) new TextDecorationCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextEffects" /> dependency property.</returns>
        public static readonly DependencyProperty TextEffectsProperty =
            TextElement.TextEffectsProperty.AddOwner(typeof(CodeRendererVisual),
                (PropertyMetadata) new FrameworkPropertyMetadata((object) new TextEffectCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> dependency property.</returns>
        public static readonly DependencyProperty LineHeightProperty =
            Block.LineHeightProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.LineStackingStrategy" /> dependency property.</returns>
        public static readonly DependencyProperty LineStackingStrategyProperty =
            Block.LineStackingStrategyProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.Padding" /> dependency property.</returns>
        public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(CodeRendererVisual),
            (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.TextBlock.TextAlignment" /> dependency property.</returns>
        public static readonly DependencyProperty TextAlignmentProperty =
            Block.TextAlignmentProperty.AddOwner(typeof(CodeRendererVisual));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property. </summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.TextBlock.TextTrimming" /> dependency property.</returns>
        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            nameof(TextTrimming), typeof(TextTrimming), typeof(CodeRendererVisual),
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
            nameof(TextWrapping), typeof(TextWrapping), typeof(CodeRendererVisual),
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

        public DrawingBrush _myDrawingBrush = new DrawingBrush();
        public DrawingGroup TextualDestination { get; set; } = new DrawingGroup();
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
        private static Type type;
        private DrawingContext _dc;
        private DrawingContext _currentDrawingContext;

        /// <summary>
        /// 
        /// </summary>
        // public SyntaxTree SyntaxTree
        // {
        // get { return (SyntaxTree) GetValue(SyntaxTreeProperty); }
        // set { SetValue(SyntaxTreeProperty, value); }
        // }

        public SyntaxTree SyntaxTree
        {
            get { return CodeAnalysisProperties.GetSyntaxTree(this); }
            set { CodeAnalysisProperties.SetSyntaxTree(this, value);}
        }
        public Compilation Compilation
        {
            get { return CodeAnalysisProperties.GetCompilation(this); }
            set { CodeAnalysisProperties.SetCompilation(this, value); }
        }
        static CodeRendererVisual()
        {
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
                return _currentRendering ?? (_currentRendering = FontRendering.CreateInstance(EmSize,
                    TextAlignment.Left,
                    null,
                    Brushes.Black,
                    Typeface));
            }
            set { _currentRendering = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomTextSource TextSource
        {
            get
            {
                return _store ?? (_store = TextSourceService.CreateAndInitTextSource(ErrorsList, EmSize, PixelsPerDip,
                    TypefaceManager, SyntaxNode, SyntaxTree,
                    (CSharpCompilation) Compilation));
            }
            private set { _store = value; }
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
        public TextFormatter Formatter { get; } = TextFormatter.Create();

        /// <summary>
        /// 
        /// </summary>
        public List<CompilationError> ErrorsList { get; } = new List<CompilationError>();

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
        public double PixelsPerDip { get; }

        /// <summary>
        /// 
        /// </summary>
        public double EmSize
        {
            get { return FontSize; }
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
            if (Compilation != null &&
                Compilation.SyntaxTrees.Contains(SyntaxTree) == false)
                // _formattedTextControl.Compilation = null;
                DebugUtils.WriteLine("Compilation does not contain syntax tree.");

            if (SyntaxNode == null || SyntaxTree == null) return;
            if (ReferenceEquals(SyntaxNode.SyntaxTree, SyntaxTree) == false)
                throw new InvalidOperationException("Node is not within syntax tree");
            DebugUtils.WriteLine("Creating new " + nameof(SyntaxNodeCustomTextSource));

            TextSource = TextSourceService.CreateAndInitTextSource(ErrorsList, EmSize, PixelsPerDip, TypefaceManager,
                SyntaxNode, SyntaxTree, (CSharpCompilation) Compilation);
            _errorTextSource =
                ErrorsList.Any() ? new ErrorsTextSource(PixelsPerDip, ErrorsList, TypefaceManager) : null;
            _baseProps = TextSource.BaseProps;
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
            FormattingHelper.UpdateFormattedText(OutputWidth, ref currentRendering, EmSize, Typeface,
                TextualDestination,
                (AppTextSource) TextSource,
                pixelsPerDip, LineInfos, RegionInfoList, ref maxX, out var maxY, null,
                new GenericTextParagraphProperties(currentRendering, pixelsPerDip), this);
            MaxX = maxX;
            MaxY = maxY;

            DrawingBrush brush = new DrawingBrush(TextualDestination);
            
//           _rectangle.Width = maxX;
//           _rectangle.Height = maxY;
            //InvalidateVisual();
        }



#if false
        public void UpdateFormattedText_()
        {
            if (CurrentRendering == null)
                CurrentRendering = new FontRendering(
                    EmSize,
                    TextAlignment.Left,
                    null,
                    Brushes.Black,
                    Typeface);

            var textStorePosition = 0;
            var linePosition = new Point(0, 0);

            // Create a DrawingGroup object for storing formatted text.

            var dc = TextualDestination.Open();

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
                                //                                TextualDestination.Children.Add(new GeometryDrawing(null, new Pen(Brushes.DarkOrange, 2), new RectangleGeometry(bounds)));
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

//            Rectangle.Width = MaxX;
//            Rectangle.Height = _pos.Y;
MaxY = _pos.Y;

            InvalidateVisual();
        }
#endif

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

#if false
        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount => _children.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
#endif
        public void PrepareDrawLines(LineContext lineContext)
        {
            _children.Clear();
            
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
                DrawingVisual dv = new DrawingVisual();
                var drawingContext = dv.RenderOpen();
                lineContext.MyTextLine.Draw(drawingContext, lineContext.LineOriginPoint, InvertAxes.None);
                drawingContext.Close();
                _children.Add(dv);
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