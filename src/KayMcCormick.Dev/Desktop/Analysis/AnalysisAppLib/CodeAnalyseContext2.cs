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

namespace AnalysisAppLib
{
    public class CodeAnalyseContext2 : ICodeAnalyseContext
    {
        private CompilationUnitSyntax _compilationUnit ;

        public CodeAnalyseContext2 ( SemanticModel currentModel )
        {
            CurrentModel     = currentModel ;
            SyntaxTree       = CurrentModel.SyntaxTree ;
            _compilationUnit = SyntaxTree.GetCompilationUnitRoot ( ) ;
        }

        #region Implementation of ISyntaxTreeContext
        public SyntaxTree SyntaxTree { get ; set ; }
        #endregion

        #region Implementation of ICompilationUnitRootContext
        public CompilationUnitSyntax CompilationUnit
        {
            get { return _compilationUnit ; }
            set { _compilationUnit = value ; }
        }
        #endregion

        #region Implementation of ISemanticModelContext
        public SemanticModel CurrentModel { get ; set ; }
        #endregion
    }
}