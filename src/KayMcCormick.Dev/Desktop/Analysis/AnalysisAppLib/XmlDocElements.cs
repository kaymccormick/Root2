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
using System.Collections.Generic ;
using System.Linq ;
using System.Xml.Linq ;
using JetBrains.Annotations ;

namespace AnalysisAppLib
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
    }

    public class Quoteinline : BlockDocElem
    {
        public Quoteinline ( ) {
        }

        public Quoteinline ( IEnumerable < XmlDocElement > @select ) : base(@select)
        {

        }
    }
}