using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using PropertyToken = MessageTemplates.Parsing.PropertyToken ;

// ReSharper disable InconsistentNaming


namespace FindLogUsages
{
    /// <summary>
    /// </summary>
    public sealed class FindLogUsagesMain
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
        /// <param name="invocActions"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ ItemNotNull ]
        public async Task < IEnumerable < ILogInvocation > > FindUsagesFuncAsync (
            [ NotNull ] Document                      d
          , BufferBlock < RejectedItem >              rejectBlock
          , IEnumerable < Action < ILogInvocation > > invocActions
        )
        {
            using (
#if DOLOG
                MappedDiagnosticsLogicalContext.SetScoped("Document", d.FilePath)
#else
                new EmptyDisposable ( )
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
                    var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
                    var root = ( tree ?? throw new InvalidOperationException ( ) )
                       .GetCompilationUnitRoot ( ) ;
                    var model = await d.GetSemanticModelAsync ( ).ConfigureAwait ( true ) ;

                    if ( model != null )
                    {
                        return await Process (
                                              _invocationFactory
                                            , model
                                            , tree
                                            , root
                       ,                      rejectBlock.Post
                       ,                      invocActions
                                             ) ;
                    }
                }
                // ReSharper disable once RedundantCatchClause
                catch ( Exception )
                {
#if DOLOG
                    Logger.Debug ( ex , ex.ToString ( ) ) ;
#endif
                    throw ;
                }
            }

            throw new InvalidOperationException ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="invocationFactory"></param>
        /// <param name="model"></param>
        /// <param name="tree"></param>
        /// <param name="root"></param>
        /// <param name="rejectAction"></param>
        /// <param name="invocActions"></param>
        /// <returns></returns>
#pragma warning disable 1998
        [ ItemNotNull ]
        public static async Task < IEnumerable < ILogInvocation > > Process (
#pragma warning restore 1998
            Func < ILogInvocation >                   invocationFactory
          , [ NotNull ] SemanticModel                 model
          , SyntaxTree                                tree
          , SyntaxNode                                root
          , Func < RejectedItem , bool >              rejectAction
          , IEnumerable < Action < ILogInvocation > > invocActions
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
            return (
                       from node in root.DescendantNodes ( ).AsParallel ( )
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
                                                         ).ProcessInvocation (
                                                                              invocationFactory
                                                                            , invocActions
                                                                             )
                       select result is ILogInvocation inv
                                  ? inv
                                  : ( object ) rejectAction (
                                                             result is RejectedItem rj
                                                                 ? rj
                                                                 : new RejectedItem ( statement )
                                                            ) ).OfType < ILogInvocation > ( ) ;
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
            private const string LogBuilderClassName     = "LogBuilder" ;
            private const string LogBuilderNamespaceName = NLogNamespace + ".Fluent" ;

            private const string LogBuilderClassFullName =
                LogBuilderNamespaceName + "." + LogBuilderClassName ;

            private const string ILoggerClassName = "ILogger" ;
            private const string LoggerClassName  = "Logger" ;

            private const string ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

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

        internal sealed class InvocationParams
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
            // ReSharper disable once FunctionComplexityOverflow
            internal object ProcessInvocation (
                Func < ILogInvocation >                   invocationFactory
              , IEnumerable < Action < ILogInvocation > > invocActions
            )
            {
                // ReSharper disable once NotAccessedVariable
                var exceptionArg = false ;
                if ( NamedTypeSymbol                     != null
                     && MethodSymbol?.Parameters.Any ( ) == true )
                {
                    // ReSharper disable once RedundantAssignment
                    exceptionArg = IsException (
                                                NamedTypeSymbol
                                              , MethodSymbol.Parameters.First ( ).Type
                                               ) ;
                }

                if ( MethodSymbol == null )
                {
                    return null ;
                }

                var msgParam = MethodSymbol
                              .Parameters.Select ( ( symbol , i ) => new { symbol , i } )
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

                // ReSharper disable twice PossibleMultipleEnumeration
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
                    var msgArg = fargs.First ( ) ;
                    var msgArgExpr = msgArg.Expression ;
                    // ReSharper disable once UnusedVariable
                    var msgArgTypeInfo =
                        ModelExtensions.GetTypeInfo ( semanticModel , msgArgExpr ) ;
                    var symbolInfo = ModelExtensions.GetSymbolInfo ( semanticModel , msgArgExpr ) ;
                    // ReSharper disable once UnusedVariable
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
                        // ReSharper disable once CollectionNeverQueried.Local
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
#if TRACE && DOLOG
                        Logger.Debug ( "{}" , string.Join ( ", " , o ) ) ;
#endif
                    }
                    else
                    {
                        //invocation.WithArgumentList(invocation.ArgumentList.)
                        if ( msgArgExpr is InterpolatedStringExpressionSyntax interpolated )
                        {
                            var n = 1 ;
                            foreach ( var s in interpolated.Contents )
                            {
                                if ( s is InterpolationSyntax expr )
                                {
                                    // ReSharper disable once NotAccessedVariable
                                    var varName = "arg" + n ;
                                    if ( expr.Expression is IdentifierNameSyntax nn )
                                    {
                                        // ReSharper disable once RedundantAssignment
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
                    t1 = GenTransforms.Transform_CSharp_Node ( ( CSharpSyntaxNode ) relevantNode ) ;
                }
                catch ( UnsupportedExpressionTypeSyntaxException unsupported )
                {
                    return new RejectedItem ( relevantNode , unsupported ) ;
                }

                var invocation2 = invocationFactory ( ) ;
                invocation2.Location       = location ;
                invocation2.SourceLocation = sourceLocation ;
                invocation2.LoggerType     = methodSymbol.ContainingType.MetadataName ;
                invocation2.MethodDisplayName = methodSymbol.ContainingType.MetadataName
                                              + "."
                                              + methodSymbol.MetadataName ;
                invocation2.TransformedRelevantNode = t1 ;
                var sourceContext = relevantNode.Parent?.ChildNodes ( ).ToList ( ) ;
                // ReSharper disable once UnusedVariable
                var i2 = sourceContext?.IndexOf ( relevantNode ) ;

                var p = relevantNode.GetLocation ( ).GetMappedLineSpan ( ).Path ;
                try
                {
                    var lines = File.ReadAllLines ( p ) ;
                    invocation2.PrecedingCode =
                        lines[ relevantNode.GetLocation ( )
                                           .GetMappedLineSpan ( )
                                           .StartLinePosition.Line
                               - 1 ] ;

                    invocation2.Code = relevantNode.ToFullString ( ) ;
                    invocation2.FollowingCode = lines[ relevantNode.GetLocation ( )
                                                                 .GetMappedLineSpan ( )
                                                                 .EndLinePosition.Line
                                                     + 1 ] ;
                }
                catch ( Exception )
                {
                    // ignored
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
                    invocation2.Arguments.Add ( logInvocationArgument ) ;
                }
#if TRACE && DOLOG
                Logger.Trace ( "{t}" , transformed ) ;
#endif
                if ( invocActions != null )
                {
                    foreach ( var invocAction in invocActions )
                    {
                        invocAction ( invocation2 ) ;
                    }
                }

                return invocation2 ;
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
                // ReSharper disable once UnusedMember.Local
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

                // ReSharper disable once UnusedAutoPropertyAccessor.Local
                // ReSharper disable once MemberCanBePrivate.Local
                public object MessageExprPojo { get ; set ; }

                // ReSharper disable once UnusedMember.Local
                public object PrimaryMessage
                {
                    get
                    {
                        return IsMessageTemplate ? MessageTemplate?.Text ?? "" : MessageExprPojo ;
                    }
                }

                // ReSharper disable once UnusedAutoPropertyAccessor.Local
                public object ConstantMessage { get ; set ; }
            }
        }
    }
}