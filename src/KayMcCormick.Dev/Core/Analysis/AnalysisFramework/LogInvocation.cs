#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocation.cs
// 
// 2020-02-25-9:21 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.ComponentModel ;
#if !NETSTANDARD2_0
using System.Text.Json.Serialization ;
#endif
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;


namespace AnalysisFramework
{
    internal class LogInvocation : CodeAnalyseContext , ILogInvocation
    {
        private LogMessageRepr _msgval ;
        private string         sourceLocation ;
        private IMethodSymbol  methodSymbol ;

        private string _LoggerType ;
        private string _methodName ;
        public LogInvocation(
            string                sourceLocation
          , IMethodSymbol         methodSymbol
          , LogMessageRepr        msgval
          , SyntaxNode relevantNode 
          , SemanticModel         currentModel
          , ICodeSource           document
          , SyntaxTree            syntaxTree
        ) : base(
                 currentModel
               , relevantNode as  StatementSyntax
               , relevantNode
               , document
               , syntaxTree, ""
                )
        { 
            Msgval         = msgval;
            SourceLocation = sourceLocation;
            MethodSymbol   = methodSymbol;
            if (methodSymbol != null)
            {
                MethodName = MethodSymbol.Name;
                LoggerType = methodSymbol.ContainingType.ContainingNamespace.MetadataName
                             + "."
                             + methodSymbol.ContainingType.MetadataName;
            }
        }


        public string SourceLocation { get => sourceLocation ; set => sourceLocation = value ; }

#if !NETSTANDARD2_0
        [ JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public IMethodSymbol MethodSymbol { get => methodSymbol ; set => methodSymbol = value ; }


        public LogMessageRepr Msgval { get => _msgval ; set => _msgval = value ; }

        public string FollowingCode { get ; set ; }

        public string PrecedingCode { get ; set ; }

        public string Code { get ; set ; }

        public string LoggerType { get => _LoggerType ; set => _LoggerType = value ; }

        public string MethodName { get => _methodName ; set => _methodName = value ; }

        public string MethodDisplayName => LoggerType + "." + MethodName ;

        public IList < ILogInvocationArgument > Arguments { get ; set ; }

        public override string ToString ( )
        {
            return $"{nameof ( SourceLocation )}: {SourceLocation}, {nameof ( Msgval )}: {Msgval}, {nameof ( MethodDisplayName )}: {MethodDisplayName}" ;
        }
    }
}