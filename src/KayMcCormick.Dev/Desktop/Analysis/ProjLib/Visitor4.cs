#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Visitor4.cs
// 
// 2020-03-02-6:25 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Threading ;
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace ProjLib
{
    public class Visitor4 : CSharpSyntaxWalker
    {
        private readonly TaskScheduler          _t ;
        private readonly SynchronizationContext _ctx ;
        private readonly ICodeRenderer          _ctl ;

        private int  _curLine         = - 1 ;
        private bool _isAtStartOfLine = true ;
#if DEBUG
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

#else
        private static Logger Logger = LogManager.CreateNullLogger();
#endif
        private List < double > _lineStart = new List < double > ( ) ;


        public Visitor4 ( TaskScheduler t , SynchronizationContext ctx , ICodeRenderer ctl ) :
            base ( SyntaxWalkerDepth.Trivia )
        {
            _t   = t ;
            _ctx = ctx ;
            _ctl = ctl ;
        }

        public async Task DefaultVisitAsync ( SyntaxNode node )
        {
            await Task.Run ( ( ) => DefaultVisit ( node ) ) ;
        }

        #region Overrides of CSharpSyntaxWalker
        public override void Visit ( SyntaxNode node )
        {
            // RecordLocation(node.GetLocation(), out var newLine);
            _ctx.Post (
                       state => {
                           _ctl.StartNode ( node ) ;
                       }
                     , null
                      ) ;

            base.Visit ( node ) ;

            _ctx.Post (
                       state => {
                           _ctl.EndNode ( node ) ;
                       }
                     , null
                      ) ;
        }

        public override void DefaultVisit ( SyntaxNode node ) { base.DefaultVisit ( node ) ; }

        private void RecordLocation ( Location getLocation , out bool newLine )
        {
            newLine = false ;
            var line = getLocation.GetLineSpan ( ).StartLinePosition.Line ;
            Logger.Trace (
                          "Start position: {start}"
                        , getLocation.GetMappedLineSpan ( ).StartLinePosition
                         ) ;
            Logger.Trace ( "{line} > ? {_curLine}" , line , _curLine ) ;
            if ( line > _curLine )
            {
                for ( ; _curLine < line - 1 ; _curLine += 1 )
                {
                    if ( _curLine >= 0 )
                    {
                        Logger.Trace ( "Insert New line {line}" , _curLine ) ;
                    }
                }
#if DEBUG
                Logger.Trace ( "New line {line}" , line ) ;
#endif
#if DEBUG
                Logger.Trace ( $"create new paragraph" ) ;
#endif
                newLine = true ;
                // AdornerDecorator d = new AdornerDecorator();
                _curLine += 1 ;
                // d.Child = new TextBlock ( ) { Text = ( line + 1 ).ToString ( ) } ;
                //AdornerLayer l = AdornerLayer.GetAdornerLayer(_curBlock.);
                //_document.Blocks.Add ( _curBlock ) ;
                Logger.Trace ( $"add to blocks" ) ;

                _isAtStartOfLine = true ;
                
            }
        }

        public List < double > LineStart => _lineStart ;

        public override void VisitToken ( SyntaxToken token )
        {
            VisitLeadingTrivia ( token ) ;
            DoToken ( token ) ;
            VisitTrailingTrivia ( token ) ;
            base.VisitToken ( token ) ;
        }

        private void DoToken ( SyntaxToken token )
        {
            RecordLocation ( token.GetLocation ( ) , out var newLine ) ;
            var text = token.ToString ( ) ;
            if ( _isAtStartOfLine )
            {
                var startChar = token.GetLocation ( )
                                     .GetMappedLineSpan ( )
                                     .StartLinePosition.Character ;
                if ( startChar > 0 )
                {
                    var x = new string ( ' ' , startChar ) ;
                    text = x + text ;
                }

                _isAtStartOfLine = false ;
            }

            _ctx.Post (
                       ( state ) => {
                           if ( newLine )
                           {
                               _ctl.NewLine ( ) ;
                           }

                           _ctl.addToken ( ( ushort ) token.RawKind , text , newLine ) ;
                       }
                     , null
                      ) ;
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

            RecordLocation ( syntaxTrivia.GetLocation ( ) , out var newLine ) ;

            var text = syntaxTrivia.ToString ( ) ;
            if ( syntaxTrivia.Kind ( ) == SyntaxKind.WhitespaceTrivia )
            {
                if ( text.Contains ( "\r" ) )
                {
                    return ;
                }
            }

            _ctx.Post (
                       ( state ) => {
                           if ( newLine )
                           {
                               _ctl.NewLine ( ) ;
                           }

                           _ctl.addTrivia ( syntaxTrivia.RawKind , text , newLine ) ;
                       }
                     , null
                      ) ;
        }

        private new void VisitTrailingTrivia ( SyntaxToken token )
        {
            foreach ( var syntaxTrivia in token.TrailingTrivia )
            {
                DoTrivia ( syntaxTrivia ) ;
            }
        }

        public override void VisitTrivia ( SyntaxTrivia trivia ) { base.VisitTrivia ( trivia ) ; }
        #endregion
    }
}