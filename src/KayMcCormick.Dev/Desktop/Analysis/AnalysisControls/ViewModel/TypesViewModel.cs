﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Windows.Markup ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib.Properties ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// </summary>
    public sealed class TypesViewModel : ITypesViewModel , INotifyPropertyChanged, ISupportInitialize
    {
        private bool _showBordersIsChecked ;

        private uint[] _hierarchyColors =
        {
            0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c , 0xff3c6e7d , 0xff533ca3
        } ;

        private          AppTypeInfo   root ;
        private List < Type > _nodeTypes ;

        private readonly Dictionary < Type , AppTypeInfo > map =
            new Dictionary < Type , AppTypeInfo > ( ) ;

        private readonly Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;
#if false
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif

        private Dictionary < Type , TypeDocInfo > _docs = new Dictionary < Type , TypeDocInfo > ();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public TypesViewModel ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public void LoadSyntaxFactoryDocs( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            _docs.TryGetValue ( typeof ( SyntaxFactory ), out var si ) ;
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                var info = map[ methodInfo.ReturnType ] ;
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

            foreach ( var pair in map.Where ( pair => pair.Key != typeof ( CSharpSyntaxNode ) ) )
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
                                // Debug.WriteLine (
                                // $"{pair.Key.Name} {propertyInfo.Name} list of {targ.Name}"
                                // ) ;
                                isList   = true ;
                                typeInfo = map[ targ ] ;
                            }
                        }
                        else
                        {
                            if ( ! map.TryGetValue ( t , out typeInfo ) )
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
                             && _docs.TryGetValue ( pair.Value.Type, out var info ) )
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
            var docDict = new Dictionary < Type, TypeDocInfo > ( ) ;
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
            var xml = Resources.doc ;
            var docuDoc = new XmlDocument ( ) ;
            docuDoc.LoadXml(xml);
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
            xmlElement.WriteTo ( new MyWriter ( ) ) ;
            var doc = XDocument.Parse ( xmlElement.OuterXml ) ;
            var xNodes = doc.Element ( "member" )?.Nodes ( ) ;

            var xmlDoc = ( xNodes ?? throw new InvalidOperationException ( ) ).Select ( Selector ) ;
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
                Debug.WriteLine($"cant find type {type}");
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
                    return new PropertyDocumentation ( elementId , t , memberName , xmlDoc ) ;
                case 'F' :
                    return new FieldDocumentation ( elementId , t , memberName , xmlDoc ) ;
                default : throw new InvalidOperationException ( kind.ToString ( ) ) ;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <exception cref="UnrecognizedElementException"></exception>
        [ CanBeNull ]
        public static XmlDocElement Selector ( [ NotNull ] XNode node )
        {
            switch ( node )
            {
                case XComment xComment : break ;
                case XCData xcData :     return new XmlDocText ( xcData.Value ) ;

                case XDocument xDocument : break ;
                case XElement element :
                {
                    XmlDocElement r = null ;
                    switch ( element.Name.LocalName )
                    {
                        case "summary" :
                            r = new Summary ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "see" :
                            r = new Crossref (
                                              element.Attribute ( XName.Get ( "cref" , "" ) )?.Value
                                              ?? ""
                                             ) ;
                            break ;
                        case "paramref" :
                            r = new Paramref (
                                              element.Attribute ( XName.Get ( "name" , "" ) )?.Value
                                              ?? ""
                                             ) ;
                            break ;
                        case "c" :
                            r = new Code ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "para" :
                            r = new Para ( element.Nodes ( ).Select ( Selector ) ) ;

                            break ;
                        case "seealso" :
                            r = new Seealso ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "em" :
                            r = new Em ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "pre" :
                            r = new Pre ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "a" :
                            r = new Anchor (
                                            element.Attribute ( XName.Get ( "href" , "" ) )?.Value
                                            ?? ""
                                          , element.Nodes ( ).Select ( Selector )
                                           ) ;
                            break ;
                        case "typeparamref" :
                            r = new Typeparamref (
                                                  element.Attribute ( XName.Get ( "name" , "" ) )
                                                        ?.Value
                                                  ?? ""
                                                 ) ;
                            break ;
                        case "param" :
                            r = new Param (element.Attribute(XName.Get("name", ""))
                                                 ?.Value
                                           ?? "") ;
                            break ;
                        case "example" :
                            r = new Example ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                    }

                    if ( r != null )
                    {
                        // Debug.WriteLine ( r.ToString ( ) ) ;
                    }

                    return r ;
                }
                case XContainer xContainer :                         break ;
                case XDocumentType xDocumentType :                   break ;
                case XProcessingInstruction xProcessingInstruction : break ;
                case XText xText :
                    return new XmlDocText ( xText.Value ) ;
            }

            throw new UnrecognizedElementException ( node.GetType ( ).FullName ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public uint[] HierarchyColors
        {
            get { return _hierarchyColors ; }
            set { _hierarchyColors = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCollection DocumentCollection { get ; set ; } = new DocumentCollection ( ) ;
        [ NotNull ]
        private AppTypeInfo CollectTypeInfos (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            TypeDocumentation docNode = null ;

            if ( _docs.TryGetValue (
                                    rootR?? throw new InvalidOperationException ( )
                                  , out var info
                                   ) )
            {
                docNode = info.TypeDocumentation ;
            }

            CollectDoc(docNode);
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

            map[ rootR ] = r ;
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

            if (Root == null ||  Root.SubTypeInfos.Count == 0 )
            {
                LoadTypeInfo ( ) ;
            }
            else
            {
                PopulateMap ( Root ) ;
            }

            LoadSyntaxFactoryDocs();
        }

        private void PopulateMap ( [ NotNull ] AppTypeInfo appTypeInfo )
        {
            map[ appTypeInfo.Type ] = appTypeInfo ;
            foreach ( AppTypeInfo subTypeInfo in appTypeInfo.SubTypeInfos )
            {
                PopulateMap(subTypeInfo);
            }
        }

        private void LoadTypeInfo ( )
        {
            var rootR = typeof(CSharpSyntaxNode);
            _nodeTypes = rootR.Assembly.GetExportedTypes()
                              .Where(t => typeof(CSharpSyntaxNode).IsAssignableFrom(t))
                              .ToList();
            Root = CollectTypeInfos(null, rootR);

        }
        #endregion
    }

    
    public class MemberBaseDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        protected string _memberName;

        /// <summary>
        /// 
        /// </summary>
        protected MemberBaseDocumentation() { }

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDocElements"></param>
        protected MemberBaseDocumentation(
            string elementId
          , [NotNull] Type type
          , [NotNull] string memberName
          , IEnumerable<XmlDocElement> xmlDocElements
        ) : base(elementId, xmlDocElements)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
        }

        /// <summary>
        /// </summary>
        public string MemberName { get { return _memberName; } set { _memberName = value; } }
    }

    /// <summary>
    /// </summary>
    [ContentProperty("XmlDoc")]
    public sealed class FieldDocumentation : MemberBaseDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDocElements"></param>
        public FieldDocumentation(
            string elementId
          , [NotNull] Type type
          , [ NotNull ] string memberName
          , IEnumerable<XmlDocElement> xmlDocElements
        ) : base(elementId, type, memberName, xmlDocElements)
        {
        }
    }

    /// <summary>
    /// </summary>
    public class PropertyDocumentation : MemberBaseDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDoc"></param>
        public PropertyDocumentation(
            string elementId
          , [ NotNull ] Type type
          , [ NotNull ] string memberName
          , IEnumerable<XmlDocElement> xmlDoc
        ) : base(elementId, type, memberName, xmlDoc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDocumentation() { }
    }

    /// <summary>
    /// </summary>
    [ContentProperty("XmlDoc")]
    public sealed class TypeDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="xmlDoc"></param>
        public TypeDocumentation(
            string elementId
          , Type type
          , IEnumerable<XmlDocElement> xmlDoc
        ) : base(elementId, xmlDoc)
        {
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public TypeDocumentation() { }
    }

    /// <summary>
    /// </summary>
    [ContentProperty("XmlDoc")]
    public class CodeElementDocumentation
    {
        private string _elementId;

        /// <summary>
        /// 
        /// </summary>
        protected Type _type;

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocumentElementCollection _xmlDoc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        protected CodeElementDocumentation(
            string elementId
          , [ NotNull ] IEnumerable<XmlDocElement> xmlDoc
        )
        {
            _xmlDoc = new XmlDocumentElementCollection(xmlDoc.ToList());
            ElementId = elementId;
        }

        /// <summary>
        /// 
        /// </summary>
        protected CodeElementDocumentation()
        {
            _xmlDoc = new XmlDocumentElementCollection();
        }

        /// <summary>
        /// </summary>
        public string ElementId { get { return _elementId; } set { _elementId = value; } }

        /// <summary>
        /// </summary>
        public Type Type { get { return _type; } set { _type = value; } }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentElementCollection XmlDoc
        {
            get { return _xmlDoc; }
            set { _xmlDoc = value; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var doc = XmlDoc != null ? string.Join("", XmlDoc) : "";
            return
                $" {GetType().Name}{nameof(ElementId)}: {ElementId}, {nameof(XmlDoc)}: {doc}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class XmlDocumentElementCollection : IList, IEnumerable, ICollection, IList <XmlDocElement>, ICollection<XmlDocElement>, IEnumerable<XmlDocElement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listImplementation"></param>
        public XmlDocumentElementCollection(List < XmlDocElement > listImplementation)
        {
            _listImplementation = listImplementation;
        }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentElementCollection()
        {
            _listImplementation = new List<XmlDocElement>();
        }

        private readonly List<XmlDocElement> _listImplementation;
        #region Implementation of IEnumerable
        public IEnumerator < XmlDocElement > GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }

        IEnumerator IEnumerable.GetEnumerator ( ) { return ( ( IEnumerable ) _listImplementation ).GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index ) { ( ( ICollection ) _listImplementation ).CopyTo ( array , index ) ; }

        public bool Remove ( XmlDocElement item ) { return _listImplementation.Remove ( item ) ; }

        int ICollection < XmlDocElement >.Count
        {
            get { return _listImplementation.Count ; }
        }

        bool ICollection < XmlDocElement >.IsReadOnly => false ;

        int ICollection.Count
        {
            get { return _listImplementation.Count ; }
        }

        public object SyncRoot
        {
            get { return ( ( ICollection ) _listImplementation ).SyncRoot ; }
        }

        public bool IsSynchronized
        {
            get { return ( ( ICollection ) _listImplementation ).IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return ( ( IList ) _listImplementation ).Add ( value ) ; }

        public bool Contains ( object value ) { return ( ( IList ) _listImplementation ).Contains ( value ) ; }

        public void Add ( XmlDocElement item ) { _listImplementation.Add ( item ) ; }

        void ICollection < XmlDocElement >.Clear ( ) { _listImplementation.Clear ( ) ; }

        public bool Contains ( XmlDocElement item ) { return _listImplementation.Contains ( item ) ; }

        public void CopyTo ( XmlDocElement[] array , int arrayIndex ) { _listImplementation.CopyTo ( array , arrayIndex ) ; }

        void IList.Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return ( ( IList ) _listImplementation ).IndexOf ( value ) ; }

        public void Insert ( int index , object value ) { ( ( IList ) _listImplementation ).Insert ( index , value ) ; }

        public void Remove ( object value ) { ( ( IList ) _listImplementation ).Remove ( value ) ; }

        public int IndexOf ( XmlDocElement item ) { return _listImplementation.IndexOf ( item ) ; }

        public void Insert ( int index , XmlDocElement item ) { _listImplementation.Insert ( index , item ) ; }

        void IList < XmlDocElement >.RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public XmlDocElement this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        void IList.RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        object IList.this [ int index ]
        {
            get { return ( ( IList ) _listImplementation )[ index ] ; }
            set { ( ( IList ) _listImplementation )[ index ] = value ; }
        }

        bool IList.IsReadOnly
        {
            get { return ( ( IList ) _listImplementation ).IsReadOnly ; }
        }

        public bool IsFixedSize
        {
            get { return ( ( IList ) _listImplementation ).IsFixedSize ; }
        }
        #endregion
    }

    /// <summary>
    /// </summary>
    [ContentProperty("XmlDoc")]
    public sealed class MethodDocumentation : MemberBaseDocumentation
    {
        private readonly string _parameters;

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <param name="parameters"></param>
        /// <param name="xmlDoc"></param>
        public MethodDocumentation(
            string elementId
          , [NotNull] Type type
          , string member
          , string parameters
          , IEnumerable<XmlDocElement> xmlDoc
        ) : base(elementId, type, member, xmlDoc)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        public MethodDocumentation() { }

        /// <summary>
        /// 
        /// </summary>
        public string Parameters { get { return _parameters; } }
    }

    /// <summary>
    /// </summary
    [ContentProperty("Text")]
    public class XmlDocText : InlineDocElem
    {
        private string text;

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        public XmlDocText(string text) : base(Enumerable.Empty<XmlDocElement>())
        {
            this.text = text;
        }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocText()
        {
        }

        /// <summary>
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return $"{Text}"; }

        #region Overrides of XmlDocElement

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override XmlDocumentElementCollection DocumentElementCollection { get; set; }
        #endregion
    }

    /// <summary>
    /// </summary>
    public sealed class Typeparamref : InlineDocElem
    {
        public Typeparamref()
        {
        }

        private string _typeParamName;

        /// <summary>
        /// </summary>
        /// <param name="typeParamName"></param>
        public Typeparamref(string typeParamName) { _typeParamName = typeParamName; }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public string TypeParamName
        {
            get { return _typeParamName; }
            set { _typeParamName = value; }
        }
    }

    /// <summary>
    /// </summary>
    public class Example : BlockDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Example(IEnumerable<XmlDocElement> elements) : base(elements) { }

        public Example()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Param : BlockDocElem
    {
        private string _name;

        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Param(string name) : base() { Name = name; }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Param()
        {
        }

        public string Name { get { return _name; } set { _name = value; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InlineDocElem : XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public InlineDocElem(IEnumerable<XmlDocElement> elements) : base(elements) { }

        /// <summary>
        /// 
        /// </summary>
        public InlineDocElem() { }
    }

    /// <summary>
    /// </summary>
    public class Anchor : InlineDocElem
    {
        private string _href;

        /// <summary>
        /// </summary>
        /// <param name="href"></param>
        /// <param name="elements"></param>
        public Anchor(string href, IEnumerable<XmlDocElement> elements) : base(elements)
        {
            _href = href;
        }

        /// <summary>
        /// 
        /// </summary>
        public Anchor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }
    }

    /// <summary>
    /// </summary>
    public class Pre : XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public Pre(IEnumerable<XmlDocElement> elements) : base(elements) { }

        /// <summary>
        /// 
        /// </summary>
        public Pre()
        {
        }
    }

    /// <summary>
    /// </summary>
    public class Em : InlineDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Em(IEnumerable<XmlDocElement> elements) : base(elements) { }

        public Em()
        {
        }
    }

    /// <summary>
    /// </summary>
    public sealed class Seealso : XmlDocElement
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Seealso(IEnumerable<XmlDocElement> elements) : base(elements) { }

        public Seealso()
        {
        }
    }

    /// <summary>
    /// </summary>
    [ContentProperty( "DocumentElementCollection")]
    public class Para : BlockDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Para(IEnumerable<XmlDocElement> elements) : base(elements) { }

        public Para(params XmlDocElement[] elements) : base(elements) { }

        /// <summary>
        /// 
        /// </summary>
        public Para()
        {
        }
    }

    /// <summary>
    /// </summary>
    public class Paramref : InlineDocElem
    {
        private string _paramName;

        /// <summary>
        /// </summary>
        /// <param name="paramName"></param>
        public Paramref(string paramName) { _paramName = paramName; }

        /// <summary>
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
    }

    /// <summary>
    /// </summary>
    public class Code : InlineDocElem
    {
        public Code()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Code(IEnumerable<XmlDocElement> elements) : base(elements) { }
    }

    /// <summary>
    /// </summary>
    public class Crossref : InlineDocElem
    {
        private string _xRefId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xRefId"></param>
        public Crossref(string xRefId) { _xRefId = xRefId; }

        public Crossref()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string XRefId
        {
            get { return _xRefId; }
            set { _xRefId = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary
    [ContentProperty("DocumentElementCollection")]
    public class XmlDocElement
    {
        private readonly XElement _element;
        /// <summary>
        /// 
        /// </summary>
        protected XmlDocumentElementCollection _xmlDocumentElementCollection;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        protected XmlDocElement(IEnumerable<XmlDocElement> elements)
        {
            _xmlDocumentElementCollection = new XmlDocumentElementCollection(elements.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocElement() { _xmlDocumentElementCollection = new XmlDocumentElementCollection(); }

        /// <summary>
        /// 
        /// </summary>
        public virtual XmlDocumentElementCollection DocumentElementCollection
        {
            get { return _xmlDocumentElementCollection; }
            set { _xmlDocumentElementCollection = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ContentProperty( "DocumentElementCollection")]
    public sealed class Summary : BlockDocElem, IAddChild
    {
        

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Summary()
        {
        }

        #region Overrides of XmlDocElement
        /// <summary>
        /// 
        /// </summary>
        public override XmlDocumentElementCollection DocumentElementCollection
        {
            get { return _xmlDocumentElementCollection ; }
            set { _xmlDocumentElementCollection = value ; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public Summary(IEnumerable<XmlDocElement> elements) : base(elements) { }

        #region Implementation of IAddChild
        public void AddChild ( object value )
        {
            DocumentElementCollection.Add ( value ) ;
        }

        public void AddText ( string text )
        {
            DocumentElementCollection.Add ( new XmlDocText ( text ) ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [ContentProperty( "DocumentElementCollection")]
    public class BlockDocElem : XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        protected BlockDocElem(IEnumerable<XmlDocElement> elements) : base(elements) { }

        /// <summary>
        /// 
        /// </summary>
        protected BlockDocElem() { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UnrecognizedElementException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnrecognizedElementException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnrecognizedElementException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnrecognizedElementException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnrecognizedElementException(
            [NotNull] SerializationInfo info
          , StreamingContext context
        ) : base(info, context)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class MyWriter : XmlWriter
    {
        private readonly Stack<string> _elements = new Stack<string>();
        #region Overrides of XmlWriter
        public override void WriteStartDocument() { }

        public override void WriteStartDocument(bool standalone) { }

        public override void WriteEndDocument() { }

        public override void WriteDocType(
            string name
          , string pubid
          , string sysid
          , string subset
        )
        {
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            _elements.Push(localName);
            if (localName == "summary")
            {
            }
        }

        public override void WriteEndElement()
        {
            var elemName = _elements.Pop();
        }

        public override void WriteFullEndElement() { }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
        }

        public override void WriteEndAttribute() { }

        public override void WriteCData(string text) { }

        public override void WriteComment(string text) { }

        public override void WriteProcessingInstruction(string name, string text) { }

        public override void WriteEntityRef(string name) { }

        public override void WriteCharEntity(char ch) { }

        public override void WriteWhitespace(string ws) { }

        public override void WriteString(string text) { }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar) { }

        public override void WriteChars(char[] buffer, int index, int count) { }

        public override void WriteRaw(char[] buffer, int index, int count) { }

        public override void WriteRaw(string data) { }

        public override void WriteBase64(byte[] buffer, int index, int count) { }

        public override void Flush() { }

        public override string LookupPrefix(string ns) { return null; }

        public override WriteState WriteState { get; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class TypeDocInfo
    {
        private List<MethodDocInfo> constructorDocumentation = new List<MethodDocInfo>();

        private Dictionary<string, List<MethodDocumentation>> methodDocumentation =
            new Dictionary<string, List<MethodDocumentation>>();

        private Dictionary<string, PropertyDocumentation> propertyDocumentation =
            new Dictionary<string, PropertyDocumentation>();

        private TypeDocumentation typeDocumentation;

        /// <summary>
        /// 
        /// </summary>
        public TypeDocumentation TypeDocumentation
        {
            get { return typeDocumentation; }
            set { typeDocumentation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<MethodDocInfo> ConstructorDocumentation
        {
            get { return constructorDocumentation; }
            set { constructorDocumentation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<MethodDocumentation>> MethodDocumentation
        {
            get { return methodDocumentation; }
            set { methodDocumentation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, PropertyDocumentation> PropertyDocumentation
        {
            get { return propertyDocumentation; }
            set { propertyDocumentation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, FieldDocumentation> FieldDocumentation { get; set; } =
            new Dictionary<string, FieldDocumentation>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class DocInfo
    {
        private string _docIdentifier;
        private IEnumerable<XmlDocElement> _docNode;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<XmlDocElement> DocNode
        {
            get { return _docNode; }
            set { _docNode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DocIdentifier
        {
            get { return _docIdentifier; }
            set { _docIdentifier = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MemberDocInfo : DocInfo
    {
        private string _memberName;

        /// <summary>
        /// 
        /// </summary>
        public string MemberName { get { return _memberName; } set { _memberName = value; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MethodDocInfo : MemberDocInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Parameters { get; set; }
    }

    internal class MethodBaseDocInfo
    {
    }
}