#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocation.cs
// 
// 2020-02-25-9:21 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;
using NLog ;
using ProjLib ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Threading.Tasks ;

namespace CodeAnalysisApp1
{
    public class CodeAnalyseContext
    {
        [ JsonIgnore ]
        protected SemanticModel _currentModel ;

        [ JsonIgnore ]
        protected CompilationUnitSyntax _currentRoot ;

        [ JsonIgnore ]
        protected StatementSyntax _statement ;

        [ JsonIgnore ]
        protected SyntaxNode node ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeAnalyseContext (
            SemanticModel         currentModel
          , CompilationUnitSyntax currentRoot
          , StatementSyntax       statement
          , SyntaxNode            node
          , ICodeSource           document
          , SyntaxTree            syntaxTree
        )
        {
            _currentModel = currentModel ;
            _currentRoot  = currentRoot ;
            _statement    = statement ;
            Node          = node ;
            Document      = document ;
            SyntaxTree    = syntaxTree ;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeAnalyseContext ( ICodeSource document ) { Document = document ; }

        public static CodeAnalyseContext Parse ( string code , string assemblyName )
        {
            var syntaxTree = CSharpSyntaxTree.ParseText ( code ) ;
            ;
            var compilation = CreateCompilation ( assemblyName , syntaxTree ) ;

            return CreateFromCompilation ( syntaxTree , compilation ) ;
        }

        private static CodeAnalyseContext CreateFromCompilation (
            SyntaxTree        syntaxTree
          , CSharpCompilation compilation
        )
        {
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            return new CodeAnalyseContext (
                                           compilation.GetSemanticModel ( syntaxTree )
                                         , syntaxTree.GetCompilationUnitRoot ( )
                                         , null
                                         , syntaxTree.GetRoot ( )
                                         , new CodeSource ( "memory" )
                                         , syntaxTree
                                          ) ;
        }

        private static CSharpCompilation CreateCompilation (
            string     assemblyName
          , SyntaxTree syntaxTree
        )
        {

            var compilation = CSharpCompilation.Create ( assemblyName )
                                               .AddReferences (
                                                               MetadataReference.CreateFromFile (
                                                                                                 typeof
                                                                                                     ( string
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )
                                                             , MetadataReference.CreateFromFile (
                                                                                                 typeof
                                                                                                     ( Logger
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )).AddSyntaxTrees(syntaxTree);

            return compilation ;
        }

        public static CodeAnalyseContext FromSyntaxTree (
            SyntaxTree               tree
          , string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var comp = CreateCompilation ( assemblyName , tree ) ;
            return CreateFromCompilation ( tree , comp ) ;
            // return new CodeAnalyseContext(comp.GetSemanticModel(tree), tree.GetCompilationUnitRoot(), null, tree.GetRoot(), new CodeSource("memory"), tree);
        }

        public static CodeAnalyseContext FromSyntaxNode (
            SyntaxNode               node
          , string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var tree = SyntaxFactory.SyntaxTree ( node ) ;
            // Logger.Info ( tree.HasCompilationUnitRoot ) ;
            return FromSyntaxTree ( tree , assemblyName , opts ) ;
        }


        [JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public StatementSyntax Statement { get => _statement ; set => _statement = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public CompilationUnitSyntax CurrentRoot
        {
            get => _currentRoot ;
            set => _currentRoot = value ;
        }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public SyntaxNode Node { get => node ; set => node = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public ICodeSource Document { get ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public SyntaxTree SyntaxTree { get ; }

    }

    public class LogInvocation : CodeAnalyseContext
    {
        private LogMessageRepr _msgval ;
        private string         sourceLocation ;
        private IMethodSymbol  methodSymbol ;

        private string _LoggerType ;
        private string _methodName ;


        public LogInvocation (
            string                sourceLocation
          , IMethodSymbol         methodSymbol
          , LogMessageRepr        msgval
          , StatementSyntax       statement
          , SemanticModel         currentModel
          , CompilationUnitSyntax currentRoot
          , ICodeSource           document
          , SyntaxTree            syntaxTree
        ) : base (
                  currentModel
                , currentRoot
                , statement
                , statement
                , document
                , syntaxTree
                 )
        {
            CurrentModel   = currentModel ;
            CurrentRoot    = currentRoot ;
            Statement      = statement ;
            Msgval         = msgval ;
            SourceLocation = sourceLocation ;
            MethodSymbol   = methodSymbol ;
            if ( methodSymbol != null )
            {
                MethodName = MethodSymbol.Name ;
                LoggerType = methodSymbol.ContainingType.ContainingNamespace.MetadataName
                             + "."
                             + methodSymbol.ContainingType.MetadataName ;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>[
        [JsonConstructor]
        public LogInvocation ( ICodeSource document , LogMessageRepr msgval , string sourceLocation , string loggerType , string methodName , IList < LogInvocationArgument > arguments , string sourceContext , string followingCode , string precedingCode , string code ) : base ( document )
        {
            _msgval = msgval ;
            this.sourceLocation = sourceLocation ;
            _LoggerType = loggerType ;
            _methodName = methodName ;
            Arguments = arguments ;
            SourceContext = sourceContext ;
            FollowingCode = followingCode ;
            PrecedingCode = precedingCode ;
            Code = code ;
        }

        public string SourceLocation { get => sourceLocation ; set => sourceLocation = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public IMethodSymbol MethodSymbol { get => methodSymbol ; set => methodSymbol = value ; }


        public LogMessageRepr Msgval { get => _msgval ; set => _msgval = value ; }

        public IList < LogInvocationArgument > Arguments { get ; set ; }

        public string SourceContext { get ; set ; }

        public string FollowingCode { get ; set ; }

        public string PrecedingCode { get ; set ; }

        public string Code { get ; set ; }

        public string LoggerType { get => _LoggerType ; set => _LoggerType = value ; }

        public string MethodName { get => _methodName ; set => _methodName = value ; }

        public string MethodDisplayName => LoggerType + "." + MethodName ;
    }
}