using System;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    public class SyntaxTokenTextCharacters : CustomTextCharacters
    {
        private readonly SyntaxToken _token;
        private readonly SyntaxNode _node;

        public override string ToString()
        {
            return $"SyntaxToken.{_token.Kind()} [{_node.Kind()}] [{Length}]";
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
            TextRunProperties textRunProperties, in SyntaxToken token, SyntaxNode node) : base(characterString, 0, length, textRunProperties, token.Span)
        {
            _token = token;
            if (_token.Kind() == SyntaxKind.None)
            {
                throw new InvalidOperationException(token.Span.ToString());
                
            }
            
            _node = node;
        }
    }
}