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
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
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
        public FormattedCode (Visitor2 visitor2 = null, TransformArgs args = null )
        {
            _visitor2 = visitor2 ;
            _args = args ;
            _args.FormattedCode = this ;
            _args.RootPanel = rootPanel ;
            InitializeComponent();
        }

        private readonly Visitor2 _visitor2 ;
        private TaskFactory _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning | TaskCreationOptions.HideScheduler, TaskContinuationOptions.AttachedToParent, TaskScheduler.Default);
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private string _sourceCode ;

        public TransformArgs _args ;
        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            LogManager.GetCurrentClassLogger ( )
                      .Info ( "{m} {p}" , nameof ( OnPropertyChanged ) , e.Property ) ;
            base.OnPropertyChanged ( e ) ;

            var eNewValue = e.NewValue ;
            if ( e.Property.Name == "Tag" )
            {
                LogManager.GetCurrentClassLogger ( ).Info ( nameof ( OnPropertyChanged ) ) ;
                Logger.Info ( "Starting transform" ) ;
                //tasks.Add ( _taskFactory.StartNew ( ( ) =>
                PerformTransform ( eNewValue ) ;
            }
        }

        public ConcurrentBag<Task> tasks { get ; set ; } = new ConcurrentBag < Task > ();

        private void PerformTransform ( object value  )
        {
            var transformArgs = _args ;
            // new TransformArgs(rootPanel
            //                                                     
            //                                            , ( CodeAnalyseContext )
            //                                              value
            //                                            , this, this._visitor2
            //                                             ) ;
            tasks.Add(
                      _taskFactory.StartNew (
                                             TransformCodeAsync
                                           , ( object ) transformArgs
                                            )
                     ) ;
        }

        public bool UiError { get ; set ; }

        private static async Task TransformCodeAsync (object o)
        {
            var (stackPanel , context , formattedCode , visitor2) = (TransformArgs)o ;
            StackPanel panel = stackPanel ;
            new LogBuilder ( Logger ).Level ( LogLevel.Info )
                                     .Message ( "{method}" , nameof ( TransformCodeAsync ) )
                                     .Write ( ) ;
            CodeAnalyseContext exx = context ;

            if ( exx != null ) {
                if ( formattedCode!= null ) {
                    CSharpSyntaxRewriter rewriter = new LogUsagesRewriter (
                                                                           exx.SyntaxTree
                                                                         , exx.CurrentModel
                                                                         , new CodeSource ( "input" )
                                                                         , exx.CurrentRoot, formattedCode.Progress
                                                                          ) ;
                    var newNode = rewriter.Visit ( exx.Node ) ;
                    CodeAnalyseContext newContext =  CodeAnalyseContext.FromSyntaxNode(newNode, "rewritten");

                    var annotations =  newNode.DescendantNodesAndTokensAndSelf ( )
                                              .Where ( token => token.HasAnnotations ( "LogInvocation" ) )
                                              .SelectMany ( token => token.GetAnnotations ( "LogInvocation" ) )
                                              .ToList ( ) ; // var root = exx.SyntaxTree.WithRootAndOptions (newNode , new CSharpParseOptions ( ) ) ;

                    File.WriteAllText ("annotations.json",
                                       JsonConvert.SerializeObject ( annotations , Formatting.Indented )
                                      ) ;
            

                    exx = newContext ;
                    var statementSyntax = exx.Node ;
                    if ( statementSyntax == null )
                    {
                        throw new Exception ( "no st" ) ;
                    }

                    if ( true)
                    {
                        panel.Dispatcher.Invoke ( ( ) => panel.Children.Clear ( ) ) ;
                        // Visitor x = new Visitor ( ) ;

                        if ( visitor2 != null )
                        {
                            visitor2.Visit ( statementSyntax ) ;
                            Logger.Info("done");
                        } else
                        {
                            Logger.Error ( "visitor is nuk" ) ;

                        }


                    }
                }
            }
        }

        private void Progress ( SyntaxNode obj, TextSpan span)
        {
            LogManager.GetCurrentClassLogger ( ).Debug ( "{proc}" , nameof ( Progress ) ) ;
            var elementName = obj.Kind ( )
                              + "."
                              + obj.Span ;
            Dispatcher.Invoke (
                               ( ) => {
                                   Logger.Warn("looking for {x}", elementName);
                                   var node = LogicalTreeHelper.FindLogicalNode (
                                                                                 this
                                                                               , elementName
                                                                                ) ;
                                   if ( node == null )
                                   {
                                       Logger.Info ( "cant find it" ) ;
                                       var t = main.Text ;              
                main.Inlines.Clear();
                main.Inlines.Add(new Run(t.Substring(0, span.Start)) { Background = Brushes.Green });
                main.Inlines.Add (
                                  new Run ( t.Substring ( span.Start , span.Length ) )
                                  {
                                      Background = Brushes.Red
                                  }
                                 ) ;
                        main.Inlines.Add(new Run(t.Substring(span.End)));
                                   }
                                   else
                                   {
                                       ( node as Run ).Background = Brushes.Gray ;
                                   }

                                   ;
                               }
                              ) ;
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

        public TaskFactory TaskFactory { get { return _taskFactory ; } }

        public string SourceCode { get { return _sourceCode ; } set { _sourceCode = value ; } }

        public TransformArgs Args { get => _args ; set => _args = value ; }
    }
}