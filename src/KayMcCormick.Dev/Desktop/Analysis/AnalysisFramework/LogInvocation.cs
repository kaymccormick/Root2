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
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;

namespace AnalysisFramework
{
    public class LogInvocation : CodeAnalyseContext
    {
        private LogMessageRepr _msgval ;
        private string         sourceLocation ;
        private IMethodSymbol  methodSymbol ;

        private string _LoggerType ;
        private string _methodName ;


        public LogInvocation (
            string                sourceLocation
          , IMethodSymbol         methodSymbol
          , LogMessageRepr        msgval
          , StatementSyntax       statement
          , SemanticModel         currentModel
          , CompilationUnitSyntax currentRoot
          , ICodeSource           document
          , SyntaxTree            syntaxTree
        ) : base (
                  currentModel
                , currentRoot
                , statement
                , statement
                , document
                , syntaxTree
                 )
        {
            CurrentModel   = currentModel ;
            Statement      = statement ;
            Msgval         = msgval ;
            SourceLocation = sourceLocation ;
            MethodSymbol   = methodSymbol ;
            if ( methodSymbol != null )
            {
                MethodName = MethodSymbol.Name ;
                LoggerType = methodSymbol.ContainingType.ContainingNamespace.MetadataName
                             + "."
                             + methodSymbol.ContainingType.MetadataName ;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>[
        [JsonConstructor]
        public LogInvocation ( ICodeSource document , LogMessageRepr msgval , string sourceLocation , string loggerType , string methodName , IList < LogInvocationArgument > arguments , string sourceContext , string followingCode , string precedingCode , string code ) : base ( document )
        {
            _msgval = msgval ;
            this.sourceLocation = sourceLocation ;
            _LoggerType = loggerType ;
            _methodName = methodName ;
            Arguments = arguments ;
            SourceContext = sourceContext ;
            FollowingCode = followingCode ;
            PrecedingCode = precedingCode ;
            Code = code ;
        }

        public string SourceLocation { get => sourceLocation ; set => sourceLocation = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public IMethodSymbol MethodSymbol { get => methodSymbol ; set => methodSymbol = value ; }


        public LogMessageRepr Msgval { get => _msgval ; set => _msgval = value ; }

        public IList < LogInvocationArgument > Arguments { get ; set ; }

        public string SourceContext { get ; set ; }

        public string FollowingCode { get ; set ; }

        public string PrecedingCode { get ; set ; }

        public string Code { get ; set ; }

        public string LoggerType { get => _LoggerType ; set => _LoggerType = value ; }

        public string MethodName { get => _methodName ; set => _methodName = value ; }

        public string MethodDisplayName => LoggerType + "." + MethodName ;

        public override string ToString ( )
        {
            return $"{nameof ( SourceLocation )}: {SourceLocation}, {nameof ( Msgval )}: {Msgval}, {nameof ( MethodDisplayName )}: {MethodDisplayName}" ;
        }
    }
}