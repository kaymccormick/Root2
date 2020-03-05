#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// AnalysisService.cs
// 
// 2020-03-05-3:10 AM
// 
// ---
#endregion
using System ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisFramework
{
    public static class AnalysisService
    {
        public static CSharpCompilation CreateCompilation (
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

        public static CodeAnalyseContext CreateFromCompilation (
            SyntaxTree        syntaxTree
          , CSharpCompilation compilation
        )
        {
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            return new CodeAnalyseContext (
                                           compilation.GetSemanticModel ( syntaxTree )
                                         , null
                                         , syntaxTree.GetRoot ( )
                                         , new CodeSource ( "memory" )
                                         , syntaxTree
                                          ) ;
        }

        public static CodeAnalyseContext Parse ( [ NotNull ] string code , [ NotNull ] string assemblyName )
        {
            if ( code == null)
            {
                throw new ArgumentNullException ( nameof ( code ) ) ;
            }

            if ( String.IsNullOrWhiteSpace ( code ) )
            {
                throw new ArgumentOutOfRangeException ( nameof ( code ) , "Empty code supplied" ) ;
            }

            if ( assemblyName == null )
            {
                throw new ArgumentNullException ( nameof ( assemblyName ) ) ;
            }

            var syntaxTree = CSharpSyntaxTree.ParseText ( code ) ;
            var compilation = AnalysisService.CreateCompilation ( assemblyName , syntaxTree ) ;
            return AnalysisService.CreateFromCompilation ( syntaxTree , compilation ) ;
        }
    }
}