using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisAppLib.XmlDoc;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using AppContext = AnalysisAppLib.AppContext;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MiscCommands
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unknown"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public List<AppTypeInfo> TypesViewModel_Stage1(
            
             out ITypesViewModel unknown
            , IAppDbContext1 db, AppDbContextHelper helper, Func<ITypesViewModel> factory, JsonSerializerOptions jsonSerializerOptions)
        {
            
            var typesViewModel = factory();
            //new TypesViewModel ( context.Scope.Resolve < JsonSerializerOptions > ( ) ) ;
            DebugUtils.WriteLine(
                $"InitializationDateTime : {typesViewModel.InitializationDateTime}"
            );
            typesViewModel.LoadTypeInfo();

            if (typesViewModel == null)
            {
                throw new ArgumentNullException(nameof(typesViewModel));
            }

            List<AppTypeInfo> r;

            {
                var appTypeInfos = typesViewModel.GetAppTypeInfos();
                var typeInfos = appTypeInfos as AppTypeInfo[] ?? appTypeInfos.ToArray();
                foreach (var appTypeInfo in typeInfos)
                {
                    DebugUtils.WriteLine(
                        $"Synchronizing {appTypeInfo.Title} ({appTypeInfo.Fields.Count})"
                    );

                    if (appTypeInfo.AppClrType != null)
                    {
                        continue;
                    }

                    
                    var clr = helper.FindOrAddClrType(db, appTypeInfo.Type);
                    appTypeInfo.AppClrType = clr;
                    if (appTypeInfo.Id != 0)
                    {
                        db.AppTypeInfos.Update(appTypeInfo);
                    }
                    else
                    {
                        db.AppTypeInfos.Add(appTypeInfo);
                    }
                }

                db.SaveChanges();
                r = db.AppTypeInfos.ToList();
            }

            WriteThisTypesViewModel(
                typesViewModel
                , model => Path.Combine(DataOutputPath, "model-v1.xaml")
            );
            DumpModelToJson(typesViewModel, jsonSerializerOptions, Path.Combine(DataOutputPath, "types-v1.json")
            );
            unknown = typesViewModel;
            return r;
        }

        private  void DumpModelToJson([NotNull] ITypesViewModel typesViewModel, JsonSerializerOptions jsonSerializerOptions, string jsonFilename = null
        )
        {
            using (var utf8Json = File.Open(jsonFilename, FileMode.Create))
            {
                var infos = typesViewModel.Map.Values.Cast<AppTypeInfo>().ToList();
                var writer = new Utf8JsonWriter(
                    utf8Json
                    , new JsonWriterOptions { Indented = true }
                );
                jsonSerializerOptions.WriteIndented = true;
                if (!jsonSerializerOptions
                    .Converters.Select(conv => conv.CanConvert(typeof(Type)))
                    .Any())

                {
                    throw new InvalidOperationException("no type converter");
                }

                foreach (var jsonConverter in jsonSerializerOptions.Converters)
                {
                    Console.WriteLine(jsonConverter);
                }

                try
                {
                    JsonSerializer.Serialize(writer, infos, jsonSerializerOptions);
                }
                catch (Exception)
                {
                    // ignored
                }

                writer.Flush();
            }
        }
        [NotNull]
        private static string ModelXamlFilename
        {
            get { return Path.Combine(DataOutputPath, ModelXamlFilenamePart); }
        }

        private void WriteThisTypesViewModel(
            [NotNull]   ITypesViewModel model
            , [CanBeNull] Func<ITypesViewModel, string> filenameFunc = null
        )
        {
            var xamlFilename = filenameFunc == null ? ModelXamlFilename : filenameFunc(model);

            DebugUtils.WriteLine($"Writing {xamlFilename}");
            var writer = XmlWriter.Create(
                xamlFilename
                , new XmlWriterSettings { Indent = true, Async = true }
            );
            foreach (var keyValuePair in model.Map.Dict)
            {
                model.Map2.Dict[keyValuePair.Key.StringValue] = keyValuePair.Value;
            }

            XamlWriter.Save(model, writer);
            writer.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="helper"></param>
        /// <param name="contextScope"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [TitleMetadata("Build Types View")]
        [UsedImplicitly]
#pragma warning disable 1998
        public  async Task BuildTypeViewAsync(
#pragma warning restore 1998
            [NotNull] IBaseLibCommand command, IAppDbContext1 db, ILifetimeScope scope
            ,
            AppDbContextHelper helper, JsonSerializerOptions options)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

           

            DebugUtils.WriteLine("Begin initialize TypeViewModel");

            //var db = new AppDbContext ( ) ;
            {
                db.AppClrType.RemoveRange(db.AppClrType);
                db.AppTypeInfos.RemoveRange(db.AppTypeInfos);
                db.SyntaxFieldInfo.RemoveRange(db.SyntaxFieldInfo);
                await db.SaveChangesAsync();
            }

            {
                if (db.AppTypeInfos.Any()
                    || db.AppClrType.Any())
                {
                    throw new InvalidOperationException();
                }
            }

            var factory = scope.Resolve<Func<ITypesViewModel>>();
            var r = TypesViewModel_Stage1(out var typesViewModel, db, helper, factory, scope.Resolve<JsonSerializerOptions>());
            var t2 = new TypesViewModel(r);
            t2.BeginInit();
            t2.EndInit();
            var sts = scope.Resolve<ISyntaxTypesService>();
            var collectionMap = sts.CollectionMap();

            SyntaxTypesService.LoadSyntax(typesViewModel, collectionMap);
            // foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            // {
            // typesViewModel.PopulateFieldTypes ( ati ) ;
            // }

            typesViewModel.DetailFields();

            WriteModelToDatabase(typesViewModel, db, helper);
            WriteThisTypesViewModel(typesViewModel);
            DumpModelToJson(typesViewModel, options, Path.Combine(DataOutputPath, TypesJsonFilename)
            );
        }
        private const string ModelXamlFilenamePart = "model.xaml";
        private const string DataOutputPath = @"C:\data\logs";
        private const string TypesJsonFilename = "types.json";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typesViewModel"></param>
        /// <param name="db"></param>
        /// <param name="helper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void WriteModelToDatabase(
            [NotNull] ITypesViewModel typesViewModel
            , IAppDbContext1 db, AppDbContextHelper helper
        )
        {
            
            if (typesViewModel == null)
            {
                throw new ArgumentNullException(nameof(typesViewModel));
            }

            {
                var appTypeInfos = typesViewModel.GetAppTypeInfos();
                var typeInfos = appTypeInfos as AppTypeInfo[] ?? appTypeInfos.ToArray();
                foreach (var appTypeInfo in typeInfos)
                {
                    DebugUtils.WriteLine(
                        $"Synchronizing {appTypeInfo.Title} ({appTypeInfo.Fields.Count})"
                    );
                    appTypeInfo.Version++;
                    var syntaxFieldCollection = appTypeInfo.Fields;
                    foreach (SyntaxFieldInfo o in syntaxFieldCollection)
                    {
                        db.SyntaxFieldInfo.Add(o);
                    }
                    // if ( appTypeInfo.AppClrType != null )
                    // {
                    // continue ;
                    // }


                    if (appTypeInfo.AppClrType == null)
                    {
                        
                        var clr = helper.FindOrAddClrType(db, appTypeInfo.Type);
                        appTypeInfo.AppClrType = clr;
                    }

                    continue;
                    if (appTypeInfo.Id != 0)
                    {
                        db.AppTypeInfos.Update(appTypeInfo);
                    }
                    else
                    {
                        db.AppTypeInfos.Add(appTypeInfo);
                    }
                }

                db.SaveChanges();
            }
        }

        public void PopulateSet (
            [ NotNull ] AppTypeInfoCollection subTypeInfos
            , ISet<Type> set
        )
        {
            foreach ( var rootSubTypeInfo in subTypeInfos )
            {
                if ( rootSubTypeInfo.Type.IsAbstract == false )
                {
                    set.Add ( rootSubTypeInfo.Type ) ;
                }

                PopulateSet ( rootSubTypeInfo.SubTypeInfos , set ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="SolutionFilePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ TitleMetadata ( "Process solution" ) ]
        [ UsedImplicitly ]
        // ReSharper disable once FunctionComplexityOverflow
        public  async Task ProcessSolutionAsync(IBaseLibCommand command, string SolutionFilePath)
        {
            //options.WriteIndented = true ;
            var workspace = MSBuildWorkspace.Create ( ) ;
            var optionsSolutionFile = SolutionFilePath;//context.Options?.SolutionFile ?? SolutionFilePath ;
            if ( String.IsNullOrEmpty ( optionsSolutionFile ) )
            {
                throw new InvalidOperationException ( "No solution file" ) ;
            }

            var solution = await workspace.OpenSolutionAsync ( optionsSolutionFile ) ;
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
                var compilation = await project.GetCompilationAsync ( ) ;
                foreach ( var diagnostic in compilation
                    .GetDiagnostics ( )
                    .Where (
                        d => ! d.IsSuppressed
                             && d.Severity >= DiagnosticSeverity.Info
                    ) )
                {
                    DebugUtils.WriteLine ( diagnostic.ToString ( ) ) ;
                }

                var compilationAssembly = compilation.Assembly ;
                foreach ( var tn in compilationAssembly.TypeNames )
                {
                    DebugUtils.WriteLine ( compilationAssembly.Name ) ;
                    DebugUtils.WriteLine ( tn ) ;
                }

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
                        if ( declared.DeclaredAccessibility != Accessibility.Public
                             || ! SupportsDocumentationComments ( node ) )
                        {
                            DebugUtils.WriteLine (
                                $"Documentation accessibility is {declared.DeclaredAccessibility}"
                            ) ;
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
                                         ?? throw new InvalidOperationException (
                                             "Null from HandleDocElementNode"
                                         ) ;
                                }
                                else
                                {
                                    o1 = XmlDocElements.CreateCodeDocumentationElementType (
                                             node
                                             , docId
                                         )
                                         ?? throw new InvalidOperationException (
                                             "Null from CreateCodeDocumentationElementType"
                                         ) ;
                                }

                                if ( String.IsNullOrWhiteSpace ( xml1 ) )
                                {
                                    o1.NeedsAttention = true ;
                                }

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
}