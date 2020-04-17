#region header
// Kay McCormick (mccor)
// 
// Proj
// ProjLib
// Visitor3.cs
// 
// 2020-02-27-12:50 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Media ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls
{
    // ReSharper disable once UnusedType.Global
    /// <summary>
    /// 
    /// </summary>
    public class Visitor3 : CSharpSyntaxWalker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly FlowDocument    _document ;
        private          Paragraph       _curBlock ;
        private          int             _curLine = - 1 ;
        private          Style           _curStyle ;
        private          bool            _isAtStartOfLine = true ;
        private readonly Stack < Style > _styles          = new Stack < Style > ( ) ;
        private          bool            attached ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="flowViewer"></param>
        public Visitor3 ( FlowDocument document , MyFlowDocumentScrollViewer flowViewer ) :
            base ( SyntaxWalkerDepth.Trivia )
        {
            FlowViewer = flowViewer ;
            _document  = document ;
        }

        /// <summary>
        /// 
        /// </summary>
        public MyFlowDocumentScrollViewer FlowViewer { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public async Task DefaultVisitAsync ( SyntaxNode node )
        {
            await Task.Run ( ( ) => DefaultVisit ( node ) ) ;
        }

        #region Overrides of CSharpSyntaxWalker
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Visit ( [ NotNull ] SyntaxNode node )
        {
            RecordLocation ( node.GetLocation ( ) ) ;
            var style = FindStyle ( node ) ;
            var doPop = false ;
            if ( style != null )
            {
                Styles.Push ( _curStyle ) ;
                doPop     = true ;
                _curStyle = new Style ( typeof ( Run ) , _curStyle ) ;
                // MergeStyle ( _curStyle , style ) ;
            }

            base.Visit ( node ) ;
            if ( doPop )
            {
                _curStyle = Styles.Pop ( ) ;
            }

            var b = VisualTreeHelper.GetDescendantBounds ( FlowViewer ) ;
            Logger.Info ( "bounds is {b}" , b ) ;
            var fileLinePositionSpan = node.GetLocation ( ).GetMappedLineSpan ( ) ;
            // ReSharper disable once UnusedVariable
            var x = fileLinePositionSpan.StartLinePosition ;
            DependencyObject elem = FlowViewer.ScrollViewer ;
            if ( elem != null )
            {
                var count = VisualTreeHelper.GetChildrenCount ( elem ) ;
                for ( var i = 0 ; i < count ; i ++ )
                {
                    elem = FlowViewer ;
                    var child = VisualTreeHelper.GetChild ( elem , 0 ) ;
                    Logger.Info ( "{}" , child.GetType ( ) ) ;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Stack < Style > Styles { get { return _styles ; } }

        [ CanBeNull ]
        private Style FindStyle ( [ NotNull ] SyntaxNode node )
        {
            var r = _document.TryFindResource ( node.Kind ( ) ) ;
            return r as Style ;
        }

        private void RecordLocation ( [ NotNull ] Location getLocation )
        {
            var line = getLocation.GetLineSpan ( ).StartLinePosition.Line ;
            Logger.Trace (
                          "Start position: {start}"
                        , getLocation.GetMappedLineSpan ( ).StartLinePosition
                         ) ;
            Logger.Info ( "{line} > ? {_curLine}" , line , _curLine ) ;
            if ( line > _curLine )
            {
                for ( ; _curLine < line - 1 ; _curLine += 1 )
                {
                    if ( _curLine >= 0 )
                    {
                        Logger.Warn ( "Insert New line {line}" , _curLine ) ;
                        _document.Blocks.Add ( new Paragraph { Margin = new Thickness ( 0 ) } ) ;
                    }
                }

                Logger.Trace ( "New line {line}" , line ) ;

                if ( _curBlock != null )
                {
                    // var rr = _curBlock.Inlines.FirstInline.ContentStart.GetCharacterRect (
                    // LogicalDirection
                    // .Forward
                    // ) ;
                    // Logger.Warn ( "{line} {}" , line, rr ) ;
                    // _oldLineStart += _curBlock.LineHeight ;
                }

                Logger.Warn ( "create new paragraph" ) ;
                _curBlock = new Paragraph
                            {
                                KeepTogether = true
                              , KeepWithNext = true
                              , Margin       = new Thickness ( 0 )
                            } ;
                // AdornerDecorator d = new AdornerDecorator();
                _curLine += 1 ;
                // d.Child = new TextBlock ( ) { Text = ( line + 1 ).ToString ( ) } ;
                //AdornerLayer l = AdornerLayer.GetAdornerLayer(_curBlock.);
                //_document.Blocks.Add ( _curBlock ) ;
                Logger.Warn ( "add to blocks" ) ;
                _document.Blocks.Add ( _curBlock ) ;
                if ( FlowViewer.ScrollViewer != null
                     && ! attached )
                {
                    FlowViewer.ScrollViewer.ScrollChanged += ScrollViewerOnScrollChanged ;
                    attached                              =  true ;
                }

                var offset = FlowViewer.ScrollViewer?.HorizontalOffset ;
                //_curLine = line;
                Logger.Warn ( "mark at start of line {offset}" , offset ) ;
                _isAtStartOfLine = true ;
            }
        }

        private static void ScrollViewerOnScrollChanged ( object sender , [ NotNull ] ScrollChangedEventArgs e )
        {
            Logger.Info ( "offset {}" , e.HorizontalOffset ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public override void VisitToken ( SyntaxToken token )
        {
            VisitLeadingTrivia ( token ) ;
            DoToken ( token ) ;
            VisitTrailingTrivia ( token ) ;
            base.VisitToken ( token ) ;
        }

        private void DoToken ( SyntaxToken token )
        {
            RecordLocation ( token.GetLocation ( ) ) ;
            var text = token.ToString ( ) ;
            if ( _isAtStartOfLine )
            {
                var startChar = token.GetLocation ( )
                                     .GetMappedLineSpan ( )
                                     .StartLinePosition.Character ;
                if ( startChar > 0 )
                {
                    LogManager.GetCurrentClassLogger ( ).Info ( "appending {s}" , startChar ) ;
                    var x = new string ( ' ' , startChar ) ;
                    text = x + text ;
                }

                _isAtStartOfLine = false ;
            }

            var run = new Run ( text ) ;



            var resource = _document.TryFindResource ( token.Kind ( ) ) ;
            if ( resource is Style )
            {
            }

            _curBlock.Inlines.Add ( run ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private void MergeStyle ( Style curStyle , [ NotNull ] Style style )
        {
            foreach ( var styleSetter in style.Setters )
            {
                if ( styleSetter is Setter s )
                {
                    var s2 = new Setter ( s.Property , s.Value , s.TargetName ) ;
//                    Logger.Info ( "adding setter {s2}" , s2 ) ;
                    curStyle.Setters.Add ( s2 ) ;
                    //toRemove.Add(s2);
                }
            }
        }

        private new void VisitLeadingTrivia ( SyntaxToken token )
        {
            foreach ( var syntaxTrivia in token.LeadingTrivia )
            {
                DoTrivia ( syntaxTrivia ) ;
            }
        }

        private void DoTrivia ( SyntaxTrivia syntaxTrivia )
        {
            if ( syntaxTrivia.Kind ( ) == SyntaxKind.EndOfLineTrivia )
            {
                return ;
            }

            RecordLocation ( syntaxTrivia.GetLocation ( ) ) ;

            var text = syntaxTrivia.ToString ( ) ;
            if ( syntaxTrivia.Kind ( ) == SyntaxKind.WhitespaceTrivia )
            {
                if ( text.Contains ( "\r" ) )
                {
                    return ;
                }
            }


            var run = new Run ( text ) ;
            // var resource = _document.TryFindResource(syntaxTrivia.Kind());
            _curBlock.Inlines.Add ( run ) ;
        }

        private new void VisitTrailingTrivia ( SyntaxToken token )
        {
            foreach ( var syntaxTrivia in token.TrailingTrivia )
            {
                DoTrivia ( syntaxTrivia ) ;
            }
        }
        #endregion
    }
}