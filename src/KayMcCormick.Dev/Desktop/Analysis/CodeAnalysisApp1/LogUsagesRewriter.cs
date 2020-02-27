using System ;
using System.Linq ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.Text ;
using Newtonsoft.Json ;
using NLog ;

namespace CodeAnalysisApp1
{
    public class LogUsagesRewriter : CSharpSyntaxRewriter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly SyntaxTree _syntaxTree ;
        private SemanticModel         model ;
        private ICodeSource           document ;
        private CompilationUnitSyntax currentRoot ;
        private readonly Action < SyntaxNode , TextSpan > _progress ;
        private INamedTypeSymbol      exceptionType ;

        public override SyntaxNode  DefaultVisit ( SyntaxNode node )
        {
            if ( _progress != null ) Logger.Info ( "calling progress" ) ;
            _progress?.Invoke(node, node.Span);
            return base.DefaultVisit ( node ) ;
        }

        public LogUsagesRewriter ( SyntaxTree syntaxTree, SemanticModel model , ICodeSource document , CompilationUnitSyntax currentRoot , Action<SyntaxNode, TextSpan> progress, bool visitIntoStructuredTrivia = false ) : base ( visitIntoStructuredTrivia )
        {
            _syntaxTree = syntaxTree ;
            this.model         = model ;
            this.document      = document ;
            this.currentRoot   = currentRoot ;
            _progress = progress ;
            this.exceptionType = this.model.Compilation.GetTypeByMetadataName("System.Exception");
        }


        /// <summary>Called when the visitor visits a InvocationExpressionSyntax node.</summary>
        public override SyntaxNode VisitInvocationExpression ( InvocationExpressionSyntax node )
        {
            if ( LogUsages.CheckInvocationExpression ( node, out var methodSymbol, model ) )
            {
                LogInvocation logInvocation = LogUsages.ProcessInvocation (
                                             new InvocationParms (
                                                                  null
                                                                , currentRoot
                                                                , model
                                                                , document
                                                                , node.AncestorsAndSelf ( )
                                                                      .OfType < StatementSyntax
                                                                       > ( )
                                                                      .First ( )
                                                                , node
                                                                , methodSymbol
                                                                , exceptionType
                                                                , null, 
                                                                  null
                                                                 )
                                            ) ;

                void Arg9 ( LogInvocation invocation )
                {
                    try { logInvocation = invocation ; }
                    catch ( Exception ex ) { Logger.Error ( ex , ex.ToString ( ) ) ; }
                }

                var serializeObject = JsonConvert.SerializeObject(logInvocation, Formatting.Indented) ;
                LogManager.GetCurrentClassLogger().Warn ( "{ser}" , serializeObject ) ;
                return node.WithAdditionalAnnotations (
                                                       new[]
                                                       {
                                                           new SyntaxAnnotation (
                                                                                 "LogInvocation"
                                                                               , serializeObject)
                                                           
                                                       }
                                                      ) ;
            }

            return node ;
        }
    }
}