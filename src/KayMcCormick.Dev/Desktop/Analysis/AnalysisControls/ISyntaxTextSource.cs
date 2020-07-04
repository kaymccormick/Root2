using System;
using System.Collections.Generic;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISyntaxTextSource : ICustomTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        Compilation Compilation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        SyntaxNode Node { get; set; }

        /// <summary>
        /// 
        /// </summary>
        SyntaxTree Tree { get; set; }

        /// <summary>
        /// 
        /// </summary>
        List<TextRun> ErrorRuns { get; }
        TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trivia"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        TextRunProperties PropsFor(in SyntaxTrivia trivia, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        TextRunProperties PropsFor(SyntaxToken token, string text);
    }
}