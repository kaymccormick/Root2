using System ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
{
    public class InvocationParms
    {
        public readonly SyntaxTree SyntaxTree ;

        public InvocationParms (
            CompilationUnitSyntax      arg2
          , SemanticModel              model
          , ICodeSource                document
          , StatementSyntax            statement
          , InvocationExpressionSyntax invocationExpression
          , IMethodSymbol              methodSymbol
          , INamedTypeSymbol           namedTypeSymbol
          , Action < ILogInvocation >   consumeAction
          , SyntaxTree                 syntaxTree
        )
        {
            SyntaxTree = syntaxTree ;
            
            Arg2 = arg2 ;
            Model = model ;
            Document = document ;
            Statement = statement ;
            InvocationExpression = invocationExpression ;
            MethodSymbol = methodSymbol ;
            NamedTypeSymbol = namedTypeSymbol ;
            ConsumeAction = consumeAction ;
        }

        public CompilationUnitSyntax Arg2 { get ; private set ; }

        public SemanticModel Model { get ; private set ; }

        public ICodeSource Document { get ; private set ; }

        public StatementSyntax Statement { get ; private set ; }

        public InvocationExpressionSyntax InvocationExpression { get ; private set ; }

        public IMethodSymbol MethodSymbol { get ; private set ; }

        public INamedTypeSymbol NamedTypeSymbol { get ; private set ; }

        public Action < ILogInvocation > ConsumeAction { get ; private set ; }
    }
}