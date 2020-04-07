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
    /// <summary>
    /// 
    /// </summary>
    public class CodeAnalyseContext2 : ICodeAnalyseContext
    {
        private CompilationUnitSyntax _compilationUnit ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentModel"></param>
        public CodeAnalyseContext2 ( SemanticModel currentModel )
        {
            CurrentModel     = currentModel ;
            SyntaxTree       = CurrentModel.SyntaxTree ;
            _compilationUnit = SyntaxTree.GetCompilationUnitRoot ( ) ;
        }

        #region Implementation of ISyntaxTreeContext
        /// <summary>
        /// 
        /// </summary>
        public SyntaxTree SyntaxTree { get ; set ; }
        #endregion

        #region Implementation of ICompilationUnitRootContext
        /// <summary>
        /// 
        /// </summary>
        public CompilationUnitSyntax CompilationUnit
        {
            get { return _compilationUnit ; }
            set { _compilationUnit = value ; }
        }
        #endregion

        #region Implementation of ISemanticModelContext
        /// <summary>
        /// 
        /// </summary>
        public SemanticModel CurrentModel { get ; set ; }
        #endregion
    }
}