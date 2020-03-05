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
        public static IEnumerable<LogInvocation> FindLogUsages(
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
            var qxy =
                from statement in qq0
                let invocations =
                    statement.DescendantNodes (
                                               node => node == statement
                                                       || ! ( node is StatementSyntax )
                                              )
                             .OfType < InvocationExpressionSyntax > ( )
                from invocation in invocations
                let @out = CheckInvocationExpression(invocation, currentModel)
                let methodSymbol = methodSymbol1
                select new { statement , invocation , methodSymbol } ;
            
            foreach ( var qqq in qxy )
            {
                try
                {

                    InvocationParms.ProcessInvocation(
                                       new InvocationParms(
                                                           document1
                                                         , syntaxTree
                                                         , currentModel
                                                          , qqq.statement
                                                          , qqq.invocation
                                                          , qqq.methodSymbol
                                                          , ExceptionType
                                                          )
                                      ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Warn ( ex , "unable to process invocation: {message}" , ex.Message ) ;
                }
            }

            return null ;


        }

        public const            string ILoggerClassName    = "ILogger" ;
        public const            string LoggerClassName     = "Logger" ;
        private static readonly string LoggerClassFullName = NLogNamespace + '.' + LoggerClassName ;

        public static readonly string
            ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

        private const string NLogNamespace = "NLog" ;


        private static bool CheckSymbol (
            IMethodSymbol    methSym
          , INamedTypeSymbol t1
          , INamedTypeSymbol t2
        )
        {
            var cType = methSym.ContainingType ;

            var r = cType == t1 || cType == t2 ;
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
        )
        {
            var symbolInfo = currentModel.GetSymbolInfo ( node.Expression ) ;
            var methodSymbol = symbolInfo.Symbol as IMethodSymbol;
            return Tuple.Create (
                                 methodSymbol != null
                                 // TODO optmize
                                 && CheckSymbol (
                                                 methodSymbol
                                               , GetNLogSymbol ( currentModel )
                                               , GetILoggerSymbol ( currentModel )
                                                )
                               , methodSymbol
                               , node
                                ) ;
        }

        private static INamedTypeSymbol GetILoggerSymbol ( SemanticModel model )
        {
            return model.Compilation.GetTypeByMetadataName(ILoggerClassFullName);
        }

        private static INamedTypeSymbol GetNLogSymbol ( SemanticModel model )
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