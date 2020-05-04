using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    internal class SyntaxTriviaTextCharacters : CustomTextCharacters
    {
        public SyntaxTrivia Trivia { get; }


        public SyntaxTriviaTextCharacters([NotNull] string characterString, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(characterString, textRunProperties, span)
        {
            Trivia = syntaxTrivia;
        }

        public SyntaxTriviaTextCharacters([NotNull] string characterString, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(characterString, offsetToFirstChar, length, textRunProperties, span)
        {
            Trivia = syntaxTrivia;
        }

        public override string ToString()
        {
            return $"SyntaxTrivia {Trivia.Kind()} [{Length}]";
        }

    }
}