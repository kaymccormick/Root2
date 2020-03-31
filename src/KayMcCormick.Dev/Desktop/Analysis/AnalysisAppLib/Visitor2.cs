#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Visitor2.cs
// 
// 2020-02-27-5:02 AM
// 
// ---
#endregion
using System ;
using System.Linq ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Fluent ;
using Span ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Visitor2 : CSharpSyntaxWalker
    {
        private static readonly Logger logger =
            LogManager.GetCurrentClassLogger ( ) ;

        private readonly Func < string , object , LogBuilder > _message ;

        private readonly Func < object , ISpanViewModel > _mFunc ;

        /// <summary>
        /// 
        /// </summary>
        public Visitor2 ( )
        {
            _message = new LogBuilder ( LogManager.GetCurrentClassLogger ( ) ).Message ;
        }

        /// <summary>
        /// 
        /// </summary>
        protected ActiveSpans ActiveSpans { get ; } = new ActiveSpans ( ) ;

        // public Dictionary<object, ISpanObject> ActiveSpans { get; } =
        //     new Dictionary<object, ISpanObject>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toke"></param>
        public override void VisitToken ( SyntaxToken toke )
        {
            //base.VisitToken ( token ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void DefaultVisit ( [ NotNull ] SyntaxNode node )
        {
            if ( node == null )
            {
                throw new ArgumentNullException ( nameof ( node ) ) ;
            }

            _message ( "{method}" , nameof ( DefaultVisit ) ).Write ( ) ;
            var model = _mFunc?.Invoke ( node ) ;
            ActiveSpans.AddSpan ( node , node.Span , model ) ;
            try
            {
                var childNodesAndTokens = node.ChildNodesAndTokens ( ).ToList ( ) ;
                var childCnt = childNodesAndTokens.Count ;
                var i = 0 ;

                do
                {
                    var child = childNodesAndTokens[ i ] ;
                    i ++ ;

                    var asNode = child.AsNode ( ) ;
                    if ( asNode != null )
                    {
                        if ( Depth >= SyntaxWalkerDepth.Node )
                        {
                            var model2 = _mFunc?.Invoke ( asNode ) ;
                            ActiveSpans.AddSpan ( asNode , asNode.Span , model2 ) ;
                            Visit ( asNode ) ;
                            ActiveSpans.Remove ( asNode ) ;
                        }
                    }
                    else
                    {
                        if ( Depth >= SyntaxWalkerDepth.Token )
                        {
                            var syntaxToken = child.AsToken ( ) ;
                            if ( _mFunc != null )
                            {
                                var model1 = _mFunc ( syntaxToken ) ;
                                ActiveSpans.AddSpan ( syntaxToken , syntaxToken.Span , model1 ) ;
                            }

                            VisitToken ( syntaxToken ) ;
                            ActiveSpans.RemoveAll ( syntaxToken ) ;
                        }
                    }
                }
                while ( i < childCnt ) ;
            }
            catch ( Exception ex )
            {
                logger.Error ( ex , ex.ToString ) ;
            }
            finally
            {
                ActiveSpans.RemoveAll ( node ) ;
            }
        }
    }
}