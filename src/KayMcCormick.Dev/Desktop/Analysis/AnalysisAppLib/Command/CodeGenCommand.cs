using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnalysisAppLib.Syntax;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisAppLib.Command
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CodeGenCommand : IBaseLibCommand
    {
        private static readonly string[] AssemblyRefs = {
                                                            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Microsoft.CSharp.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll"
                                                          , @"C:\Users\mccor.LAPTOP-T6T0BN1K\.nuget\packages\nlog\4.6.8\lib\net45\NLog.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Configuration.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.DataSetExtensions.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.IO.Compression.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Net.Http.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Runtime.Serialization.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.ServiceModel.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Transactions.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll"
                                                          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll"
                                                        };

        private readonly Func < ITypesViewModel > _factory ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public CodeGenCommand (Func <ITypesViewModel> factory ) { _factory = factory ; }

        private object _argument ;
        #region Implementation of IBaseLibCommand
        /// <inheritdoc />
        public object Argument { get { return _argument ; } set { _argument = value ; } }

        /// <inheritdoc />
        public async Task < IAppCommandResult > ExecuteAsync ( )
        {
            var outputFunc = (Action<string>)Argument;

            void DebugOut ( string s1 )
            {
                DebugUtils.WriteLine ( s1 ) ;
                outputFunc ( s1 ) ;
            }

            outputFunc("Beginning");
            var model1 = _factory();
            var types =
                new SyntaxList<MemberDeclarationSyntax
                >(); //new [] { SyntaxFactory.ClassDeclaration("SyntaxToken")} ) ;
            outputFunc($"{model1.Map.Count} Entries in Type map");
            foreach (Type mapKey in model1.Map.Keys)
            {
                DebugOut($"{mapKey}");
                var t = (AppTypeInfo)model1.Map[mapKey];
                var members = new SyntaxList<MemberDeclarationSyntax>();
                foreach (SyntaxFieldInfo tField in t.Fields)
                {
                    DebugOut($"{tField}");
                    if (tField.Type == null
                         && tField.TypeName != "bool")
                    {
                        continue;
                    }

                    var accessorDeclarationSyntaxes = new SyntaxList<AccessorDeclarationSyntax>(
                                                                                                    new
                                                                                                    []
                                                                                                    {
                                                                                                        SyntaxFactory
                                                                                                           .AccessorDeclaration (
                                                                                                                                 SyntaxKind
                                                                                                                                    .GetAccessorDeclaration
                                                                                                                                )
                                                                                                           .WithSemicolonToken (
                                                                                                                                SyntaxFactory
                                                                                                                                   .Token (
                                                                                                                                           SyntaxKind
                                                                                                                                              .SemicolonToken
                                                                                                                                          )
                                                                                                                               )
                                                                                                      , SyntaxFactory
                                                                                                       .AccessorDeclaration (
                                                                                                                             SyntaxKind
                                                                                                                                .SetAccessorDeclaration
                                                                                                                            )
                                                                                                       .WithSemicolonToken (
                                                                                                                            SyntaxFactory
                                                                                                                               .Token (
                                                                                                                                       SyntaxKind
                                                                                                                                          .SemicolonToken
                                                                                                                                      )
                                                                                                                           )
                                                                                                    }
                                                                                                   );
                    var accessorListSyntax =
                        SyntaxFactory.AccessorList(accessorDeclarationSyntaxes);

                    var tFieldTypeName = tField.TypeName;
                    if (tFieldTypeName == "SyntaxTokenList")
                    {
                        tFieldTypeName = "List<SyntaxToken>";
                    }
                    else if (tFieldTypeName.StartsWith("SyntaxList<" , StringComparison.Ordinal ))
                    {
                        tFieldTypeName = tFieldTypeName.Replace("SyntaxList<", "List<");
                    }
                    else if (tFieldTypeName.StartsWith("SeparatedSyntaxList<" , StringComparison.Ordinal ))
                    {
                        tFieldTypeName =
                            tFieldTypeName.Replace("SeparatedSyntaxList<", "List<");
                    }

                    var typeSyntax =
                        SyntaxFactory.ParseTypeName(
                                                     tFieldTypeName
                                                    ); /*SyntaxFactory.IdentifierName (
                                                                                                                 tField
                                                                                                                    .Type
                                                                                                                    .Name
                                                                                                                ) ;*/
                    if (typeSyntax is GenericNameSyntax gen)
                    {
                        var ss = (SimpleNameSyntax)gen.TypeArgumentList.Arguments[0];
                        typeSyntax = gen.WithTypeArgumentList(
                                                               SyntaxFactory.TypeArgumentList(
                                                                                               new
                                                                                                       SeparatedSyntaxList
                                                                                                       <TypeSyntax
                                                                                                       >()
                                                                                                  .Add(
                                                                                                        SyntaxFactory
                                                                                                           .ParseTypeName(
                                                                                                                           "Poco"
                                                                                                                           + ss
                                                                                                                            .Identifier
                                                                                                                            .Text
                                                                                                                          )
                                                                                                       )
                                                                                              )
                                                              );
                    }
                    else if (tField.TypeName != "bool")
                    {
                        typeSyntax =
                            SyntaxFactory.ParseTypeName(
                                                         "Poco"
                                                         + ((SimpleNameSyntax)typeSyntax)
                                                          .Identifier.Text
                                                        );
                    }

                    var tokens = new List<SyntaxToken>
                                 {
                                     SyntaxFactory.Token ( SyntaxKind.PublicKeyword )
                                   , tField.Override
                                         ? SyntaxFactory.Token ( SyntaxKind.OverrideKeyword )
                                         : SyntaxFactory.Token ( SyntaxKind.VirtualKeyword )
                                 };


                    members = members.Add(
                                           SyntaxFactory.PropertyDeclaration(
                                                                              new SyntaxList<
                                                                                  AttributeListSyntax
                                                                              >()
                                                                            , SyntaxFactory
                                                                                 .TokenList(
                                                                                             tokens
                                                                                                .ToArray()
                                                                                            )
                                                                            , typeSyntax
                                                                            , null
                                                                            , SyntaxFactory
                                                                                 .Identifier(
                                                                                              tField
                                                                                                 .Name
                                                                                             )
                                                                            , accessorListSyntax
                                                                             )
                                          );
                }

                var classDecl = SyntaxFactory
                               .ClassDeclaration("Poco" + mapKey.Name)
                               .WithModifiers(
                                               SyntaxTokenList.Create(
                                                                       SyntaxFactory.Token(
                                                                                            SyntaxKind
                                                                                               .PublicKeyword
                                                                                           )
                                                                      )
                                              )
                               .WithMembers(members);
                if (t.ParentInfo != null)
                {
                    classDecl = classDecl.WithBaseList(
                                                        SyntaxFactory.BaseList(
                                                                                new
                                                                                    SeparatedSyntaxList
                                                                                    <BaseTypeSyntax
                                                                                    >().Add(
                                                                                               SyntaxFactory
                                                                                                  .SimpleBaseType(
                                                                                                                   SyntaxFactory
                                                                                                                      .IdentifierName(
                                                                                                                                       "Poco"
                                                                                                                                       + t
                                                                                                                                        .ParentInfo
                                                                                                                                        .Type
                                                                                                                                        .Name
                                                                                                                                      )
                                                                                                                  )
                                                                                              )
                                                                               )
                                                       );
                }

                types = types.Add(classDecl);
            }

            DebugOut("About to build compilation unit");
            var compl = SyntaxFactory.CompilationUnit()
                                     .WithUsings(
                                                  new SyntaxList<UsingDirectiveSyntax>(
                                                                                           new[]
                                                                                           {
                                                                                               SyntaxFactory
                                                                                                  .UsingDirective (
                                                                                                                   SyntaxFactory
                                                                                                                      .ParseName (
                                                                                                                                  "System"
                                                                                                                                 )
                                                                                                                  )
                                                                                             , SyntaxFactory
                                                                                                  .UsingDirective (
                                                                                                                   SyntaxFactory
                                                                                                                      .ParseName (
                                                                                                                                  "System.Collections.Generic"
                                                                                                                                 )
                                                                                                                  )
                                                                                           }
                                                                                          )
                                                 )
                                     .WithMembers(
                                                   new SyntaxList<MemberDeclarationSyntax>(
                                                                                               SyntaxFactory
                                                                                                  .NamespaceDeclaration(
                                                                                                                         SyntaxFactory
                                                                                                                            .ParseName(
                                                                                                                                        "PocoSyntax"
                                                                                                                                       )
                                                                                                                        )
                                                                                                  .WithMembers(
                                                                                                                types
                                                                                                               )
                                                                                              )
                                                  )
                                     .NormalizeWhitespace();
            DebugOut("built");
            var tree = SyntaxFactory.SyntaxTree(compl);
            var src = tree.ToString();

            DebugOut("Reparsing text ??");
            var tree2 = CSharpSyntaxTree.ParseText(SourceText.From(src));
            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            var source = tree2.ToString().Split(new[] { "\r\n" }, StringSplitOptions.None);
            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            var adhoc = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            DebugOut("Add solution");
            var s = adhoc.AddSolution(
                                       SolutionInfo.Create(
                                                            SolutionId.CreateNewId()
                                                          , VersionStamp.Create()
                                                          , null
                                                          , new[]
                                                            {
                                                                ProjectInfo.Create (
                                                                                    projectId
                                                                                  , VersionStamp
                                                                                       .Create ( )
                                                                                  , "test"
                                                                                  , "test"
                                                                                  , LanguageNames
                                                                                       .CSharp
                                                                                  , null
                                                                                  , null
                                                                                  , new
                                                                                        CSharpCompilationOptions (
                                                                                                                  OutputKind
                                                                                                                     .DynamicallyLinkedLibrary
                                                                                                                 )
                                                                                   )
                                                            }
                                                           )
                                      );



            var documentInfo = DocumentInfo.Create(
                                                    DocumentId.CreateNewId(projectId)
                                                  , "test"
                                                  , null
                                                  , SourceCodeKind.Regular
                                                  , TextLoader.From(
                                                                     TextAndVersion.Create(
                                                                                            SourceText
                                                                                               .From(
                                                                                                      src
                                                                                                     )
                                                                                          , VersionStamp
                                                                                               .Create()
                                                                                           )
                                                                    )
                                                   );

            var document2 = DocumentInfo.Create(
                                                 DocumentId.CreateNewId(projectId)
                                               , "misc"
                                               , null
                                               , SourceCodeKind.Regular
                                               , TextLoader.From(
                                                                  TextAndVersion.Create(
                                                                                         SourceText
                                                                                            .From(
                                                                                                   @"public class PocoSyntaxToken { public int RawKind { get; set; } public string Kind { get; set; } public object Value {get; set;} public string ValueText { get; set; } }"
                                                                                                  )
                                                                                       , VersionStamp
                                                                                            .Create()
                                                                                        )
                                                                 )
                                                );

            //todo investigate
            var s2 = s.AddDocuments(
                                     ImmutableArray<DocumentInfo>
                                        // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
                                        .Empty.Add(documentInfo)
                                        .Add(document2)
                                    );

            //var d = project.AddDocument ( "test.cs" , src ) ;
            var rb1 = adhoc.TryApplyChanges(s2);
            if (!rb1)
            {
                throw new InvalidOperationException();
            }
            DebugOut("Applying assembly refs");
            foreach (var ref1 in AssemblyRefs)
            {
                var s3 = adhoc.CurrentSolution.AddMetadataReference(
                                                                     projectId
                                                                   , MetadataReference
                                                                        .CreateFromFile(ref1)
                                                                    );
                var rb = adhoc.TryApplyChanges(s3);
                if (!rb)
                {
                    throw new InvalidOperationException();
                }
            }

            DebugOut("Applying assembly done");
            var project = adhoc.CurrentSolution.Projects.First();
            var compilation = await project.GetCompilationAsync();
            using (var f = new StreamWriter(@"C:\data\logs\errors.txt"))
            {
                if (compilation != null)
                {
                    foreach (var diagnostic in compilation.GetDiagnostics())
                    {
                        if ( diagnostic.IsSuppressed )
                        {
                            continue ;
                        }

                        // ReSharper disable once UnusedVariable
                        var line =
                            source[diagnostic
                                  .Location.GetLineSpan()
                                  .StartLinePosition.Line];
                        await f.WriteLineAsync(diagnostic.ToString());
                    }
                }
            }

            DebugOut("attempting emit");
            var result =
                (compilation ?? throw new InvalidOperationException()).Emit(
                                                                                 @"C:\data\logs\output.dll"
                                                                                );
            if (result.Success)
            {
                DebugOut("Success");
            }

            //File.WriteAllText ( @"C:\data\logs\gen.cs" , compl.ToString ( ) ) ;
            return AppCommandResult.Success ;
        }

        /// <inheritdoc />
        public void OnFault ( AggregateException exception ) { }
        #endregion
    }
}
