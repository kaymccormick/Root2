
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
using System ;
using System.ComponentModel ;
using System.Linq ;
#if !NETSTANDARD2_0
using System.Text.Json.Serialization ;
#endif
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

using NLog ;

namespace AnalysisFramework
{
    internal class CodeAnalyseContext : ICodeAnalyseContext
    {
        // ReSharper disable once UnusedMember.Local
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public void Deconstruct (
            out SyntaxTree            syntaxTree
          , out SemanticModel         model
          , out CompilationUnitSyntax compilationUnitSyntax
        )
        {
            syntaxTree = this.SyntaxTree ;
            model = CurrentModel;
            compilationUnitSyntax = CompilationUnit;
        }
        public override string ToString ( )
        {
            return $"{nameof ( _assemblyName )}: {_assemblyName}, {nameof ( _currentModel )}: {_currentModel}, {nameof ( CompilationUnit )}: {CompilationUnit.DescendantNodes().Count()} nodes" ;
        }

        private readonly string _assemblyName ;

        protected SemanticModel _currentModel ;

        protected StatementSyntax _statement ;

        protected SyntaxNode node ;

        private readonly SyntaxTree _syntaxTree ;
        private readonly Lazy < CompilationUnitSyntax > _lazy ;

        public delegate ISyntaxTreeContext Factory1(string code , string assemblyName) ;

        
        public CodeAnalyseContext (
            SemanticModel         currentModel
          , StatementSyntax       statement
          , SyntaxNode            node
          , ICodeSource           document
          , SyntaxTree            syntaxTree
           , string assemblyName
        ) : this( assemblyName )
        {
            _currentModel = currentModel ;
            _statement    = statement ;
            Node          = node ;
            Document      = document ;
            _syntaxTree    = syntaxTree ;
        }

        
        public CodeAnalyseContext ( ICodeSource document , string assemblyName ) : this( assemblyName )
        {
            Document = document ;
        }

        private CodeAnalyseContext ( string assemblyName )
        {
            _assemblyName = assemblyName ;
            _lazy = new Lazy<CompilationUnitSyntax>(
                                                    ValueFactory
                                                   );
        }

        public static ISyntaxTreeContext FromSyntaxTree (
            SyntaxTree               tree
          , string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var comp = AnalysisService.CreateCompilation ( assemblyName , tree ) ;
            return AnalysisService.CreateFromCompilation ( tree , comp ) ;
            // return new CodeAnalyseContext(comp.GetSemanticModel(tree), tree.GetCompilationUnitRoot(), null, tree.GetRoot(), new CodeSource("memory"), tree);
        }

        public static ISyntaxTreeContext FromSyntaxNode (
            SyntaxNode               node
          , string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var tree = SyntaxFactory.SyntaxTree ( node ) ;
            // Logger.Info ( tree.HasCompilationUnitRoot ) ;
            return FromSyntaxTree ( tree , assemblyName , opts ) ;
        }

        #if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public StatementSyntax Statement { get => _statement ; set => _statement = value ; }

#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }

#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public SyntaxNode Node { get => node ; set => node = value ; }
#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public ICodeSource Document { get ; }

#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]

        public SyntaxTree SyntaxTree => _syntaxTree ;

        #region Implementation of ICompilationUnitRootContext
#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public CompilationUnitSyntax CompilationUnit
        {
            get
            {
                return _lazy.Value ;
            }
        }

        public CompilationUnitSyntax Lazy => _lazy.Value ;

        private CompilationUnitSyntax ValueFactory ( ) { return _syntaxTree.GetCompilationUnitRoot ( ) ; }
        #endregion
    }
}