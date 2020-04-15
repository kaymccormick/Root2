using System.Collections.Generic ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using AnalysisAppLib ;
using AnalysisAppLib.XmlDoc ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisControls
{
    /// <summary>
    ///     Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode2 : UserControl , ICodeRenderer
    {
        private static readonly Logger              Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        Stack < IAddChild > _stack = new Stack < IAddChild > ( ) ;
        private readonly        Visitor4            _visitor3 ;

        private readonly Color[] colors =
        {
            Colors.Red , Colors.Green , Colors.Aqua , Colors.BlueViolet , Colors.Chocolate
        } ;

        private readonly StyleInfo curSi = new StyleInfo ( ) ;

        private readonly Dictionary < ushort , StyleInfo > ss =
            new Dictionary < ushort , StyleInfo > ( ) ;

        private CompilationUnitSyntax _compilationUnitSyntax ;
        private IAddChild             _container ;
        private SemanticModel         _model ;


        private string     _sourceCode ;
        private SyntaxTree _syntaxTree ;

        private int colorI ;

        /// <summary>
        /// </summary>
        public FormattedCode2 ( )
        {
            InitializeComponent ( ) ;
            ss[ ( ushort ) SyntaxKind.PrivateKeyword ] =
                new StyleInfo { fg = new SColor ( 255 , 0 , 0 , 255 ) } ;
            ss[ ( ushort ) SyntaxKind.MethodDeclaration ] =
                new StyleInfo { bg = new SColor ( 127 , 127 , 127 , 255 ) } ;
            //_container = rootPanel ;
            var wrapPanel = new WrapPanel ( ) ;
            Content    = wrapPanel ;
            _container = wrapPanel ;
            _visitor3  = new Visitor4 ( null , new DispatcherSynchronizationContext ( ) , this ) ;
        }

        /// <summary>
        /// </summary>
        public string SourceCode { get { return _sourceCode ; } set { _sourceCode = value ; } }

        /// <summary>
        /// </summary>
        public SyntaxTree SyntaxTree { get { return _syntaxTree ; } set { _syntaxTree = value ; } }

        /// <summary>
        /// </summary>
        public SemanticModel Model { get { return _model ; } set { _model = value ; } }

        /// <summary>
        /// </summary>
        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get { return _compilationUnitSyntax ; }
            set { _compilationUnitSyntax = value ; }
        }

        /// <summary>
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        public void addToken ( ushort rawKind , string text , bool newLine )
        {
            //Token token = new Token ( rawKind , text ) ;
            var tmpSi = curSi ;
            SolidColorBrush x = null ;
            if ( ss.TryGetValue ( rawKind , out var si ) )
            {
                if ( si.fg.HasValue )
                {
                    x = new SolidColorBrush (
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

            _container.AddChild ( new Token ( rawKind , text , x , newLine ) ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        public void addTrivia ( int rawKind , string text , bool newLine )
        {
            _container.AddChild ( new Token ( rawKind , text , null , newLine ) ) ;
        }

        /// <summary>
        /// </summary>
        public void NewLine ( )
        {
            // var p = new WrapPanel ( ) ;
            // _stack.Peek ( )?.AddChild ( p ) ;
            // _container = p ;
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        public void StartNode ( [ NotNull ] SyntaxNode node )
        {
            SolidColorBrush x = null ;
            var bdr = new Border ( ) ;

            var c = new WrapPanel { Tag = node , Margin = new Thickness ( 2 ) } ;
            bdr.Child           =  c ;
            bdr.BorderBrush     =  new SolidColorBrush ( colors[ colorI % colors.Length ] ) ;
            colorI              += 1 ;
            bdr.BorderThickness =  new Thickness ( 1 ) ;
            bdr.Margin          =  new Thickness ( 2 ) ;
            bdr.ToolTip         =  new ToolTip { Content = node.Kind ( ) } ;
            //var c2 = new StackPanel ( ) ;
            //c.Children.Add ( c2 ) ;
            _container?.AddChild ( bdr ) ;
            _stack.Push ( _container ) ;
            _stack.Push ( bdr ) ;
            _container = c ;

            if ( ss.TryGetValue ( ( ushort ) node.RawKind , out var si ) )
            {
                if ( si.bg.HasValue )
                {
                    x = new SolidColorBrush (
                                             Color.FromArgb (
                                                             si.bg.Value.A
                                                           , si.bg.Value.R
                                                           , si.bg.Value.G
                                                           , si.bg.Value.B
                                                            )
                                            ) ;
                }
            }

            if ( x != null )
            {
                c.Background = x ;
            }

            Logger.Info ( "{x} {d}" , node.Kind ( ) , _stack.Count ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        public void EndNode ( [ CanBeNull ] SyntaxNode node )
        {
            var c1 = _container ;
            _container = _stack.Pop ( ) ;
            _container = _stack.Pop ( ) ;

            var n = c1 ;
            var n2 = ( SyntaxNode ) ( n as Panel )?.Tag ;
            Logger.Info ( "{x} {y}" , n2?.Kind ( ) , node?.Kind ( ) ) ;
//            Debug.Assert ( object.ReferenceEquals(n , node ) );
        }

        /// <summary>
        /// </summary>
        public void Refresh ( ) { Visit ( ) ; }

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
    }

    internal class StyleInfo
    {
        public SColor ? bg ;


        public bool     bold ;
        public SColor ? fg ;

        public bool italics ;


        public bool underline ;


        [ NotNull ]
        public StyleInfo With ( [ NotNull ] StyleInfo value )
        {
            return new StyleInfo { bg = value.bg ?? bg , fg = value.bg ?? fg } ;
        }
    }

    internal struct SColor
    {
        public byte R ;
        public byte G ;
        public byte B ;
        public byte A ;

        public SColor ( byte i , byte i1 , byte i2 , byte i3 )
        {
            R = i ;
            G = i1 ;
            B = i2 ;
            A = i3 ;
        }
    }
}