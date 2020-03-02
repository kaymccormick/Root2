using System ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Media ;

using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.Text ;
using Newtonsoft.Json ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    /// <summary>
    /// Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode : UserControl
    {
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
            // flowViewer.LayoutUpdated += ( sender , args ) => )
            // )
            // Content                       = FlowViewer ;
            // FlowViewerDocument            = new FlowDocument ( ) ;
            RichTextBox richTextBox = new RichTextBox ( ) ;// { FlowViewer.Document = FlowViewerDocument } ;
            Content = richTextBox ;
            //FlowViewer.Document  ;         = FlowViewerDocument ;
            FlowViewerDocument.FontSize   = 24 ;
            FlowViewerDocument.FontFamily = new FontFamily ( "Lucida Console" ) ;

            return await VisitAsync ( ) ;
        }

        private async Task<object> VisitAsync ( )
        {
            var visitor3 = new Visitor3 ( FlowViewerDocument , FlowViewer ) ;
            visitor3.DefaultVisit ( SyntaxTree.GetRoot ( ) ) ;
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

    public class MyFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Visual
        protected override void OnVisualChildrenChanged (
            DependencyObject visualAdded
          , DependencyObject visualRemoved
        )
        {
            Logger.Debug ( "{added}" , visualAdded ) ;
            if ( visualAdded is Visual v )
            {
                var pp = v.PointToScreen ( new Point ( 0 , 0 ) ) ;
                Logger.Debug ( "point is {pp}", pp ) ;
            }

            base.OnVisualChildrenChanged ( visualAdded , visualRemoved ) ;
        }
        #endregion
        #region Overrides of UIElement
        protected override void OnRender ( DrawingContext drawingContext )
        {
            base.OnRender ( drawingContext ) ;
            
        }
        #endregion

        /// <summary>
        /// Backing store for the <see cref="ScrollViewer"/> property.
        /// </summary>
        private ScrollViewer scrollViewer ;

        public bool doOverrideMeasure { get ; set ; }

        /// <summary>
        /// Gets the scroll viewer contained within the FlowDocumentScrollViewer control
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if ( scrollViewer == null )
                {
                    DependencyObject obj = this ;

                    do
                    {
                        if ( VisualTreeHelper.GetChildrenCount ( obj ) > 0 )
                        {
                            obj = VisualTreeHelper.GetChild ( obj as Visual , 0 ) ;
                        }
                        else
                        {
                            return null ;
                        }
                    }
                    while ( ! ( obj is ScrollViewer ) ) ;

                    scrollViewer = obj as ScrollViewer ;
                }

                return scrollViewer ;
            }
        }

        protected override Size MeasureOverride ( Size availableSize )
        {
            if ( ! doOverrideMeasure )
            {
                return base.MeasureOverride ( availableSize ) ;
            }

            var panelDesiredSize = new Size ( ) ;
            if ( double.IsInfinity ( availableSize.Height ) )
            {
                panelDesiredSize.Height = 924 ;
            }
            else
            {
                panelDesiredSize.Height = availableSize.Height - 100 ;
            }

            if ( double.IsInfinity ( panelDesiredSize.Width ) )
            {
                panelDesiredSize.Width = 1000 ;
            }
            else
            {
                panelDesiredSize.Width = availableSize.Width ;
            }

            return panelDesiredSize ;
        }
    }
}
