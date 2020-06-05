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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using AnalysisAppLib.Properties;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NLog;

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
        [NotNull]
        public static CSharpCompilation CreateCompilation([NotNull] string assemblyName
            , [NotNull] SyntaxTree syntaxTree, bool extraRefs = true)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));

            var refs = new List<MetadataReference>();
            var sysPe = MetadataReference.CreateFromFile(
                typeof
                    (string
                    ).Assembly
                    .Location
            );

            refs.Add(sysPe);
            if (extraRefs)
                refs.Add(MetadataReference.CreateFromFile(
                    typeof
                        (Logger
                        ).Assembly
                        .Location
                ));

            var outputL = OutputKind.DynamicallyLinkedLibrary;
            Platform platgorm = Platform.AnyCpu;
            var opts = new CSharpCompilationOptions(outputL, true,
                null, null, null, null,
                OptimizationLevel.Debug, false, true, null, null, default(ImmutableArray<byte>),
                null, platgorm).WithXmlReferenceResolver(new MyXmlResolver());
            var compilation = CSharpCompilation.Create(assemblyName).WithOptions(opts)
                .AddReferences(refs)
                .AddSyntaxTrees(syntaxTree);

            return compilation;
        }

        /// <summary>
        /// Create a context from a compilation.
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static ICodeAnalyseContext CreateFromCompilation(
            [NotNull] SyntaxTree syntaxTree
            , [NotNull] CSharpCompilation compilation
        )
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));

            if (compilation == null) throw new ArgumentNullException(nameof(compilation));

            // ReSharper disable once UnusedVariable
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
            return new CodeAnalyseContext(
                compilation.GetSemanticModel(syntaxTree)
                , null
                , syntaxTree.GetRoot()
                , syntaxTree
                , ""
                , compilation
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static async Task<ICodeAnalyseContext> CreateFromCompilationAsync(
            [NotNull] SyntaxTree syntaxTree
            , [NotNull] CSharpCompilation compilation
        )
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));

            if (compilation == null) throw new ArgumentNullException(nameof(compilation));

            // ReSharper disable once UnusedVariable
            var root = await syntaxTree.GetRootAsync().ConfigureAwait(false);
            return new CodeAnalyseContext(
                compilation.GetSemanticModel(syntaxTree)
                , null
                , root
                , syntaxTree
                , compilation.AssemblyName
                , compilation
            );
        }

        /// <summary>
        /// Create a context from code to parse.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="assemblyName"></param>
        /// <param name="extraRefs"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [NotNull]
        public static ICodeAnalyseContext Parse([NotNull] string code
            , [NotNull] string assemblyName, bool extraRefs = true, string filename = null)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentOutOfRangeException(nameof(code),
                    Resources.AnalysisService_Parse_Empty_code_supplied);

            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            var syntaxTree =
                CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.CSharp7_3), filename);
            var compilation = CreateCompilation(assemblyName, syntaxTree, extraRefs);
            return CreateFromCompilation(syntaxTree, compilation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="assemblyName"></param>
        /// <param name="extraRefs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICodeAnalyseContext Load([NotNull] string filename
            , [NotNull] string assemblyName, bool extraRefs = true)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            var code = File.ReadAllText(filename);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CreateCompilation(assemblyName, syntaxTree, extraRefs);
            return CreateFromCompilation(syntaxTree, compilation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static async Task<ICodeAnalyseContext> LoadAsync(string file, string s, bool b)
        {
            using (var ss = new StreamReader(file))
            {
                var code = await ss.ReadToEndAsync().ConfigureAwait(true);
                var result = await ParseAsync(code, s, b, file).ConfigureAwait(true);
                return result;
            }
        }

        private static async Task<ICodeAnalyseContext> ParseAsync(string code, string s, bool b, string file)
        {
            {
                if (code == null) throw new ArgumentNullException(nameof(code));

                if (string.IsNullOrWhiteSpace(code))
                    throw new ArgumentOutOfRangeException(nameof(code),
                        Resources.AnalysisService_Parse_Empty_code_supplied);

                if (s == null) throw new ArgumentNullException(nameof(s));

                SourceCodeKind kind=SourceCodeKind.Regular;
                DocumentationMode mode=DocumentationMode.Parse;
                var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp7_3,mode,kind);
                var syntaxTree =
                    CSharpSyntaxTree.ParseText(code, cSharpParseOptions, file);
                var compilation = CreateCompilation(s, syntaxTree, b);
                var result = await CreateFromCompilationAsync(syntaxTree, compilation);
                return result;
            }
        }
    }

    public class MyXmlResolver : XmlReferenceResolver
    {
        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return 0;
        }

        /// <inheritdoc />
        public override string? ResolveReference(string path, string baseFilePath)
        {
            return null;
        }

        /// <inheritdoc />
        public override Stream OpenRead(string resolvedPath)
        {
            return null;
        }
    }
}