using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;


namespace FindLogUsages
{
    /// <summary>
    /// </summary>
    public class FindLogUsagesMain
    {
#if DOLOG
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#endif

        private readonly Func < ILogInvocation > _invocationFactory ;

        /// <summary>
        /// </summary>
        /// <param name="invocationFactory"></param>
        public FindLogUsagesMain ( Func < ILogInvocation > invocationFactory )
        {
            _invocationFactory = invocationFactory ;
        }


        /// <summary>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="rejectBlock"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ItemNotNull]
        public async Task<IEnumerable<ILogInvocation>> FindUsagesFuncAsync(
            [NotNull] Document d
          , BufferBlock<RejectedItem> rejectBlock
        )
        {
            using (
#if DOLOG
                MappedDiagnosticsLogicalContext.SetScoped("Document", d.FilePath)
#else
                new EmptyDisposable()
#endif
            )
            {
                try
                {
#if TRACE && DOLOG
                    Logger.Trace (
                                  "[{id}] Entering {funcName}"
                        , Thread.CurrentThread.ManagedThreadId
                        , nameof ( FindUsagesFuncAsync )
                                 ) ;
#endif
                    var tree = await d.GetSyntaxTreeAsync().ConfigureAwait(true);
                    var root = (tree ?? throw new InvalidOperationException())
                       .GetCompilationUnitRoot();
                    var model = await d.GetSemanticModelAsync().ConfigureAwait(true);

                    if (model != null)
                    {
                        return await Process(
                                              _invocationFactory
                                            , model
                                            , tree
                                            , root
                       , rejectBlock.Post
                                             );
                    }
                }
                catch (Exception ex)
                {
#if DOLOG
                    Logger.Debug ( ex , ex.ToString ( ) ) ;
#endif
                    throw;
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// </summary>
        /// <param name="invocationFactory"></param>
        /// <param name="model"></param>
        /// <param name="tree"></param>
        /// <param name="root"></param>
        /// <param name="RejectAction"></param>
        /// <returns></returns>
        public static async Task < IEnumerable < ILogInvocation > > Process (
            Func < ILogInvocation >      invocationFactory
          , SemanticModel                model
          , SyntaxTree                   tree
          , SyntaxNode                   root
          , Func < RejectedItem , bool > RejectAction
        )
        {
            var exceptionType = model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;
            var t = LogUsages.GetNLogSymbol ( model ) ;
            if ( t == null )
            {
                return Array.Empty < ILogInvocation > ( ) ;
            }

            var t2 = LogUsages.GetILoggerSymbol ( model ) ;
            if ( t2 == null )
            {
                return Array.Empty < ILogInvocation > ( ) ;
            }

            var logBuilderSymbol = LogUsages.GetLogBuilderSymbol ( model ) ;
            return Enumerable.OfType < ILogInvocation > (
                                            (
                                                from node in root.DescendantNodes ( ) //.AsParallel ( )
                                                let t_ = t
                                                let t2_ = t2
                                                let builderSymbol = logBuilderSymbol
                                                let tree_ = tree
                                                let model_ = model
                                                let exType = exceptionType
                                                where node.RawKind    == ( ushort ) SyntaxKind.InvocationExpression
                                                      || node.RawKind == ( ushort ) SyntaxKind.ObjectCreationExpression
                                                let @out =
                                                    LogUsages.CheckInvocationExpression (
                                                                                         node
                                                                                       , model_
                                                                                       , builderSymbol
                                                                                       , t_
                                                                                       , t2_
                                                                                        )
                                                where @out.Item1
                                                let statement = node.AncestorsAndSelf ( ).Where ( Predicate ).First ( )
                                                let result = new InvocationParams (
                                                                                   tree_
                                                                                 , model_
                                                                                 , statement
                                                                                 , @out
                                                                                 , exType
                                                                                  ).ProcessInvocation ( invocationFactory )
                                                select result is ILogInvocation inv
                                                           ? inv
                                                           : ( object ) RejectAction (
                                                                                      result is RejectedItem rj
                                                                                          ? rj
                                                                                          : new RejectedItem ( statement )
                                                                                     ) )
                                           ) ;
        }

        private static bool Predicate ( [ NotNull ] SyntaxNode arg1 , int arg2 )
        {
            var b = arg1 is StatementSyntax || arg1 is MemberDeclarationSyntax ;
#if TRACE && DOLOG
            Logger.Debug ( "Got {arg1} - {b}" , arg1.Kind ( ) , b ) ;
#endif
            if ( b )
            {
                return true ;
            }

            return false ;
        }

        private static class LogUsages
        {
            public const string LogBuilderClassName     = "LogBuilder" ;
            public const string LogBuilderNamespaceName = NLogNamespace + ".Fluent" ;

            public const string LogBuilderClassFullName =
                LogBuilderNamespaceName + "." + LogBuilderClassName ;

            public const string ILoggerClassName = "ILogger" ;
            public const string LoggerClassName  = "Logger" ;

            public const string ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

            private const string NLogNamespace = "NLog" ;

#if DOLOG
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#endif

            private static readonly string LoggerClassFullName =
                NLogNamespace + '.' + LoggerClassName ;


            private static bool CheckSymbol (
                [ NotNull ]        IMethodSymbol      methSym
              , [ NotNull ] params INamedTypeSymbol[] t1
            )
            {
                var cType = methSym.ContainingType ;
                return CheckTypeSymbol ( cType , t1 ) ;
            }

            private static bool CheckTypeSymbol (
                INamedTypeSymbol                      cType
              , [ NotNull ] params INamedTypeSymbol[] t1
            )
            {
                var r = t1.Any (
                                symbol => SymbolEqualityComparer.Default.Equals ( cType , symbol )
                               ) ;
                return r ;
            }


            [ NotNull ]
            public static Tuple < bool , IMethodSymbol , SyntaxNode > CheckInvocationExpression (
                [ NotNull ] SyntaxNode         n1
              , [ NotNull ] SemanticModel      currentModel
              , params      INamedTypeSymbol[] t
            )
            {
                if ( n1 is InvocationExpressionSyntax node )
                {
                    var symbolInfo =
                        ModelExtensions.GetSymbolInfo ( currentModel , node.Expression ) ;
#if TRACE && DOLOG
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
                                 "symbol info is {node}"
                       , symbolInfo.Symbol?.ToDisplayString ( ) ?? "null"
                                ) ;
                    if ( symbolInfo.Symbol == null )
                    {
                        Logger.Info ( "candidate symbols: {x}" , symbolInfo.CandidateSymbols ) ;
                    }
#endif

                    var methodSymbol = symbolInfo.Symbol as IMethodSymbol ;
                    var result = methodSymbol != null
                                 // TODO optimize
                                 && CheckSymbol ( methodSymbol , t ) ;
#if TRACE && DOLOG
                    Logger.Debug ( "result is {result}" , result ) ;

#endif
                    return Tuple.Create ( result , methodSymbol , n1 ) ;
                }

                if ( n1 is ObjectCreationExpressionSyntax o )
                {
                    var symbolInfo = ModelExtensions.GetSymbolInfo ( currentModel , o.Type ) ;
                    var typeSymbol = symbolInfo.Symbol as INamedTypeSymbol ;
                    var result = CheckTypeSymbol ( typeSymbol , t ) ;
                    return Tuple.Create < bool , IMethodSymbol , SyntaxNode > (
                                                                               result
                                                                             , null
                                                                             , n1
                                                                              ) ;
                }


                throw new Exception ( "Error" ) ;
            }

            [ CanBeNull ]
            public static INamedTypeSymbol GetILoggerSymbol ( [ NotNull ] SemanticModel model )
            {
                if ( model == null )
                {
                    throw new ArgumentNullException ( nameof ( model ) ) ;
                }

                return model.Compilation.GetTypeByMetadataName ( ILoggerClassFullName ) ;
            }

            [ CanBeNull ]
            public static INamedTypeSymbol GetLogBuilderSymbol ( [ NotNull ] SemanticModel model )
            {
                if ( model == null )
                {
                    throw new ArgumentNullException ( nameof ( model ) ) ;
                }

                return model.Compilation.GetTypeByMetadataName ( LogBuilderClassFullName ) ;
            }

            [ CanBeNull ]
            public static INamedTypeSymbol GetNLogSymbol ( [ NotNull ] SemanticModel model )
            {
                if ( model == null )
                {
                    throw new ArgumentNullException ( nameof ( model ) ) ;
                }

                return model.Compilation.GetTypeByMetadataName ( LoggerClassFullName ) ;
            }
        }

        internal class InvocationParams
        {
#if DOLOG
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#endif

            public InvocationParams (
                SyntaxTree                                                syntaxTree
              , SemanticModel                                             model
              , [ CanBeNull ] SyntaxNode                                  relevantNode
              , [ NotNull ]   Tuple < bool , IMethodSymbol , SyntaxNode > tuple
              , [ NotNull ]   INamedTypeSymbol                            namedTypeSymbol
            )
            {
                if ( tuple is null )
                {
                    throw new ArgumentNullException ( nameof ( tuple ) ) ;
                }

                if ( relevantNode != null )
                {
#if TRACE && DOLOG
                    Logger.Debug (
                                  "{id} relevant node is {node}"
                        , Thread.CurrentThread.ManagedThreadId
                        , relevantNode.ToString ( )
                                 ) ;
#endif
                    Tree = syntaxTree ?? throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;

                    Model        = model ?? throw new ArgumentNullException ( nameof ( model ) ) ;
                    RelevantNode = relevantNode ;
                }

                var (_ , item2 , item3) = tuple ;
                InvocationExpression    = item3 as InvocationExpressionSyntax ;
                MethodSymbol            = item2 ;
                NamedTypeSymbol = namedTypeSymbol
                                  ?? throw new ArgumentNullException (
                                                                      nameof ( namedTypeSymbol )
                                                                     ) ;
            }

            private SyntaxTree Tree { get ; }

            private SemanticModel Model { get ; }

            private SyntaxNode RelevantNode { get ; }

            private InvocationExpressionSyntax InvocationExpression { get ; }

            private IMethodSymbol MethodSymbol { get ; }

            private INamedTypeSymbol NamedTypeSymbol { get ; }

            [ CanBeNull ]
            internal object ProcessInvocation ( Func < ILogInvocation > invocationFactory )
            {
                var exceptionArg = false ;
                if ( NamedTypeSymbol != null
                     && MethodSymbol != null
                     && MethodSymbol.Parameters.Any ( ) )
                {
                    exceptionArg = IsException (
                                                NamedTypeSymbol
                                              , MethodSymbol.Parameters.First ( ).Type
                                               ) ;
                }

                if ( MethodSymbol == null )
                {
                    return null ;
                }

                var msgParam = Enumerable.Select (
                                                  MethodSymbol
                                                     .Parameters
                                                , ( symbol , i ) => new { symbol , i } )
                                         .Where ( arg1 => arg1.symbol.Name == "message" ) ;
#if TRACE && DOLOG
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
#if TRACE && DOLOG
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
                    var msgArgTypeInfo =
                        ModelExtensions.GetTypeInfo ( semanticModel , msgArgExpr ) ;
                    var symbolInfo = ModelExtensions.GetSymbolInfo ( semanticModel , msgArgExpr ) ;
                    var arg1sym = symbolInfo.Symbol ;
#if TRACE && DOLOG
                    if ( arg1sym != null )
                    {
                        Logger.Trace (
                                      "{type} {symb}"
                            , arg1sym.GetType ( )
                            , arg1sym?.ToDisplayString ( )
                                     ) ;
                    }
#endif


                    var constant = semanticModel.GetConstantValue ( msgArgExpr ) ;

                    var msgval = new LogMessageRepr ( ) ;
                    if ( constant.HasValue )
                    {
                        msgval.IsMessageTemplate = true ;
#if TRACE && DOLOG
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
                                var t = Tuple.Create < bool , string > ( prop.IsPositional , prop.PropertyName ) ;
                                o.Add ( t ) ;
                            }
                            else if ( messageTemplateToken is TextToken t )
                            {
                                var xt = Tuple.Create < string > ( t.Text ) ;
                                o.Add ( xt ) ;
                            }
                        }
#if TRACE && DOLOG
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
#if TRACE && DOLOG
                        Logger.Trace ( "{}" , msgArgExpr ) ;
#endif
                        //msgval.MessageExprPojo = Transforms.TransformExpr ( msgArgExpr ) ;
                    }
                }
                else
                {
                    rest = invocation.ArgumentList.Arguments ;
                }

                var relevantNode = RelevantNode ;
                var location = relevantNode.GetLocation ( ) ;
                var sourceLocation = Tree.FilePath
                                     + ":"
                                     + ( location.GetMappedLineSpan ( ).StartLinePosition.Line
                                         + 1 ) ;

                object t1 ;
                try
                {
                    t1 = GenTransforms.Transform_CSharp_Node (( CSharpSyntaxNode ) relevantNode);         
                }
                catch ( UnsupportedExpressionTypeSyntaxException unsupported )
                {
                    return new RejectedItem ( relevantNode , unsupported ) ;
                }

                var debugInvo = invocationFactory ( ) ;
                debugInvo.Location       = location ;
                debugInvo.SourceLocation = sourceLocation ;
                debugInvo.LoggerType     = methodSymbol.ContainingType.MetadataName ;
                debugInvo.MethodDisplayName = methodSymbol.ContainingType.MetadataName
                                              + "."
                                              + methodSymbol.MetadataName ;
                debugInvo.TransformedRelevantNode = t1 ;
                if ( relevantNode.Parent != null )
                {
                    var sourceContext = relevantNode.Parent.ChildNodes ( ).ToList ( ) ;
                    var i2 = sourceContext.IndexOf ( relevantNode ) ;
                }

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
#if DOLOG
                    Logger.Warn ( ex , ex.ToString ( ) ) ;
#endif
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
#if TRACE && DOLOG
                Logger.Trace ( "{t}" , transformed ) ;
#endif
                return debugInvo ;
            }

            private static bool IsException (
                [ CanBeNull ] INamedTypeSymbol exceptionType
              , ITypeSymbol                    baseType
            )
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

            private sealed class LogMessageRepr
            {
                public LogMessageRepr ( bool isMessageTemplate , object constantMessage )
                {
                    IsMessageTemplate = isMessageTemplate ;
                    ConstantMessage   = constantMessage ;
                    if ( IsMessageTemplate )
                    {
                        MessageTemplate = MessageTemplate.Parse ( constantMessage.ToString ( ) ) ;
                    }
                }

                public LogMessageRepr ( ) { }

                public MessageTemplate MessageTemplate { get ; set ; }

                public bool IsMessageTemplate { get ; set ; }

                public object MessageExprPojo { get ; set ; }

                public object PrimaryMessage
                {
                    get
                    {
                        return IsMessageTemplate ? MessageTemplate?.Text ?? "" : MessageExprPojo ;
                    }
                }

                public object ConstantMessage { get ; set ; }
            }
        }
    }
}