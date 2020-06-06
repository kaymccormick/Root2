using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Markup;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisControls.ViewModel;
using Autofac;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Command;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using NLog;
using ProjectInfo = Microsoft.CodeAnalysis.ProjectInfo;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [TitleMetadata("Code Gen")]
    [CategoryMetadata(Category.Infrastructure)]
    [GroupMetadata("Tasks")]
    [CommandIdMetadata("{50AD944E-B85F-4877-951B-FD3CCEC2C2ED}")]
    public sealed class CodeGenCommand : IBaseLibCommand
    {
#pragma warning disable 1591
        public CodeGenCommand(ILifetimeScope scope, ReplaySubject<CommandProgress> progressReplaySubject)
#pragma warning restore 1591
        {
            Scope = scope;
            _progressReplaySubject = progressReplaySubject;
        }

        [NotNull]
        private static string ModelXamlFilename
        {
            get { return Path.Combine(DataOutputPath, ModelXamlFilenamePart); }
        }

        private const string DataOutputPath = @"C:\data\logs";
        private const string TypesJsonFilename = "types.json";
        private const string ModelXamlFilenamePart = "model.xaml";

        private const string SolutionFilePath =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\work2\src\KayMcCormick.Dev\ManagedProd.sln";

        // ReSharper disable once InconsistentNaming
        private const string _pocoPrefix = "Poco";

        // ReSharper disable once InconsistentNaming
        private const string _collectionSuffix = "Collection";
        private const string PocoSyntaxNamespace = "PocoSyntax";

        // ReSharper disable once InconsistentNaming
        private const string ICollection_typename = "ICollection";

        // ReSharper disable once InconsistentNaming
        private const string IList_typename = "IList";

        // ReSharper disable once InconsistentNaming
        private const string List_typename = "List";

        // ReSharper disable once InconsistentNaming
        private const string IEnumerable_typename = "IEnumerable";

        private static readonly string[] AssemblyRefs =
        {
            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Microsoft.CSharp.dll"
            , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll"
            , @"C:\Users\mccor.LAPTOP-T6T0BN1K\.nuget\packages\nlog\4.7.0\lib\net45\NLog.dll"
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
            , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Facades\netstandard.dll"
        };

        // ReSharper disable once NotAccessedField.Local
#pragma warning disable 169
        private static ILogger _logger;
#pragma warning restore 169
        private readonly ReplaySubject<CommandProgress> _progressReplaySubject;


        /// <summary>
        /// 
        /// </summary>
        [NotNull] public  string PocoPrefix { get { return _pocoPrefix; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="lifetimeScope"></param>
        /// <returns></returns>
        [TitleMetadata("Code gen")]
        [UsedImplicitly]
        public async Task<IAppCommandResult> CodeGenAsync(IBaseLibCommand command, ILifetimeScope lifetimeScope)
        {
            var result = await Task.Run(() => CodeGen(command, lifetimeScope));
            return result;
        }


#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        // ReSharper disable once FunctionComplexityOverflow
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="lifetimeScope"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public  async Task<IAppCommandResult> CodeGen(
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            [NotNull] IBaseLibCommand command, ILifetimeScope lifetimeScope)
        {
            var outputFunc = (Action<string>) command.Argument ?? (m =>
            {
            });

            void DebugOut(string s1)
            {
                DebugUtils.WriteLine(s1);
                outputFunc(s1);
            }

            var model1 = lifetimeScope.Resolve<TypesViewModel>();
            // ReSharper disable once UnusedVariable
            var sts = lifetimeScope.Resolve<ISyntaxTypesService>();
            var collectionMap = model1.CollectionMap();
            outputFunc("Beginning");
            var x = CSharpCompilation.Create(
                "test"
                , new[] { SyntaxFactory.SyntaxTree(SyntaxFactory.CompilationUnit()) }
                , AssemblyRefs.Where(File.Exists).Select(
                    r => MetadataReference
                        .CreateFromFile(r)
                )
            );
            DebugUtils.WriteLine("Missing refs: " + string.Join(";", AssemblyRefs.Where(f => !File.Exists(f))));

            var types = new SyntaxList<MemberDeclarationSyntax>();
            outputFunc($"{model1.Map.Count} Entries in Type map");
            var rewriter1 = new SyntaxRewriter1(model1);
            foreach (var mapKey1 in model1.Map.Dict.Keys)
            {
                var t = model1.Map.Dict[mapKey1];
                // ReSharper disable once UnusedVariable
                var curType = t;
                var colTypeClassName = $"{PocoPrefix}{t.Type.Name}{_collectionSuffix}";

                var classDecl1 = CreatePoco(mapKey1, t);
                var curComp = ReplaceSyntaxTree(x, classDecl1);

                // ReSharper disable once UnusedVariable
                var classDecl1Type =
                    curComp.GetTypeByMetadataName(classDecl1.Identifier.ValueText);
                // ReSharper disable once IdentifierTypo
                // ReSharper disable once InconsistentNaming
                var simplebaseType_ilist1 = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(IList_typename));
                var enumerable1 = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(IEnumerable_typename));
                // ReSharper disable once InconsistentNaming
                var simpleBaseType_ICollection =
                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(ICollection_typename));
                var classContainerDecl = SyntaxFactory.ClassDeclaration(colTypeClassName)
                    .WithBaseList(
                        SyntaxFactory.BaseList()
                            .AddTypes(
                                simplebaseType_ilist1
                                , enumerable1
                                , simpleBaseType_ICollection
                            )
                    )
                    .WithModifiers(
                        SyntaxTokenList.Create(
                            SyntaxFactory.Token(
                                SyntaxKind
                                    .PublicKeyword
                            )
                        )
                    );

                var typeSyntax2 =
                    SyntaxFactory.ParseTypeName(t.Type.FullName ?? throw new AppInvalidOperationException());
                var typeSyntax = SyntaxFactory.ParseTypeName(
                    PocoPrefix
                    + ((QualifiedNameSyntax)typeSyntax2)
                    .Right.Identifier
                );
                // ReSharper disable once InconsistentNaming
                var internal_genericIList = SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(IList_typename)
                    , SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<
                                TypeSyntax>()
                            .Add(typeSyntax)
                    )
                );
                // ReSharper disable once InconsistentNaming
                var internal_genericList = SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(List_typename)
                    , SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<TypeSyntax
                            >()
                            .Add(typeSyntax)
                    )
                );
                // ReSharper disable once InconsistentNaming
                var IListRuntimeType = typeof(IList);
                // ReSharper disable once InconsistentNaming
                var ICollectionRuntimeType = typeof(ICollection);
                // ReSharper disable once InconsistentNaming
                var IEnumerableRuntimeType = typeof(IEnumerable);
                foreach (var type1 in new[]
                {
                    IListRuntimeType , ICollectionRuntimeType
                    , IEnumerableRuntimeType
                })
                {
                    //var prov = CSharpCodeProvider.CreateProvider ( LanguageNames.CSharp ) ;

                    var typeByMetadataName =
                        x.GetTypeByMetadataName(
                            type1.FullName
                            ?? throw new AppInvalidOperationException()
                        );
                    if (typeByMetadataName == null)
                    {
                        continue;
                    }

                    var generic1 =
                        typeByMetadataName; // t123.ConstructUnboundGenericType ( ).Construct ( classDecl1Type ) ;

                    // var i = model.GetSymbolInfo ( SyntaxFactory.ParseTypeName ( s1 ) ) ;


                    var listField = SyntaxFactory.IdentifierName("_list");

                    var publicKeyword = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                    PropertyDeclarationSyntax Selector1(IPropertySymbol prop)
                    {
                        var propTypeSymbol = prop.Type;
                        var propTypeSymbolMetadataName = propTypeSymbol.MetadataName;
                        var propertyTypeParsed = SyntaxTypesService.FieldPocoCollectionType(
                            SyntaxFactory.ParseTypeName(
                                propTypeSymbolMetadataName
                            )
                            , collectionMap
                            , t
                        );
                        var propDecl = SyntaxFactory.PropertyDeclaration(propertyTypeParsed, prop.Name);
                        propDecl = propDecl.WithModifiers(publicKeyword);
                        var propertyIdentifierNameSyntax = SyntaxFactory.IdentifierName(prop.Name);
                        var propertyAccess = SyntaxFactory.MemberAccessExpression(
                            SyntaxKind
                                .SimpleMemberAccessExpression
                            , listField
                            , propertyIdentifierNameSyntax
                        );
                        var arrowExpressionClauseSyntax = SyntaxFactory.ArrowExpressionClause(propertyAccess);
                        propDecl = propDecl.WithExpressionBody(arrowExpressionClauseSyntax)
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                            );
                        return propDecl;
                    }

                    var props1 = generic1.GetMembers()
                        .OfType<IPropertySymbol>()
                        .Where(
                            x22 => x22.Kind == SymbolKind.Property
                                   && x22.IsIndexer == false
                        )
                        .Select(Selector1);
                    var indexers = generic1.GetMembers()
                        .OfType<IPropertySymbol>()
                        .Where(
                            x22 => x22.Kind == SymbolKind.Property
                                   && x22.IsIndexer
                        )
                        .Select(
                            prop => {
                                var bArgList =
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SeparatedList<
                                                ArgumentSyntax
                                            >()
                                            .AddRange(
                                                prop
                                                    .Parameters
                                                    .Select(
                                                        pp1
                                                            => SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName(
                                                                    pp1
                                                                        .Name
                                                                )
                                                            )
                                                    )
                                            )
                                    );
                                var elementAccess =
                                    SyntaxFactory.ElementAccessExpression(
                                        listField
                                        , bArgList
                                    );
                                var setArrowExpression =
                                    SyntaxFactory.ArrowExpressionClause(
                                        SyntaxFactory.AssignmentExpression(
                                            SyntaxKind
                                                .SimpleAssignmentExpression
                                            , elementAccess
                                            , SyntaxFactory.IdentifierName(
                                                "value"
                                            )
                                        )
                                    );
                                var setAccessor =
                                    SyntaxFactory.AccessorDeclaration(
                                            SyntaxKind
                                                .SetAccessorDeclaration
                                        )
                                        .WithExpressionBody(
                                            setArrowExpression
                                        )
                                        .WithSemicolonToken(
                                            SyntaxFactory.Token(
                                                SyntaxKind
                                                    .SemicolonToken
                                            )
                                        );
                                var getArrow =
                                    SyntaxFactory.ArrowExpressionClause(
                                        SyntaxFactory.ElementAccessExpression(
                                            listField
                                            , bArgList
                                        )
                                    );


                                var parameterSyntaxes =
                                    prop.Parameters.Select(
                                        p2
                                            => SyntaxFactory.Parameter(
                                                    SyntaxFactory.Identifier(
                                                        p2
                                                            .Name
                                                    )
                                                )
                                                .WithType(
                                                    SyntaxFactory.ParseTypeName(
                                                        p2
                                                            .Type
                                                            .MetadataName
                                                    )
                                                )
                                    );
                                var separatedSyntaxList =
                                    SyntaxFactory.SeparatedList<ParameterSyntax>()
                                        .AddRange(parameterSyntaxes);
                                return SyntaxFactory.IndexerDeclaration(
                                        SyntaxFactory.ParseTypeName(
                                            prop
                                                .Type
                                                .MetadataName
                                        )
                                    )
                                    .WithParameterList(
                                        SyntaxFactory.BracketedParameterList(
                                            separatedSyntaxList
                                        )
                                    )
                                    .WithModifiers(publicKeyword)
                                    .WithAccessorList(
                                        SyntaxFactory.AccessorList(
                                            SyntaxFactory.List<
                                                    AccessorDeclarationSyntax
                                                >()
                                                .AddRange(
                                                    new
                                                        []
                                                        {
                                                            SyntaxFactory.AccessorDeclaration (
                                                                    SyntaxKind
                                                                        .GetAccessorDeclaration
                                                                )
                                                                .WithExpressionBody (
                                                                    getArrow
                                                                )
                                                                .WithSemicolonToken (
                                                                    SyntaxFactory.Token (
                                                                        SyntaxKind
                                                                            .SemicolonToken
                                                                    )
                                                                )
                                                            , setAccessor
                                                        }
                                                )
                                        )
                                    );
                            }
                        );

                    var members1 = generic1.GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(
                            x22 => x22.Kind == SymbolKind.Method
                                   && x22.MethodKind == MethodKind.Ordinary
                        )
                        .Select(
                            m => {
                                var returnType =
                                    SyntaxFactory.ParseTypeName(
                                        m.ReturnType.MetadataName
                                    );
                                var methodDeclarationSyntax =
                                    SyntaxFactory.MethodDeclaration(
                                        m.ReturnsVoid
                                            ? SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(
                                                    SyntaxKind
                                                        .VoidKeyword
                                                )
                                            )
                                            : returnType
                                        , m.Name
                                    );

                                ParameterSyntax Selector(
                                    IParameterSymbol p1
                                )
                                {
                                    if (p1.Type.SpecialType
                                        == SpecialType.System_Object)
                                    {
                                        // DebugUtils.WriteLine(
                                            // $"{p1.Type}"
                                        // );
                                    }

                                    return SyntaxFactory.Parameter(
                                        SyntaxFactory.List<
                                            AttributeListSyntax
                                        >()
                                        , new
                                            SyntaxTokenList()
                                        , SyntaxFactory.ParseTypeName(
                                            p1.Type
                                                .MetadataName
                                        )
                                        , SyntaxFactory.Identifier(p1.Name)
                                        , null
                                    );
                                }

                                var separatedSyntaxList =
                                    SyntaxFactory.SeparatedList(
                                        m.Parameters.Select(
                                            Selector
                                        )
                                    );
                                return methodDeclarationSyntax
                                    .WithModifiers(publicKeyword)
                                    .WithLeadingTrivia(
                                        SyntaxTriviaList
                                            .Create(
                                                SyntaxFactory.Comment(
                                                    $"// {typeByMetadataName.ToDisplayString()}"
                                                )
                                            )
                                    )
                                    .WithParameterList(
                                        SyntaxFactory.ParameterList(
                                            separatedSyntaxList
                                        )
                                    )
                                    .WithExpressionBody(
                                        SyntaxFactory.ArrowExpressionClause(
                                            SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind
                                                            .SimpleMemberAccessExpression
                                                        , listField
                                                        , SyntaxFactory.IdentifierName(
                                                            m.Name
                                                        )
                                                    )
                                                )
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        new
                                                                SeparatedSyntaxList
                                                                <ArgumentSyntax
                                                                >()
                                                            .AddRange(
                                                                m.Parameters
                                                                    .Select(
                                                                        p
                                                                            => SyntaxFactory.Argument(
                                                                                SyntaxFactory.CastExpression(
                                                                                    p
                                                                                        .Type
                                                                                        .SpecialType
                                                                                    == SpecialType
                                                                                        .System_Object
                                                                                        ? internal_genericIList
                                                                                            .TypeArgumentList
                                                                                            .Arguments
                                                                                            [
                                                                                                0]
                                                                                        : SyntaxFactory.ParseTypeName(
                                                                                            p
                                                                                                .Type
                                                                                                .MetadataName
                                                                                        )
                                                                                    , SyntaxFactory.IdentifierName(
                                                                                        p
                                                                                            .Name
                                                                                    )
                                                                                )
                                                                            )
                                                                    )
                                                            )
                                                    )
                                                )
                                        )
                                    )
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(
                                            SyntaxKind
                                                .SemicolonToken
                                        )
                                    );
                            }
                        );
                    foreach (var methodDeclarationSyntax in members1)
                    {
                        _progressReplaySubject.OnNext(new CommandProgress() { Content = methodDeclarationSyntax });
                    }

                    classContainerDecl = classContainerDecl.WithMembers(
                        SyntaxFactory.List(
                            classContainerDecl
                                .Members
                                .Concat(
                                    members1
                                )
                                .Concat(props1)
                                .Concat(
                                    indexers
                                )
                        )
                    );

                    classDecl1 = (ClassDeclarationSyntax)rewriter1.Visit(classDecl1);
                    if (WriteDebug)
                    {
                        DebugUtils.WriteLine(
                            "\n***\n"
                            + classContainerDecl
                                .NormalizeWhitespace()
                                .ToFullString()
                            + "\n****\n"
                        );
                    }
                }

                var invocationExpressionSyntax =
                    SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind
                                    .SimpleMemberAccessExpression
                                , SyntaxFactory.ParenthesizedExpression(
                                    SyntaxFactory.CastExpression(
                                        internal_genericList
                                        , SyntaxFactory.IdentifierName(
                                            "_list"
                                        )
                                    )
                                )
                                , SyntaxFactory.Token(SyntaxKind.DotToken)
                                , (SimpleNameSyntax)SyntaxFactory.ParseName(
                                    "AddRange"
                                )
                            )
                        )
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.Token(SyntaxKind.OpenParenToken)
                                , new SeparatedSyntaxList<ArgumentSyntax
                                    >()
                                    .Add(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(
                                                "initList"
                                            )
                                        )
                                    )
                                , SyntaxFactory.Token(SyntaxKind.CloseParenToken)
                            )
                        );
                var typeArgument = internal_genericIList.TypeArgumentList.Arguments[0];

                var constructor = SyntaxFactory.ConstructorDeclaration(classContainerDecl.Identifier.ValueText)
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            new SeparatedSyntaxList<
                                ParameterSyntax>().Add(
                                SyntaxFactory.Parameter(
                                        SyntaxFactory.Identifier(
                                            "initList"
                                        )
                                    )
                                    .WithType(
                                        SyntaxFactory.GenericName(
                                                SyntaxFactory.Identifier(
                                                    "IList"
                                                )
                                            )
                                            .WithTypeArgumentList(
                                                SyntaxFactory.TypeArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList(
                                                        typeArgument
                                                    )
                                                )
                                            )
                                    )
                            )
                        )
                    )
                    .WithBody(
                        SyntaxFactory.Block(
                            SyntaxFactory.ExpressionStatement(
                                invocationExpressionSyntax
                            )
                        )
                    )
                    .WithModifiers(
                        SyntaxTokenList.Create(
                            SyntaxFactory.Token(
                                SyntaxKind
                                    .PublicKeyword
                            )
                        )
                    );

                classContainerDecl = classContainerDecl.WithMembers(
                    SyntaxFactory.List(
                        classContainerDecl
                            .Members.Concat(
                                new
                                    MemberDeclarationSyntax
                                    []
                                    {
                                        constructor
                                    }
                            )
                    )
                );

                var argumentListSyntax = SyntaxFactory.ArgumentList(
                    SyntaxFactory.Token(SyntaxKind.OpenParenToken)
                    , SyntaxFactory.SeparatedList<ArgumentSyntax>()
                    , SyntaxFactory.Token(SyntaxKind.CloseParenToken)
                );
                var listIdentifier = SyntaxFactory.Identifier("List");
                var constructedListGeneric = SyntaxFactory.GenericName(
                    listIdentifier
                    , SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            typeSyntax
                        )
                    )
                );
                var ocEx = SyntaxFactory.ObjectCreationExpression(
                    constructedListGeneric
                    , argumentListSyntax
                    , default
                );
                var fds = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(IList_typename))
                        .WithVariables(
                            SyntaxFactory.SeparatedList<
                                    VariableDeclaratorSyntax
                                >()
                                .Add(
                                    SyntaxFactory.VariableDeclarator(
                                            "_list"
                                        )
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                ocEx
                                            )
                                        )
                                )
                        )
                );
                classContainerDecl = classContainerDecl.WithMembers(
                    SyntaxFactory.List(
                        classContainerDecl
                            .Members.Concat(
                                new[]
                                {
                                    fds
                                }
                            )
                    )
                );

                types = types.Add(classContainerDecl);
                var members = new SyntaxList<MemberDeclarationSyntax>();
                foreach (SyntaxFieldInfo tField in t.Fields)
                {
                    if (tField.ClrTypeName != null)
                    {
                        tField.Type = Type.GetType(tField.ClrTypeName);
                        if (tField.Type == null)
                        {
                            DebugUtils.WriteLine(
                                $"unable to resolve type {tField.ClrTypeName}e "
                            );
                        }
                    }


                    if (tField.Type == null
                        && tField.Type != typeof(bool)
                        && tField.TypeName != "bool")
                    {
                        continue;
                    }

                    // ReSharper disable once IdentifierTypo
                    var acdsl = new SyntaxList<AccessorDeclarationSyntax>(
                        new[]
                        {
                            SyntaxFactory.AccessorDeclaration (
                                    SyntaxKind
                                        .GetAccessorDeclaration
                                )
                                .WithSemicolonToken (
                                    SyntaxFactory.Token (
                                        SyntaxKind
                                            .SemicolonToken
                                    )
                                )
                            , SyntaxFactory.AccessorDeclaration (
                                    SyntaxKind
                                        .SetAccessorDeclaration
                                )
                                .WithSemicolonToken (
                                    SyntaxFactory.Token (
                                        SyntaxKind
                                            .SemicolonToken
                                    )
                                )
                        }
                    );
                    var accessorListSyntax = SyntaxFactory.AccessorList(acdsl);

                    TypeSyntax type;
                    if (tField.IsCollection)
                    {
                        type = SyntaxFactory.ParseTypeName(tField.ElementTypeMetadataName);
                    }
                    else
                    {
                        var tFieldTypeName = tField.TypeName;

                        type = SyntaxFactory.ParseTypeName(tFieldTypeName);
                    }

                    if (type is GenericNameSyntax gen)
                    {
                        var ss = (SimpleNameSyntax)gen.TypeArgumentList.Arguments[0];
                        type = gen.WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                new SeparatedSyntaxList<
                                        TypeSyntax>()
                                    .Add(
                                        SyntaxFactory.ParseTypeName(
                                            $"{PocoPrefix}{ss.Identifier.Text}"
                                        )
                                    )
                            )
                        );
                    }


                    var tokens = new List<SyntaxToken>
                    {
                        SyntaxFactory.Token ( SyntaxKind.PublicKeyword )
                        , tField.Override
                            ? SyntaxFactory.Token ( SyntaxKind.OverrideKeyword )
                            : SyntaxFactory.Token ( SyntaxKind.VirtualKeyword )
                    };


                    var nameSyntax = SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression
                        , SyntaxFactory.IdentifierName(
                            "DesignerSerializationVisibility"
                        )
                        , SyntaxFactory.IdentifierName("Content")
                    );
                    ////.ParseName ( "System.ComponentModel.DesignerSerializationVisibility" ) ;
                    var attributeSyntax = SyntaxFactory.Attribute(
                        SyntaxFactory.ParseName("DesignerSerializationVisibility")
                        , SyntaxFactory.AttributeArgumentList(
                            SyntaxFactory.SeparatedList(
                                new[]
                                {
                                    SyntaxFactory.AttributeArgument (
                                        nameSyntax
                                    )
                                }
                            )
                        )
                    );
                    // DebugUtils.WriteLine(attributeSyntax.ToFullString());
                    var separatedSyntaxList =
                        new SeparatedSyntaxList<AttributeSyntax>().Add(attributeSyntax);
                    var attributeListSyntaxes = SyntaxFactory.List(
                        new[]
                        {
                            SyntaxFactory.AttributeList ( separatedSyntaxList )
                        }
                    );
                    var syntaxTokenList = SyntaxFactory.TokenList(tokens.ToArray());
                    var propertyName = SyntaxFactory.Identifier(tField.Name);
                    var propertyDeclarationSyntax = SyntaxFactory.PropertyDeclaration(
                        attributeListSyntaxes
                        , syntaxTokenList
                        , XmlDocElements
                            .SubstituteType(
                                tField
                                , type
                                , collectionMap
                                , model1
                            )
                        // ReSharper disable once AssignNullToNotNullAttribute
                        , null
                        , propertyName
                        , accessorListSyntax
                    );
                    // DebugUtils.WriteLine(
                        // propertyDeclarationSyntax
                            // .NormalizeWhitespace()
                            // .ToFullString()
                    // );
                    members = members.Add(propertyDeclarationSyntax);
                }

                var documentationCommentTriviaSyntax = SyntaxFactory.DocumentationCommentTrivia(
                    SyntaxKind
                        .SingleLineDocumentationCommentTrivia
                    , SyntaxFactory.List<
                            XmlNodeSyntax
                        >()
                        .Add(
                            SyntaxFactory.XmlSummaryElement()
                        )
                );
                // ReSharper disable once UnusedVariable
                var tokens1 = documentationCommentTriviaSyntax
                    .DescendantTokens(x111 => true, true)
                    .ToList();
                classDecl1 = classDecl1.WithMembers(members);

                types = types.Add(classDecl1);
            }


            DebugOut("About to build compilation unit");

            var compilation = SyntaxFactory.CompilationUnit();
            compilation = SyntaxTypesService.WithCollectionUsings(compilation);
            compilation = compilation.WithMembers(
                    new SyntaxList<MemberDeclarationSyntax>(
                        SyntaxFactory.NamespaceDeclaration(
                                SyntaxFactory.ParseName(
                                    PocoSyntaxNamespace
                                )
                            )
                            .WithMembers(
                                types
                            )
                    )
                )
                .NormalizeWhitespace();

            DebugOut("built");
            var tree = SyntaxFactory.SyntaxTree(compilation);
            var src = tree.ToString();

            // DebugOut("Reparsing text ??");

            // var tree2 = CSharpSyntaxTree.Create(
                // compilation
                // , new CSharpParseOptions(
                    // LanguageVersion.CSharp7_3
                // )
            // );

            // File.WriteAllText(@"C:\data\logs\gen.cs", compilation.ToString());

            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            // var source = tree2.ToString().Split(new[] { "\r\n" }, StringSplitOptions.None);

            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            AdhocWorkspace workspace = null;
            try
            {
                workspace = new AdhocWorkspace();
                var replay = Scope.Resolve<ReplaySubject<AdhocWorkspace>>();
                replay.OnNext(workspace);
            }
            catch (ReflectionTypeLoadException rex)
            {
                foreach (var rexLoaderException in rex.LoaderExceptions)
                {
                    DebugUtils.WriteLine(rexLoaderException.Message);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

            if (workspace == null)
            {
                return AppCommandResult.Failed;
            }
            var projectId = ProjectId.CreateNewId();
            DebugOut("Add solution");

            var s = workspace.AddSolution(
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

            var sourceText = SourceText.From(
                @"     using System;
using System.Collections;
using System.Collections.Generic;

public class PocoSyntaxTokenList : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSyntaxToken)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSyntaxToken)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSyntaxToken)value);
        // System.Collections.IList
        public void    RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly            => _list.IsReadOnly;
        public Boolean IsFixedSize           => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void    CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32   Count          => _list.Count;
        public Object  SyncRoot       => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        IList              _list = new List<PocoSyntaxToken>();
    }

    public class PocoSyntaxToken
    {
        public int RawKind { get; set; }

        public string Kind { get; set; }

        public object Value { get; set; }

        public string ValueText { get; set; }
    }
"
            );

            File.WriteAllText("misc.cs", sourceText.ToString());

            var document2 = DocumentInfo.Create(
                DocumentId.CreateNewId(projectId)
                , "misc"
                , null
                , SourceCodeKind.Regular
                , TextLoader.From(
                    TextAndVersion.Create(
                        sourceText
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

            var rb1 = workspace.TryApplyChanges(s2);
            if (!rb1)
            {
                throw new AppInvalidOperationException();
            }

            DebugOut("Applying assembly refs");
            foreach (var ref1 in AssemblyRefs.Union(
                new[]
                {
                    typeof ( DesignerSerializationOptions )
                        .Assembly.Location
                }
            ))

            {
                var s3 = workspace.CurrentSolution.AddMetadataReference(
                    projectId
                    , MetadataReference
                        .CreateFromFile(ref1)
                );
                var rb = workspace.TryApplyChanges(s3);
                if (!rb)
                {
                    throw new AppInvalidOperationException();
                }
            }

            DebugOut("Applying assembly done");
            var project = workspace.CurrentSolution.Projects.First();
            
            
            var comp1 = await project.GetCompilationAsync();
            using (var f = new StreamWriter(@"C:\data\logs\errors.txt"))
            {
                if (comp1 != null)
                {
                    foreach (var diagnostic in comp1.GetDiagnostics())
                    {
                        if (diagnostic.IsSuppressed)
                        {
                            continue;
                        }

                        // ReSharper disable once UnusedVariable
                        var line = src
                            .Skip(
                                diagnostic.Location.GetLineSpan().StartLinePosition.Line
                                - 1
                            )
                            .Take(
                                diagnostic.Location.GetLineSpan().EndLinePosition.Line
                                - diagnostic
                                    .Location.GetLineSpan()
                                    .StartLinePosition.Line
                            );
                        await f.WriteLineAsync(
                            string.Join(
                                "\n"
                                , diagnostic.ToString()
                                , line
                            )
                        );
                    }
                }
            }

            var errors = comp1?.GetDiagnostics()
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .ToList();
            if (errors?.Any() == true)
            {
                DebugUtils.WriteLine(string.Join("\n", errors));
            }

            DebugOut("attempting emit");

            var result =
                (comp1).Emit(
                    @"C:\data\logs\output.dll"
                );
            DebugUtils.WriteLine(result.Success.ToString());
            foreach (var resultDiagnostic in result.Diagnostics)
            {
                if (resultDiagnostic.Severity >= DiagnosticSeverity.Info)
                {
                    DebugUtils.WriteLine(resultDiagnostic.ToString());
                }
            }
            if (result.Success)
            {
                DebugOut("Success");
            }

            else
            {
                DebugUtils.WriteLine("Failure");
                return AppCommandResult.Failed;
            }

            return AppCommandResult.Success;
            //File.WriteAllText ( @"C:\data\logs\gen.cs" , comp.ToString ( ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteDebug { get; set; }

        [NotNull]
        private static CSharpCompilation ReplaceSyntaxTree(
            [NotNull] CSharpCompilation x
            , ClassDeclarationSyntax classDecl1
        )
        {
            return x.ReplaceSyntaxTree(
                x.SyntaxTrees[0]
                , SyntaxFactory.SyntaxTree(
                    SyntaxFactory.CompilationUnit()
                        .WithMembers(
                            SyntaxFactory.List<MemberDeclarationSyntax
                            >(new[] { classDecl1 })
                        )
                )
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [NotNull]
        public ClassDeclarationSyntax CreatePoco(
            [NotNull] AppTypeInfoKey mapKey
            , [NotNull] AppTypeInfo t
        )
        {
            var classDecl1 = SyntaxFactory.ClassDeclaration($"{_pocoPrefix}{mapKey.StringValue}")
                .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));
            if (t.ParentInfo == null)
            {
                return classDecl1;
            }

            var identifierNameSyntax = SyntaxFactory.IdentifierName(_pocoPrefix + t.ParentInfo.Type.Name);
            classDecl1 = classDecl1.WithBaseList(
                SyntaxFactory.BaseList(
                    new SeparatedSyntaxList<BaseTypeSyntax
                    >().Add(
                        SyntaxFactory.SimpleBaseType(
                            identifierNameSyntax
                        )
                    )
                )
            );

            return classDecl1;
        }


        /// <summary>
        /// 
        /// </summary>
        public object Argument { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IAppCommandResult> ExecuteAsync(object parameter)
        {
            return CodeGenAsync(this, Scope);
        }

        /// <summary>
        /// 
        /// </summary>
        public ILifetimeScope Scope { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnFault(AggregateException exception)
        {
            throw new NotImplementedException();
        }
    }
}