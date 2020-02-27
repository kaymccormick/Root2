#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// CodeAnalysisApp1
// CodeAnalyseContext.cs
// 
// 2020-02-27-3:45 AM
// 
// ---
#endregion
using System.ComponentModel ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;
using NLog ;

namespace CodeAnalysisApp1
{
    public class CodeAnalyseContext
    {
        private readonly string _code ;
        private readonly string _assemblyName ;

        [ JsonIgnore ]
        protected SemanticModel _currentModel ;

        [ JsonIgnore ]
        protected CompilationUnitSyntax _currentRoot ;

        [ JsonIgnore ]
        protected StatementSyntax _statement ;

        [ JsonIgnore ]
        protected SyntaxNode node ;

        public delegate CodeAnalyseContext Factory1(string code , string assemblyName) ;

        public CodeAnalyseContext (string code, string assemblyName )
        {
            _code = code ;
            _assemblyName = assemblyName ;
        }



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
}