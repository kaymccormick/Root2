using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
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
    public sealed class TypesViewModel : ITypesViewModel , INotifyPropertyChanged
    {
        private bool _showBordersIsChecked ;

        private uint[] _hierarchyColors =
        {
            0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c , 0xff3c6e7d , 0xff533ca3
        } ;

        private          AppTypeInfo   root ;
        private readonly List < Type > _nodeTypes ;

        private readonly Dictionary < Type , AppTypeInfo > map =
            new Dictionary < Type , AppTypeInfo > ( ) ;

        private readonly Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;
#if false
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif

        private readonly Dictionary < Type , TypeDocInfo > _docs ;
        private          ObservableCollection <CodeElementDocumentation> docelems = new ObservableCollection < CodeElementDocumentation > ();

        /// <summary>
        /// 
        /// </summary>
        public TypesViewModel ( )
        {
            //var path = Path.ChangeExtension ( typeof ( CSharpSyntaxNode ).Assembly.Location , ".xml" ) ;
            _docs = LoadXmlDoc ( ) ;


            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( null , rootR ) ;

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
                            r = new Param ( element.Nodes ( ).Select ( Selector ) ) ;
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
        public uint[] HierarchyColors
        {
            get { return _hierarchyColors ; }
            set { _hierarchyColors = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < CodeElementDocumentation > Docelems
        {
            get { return docelems ; }
            set { docelems = value ; }
        }

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
                Docelems.Add ( docNode ) ;
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
    }
}