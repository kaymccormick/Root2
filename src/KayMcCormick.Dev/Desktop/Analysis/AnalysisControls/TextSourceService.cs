using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    internal static class TextSourceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compilationErrors"></param>
        /// <param name="emSize"></param>
        /// <param name="pixelsPerDip"></param>
        /// <param name="typefaceManager"></param>
        /// <param name="syntaxNode"></param>
        /// <param name="syntaxTree"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public static ICustomTextSource CreateAndInitTextSource([NotNull] List<CompilationError> compilationErrors,
            double emSize, double pixelsPerDip,
            [NotNull] ITypefaceManager typefaceManager, SyntaxNode syntaxNode, [NotNull] SyntaxTree syntaxTree,
            [NotNull] CSharpCompilation compilation)
        {
            if (compilationErrors == null) throw new ArgumentNullException(nameof(compilationErrors));
            if (typefaceManager == null) throw new ArgumentNullException(nameof(typefaceManager));
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));
            var source = new SyntaxNodeCustomTextSource(pixelsPerDip, typefaceManager)
            {
                EmSize = emSize,
                Compilation = compilation,
                Tree = syntaxTree,
                Node = syntaxNode,
                Errors = compilationErrors
            };
            source.Init();
            return source;
        }
    }
}