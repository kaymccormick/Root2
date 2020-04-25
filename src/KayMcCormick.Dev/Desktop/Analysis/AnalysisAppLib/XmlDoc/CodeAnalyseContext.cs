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
using System.Linq ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisAppLib.XmlDoc
{
    internal sealed class CodeAnalyseContext : ICodeAnalyseContext
    {
        public delegate ISyntaxTreeContext Factory1 ( string code , string assemblyName ) ;

        // ReSharper disable once UnusedMember.Local
        private static Logger _logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly string                         _assemblyName ;
        private readonly Lazy < CompilationUnitSyntax > _lazy ;

        private SemanticModel _currentModel ;

        // ReSharper disable once NotAccessedField.Local
        private StatementSyntax _statement ;

        private SyntaxNode _node ;


        public CodeAnalyseContext (
            SemanticModel   currentModel
          , StatementSyntax statement
          , SyntaxNode      node
          , SyntaxTree      syntaxTree
          , string          assemblyName
        ) : this ( assemblyName )
        {
            _currentModel = currentModel ;
            _statement    = statement ;
            Node          = node ;

            SyntaxTree = syntaxTree ;
        }


        private CodeAnalyseContext ( string assemblyName )
        {
            _assemblyName = assemblyName ;
            _lazy         = new Lazy < CompilationUnitSyntax > ( ValueFactory ) ;
        }

        public SyntaxNode Node { get { return _node ; } set { _node = value ; } }


        public SemanticModel CurrentModel
        {
            get { return _currentModel ; }
            set { _currentModel = value ; }
        }

        public SyntaxTree SyntaxTree { get ; }

        public void Deconstruct (
            out SyntaxTree            syntaxTree
          , out SemanticModel         model
          , out CompilationUnitSyntax compilationUnitSyntax
        )
        {
            syntaxTree            = SyntaxTree ;
            model                 = CurrentModel ;
            compilationUnitSyntax = CompilationUnit ;
        }

        public override string ToString ( )
        {
            return
                $"{nameof ( _assemblyName )}: {_assemblyName}, {nameof ( _currentModel )}: {_currentModel}, {nameof ( CompilationUnit )}: {CompilationUnit.DescendantNodes ( ).Count ( )} nodes" ;
        }

        [ NotNull ]
        public static ISyntaxTreeContext FromSyntaxTree (
            [ NotNull ] SyntaxTree               tree
          , [ NotNull ] string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var comp = AnalysisService.CreateCompilation ( assemblyName , tree ) ;
            return AnalysisService.CreateFromCompilation ( tree , comp ) ;
            // return new CodeAnalyseContext(comp.GetSemanticModel(tree), tree.GetCompilationUnitRoot(), null, tree.GetRoot(), new CodeSource("memory"), tree);
        }

        [ NotNull ]
        public static ISyntaxTreeContext FromSyntaxNode (
            [ NotNull ] SyntaxNode               node
          , [ NotNull ] string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var tree = SyntaxFactory.SyntaxTree ( node ) ;
            // Logger.Info ( tree.HasCompilationUnitRoot ) ;
            return FromSyntaxTree ( tree , assemblyName , opts ) ;
        }

        #region Implementation of ICompilationUnitRootContext
        public CompilationUnitSyntax CompilationUnit { get { return _lazy.Value ; } }

        public CompilationUnitSyntax Lazy { get { return _lazy.Value ; } }

        [ NotNull ]
        private CompilationUnitSyntax ValueFactory ( )
        {
            return SyntaxTree.GetCompilationUnitRoot ( ) ;
        }
        #endregion
    }
}