using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultTypefaceManager : DependencyObject, ITypefaceManager
    {

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.BaselineOffset" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.BaselineOffset" /> dependency property.</returns>
        public static readonly DependencyProperty BaselineOffsetProperty = DependencyProperty.RegisterAttached(nameof(BaselineOffset), typeof(double), typeof(DefaultTypefaceManager), (PropertyMetadata)new FrameworkPropertyMetadata((object)double.NaN, new PropertyChangedCallback(OnBaselineOffsetChanged)));

        private static void OnBaselineOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontStyle" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontStyle" /> dependency property.</returns>
        
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontWeight" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontWeight" /> dependency property.</returns>
        
        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontStretch" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontStretch" /> dependency property.</returns>
        
        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontSize" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.FontSize" /> dependency property.</returns>
        
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Foreground" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Foreground" /> dependency property.</returns>
        
        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Background" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Background" /> dependency property.</returns>
        
        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(DefaultTypefaceManager), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextDecorations" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextDecorations" /> dependency property.</returns>
        
        public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(DefaultTypefaceManager), (PropertyMetadata)new FrameworkPropertyMetadata(default(TextDecorationCollection), FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextEffects" /> dependency property.</summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextEffects" /> dependency property.</returns>
        public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(DefaultTypefaceManager), (PropertyMetadata)new FrameworkPropertyMetadata((object)default(TextEffectCollection), FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.LineHeight" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.LineHeight" /> dependency property.</returns>
        public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.LineStackingStrategy" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.LineStackingStrategy" /> dependency property.</returns>
        public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(DefaultTypefaceManager));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Padding" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.Padding" /> dependency property.</returns>
        public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(DefaultTypefaceManager), (PropertyMetadata)new FrameworkPropertyMetadata((object)new Thickness(), FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextAlignment" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextAlignment" /> dependency property.</returns>
        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(DefaultTypefaceManager));

        public double BaselineOffset
        {
            get { return (double) GetValue(BaselineOffsetProperty);}
            set { SetValue(BaselineOffsetProperty, value); }
        }

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextTrimming" /> dependency property. </summary>
        /// <returns>The identifier of the <see cref="P:System.Windows.Controls.DefaultTypefaceManager.TextTrimming" /> dependency property.</returns>

        public Typeface GetDefaultTypeface()
        {
            var family = (FontFamily)GetValue(TextElement.FontFamilyProperty);
            var typeface = new Typeface(family, (FontStyle) GetValue(TextElement.FontStyleProperty),
                (FontWeight) GetValue(TextElement.FontWeightProperty), (FontStretch) GetValue(TextElement.FontStretchProperty));
            return typeface;
        }

        /// <inheritdoc />
        public FontRendering GetRendering(double emSize, TextAlignment left, TextDecorationCollection textDecorationCollection,
            Brush brush, Typeface typeface)
        {
            return FontRendering.CreateInstance(emSize, left, textDecorationCollection, brush, typeface);
        }
    }
}