using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics.CodeAnalysis ;
using System.Linq ;
using System.Reflection ;
using System.Xml ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisAppLib
{
    /// <summary>
    ///     Class to write type information to XML file.
    /// </summary>
    [ SuppressMessage ( "ReSharper" , "UnusedMember.Global" ) ]
    public static class TypeWriter
    {
        private const string TypesElementName = "types" ;

        private const string TypeElementName                    = "type" ;
        private const string TypeElementFullNameAttributeName   = "FullName" ;
        private const string IdAttributeName                    = "id" ;
        private const string PropertyElementPropertyTypeAttName = "PropertyType" ;
        private const string TypesFilename                      = "types.xml" ;
        private const string PropertyElementName                = "Property" ;
        private const string PropertyElementPropertyNameAttName = "Name" ;

        /// <summary>
        ///     Method namespace URI
        /// </summary>
        public const string MethodNamespaceUri = "http://kaymccormick.com/ns/method" ;

        /// <summary>
        ///     Property namespace URI
        /// </summary>
        public const string PropertyNamespaceUri = "http://kaymccormick.com/ns/property" ;

        private const string ParametersElementName                = "Parameters" ;
        private const string ReturnTypeElementName                = "ReturnType" ;
        private const string MethodElementName                    = "Method" ;
        private const string ParameterElementName                 = "Parameter" ;
        private const string MethodElementMethodNameAttName       = "Name" ;
        private const string ParameterElementParameterNameAttName = "Name" ;
        private const string ParameterElementParameterTypeAttName = "Type" ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator ( ) ;

        /// <summary>
        ///     Enable proxy
        /// </summary>
        public static bool Proxy { get ; } = false ;

        /// <summary>
        ///     Get XmlElement for Method
        /// </summary>
        /// <param name="c"></param>
        /// <param name="c2"></param>
        /// <param name="m"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static XmlElement MethodXmlElement (
            [ NotNull ] Func < string , XmlElement >          c
          , [ NotNull ] Func < string , string , XmlElement > c2
          , [ NotNull ] MethodInfo                            m
          , WriteStyle                                        style
        )
        {
            XmlElement elem ;
            switch ( style )
            {
                case WriteStyle.Compact :
                    // if ( ! nsManager.HasNamespace ( "method" ) )
                    // {
                    //     throw new ArgumentException ( ) ;
                    // }

                    elem = c2 ( "method:" + m.Name , MethodNamespaceUri ) ;
                    break ;
                case WriteStyle.Verbose :
                    elem = c ( MethodElementName ) ;
                    elem.SetAttribute ( MethodElementMethodNameAttName , m.Name ) ;
                    break ;
                default : throw new InvalidEnumArgumentException ( ) ;
            }

            if ( m.ReturnType != typeof ( void ) )
            {
                elem.SetAttribute ( ReturnTypeElementName , m.ReturnType.FullName ) ;
            }

            if ( ! m.GetParameters ( ).Any ( ) )
            {
                return elem ;
            }

            var p = c ( ParametersElementName ) ;
            foreach ( var p2 in m.GetParameters ( ) )
            {
                var p3 = c ( ParameterElementName ) ;
                p3.SetAttribute ( ParameterElementParameterNameAttName , p2.Name ) ;
                p3.SetAttribute (
                                 ParameterElementParameterTypeAttName
                               , p2.ParameterType.FullName
                                ) ;
                p.AppendChild ( p3 ) ;
            }

            elem.AppendChild ( p ) ;

            return elem ;
        }

        /// <summary>
        ///     Main routine to "discover" types and write them out to XML.
        /// </summary>
        /// <param name="style"></param>
        public static void DiscoverTypes ( WriteStyle style )
        {
            // ReSharper disable once UnusedVariable
            var doc = CreateTypesDocument ( out var xmlNameTable , out _ ) ;
            Func < string , XmlElement > c = doc.CreateElement ;
            Func < string , string , XmlElement > c2 = doc.CreateElement ;

            var types = doc.DocumentElement ;
            Logger.Debug ( doc.OuterXml ) ;
            var typeStack = new Stack < Type > ( ) ;
            typeStack.Push ( typeof ( SyntaxNode ) ) ;
            var seen = new HashSet < Type > ( ) ;

            while ( typeStack.Any ( ) )
            {
                var type = typeStack.Pop ( ) ;
                seen.Add ( type ) ;
                var typElement = c ( TypeElementName ) ;
                typElement.SetAttribute ( IdAttributeName ,                  GetXmlId ( type ) ) ;
                typElement.SetAttribute ( TypeElementFullNameAttributeName , type.FullName ) ;
                types?.AppendChild ( typElement ) ;

                foreach ( var member in type.GetMembers ( ) )
                {
                    AddMember ( typElement , member ) ;
                }

                foreach ( var p in type.GetProperties ( ) )
                {
                    var propElem = c ( PropertyElementName ) ;
                    propElem.SetAttribute ( PropertyElementPropertyNameAttName , p.Name ) ;
                    propElem.SetAttribute (
                                           PropertyElementPropertyTypeAttName
                                         , p.PropertyType.FullName
                                          ) ;

                    if ( ! seen.Contains ( p.PropertyType ) )
                    {
                        typeStack.Push ( p.PropertyType ) ;
                    }

                    typElement.AppendChild ( propElem ) ;
                }

                foreach ( var m in type.GetMethods ( )
                                       .Where ( ( info , i ) => ! info.IsSpecialName ) )
                {
                    var elem = MethodXmlElement ( c , c2 , m , style ) ;
                    typElement.AppendChild ( elem ) ;
                }
            }

            WriteDocument ( doc , TypesFilename ) ;
        }

        private static void AddMember ( XmlNode typElement , MemberInfo member )
        {
            if ( member is MethodInfo mi
                 && mi.IsSpecialName )
            {
                return ;
            }

            if ( typElement.OwnerDocument != null )
            {
                var elem = typElement.OwnerDocument.CreateElement ( "Member" ) ;
                elem.SetAttribute ( "Name" ,       member.Name ) ;
                elem.SetAttribute ( "MemberType" , member.MemberType.ToString ( ) ) ;
                if ( member.DeclaringType != null )
                {
                    elem.SetAttribute ( "DeclaringType" , member.DeclaringType.FullName ) ;
                }

                typElement.AppendChild ( elem ) ;
            }
        }

        /// <summary>
        ///     Get the document ID for an code elmement.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetXmlId ( object o )
        {
            switch ( o )
            {
                case Type t : return "T:" + t.FullName ;
                case MethodInfo m :
                    return "M:"
                           + ( m.ReflectedType != null
                               && ! string.IsNullOrEmpty ( m.ReflectedType.Namespace )
                                   ? m.ReflectedType.Namespace + "."
                                   : "" )
                           + m.ReflectedType?.Name
                           + "."
                           + m.Name
                           + "("
                           + string.Join (
                                          ","
                                        , m.GetParameters ( )
                                           .Select ( info => SubIdForType ( info.ParameterType ) )
                                         )
                           + ")" ;
                default : throw new ArgumentException ( ) ;
            }
        }

        /// <summary>
        ///     Get a subordinate ID for a generic type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string SubIdForGenericType ( Type t )
        {
            if ( ! t.IsGenericType )
            {
                throw new ArgumentException ( ) ;
            }

            var g = t.GetGenericTypeDefinition ( ) ;
            if ( g.FullName != null )
            {
                var s = g.FullName.Substring ( 0 , g.FullName.LastIndexOf ( '`' ) ) ;
                return s
                       + "{"
                       + string.Join ( "," , t.GenericTypeArguments.Select ( SubIdForType ) )
                       + "}" ;
            }

            return string.Empty ;
        }

        private static string SubIdForType ( Type arg )
        {
            if ( arg.IsGenericType )
            {
                return SubIdForGenericType ( arg ) ;
            }

            return SubIdforNonGeneric ( arg ) ;
        }

        private static string SubIdforNonGeneric ( Type type ) { return type.FullName ; }

        private static void WriteDocument ( XmlNode doc , string outFile )
        {
            var xmlWriterSettings = new XmlWriterSettings { Indent = true } ;
            using ( var xmlWriter = XmlWriter.Create ( outFile , xmlWriterSettings ) )
            {
                var writeTo = xmlWriter ;
                // if ( Proxy )
                // {
                //     var proxy =
                //         ProxyGenerator.CreateClassProxyWithTarget (
                //                                                    xmlWriter
                //                                                  , new MyInterceptor ( )
                //                                                   ) ;
                //     writeTo = proxy ;
                // }

                try
                {
                    doc.WriteTo ( writeTo ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , ex.Message ) ;
                    throw ;
                }
            }
        }

        private static XmlDocument CreateTypesDocument (
            out NameTable           xmlNameTable
          , out XmlNamespaceManager nsManager
        )
        {
            var d1 = new XmlDocument ( ) ;
            d1.LoadXml (
                        $"<{TypesElementName} xmlns:property=\"{PropertyNamespaceUri}\" xmlns:method=\"{MethodNamespaceUri}\"/>"
                       ) ;
            xmlNameTable = null ;
            nsManager    = null ;
            return d1 ;
            /*xmlNameTable = new NameTable ( ) ;
            xmlNameTable.Add ( MethodNamespaceUri ) ;

            // nsManager    = new XmlNamespaceManager ( xmlNameTable ) ;
            var doc = new XmlDocument ( xmlNameTable ) ;
            nsManager.AddNamespace ( string.Empty , "types" ) ;
            nsManager.AddNamespace ( "method" ,     MethodNamespaceUri ) ;
            var types = doc.CreateElement ( TypesElementName ) ;
            doc.AppendChild ( types ) ;
            if ( false )
            {
                foreach ( var keyValuePair in nsManager.GetNamespacesInScope ( XmlNamespaceScope.All ) )
                {
                    if ( string.IsNullOrEmpty ( keyValuePair.Key ) )
                    {
                    }
                    else
                    {
                        if ( keyValuePair.Value == "http://www.w3.org/XML/1998/namespace" )
                        {
                            continue ;
                        }

                        Logger.Debug (
                                      "{prefix} {localName} {namespaceUri}"
                                    , "xmlns"
                                    , keyValuePair.Key
                                    , keyValuePair.Value
                                     ) ;
                        var a = doc.CreateAttribute (
                                                     "xmlns"
                                                   , keyValuePair.Key
                                                   , keyValuePair.Value
                                                    ) ;
                        types.Attributes.Append ( a ) ;
                    }

                    // types.SetAttribute ( keyValuePair.Key , null , keyValuePair.Value ) ;
                }
            }

            var x = doc.OuterXml ;
            Logger.Debug ( x ) ;
            return doc ;
            */
        }
    }

    public enum WriteStyle
    {
        /// <summary>
        ///     Write verbose XML.
        /// </summary>
        Verbose = 0

       ,

        /// <summary>
        ///     Write compact XML.
        /// </summary>
        Compact = 1
    }
}