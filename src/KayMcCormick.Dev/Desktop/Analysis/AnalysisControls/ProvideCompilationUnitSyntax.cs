using System ;
using System.Windows.Markup ;
using AnalysisControls.Properties ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    public class ProvideCompilationUnitSyntax : MarkupExtension
    {
        #region Overrides of MarkupExtension
        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        [ NotNull ]
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            return SyntaxFactory.ParseCompilationUnit ( Resources.Program_Parse ) ;
        }
        #endregion
    }
}