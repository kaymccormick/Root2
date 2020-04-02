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
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Windows.Markup ;
using System.Xml ;
using System.Xml.Linq ;
using JetBrains.Annotations ;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    public class MemberBaseDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        protected string _memberName ;

        protected MemberBaseDocumentation ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDocElements"></param>
        protected MemberBaseDocumentation (
            string                        elementId
          , [ NotNull ] Type            type
          , [ NotNull ] string            memberName
          , IEnumerable < XmlDocElement > xmlDocElements
        ) : base ( elementId , xmlDocElements )
        {
            Type       = type       ?? throw new ArgumentNullException ( nameof ( type ) ) ;
            MemberName = memberName ?? throw new ArgumentNullException ( nameof ( memberName ) ) ;
        }

        /// <summary>
        /// </summary>
        public string MemberName { get { return _memberName ; } set { _memberName = value ; } }
    }

    /// <summary>
    /// </summary>
    [ ContentProperty ( "XmlDoc" ) ]
    public sealed class FieldDocumentation : MemberBaseDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDocElements"></param>
        public FieldDocumentation (
            string                        elementId
          , [ NotNull ] Type                        type
          , string                        memberName
          , IEnumerable < XmlDocElement > xmlDocElements
        ) : base ( elementId , type , memberName , xmlDocElements )
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
        public PropertyDocumentation (
            string                        elementId
          , Type type
          , string                        memberName
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , type , memberName , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDocumentation ( ) : base ( ) { }
    }

    /// <summary>
    /// </summary>
    [ ContentProperty ( "XmlDoc" ) ]
    public sealed class TypeDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="xmlDoc"></param>
        public TypeDocumentation (
            string                        elementId
          , Type                        type
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , xmlDoc )
        {
            Type = type ;
        }

        /// <summary>
        /// 
        /// </summary>
        public TypeDocumentation ( ) { }
    }

    /// <summary>
    /// </summary>
    [ ContentProperty ( "XmlDoc" ) ]
    public class CodeElementDocumentation
    {
        private string _elementId ;

        /// <summary>
        /// 
        /// </summary>
        protected Type _type ;

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocumentElementCollection _xmlDoc ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        protected CodeElementDocumentation (
            string                        elementId
          , IEnumerable < XmlDocElement > xmlDoc
        )
        {
            _xmlDoc   = new XmlDocumentElementCollection ( xmlDoc.ToList ( ) ) ;
            ElementId = elementId ;
        }

        /// <summary>
        /// 
        /// </summary>
        protected CodeElementDocumentation ( ) { }

        /// <summary>
        /// </summary>
        public string ElementId { get { return _elementId ; } set { _elementId = value ; } }

        /// <summary>
        /// </summary>
        public Type Type { get { return _type ; } set { _type = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentElementCollection XmlDoc
        {
            get { return _xmlDoc ; }
            set { _xmlDoc = value ; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            var doc = XmlDoc != null ? string.Join ( "" , XmlDoc ) : "" ;
            return
                $" {GetType ( ).Name}{nameof ( ElementId )}: {ElementId}, {nameof ( XmlDoc )}: {doc}" ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class XmlDocumentElementCollection : IList , IEnumerable , ICollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listImplementation"></param>
        public XmlDocumentElementCollection ( IList listImplementation )
        {
            _listImplementation = listImplementation ;
        }

        /// <summary>
        /// 
        /// </summary>
        public XmlDocumentElementCollection ( )
        {
            _listImplementation = new List < XmlDocElement > ( ) ;
        }

        private readonly IList _listImplementation ;
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index )
        {
            _listImplementation.CopyTo ( array , index ) ;
        }

        public int Count { get { return _listImplementation.Count ; } }

        public object SyncRoot { get { return _listImplementation.SyncRoot ; } }

        public bool IsSynchronized { get { return _listImplementation.IsSynchronized ; } }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        public void Insert ( int index , object value )
        {
            _listImplementation.Insert ( index , value ) ;
        }

        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        public bool IsReadOnly { get { return _listImplementation.IsReadOnly ; } }

        public bool IsFixedSize { get { return _listImplementation.IsFixedSize ; } }
        #endregion
    }

    /// <summary>
    /// </summary>
    [ ContentProperty ( "XmlDoc" ) ]
    public sealed class MethodDocumentation : MemberBaseDocumentation
    {
        private readonly string _parameters ;

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <param name="parameters"></param>
        /// <param name="xmlDoc"></param>
        public MethodDocumentation (
            string                        elementId
          , [ NotNull ] Type            type
          , string                        member
          , string                        parameters
          , IEnumerable < XmlDocElement > xmlDoc
        ) : base ( elementId , type , member , xmlDoc )
        {
            _parameters = parameters ;
        }

        /// <summary>
        /// 
        /// </summary>
        public MethodDocumentation ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public string Parameters { get { return _parameters ; } }
    }

    /// <summary>
    /// </summary
    [ContentProperty("Text")]
    public class XmlDocText : InlineDocElem
    {
        private string text ;

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        public XmlDocText ( string text ) : base ( Enumerable.Empty < XmlDocElement > ( ) )
        {
            this.text = text ;
        }

        /// <summary>
        /// </summary>
        public string Text
        {
            get { return text ; }
            set { text = value ; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"{Text}" ; }

        #region Overrides of XmlDocElement
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override XmlDocumentElementCollection DocumentElementCollection { get ; set ; }
        #endregion
    }

    /// <summary>
    /// </summary>
    public sealed class Typeparamref : InlineDocElem
    {
        public Typeparamref ( ) {
        }

        private  string _typeParamName ;

        /// <summary>
        /// </summary>
        /// <param name="typeParamName"></param>
        public Typeparamref ( string typeParamName ) { _typeParamName = typeParamName ; }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ] public string TypeParamName
        {
            get { return _typeParamName ; }
            set { _typeParamName = value ; }
        }
    }

    /// <summary>
    /// </summary>
    public class Example : BlockDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Example ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        public Example ( ) {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Param : BlockDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Param ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        public Param ( ) {
        }
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
        public InlineDocElem ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public InlineDocElem ( ) { }
    }

    /// <summary>
    /// </summary>
    public class Anchor : InlineDocElem
    {
        private string _href ;

        /// <summary>
        /// </summary>
        /// <param name="href"></param>
        /// <param name="elements"></param>
        public Anchor ( string href , IEnumerable < XmlDocElement > elements ) : base ( elements )
        {
            _href = href ;
        }

        /// <summary>
        /// 
        /// </summary>
        public Anchor ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Href
        {
            get { return _href ; }
            set { _href = value ; }
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
        public Pre ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Pre ( ) {
        }
    }

    /// <summary>
    /// </summary>
    public class Em : InlineDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Em ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        public Em ( ) {
        }
    }

    /// <summary>
    /// </summary>
    public sealed class Seealso : XmlDocElement
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Seealso ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        public Seealso ( ) {
        }
    }

    /// <summary>
    /// </summary>
    public class Para : BlockDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Para ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Para ( ) {
        }
    }

    /// <summary>
    /// </summary>
    public class Paramref : InlineDocElem
    {
        private  string _paramName ;

        /// <summary>
        /// </summary>
        /// <param name="paramName"></param>
        public Paramref ( string paramName ) { _paramName = paramName ; }

        /// <summary>
        /// </summary>
        public string ParamName
        {
            get { return _paramName ; }
            set { _paramName = value ; }
        }
    }

    /// <summary>
    /// </summary>
    public class Code : InlineDocElem
    {
        public Code ( ) {
        }

        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Code ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    /// <summary>
    /// </summary>
    public class Crossref : InlineDocElem
    {
        private string _xRefId ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xRefId"></param>
        public Crossref ( string xRefId ) { _xRefId = xRefId ; }

        /// <summary>
        /// 
        /// </summary>
        public string XRefId
        {
            get { return _xRefId ; }
            set { _xRefId = value ; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class XmlDocElement
    {
        private readonly XElement               _element ;
        private XmlDocumentElementCollection _xmlDocumentElementCollection ;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public XmlDocElement ( IEnumerable < XmlDocElement > elements )
        {
            _xmlDocumentElementCollection = new XmlDocumentElementCollection(elements.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocElement ( ) { _xmlDocumentElementCollection = new XmlDocumentElementCollection(); }

        /// <summary>
        /// 
        /// </summary>
        public virtual XmlDocumentElementCollection DocumentElementCollection
        {
            get { return _xmlDocumentElementCollection ; }
            set { _xmlDocumentElementCollection = value ; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Summary : BlockDocElem
    {
        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        public Summary ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public Summary ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BlockDocElem : XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        protected BlockDocElem ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        protected BlockDocElem ( ) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UnrecognizedElementException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnrecognizedElementException ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnrecognizedElementException ( string message ) : base ( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnrecognizedElementException ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnrecognizedElementException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class MyWriter : XmlWriter
    {
        private readonly Stack < string > _elements = new Stack < string > ( ) ;
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

    /// <summary>
    /// 
    /// </summary>
    public class TypeDocInfo
    {
        private List < MethodDocInfo > constructorDocumentation = new List < MethodDocInfo > ( ) ;

        private Dictionary < string , List < MethodDocumentation > > methodDocumentation =
            new Dictionary < string , List < MethodDocumentation > > ( ) ;

        private Dictionary < string , PropertyDocumentation > propertyDocumentation =
            new Dictionary < string , PropertyDocumentation > ( ) ;

        private TypeDocumentation typeDocumentation ;

        /// <summary>
        /// 
        /// </summary>
        public TypeDocumentation TypeDocumentation
        {
            get { return typeDocumentation ; }
            set { typeDocumentation = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List < MethodDocInfo > ConstructorDocumentation
        {
            get { return constructorDocumentation ; }
            set { constructorDocumentation = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary < string , List < MethodDocumentation > > MethodDocumentation
        {
            get { return methodDocumentation ; }
            set { methodDocumentation = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary < string , PropertyDocumentation > PropertyDocumentation
        {
            get { return propertyDocumentation ; }
            set { propertyDocumentation = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary < string , FieldDocumentation > FieldDocumentation { get ; set ; } =
            new Dictionary < string , FieldDocumentation > ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class DocInfo
    {
        private string                        _docIdentifier ;
        private IEnumerable < XmlDocElement > _docNode ;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < XmlDocElement > DocNode
        {
            get { return _docNode ; }
            set { _docNode = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DocIdentifier
        {
            get { return _docIdentifier ; }
            set { _docIdentifier = value ; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MemberDocInfo : DocInfo
    {
        private string _memberName ;

        /// <summary>
        /// 
        /// </summary>
        public string MemberName { get { return _memberName ; } set { _memberName = value ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MethodDocInfo : MemberDocInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Parameters { get ; set ; }
    }

    internal class MethodBaseDocInfo
    {
    }
}