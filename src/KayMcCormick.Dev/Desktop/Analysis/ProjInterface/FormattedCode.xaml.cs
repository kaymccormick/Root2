using System;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents ;
using System.Windows.Media ;
using CodeAnalysisApp1 ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;
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
        public TaskFactory _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning | TaskCreationOptions.HideScheduler, TaskContinuationOptions.AttachedToParent, TaskScheduler.Default);
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public FormattedCode (String code ) : this()
        {
            main.Text = code ;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.UserControl" /> class.</summary>
        public FormattedCode ( ) {
            InitializeComponent();
            
        }

        /// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            LogManager.GetCurrentClassLogger().Info("{m} {p}", nameof(OnPropertyChanged), e.Property);
            base.OnPropertyChanged ( e ) ;

            var eNewValue = e.NewValue ;
            if (  e.Property.Name == "Tag" )
            {
                LogManager.GetCurrentClassLogger().Info(nameof(OnPropertyChanged));
                Logger.Info ( "Starting transform" ) ;
                //tasks.Add ( _taskFactory.StartNew ( ( ) =>
                PerformTransform ( eNewValue ) ;
            }

        //               ( ) => {
            //                   if ( e.Property.Name == "Tag" )
            //                   {
            //                       try
            //                       {
            //                           return TransformCodeAsync ( ) ;
            //                       }`
            //                       catch ( Exception ex )
            //                       {
            //                           LogManager
            //                              .GetCurrentClassLogger ( )
            //                              .Error ( ex , ex.ToString ( ) ) ;
            //                           if ( UiError ) MessageBox.Show ( ex.ToString ( ) , "Error" ) ;
            //                       }
            //                   }
            //               }
            //              ) ;
        }

        public ConcurrentBag<Task> tasks { get ; set ; } = new ConcurrentBag < Task > ();

        private void PerformTransform ( object value  )
        {
            tasks.Add(
                      _taskFactory.StartNew (
                                             TransformCodeAsync
                                           , ( object ) Tuple.Create (apanel
                                                                
                                                                    , ( CodeAnalyseContext )
                                                                      value
                                                                    , this
                                                                     )
                                            )
                     ) ;
        }

        public bool UiError { get ; set ; }

        private static async Task TransformCodeAsync (object o )
        {
            Tuple < StackPanel , CodeAnalyseContext, FormattedCode > t = ( Tuple < StackPanel , CodeAnalyseContext, FormattedCode > ) o ;
            StackPanel panel = t.Item1 ;
            LogManager.GetCurrentClassLogger ( ).Info ( nameof ( TransformCodeAsync ) ) ;
            CodeAnalyseContext exx = t.Item2 ;

            if ( exx != null ) {
                if ( t.Item3 != null ) {
                    CSharpSyntaxRewriter rewriter = new LogUsagesRewriter (
                                                                           exx.SyntaxTree
                                                                         , exx.CurrentModel
                                                                         , new CodeSource ( "input" )
                                                                         , exx.CurrentRoot, t.Item3.Progress
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
            

                    // LogUsages.FindLogUsages(exx.Document, exx.CurrentRoot, exx.CurrentModel, ConsumeLogInvocation, false, false, ProcessInvocation);
                    exx = newContext ;
                    var statementSyntax = exx.Node ;
                    if ( statementSyntax == null )
                    {
                        throw new Exception ( "no st" ) ;
                    }

                    if ( true)
                    {
                        panel.Dispatcher.Invoke ( ( ) => panel.Children.Clear ( ) ) ;
                        Visitor x = new Visitor ( ) ;
                        x.Visit ( statementSyntax ) ;
                        Logger.Info ( "done" ) ;
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

     
    }
}