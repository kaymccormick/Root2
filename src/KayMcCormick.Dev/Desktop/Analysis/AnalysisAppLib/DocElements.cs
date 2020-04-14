using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Windows.Markup ;
using JetBrains.Annotations ;
#if POCO
using PocoSyntax ;
#endif

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// </summary>
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
          , [ NotNull ] Type              type
          , [ NotNull ] string                        member
          , string                        parameters
          , IEnumerable < XmlDocElement > xmlDoc = null
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
}

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    ///
    public sealed class Returns : BlockDocElem
    {
        /// <inheritdoc />
        public Returns ( IEnumerable < XmlDocElement > select ) : base ( select ) { }

        /// <inheritdoc />
        public Returns ( ) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public class MemberBaseDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        protected string _memberName ;

        /// <summary>
        /// 
        /// </summary>
        protected MemberBaseDocumentation ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDocElements"></param>
        protected MemberBaseDocumentation (
            string                        elementId
          , [ CanBeNull ] Type            type
          , [ NotNull ]   string          memberName
          , IEnumerable < XmlDocElement > xmlDocElements = null
        ) : base ( elementId , xmlDocElements )
        {
            Type       = type ;
            MemberName = memberName ?? throw new ArgumentNullException ( nameof ( memberName ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        protected MemberBaseDocumentation (
            string                                      elementId
          , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null
        ) : base ( elementId , xmlDoc )
        {
        }

        /// <summary>
        /// </summary>
        public string MemberName { [ UsedImplicitly ] get { return _memberName ; } set { _memberName = value ; } }
    }

    /// <summary>
    /// </summary>
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
          , [ NotNull ] Type              type
          , [ NotNull ] string            memberName
          , IEnumerable < XmlDocElement > xmlDocElements
        ) : base ( elementId , type , memberName , xmlDocElements )
        {
        }
    }

    /// <summary>
    /// </summary>
    public sealed class PropertyDocumentation : MemberBaseDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="xmlDoc"></param>
        public PropertyDocumentation (
            string                        elementId
          , [ NotNull ] Type              type
          , [ NotNull ] string            memberName
          , IEnumerable < XmlDocElement > xmlDoc = null
        ) : base ( elementId , type , memberName , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        public PropertyDocumentation ( ) { }
    }

    /// <summary>
    /// </summary>
    public sealed class TypeDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="type"></param>
        /// <param name="xmlDoc"></param>
        public TypeDocumentation (
            string                        elementId
          , Type                          type
          , IEnumerable < XmlDocElement > xmlDoc = null
        ) : base ( elementId , xmlDoc )
        {
            Type = type ;
        }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
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
        protected readonly XmlDocumentElementCollection _xmlDoc ;

        private bool _needsAttention ;
#if POCO
        private PocoMemberDeclarationSyntax _poco ;
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        protected CodeElementDocumentation (
            string                                      elementId
          , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null
        )
        {
            _xmlDoc = xmlDoc != null ? new XmlDocumentElementCollection ( xmlDoc.ToList ( ) ) : new XmlDocumentElementCollection ( ) ;

            ElementId = elementId ;
        }

        /// <summary>
        /// 
        /// </summary>
        protected CodeElementDocumentation ( ) { _xmlDoc = new XmlDocumentElementCollection ( ) ; }

        
        /// <summary>
        /// </summary>
        public string ElementId { get { return _elementId ; } set { _elementId = value ; } }

        /// <summary>
        /// </summary>
        public Type Type { get { return _type ; } set { _type = value ; } }

        /// <summary>
        /// 
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ) ]
        public XmlDocumentElementCollection XmlDoc
        {
            get { return _xmlDoc ; }
            // set { _xmlDoc = value; }
        }

        /// <summary>
        /// Flag indicating that this documentation element needs attention.
        /// </summary>
        public bool NeedsAttention { get { return _needsAttention ; } set { _needsAttention = value ; } }

#if POCO
        /// <summary>
        /// 
        /// </summary>
        public PocoMemberDeclarationSyntax PocoMemberDelaration { get { return _poco ; } set { _poco = value ; } }
#endif
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
    public class IndexerDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        public IndexerDocumentation ( string elementId , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null ) : base ( elementId , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public IndexerDocumentation ( ) {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EventDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        public EventDocumentation ( string elementId , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null ) : base ( elementId , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public EventDocumentation ( ) {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnumMemberDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        public EnumMemberDocumentation ( string elementId , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null ) : base ( elementId , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public EnumMemberDocumentation ( ) {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DelegateDocumentation : CodeElementDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        public DelegateDocumentation ( string elementId , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null ) : base ( elementId , xmlDoc )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateDocumentation ( ) {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConstructorDocumentation : MemberBaseDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        public ConstructorDocumentation ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="xmlDoc"></param>
        public ConstructorDocumentation (
            string                                      elementId
          , [ CanBeNull ] IEnumerable < XmlDocElement > xmlDoc = null
        ) : base ( elementId , xmlDoc )
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class XmlDocumentElementCollection : IList
      , IEnumerable
      , ICollection
      , IList < XmlDocElement >
      , ICollection < XmlDocElement >
      , IEnumerable < XmlDocElement >
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listImplementation"></param>
        public XmlDocumentElementCollection ( List < XmlDocElement > listImplementation )
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

        private readonly List < XmlDocElement > _listImplementation ;
#region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator < XmlDocElement > GetEnumerator ( )
        {
            return _listImplementation.GetEnumerator ( ) ;
        }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _listImplementation ).GetEnumerator ( ) ;
        }
#endregion
#region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index )
        {
            ( ( ICollection ) _listImplementation ).CopyTo ( array , index ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove ( XmlDocElement item ) { return _listImplementation.Remove ( item ) ; }

        int ICollection < XmlDocElement >.Count { get { return _listImplementation.Count ; } }

        bool ICollection < XmlDocElement >.IsReadOnly { get { return false ; } }

        int ICollection.Count { get { return _listImplementation.Count ; } }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot { get { return ( ( ICollection ) _listImplementation ).SyncRoot ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return ( ( ICollection ) _listImplementation ).IsSynchronized ; }
        }
#endregion
#region Implementation of IList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value )
        {
            if ( value is XmlDocumentElementCollection e )
            {
                foreach ( var xmlDocElement in e )
                {
                    _listImplementation.Add ( xmlDocElement ) ;
                }

                return 0 ;
            }

            return ( ( IList ) _listImplementation ).Add ( value ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value )
        {
            return ( ( IList ) _listImplementation ).Contains ( value ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add ( XmlDocElement item ) { _listImplementation.Add ( item ) ; }

        void ICollection < XmlDocElement >.Clear ( ) { _listImplementation.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains ( XmlDocElement item )
        {
            return _listImplementation.Contains ( item ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo ( XmlDocElement[] array , int arrayIndex )
        {
            _listImplementation.CopyTo ( array , arrayIndex ) ;
        }

        void IList.Clear ( ) { _listImplementation.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value )
        {
            return ( ( IList ) _listImplementation ).IndexOf ( value ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value )
        {
            ( ( IList ) _listImplementation ).Insert ( index , value ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { ( ( IList ) _listImplementation ).Remove ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf ( XmlDocElement item ) { return _listImplementation.IndexOf ( item ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert ( int index , XmlDocElement item )
        {
            _listImplementation.Insert ( index , item ) ;
        }

        void IList < XmlDocElement >.RemoveAt ( int index )
        {
            _listImplementation.RemoveAt ( index ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
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

        bool IList.IsReadOnly { get { return ( ( IList ) _listImplementation ).IsReadOnly ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize { get { return ( ( IList ) _listImplementation ).IsFixedSize ; } }
#endregion
    }

    /// <summary>
    /// </summary>
    [ ContentProperty ( "Text" ) ]
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
        /// 
        /// </summary>
        public XmlDocText ( ) { }

        /// <summary>
        /// </summary>
        public string Text { get { return text ; } set { text = value ; } }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"{Text}" ; }

#region Overrides of XmlDocElement
        /// <summary>
        /// 
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public override XmlDocumentElementCollection DocumentElementCollection { get ; }
#endregion
    }

    /// <summary>
    /// </summary>
    public sealed class Typeparamref : InlineDocElem
    {
        /// <summary>
        /// 
        /// </summary>
        public Typeparamref ( ) { }

        private string _typeParamName ;

        /// <summary>
        /// </summary>
        /// <param name="typeParamName"></param>
        public Typeparamref ( string typeParamName ) { _typeParamName = typeParamName ; }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        public string TypeParamName
        {
            get { return _typeParamName ; }
            set { _typeParamName = value ; }
        }
    }

    /// <summary>
    /// According to Sandcastle this is neither block nor inline.
    /// </summary>
    public sealed class Example : XmlDocElement
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Example ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Example ( ) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Param : BlockDocElem
    {
        private string _name ;

        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Param ( string name , IEnumerable < XmlDocElement > elements ) : base ( elements )
        {
            Name = name ;
        }

        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        public Param ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name ; } set { _name = value ; } }
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
        public Anchor ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public string Href { get { return _href ; } set { _href = value ; } }
    }

    /// <summary>
    /// </summary>
    public sealed class Pre : XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        public Pre ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Pre ( ) { }
    }

    /// <summary>
    /// </summary>
    public sealed class Em : InlineDocElem
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Em ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Em ( ) { }
    }

    /// <summary>
    /// </summary>
    public sealed class Seealso : XmlDocElement
    {
        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Seealso ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Seealso ( ) { }
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
        /// <param name="elements"></param>
        public Para ( params XmlDocElement[] elements ) : base ( elements ) { }

        /// <summary>
        /// 
        /// </summary>
        public Para ( ) { }
    }

    /// <summary>
    /// </summary>
    public sealed class Paramref : InlineDocElem
    {
        private string _paramName ;

        /// <summary>
        /// </summary>
        /// <param name="paramName"></param>
        public Paramref ( string paramName ) { _paramName = paramName ; }

        /// <summary>
        /// 
        /// </summary>
        public Paramref ( ) { }

        /// <summary>
        /// </summary>
        public string ParamName { get { return _paramName ; } set { _paramName = value ; } }
    }

    /// <summary>
    /// </summary>
    public class Code : InlineDocElem
    {
        /// <summary>
        /// 
        /// </summary>
        public Code ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="elements"></param>
        public Code ( IEnumerable < XmlDocElement > elements ) : base ( elements ) { }
    }

    /// <summary>
    /// </summary>
    public sealed class Crossref : InlineDocElem
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
        public Crossref ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public string XRefId { get { return _xRefId ; } set { _xRefId = value ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    [ ContentProperty ( "DocumentElementCollection" ) ]
    [ ContentWrapper ( typeof ( XmlDocElement ) ) ]
    public class XmlDocElement
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly XmlDocumentElementCollection _xmlDocumentElementCollection ;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        protected XmlDocElement ( [ NotNull ] IEnumerable < XmlDocElement > elements )
        {
            _xmlDocumentElementCollection =
                new XmlDocumentElementCollection ( elements.ToList ( ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocElement ( )
        {
            _xmlDocumentElementCollection = new XmlDocumentElementCollection ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ) ]
        public virtual XmlDocumentElementCollection DocumentElementCollection
        {
            get { return _xmlDocumentElementCollection ; }
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
        public Summary ( ) { }


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