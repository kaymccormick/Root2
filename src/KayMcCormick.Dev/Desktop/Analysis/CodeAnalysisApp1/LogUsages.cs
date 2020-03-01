using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using ProjLib ;

namespace CodeAnalysisApp1
{
    public static class LogUsages
    {
        private static  Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static void FindLogUsages(
            ICodeSource              document1
          , CompilationUnitSyntax currentRoot
          , SemanticModel         currentModel
          , Action < LogInvocation > consumeLogInvocation
          , bool                  limitToMarkedStatements
          , bool                  logVisitedStatements
          , Action < InvocationParms  > processInvocation,
            SyntaxTree syntaxTree
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
                // Logger.Trace(       
                             // "No {clas} in {document}"
                           // , LoggerClassFullName
                           // , document1
                            // ) ;
            }

            var t2 = comp.GetTypeByMetadataName ( ILoggerClassFullName ) ;
            if ( t2 == null )
            {
                return ;
            }

            var namespaceMembers = comp.GlobalNamespace.GetNamespaceMembers ( ) ;
            foreach ( var namespaceMember in namespaceMembers )
            {
                // Logger.Debug ( "{ns}" , namespaceMember.Name ) ;
            }

            var ns = namespaceMembers.Select ( symbol => symbol.MetadataName == NLogNamespace )
                                     .FirstOrDefault ( ) ;
            if ( ns == null )
            {
                Logger.Info (
                             "{0}"
                           , String.Join (
                                          ", "
                                        , namespaceMembers.Select ( symbol => symbol.Name )
                                         )
                            ) ;
                throw new InvalidOperationException ( "no " + NLogNamespace + " namespace" ) ;
            }
            // foreach ( IMethodSymbol method in methodSymbols)
            // {
            //     var x = Transforms.TransformMethodSymbol ( method ) ;
            //     new LogBuilder ( Logger )
            //        .Message ( "Method" )
            //        //.Properties ( x.ToDictionary ( ) )
            //        .Level ( LogLevel.Debug )
            //        .Write ( ) ;
            // }


            // Logger.Info("{t1}", t1);
            // Logger.Info("{t2}", t2);
            // foreach ( var s in comp.SyntaxTrees )
            // {
            //     Logger.Info ( "{count} {path}" , s.Length , s.FilePath ) ;
            // }
            // foreach ( var extRef in comp.ExternalReferences )
            // {
            //     var f = Path.GetFileName ( extRef.Display ) ;
            //     Logger.Info (
            //                  "{f} {compilationExternalReference_Display}"
            //                , f, extRef.Display
            //                 ) ;
            // }

            var qq0 =
                from node in currentRoot.DescendantNodesAndSelf ( ).OfType < StatementSyntax > ( )
                where limitToMarkedStatements == false
                      || node.GetLeadingTrivia ( )
                             .Any (
                                   trivia => trivia.Kind ( ) == SyntaxKind.SingleLineCommentTrivia
                                             && trivia.ToString ( ).Contains ( "doprocess" )
                                  )
                select node ;

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
                where CheckInvocationExpression(invocation, out methodSymbol1, currentModel)
                let methodSymbol = methodSymbol1
                select new { statement , invocation , methodSymbol } ;
            List < LogInvocation > minvocations = new List < LogInvocation > ();
            foreach ( var qqq in qxy )
            {
                try
                {
                    processInvocation (
                                       new InvocationParms( minvocations,currentRoot,currentModel,
                                       document1
                                     , qqq.statement
                                     , qqq.invocation
                                     , qqq.methodSymbol
                                     , ExceptionType, consumeLogInvocation
                    , syntaxTree
                                     )
                                      ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Warn ( ex , "unable to process invocation: {message}" , ex.Message ) ;
                }
            }

            return ;

        }

        public const string ILoggerClassName = "ILogger" ;
        public const string LoggerClassName = "Logger" ;
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
            Logger.Trace (
                          "{name} {ns} {r}"
                        , cType.MetadataName
                        , cType.ContainingNamespace.MetadataName
                        , r
                         ) ;
            return r ;
            return cType.MetadataName    == LogUsages.LoggerClassName
                   || cType.MetadataName == LogUsages.ILoggerClassName ; //  || methSym.Name == "Debug";
            return r ;
        }

        public static LogInvocation ProcessInvocation ( InvocationParms ivp )
        {
            var exceptionArg = IsException (
                                            ivp.Arg8
                                          , ivp.Arg7.Parameters.First ( ).Type
                                           ) ;
            var msgParam = ivp.Arg7.Parameters.Select ( ( symbol , i ) => new { symbol , i } )
                                       .Where ( arg1 => arg1.symbol.Name == "message" ) ;
            if ( ! msgParam.Any ( ) )
            {
                throw new NoMessageParameterException ( ) ;
            }

            var msgI = msgParam.First ( ).i ;
            var methodSymbol = ivp.Arg7 ;
            Logger.Error (
                          "params = {params}"
                        , String.Join (
                                       ", "
                                     , methodSymbol.Parameters.Select ( symbol => symbol.Name )
                                      )
                         ) ;
            var invocation = ivp.Arg6 ;
            var fargs = invocation.ArgumentList.Arguments.Skip ( msgI ) ;
            var rest = fargs.Skip ( 1 ) ;
            var msgarg = fargs.First ( ) ;
            var msgArgExpr = msgarg.Expression ;
            var semanticModel = ivp.Arg3 ;
            var msgArgTypeInfo = semanticModel.GetTypeInfo ( msgArgExpr ) ;
            var baseType = msgArgTypeInfo.Type ;
            var symbolInfo = semanticModel.GetSymbolInfo ( msgArgExpr ) ;
            var arg1sym = symbolInfo.Symbol ;
            if ( arg1sym != null )
            {
                Logger.Debug ( "{type} {symb}" , arg1sym.GetType ( ) , arg1sym ) ;
            }

            var constant = semanticModel.GetConstantValue ( msgArgExpr ) ;
            
            var             msgval = new LogMessageRepr ( ) ;
            if ( constant.HasValue )
            {
                msgval.IsMessageTemplate = true ;
                Logger.Warn ( "Constant {constant}" , constant.Value ) ;
                msgval.ConstantMessage = constant.Value ;
                var m = MessageTemplate.Parse ( ( string ) constant.Value ) ;
                var o = new List < object > ( ) ;
                msgval.MessageTemplate = m ;
                foreach ( var messageTemplateToken in m.Tokens )
                {
                    if ( messageTemplateToken is PropertyToken prop )
                    {
                        var t = Tuple.Create ( prop.IsPositional , prop.PropertyName ) ;
                        o.Add ( t ) ;
                    }
                    else if ( messageTemplateToken is TextToken t )
                    {
                        var xt = Tuple.Create ( t.Text ) ;
                        o.Add ( xt ) ;
                    }
                }


                Logger.Warn ( "{}" , String.Join ( ", " , o ) ) ;
            }
            else
            {
                Logger.Warn ( "{}" , msgArgExpr ) ;
                msgval.MessageExprPojo = Transforms.TransformExpr ( msgArgExpr ) ;
            }


            var document1 = ivp.Document ;
            var statementSyntax = ivp.Arg5 ;
            var sourceLocation = document1.FilePath
                                 + ":"
                                 + ( statementSyntax.GetLocation ( )
                                                    .GetMappedLineSpan ( )
                                                    .StartLinePosition.Line
                                     + 1 ) ;
            
            var debugInvo = new LogInvocation(sourceLocation, methodSymbol, msgval, statementSyntax, semanticModel, ivp.Arg2,   document1, ivp.SyntaxTree);
            
            var sourceContext = statementSyntax.Parent.ChildNodes ( ).ToList ( ) ;
            var i2 = sourceContext.IndexOf( statementSyntax ) ;
            string code = "" ;
            string p = statementSyntax.GetLocation ( ).GetMappedLineSpan ( ).Path ;
            try
            {
                string[] lines = File.ReadAllLines ( p ) ;
                debugInvo.PrecedingCode =
                    lines[ statementSyntax.GetLocation ( )
                                          .GetMappedLineSpan ( )
                                          .StartLinePosition.Line
                           - 1 ] ;

                debugInvo.Code = statementSyntax.ToFullString ( ) ;
                debugInvo.FollowingCode = lines[ statementSyntax.GetLocation ( )
                                                                .GetMappedLineSpan ( )
                                                                .EndLinePosition.Line
                                                 + 1 ] ;

            }
            catch ( Exception ex )
            {
                Logger.Warn(ex, ex.ToString());
            }

            debugInvo.SourceContext = code ;

            var transformed = rest.Select ( syntax => new LogInvocationArgument ( debugInvo, syntax ) ) ;
            debugInvo.Arguments = transformed.ToList ( ) ;
            Logger.Error ( "{t}" , transformed ) ;
            ivp.Arg1?.Add ( debugInvo ) ;
            ivp.Arg9?.Invoke( debugInvo ) ;
            return debugInvo ;
        }

        private static bool IsException ( INamedTypeSymbol exceptionType , ITypeSymbol baseType )
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

        public static bool CheckInvocationExpression (
            InvocationExpressionSyntax node
          , out IMethodSymbol          methodSymbol
          , SemanticModel              currentModel
        )
        {
            var symbolInfo = currentModel.GetSymbolInfo ( node.Expression ) ;
            var symbol = symbolInfo.Symbol ;
            methodSymbol = symbol as IMethodSymbol;
            return methodSymbol != null
                   && CheckSymbol (
                                   methodSymbol
                                 , GetNLogSymbol ( currentModel )
                                 , GetILoggerSymbol ( currentModel )
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

    public class NoMessageParameterException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
        public NoMessageParameterException ( ) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error. </param>
        public NoMessageParameterException ( string message ) : base ( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified. </param>
        public NoMessageParameterException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected NoMessageParameterException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}