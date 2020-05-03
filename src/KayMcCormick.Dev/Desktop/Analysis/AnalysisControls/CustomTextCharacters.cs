using System;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    public class CustomTextCharacters : TextCharacters, ICustomSpan
    {
        public CustomTextCharacters([NotNull] char[] characterArray, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(characterArray, offsetToFirstChar, length, textRunProperties)
        {
            Span = span;
        }

        public CustomTextCharacters([NotNull] string characterString, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(characterString, textRunProperties)
        {
            Span = span;
        }

        public CustomTextCharacters([NotNull] string characterString, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(characterString, offsetToFirstChar, length, textRunProperties)
        {
            Span = span;
        }

        public unsafe CustomTextCharacters([NotNull] char* unsafeCharacterString, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(unsafeCharacterString, length, textRunProperties)
        {
            Span = span;
        }

        public TextSpan Span { get; }
        public SymbolDisplayPart DisplayPart { get; set; }
        public ITypeSymbol TypeSymbol { get; set; }
        public Type Type { get; set; }
        public int Index { get; set; }
        public SyntaxTrivia SyntaxTrivia { get; set; }
    }
}