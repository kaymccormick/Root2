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
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Markup ;
using System.Windows.Media ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.VisualStudio.Text ;
using NLog ;

namespace ProjLib
{
    public class Visitor3 : CSharpSyntaxWalker
    {
        public MyFlowDocumentScrollViewer FlowViewer { get ; }

        private readonly FlowDocument _document ;
        private int _curLine = - 1 ;
        private bool _isAtStartOfLine = true ;
        private Paragraph _curBlock ;
        private Style _curStyle ;
        private Stack < Style > _styles  = new Stack < Style > ();
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private List < double > _lineStart  = new List < double> ();
        private double _oldLineStart ;
        private bool attached ;

        public Visitor3 ( FlowDocument document , MyFlowDocumentScrollViewer flowViewer ):base(SyntaxWalkerDepth.Trivia)
        {
            FlowViewer = flowViewer ;
            _document = document ;
        }

        public async Task DefaultVisitAsync ( SyntaxNode node )
        {
            await Task.Run ( ( ) => DefaultVisit ( node ) ) ;
        }

        #region Overrides of CSharpSyntaxWalker
        public override void Visit ( SyntaxNode node )
        {
            RecordLocation(node.GetLocation());
            var style = FindStyle ( node ) ;
            bool doPop = false;
            if ( style != null ) 
            {
                Styles.Push ( _curStyle ) ;
                doPop = true ;
                _curStyle = new Style(typeof(Run), _curStyle);
                // MergeStyle ( _curStyle , style ) ;
            }

            base.Visit ( node ) ;
            if ( doPop )
            {
                _curStyle = Styles.Pop ( ) ;
            }

            var b = VisualTreeHelper.GetDescendantBounds ( FlowViewer ) ;
            Logger.Info ( "bounds is {b}" , b ) ;
            var fileLinePositionSpan = node.GetLocation().GetMappedLineSpan() ;
            var x = fileLinePositionSpan.StartLinePosition;
            if ( x.Line < LineStart.Count )
            {
                var begin = LineStart[ x.Line ] ;
                // Logger.Debug ( "Begin is {begin} for {line}" , begin , x.Line ) ;
            }

            var y = fileLinePositionSpan.EndLinePosition ;
            if ( LineStart.Count >= y.Line + 1)
            {
                var s = LineStart[ y.Line ] ;
                // Logger.Debug ( "start for {line} is {s}" , y.Line , s ) ;
            }

            DependencyObject elem = FlowViewer.ScrollViewer ;
            if ( elem != null ) {
                var count = VisualTreeHelper.GetChildrenCount ( elem ) ;
                for ( int i = 0 ; i < count ; i ++ )
                {
                    elem = FlowViewer ;
                    var child = VisualTreeHelper.GetChild ( elem , 0 ) ;
                    Logger.Info ( "{}" , child.GetType ( ) ) ;
                }
            }
        }

        public Stack <Style> Styles { get { return _styles ; } } 

        private Style FindStyle ( SyntaxNode node )
        {
            var r = _document.TryFindResource ( node.Kind ( ) ) ;
            return r as Style ;
        }

        public override void DefaultVisit ( SyntaxNode node ) { base.DefaultVisit ( node ) ; }

        private void RecordLocation(Location getLocation)
        {
            var line = getLocation.GetLineSpan().StartLinePosition.Line;
            Logger.Trace (
                          "Start position: {start}"
                        , getLocation.GetMappedLineSpan ( ).StartLinePosition
                         ) ;
            Logger.Info ( "{line} > ? {_curLine}" , line , _curLine ) ;
            if (line > _curLine)
            {
                for ( ; _curLine < line - 1; _curLine += 1 )
                {
                    if ( _curLine >= 0 )
                    {
                        Logger.Warn( "Insert New line {line}" , _curLine ) ;
                        _document.Blocks.Add ( new Paragraph ( ) { Margin = new Thickness(0)} ) ;
                    }
                }
                Logger.Trace("New line {line}", line);

                if ( _curBlock != null )
                {
                    
                    // var rr = _curBlock.Inlines.FirstInline.ContentStart.GetCharacterRect (
                                                                                 // LogicalDirection
                                                                                    // .Forward
                                                                                // ) ;
                    // Logger.Warn ( "{line} {}" , line, rr ) ;
                    // _oldLineStart += _curBlock.LineHeight ;
                }

                Logger.Warn($"create new paragraph");
                _curBlock = new Paragraph ( ) { KeepTogether = true, KeepWithNext = true, Margin = new Thickness(0) } ;
                // AdornerDecorator d = new AdornerDecorator();
                _curLine += 1 ;
                // d.Child = new TextBlock ( ) { Text = ( line + 1 ).ToString ( ) } ;
                ///AdornerLayer l = AdornerLayer.GetAdornerLayer(_curBlock.);
                //_document.Blocks.Add ( _curBlock ) ;
                Logger.Warn($"add to blocks");
                _document.Blocks.Add ( _curBlock ) ;
                if(FlowViewer.ScrollViewer != null && !attached)
                {
                    FlowViewer.ScrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
                    attached = true ;
                }
                var offset = FlowViewer.ScrollViewer?.HorizontalOffset ;
                //_curLine = line;
                Logger.Warn ( "mark at start of line {offset}" , offset ) ;
                _isAtStartOfLine = true;
                LineStart.Add ( _oldLineStart) ;
            }
        }

        private void ScrollViewerOnScrollChanged ( object sender , ScrollChangedEventArgs e )
        {
            Logger.Info ( "offset {}" , e.HorizontalOffset ) ;
        }

        public List < double> LineStart { get { return _lineStart ; } }

        public override void VisitToken ( SyntaxToken token )
        {
            VisitLeadingTrivia(token);
            DoToken(token);
            VisitTrailingTrivia(token);
            base.VisitToken(token);
        }

        private void DoToken ( SyntaxToken token )
        {
            RecordLocation ( token.GetLocation ( ) ) ;
            var text = token.ToString ( ) ;
            if ( _isAtStartOfLine )
            {
                var startChar = token.GetLocation ( ).GetMappedLineSpan ( ).StartLinePosition.Character ;
                if ( startChar > 0 )
                {
                    LogManager.GetCurrentClassLogger ( ).Info ( "apending {s}" , startChar ) ;
                    var x = new String ( ' ' , startChar ) ;
                    text = x + text ;
                }

                _isAtStartOfLine = false ;
            }
            Run run = new Run ( text ) ;


            
            var resource = _document.TryFindResource ( token.Kind ( ) ) ;
            if ( resource is Style style )
            {

            }

            _curBlock.Inlines.Add ( run ) ;
        }

        private void MergeStyle ( Style curStyle , Style style )
        {
            foreach (var styleSetter in style.Setters)
            {
                if (styleSetter is Setter s)
                {
                    Setter s2 = new Setter(s.Property, s.Value, s.TargetName);
//                    Logger.Info ( "adding setter {s2}" , s2 ) ;
                    curStyle.Setters.Add(s2);
                    //toRemove.Add(s2);
                }
            }
        }

        private void VisitLeadingTrivia ( SyntaxToken token )
        {
            foreach ( var syntaxTrivia in token.LeadingTrivia )
            {
                DoTrivia ( syntaxTrivia ) ;
            }

        }

        private void DoTrivia ( in SyntaxTrivia syntaxTrivia )
        {

            if ( syntaxTrivia.Kind ( ) == SyntaxKind.EndOfLineTrivia )
            {
                return ;
            }

            RecordLocation(syntaxTrivia.GetLocation());

            var text = syntaxTrivia.ToString(); 
            if (syntaxTrivia.Kind() == SyntaxKind.WhitespaceTrivia)
            {
                if ( text.Contains ( "\r" ) )
                {
                    return ;
                }
            }


            Run run = new Run(text);
            // var resource = _document.TryFindResource(syntaxTrivia.Kind());
            _curBlock.Inlines.Add(run);
            
        }

        private void VisitTrailingTrivia ( SyntaxToken token )
        {
            foreach (var syntaxTrivia in token.TrailingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
        }

        public override void VisitTrivia ( SyntaxTrivia trivia ) { base.VisitTrivia ( trivia ) ; }
        #endregion
    }
}