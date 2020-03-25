using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Media ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using ProjLib.Interfaces ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode : UserControl , IFormattedCode
    {
        // ReSharper disable once UnusedMember.Local
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public FormattedCode ( ) { InitializeComponent ( ) ; }

        private string                _sourceCode ;
        private SyntaxTree            _syntaxTree ;
        private SemanticModel         _model ;
        private CompilationUnitSyntax _compilationUnitSyntax ;

        public FlowDocument FlowViewerDocument { get ; private set ; }

        public MyFlowDocumentScrollViewer FlowViewer { get ; private set ; }

        public string SourceCode { get => _sourceCode ; set => _sourceCode = value ; }

        public SyntaxTree SyntaxTree { get => _syntaxTree ; set => _syntaxTree = value ; }

        public SemanticModel Model { get => _model ; set => _model = value ; }

        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get => _compilationUnitSyntax ;
            set => _compilationUnitSyntax = value ;
        }

        public async Task<object>  Refresh ( )
        {
            FlowViewer = new MyFlowDocumentScrollViewer ( ) ;
            
            SetCurrentValue(ContentProperty, FlowViewer) ;
            FlowViewerDocument = new FlowDocument ( ) ;
            
            // RichTextBox richTextBox = new RichTextBox ( ) ;// 
            // Content = richTextBox ;
            FlowViewer.SetCurrentValue(FlowDocumentScrollViewer.DocumentProperty, FlowViewerDocument) ;
            FlowViewerDocument.SetCurrentValue(FlowDocument.FontSizeProperty, (double)24) ;
            FlowViewerDocument.SetCurrentValue(FlowDocument.FontFamilyProperty, new FontFamily ( "Lucida Console" )) ;

            return await VisitAsync ( ) ;
        }

        public async Task<object> VisitAsync ( )
        {
            var visitor3 = new Visitor3 ( FlowViewerDocument , FlowViewer ) ;
            var root = await SyntaxTree.GetRootAsync ( ) ;
            return new object();
            // var flowViewerScrollViewer = ( UIElement ) ( FlowViewer?.ScrollViewer ) ?? ( FlowViewer ) ;
            // var adornerLayer = AdornerLayer.GetAdornerLayer ( FlowViewer ) ;
            // adornerLayer?.Add ( new LineNumberAdorner ( flowViewerScrollViewer ) ) ;
        }
    }

    internal class LineNumberAdorner : Adorner
    {
        [ NotNull ] private readonly UIElement _adornedElement ;

        public LineNumberAdorner ( [ NotNull ] UIElement adornedElement ) : base ( adornedElement )
        {
            _adornedElement = adornedElement ;
        }

        #region Overrides of UIElement
        protected override void OnRender ( DrawingContext drawingContext )
        {
            var adornedRect = new Rect ( _adornedElement.DesiredSize ) ;
            LogManager.GetCurrentClassLogger ( ).Info ( "r: {r}" , adornedRect ) ;
            
            FlowDocumentScrollViewer s = new MyFlowDocumentScrollViewer ( ) ;
        }
        #endregion
    }
}
