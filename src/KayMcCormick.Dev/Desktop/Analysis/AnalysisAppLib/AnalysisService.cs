
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AnalysisAppLib.Properties ;
using AnalysisAppLib.XmlDoc ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Generic analysis service.
    /// </summary>
    public static class AnalysisService
    {
        /// <summary>
        /// Create a compilation.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="syntaxTree"></param>
        /// <param name="extraRefs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ NotNull ]
        public static CSharpCompilation CreateCompilation([NotNull] string assemblyName
            , [NotNull] SyntaxTree syntaxTree, bool extraRefs = true)
        {
            if ( assemblyName == null )
            {
                throw new ArgumentNullException ( nameof ( assemblyName ) ) ;
            }

            if ( syntaxTree == null )
            {
                throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;
            }

            var refs = new List<MetadataReference>();
            var sysPe = MetadataReference.CreateFromFile(
                typeof
                    (string
                    ).Assembly
                    .Location
            );

            refs.Add(sysPe);
            if (extraRefs)
            {
                refs.Add(MetadataReference.CreateFromFile(
                    typeof
                        (Logger
                        ).Assembly
                        .Location
                ));
            }
            var compilation = CSharpCompilation.Create ( assemblyName )
                                               .AddReferences (refs)
                                               .AddSyntaxTrees ( syntaxTree ) ;

            return compilation ;
        }

        /// <summary>
        /// Create a context from a compilation.
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ NotNull ]
        public static ICodeAnalyseContext CreateFromCompilation (
            [ NotNull ] SyntaxTree        syntaxTree
          , [ NotNull ] CSharpCompilation compilation
        )
        {
            if ( syntaxTree == null )
            {
                throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;
            }

            if ( compilation == null )
            {
                throw new ArgumentNullException ( nameof ( compilation ) ) ;
            }

            // ReSharper disable once UnusedVariable
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            return new CodeAnalyseContext (
                                           compilation.GetSemanticModel ( syntaxTree )
                                         , null
                                         , syntaxTree.GetRoot ( )
                                         , syntaxTree
                                         , ""
                                           , compilation
                                          ) ;
        }

        /// <summary>
        /// Create a context from code to parse.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="assemblyName"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [ NotNull ]
        public static ICodeAnalyseContext Parse([NotNull] string code
            , [NotNull] string assemblyName, bool extraRefs = true)
        {
            if ( code == null )
            {
                throw new ArgumentNullException ( nameof ( code ) ) ;
            }

            if ( string.IsNullOrWhiteSpace ( code ) )
            {
                throw new ArgumentOutOfRangeException ( nameof ( code ) , Resources.AnalysisService_Parse_Empty_code_supplied ) ;
            }

            if ( assemblyName == null )
            {
                throw new ArgumentNullException ( nameof ( assemblyName ) ) ;
            }

            var syntaxTree = CSharpSyntaxTree.ParseText ( code ) ;
            var compilation = CreateCompilation ( assemblyName , syntaxTree, extraRefs) ;
            return CreateFromCompilation ( syntaxTree , compilation ) ;
        }
        public static ICodeAnalyseContext Load([NotNull] string filename
            , [NotNull] string assemblyName, bool extraRefs = true)
        {

            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            var code = File.ReadAllText(filename);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CreateCompilation(assemblyName, syntaxTree, extraRefs);
            return CreateFromCompilation(syntaxTree, compilation);
        }

    }
}