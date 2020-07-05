using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericTextSource<T> : AppTextSource, IAppendableTextSource, ITextSourceProcess
    {
        private Typeface _typeface;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;
        private FontStretch _fontStretch;
        private FontRendering _fontRendering;

        public delegate void ProcessDelegate(GenericTextSource<T> source, T item);

        /// <inheritdoc />
        public GenericTextSource()
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
        private ProcessDelegate _processAction;


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
            if (Source != null)
                foreach (var logEventInstance in Source)
                {
                    Process(logEventInstance);
                }

            var i = 0;
            chars.Clear();
            foreach (var textRun in col)
            {
                Length += textRun.Length;
                chars.AddRange(Enumerable.Repeat(i, textRun.Length));
                i++;
            }

        }

        /// <inheritdoc />
        public void UpdateCharMap()
        {
        }

        /// <inheritdoc />
        public void SetSource(IEnumerable source)
        {
            Source = source?.Cast<T>();
            Init();
        }

        public override int Length { get; protected set; }

        /// <inheritdoc />
        public FontRendering FontRendering
        {
            get { return _fontRendering; }
            set { _fontRendering = value; }
        }

        /// <inheritdoc />
        public override GenericTextRunProperties BaseProps { get; set; }

        /// <inheritdoc />
        public int EolLength { get; } = 2;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Source { get; set; }

        public override BasicTextRunProperties BasicProps()
        {
            var xx = new BasicTextRunProperties(BaseProps);
            return xx;
        }

        public override void TextInput(int insertionPoint, InputRequest inputRequest)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eNewItems"></param>
        public void AppendRange(IEnumerable<T> eNewItems)
        {
            int colLength = col.Count;
            foreach (T inst in eNewItems)
            {
                Process(inst);
            }

            
            for (int i = colLength; i < col.Count; i++)
            {
                Length += col[i].Length;
                chars.AddRange(Enumerable.Repeat(i, col[i].Length));
            }


        }

        /// <inheritdoc />
        public virtual void AppendRange(IEnumerable eNewItems)
        {
            int colLength = col.Count;
            foreach (T inst in eNewItems)
            {
                Process(inst);
            }


            for (int i = colLength; i < col.Count; i++)
            {
                Length += col[i].Length;
                chars.AddRange(Enumerable.Repeat(i, col[i].Length));
            }


        }


        private void Process(T inst)
        {
            _processAction?.Invoke(this, inst);
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush[] levelBrushes { get; set; } = new[]
        {
            Brushes.LightBlue, Brushes.GreenYellow, Brushes.Aquamarine, Brushes.DarkOrange, Brushes.Crimson,
            Brushes.Black
        };

        /// <inheritdoc />
        public void SetProcess(ProcessDelegate proessActionDelegate)
        {
            _processAction = proessActionDelegate;
        }

        /// <inheritdoc />
        public void SetProcess(Delegate proessActionDelegate)
        {
            SetProcess((ProcessDelegate)proessActionDelegate);
        }

        public void AddTextRun(TextRun textRun)
        {
            col.Add(textRun);
        }
    }

    public interface ITextSourceProcess
    {
        void SetProcess(Delegate proessActionDelegate);
    }
}