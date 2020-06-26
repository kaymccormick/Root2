using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEventsSource : AppTextSource
    {
        private Typeface _typeface;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;
        private FontStretch _fontStretch;
        private FontRendering _fontRendering;

        public LogEventsSource(double pixelsPerDip)
        {
            PixelsPerDip = pixelsPerDip;
            _typeface = new Typeface(Family, _fontStyle, _fontWeight,
                _fontStretch);
            _fontRendering = FontRendering.CreateInstance(EmSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue,
                _typeface);
            BaseProps = new GenericTextRunProperties(
                _fontRendering,
                PixelsPerDip);

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

        public override void Init()
        {
            foreach (var logEventInstance in EventsSource)
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

        public override int Length { get; protected set; }
        public override GenericTextRunProperties BaseProps { get; set; }
        public IEnumerable<LogEventInstance> EventsSource { get; set; }

        public override BasicTextRunProperties BasicProps()
        {
            throw new System.NotImplementedException();
        }


        public override void TextInput(int InsertionPoint, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eNewItems"></param>
        public void AppendRange(IEnumerable eNewItems)
        {
            int colLength = col.Count;
                foreach (LogEventInstance inst in eNewItems)
                {
                    Process(inst);
                }

            
                for (int i = colLength; i < col.Count; i++)
                {
                    Length += col[i].Length;
                    chars.AddRange(Enumerable.Repeat(i, col[i].Length));
                }


        }

        private void Process(LogEventInstance logEventInstance)
        {
            col.Add(new CustomTextCharacters(NLog.LogLevel.FromOrdinal(logEventInstance.Level).ToString(),
                new BasicTextRunProperties(BaseProps).WithForegroundBrush(levelBrushes[logEventInstance.Level]), new TextSpan()));
            col.Add(new TabTextRun("\t", new BasicTextRunProperties(BaseProps)));
            col.Add(new CustomTextCharacters(logEventInstance.LoggerName, new BasicTextRunProperties(BaseProps), new TextSpan()));
            col.Add(new TabTextRun("\t", new BasicTextRunProperties(BaseProps)));
            col.Add(new CustomTextCharacters(logEventInstance.TimeStamp.ToString(), new BasicTextRunProperties(BaseProps), new TextSpan()));
            col.Add(new TabTextRun("\t", new BasicTextRunProperties(BaseProps)));
            col.Add(new CustomTextCharacters(logEventInstance.FormattedMessage, new BasicTextRunProperties(BaseProps), new TextSpan()));
            col.Add(new TextEndOfLine(2));
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush[] levelBrushes { get; set; } = new[]
        {
            Brushes.LightBlue, Brushes.GreenYellow, Brushes.Aquamarine, Brushes.DarkOrange, Brushes.Crimson,
            Brushes.Black
        };
    }
}