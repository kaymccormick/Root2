using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Threading.Tasks ;
using CodeAnalysisApp1 ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace ProjLib
{
    public class ProjectHandler : ISupportInitialize
    {
        public delegate void ProcessDocumentDelegate ( Document document ) ;

        public delegate void ProcessProjectDelegate (
            MSBuildWorkspace workspace
          , Project          project
        ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProcessDocumentDelegate ProcessDocument ;
        public ProcessProjectDelegate  ProcessProject ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public ProjectHandler ( string solutionPath , VisualStudioInstance instance )
        {
            SolutionPath = solutionPath ;
            Instance     = instance ;
        }

        public string SolutionPath { get ; }

        public VisualStudioInstance Instance { get ; }

        public MSBuildWorkspace Workspace { get ; set ; }


        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit ( ) { }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( ) { throw new NotImplementedException ( ) ; }

        public async Task < MSBuildWorkspace > NewMethod (
            string               solutionPath
          , VisualStudioInstance instance
        )

        {
            if ( MSBuildLocator.IsRegistered )
            {
                MSBuildLocator.Unregister ( ) ;
            }

            if ( MSBuildLocator.CanRegister )
            {
                MSBuildLocator.RegisterInstance ( instance ) ;
            }
            else
            {
                throw new Exception ( "Unable to register msbuildlocator" ) ;
            }

            MSBuildWorkspace workspace ;
            try
            {
                workspace = MSBuildWorkspace.Create ( ) ;
            }
            catch ( Exception ex )
            {
                throw ;
            }

            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += ( o , e ) => Console.WriteLine ( e.Diagnostic.Message ) ;

            // ReSharper disable once LocalizableElement
            Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
            Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

            // Attach progress reporter so we print projects as they are loaded.
            await workspace.OpenSolutionAsync ( solutionPath , progressReporter ) ;
            // , new Program.ConsoleProgressReporter()
            // );
            Console.WriteLine ( $"Finished loading solution '{solutionPath}'" ) ;
            return workspace ;
        }

        public IProgress < ProjectLoadProgress > progressReporter { get ; set ; }

        public async Task < bool > LoadAsync ( )
        {
            Workspace = await NewMethod ( SolutionPath , Instance ) ;
            return true ;
        }

        public async Task ProcessAsync ( )
        {
            foreach ( var pr in Workspace.CurrentSolution.Projects )
            {
                ProcessProject ( Workspace , pr ) ;
                foreach ( var prDocument in pr.Documents )
                {
                    ProcessDocument ( prDocument ) ;
                    await OnPrepareProcessDocumentAsync ( prDocument ) ;
                    await OnProcessDocumentAsync ( prDocument ) ;
                }
            }
        }

        public virtual async Task OnProcessDocumentAsync ( Document        document ) { return ; }
        public virtual async Task OnPrepareProcessDocumentAsync ( Document doc )      { return ; }
    }

    public class ProjectHandlerImpl : ProjectHandler
    {
        private const           string ILoggerClassName    = "ILogger" ;
        private const           string LoggerClassName     = "Logger" ;
        private static readonly Logger Logger              = LogManager.GetCurrentClassLogger ( ) ;
        private static readonly string LoggerClassFullName = NLogNamespace + '.' + LoggerClassName ;

        private static readonly string
            ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName ;

        public ProjectHandlerImpl ( string s , VisualStudioInstance vsi ) : base ( s , vsi ) { }

        private const string NLogNamespace = "NLog" ;

        /// <inheritdoc />
        public override async Task OnProcessDocumentAsync ( Document document1 )
        {
            if ( document1.Project.Name == "NLog" )
            {
                return ;
            }

            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > query ;
            try
            {
                Query1 ( document1 , CurrentRoot , CurrentModel ) ;
                //
                // if ( query != null )
                // {
                //     var qresult = query.ToList ( ) ;
                //     Collect ( qresult) ;
                //     foreach ( var expr in qresult.SelectMany (
                //                                             tuple => tuple.Item3.Select (
                //                                                                          tuple1
                //                                                                              => tuple1
                //                                                                                 .Item1
                //                                                                         )
                //                                            ) )
                //     {
                //
                //     }
                // }
            }
            catch ( Exception ex )
            {
                Logger.Info ( ex , ex.ToString ( ) ) ;
            }

            //Debug.Assert ( query != null , nameof ( query ) + " != null" ) ;
        }

        public List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > >
            OutputList { get ; } =
            new List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > ( ) ;

        private void Collect (
            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > query
        )
        {
            OutputList.AddRange ( query ) ;
        }

        public override async Task OnPrepareProcessDocumentAsync ( Document doc )
        {
            if ( doc.Project.Name == "NLog" )
            {
                return ;
            }

            Logger.Trace ( nameof ( OnProcessDocumentAsync ) ) ;
            if ( doc == null )
            {
                throw new ArgumentNullException ( nameof ( doc ) ) ;
            }

            var tree = await doc.GetSyntaxTreeAsync ( ) ;
            var model = await doc.GetSemanticModelAsync ( ) ;
            var root = tree.GetCompilationUnitRoot ( ) ;
            CurrentTree  = tree ;
            CurrentModel = model ;
            CurrentRoot  = root ;
        }

        public SyntaxTree CurrentTree { get ; set ; }

        public SemanticModel CurrentModel { get ; set ; }

        public CompilationUnitSyntax CurrentRoot { get ; set ; }


        public void Query1 ( Document document1 , SyntaxNode root , SemanticModel model )
        {
            var comp = model.Compilation ;
            var ExceptionType = comp.GetTypeByMetadataName ( "System.Exception" ) ;
            if ( ExceptionType == null )
            {
                throw new Exception ( "No exception type" ) ;
            }

            var t1 = comp.GetTypeByMetadataName ( LoggerClassFullName ) ;
            if ( t1 == null )
            {
                Logger.Warn (
                             "No {clas} in {document}"
                           , LoggerClassFullName
                           , document1.RelativePath ( )
                            ) ;
                return ;
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
                           , string.Join (
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
                from node in root.DescendantNodesAndSelf ( ).OfType < StatementSyntax > ( )
                where LimitToMarkedStatements == false
                      || node.GetLeadingTrivia ( )
                             .Any (
                                   trivia => trivia.Kind ( ) == SyntaxKind.SingleLineCommentTrivia
                                             && trivia.ToString ( ).Contains ( "doprocess" )
                                  )
                select node ;

            if ( LogVisitedStatements )
            {
                foreach ( var q in qq0 )
                {
                    Logger.Info ( q.ToString ) ;
                }
            }

            var qxy =
                from statement in qq0
                let invocations =
                    statement.DescendantNodes (
                                               node => node == statement
                                                       || ! ( node is StatementSyntax )
                                              )
                             .OfType < InvocationExpressionSyntax > ( )
                from invocation in invocations
                let symbolInfo = model.GetSymbolInfo ( invocation.Expression )
                let symbol = symbolInfo.Symbol
                where symbol != null
                      && symbol is IMethodSymbol methSym
                      && CheckSymbol ( methSym , t1 , t2 )
                select new { statement , invocation , methodSymbol = ( IMethodSymbol ) symbol } ;
            foreach ( var qqq in qxy )
            {
                try
                {
                    ProcessInvocation (
                                       document1
                                     , qqq.statement
                                     , qqq.invocation
                                     , qqq.methodSymbol
                                     , ExceptionType
                                      ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Warn ( ex , "unable to process invocation: {message}" , ex.Message ) ;
                }
            }

            return ;
#if false
            var qq1 =
                from node in root.DescendantNodesAndSelf ( )
                let symbol = model.GetTypeInfo ( node )
                where symbol.Type?.ContainingAssembly?.Identity?.Name == NLogNamespace
                select new { node , symbol } ;

            var query1 = root.DescendantNodesAndSelf ( )
                             .Select ( node => new { node , symbol = model.GetTypeInfo ( node ) } )
                             .Where (
                                     arg => arg.symbol.Type?.ContainingAssembly?.Identity?.Name
                                            == NLogNamespace
                                    ) ;

            var tempq = qq1.Select (
                                       ( arg , i )
                                           => new
                                              {
                                                  arg.symbol
                                        , statement =
                                                      arg.node.AncestorsAndSelf ( )
                                                         .OfType < StatementSyntax > ( )
                                                         .FirstOrDefault ( )
                                              }
                                      )
                              .Distinct ( )
                              .Where ( arg => arg.statement != null )
                              .ToList ( ) ;
            var query = tempq ;
            // Logger.Warn ( "{} {}" , query.Count , query.Distinct ( ).Count ( ) ) ;

            foreach ( var arg3 in query )
            {
                if ( arg3 != null )
                {
                    Logger.Debug (
                                  "st: {symbol} {document} {line} {statementSyntax}"
                        , document1?.RelativePath()
                        , arg3.symbol.ConvertedType.ToDisplayString ( )
                        , arg3.statement.GetLocation ( )
                                      .GetMappedLineSpan ( )
                                      .StartLinePosition.Line
                                  + 1
                        , arg3
                                 ) ;
                }
            }


            var q2 = query.Where ( ( syntax , i ) => syntax != null )
                          .Select (
                                   syntax => syntax
                                            .statement.DescendantNodesAndSelf ( )
                                            .Select (
                                                     node => Tuple.Create (
                                                                           node
                                                                 , model.GetSymbolInfo (
                                                                                                node
                                                                                               )
                                                                          )
                                                    )
                                  ) ;
            var qq = q2.SelectMany (
                                    tuples => tuples.Where (
                                                            tuple => tuple.Item2.Symbol != null
                                                            /*|| tuple.Item2.CandidateSymbols
                                      .Any ( )*/
                                                           )
                                                    .Where (
                                                            tuple => tuple.Item2.Symbol != null
                                                                     && new[]
                                                                        {
                                                                            ILoggerClassName
                                                                  , LoggerClassName
                                                                        }.Contains (
                                                                                    tuple
                                                                                       .Item2.Symbol
                                                                                      ?.ContainingType
                                                                                      ?.Name
                                                                                   )
                                                                     && tuple.Item2.Symbol.Kind
                                                                     == SymbolKind.Method
                                                           )
                                                    .Select (
                                                             tuple => Tuple.Create (
                                                                                    tuple
                                                                                       .Item1
                                                                                       .AncestorsAndSelf ( )
                                                                                       .OfType <
                                                                                            InvocationExpressionSyntax
                                                                                        > ( )
                                                                                       .FirstOrDefault ( )
                                                                          , ( IMethodSymbol
                                                                                    ) tuple
                                                                                     .Item2.Symbol
                                                                                   )
                                                            )
                                   )
                       .ToList ( ) ;

            var result =
                new List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > >
                > ( ) ;
            foreach ( var (invocation , methodSymbol) in qq )
            {
                if ( invocation != null )
                {
                    Logger.Debug ( invocation.ToString ( ) ) ;
                    Logger.Debug ( methodSymbol ) ;

                    foreach ( var enumerable in invocation.ArgumentList.Arguments )
                    {
                        Logger.Debug ( enumerable ) ;
                    }
                }
            }


            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > d2 ;

            Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > expr1 (
                Tuple < InvocationExpressionSyntax , IMethodSymbol > syntax
      , int                                                  i
            )
            {
                if ( syntax.Item1 != null )
                {
                    var line = syntax.Item1.GetLocation ( )
                                     .GetMappedLineSpan ( )
                                     .StartLinePosition.Line
                               + 1 ;
                    var l = syntax.Item1.ArgumentList.Arguments ;
                    var xxx = l.Select (
                                        argumentSyntax => Tuple.Create (
                                                                        argumentSyntax.Expression
                                                              , Transforms.TransformExpr (
                                                                                                  argumentSyntax
                                                                                                     .Expression
                                                                                                 )
                                                                       )
                                       ) ;
                    return Tuple.Create (
                                         line
                               , syntax.Item1.Expression.ToString ( )
                               , xxx.ToList ( )
                                        ) ;
                }

                return new Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > (
                                                                                                  0
                                                                                        , null
                                                                                        , null
                                                                                                 ) ;
            }

            d2 = qq.Distinct ( )
                   .Where ( tuple => tuple.Item1 != null )
                   .Select ( expr1 )
                   .ToList ( ) ;
            foreach ( var (item1 , item2 , item3) in d2 )
            {
                foreach ( var s in item3 )
                {
                    {
                        Logger.Debug ( "{s}" , s.Item1.ToString ( ) ) ;
                    }
                }
            }

            return d2 ;
#endif
        }

        public bool LogVisitedStatements { get ; set ; }

        public bool LimitToMarkedStatements { get ; set ; }

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
            return cType.MetadataName    == LoggerClassName
                   || cType.MetadataName == ILoggerClassName ; //  || methSym.Name == "Debug";
            return r ;
        }

        private void ProcessInvocation (
            Document                   document1
          , StatementSyntax            statement
          , InvocationExpressionSyntax invocation
          , IMethodSymbol              methodSymbol
          , INamedTypeSymbol           exceptionType
        )
        {
            var exceptionArg = IsException (
                                            exceptionType
                                          , methodSymbol.Parameters.First ( ).Type
                                           ) ;
            var msgParam = methodSymbol.Parameters.Select ( ( symbol , i ) => new { symbol , i } )
                                       .Where ( arg1 => arg1.symbol.Name == "message" ) ;
            if ( ! msgParam.Any ( ) )
            {
                throw new NoMessageParameterException ( ) ;
            }

            var msgI = msgParam.First ( ).i ;
            Logger.Error (
                          "params = {params}"
                        , string.Join (
                                       ", "
                                     , methodSymbol.Parameters.Select ( symbol => symbol.Name )
                                      )
                         ) ;
            var fargs = invocation.ArgumentList.Arguments.Skip ( msgI ) ;
            fargs = fargs.TakeWhile ( ( syntax , i ) => i < msgI ) ;
            var rest = fargs.Skip ( 1 ) ;
            var msgarg = fargs.First ( ) ;
            var msgArgExpr = msgarg.Expression ;
            var msgArgTypeInfo = CurrentModel.GetTypeInfo ( msgArgExpr ) ;
            var baseType = msgArgTypeInfo.Type ;
            var symbolInfo = CurrentModel.GetSymbolInfo ( msgArgExpr ) ;
            var arg1sym = symbolInfo.Symbol ;
            if ( arg1sym != null )
            {
                Logger.Debug ( "{type} {symb}" , arg1sym.GetType ( ) , arg1sym ) ;
            }

            var constant = CurrentModel.GetConstantValue ( msgArgExpr ) ;
            if ( constant.HasValue )
            {
                Logger.Warn ( "Constant {constant}" , constant.Value ) ;
                var m = MessageTemplate.Parse ( ( string ) constant.Value ) ;
                var o = new List < object > ( ) ;
                foreach ( var messageTemplateToken in m.Tokens )
                {
                    if ( messageTemplateToken is PropertyToken p )
                    {
                        var t = Tuple.Create ( p.IsPositional , p.PropertyName ) ;
                        o.Add ( t ) ;
                    }
                    else if ( messageTemplateToken is TextToken t )
                    {
                        var xt = Tuple.Create ( t.Text ) ;
                        o.Add ( xt ) ;
                    }
                }

                Logger.Warn ( "{}" , string.Join ( ", " , o ) ) ;
            }
            else
            {
                Logger.Warn ( "{}" , msgArgExpr ) ;
            }

            var transformed = invocation.ArgumentList.Arguments.Select (
                                                                        syntax => Transforms
                                                                           .TransformExpr (
                                                                                           syntax
                                                                                              .Expression
                                                                                          )
                                                                       )
                                        .ToList ( ) ;
            Logger.Error ( "{t}" , transformed ) ;
            var sourceLocation = document1.RelativePath ( )
                                 + ":"
                                 + statement
                                  .GetLocation ( )
                                  .GetMappedLineSpan ( )
                                  .StartLinePosition.Line
                                 + 1 ;
            var debugInvo = new LogInvocation ( sourceLocation , methodSymbol , transformed ) ;
            Invocations.Add ( debugInvo ) ;
        }

        public class LogInvocation
        {
            private string          sourceLocation ;
            private IMethodSymbol   methodSymbol ;
            private List < object > transformed ;

            public LogInvocation (
                string          sourceLocation
              , IMethodSymbol   methodSymbol
              , List < object > transformed
            )
            {
                SourceLocation = sourceLocation ;
                MethodSymbol   = methodSymbol ;
                Transformed    = transformed ;
            }

            public string SourceLocation { get => sourceLocation ; set => sourceLocation = value ; }

            public IMethodSymbol MethodSymbol
            {
                get => methodSymbol ;
                set => methodSymbol = value ;
            }

            public List < object > Transformed { get => transformed ; set => transformed = value ; }
        }

        public List < LogInvocation > Invocations { get ; set ; } = new List < LogInvocation > ( ) ;

        private static bool IsException ( INamedTypeSymbol exceptionType , ITypeSymbol baseType )
        {
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


        private static void NewMethod1 ( Compilation comp , INamedTypeSymbol t1 )
        {
            var t2 = comp.GetTypeByMetadataName ( ILoggerClassFullName ) ;
            var methodSymbols = t1.GetMembers ( )
                                  .Concat ( t2.GetMembers ( ) )
                                  .Where ( symbol => symbol.Kind == SymbolKind.Method )
                                  .Select ( symbol => ( IMethodSymbol ) symbol )
                                  .Where ( symbol => symbol.MethodKind == MethodKind.Ordinary )
                                  .ToList ( ) ;
            foreach ( var methodSymbol in methodSymbols )
            {
                Logger.Debug ( methodSymbol.ToString ( ) ) ;
            }
        }
    }

    internal class NoMessageParameterException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
        public NoMessageParameterException ( ) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error. </param>
        public NoMessageParameterException ( string message ) : base ( message ) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified. </param>
        public NoMessageParameterException ( string message , Exception innerException ) : base (
                                                                                                 message
                                                                                               , innerException
                                                                                                )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected NoMessageParameterException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}
