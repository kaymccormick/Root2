using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using AnalysisAppLib;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Command;
using KmDevLib;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;


namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtractDocCommentsCommand
    {
    private MyReplaySubject<CodeElementDocumentation> _replay;
    private readonly MyReplaySubject<Document> _docs;

    public ExtractDocCommentsCommand(MyReplaySubject<CodeElementDocumentation> replay, MyReplaySubject<Document> docs)
    {
        replay.ListView = false;
        docs.ListView = false;
        _replay = replay;
        _docs = docs;
    }

    /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="SolutionFilePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ TitleMetadata ( "Process solution" ) ]
        [RequiredParameterMetadata("Solution file")]
        [ UsedImplicitly ]
        // ReSharper disable once FunctionComplexityOverflow
        public async Task<IAppCommandResult> ProcessSolutionAsync(IBaseLibCommand command, string SolutionFilePath)
        {
            //options.WriteIndented = true ;
            var workspace = MSBuildWorkspace.Create ( ) ;
            var optionsSolutionFile = SolutionFilePath;//context.Options?.SolutionFile ?? SolutionFilePath ;
            if ( String.IsNullOrEmpty ( optionsSolutionFile ) )
            {
                throw new AppInvalidOperationException ( "No solution file" ) ;
            }

            var solution = await workspace.OpenSolutionAsync(optionsSolutionFile);
                // new ProgressWithCompletion<ProjectLoadProgress>(progress => { }), CancellationToken.None);
            var documentsOut = new List < CodeElementDocumentation > ( ) ;
            var solutionProjects = solution.Projects ;

            foreach ( var project in Enumerable.Where<Project>(solutionProjects, proj => proj.Name != "Explorer"
                                                                                         && proj.CompilationOptions
                                                                                             ?.OutputKind
                                                                                         == OutputKind
                                                                                             .DynamicallyLinkedLibrary
                                                                                         || proj.CompilationOptions
                                                                                             ?.OutputKind
                                                                                         == OutputKind
                                                                                             .WindowsApplication
            ) )
            {
                var callers = new List < CallerInfo > ( ) ;

                DebugUtils.WriteLine ( $"{project} {project.CompilationOptions.OutputKind}" ) ;
                // ReSharper disable once UnusedVariable
                var compilation = await project.GetCompilationAsync().ConfigureAwait(false);

                var compilationAssembly = compilation.Assembly ;
                // foreach ( var tn in compilationAssembly.TypeNames )
                // {
                    // DebugUtils.WriteLine ( compilationAssembly.Name ) ;
                    // DebugUtils.WriteLine ( tn ) ;
                // }

#if false
                foreach ( var symbol in compilation.GetSymbolsWithName ( s => true ) )
                {
                    if ( ! symbol.ContainingAssembly.Equals ( compilationAssembly ) )
                    {
                        DebugUtils.WriteLine ( $"Skipping {symbol}" ) ;
                        continue ;
                    }

                    DebugUtils.WriteLine (
                        String.Join (
                            ";"
                            , compilation.SyntaxTrees.Select (
                                dt => dt
                                    .FilePath
                            )
                        )
                    ) ;
                    DebugUtils.WriteLine (
                        $"{symbol.ToDisplayString ( )} {symbol.DeclaredAccessibility}"
                    ) ;

                    var res = await SymbolFinder.FindCallersAsync ( symbol , solution ) ;
                    var uses = 0 ;
                    foreach ( var use in res )
                    {
                        // DebugUtils.WriteLine ( "Symbol kind "   + use.CalledSymbol.Kind ) ;
                        // DebugUtils.WriteLine ( "Called symbol " + use.CalledSymbol.ToString ( ) ) ;
                        // DebugUtils.WriteLine (
                        // "Calling symbol " + use.CallingSymbol.ToString ( )
                        // ) ;
                        callers.Add (
                            new CallerInfo (
                                use.CalledSymbol.ToDisplayString ( )
                                , use.CallingSymbol.ToDisplayString ( )
                                , use.IsDirect
                                , use.Locations.Select (
                                    l => {
                                        var
                                            fileLinePositionSpan
                                                = l
                                                    .GetMappedLineSpan ( ) ;
                                        return new
                                            LocationInfo (
                                                fileLinePositionSpan
                                                    .Path
                                                , fileLinePositionSpan
                                                    .StartLinePosition
                                                    .Character
                                                , fileLinePositionSpan
                                                    .StartLinePosition
                                                    .Line
                                                , fileLinePositionSpan
                                                    .EndLinePosition
                                                    .Character
                                                , fileLinePositionSpan
                                                    .EndLinePosition
                                                    .Line
                                                , l
                                                    .MetadataModule
                                                    ?.MetadataName
                                                , l
                                                    .SourceSpan
                                                    .Start
                                                , l
                                                    .SourceSpan
                                                    .End
                                            ) ;
                                    }
                                )
                            )
                        ) ;
                        uses += use.Locations.Count ( ) ;
                    }

                    if ( uses > 0 )
                    {
                        DebugUtils.WriteLine ( "Total usages is " + uses ) ;
                    }
                    else
                    {
                        DebugUtils.WriteLine ( "0 uses" ) ;
                        DebugUtils.WriteLine ( "Symbol kind "   + symbol.Kind ) ;
                        DebugUtils.WriteLine ( "Called symbol " + symbol ) ;
                    }
                }
#endif
                // foreach ( var namespaceOrTypeSymbol in compilation
                // .GetCompilationNamespace (compilationAssembly.ContainingNamespace )
                // .GetMembers ( ) )
                // {
                // if ( namespaceOrTypeSymbol.IsNamespace )
                // {

                // } else if ( namespaceOrTypeSymbol.IsType )
                // {
                // var c = namespaceOrTypeSymbol.ContainingType ;

                // }
                // }
                foreach ( var doc in project.Documents )
                {
                    _docs.Subject1.OnNext(doc);
                    // var textAsync = await doc.GetTextAsync() ;
                    // var classified = await Microsoft.CodeAnalysis.Classification.Classifier.GetClassifiedSpansAsync (
                    // doc
                    // , new
                    // TextSpan (
                    // 0
                    // , textAsync
                    // .Length
                    // )
                    // ) ;

                    // foreach ( var classifiedSpan in classified )
                    // {
                    // DebugUtils.WriteLine($"{classifiedSpan.ClassificationType} : {classifiedSpan.TextSpan}");
                    // }

                    // ReSharper disable once UnusedVariable
                    var model = await doc.GetSemanticModelAsync ( ) ;

                    Console.WriteLine ( doc.Name ) ;
                    // ReSharper disable once UnusedVariable
                    var tree = await doc.GetSyntaxRootAsync ( ) ;

                    var visitor = new SyntaxWalker2 ( model ) ;
                    visitor.Visit ( tree ) ;
                    foreach ( var node in tree
                        .DescendantNodesAndSelf ( )
                        .OfType < MemberDeclarationSyntax > ( ) )
                    {
                        var declared = model.GetDeclaredSymbol ( node ) ;
                        if ( declared == null )
                        {
                            continue ;
                        }

                        var xml1 = declared.GetDocumentationCommentXml ( ) ;
                        if ( declared.DeclaredAccessibility != Microsoft.CodeAnalysis.Accessibility.Public
                             || !SupportsDocumentationComments ( node ) )
                        {
                            // DebugUtils.WriteLine (
                                // $"Documentation accessibility is {declared.DeclaredAccessibility}"
                            // ) ;
                        }
                        else
                        {
                            var docId = declared.GetDocumentationCommentId ( ) ;

                            // ReSharper disable once UnusedVariable
                            var o = new
                            {
                                // ReSharper disable once RedundantAnonymousTypePropertyName
                                docId    = docId
                                , xml      = xml1
                                , declared = declared.ToDisplayString ( )
                            } ;

                            try
                            {
                                XDocument doc1 = null ;
                                if ( ! String.IsNullOrWhiteSpace ( xml1 ) )
                                {
                                    doc1 = XDocument.Parse ( xml1 ) ;
                                }

                                CodeElementDocumentation o1 ;
                                if ( doc1 != null )
                                {
                                    o1 = XmlDocElements.HandleDocElementNode (
                                             doc1
                                             , docId
                                             , node
                                             , declared
                                         )
                                         ?? throw new AppInvalidOperationException (
                                             "Null from HandleDocElementNode"
                                         ) ;
                                }
                                else
                                {
                                    o1 = XmlDocElements.CreateCodeDocumentationElementType (
                                             node
                                             , docId
                                         )
                                         ?? throw new AppInvalidOperationException (
                                             "Null from CreateCodeDocumentationElementType"
                                         ) ;
                                }

                                if ( String.IsNullOrWhiteSpace ( xml1 ) )
                                {
                                    o1.NeedsAttention = true ;
                                }

                                _replay.Subject1.OnNext(o1);
                                documentsOut.Add ( o1 ) ;
                            }
                            catch ( Exception ex )
                            {
                                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                            }

                            // DebugUtils.WriteLine ( JsonSerializer.Serialize ( o ) ) ;
                        }
                    }


                    // => tuple.Item2.Symbol
                    // != null
                    // && tuple.Item2.Symbol
                    // .DeclaredAccessibility
                    // == Accessibility.Public
                    // ) )
                    // {
                    // if ( ! tuple.Item1.GetLeadingTrivia ( )
                    // .Any ( SyntaxKind.SingleLineDocumentationCommentTrivia ) )
                    // {
                    // DebugUtils.WriteLine(DocumentationCommentId.CreateDeclarationId(tuple.Item2.Symbol));
                    // }

                    // var gen =
                    // FindLogUsages.GenTransforms.Transform_CSharp_Node (
                    // ( CSharpSyntaxNode ) tree
                    // ) ;

                    // DebugUtils.WriteLine ( JsonSerializer.Serialize ( gen , options ) ) ;
                }

                var jsonOut = JsonSerializer.Serialize (
                    callers
                    , new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                ) ;
                File.WriteAllText ( @"C:\temp\" + project.Name + ".json" , jsonOut ) ;
            }

            var li = new ArrayList ( ) ;
            foreach ( var codeElementDocumentation in documentsOut )
            {
                if ( codeElementDocumentation != null )
                {
                    li.Add ( codeElementDocumentation ) ;
                }
            }


            var xmlWriter = XmlWriter.Create (
                @"C:\temp\docs.xaml"
                , new XmlWriterSettings { Indent = true }
            ) ;
            XamlWriter.Save ( li , xmlWriter ) ;
            xmlWriter.Close ( ) ;
            return AppCommandResult.Success;
        }

        private static bool SupportsDocumentationComments(
            [CanBeNull] MemberDeclarationSyntax member
        )
        {
            if (member == null)
            {
                return false;
            }

            switch (member.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return true;

                default: return false;
            }
        }
    }

    [MetadataAttribute()]
    public class RequiredParameterMetadataAttribute : Attribute
    {
        public string ParameterName { get; set; }

        /// <inheritdoc />
        public RequiredParameterMetadataAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}