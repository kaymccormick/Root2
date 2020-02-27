using System ;
using System.Collections.Generic ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace CodeAnalysisApp1
{
    public class InvocationParms
    {
        public readonly SyntaxTree SyntaxTree ;

        public InvocationParms ( List < LogInvocation > arg1 , CompilationUnitSyntax arg2 , SemanticModel arg3 , ICodeSource  document , StatementSyntax arg5 , InvocationExpressionSyntax arg6 , IMethodSymbol arg7 , INamedTypeSymbol arg8 , Action < LogInvocation > arg9, SyntaxTree syntaxTree )
        {
            SyntaxTree = syntaxTree ;
            Arg1 = arg1 ;
            Arg2 = arg2 ;
            Arg3 = arg3 ;
            Document = document ;
            Arg5 = arg5 ;
            Arg6 = arg6 ;
            Arg7 = arg7 ;
            Arg8 = arg8 ;
            Arg9 = arg9 ;
        }

        public List < LogInvocation > Arg1 { get ; private set ; }

        public CompilationUnitSyntax Arg2 { get ; private set ; }

        public SemanticModel Arg3 { get ; private set ; }

        public ICodeSource Document { get ; private set ; }

        public StatementSyntax Arg5 { get ; private set ; }

        public InvocationExpressionSyntax Arg6 { get ; private set ; }

        public IMethodSymbol Arg7 { get ; private set ; }

        public INamedTypeSymbol Arg8 { get ; private set ; }

        public Action < LogInvocation > Arg9 { get ; private set ; }
    }
}