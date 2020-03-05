﻿using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisFramework
{
    public class InvocationParms
    {
        public readonly SyntaxTree SyntaxTree ;
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public InvocationParms (
            ICodeSource                codeSoure
          , SyntaxTree                 syntaxTree
          , SemanticModel              model
          , StatementSyntax            statement
            , Tuple<bool, IMethodSymbol, InvocationExpressionSyntax> tuple
          , INamedTypeSymbol           namedTypeSymbol

        )
        {
            SyntaxTree = syntaxTree ;

            Model = model ;
            ICodeSoure = codeSoure ;
            Statement = statement ;
            var (_ , item2 , item3) = tuple ;
            InvocationExpression = item3;
            MethodSymbol = item2;
            NamedTypeSymbol = namedTypeSymbol ;
        }

        public InvocationParms ( ICodeSource codeSoure , SyntaxTree syntaxTree , SemanticModel currentModel , StatementSyntax statement , InvocationExpressionSyntax invocation , IMethodSymbol namedTypeSymbol , INamedTypeSymbol exceptionType ) { }

        public SemanticModel Model { get ; private set ; }

        public ICodeSource ICodeSoure { get ; private set ; }

        public StatementSyntax Statement { get ; private set ; }

        public InvocationExpressionSyntax InvocationExpression { get ; private set ; }

        public IMethodSymbol MethodSymbol { get ; private set ; }

        public INamedTypeSymbol NamedTypeSymbol { get ; private set ; }

        public static ILogInvocation ProcessInvocation (  InvocationParms ivp )
        {
            var exceptionArg = LogUsages.IsException (
                                                      ivp.NamedTypeSymbol
                                                    , ivp.MethodSymbol.Parameters.First ( ).Type
                                                     ) ;
            var msgParam = ivp.MethodSymbol.Parameters.Select ( ( symbol , i ) => new { symbol , i } )
                              .Where ( arg1 => arg1.symbol.Name == "message" ) ;
            if ( ! msgParam.Any ( ) )
            {
                Logger.Info ( "{params}", String.Join(", ", ivp.MethodSymbol.Parameters.Select ( symbol => symbol.Name )) ) ;
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
                    StringBuilder t = new StringBuilder();
                    //invocation.WithArgumentList(invocation.ArgumentList.)
                    if ( msgArgExpr is InterpolatedStringExpressionSyntax interp )
                    {
                        int n = 1 ;
                        foreach ( var s in interp.Contents )
                        {
                            if ( s is InterpolationSyntax expr )
                            {
                                string varName = "arg" + n.ToString();
                                if(expr.Expression is IdentifierNameSyntax nn)
                                {
                                    varName = nn.Identifier.ValueText ;
                                }
                                  
                                //expr.Expression.
                            }
                        }
                    }
                    Logger.Debug("{}", msgArgExpr);
                    msgval.MessageExprPojo = Transforms.TransformExpr(msgArgExpr);
                }

            }
            else
            {
                rest = invocation.ArgumentList.Arguments ;
            }

            var codeSource = ivp.ICodeSoure ;
            var statementSyntax = ivp.Statement ;
            var sourceLocation = codeSource.FilePath
                                 + ":"
                                 + ( statementSyntax.GetLocation ( )
                                                    .GetMappedLineSpan ( )
                                                    .StartLinePosition.Line
                                     + 1 ) ;
            
            var debugInvo = new LogInvocation(sourceLocation, methodSymbol, msgval, statementSyntax, semanticModel, null, codeSource, ivp.SyntaxTree);
            
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

            var transformed = rest.Select ( syntax => (ILogInvocationArgument)(new LogInvocationArgument ( debugInvo, syntax )) ) ;
            debugInvo.Arguments = transformed.ToList ( ) ;
            Logger.Debug ( "{t}" , transformed ) ;
            return debugInvo ;
        }
    }
}