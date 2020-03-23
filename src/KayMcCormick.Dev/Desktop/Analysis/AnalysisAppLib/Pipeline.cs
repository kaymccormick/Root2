using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Syntax ;
using Buildalyzer ;
using Buildalyzer.Workspaces ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using NLog.Fluent ;

namespace AnalysisAppLib
{
    public class Pipeline
    {
        private readonly Func < ILogInvocation > _invocationFactory ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public IPropagatorBlock < AnalysisRequest , ILogInvocation > PipelineInstance
        {
            get ;
            private set ;
        }

        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        private DataflowLinkOptions _linkOptions =
            new DataflowLinkOptions { PropagateCompletion = true } ;

        private IPropagatorBlock < AnalysisRequest , AnalysisRequest > _currentBlock ;

        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        public DataflowLinkOptions LinkOptions
        {
            get { return _linkOptions ; }
            set { _linkOptions = value ; }
        }

        public IPropagatorBlock < AnalysisRequest , AnalysisRequest > CurrentBlock
        {
            get { return _currentBlock ; }
            set { _currentBlock = value ; }
        }

        public Pipeline ( Func < ILogInvocation > invocationFactory )
        {
            _invocationFactory = invocationFactory ;
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) ) ;
        }

        public virtual void BuildPipeline ( )
        {
            var initWorkspace = Register ( Workspaces.InitializeWorkspace2Block ( ) ) ;

            var cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register ( Workspaces.SolutionDocumentsBlock ( ) ) ;


            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register ( DataflowBlocks.FindLogUsages1 ( _invocationFactory ) ) ;
            toDocuments.LinkTo ( findLogUsages , LinkOptions ) ;
            findLogUsages.LinkTo ( Register ( ResultBufferBlock ) , LinkOptions ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( Head , ResultBufferBlock ) ;
        }

        private T Register < T > ( T block )
            where T : IDataflowBlock
        {
            _dataflowBlocks.Add ( block ) ;
            Continuation ( block , ( ( object ) block ).ToString ( ) ) ;
            return block ;
        }

        private static void Continuation ( IDataflowBlock block , string writeOnceBlockName )
        {
            block.Completion.ContinueWith (
                                           task => ContinuationFunction (
                                                                         task
                                                                       , writeOnceBlockName
                                                                        )
                                          ) ;
        }

        private static void ContinuationFunction ( Task task , string logName )
        {
            if ( task.IsFaulted )
            {
                if ( task.Exception != null )
                {
                    var faultReaon = task.Exception.Message ;
                    new LogBuilder ( Logger )
                       .LoggerName ( $"{Logger.Name}.{logName}" )
                       .Level ( LogLevel.Trace )
                       .Exception ( task.Exception )
                       .Message ( "fault is {ex}" , faultReaon )
                       .Write ( ) ;
                }
            }
            else { Logger.Trace ( $"{logName} complete - not faulted" ) ; }
        }

        protected virtual IPropagatorBlock < AnalysisRequest , AnalysisRequest > ConfigureInput ( )
        {
            var input = new WriteOnceBlock < AnalysisRequest > ( s => s ) ;
            Head = input ;
            return input ;
        }

        public ITargetBlock < AnalysisRequest > Head { get ; set ; }

        private static class Workspaces
        {
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


            public static TransformBlock < AnalysisRequest , Workspace >
                InitializeWorkspace2Block ( )
            {
                var makeWs =
                    new TransformBlock < AnalysisRequest , Workspace > ( MakeWorkspace2Async ) ;
                return makeWs ;
            }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            public static async Task < Workspace > MakeWorkspace2Async (
                [ NotNull ] AnalysisRequest req
            )
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            {
                try
                {
                    var manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                    var workspace = new AdhocWorkspace ( ) ;
                    foreach ( var keyValuePair in manager.Projects )
                    {
                        Logger.Debug ( keyValuePair.Key ) ;
                        keyValuePair.Value.Build ( ) ;
                        keyValuePair.Value.AddToWorkspace ( workspace ) ;
                    }

                    return workspace ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , "here" ) ;
                    throw ;
                }
            }


            public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock ( )
            {
                return new TransformManyBlock < Workspace , Document > (
                                                                        workspace => workspace
                                                                                    .CurrentSolution
                                                                                    .Projects
                                                                                    .AsParallel ( )
                                                                                    .Where (
                                                                                            project
                                                                                                => {
                                                                                                Logger
                                                                                                   .Warn (
                                                                                                          "{project}"
                                                                                                        , project
                                                                                                             .Name
                                                                                                         ) ;
                                                                                                return
                                                                                                    true ;
                                                                                            }
                                                                                           )
                                                                                    .SelectMany (
                                                                                                 project
                                                                                                     => project
                                                                                                        .Documents
                                                                                                )
                                                                                    .Where (
                                                                                            document
                                                                                                => {
                                                                                                Logger
                                                                                                   .Info (
                                                                                                          "{document}"
                                                                                                        , document
                                                                                                             .Name
                                                                                                         ) ;
                                                                                                return
                                                                                                    true ;
                                                                                            }
                                                                                           )
                                                                       ) ;
            }
        }

        internal static class DataflowBlocks
        {
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

            public static TransformManyBlock < Document , ILogInvocation > FindLogUsages1 (
                Func < ILogInvocation > invocationFactory
            )
            {
                Logger.Trace ( "Constructing FindUsagesBlock" ) ;
                var flu = new FindLogUsages ( invocationFactory ) ;
                var findLogUsagesBlock =
                    new TransformManyBlock < Document , ILogInvocation > (
                                                                          flu.FindUsagesFunc
                                                                        , new
                                                                          ExecutionDataflowBlockOptions
                                                                          {
                                                                              MaxDegreeOfParallelism
                                                                                  = 4
                                                                          }
                                                                         ) ;
                return findLogUsagesBlock ;
            }
#if USEMSBUILD
        public static TransformBlock < string , BuildResults > PackagesRestore ( )
        {
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => BuildTool.BuildRepository ( s )
                                                             ) ;
            return buildTransformBlock ;
        }
#else
#if NUGET
        public static TransformBlock<string, string> PackagesRestore()
        {
            var buildTransformBlock =
                new TransformBlock<string, string>(
                                                         s => NugetTool.RestorePackages(s)
                                                        );
            return buildTransformBlock;
        }
#endif
#endif

#if VERSIONCONTROL
        public static TransformBlock <AnalysisRequest, AnalysisRequest> CloneSource ( )
        {
            return new TransformBlock <AnalysisRequest, AnalysisRequest> ( VersionControl.CloneProjectAsync ) ;
        }
#endif
            internal class FindLogUsages
            {
                private readonly Func < ILogInvocation > _invocationFactory ;

                public FindLogUsages ( Func < ILogInvocation > invocationFactory )
                {
                    _invocationFactory = invocationFactory ;
                }


                public async Task < IEnumerable < ILogInvocation > > FindUsagesFunc ( Document d )
                {
                    using ( MappedDiagnosticsLogicalContext.SetScoped ( "Document" , d.FilePath ) )
                    {
                        try
                        {
#if TRACE
                            Logger.Trace (
                                          "[{id}] Entering {funcName}"
                                        , Thread.CurrentThread.ManagedThreadId
                                        , nameof ( FindUsagesFunc )
                                         ) ;
#endif
                            var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
                            var root = ( tree ?? throw new InvalidOperationException ( ) )
                               .GetCompilationUnitRoot ( ) ;
                            var model = await d.GetSemanticModelAsync ( ).ConfigureAwait ( true ) ;

                            if ( model != null )
                            {
                                var exceptionType =
                                    model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;
                                var t = LogUsages.GetNLogSymbol ( model ) ;
                                if ( t == null )
                                {
                                    return Array.Empty < ILogInvocation > ( ) ;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                                }

                                var t2 = LogUsages.GetILoggerSymbol ( model ) ;
                                if ( t2 == null )
                                {
                                    return Array.Empty < ILogInvocation > ( ) ;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                                }

                                var logBuilderSymbol = LogUsages.GetLogBuilderSymbol ( model ) ;
                                var rootNode = await tree.GetRootAsync ( ).ConfigureAwait ( true ) ;
                                return
                                    from node in root.DescendantNodes ( ) //.AsParallel ( )
                                    let t_ = t
                                    let t2_ = t2
                                    let builderSymbol = logBuilderSymbol
                                    let tree_ = tree
                                    let model_ = model
                                    let exType = exceptionType
                                    where node.RawKind == ( ushort ) SyntaxKind.InvocationExpression
                                          || node.RawKind
                                          == ( ushort ) SyntaxKind.ObjectCreationExpression
                                    let @out =
                                        LogUsages.CheckInvocationExpression (
                                                                             node
                                                                           , model_
                                                                           , builderSymbol
                                                                           , t_
                                                                           , t2_
                                                                            )
                                    where @out.Item1
                                    let statement =
                                        node.AncestorsAndSelf ( ).Where ( Predicate ).First ( )
                                    select new InvocationParams (
                                                                 tree_
                                                               , model_
                                                               , statement
                                                               , @out
                                                               , exType
                                                                ).ProcessInvocation (
                                                                                     _invocationFactory
                                                                                    ) ;
                            }
                        }
                        catch ( Exception ex )
                        {
                            Logger.Debug ( ex , ex.ToString ( ) ) ;
                            throw ;
                        }
                    }

                    throw new InvalidOperationException ( ) ;
                }

                private static bool Predicate ( [ NotNull ] SyntaxNode arg1 , int arg2 )
                {
                    var b = arg1 is StatementSyntax || arg1 is MemberDeclarationSyntax ;
#if TRACE
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

                    public const string ILoggerClassFullName =
                        NLogNamespace + "." + ILoggerClassName ;

                    private const string NLogNamespace = "NLog" ;

                    private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

                    private static readonly string LoggerClassFullName =
                        NLogNamespace + '.' + LoggerClassName ;


                    // ReSharper disable once SuggestBaseTypeForParameter
                    private static bool CheckSymbol (
                        IMethodSymbol             methSym
                      , params INamedTypeSymbol[] t1
                    )
                    {
                        var cType = methSym.ContainingType ;
                        return CheckTypeSymbol ( cType , t1 ) ;
                    }

                    private static bool CheckTypeSymbol (
                        // ReSharper disable once SuggestBaseTypeForParameter
                        INamedTypeSymbol                      cType
                      , [ NotNull ] params INamedTypeSymbol[] t1
                    )
                    {
                        var r = t1.Any (
                                        symbol => SymbolEqualityComparer.Default.Equals (
                                                                                         cType
                                                                                       , symbol
                                                                                        )
                                       ) ;
                        return r ;
                    }


                    [ NotNull ]
                    public static Tuple < bool , IMethodSymbol , SyntaxNode >
                        CheckInvocationExpression (
                            [ NotNull ] SyntaxNode         n1
                          , [ NotNull ] SemanticModel      currentModel
                          , params      INamedTypeSymbol[] t
                        )
                    {
                        if ( n1 is InvocationExpressionSyntax node )
                        {
                            var symbolInfo =
                                ModelExtensions.GetSymbolInfo ( currentModel , node.Expression ) ;
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
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                                         "symbol info is {node}"
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                                       , symbolInfo.Symbol?.ToDisplayString ( ) ?? "null"
                                        ) ;
                            if ( symbolInfo.Symbol == null )
                            {
                                Logger.Info (
                                             "candidate symbols: {x}"
                                           , symbolInfo.CandidateSymbols
                                            ) ;
                            }
#endif

                            var methodSymbol = symbolInfo.Symbol as IMethodSymbol ;
                            var result = methodSymbol != null
                                         // TODO optimize
                                         && CheckSymbol ( methodSymbol , t ) ;
#if TRACE
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                            Logger.Debug ( "result is {result}" , result ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
#endif
                            return Tuple.Create ( result , methodSymbol , n1 ) ;
                        }

                        if ( n1 is ObjectCreationExpressionSyntax o )
                        {
                            var symbolInfo =
                                ModelExtensions.GetSymbolInfo ( currentModel , o.Type ) ;
                            var typeSymbol = symbolInfo.Symbol as INamedTypeSymbol ;
                            var result = CheckTypeSymbol ( typeSymbol , t ) ;
                            return Tuple.Create < bool , IMethodSymbol , SyntaxNode > (
                                                                                       result
                                                                                     , null
                                                                                     , n1
                                                                                      ) ;
                        }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                        throw new Exception ( "Error" ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    }

                    [ CanBeNull ]
                    public static INamedTypeSymbol GetILoggerSymbol (
                        [ NotNull ] SemanticModel model
                    )
                    {
                        if ( model == null )
                        {
                            throw new ArgumentNullException ( nameof ( model ) ) ;
                        }

                        return model.Compilation.GetTypeByMetadataName ( ILoggerClassFullName ) ;
                    }

                    [ CanBeNull ]
                    public static INamedTypeSymbol GetLogBuilderSymbol (
                        [ NotNull ] SemanticModel model
                    )
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
            }

            private class InvocationParams
            {
                public InvocationParams (
                    SyntaxTree                                  syntaxTree
                  , SemanticModel                               model
                  , SyntaxNode                                  relevantNode
                  , Tuple < bool , IMethodSymbol , SyntaxNode > tuple
                  , INamedTypeSymbol                            namedTypeSymbol
                )
                {
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
                        Tree = syntaxTree
                               ?? throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;

                        Model =
                            model ?? throw new ArgumentNullException ( nameof ( model ) ) ;
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

                internal ILogInvocation ProcessInvocation (
                    Func < ILogInvocation > invocationFactory
                )
                {
                    var exceptionArg = false ;
                    if ( NamedTypeSymbol != null
                         && MethodSymbol != null
                         && MethodSymbol.Parameters.Any ( ) )
                    {
                        // ReSharper disable once RedundantAssignment
                        exceptionArg = IsException (
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
                                                                                       symbol
                                                                                           => symbol
                                                                                              .Name
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
                                                 , methodSymbol.Parameters.Select (
                                                                                   symbol => symbol
                                                                                      .Name
                                                                                  )
                                                  )
                                     ) ;
#endif
                        var invocation = InvocationExpression ;
                        IEnumerable < ArgumentSyntax > rest ;
                        var semanticModel = Model ;
                        if ( msgI != null )
                        {
                            var fargs = invocation
                                       .ArgumentList.Arguments.Skip ( msgI.Value )
                                       .ToList ( ) ;
                            rest = fargs.Skip ( 1 ) ;
                            var msgarg = fargs.First ( ) ;
                            var msgArgExpr = msgarg.Expression ;
                            var msgArgTypeInfo =
                                ModelExtensions.GetTypeInfo ( semanticModel , msgArgExpr ) ;
                            var symbolInfo =
                                ModelExtensions.GetSymbolInfo ( semanticModel , msgArgExpr ) ;
                            var arg1sym = symbolInfo.Symbol ;
#if TRACE
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
                                        var t = Tuple.Create (
                                                              prop.IsPositional
                                                            , prop.PropertyName
                                                             ) ;
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
                                //msgval.MessageExprPojo = Transforms.TransformExpr ( msgArgExpr ) ;
                            }
                        }
                        else
                        {
                            rest = invocation.ArgumentList.Arguments ;
                        }

                        var relevantNode = RelevantNode ;
                        var sourceLocation = Tree.FilePath
                                             + ":"
                                             + ( relevantNode.GetLocation ( )
                                                             .GetMappedLineSpan ( )
                                                             .StartLinePosition.Line
                                                 + 1 ) ;

                        var t1 = Transforms.TransformSyntaxNode ( relevantNode ) ;
                        var debugInvo = invocationFactory ( ) ;
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

                    throw new InvalidOperationException() ;
                }

                private static bool IsException (
                    INamedTypeSymbol exceptionType
                  , ITypeSymbol      baseType
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

                private class LogMessageRepr
                {
                    public LogMessageRepr ( bool isMessageTemplate , object constantMessage )
                    {
                        IsMessageTemplate = isMessageTemplate ;
                        ConstantMessage   = constantMessage ;
                        if ( IsMessageTemplate )
                        {
                            MessageTemplate =
                                MessageTemplate.Parse ( constantMessage.ToString ( ) ) ;
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
                            return IsMessageTemplate
                                       ? MessageTemplate?.Text ?? ""
                                       : MessageExprPojo ;
                        }
                    }

                    public object ConstantMessage { get ; set ; }
                }
            }
        }
#if NUGET
    internal static class NugetTool
    {
        public static string RestorePackages ( string s ) { return s ; }
    }
#endif
    }
}