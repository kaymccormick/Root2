using System ;
using System.Collections.Generic ;
using System.Text.RegularExpressions ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Markup ;
using System.Windows.Media ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharpExtensions ;

namespace ProjLib
{
    public class Visitor : CSharpSyntaxWalker
    {
        private readonly Func < object , object > _findResource ;
        private readonly LogInvocationBase        _b ;
        private          int                      _i ;
        private          Brush[]                  _colors = new[] { Brushes.Red , Brushes.Yellow } ;

        public override void DefaultVisit ( SyntaxNode node )
        {
            var obj = new SyntaxNodeSpanObject ( node.Span, node ) ;
            SetSyntaxNodeSpan ( node.FullSpan , node , obj ) ;
#pragma warning disable CA1305 // Specify IFormatProvider
            LogManager.GetCurrentClassLogger ( )
                      .Error (
                              "{method}{where}{line}{kind}"
                            , nameof ( DefaultVisit )
                            , ""
                            , node.GetLocation ( ).GetMappedLineSpan ( ).StartLinePosition.Line + 1
                            , node.Kind ( )
                             ) ;
#pragma warning restore CA1305 // Specify IFormatProvider
            base.DefaultVisit ( node ) ;
            Brush bg = Brushes.Transparent , fg = null ;
            var info = _b.CurrentModel.GetSymbolInfo ( node ) ;
            if ( DoSym && info.Symbol != null )
            {
                var z = new Z ( CurBlock , node.FullSpan , _findResource ) ;
                z.Visit ( info.Symbol ) ;
            }

            var x = node.Kind ( ).ToString ( ) ;
            var m = Regex.Match ( x , "([A-Z][a-z]*)$" ) ;
            var style = m.Groups[ 1 ].Captures[ 0 ].Value ;

            var run = creATErUN ( node.ToString ( ) ) ;
            run.Style = ( Style ) _findResource ( node.Kind ( ) ) ;
            if ( run.Style == null )
            {
//                LogManager.GetCurrentClassLogger().Info(style);
            }

            // CurBlock.Inlines.Add ( run );
            ActiveSpans.Remove ( node ) ;
        }

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

        public Visitor (
            TextBlock                block
          , Func < object , object > findResource
          , LogInvocationBase        b
          , SyntaxWalkerDepth        depth = SyntaxWalkerDepth.Trivia
        ) : base ( depth )
        {
            _findResource = findResource ;
            _b            = b ;
            CurBlock      = block ;
        }

        public override void VisitToken ( SyntaxToken token )
        {
          
            Brush bg = Brushes.Transparent , fg = null ;
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
                AddRun ( RenderTrivia ( syntaxTrivia ) , syntaxTrivia.Span ) ;
            }

            SetTokenSpan(
                         token.Span
                       , token.GetLocation()
                       , token
                       , new TokenSpanObject(token.Span, token)
                        );
            LogManager.GetCurrentClassLogger ( ).Warn ( "{s}" , token ) ;
            var run = creATErUN ( token.ToString ( ) ) ;
            run.Style = ( Style ) _findResource ( style ) ;
            run.Style = ( Style ) _findResource ( style ) ;
            var spanToolTip = new SpanToolTip ( ) ;
            var tt = new SpanTT ( spanToolTip ) ;
            run.ToolTip = tt ;
            if ( run.Style == null )
            {
                LogManager.GetCurrentClassLogger ( ).Error ( style.ToString ( ) ) ;
            }

            AddRun ( run , token.Span ) ;
            ActiveSpans.Remove ( token ) ;
            foreach ( var syntaxTrivia in token.TrailingTrivia )
            {
                AddRun ( RenderTrivia ( syntaxTrivia ) , syntaxTrivia.Span ) ;
            }
        }

        private void AddRun ( Run renderTrivia , TextSpan span )
        {
            if ( StartOfLine )
            {
                //renderTrivia.Background = Brushes.Gray ;
                StartOfLine             = false ;
            }

            SpanTT tt = null ;
            if ( renderTrivia.ToolTip == null )
            {
                tt                   = new SpanTT ( new SpanToolTip ( ) ) ;
                renderTrivia.ToolTip = tt ;
            }
            else
            {
                tt = renderTrivia.ToolTip as SpanTT ;
            }

            if ( tt != null )
            {
                foreach ( var activeSpan in ActiveSpans )
                {
                    var key = activeSpan.Key ;
                    var val = activeSpan.Value ;
                    if ( val.Span.OverlapsWith ( span ) )
                    {
                        var valToolTipContent = val.GetToolTipContent();
                        if ( valToolTipContent != null )
                        {
                            tt.CustomToolTip.Add ( valToolTipContent ) ;
                        }
                    }
                }


            }
            CurBlock.Inlines.Add(renderTrivia);
        }

        private void SetSpan ( in TextSpan tokenFullSpan ) { currentSpan = tokenFullSpan ; }

        public TextSpan currentSpan { get ; set ; }

        public TextBlock CurBlock { get ; set ; }

        public override void VisitTrivia ( SyntaxTrivia trivia )
        {
            return ;
            var run = RenderTrivia ( trivia ) ;

            CurBlock.Inlines.Add ( run ) ;
            base.VisitTrivia ( trivia ) ;
        }

        private Run RenderTrivia ( SyntaxTrivia trivia )
        {
            LogManager.GetCurrentClassLogger ( ).Error ( "trivia: {t}" , trivia.Kind ( ) ) ;

            var run = creATErUN ( trivia.ToString ( ) ) ;
            run.Style = ( Style ) _findResource ( trivia.Kind ( ) ) ;
            // run.ToolTip = new ToolTip ( )
            //               {
            //                   Content = new TextBlock ( new Run ( trivia.Kind ( ).ToString ( ) ) )
            //               } ;
            return run ;
        }

        private Run creATErUN ( string toString )
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

    public abstract class SpanObject < T > : ISpanObject
    {
        public T _instance ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SpanObject ( TextSpan span , T instance )
        {
            Span      = span ;
            _instance = instance ;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>

        public T Instance { get => _instance ; set => _instance = value ; }

        public object getInstance ( ) { return Instance ; }

        public TextSpan Span { get ; set ; }

        public abstract UIElement GetToolTipContent();
    }

    public interface ISpanObject
    {
        object getInstance ( ) ;

        TextSpan Span { get ; }

        UIElement GetToolTipContent();
    }

    public class TriviaSpanObject : SpanObject < SyntaxTrivia >
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TriviaSpanObject ( TextSpan span , SyntaxTrivia instance ) : base ( span , instance )
        {
        }

        public override UIElement GetToolTipContent ( )
        {
            var kind = Instance.Kind ().ToString() ;
            return new TextBlock ( ) { Text = kind  } ;
        }
    }


    internal class SyntaxNodeSpanObject : SpanObject < SyntaxNode >
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SyntaxNodeSpanObject ( TextSpan span , SyntaxNode instance ) : base (
                                                                                    span
                                                                                  , instance
                                                                                   )
        {
        }

        public override UIElement GetToolTipContent ( )
        {
            var kind = Instance.Kind ( ).ToString ( ) ;
            return new TextBlock() { Text = kind };
        }
    }

    public class TokenSpanObject : SpanObject < SyntaxToken >
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TokenSpanObject ( TextSpan span , SyntaxToken instance ) : base ( span , instance )
        {
        }

        public override UIElement GetToolTipContent ( )
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal};
            var toolTipContent = new TextBlock ( )
                                 {
                                     Text = Instance.Kind ( ) + " (" + Instance.RawKind + ")"
                                 } ;
            panel.Children.Add ( toolTipContent ) ;
            if(Instance.Value != null) {
                panel.Children.Add ( new Label() { Content = "Value" });
                panel.Children.Add ( new TextBlock ( ) { Text = Instance.ValueText } ) ;
            }
            return panel ;
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

    internal class SpanTT : ToolTip
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ToolTip" /> class. </summary>
        public SpanTT ( SpanToolTip content ) { Content = CustomToolTip = content ; }

        public SpanToolTip CustomToolTip { get ; set ; }
    }

    internal class SpanToolTip : UserControl
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.UserControl" /> class.</summary>
        ///
        public StackPanel Panel { get ; set ; }

        public SpanToolTip ( )
        {
            Content = Panel = new StackPanel ( ) { Orientation = Orientation.Vertical } ;
        }

        public void Add ( UIElement o )
        {
            if ( o != null )
            {
                Panel.Children.Add ( o ) ;
            }
        }
    }
}