﻿
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
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;
using NLog ;

namespace AnalysisFramework
{
    public class CodeAnalyseContext : ICodeAnalyseContext
    {
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

        private readonly string _code ;
        private readonly string _assemblyName ;

        [ JsonIgnore ]
        protected SemanticModel _currentModel ;

        [ JsonIgnore ]
        protected StatementSyntax _statement ;

        [ JsonIgnore ]
        protected SyntaxNode node ;

        private CompilationUnitSyntax _compilationUnit ;
        private readonly SyntaxTree _syntaxTree ;
        private Lazy < CompilationUnitSyntax > _lazy ;

        public delegate ISyntaxTreeContext Factory1(string code , string assemblyName) ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeAnalyseContext (
            SemanticModel         currentModel
          , StatementSyntax       statement
          , SyntaxNode            node
          , ICodeSource           document
          , SyntaxTree            syntaxTree
        ) : this()
        {
            _currentModel = currentModel ;
            _statement    = statement ;
            Node          = node ;
            Document      = document ;
            _syntaxTree    = syntaxTree ;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeAnalyseContext ( ICodeSource document ) : this()
        {
            Document = document ;
        }

        private CodeAnalyseContext ( )
        {
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


        [JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public StatementSyntax Statement { get => _statement ; set => _statement = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public SyntaxNode Node { get => node ; set => node = value ; }

        [ JsonIgnore ]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        public ICodeSource Document { get ; }

        [ JsonIgnore ]
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]

        public SyntaxTree SyntaxTree => _syntaxTree ;

        #region Implementation of ICompilationUnitRootContext
        public CompilationUnitSyntax CompilationUnit
        {
            get
            {
                return _lazy.Value ;
            }
        }

        private CompilationUnitSyntax ValueFactory ( ) { return _syntaxTree.GetCompilationUnitRoot ( ) ; }
        #endregion
    }
}