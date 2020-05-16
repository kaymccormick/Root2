using System.Collections.Generic;
using AnalysisAppLib;

namespace AnalysisControls
{
    static internal class TextSourceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compilationErrors"></param>
        /// <param name="emSize"></param>
        /// <param name="pixelsPerDip"></param>
        /// <param name="typefaceManager"></param>
        /// <param name="codeAnalyseContext"></param>
        /// <returns></returns>
        public static ICustomTextSource CreateAndInitTextSource(List<CompilationError> compilationErrors, double emSize, double pixelsPerDip, ITypefaceManager typefaceManager, ICodeAnalyseContext codeAnalyseContext)
        {
            var source = new SyntaxNodeCustomTextSource(pixelsPerDip, typefaceManager)
            {
                EmSize = emSize,
                Compilation = codeAnalyseContext.Compilation,
                Tree = codeAnalyseContext.SyntaxTree,
                Node = codeAnalyseContext.Node,
                Errors = compilationErrors
            };
            source.Init();
            return source;
        }
    }
}