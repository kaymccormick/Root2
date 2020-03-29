#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// TypesViewModel.cs
// 
// 2020-03-11-7:05 PM
// 
// ---
#endregion
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.VisualBasic.Syntax ;
using NLog ;
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

namespace AnalysisAppLib.ViewModel
{
    public class TypesViewModel : ITypesViewModel , INotifyPropertyChanged
    {
        private bool _showBordersIsChecked = false ;

        private uint[] _hierarchyColors = new[]
                                          {
                                              0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c
                                            , 0xff3c6e7d , 0xff533ca3
                                          } ;

        private AppTypeInfo root =
            new AppTypeInfo (
                             new ObservableCollection < AppTypeInfo > ( )
                             {
                                 new AppTypeInfo ( ) { Type = typeof ( EndBlockStatementSyntax ) }
                             }
                            ) { Type = typeof ( CSharpSyntaxNode ) } ;

        private List < Type >                     _nodeTypes ;
        private Dictionary < Type , AppTypeInfo > map = new Dictionary < Type , AppTypeInfo > ( ) ;
        private Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;
#if false
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif

        private HashSet < string > _unknownElems ;

        private Dictionary < string , TypeDocInfo > _docs ;

        public TypesViewModel ( )
        {
            //var path = Path.ChangeExtension ( typeof ( CSharpSyntaxNode ).Assembly.Location , ".xml" ) ;
            _docs = LoadXmlDoc ( ) ;


            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( null , rootR ) ;
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                var info = map[ methodInfo.ReturnType ] ;
                info.FactoryMethods.Add ( new AppMethodInfo { MethodInfo = methodInfo } ) ;
                Logger.Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
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
                                Debug.WriteLine (
                                                 $"{pair.Key.Name} {propertyInfo.Name} list of {targ.Name}"
                                                ) ;
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
                                    otherTypeInfo =
                                        otherTyps[ t ] = new AppTypeInfo ( ) { Type = t } ;
                                }
                            }
                        }

                        if ( typeInfo         == null
                             && otherTypeInfo == null )
                        {
                            continue ;
                        }

                        pair.Value.Components.Add (
                                                   new ComponentInfo ( )
                                                   {
                                                       IsSelfOwned    = true
                                                     , OwningTypeInfo = pair.Value
                                                     , IsList         = isList
                                                     , TypeInfo       = typeInfo ?? otherTypeInfo
                                                     , PropertyName   = propertyInfo.Name
                                                   }
                                                  ) ;
                        Logger.Info ( t.ToString ( ) ) ;
                    }
                }
            }
        }

        [ NotNull ]
        public static Dictionary < string , TypeDocInfo > LoadXmlDoc ( )
        {
            var docDict = new Dictionary < string , TypeDocInfo > ( ) ;
            var doc = LoadDoc ( ) ;
            if ( doc.DocumentElement == null )
            {
                throw new InvalidOperationException ( ) ;
            }

            foreach ( var xmlElement in DocMembers ( doc ) )
            {
                var elem = HandleDocElement ( xmlElement ) ;
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

        [ NotNull ]
        public static XmlDocument LoadDoc ( )
        {
            var xml =
                @"c:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisAppLib\doc.xml" ;
            var docuDoc = new XmlDocument ( ) ;
            docuDoc.Load ( xml ) ;
            return docuDoc ;
        }

        [ NotNull ]
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

                parameters = type.Substring ( leftParen , rightParen - leftParen ) ;
                type       = type.Substring ( 0 ,         leftParen ) ;
            }

            string memberName = null ;
            if ( kind    == 'M'
                 || kind == 'P'|| kind == 'F' )
            {
                memberName = type.Substring ( type.LastIndexOf ( '.' ) ) ;
                type       = type.Substring ( 0 , type.LastIndexOf ( '.' ) ) ;
            }

            switch ( kind )
            {
                case 'T' : return new TypeDocumentation ( elementId , type , xmlDoc ) ;
                case 'M' :
                    return new MethodDocumentation (
                                                    elementId
                                                  , type
                                                  , memberName
                                                  , parameters
                                                  , xmlDoc
                                                   ) ;
                case 'P' :
                    return new PropertyDocumentation ( elementId , type , memberName , xmlDoc ) ;
                case 'F' :
                    return new FieldDocumentation ( elementId , type , memberName , xmlDoc ) ;
                default : throw new InvalidOperationException ( kind.ToString ( ) ) ;
            }
        }

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
                            r = new Crossref ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "paramref" :
                            r = new Paramref ( element.Nodes ( ).Select ( Selector ) ) ;
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
                            r = new Anchor ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "typeparamref" :
                            r = new Typeparamref ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "param" :
                            r = new Param ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        case "example" :
                            r = new Example ( element.Nodes ( ).Select ( Selector ) ) ;
                            break ;
                        default : break ;
                    }

                    if ( r != null )
                    {
                        Debug.WriteLine ( r.ToString ( ) ) ;
                    }

                    return r ;
                }
                case XContainer xContainer :                         break ;
                case XDocumentType xDocumentType :                   break ;
                case XProcessingInstruction xProcessingInstruction : break ;
                case XText xText :
                    return new XmlDocText ( xText.Value ) ;
                default : break ;
            }

            throw new UnrecognizedElementException ( node.GetType ( ).FullName ) ;
        }

        public AppTypeInfo Root
        {
            get { return root ; }
            set
            {
                root = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public bool ShowBordersIsChecked
        {
            get { return _showBordersIsChecked ; }
            set
            {
                _showBordersIsChecked = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public uint[] HierarchyColors
        {
            get { return _hierarchyColors ; }
            set { _hierarchyColors = value ; }
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos (
            AppTypeInfo parentTypeInfo
          , Type        rootR
          , int         level = 0
        )
        {
            TypeDocumentation docNode = null ;

            if ( _docs.TryGetValue (
                                    rootR.FullName ?? throw new InvalidOperationException ( )
                                  , out var info
                                   ) )
            {
                docNode = info.TypeDocumentation ;
            }

            var r = new AppTypeInfo ( )
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

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    public class MemberBaseDocumentation : CodeElementDocumentation
    {
        protected string _memberName ;

        protected MemberBaseDocumentation (
            string                        elementId
          , [ NotNull ] string                        type
          , [ NotNull ] string                        memberName
          , IEnumerable < XmlDocElement > xmlDocElements
        ) : base ( elementId , xmlDocElements )
        {
            Type       = type ?? throw new ArgumentNullException ( nameof ( type ) ) ;
            MemberName = memberName ?? throw new ArgumentNullException ( nameof ( memberName ) ) ;
        }

        public string MemberName { get { return _memberName ; } set { _memberName = value ; } }
    }

    public sealed class FieldDocumentation : MemberBaseDocumentation
    {
        public FieldDocumentation (
            string                        elementId
          , string                        type
          , string                        memberName
          , IEnumerable < XmlDocElement > xmlDocElements
        ) : base ( elementId , type , memberName , xmlDocElements )
        {
        }
    }

    public class PropertyDocumentation : MemberBaseDocumentation
    {
        public PropertyDocumentation (
            string                        elementId
          , string                        type
          , string                        memberName
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , type , memberName , xmlDoc )
        {
        }
    }

    public sealed class TypeDocumentation : CodeElementDocumentation
    {
        public TypeDocumentation (
            string                        elementId
          , string                        type
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , xmlDoc )
        {
            Type = type ;
        }
    }

    public class CodeElementDocumentation
    {
        protected string                 _type ;
        protected List < XmlDocElement > _xmlDoc ;
        private   string                 _elementId ;

        protected CodeElementDocumentation (
            string                        elementId
          , IEnumerable < XmlDocElement > xmlDoc
        )
        {
            _xmlDoc   = xmlDoc.ToList ( ) ;
            ElementId = elementId ;
        }

        public string ElementId { get { return _elementId ; } set { _elementId = value ; } }

        public string Type { get { return _type ; } set { _type = value ; } }

        public IEnumerable < XmlDocElement > XmlDoc { get { return _xmlDoc ; } }

        public override string ToString ( )
        {
            return
                $" {GetType ( ).Name}{nameof ( ElementId )}: {ElementId}, {nameof ( XmlDoc )}: {string.Join ( "" , XmlDoc ?? Enumerable.Empty < XmlDocElement > ( ) )}," ;
        }
    }

    public sealed class MethodDocumentation : MemberBaseDocumentation
    {
        private readonly string _parameters ;

        public MethodDocumentation (
            string                        elementId
          , string                        type
          , string                        member
          , string                        parameters
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , type , member , xmlDoc )
        {
            _parameters = parameters ;
        }

        public string Parameters { get { return _parameters ; } }
    }

    public class XmlDocText : XmlDocElement
    {
        private readonly string text ;

        public XmlDocText ( string text ) : base ( Enumerable.Empty < XmlDocElement > ( ) )
        {
            this.text = text ;
        }

        public          string Text         { get { return text ; } }
        public override string ToString ( ) { return $"{Text}" ; }
    }

    public class Typeparamref : XmlDocElement
    {
        public Typeparamref ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Example : XmlDocElement
    {
        public Example ( XElement element ) : base ( element ) { }

        public Example ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Param : XmlDocElement
    {
        public Param ( XElement element ) : base ( element ) { }

        public Param ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Anchor : XmlDocElement
    {
        public Anchor ( XElement element ) : base ( element ) { }

        public Anchor ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Pre : XmlDocElement
    {
        public Pre ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        public Pre ( XElement element ) : base ( element ) { }
    }

    public class Em : XmlDocElement
    {
        public Em ( XElement element ) : base ( element ) { }

        public Em ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Seealso : XmlDocElement
    {
        public Seealso ( XElement element ) : base ( element ) { }

        public Seealso ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Para : XmlDocElement
    {
        public Para ( XElement element ) : base ( element ) { }

        public Para ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Paramref : XmlDocElement
    {
        public Paramref ( XElement element ) : base ( element ) { }

        public Paramref ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Code : XmlDocElement
    {
        public Code ( XElement element ) : base ( element ) { }

        public Code ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class Crossref : XmlDocElement
    {
        // public Crossref ( XElement element ) : base ( element )
        // {
        // }


        public Crossref ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class XmlDocElement
    {
        private readonly List < XmlDocElement > _elements ;
        private readonly XElement               _element ;

        public XmlDocElement ( XElement element ) { _element = element ; }

        public XmlDocElement ( IEnumerable < XmlDocElement > elements )
        {
            _elements = elements.ToList ( ) ;
        }

        public List < XmlDocElement > Elements { get { return _elements ; } }

        public override string ToString ( )
        {
            return $"[{GetType ( ).Name}] {string.Join ( " " , Elements )}" ;
        }
    }

    public class Summary : XmlDocElement
    {
        public Summary ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    public class UnrecognizedElementException : Exception
    {
        public UnrecognizedElementException ( ) { }

        public UnrecognizedElementException ( string message ) : base ( message ) { }

        public UnrecognizedElementException ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        protected UnrecognizedElementException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }

    public class MyWriter : XmlWriter
    {
        private Stack < string > _elements = new Stack < string > ( ) ;
        #region Overrides of XmlWriter
        public override void WriteStartDocument ( ) { }

        public override void WriteStartDocument ( bool standalone ) { }

        public override void WriteEndDocument ( ) { }

        public override void WriteDocType (
            string name
          , string pubid
          , string sysid
          , string subset
        )
        {
        }

        public override void WriteStartElement ( string prefix , string localName , string ns )
        {
            _elements.Push ( localName ) ;
            if ( localName == "summary" )
            {
            }
        }

        public override void WriteEndElement ( )
        {
            var elemName = _elements.Pop ( ) ;
        }

        public override void WriteFullEndElement ( ) { }

        public override void WriteStartAttribute ( string prefix , string localName , string ns )
        {
        }

        public override void WriteEndAttribute ( ) { }

        public override void WriteCData ( string text ) { }

        public override void WriteComment ( string text ) { }

        public override void WriteProcessingInstruction ( string name , string text ) { }

        public override void WriteEntityRef ( string name ) { }

        public override void WriteCharEntity ( char ch ) { }

        public override void WriteWhitespace ( string ws ) { }

        public override void WriteString ( string text ) { }

        public override void WriteSurrogateCharEntity ( char lowChar , char highChar ) { }

        public override void WriteChars ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( string data ) { }

        public override void WriteBase64 ( byte[] buffer , int index , int count ) { }

        public override void Flush ( ) { }

        public override string LookupPrefix ( string ns ) { return null ; }

        public override WriteState WriteState { get ; }
        #endregion
    }

    public class TypeDocInfo
    {
        private TypeDocumentation      typeDocumentation ;
        private List < MethodDocInfo > constructorDocumentation = new List < MethodDocInfo > ( ) ;

        private Dictionary < string , List < MethodDocumentation > > methodDocumentation =
            new Dictionary < string , List < MethodDocumentation > > ( ) ;

        private Dictionary < string , PropertyDocumentation > propertyDocumentation =
            new Dictionary < string , PropertyDocumentation > ( ) ;

        public TypeDocumentation TypeDocumentation
        {
            get { return typeDocumentation ; }
            set { typeDocumentation = value ; }
        }

        public List < MethodDocInfo > ConstructorDocumentation
        {
            get { return constructorDocumentation ; }
            set { constructorDocumentation = value ; }
        }

        public Dictionary < string , List < MethodDocumentation > > MethodDocumentation
        {
            get { return methodDocumentation ; }
            set { methodDocumentation = value ; }
        }

        public Dictionary < string , PropertyDocumentation > PropertyDocumentation
        {
            get { return propertyDocumentation ; }
            set { propertyDocumentation = value ; }
        }

        public Dictionary<string,  FieldDocumentation> FieldDocumentation { get ; set ; } = new Dictionary < string , FieldDocumentation > ();

    }

    public class DocInfo
    {
        private IEnumerable < XmlDocElement > _docNode ;
        private string                        _docIdentifier ;

        public IEnumerable < XmlDocElement > DocNode
        {
            get { return _docNode ; }
            set { _docNode = value ; }
        }

        public string DocIdentifier
        {
            get { return _docIdentifier ; }
            set { _docIdentifier = value ; }
        }
    }

    public class MemberDocInfo : DocInfo
    {
        private string _memberName ;

        public string MemberName { get { return _memberName ; } set { _memberName = value ; } }
    }

    public class MethodDocInfo : MemberDocInfo
    {
        public string Parameters { get ; set ; }
    }

    internal class MethodBaseDocInfo
    {
    }
}