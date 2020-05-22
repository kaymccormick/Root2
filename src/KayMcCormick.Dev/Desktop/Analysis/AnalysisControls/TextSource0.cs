using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TextSource0 : AppTextSource, IReadWriteTextSource
    {
        private Typeface _typeface;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;
        private FontStretch _fontStretch;
        private FontRendering _fontRendering;


        /// <inheritdoc />
        public TextSource0()
        {
            _typeface = new Typeface(Family, _fontStyle, _fontWeight,
                _fontStretch);
            _fontRendering = FontRendering.CreateInstance(EmSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue,
                _typeface);
            BaseProps = TextPropertiesManager.GetBasicTextRunProperties(_fontRendering);

        }

        public double EmSize { get; set; } = 18;

        public FontFamily Family { get; set; } = new FontFamily("Arial");

        private readonly List<int> chars = new List<int>();
        private readonly List<TextRun> col = new List<TextRun>();


        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(textSourceCharacterIndex), "Value must be greater than 0.");
            if (textSourceCharacterIndex >= Length) return new TextEndOfParagraph(1);

            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < Length)
            {
                var xx = chars[textSourceCharacterIndex];
                // var xx2 = chars2[textSourceCharacterIndex];
                if (col[xx] is CustomTextCharacters tc)
                {
                    var z = textSourceCharacterIndex - tc.Index;
                    var zz = tc.Length - z;

                    var tf = tc.Properties.Typeface;
                    return tc;
                }

                return col[xx];
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void TakeTextRun(TextRun obj)
        {
            AddTextRun(obj);
        }

        public override void Init()
        {

            ProcessDocumentIntoTextRuns();

            var i = 0;
            chars.Clear();
            foreach (var textRun in col)
            {
                Length += textRun.Length;
                chars.AddRange(Enumerable.Repeat(i, textRun.Length));
                i++;
            }

        }

        private void ProcessDocumentIntoTextRuns()
        {
        }

        /// <inheritdoc />
        public void UpdateCharMap()
        {
        }


        public override int Length { get; protected set; }

        /// <inheritdoc />
        public FontRendering FontRendering
        {
            get { return _fontRendering; }
            set { _fontRendering = value; }
        }

        /// <inheritdoc />
        public override GenericTextRunProperties BaseProps { get; }

        /// <inheritdoc />
        public int EolLength { get; } = 2;

      

        public override BasicTextRunProperties BasicProps()
        {
            var xx = new BasicTextRunProperties(BaseProps);
            return xx;
        }

        /// <inheritdoc />
        public virtual void GenerateText()
        {
        }

        /// <inheritdoc />
        public TextRunProperties MakeProperties(object arg, string text)
        {
            var r = BasicProps();
            return r;
        }

        public override void TextInput(int InsertionPoint, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush[] levelBrushes { get; set; } = new[]
        {
            Brushes.LightBlue, Brushes.GreenYellow, Brushes.Aquamarine, Brushes.DarkOrange, Brushes.Crimson,
            Brushes.Black
        };

        public void AddTextRun(TextRun textRun)
        {
            col.Add(textRun);
        }
    }

    public interface IReadWriteTextSource : ICustomTextSource
    {
    }
}