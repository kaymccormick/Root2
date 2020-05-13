using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticError : CompilationError
    {
        private readonly Diagnostic _diagnostic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnostic"></param>
        public DiagnosticError(Diagnostic diagnostic)
        {
            _diagnostic = diagnostic;
            foreach (var kv in _diagnostic.Properties) DebugUtils.WriteLine($"{_diagnostic.Id}: {kv.Key}: {kv.Value}");
            Message = _diagnostic.GetMessage();
        }
    }
}