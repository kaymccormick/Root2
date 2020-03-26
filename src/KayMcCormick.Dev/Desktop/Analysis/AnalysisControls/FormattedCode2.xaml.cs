using System.Collections.Generic ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using ProjLib ;
using ProjLib.Interfaces ;
using Color = System.Windows.Media.Color ;


namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode2 : UserControl, ICodeRenderer
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        Dictionary <ushort, StyleInfo > ss  = new Dictionary < ushort, StyleInfo > ();
        public FormattedCode2 ( ) {
            InitializeComponent();
            ss[ (ushort)SyntaxKind.PrivateKeyword] =
                new StyleInfo ( ) { fg = new SColor (  255 ,0 , 0 , 255 ) } ;
            ss[ ( ushort ) SyntaxKind.MethodDeclaration ] =
                new StyleInfo ( ) { bg = new SColor ( 127 , 127 , 127 , 255 ) } ;
            //_container = rootPanel ;
            var wrapPanel = new WrapPanel ( ) ;
            Content = wrapPanel ;
            _container = wrapPanel ;
            _visitor3 = new Visitor4 (null, new DispatcherSynchronizationContext ( ), this ) ;
        }


        private string                _sourceCode ;
        private SyntaxTree            _syntaxTree ;
        private SemanticModel         _model ;
        private CompilationUnitSyntax _compilationUnitSyntax ;
        private IAddChild _container ;
        private Visitor4 _visitor3 ;
        private Stack <IAddChild> _stack = new Stack < IAddChild > ();
        private StyleInfo curSi  = new StyleInfo();

        private Color[] colors = new[]
                                 {
                                     Colors.Red , Colors.Green , Colors.Aqua , Colors.BlueViolet
                                   , Colors.Chocolate
                                 } ;

        private int colorI = 0 ;
        public string SourceCode { get => _sourceCode ; set => _sourceCode = value ; }

        public SyntaxTree SyntaxTree { get => _syntaxTree ; set => _syntaxTree = value ; }

        public SemanticModel Model { get => _model ; set => _model = value ; }

        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get => _compilationUnitSyntax ;
            set => _compilationUnitSyntax = value ;
        }

        public void Refresh ( )
        {
         Visit ( ) ;
        }

        private void Visit ( )
        {
            if ( CompilationUnitSyntax != null )
            {
                _visitor3.DefaultVisit ( CompilationUnitSyntax ) ;
            }

            // var flowViewerScrollViewer = ( UIElement ) ( FlowViewer?.ScrollViewer ) ?? ( FlowViewer ) ;
            // var adornerLayer = AdornerLayer.GetAdornerLayer ( FlowViewer ) ;
            // adornerLayer?.Add ( new LineNumberAdorner ( flowViewerScrollViewer ) ) ;
        }

        #region Implementation of ICodeRenderer
        public void addToken ( ushort rawKind , string text , bool newLine )
        {
            //Token token = new Token ( rawKind , text ) ;
            StyleInfo tmpSi = curSi ;
            SolidColorBrush x = null ;
            if ( ss.TryGetValue ( rawKind , out var si ) )
            {
                if(si.fg.HasValue)
                {
                    x = new SolidColorBrush(
                                            Color.FromArgb (
                                                            si.fg.Value.A
                                                          , si.fg.Value.R
                                                          , si.fg.Value.G
                                                          , si.fg.Value.B
                                                           )
                                           ) ;
                }
                tmpSi = curSi.With ( si ) ;
            }

            _container.AddChild ( new Token ( rawKind , text, x, newLine ) ) ;
        }

        public void addTrivia ( int rawKind , string text , bool newLine )
        {
            _container.AddChild(new Token(rawKind, text, null, newLine));
        }

        public void NewLine ( )
        {
            // var p = new WrapPanel ( ) ;
            // _stack.Peek ( )?.AddChild ( p ) ;
            // _container = p ;
        }

        public void StartNode ( SyntaxNode node )
        {
            SolidColorBrush x = null ;
            var bdr = new Border ( ) ;
            
            var c = new WrapPanel ( ) { Tag = node, Margin = new Thickness(2) } ;
            bdr.Child = c;
            bdr.BorderBrush = new SolidColorBrush ( colors[colorI % colors.Length]) ;
            colorI += 1 ;
            bdr.BorderThickness = new Thickness (1 ) ;
            bdr.Margin = new Thickness ( 2 ) ;
            bdr.ToolTip = new ToolTip ( ) { Content = node.Kind ( ) } ;
            //var c2 = new StackPanel ( ) ;
            //c.Children.Add ( c2 ) ;
            _container?.AddChild(bdr);
            _stack.Push(_container);
            _stack.Push ( bdr ) ;
            _container = c;
            
            if ( ss.TryGetValue ( ( ushort ) node.RawKind , out var si ) )
            {
                if(si.bg.HasValue){
                    x = new SolidColorBrush(
                                            Color.FromArgb(
                                                           si.bg.Value.A
                                                         , si.bg.Value.R
                                                         , si.bg.Value.G
                                                         , si.bg.Value.B
                                                          )
                                           );
                }
            }
            if(x != null)
            {
                c.Background = x ;
            }

            Logger.Info ( "{x} {d}" , node.Kind ( ), _stack.Count ) ;
        }

        public void EndNode ( SyntaxNode node )
        {
            IAddChild c1 = _container ;
            _container = _stack.Pop();
            _container = _stack.Pop();

            var n = c1 ;
            SyntaxNode n2 = ( SyntaxNode ) (n as Panel ).Tag ;
            Logger.Info ( "{x} {y}" , n2?.Kind ( ) , node?.Kind ( ) ) ;
//            Debug.Assert ( object.ReferenceEquals(n , node ) );

        }
        #endregion
    }

    internal class StyleInfo
    {
        public SColor? fg ;
        public SColor? bg ;

        public bool italics ;


        public bool bold ;


        public bool underline ;


        public StyleInfo With ( StyleInfo value )
        {
            return new StyleInfo ( ) { bg = value.bg ?? this.bg , fg = value.bg ?? this.fg } ;
        }
    }

    internal struct SColor
    {
        public byte R ;
        public byte G ;
        public byte  B ;
        public byte A ;

        public SColor ( byte i , byte i1 , byte i2 , byte i3 ) { R = i ;
            G = i1 ;
            B = i2 ;
            A = i3 ;
        }
    }
}
