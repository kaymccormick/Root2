using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Threading ;
using AnalysisFramework.Interfaces ;
using AnalysisFramework.LogUsage.Interfaces ;
using AnalysisFramework.SyntaxTransform ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisFramework.LogUsage
{
    public class InvocationParams
    {
        public SyntaxTree Tree { get ; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public InvocationParams (
            [ NotNull ] ICodeSource                     codeSoure
          , SyntaxTree                                  syntaxTree
          , SemanticModel                               model
          , SyntaxNode                                  relevantNode
          , Tuple < bool , IMethodSymbol , SyntaxNode > tuple
          , INamedTypeSymbol                            namedTypeSymbol
        )
        {
            if ( codeSoure == null )
            {
                throw new ArgumentNullException ( nameof ( codeSoure ) ) ;
            }

            if ( tuple is null )
            {
                throw new ArgumentNullException ( nameof ( tuple ) ) ;
            }

            if ( relevantNode != null )
            {
#if TRACE
                Logger.Debug (
                              "{id} relevant node is {node}"
                            , Thread.CurrentThread.ManagedThreadId
                            , relevantNode.ToString ( )
                             ) ;
#endif
                Tree = syntaxTree ?? throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;

                Model        = model ?? throw new ArgumentNullException ( nameof ( model ) ) ;
                ICodeSoure   = codeSoure ;
                RelevantNode = relevantNode ;
            }

            var (_ , item2 , item3) = tuple ;
            InvocationExpression    = item3 as InvocationExpressionSyntax ;
            MethodSymbol            = item2 ;
            NamedTypeSymbol = namedTypeSymbol
                              ?? throw new ArgumentNullException ( nameof ( namedTypeSymbol ) ) ;
        }

        public SemanticModel Model { get ; }

        public ICodeSource ICodeSoure { get ; }

        public SyntaxNode RelevantNode { get ; }

        public InvocationExpressionSyntax InvocationExpression { get ; }

        public IMethodSymbol MethodSymbol { get ; }

        public INamedTypeSymbol NamedTypeSymbol { get ; }

        public ILogInvocation ProcessInvocation ( )
        {
            var exceptionArg = false ;
            if ( NamedTypeSymbol != null
                 && MethodSymbol != null
                 && MethodSymbol.Parameters.Any ( ) )
            {
                exceptionArg = LogUsages.IsException (
                                                      NamedTypeSymbol
                                                    , MethodSymbol.Parameters.First ( ).Type
                                                     ) ;
            }

            if ( MethodSymbol != null )
            {
                var msgParam = MethodSymbol
                              .Parameters.Select ( ( symbol , i ) => new { symbol , i } )
                              .Where ( arg1 => arg1.symbol.Name == "message" ) ;
#if TRACE
                if ( ! msgParam.Any ( ) )
                {
                    Logger.Trace (
                                  "{params}"
                                , string.Join (
                                               ", "
                                             , MethodSymbol.Parameters.Select (
                                                                               symbol => symbol.Name
                                                                              )
                                              )
                                 ) ;
                }
#endif

                var msgI = msgParam.Any ( ) ? ( int ? ) msgParam.First ( ).i : null ;
                var methodSymbol = MethodSymbol ;
#if TRACE
                Logger.Trace (
                              "params = {params}"
                            , string.Join (
                                           ", "
                                         , methodSymbol.Parameters.Select ( symbol => symbol.Name )
                                          )
                             ) ;
#endif
                var invocation = InvocationExpression ;
                IEnumerable < ArgumentSyntax > rest ;
                var semanticModel = Model ;
                if ( msgI != null )
                {
                    var fargs = invocation.ArgumentList.Arguments.Skip ( msgI.Value ).ToList ( ) ;
                    rest = fargs.Skip ( 1 ) ;
                    var msgarg = fargs.First ( ) ;
                    var msgArgExpr = msgarg.Expression ;
                    var msgArgTypeInfo = semanticModel.GetTypeInfo ( msgArgExpr ) ;
                    var symbolInfo = semanticModel.GetSymbolInfo ( msgArgExpr ) ;
                    var arg1sym = symbolInfo.Symbol ;
#if TRACE
                    if ( arg1sym != null )
                    {
                        Logger.Trace ( "{type} {symb}" , arg1sym.GetType ( ) , arg1sym ) ;
                    }
#endif


                    var constant = semanticModel.GetConstantValue ( msgArgExpr ) ;

                    var msgval = new LogMessageRepr ( ) ;
                    if ( constant.HasValue )
                    {
                        msgval.IsMessageTemplate = true ;
#if TRACE
                        Logger.Trace ( "Constant {constant}" , constant.Value ) ;
#endif
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
#if TRACE
                        Logger.Debug ( "{}" , string.Join ( ", " , o ) ) ;
#endif
                    }
                    else
                    {
                        var t = new StringBuilder ( ) ;
                        //invocation.WithArgumentList(invocation.ArgumentList.)
                        if ( msgArgExpr is InterpolatedStringExpressionSyntax interp )
                        {
                            var n = 1 ;
                            foreach ( var s in interp.Contents )
                            {
                                if ( s is InterpolationSyntax expr )
                                {
                                    var varName = "arg" + n ;
                                    if ( expr.Expression is IdentifierNameSyntax nn )
                                    {
                                        varName = nn.Identifier.ValueText ;
                                    }

                                    //expr.Expression.
                                }
                            }
                        }
#if TRACE
                        Logger.Trace ( "{}" , msgArgExpr ) ;
#endif
                        msgval.MessageExprPojo = Transforms.TransformExpr ( msgArgExpr ) ;
                    }
                }
                else
                {
                    rest = invocation.ArgumentList.Arguments ;
                }

                var codeSource = ICodeSoure ;
                var relevantNode = RelevantNode ;
                var sourceLocation = codeSource.FilePath
                                     + ":"
                                     + ( relevantNode.GetLocation ( )
                                                     .GetMappedLineSpan ( )
                                                     .StartLinePosition.Line
                                         + 1 ) ;

                var debugInvo = LogUsages.CreateLogInvocation (
                                                               sourceLocation
                                                             , methodSymbol
                                                             , relevantNode
                                                             , semanticModel
                                                             , null
                                                              ) ;
                var sourceContext = relevantNode.Parent.ChildNodes ( ).ToList ( ) ;
                var i2 = sourceContext.IndexOf ( relevantNode ) ;

                var p = relevantNode.GetLocation ( ).GetMappedLineSpan ( ).Path ;
                try
                {
                    var lines = File.ReadAllLines ( p ) ;
                    debugInvo.PrecedingCode =
                        lines[ relevantNode.GetLocation ( )
                                           .GetMappedLineSpan ( )
                                           .StartLinePosition.Line
                               - 1 ] ;

                    debugInvo.Code = relevantNode.ToFullString ( ) ;
                    debugInvo.FollowingCode = lines[ relevantNode.GetLocation ( )
                                                                 .GetMappedLineSpan ( )
                                                                 .EndLinePosition.Line
                                                     + 1 ] ;
                }
                catch ( Exception ex )
                {
                    Logger.Warn ( ex , ex.ToString ( ) ) ;
                }

                var transformed = rest.Select (
                                               syntax
                                                   => ( ILogInvocationArgument )
                                                   new LogInvocationArgument ( syntax )
                                              ) ;
                foreach ( var logInvocationArgument in transformed )
                {
                    debugInvo.Arguments.Add ( logInvocationArgument ) ;
                }
#if TRACE
                Logger.Trace ( "{t}" , transformed ) ;
#endif
                return debugInvo ;
            }

            return null ;
        }
    }
}