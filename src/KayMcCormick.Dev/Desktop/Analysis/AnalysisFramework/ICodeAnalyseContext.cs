#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ICodeAnalyseContext.cs
// 
// 2020-03-05-3:11 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
{
    public interface ICodeAnalyseContext : ICompilationUnitRootContext , ISemanticModelContext
    {
    }

    public class CodeAnalyseContext2 : ICodeAnalyseContext
    {
        public CodeAnalyseContext2 ( SemanticModel currentModel )
        {
            _currentModel = currentModel ;
            _syntaxTree = _currentModel.SyntaxTree ;
            _compilationUnit = _syntaxTree.GetCompilationUnitRoot ( ) ;
        }

        private SyntaxTree _syntaxTree ;
        private CompilationUnitSyntax _compilationUnit ;
        private SemanticModel _currentModel ;
        #region Implementation of ISyntaxTreeContext
        public SyntaxTree SyntaxTree { get => _syntaxTree ; set => _syntaxTree = value ; }
        #endregion

        #region Implementation of ICompilationUnitRootContext
        public CompilationUnitSyntax CompilationUnit { get => _compilationUnit ; set => _compilationUnit = value ; }
        #endregion

        #region Implementation of ISemanticModelContext
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }
        #endregion
    }
}