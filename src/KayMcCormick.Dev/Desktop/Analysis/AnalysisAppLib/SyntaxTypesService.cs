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
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Windows.Markup ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.Xaml ;
using AnalysisAppLib.XmlDoc ;
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
    public class SyntaxTypesService : ISyntaxTypesService , ISupportInitializeNotification
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
        };

        private const string _predefinedNodeElementName = "PredefinedNode";
        private const string _abstractNodeElementName   = "AbstractNode";
        private const string _nodeElementName           = "Node";

        private static readonly string _pocoPrefix       = "Poco";
        private static readonly string _collectionSuffix = "Collection";

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
            if ( identifier is Type type )
            {
                unqualifiedTypeName = type.Name ;
            }
            else if ( identifier is string s1 )
            {
                unqualifiedTypeName = s1 ;
            }
            else if ( identifier is AppTypeInfoKey k1 )
            {
                key = k1 ;
            }
            else
            {
                throw new InvalidOperationException ( "Bad key" ) ;
            }

            if ( unqualifiedTypeName != null
                 && key              == null )
            {
                key = new AppTypeInfoKey ( unqualifiedTypeName ) ;
            }

            if ( Map.dict.TryGetValue ( key , out var typeInfo ) )
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
            AppTypeCSharpSyntaxNode = CollectTypeInfos2 ( null , _cSharpSyntaxNodeClrType ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public AppTypeInfo AppTypeCSharpSyntaxNode
        {
            get { return _appTypeCSharpSyntaxNode ; }
            set { _appTypeCSharpSyntaxNode = value ; }
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            DebugUtils.WriteLine ( $"{rootR}" ) ;

            if ( ! Map.dict.TryGetValue ( new AppTypeInfoKey(rootR) , out var curTypeInfo ) )
            {
                throw new InvalidOperationException ( ) ;
            }

            DebugUtils.WriteLine ( $"{curTypeInfo}" ) ;
            var r = Map.dict[ new AppTypeInfoKey ( rootR ) ] ;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            //r.ColorValue     = HierarchyColors[level];
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                var theTypeInfo = CollectTypeInfos2 ( r , type1 , level + 1 ) ;
                r.SubTypeInfos.Add ( theTypeInfo ) ;
            }

            return r ;
        }

        /// <summary>
        /// -
        /// </summary>
        public  TypeMapDictionary Map { get { return _map ; } set { _map = value ; } }
        private TypeMapDictionary _map = new TypeMapDictionary ( ) ;
        private bool              _isInitialized ;
        private AppTypeInfo       _appTypeCSharpSyntaxNode ;
        #region Implementation of ISupportInitialize
        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( )
        {
            IsInitialized = true ;
        }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized
        {
            get { return _isInitialized ; }
            set { _isInitialized = value ; }
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

        internal void LoadFactoryMethodSignaturesAndDocumentation
            ( ISyntaxTypesService sts, [ NotNull ] IDocInterface docface )
        {

            var si = docface.GetTypeDocumentation(typeof ( SyntaxFactory ));
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                AppTypeInfoKey key = new AppTypeInfoKey(methodInfo.ReturnType);
                var info = sts.GetAppTypeInfo ( key ) ;
                var appMethodInfo = new AppMethodInfo { MethodInfo = methodInfo } ;
                if ( si != null
                     && si.MethodDocumentation.TryGetValue ( methodInfo.Name , out var mdoc ) )
                {
                    var p = String.Join (
                                         ","
                                       , methodInfo
                                        .GetParameters ( )
                                        .Select (
                                                 parameterInfo
                                                     => parameterInfo.ParameterType.FullName
                                                )
                                        ) ;
                    //DebugUtils.WriteLine ( $"xx: {p}" ) ;
                    foreach ( var methodDocumentation in mdoc )
                    {
                        //  DebugUtils.WriteLine ( methodDocumentation.Parameters ) ;
                        if ( methodDocumentation.Parameters == p )
                        {
                            //    DebugUtils.WriteLine ( $"Docs for {methodInfo}" ) ;
                            appMethodInfo.XmlDoc = methodDocumentation ;
                            docface.CollectDoc ( methodDocumentation ) ;
                        }
                    }
                }

                info.FactoryMethods.Add ( appMethodInfo ) ;
                //Logger.Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach ( var pair in Map.dict.Where(v => ! v.Key.Equals ( new AppTypeInfoKey(typeof(CSharpSyntaxNode)) ))
            )
            {
                //}.Where ( pair => pair.Key.IsAbstract == false ) )
                {
                    var type = GetTypeInfo ( pair.Value ) ;
                    foreach ( var propertyInfo in type.GetProperties (
                                                                          BindingFlags.DeclaredOnly
                                                                          | BindingFlags.Instance
                                                                          | BindingFlags.Public
                                                                         ) )
                    {
                        if ( propertyInfo.DeclaringType != type )
                        {
                            continue ;
                        }

                        var t = propertyInfo.PropertyType ;

                        AppTypeInfo typeInfo = null ;
                        AppTypeInfo otherTypeInfo = null ;
                        if ( t.IsGenericType )
                        {
                            DebugUtils.WriteLine ( $"{t} is Generic" ) ;
                            var targ = t.GenericTypeArguments[ 0 ] ;
                            DebugUtils.WriteLine ( $"{targ}" ) ;
                            if ( typeof ( SyntaxNode ).IsAssignableFrom ( targ )
                                 && typeof ( IEnumerable ).IsAssignableFrom ( t ) )
                            {
                                typeInfo = ( AppTypeInfo ) Map[ targ ] ;
                            }
                        }
                        else
                        {
                            if ( ! Map.dict.TryGetValue ( new AppTypeInfoKey ( t ) , out typeInfo ) )
                            {
                                if ( ! otherTyps.TryGetValue ( t , out otherTypeInfo ) )
                                {
                                    otherTypeInfo = otherTyps[ t ] = new AppTypeInfo { Type = t } ;
                                }
                            }
                        }

                        if ( typeInfo         == null
                             && otherTypeInfo == null )
                        {
                            continue ;
                        }

                        PropertyDocumentation propDoc = null ;
                        var info = docface.GetTypeDocumentation ( type ) ;
                        if ( type != null)
                        {
                            if ( info.PropertyDocumentation.TryGetValue (
                                                                         propertyInfo.Name
                                                                       , out propDoc
                                                                        ) )
                            {
                            }
                        }

                        if ( propDoc != null )
                        {
                            docface.CollectDoc ( propDoc ) ;
                            // pair.Value.Components.Add (
                                                       // new ComponentInfo
                                                       // {
                                                           // XmlDoc         = propDoc
                                                         // , IsSelfOwned    = true
                                                         // , OwningTypeInfo = pair.Value
                                                         // , IsList         = isList
                                                         // , TypeInfo =
                                                               // typeInfo ?? otherTypeInfo
                                                         // , PropertyName = propertyInfo.Name
                                                       // }
                                                      // ) ;
                        }

                        //Logger.Info ( t.ToString ( ) ) ;
                    }
                }
            }
        }

        private Type GetTypeInfo ( [ NotNull ] AppTypeInfo pairValue )
        {
            return pairValue.Type ;
        }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCollection DocumentCollection { get; set; } = new DocumentCollection();
        private readonly Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public IReadOnlyDictionary < string , object > CollectionMap(
        )
        {
            DebugUtils.WriteLine("Populating collectionMap");
            var collectionMap = new Dictionary < string , object > ();
            foreach (var kvp in Map.dict)
            {
                var mapKey = kvp.Key;
                var t = (AppTypeInfo)Map[mapKey];
                var colType = $"{_pocoPrefix}{t.Type.Name}{_collectionSuffix}";
                collectionMap[(string)t.KeyValue] = colType;
            }

            return collectionMap;
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
                    if ( syntax.Root != null )
                    {
                        var rootType = syntax.Root.Attribute ( XName.Get ( "Root" ) )?.Value ;
                        if ( model1.Map.Values != null )
                        {
                            var result =
                                Enumerable.Where < KeyValuePair < AppTypeInfoKey , AppTypeInfo > > (
                                                                                                    model1
                                                                                                       .Map
                                                                                                       .dict
                                                                                                  , pair
                                                                                                        => pair
                                                                                                          .Value
                                                                                                          .Type
                                                                                                          .Name
                                                                                                           == rootType
                                                                                                   ) ;
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
                            AppTypeInfo ati = null ;
                            var keyValuePairs = result as KeyValuePair < AppTypeInfoKey , AppTypeInfo >[] ?? result.ToArray ( ) ;
                            if ( ! keyValuePairs.Any ( ) )
                            {
                                DebugUtils.WriteLine ( $"No results for {rootType}" ) ;
                            }
                            else
                            {
                                ati = keyValuePairs.First ( ).Value ;
                                DebugUtils.WriteLine ( $"{ati}" ) ;
                            }
                        }

                        foreach ( var xElement in syntax.Root.Elements ( ) )
                        {
                            Type t = null ;
                            var xElementNameElementNameLocalName = xElement.Name.LocalName ;
                            switch ( xElementNameElementNameLocalName )
                            {
                                case _predefinedNodeElementName :
                                    var xName_Name = XName.Get ( "Name" ) ;
                                    var typeName = xElement.Attribute ( xName_Name )?.Value ;
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
                                case _abstractNodeElementName :
                                    ParseNodeBasics ( model1 , xElement , collectionMap ) ;
                                    break ;
                                case _nodeElementName :
                                    ParseNodeBasics ( model1 , xElement , collectionMap ) ;
                                    break ;

                                default : throw new InvalidOperationException ( ) ;
                            }
                        }
                    }
                }
            }
        }

        private static void ParseNodeBasics (
            [ NotNull ] ITypesViewModel                          model1
          , [ NotNull ] XElement                                xElement
          , [ NotNull ] IReadOnlyDictionary < string , object > collectionMap
        )
        {
            if ( collectionMap == null )
            {
                throw new ArgumentNullException ( nameof ( collectionMap ) ) ;
            }

            
            var typeName2 = xElement.Attribute ( XName.Get ( "Name" ) )?.Value ;
            var t2 = MapTypeNameToSyntaxNode ( model1, typeName2 ) ;
            if ( t2 == null )
            {
                DebugUtils.WriteLine ( "No type for " + typeName2 ) ;
            }

            else
            {
                var typ2 = model1.Map.dict[ new AppTypeInfoKey ( t2 ) ] ;
                typ2.ElementName = xElement.Name.LocalName ;
                var kinds1 = xElement.Elements ( XName.Get ( "Kind" ) ) ;
                var xElements = kinds1 as XElement[] ?? kinds1.ToArray ( ) ;
                if ( xElements.Any ( ) )
                {
                    typ2.Kinds.Clear ( ) ;
                    var nodekinds = xElements
                                   .Select ( element => element.Attribute ( "Name" )?.Value )
                                   .ToList ( ) ;
                    foreach ( var nodekind in nodekinds )
                    {
                        typ2.Kinds.Add ( nodekind ) ;
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
            var NameAttributeName = XName.Get ( "Name" ) ;
            var typeAttributeName = XName.Get("Type");
            var overrideAttributeName = XName.Get("Override");
            var optionalAttributeName = XName.Get("Optional");

            var fieldName = field.Attribute ( NameAttributeName )?.Value ;
            var fieldType = field.Attribute ( typeAttributeName )?.Value ;
            if(fieldType == "SyntaxList<SyntaxToken>")
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
                
                DebugUtils.WriteLine(string.Join(", ", kinds));
            }


            var fTypeP = SyntaxFactory.ParseTypeName ( fieldType ) ;
            var enumerable = false ;

            ITypeSymbol arg = null ;
            if ( fTypeP is GenericNameSyntax g )
            {
                var pds = SyntaxFactory.PropertyDeclaration ( g , "type1" ) ;
                

                var openBrace = SyntaxFactory.Token ( SyntaxKind.OpenBraceToken ) ;
                var closeBrace = SyntaxFactory.Token ( SyntaxKind.CloseBraceToken ) ;
                var empty = new SyntaxList < StatementSyntax > ( ) ;
                var blockSyntax = SyntaxFactory.Block ( openBrace , empty , closeBrace ) ;
                var ads1 = SyntaxFactory.AccessorDeclaration ( SyntaxKind.SetAccessorDeclaration , blockSyntax ) ;
                var ads = new SyntaxList < AccessorDeclarationSyntax > ( ads1 ) ;
                var als = SyntaxFactory.AccessorList ( ads ) ;
                var mds = pds.WithAccessorList ( als ) ;
                var declarationSyntaxes = SyntaxFactory.SingletonList < MemberDeclarationSyntax > ( mds ) ;
                var memberDeclarationSyntaxes =
                    SyntaxFactory.SingletonList < MemberDeclarationSyntax > (
                                                                             SyntaxFactory.ClassDeclaration ( "placeholder" )
                                                                                          .WithMembers (
                                                                                                        declarationSyntaxes
                                                                                                       )
                                                                            ) ;
                var syntaxTrees = SyntaxFactory.SyntaxTree (
                                                            WithCollectionUsings (
                                                                                  SyntaxFactory
                                                                                     .CompilationUnit ( )
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
                                                                        typeof (
                                                                            SyntaxFactory
                                                                        )
                                                                      , typeof (
                                                                            ValueType )
                                                                      , typeof (
                                                                            SyntaxNode
                                                                        )
                                                                      , typeof (
                                                                            TypeSyntax
                                                                        )
                                                                      , typeof ( object
                                                                        )
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
                                                                    ar
                                                                        => MetadataReference
                                                                           .CreateFromFile (
                                                                                            ar
                                                                                           )
                                                                   )
                                                          , new
                                                                CSharpCompilationOptions (
                                                                                          OutputKind
                                                                                             .DynamicallyLinkedLibrary
                                                                                         )
                                                           ) ;
                    

                        var ms = new MemoryStream ( ) ;
                        var result = compilation.Emit ( ms ) ;
                        var resultSuccess = ( bool ? ) result.Success
                                            ?? throw new InvalidOperationException ( ) ;

                        var syntaxTree = compilation.SyntaxTrees.First ( ) ;
                        var model = compilation.GetSemanticModel ( syntaxTree ) ;
                        var source = syntaxTree.ToString ( )
                                               .Split (
                                                       new[] { "\r\n" }
                                                     , StringSplitOptions.None
                                                      ) ;

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
                                        - diagnostic1
                                         .Location.GetLineSpan ( )
                                         .StartLinePosition.Line
                                        + 1 ;
                            var locationSourceSpan = diagnostic1.Location.SourceSpan ;
                            if ( diagnostic1.Location.SourceTree != null )
                            {
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
                                var lines = String.Join (
                                                         "\n"
                                                       , code.Lines.Skip ( startLine )
                                                             .Take ( count )
                                                             .ToList ( )
                                                        ) ;
                                // ReSharper disable once UnusedVariable
                                var line = source.Skip ( startLine ).Take ( count ).ToList ( ) ;
                                DebugUtils.WriteLine ( $"{lines}: {diagnostic1}" ) ;
                            }
                        }

                        var declarationSyntax = syntaxTree.GetRoot ( )
                                                          .DescendantNodes ( )
                                                          .OfType < PropertyDeclarationSyntax > ( )
                                                          .First ( ) ;
                        // ReSharper disable once UnusedVariable
                        var typeSyntax = declarationSyntax.Type ;
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
                                      Override = @override , Optional = optional
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

        private static Type MapTypeNameToSyntaxNode([NotNull] ITypesViewModel model1, string typeName2)
        {
            return model1.Map.dict.First(k => k.Value.Type.Name == typeName2).Value.Type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compl"></param>
        /// <returns></returns>
        [ NotNull ]
        public static CompilationUnitSyntax
            WithCollectionUsings ( [ NotNull ] CompilationUnitSyntax compl )
        {
            return compl.WithUsings (
                                     new SyntaxList < UsingDirectiveSyntax > (
                                                                              new[]
                                                                              {
                                                                                  SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "System"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "System.Collections.Generic"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "System.Collections"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "System.ComponentModel"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "Microsoft.CodeAnalysis"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "Microsoft.CodeAnalysis.CSharp"
                                                                                                                                        )
                                                                                                               )
                                                                                , SyntaxFactory.UsingDirective (
                                                                                                                SyntaxFactory.ParseName (
                                                                                                                                         "Microsoft.CodeAnalysis.CSharp.Syntax"
                                                                                                                                        )
                                                                                                               )
                                                                              }
                                                                             )
                                    ) ;
        }
    }
}