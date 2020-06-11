using System ;
using System.Collections;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Reflection;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Xml ;
using AnalysisAppLib ;
using AnalysisAppLib.Properties ;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.Xaml ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Serialization ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// </summary>
    [ NoJsonConverter ]
    public sealed class TypesViewModel : ITypesViewModel
      , INotifyPropertyChanged
      , ISupportInitializeNotification
    {
        /// <summary>
        /// 
        /// </summary>
        public TypesViewModel ( ) {
        }

        private readonly List < AppTypeInfo > _typeInfos ;
        private const string PocoPrefix = "Poco" ;
        private const string CollectionSuffix = "Collection" ;

        private DateTime _initializationDateTime ;
        private bool     _showBordersIsChecked ;

        private uint[] _hierarchyColors =
        {
            0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c , 0xff3c6e7d , 0xff533ca3
        } ;

        private AppTypeInfo   _root ;
        private List < Type > _nodeTypes ;

        private TypeMapDictionary  _map   = new TypeMapDictionary ( ) ;
        private TypeMapDictionary2 _map2 = new TypeMapDictionary2 ( ) ;


#if true
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
#endif

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly Dictionary < Type , TypeDocInfo > _docs =
            new Dictionary < Type , TypeDocInfo > ( ) ;

        private readonly DocumentCollection _documentCollection = new DocumentCollection ( ) ;
        [ CanBeNull ] private readonly object _docInfo = null ;
        private Dictionary<Type, AppTypeInfo> otherTyps = new Dictionary<Type, AppTypeInfo>();

        /// <summary>
        /// 
        /// </summary>
        public TypesViewModel ( List < AppTypeInfo > typeInfos =null) { _typeInfos = typeInfos ; }

        /// <summary>
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public TypesViewModel ( JsonSerializerOptions options )
        {
            options.WriteIndented = true ;
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public static XmlDocument LoadDoc ( )
        {
            var xml = Resources.doc ;
            var docuDoc = new XmlDocument ( ) ;
            docuDoc.LoadXml ( xml ) ;
            return docuDoc ;
        }

        /// <summary>
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        [ JsonIgnore ]
        public AppTypeInfo Root
        {
            get { return _root ; }
            set
            {
                _root = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        public bool ShowBordersIsChecked
        {
            get { return _showBordersIsChecked ; }
            set
            {
                _showBordersIsChecked = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        public uint[] HierarchyColors
        {
            get { return _hierarchyColors ; }
            set { _hierarchyColors = value ; }
        }

        /// <summary>
        /// </summary>
        public DocumentCollection DocumentCollection { get { return _documentCollection ; } }

        /// <summary>
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        public TypeMapDictionary Map { get { return _map ; } set { _map = value ; } }

        /// <summary>
        /// </summary>
        public AppTypeInfoCollection StructureRoot { get ; set ; } = new AppTypeInfoCollection ( ) ;

        /// <summary>
        /// 
        /// </summary>
        [ CanBeNull ] public object DocInfo { get { return _docInfo ; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public AppTypeInfo GetAppTypeInfo ( [ NotNull ] object identifier )
        {
            AppTypeInfoKey key = null ;
            string unqualifiedTypeName = null ;
            switch ( identifier )
            {
                case Type type :         unqualifiedTypeName = type.Name ;
                    break ;
                case string s1 :         unqualifiedTypeName = s1 ;
                    break ;
                case AppTypeInfoKey k1 : key                 = k1 ;
                    break ;
                default :                throw new AppInvalidOperationException ( "Bad key" ) ;
            }

            if ( unqualifiedTypeName != null )
            {
                key = new AppTypeInfoKey ( unqualifiedTypeName ) ;
            }

            if ( Map.Dict.TryGetValue ( key , out var typeInfo ) )
            {
                return typeInfo ;
            }

            throw new AppInvalidOperationException ( "No such type" ) ;
        }


 
        public bool TryGetAppTypeInfo([NotNull] object identifier, out AppTypeInfo appTypeInfo)
        {
            AppTypeInfoKey key = null;
            string unqualifiedTypeName = null;
            switch (identifier)
            {
                case Type type:
                    unqualifiedTypeName = type.Name;
                    break;
                case string s1:
                    unqualifiedTypeName = s1;
                    break;
                case AppTypeInfoKey k1:
                    key = k1;
                    break;
                default:
                    appTypeInfo = null;
                    return false;
            }

            if (unqualifiedTypeName != null)
            {
                key = new AppTypeInfoKey(unqualifiedTypeName);
            }

            if (Map.Dict.TryGetValue(key, out appTypeInfo))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Discover information about the given Syntax Node class and all children recursively, depth-first.
        /// </summary>
        /// <param name="clrSyntaxNodeType"></param>
        /// <param name="parentTypeInfo"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [NotNull ]
        public AppTypeInfo CollectTypeInfos (
            [ NotNull ] Type clrSyntaxNodeType
          , AppTypeInfo      parentTypeInfo = null
          , int              level          = 0
        )
        {
            if ( clrSyntaxNodeType == null )
            {
                throw new ArgumentNullException ( nameof ( clrSyntaxNodeType ) ) ;
            }

            TypeDocumentation docNode = null ;

            if ( _docs.TryGetValue ( clrSyntaxNodeType , out var info ) )
            {
                if ( info.TypeDocumentation != null )
                {
                    docNode = info.TypeDocumentation ;
                }
            }

            if ( docNode != null )
            {
                CollectDoc ( docNode ) ;
            }

            /* Construct appTypeInfo */
            var appTypeInfoKey = new AppTypeInfoKey ( clrSyntaxNodeType ) ;
            const int version = 1 ;
            var appTypeInfo = new AppTypeInfo
                    {
                        Type           = clrSyntaxNodeType
                      , DocInfo        = docNode
                      , ParentInfo     = parentTypeInfo
                      , HierarchyLevel = level
                      , ColorValue     = HierarchyColors[ level ]
                      , Version        = version
                      , KeyValue       = appTypeInfoKey.StringValue
                    } ;
            //DebugUtils.WriteLine ( $"{JsonSerializer.Serialize ( appTypeInfo , _options )}" ) ;
            /* Descend into related subtypes to populate complete syntax node structure. */
            foreach ( var subType in _nodeTypes.Where ( type => type.BaseType == clrSyntaxNodeType ) )
            {
                appTypeInfo.SubTypeInfos.Add ( CollectTypeInfos ( subType , appTypeInfo , level + 1 ) ) ;
            }

            Map.Dict[ appTypeInfoKey ] = appTypeInfo ;
            return appTypeInfo ;
        }

        private void CollectDoc ( [ CanBeNull ] CodeElementDocumentation docNode )
        {
            if ( docNode == null )
            {
                return ;
            }

            DebugUtils.WriteLine ( $"{docNode}" ) ;
            DocumentCollection.Add ( docNode ) ;
        }

        #region Implementation of ISerializable
        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISupportInitialize
        /// <summary>
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// </summary>
        public void EndInit ( )
        {
            Logger.Info ( nameof ( EndInit ) ) ;
            if ( _typeInfos?.Any ( ) == true )
            {
                foreach ( var appTypeInfo in _typeInfos )
                {
                    var keyValue = ( string ) appTypeInfo.KeyValue ;
                    Map2.Dict[ keyValue ] = appTypeInfo ;
                }
            }
            else
            {
                foreach ( var keyValuePair in Map2.Dict )
                {
                    Map.Dict[ new AppTypeInfoKey ( keyValuePair.Key ) ] = keyValuePair.Value ;
                }
            }

                foreach ( var keyValuePair in Map2.Dict )
                {
                    var appTypeInfo = keyValuePair.Value;
                    foreach (var kind in appTypeInfo.Kinds)
                    {
                        var v = (SyntaxKind)Enum.Parse(typeof(SyntaxKind), kind);
                        appTypeInfo.SyntaxKinds.Add(v);
                    }
                    foreach(SyntaxFieldInfo f in appTypeInfo.Fields) {
		    f.Model = this;
		    }
                }

var mapCount = Map.Count ;
            Logger.Info ( $"Map Count {mapCount}" ) ;
            if ( mapCount != 0 )
            {
                CreateSubtypeLinkages ( ) ;
            }
            else if ( Root                       == null
                      || Root.SubTypeInfos.Count == 0 )
            {
                LoadTypeInfo ( ) ;
            }
            else
            {
                PopulateMap ( Root ) ;
            }

            DetailFields ( ) ;

            LoadSyntaxFactoryDocs ( _docs ) ;
//            StructureRoot = new AppTypeInfoCollection { Map[ typeof ( CompilationUnitSyntax ) ] } ;
            IsInitialized          = true ;
            InitializationDateTime = DateTime.Now ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docs"></param>
        /// 
        public void LoadSyntaxFactoryDocs(Dictionary<Type, TypeDocInfo> docs)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _docs.TryGetValue(typeof(SyntaxFactory), out var si);
            var methodInfos = typeof(SyntaxFactory)
                             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                             .ToList();

            foreach (var methodInfo in methodInfos.Where(
                                                           info => typeof(SyntaxNode)
                                                              .IsAssignableFrom(info.ReturnType)
                                                          ))
            {
                var info = Map.Dict[new AppTypeInfoKey(methodInfo.ReturnType)];
                var appMethodInfo = new AppMethodInfo { MethodInfo = methodInfo };
                if (si != null
                     && si.MethodDocumentation.TryGetValue(methodInfo.Name, out var mdoc))
                {
                    var p = string.Join(
                                         ","
                                       , methodInfo
                                        .GetParameters()
                                        .Select(
                                                 parameterInfo
                                                     => parameterInfo.ParameterType.FullName
                                                )
                                        );
                    //Debug.WriteLine ( $"xx: {p}" ) ;
                    foreach (var methodDocumentation in mdoc)
                    {
                        //  Debug.WriteLine ( methodDocumentation.Parameters ) ;
                        if (methodDocumentation.Parameters == p)
                        {
                            //    Debug.WriteLine ( $"Docs for {methodInfo}" ) ;
                            appMethodInfo.XmlDoc = methodDocumentation;
                            CollectDoc(methodDocumentation);
                        }
                    }
                }

                info.FactoryMethods.Add(appMethodInfo);
                //Logger.Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach (var pair in Map.Dict.Where(pair => !pair.Key.Equals( new AppTypeInfoKey(typeof(CSharpSyntaxNode))))
            )
            {
                //}.Where ( pair => pair.Key.IsAbstract == false ) )
                {
                    foreach (var propertyInfo in pair.Value.Type.GetProperties(
                                                                          BindingFlags.DeclaredOnly
                                                                          | BindingFlags.Instance
                                                                          | BindingFlags.Public
                                                                         ))
                    {
                        if (propertyInfo.DeclaringType != pair.Value.Type)
                        {
                            continue;
                        }

                        var t = propertyInfo.PropertyType;
                        // if ( t == typeof ( SyntaxToken ) )
                        // {
                        // continue ;
                        // }

                        AppTypeInfo typeInfo = null;
                        AppTypeInfo otherTypeInfo = null;
                        if (t.IsGenericType)
                        {
                            var targ = t.GenericTypeArguments[0];
                            if (typeof(SyntaxNode).IsAssignableFrom(targ)
                                 && typeof(IEnumerable).IsAssignableFrom(t))
                            {
                                // Debug.WriteLine (
                                // $"{pair.Key.Name} {propertyInfo.Name} list of {targ.Name}"
                                // ) ;
                                typeInfo = (AppTypeInfo)Map[targ];
                            }
                        }
                        else
                        {
                            if (!Map.Dict.TryGetValue(new AppTypeInfoKey(t), out typeInfo))
                            {
                                if (!otherTyps.TryGetValue(t, out otherTypeInfo))
                                {
                                    otherTypeInfo = otherTyps[t] = new AppTypeInfo { Type = t };
                                }
                            }
                        }

                        if (typeInfo == null
                             && otherTypeInfo == null)
                        {
                            continue;
                        }

                        PropertyDocumentation propDoc = null;
                        if (pair.Value.Type != null
                             && _docs.TryGetValue(pair.Value.Type, out var info))
                        {
                            if (info.PropertyDocumentation.TryGetValue(
                                                                         propertyInfo.Name
                                                                       , out propDoc
                                                                        ))
                            {
                            }
                        }

                        CollectDoc(propDoc);
                        // pair.Value.Components.Add(
                        //                            new ComponentInfo
                        //                            {
                        //                                XmlDoc = propDoc
                        //                              ,
                        //                                IsSelfOwned = true
                        //                              ,
                        //                                OwningTypeInfo = pair.Value
                        //                              ,
                        //                                IsList = isList
                        //                              ,
                        //                                TypeInfo = typeInfo ?? otherTypeInfo
                        //                              ,
                        //                                PropertyName = propertyInfo.Name
                        //                            }
                        //                           );
                        //Logger.Info ( t.ToString ( ) ) ;
                    }
                }
            }
        }


        /// <summary>
        /// </summary>
        public void DetailFields ( )
        {
            foreach ( var rField in Map.Dict.SelectMany (
                                                         keyValuePair
                                                             => keyValuePair
                                                               .Value.Fields
                                                        ) )
            {
                if ( rField.Type == null )
                {
                    DebugUtils.WriteLine ( $"type is null for {rField.TypeName}" ) ;
                }

                if ( rField.Type != null
                     && rField.Type.IsGenericType
                     && ( rField.Type.GetGenericTypeDefinition ( ) == typeof ( SyntaxList <> )
                          || rField.Type.GetGenericTypeDefinition ( )
                          == typeof ( SeparatedSyntaxList <> ) ) )
                {
                    var tz = rField.Type.GetGenericArguments ( )[ 0 ] ;
                    if ( ! Map.Contains ( tz ) )
                    {
                        continue ;
                    }

                    var ati = Map.Dict[ new AppTypeInfoKey ( tz ) ] ;
                    var types = new List < AppTypeInfo > ( ) ;
                    Collect ( ati , types ) ;
                    foreach ( var appTypeInfo in types )
                    {
                        rField.Types.Add ( appTypeInfo ) ;
                    }
                }
                else
                {
                    if ( rField.Type != null
                         && Map.Contains ( rField.Type ) )
                    {
                        rField.Types.Add ( Map.Dict[ new AppTypeInfoKey ( rField.Type ) ] ) ;
                    }
                }
            }
        }

    
    


    private void CreateSubtypeLinkages ( )
        {
            DebugUtils.WriteLine ( $"Performing {nameof ( CreateSubtypeLinkages )}" ) ;
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos2 ( null , rootR ) ;
        }

        [ CanBeNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            // DebugUtils.WriteLine ( $"{rootR}" ) ;
            // if (_docs.TryGetValue(
            //                       rootR ?? throw new AppInvalidOperationException()
            //                     , out var info
            //                      ))
            // {
            //     docNode = info.TypeDocumentation;
            // }

//            CollectDoc(docNode);
var appTypeInfoKey = new AppTypeInfoKey ( rootR );
if (!Map.Dict.TryGetValue(appTypeInfoKey, out var curTypeInfo))
{

    return null;
                throw new AppInvalidOperationException ("Cant find in map" ) ;
            }

            // DebugUtils.WriteLine ( $"{curTypeInfo}" ) ;
            var r = Map.Dict[ appTypeInfoKey ] ;
            r.AllTypes = GetAppTypeInfos();
            r.Model = this;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            r.ColorValue     = HierarchyColors[ level ] ;
            foreach ( var theTypeInfo in _nodeTypes.Where ( type => type.BaseType == rootR ).Select ( type1 => CollectTypeInfos2 ( r , type1 , level + 1 ) ) )
            {
                if (theTypeInfo != null) r.SubTypeInfos.Add(theTypeInfo);
            }

            PopulateFieldTypes ( r ) ;

            return r ;
        }

        /// <summary>
        /// </summary>
        /// <param name="r"></param>
        public void PopulateFieldTypes ( [ NotNull ] AppTypeInfo r )
        {
            // find way to eliminate this
            foreach ( SyntaxFieldInfo rField in r.Fields )
            {
                if ( rField.TypeName == "SyntaxList<SyntaxToken>" )
                {
//                    rField.TypeName = "SyntaxTokenList" ;
                }

                TypeSyntax typs ;
                try
                {
                    typs = SyntaxFactory.ParseTypeName ( rField.TypeName ) ;
                }
                catch ( FileNotFoundException )
                {
                    continue ;
                }

                if ( typs is GenericNameSyntax gns )
                {
                    var id = gns.Identifier.ValueText ;
                    var t0 = typeof ( SyntaxNode ).Assembly.GetType (
                                                                     "Microsoft.CodeAnalysis."
                                                                     + id
                                                                     + "`1"
                                                                    ) ;
                    if ( t0 == null )
                    {
                        DebugUtils.WriteLine ( "fail" + id ) ;
                    }
                    else
                    {
                        var s = ( SimpleNameSyntax ) gns.TypeArgumentList.Arguments[ 0 ] ;
                        var t1 = typeof ( CSharpSyntaxNode ).Assembly.GetType (
                                                                               "Microsoft.CodeAnalysis.CSharp.Syntax."
                                                                               + s.Identifier
                                                                                  .ValueText
                                                                              ) ;
                        if ( t1 == null )
                        {
                            t1 = typeof ( SyntaxNode ).Assembly.GetType (
                                                                         "Microsoft.CodeAnalysis."
                                                                         + s.Identifier.ValueText
                                                                        ) ;
                        }

                        var t2 = t0.MakeGenericType ( t1 ) ;
                        rField.Type = t2 ;
                    }
                }
                else
                {
                    var t = typeof ( CSharpSyntaxNode ).Assembly.GetType (
                                                                          "Microsoft.CodeAnalysis.CSharp.Syntax."
                                                                          + rField.TypeName
                                                                         ) ;
                    if ( t == null )
                    {
                        t = typeof ( SyntaxNode ).Assembly.GetType (
                                                                    "Microsoft.CodeAnalysis."
                                                                    + rField.TypeName
                                                                   ) ;
                        if ( t == null )
                        {
                            //DebugUtils.WriteLine ( rField.TypeName ) ;
                        }
                    }

                    rField.Type = t ;
                }
            }
        }

        private static void Collect ( [ NotNull ] AppTypeInfo ati , List < AppTypeInfo > types )
        {
            if ( ! ati.Type.IsAbstract )
            {
                types.Add ( ati ) ;
            }

            foreach ( var atiSubTypeInfo in ati.SubTypeInfos )
            {
                Collect ( atiSubTypeInfo , types ) ;
            }
        }

        private void PopulateMap ( [ NotNull ] AppTypeInfo appTypeInfo )
        {
            Map[ appTypeInfo.Type ] = appTypeInfo ;
            foreach ( var subTypeInfo in appTypeInfo.SubTypeInfos )
            {
                PopulateMap ( subTypeInfo ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadTypeInfo ( )
        {
            var cSharpRootSyntaxNodeType = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = cSharpRootSyntaxNodeType.Assembly.GetExportedTypes ( )
                                                 .Where (
                                                         t => typeof ( CSharpSyntaxNode )
                                                            .IsAssignableFrom ( t )
                                                        )
                                                 .ToList ( ) ;
            Root = CollectTypeInfos ( cSharpRootSyntaxNodeType ) ;
        }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <inheritdoc />
        public bool IsInitialized { get ; set ; }

        /// <summary>
        ///     An approximate time as to when the view model was initialized and/or
        ///     populated with extended information.
        /// </summary>
        public DateTime InitializationDateTime
        {
            [ UsedImplicitly ] get { return _initializationDateTime ; }
            set { _initializationDateTime = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TypeMapDictionary2 Map2 { get { return _map2 ; } set { _map2 = value ; } }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion

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
                collectionMap[ t.Type.Name ] = colType ;
            }

            return collectionMap ;
        }

        /// <summary>
        /// Get all app type infos
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public IEnumerable < AppTypeInfo > GetAppTypeInfos ( )
        {
            return ( IEnumerable < AppTypeInfo > ) _typeInfos ?? Map.Dict.Values ;
        }
    }
}