using System.Composition;
using System.Linq;
using System.Net.Mime ;
using System.Threading;
using System.Threading.Tasks;
using AnalysisFramework ;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

namespace CodeRefactoring1
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(CodeRefactoring1CodeRefactoringProvider)), Shared]
    internal class CodeRefactoring1CodeRefactoringProvider : CodeRefactoringProvider
    {
        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            // TODO: Replace the following code with your own analysis, generating a CodeAction for each refactoring to offer

            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // Find the node at the selection.
            var node = root.FindNode(context.Span);

            // Only offer a refactoring if the selected node is a type declaration node.
            if (!(node is InvocationExpressionSyntax inv))
            {
                return;
            }

            var model = await context.Document.GetSemanticModelAsync ( context.CancellationToken )
                                     .ConfigureAwait ( false ) ;

            if ( !LogUsages.CheckInvocationExpression ( inv , out var methodSymbol , model ) )
            {
                return ;
            }
                // For any type declaration node, create a code action to reverse the identifier text.
                var action = CodeAction.Create("Update log usage", c => ReverseTypeNameAsync(context.Document, inv, model, methodSymbol, c));

            // Register this code action.
            context.RegisterRefactoring(action);
        }

        private async Task < Solution > ReverseTypeNameAsync (
            Document                   document
          , InvocationExpressionSyntax inv
          , SemanticModel              model
          , IMethodSymbol              methodSymbol
          , CancellationToken          cancellationToken
        )
        {
            var syntaxTree = await document.GetSyntaxTreeAsync ( cancellationToken ) ;
            ILogInvocation logInvocation = InvocationParms.ProcessInvocation(
                                                                      new InvocationParms(
                                                                                          new CodeSource(document.FilePath)
                                                                                         ,
                                                                                          null
                                                                                        , model
                                                                                         , inv.AncestorsAndSelf()
                                                                                              .OfType<StatementSyntax
                                                                                               >()
                                                                                              .First()
                                                                                         , inv
                                                                                         , methodSymbol
                                                                                         , null
                                                                                         )
                                                                     );

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            // var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return originalSolution;
        }
    }
}
