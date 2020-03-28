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
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.VisualBasic.Syntax ;
using NLog ;

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

        private XmlDocument _docuDoc ;
        private Dictionary < string , TypeDocInfo > _docDict ;

        public TypesViewModel ( )
        {
            //var path = Path.ChangeExtension ( typeof ( CSharpSyntaxNode ).Assembly.Location , ".xml" ) ;
            var xml =
                @"c:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisAppLib\doc.xml" ;

            _docDict = new Dictionary < string , TypeDocInfo > ( ) ;
            _docuDoc = new XmlDocument ( ) ;
            _docuDoc.Load ( xml ) ;
            foreach ( var xmlElement in _docuDoc
                                       .DocumentElement.ChildNodes.OfType < XmlElement > ( )
                                       .Where ( child => child.Name == "members" )
                                       .First ( )
                                       .ChildNodes.OfType < XmlElement > ( ) )
            {
                var name = xmlElement.GetAttribute ( "name" ) ;
                xmlElement.WriteTo(new MyWriter());
                XDocument doc = XDocument.Parse ( xmlElement.OuterXml ) ;
                if ( doc.Element("member").Elements ( "summary" )
                        .Elements ( )
                        .Select (
                                 element => {
                                     object r = null ;
                                     switch ( element.Name.LocalName )
                                     {
                                         // case "see" :
                                         // r = new Crossref ( ) ;
                                         // break ;
                                         default :
                                             throw new UnrecognizedElementException (
                                                                                     element
                                                                                        .Name
                                                                                        .LocalName
                                                                                    ) ;

                                     }

                                     return r ;
                                 }
                                )
                        .Any ( o => o == null ) )
                {
                    throw new UnrecognizedElementException(name);
                }
                
                var kind = name[ 0 ] ;
                var type = name.Substring ( 2 ) ;
                string parameters = null ;
                if ( type.Contains ( '(' ) )
                {
                    var leftParen = type.IndexOf ( '(' ) ;
                    var rightParen = type.LastIndexOf ( ')' ) ;

                    parameters = type.Substring ( leftParen , rightParen - leftParen ) ;
                    type       = type.Substring ( 0 ,         leftParen ) ;
                }

                string method = null ;
                if ( kind == 'M' )
                {
                    method = type.Substring ( type.LastIndexOf ( '.' ) ) ;

                    type = type.Substring ( 0 , type.LastIndexOf ( '.' ) ) ;
                }

                if ( ! _docDict.TryGetValue ( type , out var typeDict ) )
                {
                    typeDict        = new TypeDocInfo ( ) ;
                    _docDict[ type ] = typeDict ;
                }

                if ( kind == 'T' )
                {
                    typeDict.TypeDocumentation = xmlElement ;
                }
                else if ( kind == 'M' )
                {
                    if ( ! typeDict.MethodDocumentation.TryGetValue (
                                                                     method
                                                                   , out var methodDocInfo
                                                                    ) )
                    {
                        methodDocInfo                          = new List < MethodDocInfo > ( ) ;
                        typeDict.MethodDocumentation[ method ] = methodDocInfo ;
                    }

                    methodDocInfo.Add (
                                       new MethodDocInfo ( )
                                       {
                                           MemberName    = method
                                         , Parameters    = parameters
                                         , DocIdentifier = name
                                         , DocNode       = xmlElement
                                       }
                                      ) ;
                }


                Debug.WriteLine ( type ) ;
            }

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

        private AppTypeInfo CollectTypeInfos (
            AppTypeInfo parentTypeInfo
          , Type        rootR
          , int         level = 0
        )
        {
            // _docuDoc.SelectSingleNode (
            // "//member[starts-with(@name, '" + path + "')]/summary"
            // ) ;


            XmlElement docNode = null ;
            if ( _docDict.TryGetValue ( rootR.FullName , out var info ) )
            {
                docNode = ( XmlElement ) info.TypeDocumentation ;
            }
            var r = new AppTypeInfo ( )
                    {
                        Type           = rootR,
                        DocInfo = docNode
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

    public class Crossref
    {
    }

    public class UnrecognizedElementException : Exception
    {
        public UnrecognizedElementException ( ) {
        }

        public UnrecognizedElementException ( string message ) : base ( message )
        {
        }

        public UnrecognizedElementException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected UnrecognizedElementException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }

    public class MyWriter : XmlWriter
    {
        private Stack<string> _elements = new Stack < string > ();
        #region Overrides of XmlWriter
        public override void WriteStartDocument ( ) { }

        public override void WriteStartDocument ( bool standalone ) { }

        public override void WriteEndDocument ( ) { }

        public override void WriteDocType ( string name , string pubid , string sysid , string subset ) { }

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

        public override void WriteStartAttribute ( string prefix , string localName , string ns ) { }

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
        private object                 typeDocumentation ;
        private List < MethodDocInfo > constructorDocumentation = new List < MethodDocInfo > ( ) ;

        private Dictionary < string , List < MethodDocInfo > > methodDocumentation =
            new Dictionary < string , List < MethodDocInfo > > ( ) ;

        private Dictionary < string , List < MemberDocInfo > > propertyDocumentation =
            new Dictionary < string , List < MemberDocInfo > > ( ) ;

        public object TypeDocumentation
        {
            get { return typeDocumentation ; }
            set { typeDocumentation = value ; }
        }

        public List < MethodDocInfo > ConstructorDocumentation
        {
            get { return constructorDocumentation ; }
            set { constructorDocumentation = value ; }
        }

        public Dictionary < string , List < MethodDocInfo > > MethodDocumentation
        {
            get { return methodDocumentation ; }
            set { methodDocumentation = value ; }
        }

        public Dictionary < string , List < MemberDocInfo > > PropertyDocumentation
        {
            get { return propertyDocumentation ; }
            set { propertyDocumentation = value ; }
        }
    }

    public class DocInfo
    {
        private XmlElement _docNode ;
        private string     _docIdentifier ;
        public  XmlElement DocNode { get { return _docNode ; } set { _docNode = value ; } }

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