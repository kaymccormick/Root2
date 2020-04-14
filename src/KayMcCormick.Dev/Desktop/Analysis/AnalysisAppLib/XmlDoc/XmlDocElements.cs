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
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Xml ;
using System.Xml.Linq ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
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
        private static readonly string _pocoPrefix = "Poco";
        private static readonly string _collectionSuffix = "Collection" ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ CanBeNull ]
        // ReSharper disable once UnusedMember.Global
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
            XmlDocElement r ;
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
        // ReSharper disable once UnusedMember.Global
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
        [ NotNull ]
        // ReSharper disable once UnusedMember.Global
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
            var xmlElement = xDocument.Root ;

            if (elementId == null)
            {
                elementId = xmlElement?.Attribute(XName.Get("name"))?.Value;
            }

            var xNodes = xmlElement?.Nodes() ?? Enumerable.Empty<XNode> (  );

            var xmlDoc = (xNodes).Select(XmlDocElements.Selector);
            CodeElementDocumentation docElem = null ;

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
                // ReSharper disable once UnusedVariable
                case EventFieldDeclarationSyntax eventFieldDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case FieldDeclarationSyntax fieldDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case ConstructorDeclarationSyntax constructorDeclarationSyntax :
                    break ;
                // ReSharper disable once UnusedVariable
                case ConversionOperatorDeclarationSyntax conversionOperatorDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case DestructorDeclarationSyntax destructorDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case MethodDeclarationSyntax methodDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case OperatorDeclarationSyntax operatorDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case EventDeclarationSyntax eventDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case IndexerDeclarationSyntax indexerDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case PropertyDeclarationSyntax propertyDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case ClassDeclarationSyntax classDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case EnumDeclarationSyntax enumDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case StructDeclarationSyntax structDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case DelegateDeclarationSyntax delegateDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
                case EnumMemberDeclarationSyntax enumMemberDeclarationSyntax : break ;
                // ReSharper disable once UnusedVariable
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeSyntax TransformParsePocoType ( [ NotNull ] TypeSyntax type )
        {
            if ( type is PredefinedTypeSyntax )
            {
                DebugUtils.WriteLine($"Predefined type {type} does not apply for poco transform.");
                return type;
            }
            SimpleNameSyntax sns1 = null ;
            if ( type is QualifiedNameSyntax qual )
            {
                if ( qual.Right is SimpleNameSyntax sns )
                {
                    sns1 = sns ;
                }
            } else if ( type is SimpleNameSyntax sns )
            {
                sns1 = sns ;
            }
            else
            {
                throw new InvalidOperationException();
            }

            if ( sns1.Identifier.Text.StartsWith ( _pocoPrefix ) )
            {
                throw new InvalidOperationException ( "Type already has poco prefix" ) ;
            }
            var pocoType = SyntaxFactory.ParseTypeName ( _pocoPrefix + sns1.Identifier.Text ) ;
            return pocoType ;
        }


        /// <summary>
        /// Called when generating PropertyDeclarationSyntax nodes for
        /// generated Poco-style properties.
        /// </summary>
        /// <param name="tField"></param>
        /// <param name="candidateTypeSyntax"></param>
        /// <param name="collectionMap"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TypeSyntax SubstituteType (
            [ NotNull ]   SyntaxFieldInfo                tField
          , [ CanBeNull ] TypeSyntax                     candidateTypeSyntax
          , IReadOnlyDictionary < string , object > collectionMap
          , ISyntaxTypesService model
        )
        {
            if ( tField == null )
            {
                throw new ArgumentNullException ( nameof ( tField ) ) ;
            }

            if ( candidateTypeSyntax != null )
            {
                return TransformParsePocoType ( candidateTypeSyntax ) ;
            }

            string info = null;
            var typeSyntax = ( SimpleNameSyntax ) candidateTypeSyntax ;

            var appTypeInfo = model.GetAppTypeInfo ( typeSyntax.Identifier.ValueText ) ;
            if ( appTypeInfo == null )
            {
                throw new InvalidOperationException ( "Invalid type info" ) ;
            }
            if(collectionMap.TryGetValue(typeSyntax.Identifier.ValueText, out var info2)) { 
                // ReSharper disable once RedundantAssignment
                info = ( string ) info2 ;
            }
            else
            {
                throw new InvalidOperationException ( "No collection type in the map." ) ;
            }
            return SyntaxTypesService.FieldPocoCollectionType( candidateTypeSyntax , collectionMap ,appTypeInfo
                                             ) ;
        }
    }

    /// <summary>
    /// Quote inline block XML documentation element.
    /// </summary>
    public sealed class Quoteinline : BlockDocElem
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
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