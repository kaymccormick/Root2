using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;
using Newtonsoft.Json ;
using NLog ;
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text.RegularExpressions ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Media ;
using System.Windows.Shapes ;
using System.Windows.Threading ;
using JetBrains.Annotations ;

namespace ProjLib
{
    public class Visitor : CSharpSyntaxWalker
    {
        private readonly Func < object , object > _findResource ;
        private static   Logger                   Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly CodeAnalyseContext       _b ;
        private          int                      _i ;
        private          Brush[]                  _colors = new[] { Brushes.Red , Brushes.Yellow } ;
        private          SemanticModel            _model ;

        public override void DefaultVisit ( SyntaxNode node )
        {
            var obj = new SyntaxNodeSpanObject ( node.Span , node ) ;
            var logInvAnno_ = node.GetAnnotations ( "LogInvocation" ) ;
            SyntaxAnnotation logInvAnno = null ;
            if ( logInvAnno_.Any ( ) )

            {
                logInvAnno = logInvAnno_.First ( ) ;
                var inv = JsonConvert.DeserializeObject < LogInvocation > ( logInvAnno.Data ) ;
                if ( inv != null )
                {
                    inv.CurrentModel = Model ;
                    inv.MethodSymbol = Model.Compilation.GetTypeByMetadataName ( inv.LoggerType )
                                            .GetMembers ( inv.MethodName )
                                            .OfType < IMethodSymbol > ( )
                                            .FirstOrDefault ( ) ;
                    ActiveSpans[ logInvAnno ] =
                        new LogInvocationSpan ( node.Span , inv , _findResource ) ;
                }
                else
                {
                    LogManager.GetCurrentClassLogger ( ).Warn ( "null deserialziation" ) ;
                }
            }

            try
            {
                var xx = Dispatch.Invoke ( new createPanel ( Target ) ) ;

                var wrapPanel = ( WrapPanel ) xx ;
                Debug.Assert ( wrapPanel != null , nameof ( wrapPanel ) + " != null" ) ;
                Nodes.Peek ( ) ( wrapPanel ) ;

                Nodes.Push (
                            o => {
                                try
                                {
                                    wrapPanel.Dispatcher.Invoke (
                                                                 ( ) => wrapPanel.Children.Add (
                                                                                                ( UIElement
                                                                                                ) o
                                                                                               )
                                                               , DispatcherPriority.Send
                                                                ) ;
                                }
                                catch ( Exception ex )
                                {
                                    Logger.Error ( ex , "zz: " + ex.ToString ( ) ) ;
                                }
                            }
                           ) ;

                SetSyntaxNodeSpan ( node.FullSpan , node , obj ) ;
#pragma warning disable CA1305 // Specify IFormatProvider
                LogManager.GetCurrentClassLogger ( )
                          .Error (
                                  "{method}{where}{line}{kind}"
                                , nameof ( DefaultVisit )
                                , ""
                                , node.GetLocation ( ).GetMappedLineSpan ( ).StartLinePosition.Line
                                  + 1
                                , node.Kind ( )
                                 ) ;
#pragma warning restore CA1305 // Specify IFormatProvider
                base.DefaultVisit ( node ) ;
                // Brush bg = Brushes.Transparent , fg = null ;
                // SymbolInfo info = default ;
                // try
                // {
                //     var bCurrentModel = _b.CurrentModel ;
                //     info = bCurrentModel.GetSymbolInfo ( node ) ;
                // }
                // catch ( Exception ex )
                // {
                //     LogManager.GetCurrentClassLogger ( ).Warn ( ex , ex.ToString ( ) ) ;
                //     throw ;
                // }
                //
                // if ( DoSym && info.Symbol != null )
                // {
                //     var z = new Z ( CurBlock , node.FullSpan , _findResource ) ;
                //     z.Visit ( info.Symbol ) ;
                // }

                // var x = node.Kind ( ).ToString ( ) ;
                // var m = Regex.Match ( x , "([A-Z][a-z]*)$" ) ;
                // var style = m.Groups[ 1 ].Captures[ 0 ].Value ;
                //
                // var run = creATErUN ( node.ToString ( ) , _findResource , node.Kind ( ) ) ;

                // CurBlock.Inlines.Add ( run );
                ActiveSpans.Remove ( node ) ;
                if ( logInvAnno != null )
                {
                    ActiveSpans.Remove ( logInvAnno ) ;
                }

                Nodes.Pop ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ) ;
            }
        }

        private Panel Target ( ) { return new WrapPanel ( ) ; }

        private delegate Panel createPanel ( ) ;

        public Stack < Action < object > > Nodes { get ; } = new Stack < Action < object > > ( ) ;

        public SemanticModel Model { get => _model ; set => _model = value ; }

        private void SetSyntaxNodeSpan (
            in TextSpan          nodeFullSpan
          , SyntaxNode           node
          , SyntaxNodeSpanObject spanObject
        )
        {
            SyntaxNodeSpan      = nodeFullSpan ;
            ActiveSpans[ node ] = spanObject ;
        }

        public Dictionary < object , ISpanObject > ActiveSpans { get ; } =
            new Dictionary < object , ISpanObject > ( ) ;

        public TextSpan SyntaxNodeSpan { get ; set ; }

        public bool DoSym { get ; set ; }

        public Visitor ( SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node ) : base ( depth ) { }

        public Visitor (
            [ NotNull ] Panel        container
          , Func < object , object > findResource
          , CodeAnalyseContext       b
          , SemanticModel            model
          , SyntaxWalkerDepth        depth = SyntaxWalkerDepth.Trivia
        ) : base ( depth )
        {
            if ( container == null )
            {
                throw new ArgumentNullException ( nameof ( container ) ) ;
            }

            try
            {
                _findResource = findResource ?? container.TryFindResource ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }

            _b     = b ;
            _model = model ;
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

        public Dispatcher Dispatch { get ; set ; }

        public override void VisitToken ( SyntaxToken token )
        {
            var x = token.Kind ( ).ToString ( ) ;
            var m = Regex.Match ( x , "[a-z]+([A-Z][a-z]*)$" ) ;
            var style = m.Groups[ 1 ].Captures[ 0 ].Value ;
            AddToken ( token , token.Kind ( ) ) ;
            ActiveSpans.Remove ( token ) ;
            //base.VisitToken ( token ) ;
        }

        private void SetTokenSpan (
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

        public TextSpan TokenSpan { get ; set ; }

        private void RecordLocation ( Location getLocation )
        {
            var line = getLocation.GetLineSpan ( ).StartLinePosition.Line ;
            if ( line > CurLine )
            {
                CurLine     = line ;
                StartOfLine = true ;
            }
        }

        public bool StartOfLine { get ; set ; }

        public int CurLine { get ; set ; }

        private void RecordSpan ( in TextSpan tokenSpan ) { }

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

            SetTokenSpan (
                          token.Span
                        , token.GetLocation ( )
                        , token
                        , new TokenSpanObject ( token.Span , token )
                         ) ;
            LogManager.GetCurrentClassLogger ( ).Warn ( "{s}" , token ) ;
            var text = token.ToString ( ) ;
            var tokenSpan = token.Span ;
            Dispatch.Invoke (
                             ( ) => {
                                 _CreateTokenItem ( style , text , tokenSpan ) ;
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

        private void _CreateTokenItem ( object style , string Text , TextSpan textSpan )
        {
            var run = creATErUN ( Text ) ;
            run.Style = ( Style ) _findResource ( style ) ;
            var spanToolTip = new SpanToolTip ( ) ;
            var tt = new SpanTT ( spanToolTip ) ;
            run.ToolTip = tt ;
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

                tt.Spans = spans ;
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

        private void SetSpan ( in TextSpan tokenFullSpan ) { currentSpan = tokenFullSpan ; }

        public TextSpan currentSpan { get ; set ; }

        public TextBlock CurBlock { get ; set ; }

        public override void VisitTrivia ( SyntaxTrivia trivia )
        {
            return ;
            var run = RenderTrivia ( trivia.ToString ( ) , trivia.Kind ( ) ) ;

            // CurBlock.Inlines.Add ( run ) ;
            base.VisitTrivia ( trivia ) ;
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
                                                                           ,
                                                                        }
                                                                       )
                    } ;
            return r ;
        }
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
                var resourceKey = "SymbolPart" + symbolDisplayPart.Kind.ToString ( ) ;
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