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
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CodeRefactorings ;

namespace CodeAnalysisApp1
{
    /// <summary>
    /// Refactoring for logging.
    /// </summary>
    [ UsedImplicitly ]
    public class RefactorLogUsage : CodeRefactoringProvider
    {
        /// <summary>
        /// Main refactoring routine.
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
            if ( root == null )
            {
                return ;
            }

            var token = root.FindToken ( textSpan.Start ) ;
            if ( token.Parent == null )
            {
                return ;
            }

            var model = await document.GetSemanticModelAsync ( cancellationToken ) ;
            if ( model != null )
            {
                // ReSharper disable once UnusedVariable
#pragma warning disable IDE0059 // Unnecessary assignment of a value


                var query = Common.Query1 ( root , model ) ;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }
        }
    }
}