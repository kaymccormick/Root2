﻿
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ NotNull ]
        public static CSharpCompilation CreateCompilation (
            [ NotNull ] string     assemblyName
          , [ NotNull ] SyntaxTree syntaxTree
        )
        {
            if ( assemblyName == null )
            {
                throw new ArgumentNullException ( nameof ( assemblyName ) ) ;
            }

            if ( syntaxTree == null )
            {
                throw new ArgumentNullException ( nameof ( syntaxTree ) ) ;
            }

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
                                                                                                )
                                                              )
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
                                          ) ;
        }

        /// <summary>
        /// Create a context from code to parse.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [ NotNull ]
        public static ICodeAnalyseContext Parse (
            [ NotNull ] string code
          , [ NotNull ] string assemblyName
        )
        {
            if ( code == null )
            {
                throw new ArgumentNullException ( nameof ( code ) ) ;
            }

            if ( string.IsNullOrWhiteSpace ( code ) )
            {
                throw new ArgumentOutOfRangeException ( nameof ( code ) , "Empty code supplied" ) ;
            }

            if ( assemblyName == null )
            {
                throw new ArgumentNullException ( nameof ( assemblyName ) ) ;
            }

            var syntaxTree = CSharpSyntaxTree.ParseText ( code ) ;
            var compilation = CreateCompilation ( assemblyName , syntaxTree ) ;
            return CreateFromCompilation ( syntaxTree , compilation ) ;
        }
    }
}