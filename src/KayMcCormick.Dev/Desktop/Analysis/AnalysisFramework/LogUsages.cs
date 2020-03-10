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

        #if false
        public static IEnumerable < ILogInvocation > FindLogUsages (
            ICodeSource           document1
          , CompilationUnitSyntax currentRoot
          , SemanticModel         currentModel
          , SyntaxTree            syntaxTree
        )
        {
            var comp = currentModel.Compilation ;
            var ExceptionType = comp.GetTypeByMetadataName ( "System.Exception" ) ;
            if ( ExceptionType == null )
            {
                Logger.Warn ( "No exception type" ) ;
            }

            var t1 = comp.GetTypeByMetadataName ( LoggerClassFullName ) ;

            if ( t1 == null )
            {
                throw new MissingTypeException ( LoggerClassFullName ) ;
            }

            var t2 = comp.GetTypeByMetadataName ( ILoggerClassFullName ) ;
            if ( t2 == null )
            {
                throw new MissingTypeException ( ILoggerClassFullName ) ;
            }

            var limitToMarkedStatements = false ;
            var qq0 =
               ( from node in currentRoot.DescendantNodesAndSelf ( ).OfType < StatementSyntax > ( )
                where limitToMarkedStatements == false
                      || node.GetLeadingTrivia ( )
                             .Any (
                                   trivia => {
                                       var b = trivia.Kind ( ) == SyntaxKind.SingleLineCommentTrivia ;
                                       var contains = trivia.ToString ( ).Contains ( "doprocess" ) ;
                                       return b && contains ;
                                   }
                                  )
                select node).ToList (  ) ;

            var logVisitedStatements = false ;
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
                select (new InvocationParms (
                                                                               document1
                                                                             , syntaxTree
                                                                             , currentModel
                                                                             , statement
                                                                             , @out
                                                                             , ExceptionType
                                                                              ).ProcessInvocation());
                                                         
        }
        #endif
        public const string LogBuilderClassName= "LogBuilder";
        public const string LogBuilderNamespaceName = NLogNamespace + ".Fluent" ;

        public const string LogBuilderClassFullName =
            LogBuilderNamespaceName + "." + LogBuilderClassName ;
        public const string ILoggerClassName = "ILogger";
        public const            string LoggerClassName     = "Logger";
        private static readonly string LoggerClassFullName = NLogNamespace + '.' + LoggerClassName;

        public static readonly string
            ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

        private const string NLogNamespace = "NLog" ;


        private static bool CheckSymbol ( IMethodSymbol methSym , params INamedTypeSymbol[] t1 )
        {
            var cType = methSym.ContainingType ;
            return CheckTypeSymbol ( cType , t1 ) ;
        }

        private static bool CheckTypeSymbol (
            INamedTypeSymbol          cType
          , params INamedTypeSymbol[] t1
        )
        {
            var r = t1.Any ( symbol => SymbolEqualityComparer.Default.Equals ( cType , symbol ) ) ;
            return r ;
        }

        public static bool IsException ( INamedTypeSymbol exceptionType , ITypeSymbol baseType )
        {
            if ( exceptionType == null )
            {
                return false ;
            }

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

        public static Tuple < bool , IMethodSymbol , SyntaxNode > CheckInvocationExpression (
                SyntaxNode n1
              , SemanticModel              currentModel
              , params INamedTypeSymbol[]  t
            )
        {
            try
            {
                if ( n1 is InvocationExpressionSyntax node )
                {
                    var symbolInfo = currentModel.GetSymbolInfo ( node.Expression ) ;
#if TRACE
                    Logger.Debug (
                                  "{method} node location is {node}"
                                , nameof ( CheckInvocationExpression )
                                , node.GetLocation ( ).ToString ( )
                                 ) ;
                    Logger.Debug (
                                  "{exprKind}, {expr}"
                                , node.Expression.Kind ( )
                                , node.Expression.ToString ( )
                                 ) ;

                    Logger.Info (
                                 "symbolinfo is {node}"
                               , symbolInfo.Symbol?.ToDisplayString ( ) ?? "null"
                                ) ;
                    if ( symbolInfo.Symbol == null )
                    {
                        Logger.Info ( "candidate symbols: {x}" , symbolInfo.CandidateSymbols ) ;
                    }
#endif

                    var methodSymbol = symbolInfo.Symbol as IMethodSymbol ;
                    var result = methodSymbol != null
                                 // TODO optmize
                                 && CheckSymbol ( methodSymbol , t ) ;
#if TRACE
                    Logger.Debug ( "result is {result}" , result ) ;
#endif
                    return Tuple.Create(result, methodSymbol, n1);
                }
                else if (n1 is ObjectCreationExpressionSyntax o)
                {
                    var symbolInfo = currentModel.GetSymbolInfo(o.Type);
                    var typeSymbol = symbolInfo.Symbol as INamedTypeSymbol;
                    var result = 
                                  CheckTypeSymbol(typeSymbol, t);
                    return Tuple.Create <bool, IMethodSymbol, SyntaxNode> ( result , null, n1 ) ;
                }

                throw new Exception ( "Error" ) ;

            }
            catch ( Exception ex )
            {
                throw ;
                // return Tuple.Create ( false , null , node  ) ;
            }
        }

        public static INamedTypeSymbol GetILoggerSymbol( [ NotNull ] SemanticModel model)
        {
            if ( model == null )
            {
                throw new ArgumentNullException ( nameof ( model ) ) ;
            }

            return model.Compilation.GetTypeByMetadataName(ILoggerClassFullName);
        }

        public static INamedTypeSymbol GetLogBuilderSymbol( [ NotNull ] SemanticModel model)
        {
            if ( model == null )
            {
                throw new ArgumentNullException ( nameof ( model ) ) ;
            }

            return model.Compilation.GetTypeByMetadataName(LogBuilderClassFullName);
        }
        public static INamedTypeSymbol GetNLogSymbol ( [ NotNull ] SemanticModel model )
        {
            if ( model == null )
            {
                throw new ArgumentNullException ( nameof ( model ) ) ;
            }

            return model.Compilation.GetTypeByMetadataName ( LoggerClassFullName ) ;
        }

        internal static ILogInvocation CreateLogInvocation (
            string                           sourceLocation
          , IMethodSymbol                    methodSymbol
          , LogMessageRepr                   msgVal
          , SyntaxNode                       relevantNode
          , SemanticModel                    semanticModel
          , CompilationUnitSyntax            o
          , ICodeSource                      codeSource
          , SyntaxTree                       ivpSyntaxTree
          , IList < ILogInvocationArgument > args
        )
        {
            return new LogInvocation2(sourceLocation, null, null, null, null, methodSymbol.ContainingType.MetadataName, methodSymbol.MetadataName, methodSymbol.ContainingType.MetadataName + "." + methodSymbol.MetadataName, args);
        }
    }

    [Serializable]
    public class MissingTypeException : Exception
    {
        public MissingTypeException ( ) { }

        public MissingTypeException ( string message ) : base ( message ) { }

        public MissingTypeException ( string message , Exception innerException ) : base (
                                                                                          message
                                                                                        , innerException
                                                                                         )
        {
        }

        protected MissingTypeException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}