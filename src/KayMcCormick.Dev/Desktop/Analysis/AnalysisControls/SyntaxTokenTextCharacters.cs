using System;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <inheritdoc />
    public class SyntaxTokenTextCharacters : CustomTextCharacters
    {
        public SyntaxToken Token { get; }
        public SyntaxNode Node { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"SyntaxToken.{Token.Kind()} [{Node.Kind()}] [{Length}]";
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterString"></param>
        /// <param name="length"></param>
        /// <param name="textRunProperties"></param>
        /// <param name="token"></param>
        /// <param name="node"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public SyntaxTokenTextCharacters(string characterString, int length,
            TextRunProperties textRunProperties, SyntaxToken token, SyntaxNode node) : base(characterString, 0, length, textRunProperties, token.Span)
        {
            Token = token;
            if (Token.Kind() == SyntaxKind.None)
            {
                throw new InvalidOperationException(token.Span.ToString());
                
            }
            
            Node = node;
        }
    }
}