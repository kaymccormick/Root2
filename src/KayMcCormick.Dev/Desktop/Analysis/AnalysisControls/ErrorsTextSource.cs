using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.TextFormatting;

namespace AnalysisControls
{
    public class ErrorsTextSource : CustomTextSource3
    {
        private IEnumerable<CompilationError> errors;

        public ErrorsTextSource(double pixelsPerDip, IEnumerable<CompilationError> errors) : base(pixelsPerDip)
        {
            this.errors = errors;
        }

        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            if (Errors.Any())
            {
                if (textSourceCharacterIndex < 0)
                    throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");

                int count = 0;
                var inu = ErrorRuns.GetEnumerator();
                TextRun last = null;
                while (inu.MoveNext() && count < textSourceCharacterIndex)
                {
                    count += inu.Current.Length;
                    count += EolLength;
                    last = inu.Current;
                }

                if (inu.Current == null)
                {
                    return new TextEndOfParagraph(EolLength);
                }

                var x = count - textSourceCharacterIndex;
                if (x == 2)
                {
                    return new TextEndOfLine(EolLength);
                }

                return inu.Current;

            }
            return new TextEndOfParagraph(EolLength);
        }
    }
}