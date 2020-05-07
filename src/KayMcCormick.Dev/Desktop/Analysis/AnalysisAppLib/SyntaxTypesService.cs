#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// SyntaxTypesService.cs
// 
// 2020-04-13-3:57 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Windows.Markup ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.Xaml ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.Text ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Syntax types service.
    /// </summary>
    public sealed class SyntaxTypesService : ISyntaxTypesService , ISupportInitializeNotification
    {
        private static readonly string[] AssemblyRefs =
        {
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
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Facades\netstandard.dll"
        } ;

        private const string PredefinedNodeElementName = "PredefinedNode" ;
        private const string AbstractNodeElementName   = "AbstractNode" ;
        private const string NodeElementName           = "Node" ;

        private const string PocoPrefix       = "Poco" ;
        private const string CollectionSuffix = "Collection" ;

        private Type          _cSharpSyntaxNodeClrType ;
        private List < Type > _nodeTypes ;

        #region Implementation of ISyntaxTypesService
        /// <summary>
        /// Get the <see cref="AppTypeInfo"/> instance for a particular identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public AppTypeInfo GetAppTypeInfo ( [ NotNull ] object identifier )
        {
            AppTypeInfoKey key = null ;
            string unqualifiedTypeName = null ;
            switch ( identifier )
            {
                case Type type :
                    unqualifiedTypeName = type.Name ;
                    break ;
                case string s1 :
                    unqualifiedTypeName = s1 ;
                    break ;
                case AppTypeInfoKey k1 :
                    key = k1 ;
                    break ;
                default : throw new InvalidOperationException ( "Bad key" ) ;
            }

            if ( unqualifiedTypeName != null )
            {
                key = new AppTypeInfoKey ( unqualifiedTypeName ) ;
            }

            if ( Map.Dict.TryGetValue ( key , out var typeInfo ) )
            {
                return typeInfo ;
            }

            throw new InvalidOperationException ( "No such type" ) ;
        }
        #endregion

        // ReSharper disable once UnusedMember.Local
        private void CreateSubtypeLinkages ( )
        {
            DebugUtils.WriteLine ( $"Performing {nameof ( CreateSubtypeLinkages )}" ) ;
            _cSharpSyntaxNodeClrType = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = _cSharpSyntaxNodeClrType.Assembly.GetExportedTypes ( )
                                                 .Where (
                                                         t => typeof ( CSharpSyntaxNode )
                                                            .IsAssignableFrom ( t )
                                                        )
                                                 .ToList ( ) ;
            CollectTypeInfos2 ( null , _cSharpSyntaxNodeClrType ) ;
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            DebugUtils.WriteLine ( $"{rootR}" ) ;

            if ( ! Map.Dict.TryGetValue ( new AppTypeInfoKey ( rootR ) , out var curTypeInfo ) )
            {
                throw new InvalidOperationException ( ) ;
            }

            DebugUtils.WriteLine ( $"{curTypeInfo}" ) ;
            var r = Map.Dict[ new AppTypeInfoKey ( rootR ) ] ;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            //r.ColorValue     = HierarchyColors[level];
            foreach ( var theTypeInfo in _nodeTypes.Where ( type => type.BaseType == rootR ).Select ( type1 => CollectTypeInfos2 ( r , type1 , level + 1 ) ) )
            {
                r.SubTypeInfos.Add ( theTypeInfo ) ;
            }

            return r ;
        }

        /// <summary>
        /// -
        /// </summary>
        private TypeMapDictionary Map { get { return _map ; } }

        private readonly TypeMapDictionary _map = new TypeMapDictionary ( ) ;
        private bool              _isInitialized ;
        #region Implementation of ISupportInitialize
        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( ) { IsInitialized = true ; }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized
        {
            get { return _isInitialized ; }
            private set { _isInitialized = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Initialized ;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeSyntax"></param>
        /// <param name="collectionMap"></param>
        /// <param name="appTypeInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ NotNull ]
        public static TypeSyntax FieldPocoCollectionType (
            [ NotNull ] TypeSyntax                              typeSyntax
          , [ NotNull ] IReadOnlyDictionary < string , object > collectionMap
          , [ NotNull ] AppTypeInfo                             appTypeInfo
        )
        {
            if ( typeSyntax == null )
            {
                throw new ArgumentNullException ( nameof ( typeSyntax ) ) ;
            }

            if ( appTypeInfo == null )
            {
                throw new ArgumentNullException ( nameof ( appTypeInfo ) ) ;
            }

            var q =
                from SyntaxFieldInfo field in appTypeInfo.Fields
                let s = typeSyntax as SimpleNameSyntax
                where field.Name == s?.Identifier.Text
                select field.Types ;

            var key = q.FirstOrDefault ( )?.FirstOrDefault ( ) ;
            if ( key != null
                 && collectionMap.TryGetValue (
                                               key.KeyValue.ToString ( )
                                             , out var fieldTypeName
                                              ) )
            {
                return SyntaxFactory.ParseTypeName ( ( string ) fieldTypeName ) ;
            }

            return typeSyntax ;
        }

        // ReSharper disable once UnusedMember.Global

        private Type GetTypeInfo ( [ NotNull ] AppTypeInfo pairValue ) { return pairValue.Type ; }

        private readonly Dictionary < Type , AppTypeInfo > _otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public IReadOnlyDictionary < string , object > CollectionMap ( )
        {
            DebugUtils.WriteLine ( "Populating collectionMap" ) ;
            var collectionMap = new Dictionary < string , object > ( ) ;
            foreach ( var kvp in Map.Dict )
            {
                var mapKey = kvp.Key ;
                var t = ( AppTypeInfo ) Map[ mapKey ] ;
                var colType = $"{PocoPrefix}{t.Type.Name}{CollectionSuffix}" ;
                collectionMap[ ( string ) t.KeyValue ] = colType ;
            }

            return collectionMap ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="collectionMap"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void LoadSyntax (
            [ NotNull ] ITypesViewModel             model1
          , IReadOnlyDictionary < string , object > collectionMap
        )
        {
            using ( var stream =
                typeof ( AnalysisAppLibModule ).Assembly.GetManifestResourceStream (
                                                                                    "AnalysisAppLib.Resources.Syntax.xml"
                                                                                   ) )
            {
                using ( var reader = XmlReader.Create (
                                                       stream
                                                       ?? throw new InvalidOperationException ( )
                                                     , new XmlReaderSettings { Async = true }
                                                      ) )
                {
                    var syntax = XDocument.Load ( reader ) ;
                    if ( syntax.Root == null )
                    {
                        return ;
                    }

                    var rootType = syntax.Root.Attribute ( XName.Get ( "Root" ) )?.Value ;
                    var result =
                        model1.Map.Dict.Where ( pair => pair.Value.Type.Name == rootType ) ;
                    // var result = (valueCollection.Where ( t => {
                    // DebugUtils.WriteLine(t.Type.Name);
                    // var b = t.Type.Name
                    // == rootType ;
                    // if ( b )
                    // DebugUtils
                    // .WriteLine ( "true" ) ;
                    // return b ;
                    // }
                    // )) ;
                    var keyValuePairs =
                        result as KeyValuePair < AppTypeInfoKey , AppTypeInfo >[]
                        ?? result.ToArray ( ) ;
                    if ( ! keyValuePairs.Any ( ) )
                    {
                        DebugUtils.WriteLine ( $"No results for {rootType}" ) ;
                    }
                    else
                    {
                        var ati = keyValuePairs.First ( ).Value ;
                        DebugUtils.WriteLine ( $"{ati}" ) ;
                    }

                    foreach ( var xElement in syntax.Root.Elements ( ) )
                    {
                        Type t = null ;
                        var xElementNameElementNameLocalName = xElement.Name.LocalName ;
                        switch ( xElementNameElementNameLocalName )
                        {
                            case PredefinedNodeElementName :
                                var xNameName = XName.Get ( "Name" ) ;
                                var typeName = xElement.Attribute ( xNameName )?.Value ;
                                switch ( typeName )
                                {
                                    case nameof ( CSharpSyntaxNode ) :
                                        t = typeof ( CSharpSyntaxNode ) ;
                                        break ;
                                    case nameof ( SyntaxNode ) :
                                        t = typeof ( SyntaxNode ) ;
                                        break ;
                                    case nameof ( StructuredTriviaSyntax ) :
                                        t = typeof ( StructuredTriviaSyntax ) ;
                                        break ;
                                    case nameof ( SyntaxToken ) :
                                        t = typeof ( SyntaxToken ) ;
                                        break ;
                                }

                                if ( t == null )
                                {
                                    DebugUtils.WriteLine ( "No type for " + typeName ) ;
                                }
                                else if ( model1.Map.Contains ( t ) )
                                {
                                    var typ = ( AppTypeInfo ) model1.Map[ t ] ;
                                    typ.ElementName = xElementNameElementNameLocalName ;
                                }

                                break ;
                            case AbstractNodeElementName :
                                ParseNodeBasics ( model1 , xElement , collectionMap ) ;
                                break ;
                            case NodeElementName :
                                ParseNodeBasics ( model1 , xElement , collectionMap ) ;
                                break ;

                            default : throw new InvalidOperationException ( ) ;
                        }
                    }
                }
            }
        }

        private static void ParseNodeBasics (
            [ NotNull ] ITypesViewModel                         model1
          , [ NotNull ] XElement                                xElement
          , [ NotNull ] IReadOnlyDictionary < string , object > collectionMap
        )
        {
            if ( collectionMap == null )
            {
                throw new ArgumentNullException ( nameof ( collectionMap ) ) ;
            }


            var typeName2 = xElement.Attribute ( XName.Get ( "Name" ) )?.Value ;
            var t2 = MapTypeNameToSyntaxNode ( model1 , typeName2 ) ;
            if ( t2 == null )
            {
                DebugUtils.WriteLine ( "No type for " + typeName2 ) ;
            }

            else
            {
                var typ2 = model1.Map.Dict[ new AppTypeInfoKey ( t2 ) ] ;
                typ2.ElementName = xElement.Name.LocalName ;
                var kinds1 = xElement.Elements ( XName.Get ( "Kind" ) ) ;
                var xElements = kinds1 as XElement[] ?? kinds1.ToArray ( ) ;
                if ( xElements.Any ( ) )
                {
                    typ2.Kinds.Clear ( ) ;
                    var nodeKinds = xElements
                                   .Select ( element => element.Attribute ( "Name" )?.Value )
                                   .ToList ( ) ;
                    foreach ( var nodeKind in nodeKinds )
                    {
                        typ2.Kinds.Add ( nodeKind ) ;
                    }

                    DebugUtils.WriteLine ( typ2.Title ) ;
                }

                // ReSharper disable once UnusedVariable
                var comment = xElement.Element ( XName.Get ( "TypeComment" ) ) ;
                //DebugUtils.WriteLine ( comment ) ;

                typ2.Fields.Clear ( ) ;
                var choices = xElement.Elements ( XName.Get ( "Choice" ) ) ;
                var choiceAry = choices as XElement[] ?? choices.ToArray ( ) ;
                if ( choiceAry.Any ( ) )
                {
                    var choice = choiceAry.First ( ) ;
                    // ReSharper disable once UnusedVariable
                    foreach ( var element in choice.Elements ( ) )
                    {
                    }

                    // ReSharper disable once UnusedVariable
                    foreach ( var element in choice.Elements ( XName.Get ( "Field" ) ) )
                    {
                    }

                    DebugUtils.WriteLine ( typ2.Title ) ;
                }

                foreach ( var field in xElement.Elements ( XName.Get ( "Field" ) )
                                               .Concat (
                                                        xElement.Elements ( XName.Get ( "Choice" ) )
                                                                .Elements ( XName.Get ( "Field" ) )
                                                       ) )
                {
                    ParseField ( field , typ2 ) ;
                }
            }
        }

        private static void ParseField ( [ NotNull ] XElement field , [ NotNull ] AppTypeInfo typ2 )
        {
            var nameAttributeName = XName.Get ( "Name" ) ;
            var typeAttributeName = XName.Get ( "Type" ) ;
            var overrideAttributeName = XName.Get ( "Override" ) ;
            var optionalAttributeName = XName.Get ( "Optional" ) ;

            var fieldName = field.Attribute ( nameAttributeName )?.Value ;
            var fieldType = field.Attribute ( typeAttributeName )?.Value ;
            if ( fieldType == "SyntaxList<SyntaxToken>" )
            {
                fieldType = "SyntaxTokenList" ;
            }

            var @override = field.Attribute ( overrideAttributeName )?.Value == "true" ;
            var optional = field.Attribute ( optionalAttributeName )?.Value  == "true" ;

            var kinds = field.Elements ( "Kind" )
                             .Select ( element => element.Attribute ( "Name" )?.Value )
                             .ToList ( ) ;
            if ( kinds.Any ( ) )
            {
                DebugUtils.WriteLine ( string.Join ( ", " , kinds ) ) ;
            }


            var fTypeP = SyntaxFactory.ParseTypeName ( fieldType ?? throw new InvalidOperationException ( ) ) ;
            var enumerable = false ;

            ITypeSymbol arg = null ;
            if ( fTypeP is GenericNameSyntax g )
            {
                var pds = SyntaxFactory.PropertyDeclaration ( g , "type1" ) ;


                var openBrace = SyntaxFactory.Token ( SyntaxKind.OpenBraceToken ) ;
                var closeBrace = SyntaxFactory.Token ( SyntaxKind.CloseBraceToken ) ;
                var empty = new SyntaxList < StatementSyntax > ( ) ;
                var blockSyntax = SyntaxFactory.Block ( openBrace , empty , closeBrace ) ;
                var ads1 = SyntaxFactory.AccessorDeclaration (
                                                              SyntaxKind.SetAccessorDeclaration
                                                            , blockSyntax
                                                             ) ;
                var ads = new SyntaxList < AccessorDeclarationSyntax > ( ads1 ) ;
                var als = SyntaxFactory.AccessorList ( ads ) ;
                var mds = pds.WithAccessorList ( als ) ;
                var declarationSyntaxes =
                    SyntaxFactory.SingletonList < MemberDeclarationSyntax > ( mds ) ;
                var memberDeclarationSyntaxes =
                    SyntaxFactory.SingletonList < MemberDeclarationSyntax > (
                                                                             SyntaxFactory
                                                                                .ClassDeclaration (
                                                                                                   "placeholder"
                                                                                                  )
                                                                                .WithMembers (
                                                                                              declarationSyntaxes
                                                                                             )
                                                                            ) ;
                var syntaxTrees = SyntaxFactory.SyntaxTree (
                                                            WithCollectionUsings (
                                                                                  SyntaxFactory
                                                                                     .CompilationUnit ( )
                                                                                 )
                                                               .WithUsings (
                                                                            CodeAnalysisUsings ( )
                                                                           )
                                                               .WithMembers (
                                                                             memberDeclarationSyntaxes
                                                                            )
                                                               .NormalizeWhitespace ( )
                                                           ) ;
                var compilation = CSharpCompilation.Create (
                                                            "test"
                                                          , new[] { syntaxTrees }
                                                          , AssemblyRefs
                                                           .Concat (
                                                                    new[]
                                                                    {
                                                                        typeof ( SyntaxFactory )
                                                                      , typeof ( ValueType )
                                                                      , typeof ( SyntaxNode )
                                                                      , typeof ( TypeSyntax )
                                                                      , typeof ( object )
                                                                      , typeof (
                                                                            DesignerSerializationOptionsAttribute
                                                                        )
                                                                    }.Select (
                                                                              t => t
                                                                                  .Assembly
                                                                                  .Location
                                                                             )
                                                                   )
                                                           .Select (
                                                                    ar => MetadataReference
                                                                       .CreateFromFile ( ar )
                                                                   )
                                                          , new CSharpCompilationOptions (
                                                                                          OutputKind
                                                                                             .DynamicallyLinkedLibrary
                                                                                         )
                                                           ) ;


                var ms = new MemoryStream ( ) ;
                var result = compilation.Emit ( ms ) ;
                // ReSharper disable once UnusedVariable
                var resultSuccess = ( bool ? ) result.Success
                                    ?? throw new InvalidOperationException ( ) ;

                var syntaxTree = compilation.SyntaxTrees.First ( ) ;
                var model = compilation.GetSemanticModel ( syntaxTree ) ;
                var source = syntaxTree.ToString ( )
                                       .Split ( new[] { "\r\n" } , StringSplitOptions.None ) ;

                foreach ( var diagnostic1 in compilation
                                            .GetDiagnostics ( )
                                            .Where (
                                                    diagnostic
                                                        => diagnostic.Severity
                                                           == DiagnosticSeverity.Error
                                                   ) )
                {
                    if ( diagnostic1.Id == "CS0315" )
                    {
                    }

                    var startLine =
                        diagnostic1.Location.GetLineSpan ( ).StartLinePosition.Line - 1 ;
                    var count = diagnostic1.Location.GetLineSpan ( ).EndLinePosition.Line
                                - diagnostic1.Location.GetLineSpan ( ).StartLinePosition.Line
                                + 1 ;
                    var locationSourceSpan = diagnostic1.Location.SourceSpan ;
                    if ( diagnostic1.Location.SourceTree == null )
                    {
                        continue ;
                    }

                    var code = diagnostic1.Location.SourceTree.GetText ( ) ;
                    var end = locationSourceSpan.End     + 10 ;
                    var start = locationSourceSpan.Start - 10 ;
                    if ( start < 0 )
                    {
                        start = 0 ;
                    }

                    if ( end >= code.Length )
                    {
                        end = code.Length - 1 ;
                    }

                    var sp = new TextSpan ( start , end - start ) ;
                    // ReSharper disable once UnusedVariable
                    var codePart = code.GetSubText ( sp ) ;
                    var lines = string.Join (
                                             "\r\n"
                                           , code.Lines.Skip ( startLine ).Take ( count ).ToList ( )
                                            ) ;
                    // ReSharper disable once UnusedVariable
                    var line = source.Skip ( startLine ).Take ( count ).ToList ( ) ;
                    DebugUtils.WriteLine ( $"{lines}: {diagnostic1}" ) ;
                }

                var emitResult = compilation.Emit ( @"C:\temp\emit1.dll" ) ;
                if ( emitResult.Success == false )
                {
                    DebugUtils.WriteLine ( string.Join ( "\r\n" , source ) ) ;
                }

                var declarationSyntax = syntaxTree.GetRoot ( )
                                                  .DescendantNodes ( )
                                                  .OfType < PropertyDeclarationSyntax > ( )
                                                  .First ( ) ;
                // ReSharper disable once UnusedVariable
                var typeSyntax = declarationSyntax.Type ;
                // ReSharper disable once UnusedVariable
                var x1 = model.GetDeclaredSymbol ( declarationSyntax ) ;

                var symbol1 = model.GetSymbolInfo ( typeSyntax ) ;
                if ( symbol1.Symbol is INamedTypeSymbol namedTypeSymbol )
                {
                    enumerable = namedTypeSymbol.AllInterfaces.Any (
                                                                    i => i.SpecialType
                                                                         == SpecialType
                                                                            .System_Collections_IEnumerable
                                                                         || i.SpecialType
                                                                         == SpecialType
                                                                            .System_Collections_Generic_IEnumerable_T
                                                                   ) ;
                    if ( namedTypeSymbol.IsGenericType )
                    {
                        arg = namedTypeSymbol.TypeArguments.First ( ) ;
                        namedTypeSymbol.TypeParameters.First ( ) ;
                    }
                }
            }

            var syntaxFieldInfo = new SyntaxFieldInfo ( fieldName , fieldType , kinds.ToArray ( ) )
                                  {
                                      Override = @override , Optional = optional,
                                      AppTypeInfo = typ2
                                  } ;
            if ( enumerable )
            {
                syntaxFieldInfo.IsCollection = true ;
                if ( arg != null )
                {
                    syntaxFieldInfo.ElementTypeMetadataName = arg.ToDisplayString ( ) ;
                }
            }

            //DebugUtils.WriteLine ($"{typ2.Title}: {fieldName}: {fieldType} = {string.Join(", ", kinds)}"  );
            typ2.Fields.Add ( syntaxFieldInfo ) ;
        }

        private static SyntaxList < UsingDirectiveSyntax > CodeAnalysisUsings ( )
        {
            return SyntaxFactory.List < UsingDirectiveSyntax > ( )
                                .Add (
                                      SyntaxFactory.UsingDirective (
                                                                    SyntaxFactory.ParseName (
                                                                                             "Microsoft.CodeAnalysis"
                                                                                            )
                                                                   )
                                     )
                                .Add (
                                      SyntaxFactory.UsingDirective (
                                                                    SyntaxFactory.ParseName (
                                                                                             "Microsoft.CodeAnalysis.CSharp"
                                                                                            )
                                                                   )
                                     )
                                .Add (
                                      SyntaxFactory.UsingDirective (
                                                                    SyntaxFactory.ParseName (
                                                                                             "Microsoft.CodeAnalysis.CSharp.Syntax"
                                                                                            )
                                                                   )
                                     ) ;
        }

        private static Type MapTypeNameToSyntaxNode (
            [ NotNull ] ITypesViewModel model1
          , string                      typeName2
        )
        {
            return model1.Map.Dict.First ( k => k.Value.Type.Name == typeName2 ).Value.Type ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        [ NotNull ]
        public static CompilationUnitSyntax
            WithCollectionUsings ( [ NotNull ] CompilationUnitSyntax compilation )
        {
            return compilation.WithUsings (
                                           new SyntaxList < UsingDirectiveSyntax > (
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
                                                                                      , SyntaxFactory
                                                                                           .UsingDirective (
                                                                                                            SyntaxFactory
                                                                                                               .ParseName (
                                                                                                                           "System.Collections"
                                                                                                                          )
                                                                                                           )
                                                                                      , SyntaxFactory
                                                                                           .UsingDirective (
                                                                                                            SyntaxFactory
                                                                                                               .ParseName (
                                                                                                                           "System.ComponentModel"
                                                                                                                          )
                                                                                                           )
                                                                                    }
                                                                                   )
                                          ) ;
        }
    }
}