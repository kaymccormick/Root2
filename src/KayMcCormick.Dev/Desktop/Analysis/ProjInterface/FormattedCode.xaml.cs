using System;
using System.Collections.Generic ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;
using NLog ;
using ProjLib ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode : UserControl
    {
        public FormattedCode ( )
        {
            InitializeComponent ( ) ;
        }

        /// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            base.OnPropertyChanged ( e ) ;
            if ( e.Property.Name == "Tag" )
            {
                try
                {

                    TextBlock block = ( TextBlock ) this.Content ;
                    block.Text = "" ;
                    CodeAnalyseContext exx = ( CodeAnalyseContext ) this.Tag ;

                    CSharpSyntaxRewriter rewriter = new MyRewriter(exx.CurrentModel,  new CodeSource("input"), exx.CurrentRoot);
                    var newNode = rewriter.Visit ( exx.Node ) ;

                    CodeAnalyseContext newContext = new CodeAnalyseContext (
                                                                            exx.CurrentModel
                                                                          , exx.CurrentRoot
                                                                          , exx.Statement
                                                                          , newNode
                                                                          , exx.Document
                                                                           ) ;
                    // LogUsages.FindLogUsages(exx.Document, exx.CurrentRoot, exx.CurrentModel, ConsumeLogInvocation, false, false, ProcessInvocation);
                    var statementSyntax = exx.Node ;
                    if ( statementSyntax == null )
                    {
                        throw new Exception ( "no st" ) ;
                    }

                    if ( DoSym )
                    {
                        Z z = new Z ( block , statementSyntax.FullSpan , TryFindResource ) ;
                        var enclosingSymbol =
                            exx.CurrentModel.GetEnclosingSymbol ( statementSyntax.SpanStart ) ;
                        z.Visit ( enclosingSymbol ) ;
                    }

                    if ( DoVisit )
                    {
                        Visitor x = new Visitor (
                                                 block
                                               , TryFindResource
                                               , newContext
                                               , SyntaxWalkerDepth.Trivia
                                                ) ;
                        x.Visit ( statementSyntax ) ;
                    }
                }
                catch ( Exception ex )
                {
                    MessageBox.Show ( ex.ToString ( ) , "Error" ) ;
                    LogManager.GetCurrentClassLogger().Error ( ex , ex.ToString ( ) ) ;
                }
            }
        }

        private void ProcessInvocation (
            InvocationParms invocationParms
        )
        {
            LogUsages.ProcessInvocation ( invocationParms ) ;
        }

        private void ConsumeLogInvocation ( LogInvocation obj )
        {
            ByNode[ obj.Node ] = obj ;
            ByStatement[ obj.Statement ] = obj ;
        }

        public Dictionary<object, LogInvocation> ByNode { get; } = new Dictionary<object, LogInvocation>();
        public Dictionary<object, LogInvocation> ByStatement{ get; } = new Dictionary<object, LogInvocation>();

        public bool DoSym { get ; set ; }

        public bool DoVisit { get ; set ; } = true ;
    }

    public class MyRewriter : CSharpSyntaxRewriter
    {
        private SemanticModel model ;
        private ICodeSource document ;
        private CompilationUnitSyntax currentRoot ;
        private INamedTypeSymbol exceptionType ;

        public MyRewriter ( SemanticModel model , ICodeSource document , CompilationUnitSyntax currentRoot , bool visitIntoStructuredTrivia = false ) : base ( visitIntoStructuredTrivia )
        {
            this.model = model ;
            this.document = document ;
            this.currentRoot = currentRoot ;
            this.exceptionType = this.model.Compilation.GetTypeByMetadataName("System.Exception");
        }


        /// <summary>Called when the visitor visits a InvocationExpressionSyntax node.</summary>
        public override SyntaxNode VisitInvocationExpression ( InvocationExpressionSyntax node )
        {
            if ( LogUsages.CheckInvocationExpression ( node, out var methodSymbol, model ) )
            {
                LogInvocation logInvocation = null ;
                LogUsages.ProcessInvocation (
                                             new InvocationParms (
                                                                  null
                                                                , currentRoot
                                                                , model
                                                                , document
                                                                , node.AncestorsAndSelf ( )
                                                                      .OfType < StatementSyntax
                                                                       > ( )
                                                                      .First ( )
                                                                , node
                                                                , methodSymbol
                                                                , exceptionType
                                                                , invocation => {
                                                                      logInvocation = invocation ;
                                                                  }
                                                                 )
                                            ) ;
                                 
                return node.WithAdditionalAnnotations (
                                                       new[]
                                                       {
                                                           new SyntaxAnnotation (
                                                                                 "LogInvocation"
                                                                               , JsonConvert.SerializeObject(logInvocation))
                                                           
                                                       }
                                                      ) ;
            }

            return node ;
        }
    }
}