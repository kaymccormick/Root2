#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// CodeAnalysisApp1
// AWalker.cs
// 
// 2020-02-14-1:56 PM
// 
// ---
#endregion
using System ;
using System.Xml ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace CodeAnalysisApp1
{
    /// <summary>
    ///     XML Walker
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class AWalker : SyntaxWalker
    {
        private static readonly Logger     Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        XmlElement _element ;

        /// <summary>Creates a new walker instance.</summary>
        /// <param name="element"></param>
        /// <param name="depth">
        ///     Syntax the
        ///     <see cref="SyntaxWalker" /> should descend
        ///     into.
        /// </param>
        public AWalker (
            XmlElement        element
          , SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node
        ) : base ( depth )
        {
            _element      = element ;
            CurXmlElement = element ;
        }

        /// <summary>
        /// Current XML Element
        /// </summary>
        public XmlElement CurXmlElement { get ; set ; }

        /// <summary>
        ///     Called when the walker visits a node.  This method may be overridden if
        ///     subclasses want
        ///     to handle the node.  Overrides should call back into this base method if
        ///     they want the
        ///     children of this node to be visited.
        /// </summary>
        /// <param name="node">The current node that the walker is visiting.</param>
        public override void Visit ( SyntaxNode node )
        {
            try
            {
                switch ( node.Kind ( ) )
                {
                    case SyntaxKind.SimpleMemberAccessExpression :
                        var s = ( MemberAccessExpressionSyntax ) node ;
                        base.Visit ( node ) ;

                        // ReSharper disable once PossibleNullReferenceException
                        var elem = _element.OwnerDocument.CreateElement ( "M" ) ;
                        elem.AppendChild ( CurXmlElement ) ;
                        CurXmlElement = elem ;
                        var name = _element.OwnerDocument.CreateElement ( "Name" ) ;
                        elem.AppendChild ( name ) ;
                        name.SetAttribute ( "ValueText" , s.Name.Identifier.ValueText ) ;
                        return ;
                    case SyntaxKind.IdentifierName :
                        CurXmlElement =
                            // ReSharper disable once PossibleNullReferenceException
                            _element.OwnerDocument.CreateElement ( node.Kind ( ).ToString ( ) ) ;
                        CurXmlElement.SetAttribute (
                                                    "Value"
                                                  , ( ( IdentifierNameSyntax ) node )
                                                   .Identifier.ValueText
                                                   ) ;
                        //((IdentifierNameSyntax)node).Identifier.
                        break ;
                    case SyntaxKind.StringLiteralExpression :
                        CurXmlElement.AppendChild (
                                                   // ReSharper disable once PossibleNullReferenceException
                                                   _element.OwnerDocument.CreateTextNode (
                                                                                          ( (
                                                                                                  LiteralExpressionSyntax
                                                                                              ) node
                                                                                          ).Token
                                                                                           .ValueText
                                                                                         )
                                                  ) ;
                        break ;
                    default :
                        Logger.Fatal (
                                      "{type} {kind}"
                                    , node.GetType ( ).FullName
                                    , node.Kind ( )
                                     ) ;
                        break ;
                    // return ;
                }
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , "{a} {b}" , node.Kind ( ) , ex.Message ) ;
                // return ;
            }

            base.Visit ( node ) ;
        }
    }
}