using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <inheritdoc />
    public class CustomTextEndOfLine : TextEndOfLine, ICustomSpan
    {
        private TextSpan _span;

        public TextSpan Span
        {
            get { return _span; }
            set { _span = value; }
        }

        public CustomTextEndOfLine(int length, TextSpan span) : base(length)
        {
            _span = span;
        }

        public CustomTextEndOfLine(int length, TextRunProperties textRunProperties, TextSpan span) : base(length, textRunProperties)
        {
        }
    }
}