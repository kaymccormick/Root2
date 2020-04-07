using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text.Json.Serialization ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Properties ;
using Corenet.Desktop.Analysis.AnalysisAppLib.Properties ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Serialization ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// </summary>
    [NoJsonConverter]
    public sealed class TypesViewModel : ITypesViewModel
      , INotifyPropertyChanged
      , ISupportInitializeNotification
    {
        private bool _showBordersIsChecked ;

        private uint[] _hierarchyColors =
        {
            0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c , 0xff3c6e7d , 0xff533ca3
        } ;

        private AppTypeInfo   root ;
        private List < Type > _nodeTypes ;

        private TypeMapDictionary map = new TypeMapDictionary ( ) ;

        private readonly Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;
#if false
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif

        private Dictionary < Type , TypeDocInfo >
            _docs = new Dictionary < Type , TypeDocInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public TypesViewModel ( ) { }

        internal void LoadSyntaxFactoryDocs ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _docs.TryGetValue ( typeof ( SyntaxFactory ) , out var si ) ;
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                var info = ( AppTypeInfo ) Map[ methodInfo.ReturnType ] ;
                var appMethodInfo = new AppMethodInfo { MethodInfo = methodInfo } ;
                if ( si != null
                     && si.MethodDocumentation.TryGetValue ( methodInfo.Name , out var mdoc ) )
                {
                    var p = string.Join (
                                         ","
                                       , methodInfo
                                        .GetParameters ( )
                                        .Select (
                                                 parameterInfo
                                                     => parameterInfo.ParameterType.FullName
                                                )
                                        ) ;
                    //Debug.WriteLine ( $"xx: {p}" ) ;
                    foreach ( var methodDocumentation in mdoc )
                    {
                        //  Debug.WriteLine ( methodDocumentation.Parameters ) ;
                        if ( methodDocumentation.Parameters == p )
                        {
                            //    Debug.WriteLine ( $"Docs for {methodInfo}" ) ;
                            appMethodInfo.XmlDoc = methodDocumentation ;
                            CollectDoc ( methodDocumentation ) ;
                        }
                    }
                }

                info.FactoryMethods.Add ( appMethodInfo ) ;
                //Logger.Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach ( var pair in Map.dict.Where ( pair => pair.Key != typeof ( CSharpSyntaxNode ) )
            )
            {
                //}.Where ( pair => pair.Key.IsAbstract == false ) )
                {
                    foreach ( var propertyInfo in pair.Key.GetProperties (
                                                                          BindingFlags.DeclaredOnly
                                                                          | BindingFlags.Instance
                                                                          | BindingFlags.Public
                                                                         ) )
                    {
                        if ( propertyInfo.DeclaringType != pair.Key )
                        {
                            continue ;
                        }

                        var t = propertyInfo.PropertyType ;
                        // if ( t == typeof ( SyntaxToken ) )
                        // {
                        // continue ;
                        // }

                        var isList = false ;
                        AppTypeInfo typeInfo = null ;
                        AppTypeInfo otherTypeInfo = null ;
                        if ( t.IsGenericType )
                        {
                            var targ = t.GenericTypeArguments[ 0 ] ;
                            if ( typeof ( SyntaxNode ).IsAssignableFrom ( targ )
                                 && typeof ( IEnumerable ).IsAssignableFrom ( t ) )
                            {
                                isList   = true ;
                                typeInfo = ( AppTypeInfo ) Map[ targ ] ;
                            }
                        }
                        else
                        {
                            if ( ! Map.dict.TryGetValue ( t , out typeInfo ) )
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
                        if ( pair.Value.Type != null
                             && _docs.TryGetValue ( pair.Value.Type , out var info ) )
                        {
                            if ( info.PropertyDocumentation.TryGetValue (
                                                                         propertyInfo.Name
                                                                       , out propDoc
                                                                        ) )
                            {
                            }
                        }

                        CollectDoc ( propDoc ) ;
                        pair.Value.Components.Add (
                                                   new ComponentInfo
                                                   {
                                                       XmlDoc         = propDoc
                                                     , IsSelfOwned    = true
                                                     , OwningTypeInfo = pair.Value
                                                     , IsList         = isList
                                                     , TypeInfo       = typeInfo ?? otherTypeInfo
                                                     , PropertyName   = propertyInfo.Name
                                                   }
                                                  ) ;
                        //Logger.Info ( t.ToString ( ) ) ;
                    }
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [ NotNull ]
        public static Dictionary < Type , TypeDocInfo > LoadXmlDoc ( )
        {
            var docDict = new Dictionary < Type , TypeDocInfo > ( ) ;
            var doc = LoadDoc ( ) ;
            if ( doc.DocumentElement == null )
            {
                throw new InvalidOperationException ( ) ;
            }

            foreach ( var xmlElement in DocMembers ( doc ) )
            {
                var elem = HandleDocElement ( xmlElement ) ;
                if ( elem == null )
                {
                    continue ;
                }

                if ( ! docDict.TryGetValue ( elem.Type , out var info ) )
                {
                    info                 = new TypeDocInfo ( ) ;
                    docDict[ elem.Type ] = info ;
                }

                switch ( elem )
                {
                    case FieldDocumentation fieldDocumentation :
                        info.FieldDocumentation[ fieldDocumentation.MemberName ] =
                            fieldDocumentation ;
                        break ;
                    case MethodDocumentation methodDocumentation :
                        if ( ! info.MethodDocumentation.TryGetValue (
                                                                     methodDocumentation.MemberName
                                                                   , out var docs
                                                                    ) )
                        {
                            docs =
                                new List < MethodDocumentation > ( ) ;
                            info.MethodDocumentation[ methodDocumentation.MemberName ] = docs ;
                        }

                        docs.Add ( methodDocumentation ) ;
                        break ;
                    case PropertyDocumentation propertyDocumentation :
                        info.PropertyDocumentation[ propertyDocumentation.MemberName ] =
                            propertyDocumentation ;

                        break ;
                    case TypeDocumentation typeDocumentation :
                        info.TypeDocumentation = typeDocumentation ;
                        break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( elem ) ) ;
                }
            }

            return docDict ;
        }

        /// <summary>
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ NotNull ]
        public static IEnumerable < XmlElement > DocMembers ( [ NotNull ] XmlDocument doc )
        {
            if ( doc.DocumentElement == null )
            {
                throw new InvalidOperationException ( ) ;
            }

            return doc.DocumentElement.ChildNodes.OfType < XmlElement > ( )
                      .First ( child => child.Name == "members" )
                      .ChildNodes.OfType < XmlElement > ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public static XmlDocument LoadDoc ( )
        {
            var xml = Resource1.doc;
            var docuDoc = new XmlDocument ( ) ;
            docuDoc.LoadXml ( xml ) ;
            return docuDoc ;
        }

        /// <summary>
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [ CanBeNull ]
        public static CodeElementDocumentation HandleDocElement (
            [ NotNull ] XmlElement xmlElement
        )
        {
            if ( xmlElement == null )
            {
                throw new ArgumentNullException ( nameof ( xmlElement ) ) ;
            }

            var elementId = xmlElement.GetAttribute ( "name" ) ;
            var doc = XDocument.Parse ( xmlElement.OuterXml ) ;
            var xNodes = doc.Element ( "member" )?.Nodes ( ) ;

            var xmlDoc = ( xNodes ?? throw new InvalidOperationException ( ) ).Select ( XmlDocElements.Selector ) ;
            var kind = elementId[ 0 ] ;
            var type = elementId.Substring ( 2 ) ;
            string parameters = null ;
            if ( type.Contains ( '(' ) )
            {
                var leftParen = type.IndexOf ( '(' ) ;
                var rightParen = type.LastIndexOf ( ')' ) ;

                parameters = type.Substring ( leftParen + 1 , rightParen - leftParen - 1 ) ;
                type       = type.Substring ( 0 ,             leftParen ) ;
            }

            string memberName = null ;
            if ( kind    == 'M'
                 || kind == 'P'
                 || kind == 'F' )
            {
                memberName = type.Substring ( type.LastIndexOf ( '.' ) + 1 ) ;
                type       = type.Substring ( 0 , type.LastIndexOf ( '.' ) ) ;
            }

            var t = typeof ( CSharpSyntaxNode ).Assembly.GetType ( type ) ;
            if ( t == null )
            {
                Debug.WriteLine ( $"cant find type {type}" ) ;
                return null ;
            }

            //var t = Type.GetType ( type ) ;
            switch ( kind )
            {
                case 'T' : return new TypeDocumentation ( elementId , t , xmlDoc ) ;
                case 'M' :
                    return new MethodDocumentation (
                                                    elementId
                                                  , t
                                                  , memberName
                                                  , parameters
                                                  , xmlDoc
                                                   ) ;
                case 'P' :
                    return new PropertyDocumentation (
                                                      elementId
                                                    , t
                                                    , memberName
                                                      ?? throw new InvalidOperationException ( )
                                                    , xmlDoc
                                                     ) ;
                case 'F' :
                    return new FieldDocumentation (
                                                   elementId
                                                 , t
                                                 , memberName
                                                   ?? throw new InvalidOperationException ( )
                                                 , xmlDoc
                                                  ) ;
                default : throw new InvalidOperationException ( kind.ToString ( ) ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        [JsonIgnore]
        public AppTypeInfo Root
        {
            get { return root ; }
            set
            {
                root = value ;
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
        /// 
        /// </summary>
        public DocumentCollection DocumentCollection { get ; set ; } = new DocumentCollection ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public TypeMapDictionary Map { get { return map ; } set { map = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public AppTypeInfoCollection StructureRoot { get ; set ; } = new AppTypeInfoCollection();

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            TypeDocumentation docNode = null ;

            if ( _docs.TryGetValue (
                                    rootR ?? throw new InvalidOperationException ( )
                                  , out var info
                                   ) )
            {
                docNode = info.TypeDocumentation ;
            }

            CollectDoc ( docNode ) ;
            var r = new AppTypeInfo
                    {
                        Type           = rootR
                      , DocInfo        = docNode
                      , ParentInfo     = parentTypeInfo
                      , HierarchyLevel = level
                      , ColorValue     = HierarchyColors[ level ]
                    } ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                r.SubTypeInfos.Add ( CollectTypeInfos ( r , type1 , level + 1 ) ) ;
            }

            Map[ rootR ] = r ;
            return r ;
        }

        private void CollectDoc ( [ CanBeNull ] CodeElementDocumentation docNode )
        {
            if ( docNode != null )
            {
                DocumentCollection.Add ( docNode ) ;
            }
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
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( )
        {
            if ( DocumentCollection.Count == 0 )
            {
                _docs = LoadXmlDoc ( ) ;
            }

            if ( Map.Count != 0 )
            {
                LoadTypeInfo2 ( ) ;
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

            LoadSyntaxFactoryDocs ( ) ;
//            StructureRoot = new AppTypeInfoCollection { Map[ typeof ( CompilationUnitSyntax ) ] } ;
            IsInitialized = true ;
        }

        public void DetailFields ( )
        {
            foreach ( var keyValuePair in Map.dict )
            {
                foreach ( SyntaxFieldInfo rField in keyValuePair.Value.Fields )
                {
                    if ( rField.Type != null
                         && rField.Type.IsGenericType
                         && ( rField.Type.GetGenericTypeDefinition ( ) == typeof ( SyntaxList <> )
                              || rField.Type.GetGenericTypeDefinition ( )
                              == typeof ( SeparatedSyntaxList <> ) ) )
                    {
                        var tz = rField.Type.GetGenericArguments ( )[ 0 ] ;
                        if ( Map.Contains ( tz ) )
                        {
                            var ati = Map.dict[ tz ] ;
                            var types = new List < AppTypeInfo > ( ) ;
                            Collect ( ati , types ) ;
                            foreach ( var appTypeInfo in types )
                            {
                                rField.Types.Add ( appTypeInfo ) ;
                            }
                        }
                    }
                    else
                    {
                        if ( rField.Type != null && Map.Contains(rField.Type))
                        {
                            rField.Types.Add ( Map.dict[ rField.Type ] ) ;
                        }
                    }
                }
            }
        }

        private void LoadTypeInfo2 ( )
        {
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos2 ( null , rootR ) ;
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo parentTypeInfo
          , [ NotNull ] Type        rootR
          , int         level = 0
        )
        {
            // if (_docs.TryGetValue(
            //                       rootR ?? throw new InvalidOperationException()
            //                     , out var info
            //                      ))
            // {
            //     docNode = info.TypeDocumentation;
            // }

//            CollectDoc(docNode);
            var r = Map.dict[ rootR ] ;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            r.ColorValue     = HierarchyColors[ level ] ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                r.SubTypeInfos.Add ( CollectTypeInfos2 ( r , type1 , level + 1 ) ) ;
            }

            PopulateFieldTypes ( r ) ;

            return r ;
        }

        public void PopulateFieldTypes ( AppTypeInfo r )
        {
            foreach ( SyntaxFieldInfo rField in r.Fields )
            {
                if ( rField.TypeName == "SyntaxList<SyntaxToken>" )
                {
                    rField.TypeName = "SyntaxTokenList" ;
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
                                                                     "Microsoft.CodeAnalysis." + id + "`1"
                                                                    ) ;
                    if ( t0 == null )
                    {
                        Debug.WriteLine ( "fail" + id ) ;
                    }
                    else
                    {
                        var s = ( SimpleNameSyntax ) gns.TypeArgumentList.Arguments[ 0 ] ;
                        var t1 = typeof ( CSharpSyntaxNode ).Assembly.GetType (
                                                                               "Microsoft.CodeAnalysis.CSharp.Syntax."
                                                                               + s.Identifier.ValueText
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
                            Debug.WriteLine ( rField.TypeName ) ;
                        }
                    }

                    rField.Type = t ;
                }
            }
        }

        private void Collect ( [ NotNull ] AppTypeInfo ati , List < AppTypeInfo > types )
        {
            if ( ! ati.Type.IsAbstract )
            {
                types.Add ( ati ) ;
            }

            foreach ( AppTypeInfo atiSubTypeInfo in ati.SubTypeInfos )
            {
                Collect ( atiSubTypeInfo , types ) ;
            }
        }

        private void PopulateMap ( [ NotNull ] AppTypeInfo appTypeInfo )
        {
            Map[ appTypeInfo.Type ] = appTypeInfo ;
            foreach ( AppTypeInfo subTypeInfo in appTypeInfo.SubTypeInfos )
            {
                PopulateMap ( subTypeInfo ) ;
            }
        }

        private void LoadTypeInfo ( )
        {
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( null , rootR ) ;
        }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized { get ; set ; }

        /// <summary>
        /// </summary>
        public event EventHandler Initialized ;
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class TypeMapDictionary : IDictionary , ICollection , IEnumerable
    {
        internal Dictionary < Type , AppTypeInfo >
            dict = new Dictionary < Type , AppTypeInfo > ( ) ;

        private IDictionary _dict ;
        /// <summary>
        /// 
        /// </summary>
        public TypeMapDictionary ( ) { _dict = dict ; }
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains ( object key ) { return _dict.Contains ( key ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add ( object key , object value ) { _dict.Add ( key , value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _dict.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator ( ) { return _dict.GetEnumerator ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove ( object key ) { _dict.Remove ( key ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public object this [ object key ]
        {
            get { return _dict[ key ] ; }
            set { _dict[ key ] = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection Keys { get { return _dict.Keys ; } }

        /// <summary>
        /// 
        /// </summary>
        public ICollection Values { get { return _dict.Values ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly { get { return _dict.IsReadOnly ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize { get { return _dict.IsFixedSize ; } }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _dict ).GetEnumerator ( ) ;
        }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _dict.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return _dict.Count ; } }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot { get { return _dict.SyncRoot ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized { get { return _dict.IsSynchronized ; } }
        #endregion
    }
}