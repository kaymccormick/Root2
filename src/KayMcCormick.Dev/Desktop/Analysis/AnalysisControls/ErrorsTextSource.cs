using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorsTextSource : CustomTextSource4
    {
        private IEnumerable<CompilationError> _errors;

        public ErrorsTextSource(double pixelsPerDip, FontRendering fontRendering, GenericTextRunProperties genericTextRunProperties, [NotNull] SynchronizationContext synchContext, IEnumerable<CompilationError> errors) : base(pixelsPerDip, fontRendering, genericTextRunProperties, synchContext)
        {
            _errors = errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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