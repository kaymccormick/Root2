using System ;
using System.Collections.Generic ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisFramework
{
    public class InvocationParms
    {
        public readonly SyntaxTree SyntaxTree ;

        public InvocationParms ( List < LogInvocation > arg1 , CompilationUnitSyntax arg2 , SemanticModel model , ICodeSource  document , StatementSyntax statement , InvocationExpressionSyntax invocationExpression , IMethodSymbol methodSymbol , INamedTypeSymbol namedTypeSymbol , Action < LogInvocation > consumeAction, SyntaxTree syntaxTree )
        {
            SyntaxTree = syntaxTree ;
            Arg1 = arg1 ;
            Arg2 = arg2 ;
            Model = model ;
            Document = document ;
            Statement = statement ;
            InvocationExpression = invocationExpression ;
            MethodSymbol = methodSymbol ;
            NamedTypeSymbol = namedTypeSymbol ;
            ConsumeAction = consumeAction ;
        }

        public List < LogInvocation > Arg1 { get ; private set ; }

        public CompilationUnitSyntax Arg2 { get ; private set ; }

        public SemanticModel Model { get ; private set ; }

        public ICodeSource Document { get ; private set ; }

        public StatementSyntax Statement { get ; private set ; }

        public InvocationExpressionSyntax InvocationExpression { get ; private set ; }

        public IMethodSymbol MethodSymbol { get ; private set ; }

        public INamedTypeSymbol NamedTypeSymbol { get ; private set ; }

        public Action < LogInvocation > ConsumeAction { get ; private set ; }
    }
}