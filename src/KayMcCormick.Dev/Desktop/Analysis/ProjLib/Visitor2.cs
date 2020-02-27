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
using System.Collections.Generic ;
using System.Linq ;
using System.Windows.Markup ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public class Visitor2 : CSharpSyntaxWalker
    {
        private          SemanticModel                 _model ;
        private readonly Func <object, ISpanViewModel> _mFunc ;

        private        Stack < IAddChild >                   _nodes = new Stack < IAddChild > ( ) ;
        protected      object                                _logInvAnno ;
        private static Logger                                logger = LogManager.GetCurrentClassLogger ( ) ;
        private        Func < string , object , LogBuilder > _message ;

        public Visitor2 ( IHassModel model)
        {
            _model = model.Model ;
            _message = new LogBuilder(LogManager.GetCurrentClassLogger()).Message ;
        }
        
        public ActiveSpans ActiveSpans { get ; } = new ActiveSpans ( ) ;

        // public Dictionary<object, ISpanObject> ActiveSpans { get; } =
        //     new Dictionary<object, ISpanObject>();
        public override void VisitToken(SyntaxToken toke)
        {
            //base.VisitToken ( token ) ;
        }
        
        public override void DefaultVisit ( SyntaxNode node )
        {
            _message("{method}", nameof(DefaultVisit)).Write();
            ISpanViewModel model =  _mFunc?.Invoke (node ) ;
            ActiveSpans.AddSpan ( node , node.Span, model ) ;
            try
            {
                var childNodesAndTokens = node.ChildNodesAndTokens ( ).ToList ( ) ;
                var childCnt = childNodesAndTokens.Count ;
                int i = 0 ;

                do
                {
                    var child = childNodesAndTokens[ i ] ;
                    i ++ ;

                    var asNode = child.AsNode ( ) ;
                    if ( asNode != null )
                    {
                        if ( this.Depth >= SyntaxWalkerDepth.Node )
                        {
                            ISpanViewModel model2 = _mFunc?.Invoke(asNode);
                            ActiveSpans.AddSpan(asNode, asNode.Span, model2);
                            this.Visit ( asNode ) ;
                            ActiveSpans.Remove ( asNode ) ;
                        }
                    }
                    else
                    {
                        if ( this.Depth >= SyntaxWalkerDepth.Token )
                        {

                            var syntaxToken = child.AsToken ( ) ;
                            var model1 = _mFunc ( syntaxToken ) ;
                            ActiveSpans.AddSpan(syntaxToken, syntaxToken.Span, model1);
                            this.VisitToken ( syntaxToken ) ;
                            ActiveSpans.RemoveAll(syntaxToken);
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
                ActiveSpans.RemoveAll(node);
            }
        }

        public Stack < IAddChild > Nodes { get { return _nodes ; } }

        public SemanticModel Model { get { return _model ; } }

    }
}