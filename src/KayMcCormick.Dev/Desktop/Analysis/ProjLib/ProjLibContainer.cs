#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Container.cs
// 
// 2020-02-27-1:59 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Linq ;
using AnalysisFramework ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Activators.Reflection ;
using Autofac.Core.Lifetime ;
using Autofac.Integration.Mef ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public static class ProjLibContainer
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static ILifetimeScope GetScope (params IModule[] modules )
        {
            var b = new ContainerBuilder ( ) ;
            b.RegisterMetadataRegistrationSources ( ) ;
            // b.RegisterGeneric ( typeof ( SpanObject <> ) ).As ( typeof ( ISpanObject <> ) ) ;
            foreach ( var module in modules )
            {
                b.RegisterModule ( module ) ;
            }
            b.RegisterType < LogInvocationSpan > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < LogInvocation > > ( ) ;
            b.RegisterType < TokenSpanObject > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < SyntaxToken > > ( ) ;
            b.RegisterType < TriviaSpanObject > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < SyntaxTrivia > > ( ) ;
            b.RegisterType < Visitor2 > ( ).AsSelf ( ).As < CSharpSyntaxWalker > ( ) ;
            b.RegisterType < FormattedCode > ( ).AsSelf ( ) ;
            // b.RegisterType<HasSourceCode>()
            //  .As<ISourceCode>()
            //  .WithParameter(new PositionalParameter(0, Resources.Program_Parse));

            b.RegisterType < TransformScope > ( )
             .WithParameter ( new NamedParameter ( "sourceCode" , LibResources.Program_Parse ) )
             .AsSelf ( )
             .InstancePerLifetimeScope ( ) ;
            b.RegisterAdapter < IHasCompilation , IEnumerable < IHasTreeAndCompilation > > (
                                                                                            (
                                                                                                context
                                                                                              , x
                                                                                            ) => {
                                                                                                new
                                                                                                        LogBuilder (
                                                                                                                    Logger
                                                                                                                   ).Level (
                                                                                                                            LogLevel
                                                                                                                               .Debug
                                                                                                                           )
                                                                                                                    .Property (
                                                                                                                               "TreeAndCompilation"
                                                                                                                             , x
                                                                                                                              )
                                                                                                                    .Message (
                                                                                                                              "Debug adapte"
                                                                                                                             )
                                                                                                                    .Write ( ) ;
                                                                                                IList
                                                                                                < IHasTreeAndCompilation
                                                                                                > r
                                                                                                    = new
                                                                                                        List
                                                                                                        < IHasTreeAndCompilation
                                                                                                        > ( ) ;
                                                                                                foreach
                                                                                                ( var
                                                                                                      compilationSyntaxTree
                                                                                                    in
                                                                                                    x
                                                                                                       .Compilation
                                                                                                       .SyntaxTrees
                                                                                                )
                                                                                                {
                                                                                                    var x2 =
                                                                                                        new
                                                                                                            HasTreeAndCompilation (
                                                                                                                                   x
                                                                                                                                 , new
                                                                                                                                       HasTree (
                                                                                                                                                compilationSyntaxTree
                                                                                                                                               )
                                                                                                                                  ) ;
                                                                                                    r.Add (
                                                                                                           x2
                                                                                                          ) ;
                                                                                                }

                                                                                                return
                                                                                                    r ;
                                                                                            }
                                                                                           ) ;
            b.RegisterAdapter < IHasTreeAndCompilation , IHasTreeAndModel > (
                                                                             (
                                                                                 context
                                                                               , compilation
                                                                             ) => {
                                                                                 Logger.Info("{tree}", compilation.Tree.GetDiagnostics());
                                                                                 var semanticModel = compilation
                                                                                                    .Compilation
                                                                                                    .GetSemanticModel (
                                                                                                                       compilation
                                                                                                                          .Tree
                                                                                                                      ) ;

                                                                                 var hassModelImplementation = new
                                                                                     HassModel (
                                                                                                semanticModel
                                                                                               ) ;
                                                                                 return new
                                                                                     HasTreeAndModel (
                                                                                                      hassModelImplementation
                                                                                                    , compilation
                                                                                                     ) ;
                                                                             }
                                                                            ) ;
                
            b.RegisterAdapter < IHasTreeAndModel , IHasLogInvocations > (
                                                                         ( context , tree ) => {
                                                                             var visitor =
                                                                                 context
                                                                                    .Resolve <
                                                                                         Visitor2
                                                                                     > ( ) ;
                                                                             visitor.Visit (
                                                                                            tree
                                                                                               .Tree
                                                                                               .GetRoot ( )
                                                                                           ) ;
                                                                             return new
                                                                                 HasLogInvocations ( ) ;
                                                                         }
                                                                        ) ;

            b.RegisterAdapter < IHasTree , IHasCompilation > (
                                                              ( context , tree )
                                                                  => new HasCompilation (
                                                                                         CSharpCompilation
                                                                                            .Create (
                                                                                                     "random"
                                                                                                    )
                                                                                            .AddReferences (
                                                                                                            MetadataReference
                                                                                                               .CreateFromFile (
                                                                                                                                typeof
                                                                                                                                    ( string
                                                                                                                                    ).Assembly
                                                                                                                                     .Location
                                                                                                                               )
                                                                                                          , MetadataReference
                                                                                                               .CreateFromFile (
                                                                                                                                typeof
                                                                                                                                    ( Logger
                                                                                                                                    ).Assembly
                                                                                                                                     .Location
                                                                                                                               )
                                                                                                           )
                                                                                            .AddSyntaxTrees (
                                                                                                             tree
                                                                                                                .Tree
                                                                                                            )
                                                                                        )
                                                             ) ;
            b.RegisterAdapter < ISourceCode , IHasTree > (
                                                          ( context , code )
                                                              => new HasTree (
                                                                              CSharpSyntaxTree
                                                                                 .ParseText (
                                                                                             code
                                                                                                .SourceCode
                                                                                            )
                                                                             )
                                                         ) ;

            // b.RegisterAdapter < ISourceContext , IIntermediateContext > ( ) ;
            b.RegisterType < TransformArgs > ( ).AsSelf ( ) ;
            // ( c , p ) => new TransformArgs.TransformArgsFactory(new TransformArgs())  p.TypedAs < CodeAnalyseContext > ( )
            //                                                     , c.Resolve < Visitor2 > ( ))).AsSelf();
            b.Register ( ( c , p ) => new CodeAnalyseContext.Factory1 ( CodeAnalyseContext.Parse ) )
             .AsSelf ( )
             .InstancePerLifetimeScope ( ) ;
            // b.RegisterAdapter<IHasTreeAndCompilation, IHasTreeAndModel>();
            b.RegisterAdapter < IHasCompilation , IHasTreeAndCompilation > (
                                                                            (
                                                                                context
                                                                              , compilation
                                                                            ) => new
                                                                                HasTreeAndCompilation (
                                                                                                       compilation
                                                                                                     , context
                                                                                                          .Resolve
                                                                                                           < IHasTree
                                                                                                           > ( )
                                                                                                      )
                                                                           ) ;

            b.Register (
                        ( c , p ) => new MakeInfo (
                                                   null
                                                 , p.Named < FormattedCode > ( "control" )
                                                 , p.Named < string > ( "sourceText" )
                                                 , null
                                                 , c.Resolve < CodeAnalyseContext.Factory1 > ( )
                                                  )
                       )
             .AsSelf ( ) ;

            return b.Build ( ).BeginLifetimeScope ( ) ;
        }
    }

    public interface IHasTreeAndCompilation : IHasTree , IHasCompilation
    {
    }

    internal class HasTreeAndCompilation : IHasTreeAndCompilation
    {
        private IHasCompilation _hasCompilationImplementation ;
        private IHasTree        _hasTreeImplementation ;

        #region Implementation of IHasCompilation
        public HasTreeAndCompilation (
            IHasCompilation hasCompilationImplementation
          , IHasTree        hasTreeImplementation
        )
        {
            _hasCompilationImplementation = hasCompilationImplementation ;
            _hasTreeImplementation        = hasTreeImplementation ;
        }

        public CSharpCompilation Compilation => _hasCompilationImplementation.Compilation ;
        #endregion

        #region Implementation of IHasTree
        public SyntaxTree Tree => _hasTreeImplementation.Tree ;
        #endregion
    }

    public interface IHasLogInvocations
    {
        IList < LogInvocation > LogInvocationList { get ; }
    }

    internal class HasLogInvocations : IHasLogInvocations
    {
        private IList < LogInvocation > _logInvocationList = new List < LogInvocation > ( ) ;
        #region Implementation of IHasLogInvocations
        public IList < LogInvocation > LogInvocationList
        {
            get => _logInvocationList ;
            set => _logInvocationList = value ;
        }
        #endregion
    }

    public class HasSourceCode : ISourceCode
    {
        private string _sourceCode ;
        #region Implementation of ISourceCode
        public string SourceCode { get => _sourceCode ; set => _sourceCode = value ; }
        #endregion

        public HasSourceCode ( string sourceCode ) { _sourceCode = sourceCode ; }
    }

    public interface IHasCompilation
    {
        CSharpCompilation Compilation { get ; }
    }

    internal class HasCompilation : IHasCompilation
    {
        private CSharpCompilation _compilation ;
        #region Implementation of IHasCompilation
        public CSharpCompilation Compilation { get => _compilation ; set => _compilation = value ; }
        #endregion

        public HasCompilation ( CSharpCompilation compilation ) { _compilation = compilation ; }
    }

    public class HasTree : IHasTree
    {
        private SyntaxTree _tree ;

        public HasTree ( SyntaxTree parseText ) { _tree = parseText ; }

        #region Implementation of IHasTree
        public SyntaxTree Tree { get => _tree ; set => _tree = value ; }
        #endregion
    }

    public interface IIntermediateContext
    {
    }

    public interface ISourceContext : ISourceCode , IHassModel , IHasTree
    {
    }

    public interface IHasTreeAndModel : IHasTree , IHassModel
    {
    }

    internal class HasTreeAndModel : IHasTree , IHassModel , IHasTreeAndModel
    {
        private IHassModel _hassModelImplementation ;
        private IHasTree   _hasTreeImplementation ;

        #region Implementation of IHassModel
        public SemanticModel Model => _hassModelImplementation.Model ;
        #endregion

        #region Implementation of IHasTree
        public SyntaxTree Tree => _hasTreeImplementation.Tree ;
        #endregion

        public HasTreeAndModel (
            IHassModel hassModelImplementation
          , IHasTree   hasTreeImplementation
        )
        {
            _hassModelImplementation = hassModelImplementation ;
            _hasTreeImplementation   = hasTreeImplementation ;
        }
    }

    public interface IHasTree
    {
        SyntaxTree Tree { get ; }
    }

    public interface IHassModel
    {
        SemanticModel Model { get ; }
    }

    internal class HassModel : IHassModel
    {
        private SemanticModel _model ;
        #region Implementation of IHassModel
        public SemanticModel Model { get => _model ; set => _model = value ; }
        #endregion

        public HassModel ( SemanticModel model ) { _model = model ; }
    }

    public interface ISourceCode
    {
        string SourceCode { get ; }
    }
}