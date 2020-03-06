using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisFramework
{
    public static class LogUsages
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static IEnumerable<ILogInvocation> FindLogUsages(
            ICodeSource                 document1
          , CompilationUnitSyntax       currentRoot
          , SemanticModel               currentModel
           ,
            SyntaxTree                  syntaxTree
        )
        {
            var comp = currentModel.Compilation ;
            var ExceptionType = comp.GetTypeByMetadataName ( "System.Exception" ) ;
            if ( ExceptionType == null )
            {
                Logger.Warn( "No exception type" ) ;
            }

            var t1 = comp.GetTypeByMetadataName ( LoggerClassFullName ) ;

            if ( t1 == null )
            {
                throw new MissingTypeException ( LoggerClassFullName ) ;
            }

            var t2 = comp.GetTypeByMetadataName ( ILoggerClassFullName ) ;
            if ( t2 == null )
            {
                throw new MissingTypeException(ILoggerClassFullName);
            }

            bool limitToMarkedStatements = false ;
            var qq0 =
                from node in currentRoot.DescendantNodesAndSelf ( ).OfType < StatementSyntax > ( )
                where limitToMarkedStatements == false
                      || node.GetLeadingTrivia ( )
                             .Any (
                                   trivia => trivia.Kind ( ) == SyntaxKind.SingleLineCommentTrivia
                                             && trivia.ToString ( ).Contains ( "doprocess" )
                                  )
                select node ;

            bool logVisitedStatements = false ;
            if ( logVisitedStatements )
            {
                foreach ( var q in qq0 )
                {
                    Logger.Info ( q.ToString ) ;
                }
            }

            IMethodSymbol methodSymbol1 = null ;
return
                from statement in qq0
                let invocations =
                    statement.DescendantNodes (
                                               node => node == statement
                                                       || ! ( node is StatementSyntax )
                                              )
                             .OfType < InvocationExpressionSyntax > ( )
                from invocation in invocations
                let @out = CheckInvocationExpression ( invocation , currentModel )
                let methodSymbol = methodSymbol1
                where @out.Item1
                select InvocationParms.ProcessInvocation (
                                                          new InvocationParms (
                                                                               document1
                                                                             , syntaxTree
                                                                             , currentModel
                                                                             , statement
                                                                             , @out
                                                                             , ExceptionType
                                                                              )
                                                         ) ;


        }

        public const            string ILoggerClassName    = "ILogger" ;
        public const            string LoggerClassName     = "Logger" ;
        private static readonly string LoggerClassFullName = NLogNamespace + '.' + LoggerClassName ;

        public static readonly string
            ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

        private const string NLogNamespace = "NLog" ;


        private static bool CheckSymbol (
            IMethodSymbol    methSym
          , params INamedTypeSymbol[] t1
          
        )
        {
            var cType = methSym.ContainingType ;
            var r = t1.Any ( symbol => SymbolEqualityComparer.Default.Equals ( cType , symbol ) ) ;
            return r ;
        }

        public static bool IsException ( INamedTypeSymbol exceptionType , ITypeSymbol baseType )
        {
            if ( exceptionType == null ) return false ;
            var isException = false ;
            while ( baseType != null )
            {
                if ( SymbolEqualityComparer.Default.Equals ( baseType , exceptionType ) )
                {
                    isException = true ;
                }

                baseType = baseType.BaseType ;
            }

            return isException ;
        }

        public static Tuple<bool, IMethodSymbol, InvocationExpressionSyntax> CheckInvocationExpression (
            InvocationExpressionSyntax node
          , SemanticModel              currentModel
,
params INamedTypeSymbol[] t)
        {
            try
            {
                var symbolInfo = currentModel.GetSymbolInfo(node.Expression);
#if DEBUG
                Logger.Debug (
                              "{method} mode is {node}"
                            , nameof ( CheckInvocationExpression )
                            , node.GetLocation ( )
                             ) ;
                Logger.Debug ( "{exprKind}, {exor}" , node.Expression.Kind ( ) , node.Expression ) ;

                Logger.Info ( "symbolinfo is {node}" , symbolInfo.Symbol?.ToDisplayString() ?? "null" ) ;
                if(symbolInfo.Symbol == null)
                {
                    Logger.Info ( "candidate symbols: {x}" , symbolInfo.CandidateSymbols ) ;
                }
                #endif
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol ;
                var result = methodSymbol != null
                                  // TODO optmize
                                  && CheckSymbol (
                                                  methodSymbol, t
                                                ) ;
                #if DEBUG
                Logger.Debug ( "result is {result}" , result ) ;
                #endif
                return Tuple.Create (
                                     result
                                   , methodSymbol
                                   , node
                                    ) ;
            } catch(Exception ex)
            {
                throw ;
               // return Tuple.Create ( false , null , node  ) ;
            }
        }

        public static INamedTypeSymbol GetILoggerSymbol ( SemanticModel model )
        {
            return model.Compilation.GetTypeByMetadataName(ILoggerClassFullName);
        }

        public static INamedTypeSymbol GetNLogSymbol ( SemanticModel model )
        {
            return model.Compilation.GetTypeByMetadataName(LoggerClassFullName);

        }
    }

    public class MissingTypeException : Exception
    {
        public MissingTypeException ( ) {
        }

        public MissingTypeException ( string message ) : base ( message )
        {
        }

        public MissingTypeException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected MissingTypeException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}