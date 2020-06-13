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

using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NLog;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CodeAnalyseContext : ICodeAnalyseContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="assemblyName"></param>
        public delegate ISyntaxTreeContext Factory1 ( string code , string assemblyName ) ;

        // ReSharper disable once UnusedMember.Local
        private static Logger _logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly string                         _assemblyName ;
        private readonly Lazy < CompilationUnitSyntax > _lazy ;

        private SemanticModel _currentModel ;

        // ReSharper disable once NotAccessedField.Local
        private StatementSyntax _statement ;
        /// <summary>
        /// 
        /// </summary>
        public CSharpCompilation Compilation { get; }

        private SyntaxNode _node ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentModel"></param>
        /// <param name="statement"></param>
        /// <param name="node"></param>
        /// <param name="syntaxTree"></param>
        /// <param name="assemblyName"></param>
        /// <param name="compilation"></param>
        public CodeAnalyseContext(SemanticModel currentModel
            , StatementSyntax statement
            , SyntaxNode node
            , SyntaxTree syntaxTree
            , string assemblyName, CSharpCompilation compilation) : this ( assemblyName )
        {
            _currentModel = currentModel ;
            _statement    = statement ;
            Compilation = compilation;
            Node          = node ;

            SyntaxTree = syntaxTree ;
        }


        private CodeAnalyseContext ( string assemblyName )
        {
            _assemblyName = assemblyName ;
            _lazy         = new Lazy < CompilationUnitSyntax > ( ValueFactory ) ;
        }

        /// <summary>
        /// SyntaxNode for analysis
        /// </summary>
        public SyntaxNode Node { get { return _node ; } set { _node = value ; } }


        /// <summary>
        /// Semantic model for analysis
        /// </summary>
        public SemanticModel CurrentModel
        {
            get { return _currentModel ; }
            set { _currentModel = value ; }
        }

        /// <summary>
        /// Syntax tree for analysis
        /// </summary>
        public SyntaxTree SyntaxTree { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="model"></param>
        /// <param name="compilationUnitSyntax"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return
                $"{nameof ( _assemblyName )}: {_assemblyName}, {nameof ( _currentModel )}: {_currentModel}, {nameof ( CompilationUnit )}: {CompilationUnit.DescendantNodes ( ).Count ( )} nodes" ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="assemblyName"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static ISyntaxTreeContext FromSyntaxTree (
            [ JetBrains.Annotations.NotNull ] SyntaxTree               tree
          , [ JetBrains.Annotations.NotNull ] string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var comp = AnalysisService.CreateCompilation ( assemblyName , tree, true) ;
            return AnalysisService.CreateFromCompilation ( tree , comp ) ;
            // return new CodeAnalyseContext(comp.GetSemanticModel(tree), tree.GetCompilationUnitRoot(), null, tree.GetRoot(), new CodeSource("memory"), tree);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="assemblyName"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static ISyntaxTreeContext FromSyntaxNode (
            [ JetBrains.Annotations.NotNull ] SyntaxNode               node
          , [ JetBrains.Annotations.NotNull ] string                   assemblyName
          , CSharpCompilationOptions opts = null
        )
        {
            var tree = SyntaxFactory.SyntaxTree ( node ) ;
            // Logger.Info ( tree.HasCompilationUnitRoot ) ;
            return FromSyntaxTree ( tree , assemblyName , opts ) ;
        }

        #region Implementation of ICompilationUnitRootContext
        /// <summary>
        /// 
        /// </summary>
        public CompilationUnitSyntax CompilationUnit { get { return _lazy.Value ; } }

        /// <summary>
        /// 
        /// </summary>
        public CompilationUnitSyntax Lazy { get { return _lazy.Value ; } }

        [ JetBrains.Annotations.NotNull ]
        private CompilationUnitSyntax ValueFactory ( )
        {
            return SyntaxTree.GetCompilationUnitRoot ( ) ;
        }
        #endregion
    }
}