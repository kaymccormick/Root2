#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// CodeAnalysisApp1
// RefactorLogUsage.cs
// 
// 2020-02-26-7:15 AM
// 
// ---
#endregion
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis.CodeRefactorings ;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    ///     Refactoring for logging.
    /// </summary>
    public class RefactorLogUsage : CodeRefactoringProvider
    {
        /// <summary>
        ///     Main refactoring routine.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public sealed override async Task ComputeRefactoringsAsync (
            CodeRefactoringContext context
        )
        {
            var document = context.Document ;
            var textSpan = context.Span ;
            var cancellationToken = context.CancellationToken ;

            var root = await document.GetSyntaxRootAsync ( cancellationToken )
                                     .ConfigureAwait ( false ) ;

            var token = root?.FindToken ( textSpan.Start ) ;
            if ( token?.Parent == null )
            {
                return ;
            }

            var model = await document.GetSemanticModelAsync ( cancellationToken ) ;
            if ( model != null )
            {
                // var query = Common.Query1 ( root 
                // , model ) ;
            }
        }
    }
}