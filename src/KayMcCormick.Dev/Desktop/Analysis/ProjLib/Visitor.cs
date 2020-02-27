using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text.RegularExpressions ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using CodeAnalysisApp1 ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;
using Newtonsoft.Json ;
using NLog ;

namespace ProjLib
{
    public class Visitor : Visitor2Impl
    {
    }

    public class NewVisitor : IVisitor
    {
        private IVisitor _visitorImplementation ;

        class Visitor : CSharpSyntaxWalker
        {
        }

        #region Implementation of IVisitor
        public NewVisitor (
            IVisitor          visitorImplementation
          , SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node
        ) : base ( depth )
        {
            _visitorImplementation = visitorImplementation ;
        }

        public Stack < Action < object > > Nodes
        {
            get => _visitorImplementation.Nodes ;
            set => _visitorImplementation.Nodes = value ;
        }

        public TextSpan TokenSpan
        {
            get => _visitorImplementation.TokenSpan ;
            set => _visitorImplementation.TokenSpan = value ;
        }

        public bool StartOfLine
        {
            get => _visitorImplementation.StartOfLine ;
            set => _visitorImplementation.StartOfLine = value ;
        }

        public int CurLine
        {
            get => _visitorImplementation.CurLine ;
            set => _visitorImplementation.CurLine = value ;
        }

        public TextSpan currentSpan
        {
            get => _visitorImplementation.currentSpan ;
            set => _visitorImplementation.currentSpan = value ;
        }

        public TextBlock CurBlock
        {
            get => _visitorImplementation.CurBlock ;
            set => _visitorImplementation.CurBlock = value ;
        }

        public SemanticModel Model
        {
            get => _visitorImplementation.Model ;
            set => _visitorImplementation.Model = value ;
        }

        public Dictionary < object , ISpanViewModel > ActiveSpans
        {
            get => _visitorImplementation.ActiveSpans ;
            set => _visitorImplementation.ActiveSpans = value ;
        }

        public TextSpan SyntaxNodeSpan
        {
            get => _visitorImplementation.SyntaxNodeSpan ;
            set => _visitorImplementation.SyntaxNodeSpan = value ;
        }
        #endregion
    }

    public class Visitor2 : CSharpSyntaxWalker
    {
        private readonly NodeAdapter.Visitor _visitor ;
        private          SemanticModel       _model ;

        private        Stack < IAddChild > _nodes = new Stack < IAddChild > ( ) ;
        private        object              _logInvAnno ;
        private static Logger              logger = LogManager.GetCurrentClassLogger ( ) ;

        public Visitor2 ( NodeAdapter.Visitor visitor , SemanticModel model )
        {
            _visitor = visitor ;
            _model   = model ;
        }

        public ActiveSpans ActiveSpans { get ; } = new ActiveSpans ( ) ;

        // public Dictionary<object, ISpanObject> ActiveSpans { get; } =
        //     new Dictionary<object, ISpanObject>();
        public override void VisitToken ( SyntaxToken token ) { base.VisitToken ( token ) ; }

        public override void DefaultVisit ( SyntaxNode node )
        {
            var obj = new SyntaxNodeSpanObject ( node.Span , node ) ;

            try
            {
#pragma warning restore CA1305 // Specify IFormatProvider
                base.DefaultVisit ( node ) ;

                ActiveSpans.RemoveAll ( node ) ;
                Nodes.Pop ( ) ;
            }
            catch ( Exception ex )
            {
                logger.Error ( ex , ex.ToString ) ;
            }
        }

        public object logInvAnno { get { return _logInvAnno ; } set { _logInvAnno = value ; } }

        public Stack < IAddChild > Nodes { get { return _nodes ; } }

        public SemanticModel Model { get { return _model ; } }
    }

    public class NodeAdapter
    {
        private Dispatcher _dispatch ;

        public void vlid ( )
        {
            var _visitor = this ;
            var xx = _dispatch.Invoke ( this.createPanel ( ) ) ;

            var wrapPanel = ( WrapPanel ) xx ;
            Debug.Assert ( wrapPanel != null , nameof ( wrapPanel ) + " != null" ) ;
            // Nodes.Peek()(wrapPanel);

            // Nodes.Push(
            // o => {
            // try
            // {
            // wrapPanel.Dispatcher.Invoke(
            // () => wrapPanel.Children.Add(
            // (UIElement
            // )o
            // )
            // , DispatcherPriority.Send
            // );
            // }
            // catch (Exception ex)
            // {
            // Visitor.Logger.Error(ex, "zz: " + ex);
            // }
            // }
            // );

#pragma warning disable CA1305 // Specify IFormatProvider
            // logger
            // .Error(
            // "{method}{where}{line}{kind}"
            // , nameof()
            // , ""
            // , node.GetLocation().GetMappedLineSpan().StartLinePosition.Line
            // + 1
            // , node.Kind()
            // );)

        }

        private INodePanel createPanel ( ) { }


        public Dispatcher Dispatcher { get { return _dispatch ; } }

        public class ReprNode < T , TChild >
        {
            private List < TChild > _children = new List < TChild > ( ) ;
            private void            Add ( TChild item ) { Children.Add ( item ) ; }

            public List < TChild > Children
            {
                get { return _children ; }
                set => _children = value ;
            }
        }


        public class VisitorOld : CSharpSyntaxWalker
        {
            private readonly IVisitor _visitor ;

            private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

            private readonly CodeAnalyseContext       _b ;
            private readonly Func < object , object > _findResource ;

            private readonly Brush[] _colors = { Brushes.Red , Brushes.Yellow } ;

            private          int      _i ;
            private readonly Visitor2 _visitor2 ;

            public VisitorOld(
                IVisitor          visitor
              , SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node
            ) : base(depth )
            {
                _visitor2 = new Visitor2 ( this ) ;
                _visitor  = visitor ;
            }

            public VisitorOld (
                [ NotNull ] Panel        container
              , Func < object , object > findResource
              , CodeAnalyseContext       b
              , SemanticModel            model
              , SyntaxWalkerDepth        depth = SyntaxWalkerDepth.Trivia
            ) : base (depth )
            {
                if ( container == null )
                {
                    throw new ArgumentNullException ( nameof ( container ) ) ;
                }

                try
                {
                    _visitor2     = new Visitor2 ( this ) ;
                    _findResource = findResource ?? container.TryFindResource ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , ex.ToString ( ) ) ;
                }

                _b    = b ;
                Model = model ;
                // CurBlock      = block ;
                Dispatch = container.Dispatcher ;

                Nodes.Push (
                            o => {
                                if ( o == null )
                                {
                                    Logger.Error ( "here1" ) ;
                                    throw new ArgumentNullException ( nameof ( o ) ) ;
                                }

                                Logger.Error ( "here" ) ;
                                container.Dispatcher.Invoke (
                                                             ( ) => {
                                                                 if ( o != null )
                                                                 {
                                                                     container.Children.Add (
                                                                                             ( UIElement
                                                                                             ) o
                                                                                            ) ;
                                                                 }
                                                             }
                                                           , DispatcherPriority.Send
                                                            ) ;
                            }
                           ) ;
                ;
            }

            public Stack < Action < object > > Nodes { get ; } =
                new Stack < Action < object > > ( ) ;

            public bool DoSym { get ; set ; }

            public Dispatcher Dispatch { get ; set ; }

            public TextSpan TokenSpan { get ; set ; }

            public bool StartOfLine { get ; set ; }

            public int CurLine { get ; set ; }

            public TextSpan currentSpan { get ; set ; }

            public TextBlock CurBlock { get ; set ; }

            public SemanticModel Model { get ; set ; }

            public ActiveSpans ActiveSpans { get ; } = new ActiveSpans ( ) ;

            public TextSpan SyntaxNodeSpan { get ; set ; }

            public override void DefaultVisit ( SyntaxNode node )
            {
                _visitor2.DefaultVisit ( node ) ;
            }

            private Panel Target ( ) { return new WrapPanel ( ) ; }

            public void SetSyntaxNodeSpan (
                in TextSpan          nodeFullSpan
              , SyntaxNode           node
              , SyntaxNodeSpanObject spanObject
            )
            {
                SyntaxNodeSpan      = nodeFullSpan ;
                ActiveSpans.AddSpan ( node , node, spanObject ) ;
            }


            public void SetTokenSpan (
                in TextSpan     tokenSpan
              , Location        getLocation
              , SyntaxToken     token
              , TokenSpanObject spano
            )
            {
                RecordSpan ( tokenSpan ) ;
                RecordLocation ( getLocation ) ;
                ActiveSpans[ token ] = spano ;
                TokenSpan            = tokenSpan ;
            }

            public void RecordLocation ( Location getLocation )
            {
                var line = getLocation.GetLineSpan ( ).StartLinePosition.Line ;
                if ( line > CurLine )
                {
                    CurLine     = line ;
                    StartOfLine = true ;
                }
            }

            public void RecordSpan ( in TextSpan tokenSpan ) { }

            private void AddToken ( in SyntaxToken token , object style )
            {
                foreach ( var syntaxTrivia in token.LeadingTrivia )
                {
                    var s = syntaxTrivia.ToString ( ) ;
                    var syntaxKind = syntaxTrivia.Kind ( ) ;
                    var syntaxTriviaSpan = syntaxTrivia.Span ;
                    Dispatch.Invoke (
                                     ( ) => AddRun (
                                                    RenderTrivia ( s , syntaxKind )
                                                  , syntaxTriviaSpan
                                                   )
                                    ) ;
                }

                var tokenLocation = token.GetLocation ( ) ;
                SetTokenSpan (
                              token.Span
                            , tokenLocation
                            , token
                            , new TokenSpanObject ( token.Span , token )
                             ) ;
                LogManager.GetCurrentClassLogger ( ).Warn ( "{s}" , token ) ;
                var text = token.ToString ( ) ;
                var tokenSpan = token.Span ;

                Dispatch.Invoke (
                                 ( ) => {
                                     _CreateTokenItem ( style , text , tokenSpan , tokenLocation ) ;
                                 }
                               , DispatcherPriority.Send
                                ) ;
                LogManager.GetCurrentClassLogger ( ).Debug ( "hello" ) ;
                ActiveSpans.Remove ( token ) ;
                foreach ( var syntaxTrivia in token.TrailingTrivia )
                {
                    var s = syntaxTrivia.ToString ( ) ;
                    var syntaxKind = syntaxTrivia.Kind ( ) ;
                    var syntaxTriviaSpan = syntaxTrivia.Span ;
                    Dispatch.Invoke (
                                     ( ) => AddRun (
                                                    RenderTrivia ( s , syntaxKind )
                                                  , syntaxTriviaSpan
                                                   )
                                    ) ;
                }

                LogManager.GetCurrentClassLogger ( ).Debug ( "hello" ) ;
            }

            private void _CreateTokenItem (
                object   style
              , string   Text
              , TextSpan textSpan
              , Location tokenLocation
            )
            {
                var run = creATErUN ( Text ) ;
                run.Style = ( Style ) _findResource ( style ) ;

                var spanToolTip = new SpanToolTip ( ) ;
                var tt = new SpanTT ( spanToolTip ) ;
                ISpanToolTipViewModel model = new SpanToolTipViewModel ( ) ;
                tt.ViewModel          = model ;
                tt.ViewModel.Location = tokenLocation ;
                run.ToolTip           = tt ;
                if ( run.Style == null )
                {
                    LogManager.GetCurrentClassLogger ( ).Error ( style.ToString ( ) ) ;
                }

                AddRun ( run , textSpan ) ;
                //LogManager.GetCurrentClassLogger().Info("hello");
            }

            private void AddRun ( Run run , TextSpan span )
            {
                if ( StartOfLine )
                {
                    //renderTrivia.Background = Brushes.Gray ;
                    StartOfLine = false ;
                    Nodes.Pop ( ) ;
                    var wrapPanel = new WrapPanel ( ) ;
                    Nodes.Peek ( ) ( wrapPanel ) ;
                    Nodes.Push (
                                o => wrapPanel.Dispatcher.Invoke (
                                                                  ( ) => wrapPanel.Children.Add (
                                                                                                 ( UIElement
                                                                                                 ) o
                                                                                                )
                                                                , DispatcherPriority.Send
                                                                 )
                               ) ;
                }

                SpanTT tt = null ;
                if ( run.ToolTip == null )
                {
                    tt          = new SpanTT ( new SpanToolTip ( ) ) ;
                    run.ToolTip = tt ;
                }
                else
                {
                    tt = run.ToolTip as SpanTT ;
                }

                LogManager.GetCurrentClassLogger ( ).Info ( "hello1" ) ;

                if ( tt != null )
                {
                    var spans = ActiveSpans
                               .Where ( ( pair , i ) => pair.Value.Span.OverlapsWith ( span ) )
                               .Select ( pair => pair.Value )
                               .Select ( o => ( object ) o )
                               .ToList ( ) ;

                    tt.ViewModel.Spans = spans ;
                    // foreach ( var activeSpan in ActiveSpans )
                    // {
                    //     var key = activeSpan.Key ;
                    //     var val = activeSpan.Value ;
                    //     if ( val.Span.OverlapsWith ( span ) )
                    //     {
                    //         var valToolTipContent = val.GetToolTipContent ( ) ;
                    //         if ( valToolTipContent != null )
                    //         {
                    //             tt.CustomToolTip.Add ( valToolTipContent ) ;
                    //         }
                    //     }
                    // }
                }

                //LogManager.GetCurrentClassLogger().Info("hello");
                Nodes.Peek ( ) ( new TextBlock ( run ) ) ;
                //CurBlock.Isnlines.Add ( renderTrivia ) ;
            }

            public void SetSpan ( in TextSpan tokenFullSpan ) { currentSpan = tokenFullSpan ; }

            public override void VisitTrivia ( SyntaxTrivia trivia )
            {
                _visitor2.VisitTrivia ( trivia ) ;
            }

            private Run RenderTrivia ( string toString , SyntaxKind kind )
            {
                LogManager.GetCurrentClassLogger ( ).Error ( "trivia: {t}" , kind ) ;

                var run = creATErUN ( toString ) ;
                run.Style = ( Style ) _findResource ( kind ) ;
                // run.ToolTip = new ToolTip ( )
                //               {
                //                   Content = new TextBlock ( new Run ( trivia.Kind ( ).ToString ( ) ) )
                //               } ;
                return run ;
            }


            private Run creATErUN (
                string                   toString
              , Func < object , object > _findResource = null
              , object                   key           = null
            )
            {
                return Dispatch.Invoke (
                                        ( ) => {
                                            var r = _creATErUN ( toString ) ;
                                            if ( _findResource != null )
                                            {
                                                r.Style = _findResource ( key ) as Style ;
                                            }

                                            return r ;
                                        }
                                      , DispatcherPriority.Send
                                       ) ;
            }

            private Run _creATErUN ( string toString )
            {
                _i = _i + 1 ;
                var r = new Run ( toString )
                        {
                            TextDecorations = new TextDecorationCollection (
                                                                            new[]
                                                                            {
                                                                                new TextDecoration (
                                                                                                    TextDecorationLocation
                                                                                                       .Underline
                                                                                                  , new
                                                                                                        Pen (
                                                                                                             _colors
                                                                                                                 [ _i
                                                                                                                   % _colors
                                                                                                                      .Length ]
                                                                                                           , 1
                                                                                                            )
                                                                                                  , 0
                                                                                                  , TextDecorationUnit
                                                                                                       .Pixel
                                                                                                  , TextDecorationUnit
                                                                                                       .Pixel
                                                                                                   )
                                                                            }
                                                                           )
                        } ;
                return r ;
            }

            private delegate Panel createPanel ( ) ;
        }


        public class Z : SymbolVisitor
        {
            private readonly TextBlock                _block ;
            private readonly Func < object , object > _findResource ;

            public Z (
                TextBlock                block
              , TextSpan                 statementFullSpan
              , Func < object , object > findResource
            )
            {
                _block        = block ;
                _findResource = findResource ;
            }

            public override void Visit ( ISymbol symbol )
            {
                base.Visit ( symbol ) ;
                foreach ( var symbolDisplayPart in symbol.ToDisplayParts ( ) )
                {
                    var dp = symbolDisplayPart.ToString ( ) ;
                    LogManager.GetCurrentClassLogger ( ).Info ( "dp: {dp}" , dp ) ;
                    var run = new Run ( dp ) ;
                    var resourceKey = "SymbolPart" + symbolDisplayPart.Kind ;
                    run.Style = ( Style ) _findResource ( resourceKey ) ;
                    if ( run.Style == null )
                    {
                        LogManager.GetCurrentClassLogger ( )
                                  .Error ( "need style {name}" , resourceKey ) ;
                    }

                    _block.Inlines.Add ( run ) ;


                    LogManager.GetCurrentClassLogger ( )
                              .Info (
                                     "{a} {b} {c}"
                                   , symbolDisplayPart.Kind
                                   , symbolDisplayPart.Symbol
                                   , dp
                                    ) ;
                }
            }


            public override void DefaultVisit ( ISymbol symbol ) { base.DefaultVisit ( symbol ) ; }
        }
    }
}

    