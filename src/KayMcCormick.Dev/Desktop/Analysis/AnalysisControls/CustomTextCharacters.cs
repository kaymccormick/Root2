using System;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomTextCharacters : TextCharacters, ICustomSpan
    {
        [NotNull] private readonly string _text;

        [NotNull]
        public string Text
        {
            get { return _text; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterString"></param>
        /// <param name="textRunProperties"></param>
        /// <param name="span"></param>
        public CustomTextCharacters([NotNull] string characterString, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(characterString, textRunProperties)
        {
            _text = characterString;
            Span = span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterString"></param>
        /// <param name="offsetToFirstChar"></param>
        /// <param name="length"></param>
        /// <param name="textRunProperties"></param>
        /// <param name="span"></param>
        public CustomTextCharacters([NotNull] string characterString, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span) : base(characterString, offsetToFirstChar, length, textRunProperties)
        {
            _text = characterString;
            Span = span;
        }

        /// <summary>
        /// 
        /// </summary>
        public TextSpan Span { get; }
#if false
        public SymbolDisplayPart DisplayPart { get; set; }
        public ITypeSymbol TypeSymbol { get; set; }
        public Type Type { get; set; }
        
        public SyntaxTrivia SyntaxTrivia { get; set; }
#endif
        /// <summary>
        /// 
        /// </summary>
        public int? Index { get; set; }
    }
}