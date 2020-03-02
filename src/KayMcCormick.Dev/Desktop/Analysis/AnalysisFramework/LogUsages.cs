using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisFramework
{
    public static class LogUsages
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static void FindLogUsages(
            ICodeSource                 document1
          , CompilationUnitSyntax       currentRoot
          , SemanticModel               currentModel
          , Action < LogInvocation >    consumeLogInvocation
          , bool                        limitToMarkedStatements
          , bool                        logVisitedStatements
          , Action < InvocationParms  > processInvocation,
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
            // Logger.Trace (
                          // "{name} {ns} {r}"
                        // , cType.MetadataName
                        // , cType.ContainingNamespace.MetadataName
                        // , r
                         // ) ;
            return r ;
            return cType.MetadataName    == LogUsages.LoggerClassName
                   || cType.MetadataName == LogUsages.ILoggerClassName ; //  || methSym.Name == "Debug";
            return r ;
        }

        public static LogInvocation ProcessInvocation ( InvocationParms ivp )
        {
            var exceptionArg = IsException (
                                            ivp.NamedTypeSymbol
                                          , ivp.MethodSymbol.Parameters.First ( ).Type
                                           ) ;
            var msgParam = ivp.MethodSymbol.Parameters.Select ( ( symbol , i ) => new { symbol , i } )
                              .Where ( arg1 => arg1.symbol.Name == "message" ) ;
            if ( ! msgParam.Any ( ) )
            {
                Logger.Info ( "{params}", string.Join(", ", ivp.MethodSymbol.Parameters.Select ( symbol => symbol.Name )) ) ;
            }

            int ? msgI = msgParam.Any ( ) ? (int?) msgParam.First ( ).i : null ;
            var methodSymbol = ivp.MethodSymbol ;
            Logger.Debug (
                          "params = {params}"
                        , String.Join (
                                       ", "
                                     , methodSymbol.Parameters.Select ( symbol => symbol.Name )
                                      )
                         ) ;
            var invocation = ivp.InvocationExpression ;
            IEnumerable < ArgumentSyntax > rest ;
            var semanticModel = ivp.Model;
            LogMessageRepr msgval = null ;
            if ( msgI != null )
            {
                var fargs = invocation.ArgumentList.Arguments.Skip ( msgI.Value ) ;
                rest = fargs.Skip ( 1 ) ;
                var msgarg = fargs.First();
                var msgArgExpr = msgarg.Expression;
                var msgArgTypeInfo = semanticModel.GetTypeInfo(msgArgExpr);
                var baseType = msgArgTypeInfo.Type;
                var symbolInfo = semanticModel.GetSymbolInfo(msgArgExpr);
                var arg1sym = symbolInfo.Symbol;
                if (arg1sym != null)
                {
                    Logger.Debug("{type} {symb}", arg1sym.GetType(), arg1sym);
                }

                var constant = semanticModel.GetConstantValue(msgArgExpr);

                msgval = new LogMessageRepr() ;
                if ( constant.HasValue )
                {
                    msgval.IsMessageTemplate = true ;
                    Logger.Debug ( "Constant {constant}" , constant.Value ) ;
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
                    Logger.Debug("{}", String.Join(", ", o));
                }


                else
                    {
                        Logger.Debug("{}", msgArgExpr);
                        msgval.MessageExprPojo = Transforms.TransformExpr(msgArgExpr);
                    }

            }
            else
            {
                rest = invocation.ArgumentList.Arguments ;
            }

            

            var document1 = ivp.Document ;
            var statementSyntax = ivp.Statement ;
            var sourceLocation = document1.FilePath
                                 + ":"
                                 + ( statementSyntax.GetLocation ( )
                                                    .GetMappedLineSpan ( )
                                                    .StartLinePosition.Line
                                     + 1 ) ;
            
            var debugInvo = new LogInvocation(sourceLocation, methodSymbol, msgval, statementSyntax, semanticModel, ivp.Arg2, document1, ivp.SyntaxTree);
            
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
            Logger.Debug ( "{t}" , transformed ) ;
            ivp.Arg1?.Add ( debugInvo ) ;
            ivp.ConsumeAction?.Invoke( debugInvo ) ;
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
}