#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// XmlDocElements.cs
// 
// 2020-04-04-5:54 AM
// 
// ---
#endregion
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
#if POCO

#endif

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlDocElements
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public static XmlDocElement HandleName(string name) {
            
            XmlDocElement r = null ;
            switch (name)
            {
                case "summary":
                    r = new Summary ( ) ;
                    break;
                case "see":
                    r = new Crossref ( ) ;
                    break;
                case "paramref":
                    r = new Paramref ( ) ;
                    break;
                case "c":
                    r = new Code ( ) ;
                    break;
                case "para":
                    r = new Para ( ) ;

                    break;
                case "seealso":
                    r = new Seealso ( ) ;
                    break;
                case "em":
                    r = new Em ( ) ;
                    break;
                case "pre":
                    r = new Pre ( ) ;
                    break;
                case "a":
                    r = new Anchor ( ) ;
                    break;
                case "typeparamref":
                    r = new Typeparamref ( ) ;
                    break;
                case "param":
                    r = new Param ( ) ;
                    break;
                case "returns":
                    r = new Returns ( ) ;
                    break;

                case "example":
                    r = new Example ( ) ;
                    break;
            }

            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public static XmlDocElement HandleName ( XElement element ,  string name )
        {
            XmlDocElement r = null ;
            switch ( name )
            {
                case "summary" :
                    r = new Summary ( element.Nodes ( ).Select ( Selector ) ) ;
                    break ;
                case "see" :
                    r = new Crossref ( element.Attribute ( XName.Get ( "cref" , "" ) )?.Value ?? "" ) ;
                    break ;
                case "paramref" :
                    r = new Paramref ( element.Attribute ( XName.Get ( "name" , "" ) )?.Value ?? "" ) ;
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
                                    element.Attribute ( XName.Get ( "href" , "" ) )?.Value ?? ""
                                  , element.Nodes ( ).Select ( Selector )
                                   ) ;
                    break ;
                case "typeparamref" :
                    r = new Typeparamref ( element.Attribute ( XName.Get ( "name" , "" ) )?.Value ?? "" ) ;
                    break ;
                case "param" :
                    r = new Param (
                                   element.Attribute ( XName.Get ( "name" , "" ) )?.Value ?? ""
                                 , element.Nodes ( ).Select ( Selector )
                                  ) ;
                    break ;
                case "returns" :
                    r = new Returns ( element.Nodes ( ).Select ( Selector ) ) ;
                    break ;

                case "example":
                    r = new Example(element.Nodes().Select(Selector));
                    break;
                case "quoteInline":
                    r = new Quoteinline(element.Nodes().Select(Selector));
                    break;
                default:
                    return null ;
                    throw new UnrecognizedElementException(name);
            }

            return r ;
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
                case XCData xcData : return new XmlDocText ( xcData.Value ) ;
                case XElement element :
                {
                    var r = XmlDocElements.HandleName ( element , element.Name.LocalName ) ;

                    return r ;
                }
                case XText xText :
                    return new XmlDocText ( xText.Value ) ;
            }

            throw new UnrecognizedElementException ( node.GetType ( ).FullName ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xdoc"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [ CanBeNull ]
        public static XmlDocElement HandleXDocument ( [ NotNull ] XDocument xdoc )
        {
            if ( xdoc.Root != null )
            {
                return HandleName ( xdoc.Root , xdoc.Root.Name.LocalName ) ;
            }

            throw new InvalidOperationException ( ) ;
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
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable DocMembers ( [ NotNull ] XDocument doc )
        {
            if ( doc == null )
            {
                throw new ArgumentNullException ( nameof ( doc ) ) ;
            }

            if ( doc.Root != null )
            {
                return doc.Root.Elements ( "member" ) ;
            }

            throw new InvalidOperationException ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDocument"></param>
        /// <param name="elementId"></param>
        /// <param name="member"></param>
        /// <param name="declared"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [CanBeNull]
        public static CodeElementDocumentation HandleDocElementNode (
            [ NotNull ] XDocument   xDocument
          , string                  elementId
          , [ NotNull ] MemberDeclarationSyntax member
          , ISymbol                declared
        )
        {
            var xmlElement = xDocument?.Root ;

            if (elementId == null)
            {
                elementId = xmlElement?.Attribute(XName.Get("name"))?.Value;
            }

            var xNodes = xmlElement?.Nodes() ?? Enumerable.Empty<XNode> (  );

            var xmlDoc = (xNodes).Select(XmlDocElements.Selector);
            CodeElementDocumentation docElem = null ;
            var type = declared.ContainingType ;
            
            Dictionary < SyntaxKind , Type > kindType = new Dictionary < SyntaxKind , Type >
                                                        {
                                                            [ SyntaxKind.ClassDeclaration ] =
                                                                typeof ( TypeDocumentation ),
                                                            [SyntaxKind.InterfaceDeclaration] = typeof(TypeDocumentation),
                                                            [SyntaxKind.StructDeclaration] = typeof(TypeDocumentation),
                                                            [SyntaxKind.DelegateDeclaration] = typeof(DelegateDocumentation),
                                                            
                                                            [SyntaxKind.EnumDeclaration] =  typeof(TypeDocumentation),
                                                            
                                                            [SyntaxKind.EnumMemberDeclaration]  = typeof(EnumMemberDocumentation),
                                                            
                                                            [SyntaxKind.FieldDeclaration] = typeof(FieldDocumentation),
                                                            
                                                            [SyntaxKind.MethodDeclaration] = typeof(MethodDocumentation),

                [SyntaxKind.ConstructorDeclaration] = typeof(ConstructorDocumentation),
                                                            
                                                            [SyntaxKind.DestructorDeclaration] = typeof(CodeElementDocumentation),
                                                            
                                                            [SyntaxKind.PropertyDeclaration] = typeof(PropertyDocumentation),

                [SyntaxKind.IndexerDeclaration] = typeof(CodeElementDocumentation),

                [SyntaxKind.EventDeclaration] = typeof(CodeElementDocumentation),

                [SyntaxKind.EventFieldDeclaration] = typeof(CodeElementDocumentation),

                [SyntaxKind.OperatorDeclaration] = typeof(CodeElementDocumentation),

                [SyntaxKind.ConversionOperatorDeclaration] = typeof(CodeElementDocumentation),


                                                        };
            switch ( member )
            {
                case EventFieldDeclarationSyntax eventFieldDeclarationSyntax : break ;
                case FieldDeclarationSyntax fieldDeclarationSyntax : break ;
                case BaseFieldDeclarationSyntax baseFieldDeclarationSyntax : break ;
                case ConstructorDeclarationSyntax constructorDeclarationSyntax :
                    break ;
                case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax : break ;
                case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
                case MethodDeclarationSyntax methodDeclarationSyntax : break ;
                case OperatorDeclarationSyntax operatorDeclarationSyntax : break ;
                case BaseMethodDeclarationSyntax baseMethodDeclarationSyntax : break ;
                case EventDeclarationSyntax eventDeclarationSyntax : break ;
                case IndexerDeclarationSyntax indexerDeclarationSyntax : break ;
                case PropertyDeclarationSyntax propertyDeclarationSyntax : break ;
                case BasePropertyDeclarationSyntax basePropertyDeclarationSyntax : break ;
                case ClassDeclarationSyntax classDeclarationSyntax : break ;
                case EnumDeclarationSyntax enumDeclarationSyntax : break ;
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax : break ;
                case StructDeclarationSyntax structDeclarationSyntax : break ;
                case TypeDeclarationSyntax typeDeclarationSyntax : break ;
                case BaseTypeDeclarationSyntax baseTypeDeclarationSyntax : break ;
                case DelegateDeclarationSyntax delegateDeclarationSyntax : break ;
                case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax : break ;
                case GlobalStatementSyntax globalStatementSyntax : break ;
                case IncompleteMemberSyntax incompleteMemberSyntax : break ;
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax : break ;
                default : throw new ArgumentOutOfRangeException ( nameof ( member ) ) ;
            }

            if ( kindType.TryGetValue ( member.Kind ( ) , out var kind1 ) )
            {
                docElem = ( CodeElementDocumentation ) Activator.CreateInstance ( kind1 ) ;
            }

#if false
            docElem.PocoMemberDelaration = GenTransforms.Transform_Member_Declaration ( member ) ;
            switch (docElem.PocoMemberDelaration)
            {
                case PocoBaseMethodDeclarationSyntax methodB:
                    methodB.Body = null;
                    break;
            }

            switch (docElem.PocoMemberDelaration)
            {
                case PocoEventFieldDeclarationSyntax pocoEventFieldDeclarationSyntax :
                    break ;
                case PocoFieldDeclarationSyntax pocoFieldDeclarationSyntax : break ;
                case PocoBaseFieldDeclarationSyntax pocoBaseFieldDeclarationSyntax : break ;
                case PocoConstructorDeclarationSyntax pocoConstructorDeclarationSyntax :
                    pocoConstructorDeclarationSyntax.Initializer = null ;
                    break ;
                case PocoMethodDeclarationSyntax pocoMethodDeclarationSyntax :
                    break ;
                case PocoOperatorDeclarationSyntax pocoOperatorDeclarationSyntax : break ;
                case PocoBaseMethodDeclarationSyntax pocoBaseMethodDeclarationSyntax : break ;
                case PocoEventDeclarationSyntax pocoEventDeclarationSyntax : break ;
                case PocoIndexerDeclarationSyntax pocoIndexerDeclarationSyntax : break ;
                case PocoPropertyDeclarationSyntax pocoPropertyDeclarationSyntax : break ;
                case PocoBasePropertyDeclarationSyntax pocoBasePropertyDeclarationSyntax : break ;
                case PocoClassDeclarationSyntax pocoClassDeclarationSyntax :
                    pocoClassDeclarationSyntax.Members.Clear();
                    break ;
                case PocoEnumDeclarationSyntax pocoEnumDeclarationSyntax :
                    pocoEnumDeclarationSyntax.Members.Clear ( ) ;
                    break ;
                case PocoInterfaceDeclarationSyntax pocoInterfaceDeclarationSyntax : break ;
                case PocoStructDeclarationSyntax pocoStructDeclarationSyntax :
                    pocoStructDeclarationSyntax.Members.Clear();
                    break ;
                case PocoTypeDeclarationSyntax pocoTypeDeclarationSyntax :
                    pocoTypeDeclarationSyntax.Members.Clear();
                    break ;
                case PocoBaseTypeDeclarationSyntax pocoBaseTypeDeclarationSyntax : break ;
                case PocoDelegateDeclarationSyntax pocoDelegateDeclarationSyntax : break ;
                case PocoEnumMemberDeclarationSyntax pocoEnumMemberDeclarationSyntax : break ;
                case PocoGlobalStatementSyntax pocoGlobalStatementSyntax : break ;
                case PocoIncompleteMemberSyntax pocoIncompleteMemberSyntax : break ;
                case PocoNamespaceDeclarationSyntax pocoNamespaceDeclarationSyntax :
                    pocoNamespaceDeclarationSyntax.Members.Clear();
                    break ;
                default : throw new ArgumentOutOfRangeException ( ) ;
            }
#endif
            if ( docElem != null )
            {
                docElem.ElementId = elementId ;

                foreach ( var xmlDocElement in xmlDoc )
                {
                    docElem.XmlDoc.Add ( xmlDocElement ) ;
                }

                return docElem ;
            }

            throw new InvalidOperationException ( ) ;
        }
    }

    /// <summary>
    /// Quote inline block XML documentation element.
    /// </summary>
    public class Quoteinline : BlockDocElem
    {
        /// <summary>
        /// 
        /// </summary>
        public Quoteinline ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="select"></param>
        public Quoteinline ( IEnumerable < XmlDocElement > select ) : base(select)
        {

        }
    }
}