﻿#region header
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
using AnalysisAppLib ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    public sealed class Visitor4 : CSharpSyntaxWalker
    {
        private readonly SynchronizationContext _ctx ;
        private readonly ICodeRenderer          _ctl ;

        private int  _curLine         = - 1 ;
        private bool _isAtStartOfLine = true ;
#if DEBUG
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

#else
        private static Logger Logger = LogManager.CreateNullLogger();
#endif
        private readonly List < double > _lineStart = new List < double > ( ) ;


        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ctx"></param>
        /// <param name="ctl"></param>
        // ReSharper disable once UnusedParameter.Local
        public Visitor4 ( TaskScheduler t , SynchronizationContext ctx , ICodeRenderer ctl ) :
            base ( SyntaxWalkerDepth.Trivia )
        {
            _ctx = ctx ;
            _ctl = ctl ;
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task DefaultVisitAsync ( SyntaxNode node )
        {
            await Task.Run ( ( ) => DefaultVisit ( node ) ) ;
        }

        #region Overrides of CSharpSyntaxWalker
        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void Visit ( SyntaxNode node )
        {
            // RecordLocation(node.GetLocation(), out var newLine);
            _ctx.Post (
                       state => {
                           if ( state == null )
                           {
                               throw new ArgumentNullException ( nameof ( state ) ) ;
                           }

                           _ctl.StartNode ( node ) ;
                       }
                     , null
                      ) ;

            base.Visit ( node ) ;

            _ctx.Post ( state => _ctl.EndNode ( node ) , null ) ;
        }

        private void RecordLocation ( [ NotNull ] Location getLocation , out bool newLine )
        {
            newLine = false ;
            var line = getLocation.GetLineSpan ( ).StartLinePosition.Line ;
            Logger.Trace (
                          "Start position: {start}"
                        , getLocation.GetMappedLineSpan ( ).StartLinePosition
                         ) ;
            Logger.Trace ( "{line} > ? {_curLine}" , line , _curLine ) ;
            if ( line <= _curLine )
            {
                return ;
            }

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
            Logger.Trace ( "create new paragraph" ) ;
#endif
            newLine = true ;
            // AdornerDecorator d = new AdornerDecorator();
            _curLine += 1 ;
            // d.Child = new TextBlock ( ) { Text = ( line + 1 ).ToString ( ) } ;
            //AdornerLayer l = AdornerLayer.GetAdornerLayer(_curBlock.);
            //_document.Blocks.Add ( _curBlock ) ;
            Logger.Trace ( "add to blocks" ) ;

            _isAtStartOfLine = true ;
        }

        /// <summary>
        /// </summary>
        public List < double > LineStart { get { return _lineStart ; } }

        /// <summary>
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
                       state => {
                           if ( newLine )
                           {
                               _ctl.NewLine ( ) ;
                           }

                           _ctl.AddToken ( ( ushort ) token.RawKind , text , newLine ) ;
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
                       state => {
                           if ( newLine )
                           {
                               _ctl.NewLine ( ) ;
                           }

                           _ctl.AddTrivia ( syntaxTrivia.RawKind , text , newLine ) ;
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
        #endregion
    }
}