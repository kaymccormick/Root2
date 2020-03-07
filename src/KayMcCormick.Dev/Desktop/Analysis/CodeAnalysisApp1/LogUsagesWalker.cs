#region header
// Kay McCormick (mccor)
// 
// ConsoleApp1
// CodeAnalysisApp1
// LogUsagesRewriter2.cs
// 
// 2020-03-02-3:07 AM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Text.Json ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.Text ;

using NLog ;

namespace CodeAnalysisApp1
{
    public class LogUsagesWalker : CSharpSyntaxWalker
    {
        private static   Logger                           Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly SyntaxTree                       _syntaxTree ;
        private          SemanticModel                    model ;
        private          ICodeSource                      document ;
        private          CompilationUnitSyntax            currentRoot ;
        private readonly Action < SyntaxNode , TextSpan > _progress ;
        private          INamedTypeSymbol                 exceptionType ;

        public LogUsagesWalker ( SyntaxTree syntaxTree, SemanticModel model , ICodeSource document , CompilationUnitSyntax currentRoot , Action<SyntaxNode, TextSpan> progress, bool visitIntoStructuredTrivia = false ) : base ( SyntaxWalkerDepth.Trivia )
        {
            _syntaxTree        = syntaxTree ;
            this.model         = model ;
            this.document      = document ;
            this.currentRoot   = currentRoot ;
            _progress          = progress ;
            this.exceptionType = this.model.Compilation.GetTypeByMetadataName("System.Exception");
        }


        /// <summary>Called when the visitor visits a InvocationExpressionSyntax node.</summary>
        public override void VisitInvocationExpression ( InvocationExpressionSyntax node )
        {
            var check = LogUsages.CheckInvocationExpression(node, model);
            if (check.Item1)
            {
                ILogInvocation logInvocation = InvocationParms.ProcessInvocation(
                                                                                 new InvocationParms(
                                                                                                     document
                                                                                                    ,
                                                                                                     model.SyntaxTree
                                                                                                   , model
                                                                                                   , node.AncestorsAndSelf()
                                                                                                         .OfType<StatementSyntax
                                                                                                          >()
                                                                                                         .First()
                                                                                                   , check
                                                                                                   , exceptionType
                                                                                                    )
                                                                                );

                void Arg9 ( ILogInvocation invocation )
                {
                    try { logInvocation = invocation ; }
                    catch ( Exception ex ) { Logger.Error ( ex , ex.ToString ( ) ) ; }
                }
                var serializeObject = JsonSerializer.Serialize(logInvocation);
                LogManager.GetCurrentClassLogger ( ).Warn ( "{ser}" , serializeObject ) ;
            }
        }
    }
}