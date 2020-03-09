#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjTests
// ProjTests.cs
// 
// 2020-02-21-12:38 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.ExceptionServices ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Windows ;
using AnalysisControls ;
using AnalysisFramework ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Layouts ;
using ProjInterface ;
using ProjLib ;
using Xunit ;
using Xunit.Abstractions ;
using FormattedCode = AnalysisControls.FormattedCode ;
using LogLevel = NLog.LogLevel ;

namespace ProjTests
{

    /// GlobalLoggingFixture" />
    [CollectionDefinition( "GeneralPurpose")]
    [UsedImplicitly]
    public class GeneralPurpose : ICollectionFixture<GlobalLoggingFixture>
    {
    }

    [Collection ( "GeneralPurpose" ) ]
    [ ClearLoggingRules ]
    #if VSSETTINGS
    [ LoggingRule ( typeof ( VsCollector ) ,             nameof ( LogLevel.Info ) ) ]
#endif
    [ LoggingRule ( typeof ( DefaultObjectIdProvider ) , nameof ( LogLevel.Warn ) ) ]
    [ LoggingRule ( typeof ( ProjTests ) ,               nameof ( LogLevel.Trace ) ) ]
    [ LoggingRule ( "*" ,                                nameof ( LogLevel.Info ) ) ]
    [BeforeAfterLogger]
    public class ProjTests : IClassFixture < LoggingFixture >
      , IClassFixture < ProjectFixture >
      , IDisposable
    {
        private string code =
            "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing NLog ;\n\nnamespace LogTest\n{\n    class Program\n    {\n        private static readonly  Logger Logger = LogManager.GetCurrentClassLogger();\n        static void Main(string[] args) {\n            Action<string> xx = Logger.Info;\n            xx(\"hi\");\n            Logger.Debug ( \"Hello\" ) ;\n            try {\n                string xxx = null;\n                var q = xxx.ToString();\n            } catch(Exception ex) {\n                Logger.Info(ex, ex.Message);\n            }\n            var x = Logger;\n            // doprocess\n            x.Info(\"hello {test} {ab}\", 123, 45);\n        }\n\n    }\n}\n" ;

        public SyntaxTree TestSyntaxTree
        {
            get
            {
                if ( _testSyntaxTree == null )
                {
                    _testSyntaxTree = CSharpSyntaxTree.ParseText ( code ) ;
                }

                return _testSyntaxTree ;
            }
        }


        public CSharpCompilation Compilation
        {
            get
            {
                if ( _compilation == null )
                {
                    _compilation = CreateCompilation ( TestSyntaxTree , "TestSyntaxTree" ) ;
                }

                return _compilation ;
            }
        }

        public CSharpCompilation CreateCompilation ( SyntaxTree syntaxTree , string assemblyName )
        {
            var compilation = CSharpCompilation.Create ( assemblyName )
                                               .AddReferences (
                                                               MetadataReference.CreateFromFile (
                                                                                                 typeof
                                                                                                     ( string
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )
                                                             , MetadataReference.CreateFromFile (
                                                                                                 typeof
                                                                                                     ( Logger
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )
                                                              )
                                               .AddSyntaxTrees ( syntaxTree ) ;
            return compilation ;
        }


        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private static   Logger            Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly ITestOutputHelper _output ;
        private readonly LoggingFixture    _loggingFixture ;
        private          SyntaxTree        _testSyntaxTree ;
        private          CSharpCompilation _compilation ;
        private List<Action> _finalizers = new List < Action > ();

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests (
            ITestOutputHelper output
          , LoggingFixture    loggingFixture
          , ProjectFixture    projectFixture
        )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException ;
            _output                                      =  output ;
            _loggingFixture                              =  loggingFixture ;
            //VSI                                          =  projectFixture.I ;
            loggingFixture.SetOutputHelper ( output , this ) ;
            _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
        }

        [Fact]
        public void remote1 ( )
        {
        }
        [ WpfFact ]
        public void TestViewModel1 ( )
        {
            Logger.Debug ( "heelo" ) ;

#if false
            var utempDir = Path.Combine ( @"e:\scratch\projtests" , "temp" ) ;
            var x = Path.GetRandomFileName ( ) ;
            var tempDir = Path.Combine ( utempDir , x ) ;
            var cloneDir = Path.Combine ( tempDir , "clone" ) ;
            var info = Directory.CreateDirectory ( tempDir ) ;
#if false
            this.Finalizers.Add (
                                 ( ) => {
                                     foreach ( var enumerateFileSystemEntry in Directory
                                        .EnumerateFileSystemEntries (
                                                                     tempDir
                                                                   , "*"
                                                                   , SearchOption.AllDirectories
                                                                    ) )
                                     {
                                         if ( File.Exists ( enumerateFileSystemEntry ) )
                                         {
                                             try
                                             {
                                                 File.SetAttributes(enumerateFileSystemEntry, FileAttributes.Normal);

                                                 var fileInfo = new FileInfo(enumerateFileSystemEntry) ;
                                                 fileInfo.Delete();
                                                 // File.Delete ( enumerateFileSystemEntry ) ;
                                             }
                                             catch ( Exception ex )
                                             {
                                                 Logger.Warn ( ex , ex.ToString() ) ;
                                             }
                                         }
                                     }
                                 }
                                ) ;
#endif
                Logger.Info ( "tempdir is {tempDir}" , tempDir ) ;
            var cloneOptions = new CloneOptions ( ) ;
            cloneOptions.OnCheckoutProgress = ( path , steps , totalSteps )
                => Logger.Debug (
                                 "Checkout progress: {path} ( {steps} / {totalSteps} )"
                               , path
                               , steps
                               , totalSteps
                                ) ;
            cloneOptions.RepositoryOperationStarting = context => {
                Logger.Info ( "{a} {b}" , context.ParentRepositoryPath , context.RemoteUrl ) ;
                return true ;
            } ;
            //cloneOptions.OnUpdateTips = ( name , id , newId ) => 
            cloneOptions.RepositoryOperationCompleted =
                context => Logger.Info ( context.ToString ( ) ) ;
            cloneOptions.OnTransferProgress = progress => {
                Logger.Debug ("Transfer Progress: {IndexedObjects} {x} {y} {z}", progress.IndexedObjects, progress.TotalObjects,progress.ReceivedBytes , progress.ReceivedObjects ) ;
                return true ;
            } ;
            cloneOptions.OnProgress = output => {
                Logger.Debug ( output ) ;
                return true ;
            } ;
            
            Repository.Clone (
                              "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                            , cloneDir
                      /*      , cloneOptions*/
                             ) ;
            var dd = new DirectoryInfo ( cloneDir ) ;
            var f = Directory.EnumerateFiles (
                                              cloneDir
                                            , "KayMcCormick.dev.sln"
                                            , SearchOption.AllDirectories
                                             )
                             .ToList ( ) ;
            Assert.NotEmpty ( f ) ;
            foreach ( var s in f )
            {
                Logger.Info ( s ) ;
            }
#endif
            #if MSBUILDLOCATOR  
            var instances = MSBuildLocator.QueryVisualStudioInstances()
                                          .Where(
                                                 (instance, i)
                                                     => instance.Version.Major    == 16
                                                        && instance.Version.Minor == 4
                                                );
            MSBuildLocator.RegisterInstance(instances.First());
#endif
            var scope = InterfaceContainer.GetContainer ( ) ;
            var viewModel = scope.Resolve < IWorkspacesViewModel > ( ) ;
            viewModel.AnalyzeCommand(viewModel.ProjectBrowserViewModel.RootCollection.OfType<IProjectBrowserNode>().First());
#if false
            var vsInstance = viewModel.VsCollection.First (instance => instance.InstallationVersion.StartsWith("16.4.")) ;
            var mruItem = vsInstance.MruItems.Skip ( 1 ).First ( item => item.Exists ) ;
            Assert.NotNull ( viewModel.PipelineViewModel.Pipeline ) ;
            var i = MSBuildLocator
                   .QueryVisualStudioInstances (
                                                new VisualStudioInstanceQueryOptions ( )
                                                {
                                                    DiscoveryTypes =
                                                        DiscoveryType.VisualStudioSetup
                                                  , WorkingDirectory = @"c:\\temp\work1"
                                                }
                                               )
                   .Where (
                           instance => instance.VisualStudioRootPath == vsInstance.InstallationPath
                          )
                   .First ( ) ;

            var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos" ;
            var solution = @"V3\TestCopy\src\KayMcCormick.Dev\KayMcCormick.Dev.sln" ;
            //solution = @"V2\LogTest\LogTest.sln";

            
            var spath = Path.Combine ( root , solution ) ;
            Logger.Debug("posting {file}", f[0]);
            viewModel.PipelineViewModel.Pipeline.PipelineInstance.Post (f[0]) ;
#endif

            #if false
            viewModel.PipelineViewModel.Pipeline.PipelineInstance.Completion.ContinueWith (
                                                                                           task => {
                                                                                               if (
                                                                                                   task
                                                                                                      .IsFaulted
                                                                                               )
                                                                                               {
                                                                                                   Logger
                                                                                                      .Fatal ( task
                                                                                                                  .Exception
                                                                                                    , "Faulted with {ex}"
                                                                                                     , task
                                                                                                      .Exception
                                                                                                       ) ;
                                                                                               }
                                                                                           }
                                                                                          )
                     .Wait ( TimeSpan.FromMinutes ( 5 )) ;
            #endif
        }

        [Fact]
        public void DeserializeLog ( )
        {
            var ctx = AnalysisService.Parse ( LibResources.Program_Parse , "test" ) ;
            LogEventInfo info1 = LogEventInfo.Create(LogLevel.Debug, "test", "test");
            info1.Properties[ "node" ] = ctx.CompilationUnit ;
            
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add ( new LogEventInfoConverter ( ));
            options.Converters.Add (new JsonSyntaxNodeConverter()) ;

            var json = JsonSerializer.Serialize ( info1 , options ) ;
            Logger.Info ( json ) ;
            var info2 = JsonSerializer.Deserialize < LogEventInfo > ( json , options ) ;
            return ;
            
            var t = File.OpenText ( @"C:\data\logs\ProjInterface.json" ) ;
            while ( ! t.EndOfStream )
            {
                var line = t.ReadLine ( ) ;

                LogEventInfo info = JsonSerializer.Deserialize < LogEventInfo > ( line, options ) ;
                Logger.Debug(info.FormattedMessage);
                foreach ( var keyValuePair in info.Properties )
                {
                    Logger.Debug ( keyValuePair.Key ) ;
                    Logger.Debug ( keyValuePair.Value ) ;
                }
            }
        }
        public List<Action> Finalizers { get { return _finalizers ; } set { _finalizers = value ; } }

//        public //VisualStudioInstance VSI { get ; set ; }

        /// <summary>Tests application of configuration in the app.config file.</summary>
        /// <autogeneratedoc />d ndfajdsad
        /// TODO Edit XML Comment Template for TestApplyConfiguration
#if false
        [Fact]
        public async Task TestProject( )
        {
            Assert.NotNull(VSI);
            var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos";
            var p = Path.Combine(root, @"V2\LogTest\LogTest.sln");
            Assert.True(File.Exists(p));
            ProjLib.ProjectHandler v =
                new ProjectHandler(p, VSI);
            await v.LoadAsync ( );
            v.ProcessProject += ( workspace , project ) => {
                Logger.Debug ( "project is {project}" , project.Name ) ;
            } ;
            v.ProcessDocument += document => {
                Logger.Debug (
                              "Document: {doc} {sourcecode}"
                , document.Name
                , document.SourceCodeKind
                             ) ;
            } ;
            await v.ProcessAsync ( ) ;

        }
        [ Theory ]
        //[InlineData( @"V2\WpfApp\WpfApp.sln")]
        [ InlineData ( @"V3\copy\src\KayMcCormick.Dev\KayMcCormick.dev.sln" ) ]
        [ InlineData ( @"V2\LogTest\LogTest.sln" ) ]
        public async Task<object> TestProject2 ( string p1 )
        {
            Assert.NotNull ( VSI ) ;
            var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos" ;
            var solution = @"V2\WpfApp\WpfApp.sln" ;
            solution = @"V2\LogTest\LogTest.sln" ;
            var p = Path.Combine ( root , p1 ) ;
            Assert.True ( File.Exists ( p ) ) ;
            var projectHandlerImpl =
 new ProjectHandlerImpl ( p , VSI, SynchronizationContext.Current) ;
            await projectHandlerImpl.LoadAsync ( ) ;
            projectHandlerImpl.ProcessProject += ( workspace , project ) => {
                Logger.Debug ( "project is {project}" , project.Name, Dispatcher.CurrentDispatcher) ;
            } ;
            projectHandlerImpl.ProcessDocument += document => {
                Logger.Trace (
                              "Document: {doc} {sourcecode}"
                        , document.Name
                        , document.SourceCodeKind
                             ) ;
            } ;
            Func<Tuple<SyntaxTree, SemanticModel, CompilationUnitSyntax>,
                WorkspacesViewModel.CreateFormattedCodeDelegate2> d = t
                => new WorkspacesViewModel.CreateFormattedCodeDelegate2(() => new FormattedCode());

            await projectHandlerImpl.ProcessAsync ( invocation => { }, SynchronizationContext.Current, d ) ;
            foreach ( var yy in projectHandlerImpl.OutputList )
            {
                Logger.Info (
                             "{item1} {item2} {item3}"
                       , yy.Item1
                       , yy.Item2
                       , string.Join ( ";" , yy.Item3.Select ( tuple => tuple.Item2 ) )
                            ) ;
            }

            foreach ( var inv in projectHandlerImpl.Invocations )
            {
                Logger.Error (
                              "{path} {line} {msgval} {list}"
                        , inv.SourceLocation
                        , inv.MethodSymbol.Name
                        , inv.Msgval
                             ) ;
            }
        }

        [ WpfTheory ]
        [ InlineData ( @"V2\LogTest\LogTest.sln" , "LogTest" , "Program.cs" ) ]
        public void TestProject3 ( string p1 , string proj , string doc )
        {
            Task.WaitAll ( Command_ ( p1 , proj , doc, Container.GetScope() ) ) ;
        }

        private async Task Command_ (
            string         p1
      , string         proj
      , string         doc
      , ILifetimeScope scope
        )
        {
            Assert.NotNull ( VSI ) ;
            var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos" ;
            var p = Path.Combine ( root , p1 ) ;
            Assert.True ( File.Exists ( p ) ) ;
            // getScope.Resolve < ProjectHandlerImpl > ( ) ;
            var projectHandlerImpl =
 new ProjectHandlerImpl ( p , VSI, SynchronizationContext.Current) ;
            await projectHandlerImpl.LoadAsync ( ) ;
            projectHandlerImpl.ProcessProject += ( workspace , project ) => {
                Logger.Debug ( "project is {project}" , project.Name ) ;
            } ;
            projectHandlerImpl.ProcessDocument += document => {
                Logger.Trace ( "Document: {doc} {sourcecode}" , document.Name , document.SourceCodeKind ) ;
            } ;
            var theDocument = projectHandlerImpl.Workspace.CurrentSolution.Projects
                                                .Single ( project => project.Name             == proj )
                                                .Documents.Single ( document => document.Name == doc ) ;
            await projectHandlerImpl.OnPrepareProcessDocumentAsync ( theDocument ).ConfigureAwait ( true ) ;
            Action < LogInvocation > consumeLogInvocation = invocation => {
                var container = new StackPanel ( ) { Orientation = Orientation.Vertical } ;

                var visitor = scope.Resolve < Visitor2 > ( ) ;
                visitor.Visit ( invocation.Statement ) ;
            } ;
            await projectHandlerImpl.OnProcessDocumentAsync ( theDocument , consumeLogInvocation )
                                    .ConfigureAwait ( true ) ;
            foreach ( var yy in projectHandlerImpl.OutputList )
            {
                Logger.Info (
                             "{item1} {item2} {item3}"
                       , yy.Item1
                       , yy.Item2
                       , string.Join ( ";" , yy.Item3.Select ( tuple => tuple.Item2 ) )
                            ) ;
            }
        }

        [ WpfFact ]
        public void TestSyyles ( )
        {
            var x = new ResourceDictionary ( ) ;
            foreach ( var xx in Enum.GetValues ( typeof ( SyntaxKind ) ) )
            {
                var s = new Style ( ) ;
                s.Setters.Add ( new Setter ( Control.ForegroundProperty , Brushes.Black ) ) ;
                Logger.Info ( "{x}" , s ) ;
                x.Add ( xx.ToString ( ) , s ) ;
            }

            Logger.Info ( "{}" , XamlWriter.Save ( x ) ) ;
        }

        [ WpfFact ]
        public void TestStyles2 ( )
        {
            var x = new ResourceDictionary ( ) ;
            foreach ( var xx in Enum.GetValues ( typeof ( SyntaxKind ) ) )
            {
                var s = new Style ( ) ;
                s.Setters.Add ( new Setter ( Control.ForegroundProperty , Brushes.Black ) ) ;
                s.Setters.Add ( new Setter ( Control.BackgroundProperty , Brushes.White ) ) ;
                Logger.Info ( "{x}" , s ) ;
                x.Add ( xx , s ) ;
            }

            File.WriteAllText ( "Resources.xaml" , XamlWriter.Save ( x ) ) ;
        }
#endif
        private void CurrentDomainOnFirstChanceException (
            object                        sender
          , FirstChanceExceptionEventArgs e
        )
        {
            HandleInnerExceptions ( e ) ;
        }

        private void HandleInnerExceptions ( FirstChanceExceptionEventArgs e )
        {
            try
            {
                var msg = $"{e.Exception}" ;
#if false
                try
                {

                    if ( _output != null )
                    {
                        _output.WriteLine ( "First chance exception: " + msg ) ;
                    }
                } catch(InvalidOperationException invalid)
                {
                    Logger.Trace ( invalid.ToString ) ;
                }
                Logger.Error(msg);
#endif
                Debug.WriteLine ( "Exception: " + e.Exception ) ;
                var inner = e.Exception.InnerException ;
                var seen = new HashSet < object > ( ) ;
                while ( inner != null
                        && ! seen.Contains ( inner ) )
                {
#if false
Logger.Error(inner, inner.ToString);
#endif

                    Debug.WriteLine ( "Exception: " + e.Exception ) ;
                    seen.Add ( inner ) ;
                    inner = inner.InnerException ;
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( "Exception: " + ex ) ;
            }
        }

        [ WpfFact ]
        public void TestContainer ( )
        {
            var scope = ProjLibContainer.GetScope ( ) ;
            // var q1= scope.Resolve<IEnumerable<ISourceCode>>();
            // foreach ( var sourceCode in q1 )
            // {
            // Logger.Trace ( "SourceCode is {sourceCode}" , sourceCode.SourceiviCode ) ;
            // }


            var fmt = scope.Resolve < IEnumerable < IHasLogInvocations > > ( ) ;
            foreach ( var q in fmt )
            {
                Logger.Info ( "tre is {x}" , q.LogInvocationList ) ;
            }

            Assert.NotNull ( fmt ) ;
        }

        [ WpfFact ]
        public void TestFormattedCodeControl()
        {
            var codeControl = new FormattedCode();
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = LibResources.Program_Parse;
            codeControl.SourceCode = sourceText;
            var w = new Window();
            w.Content = codeControl;

            var context = AnalysisService.Parse(sourceText, "test1");
            var (syntaxTree, model, compilationUnitSyntax) = context;
            Logger.Info("Context is {Context}", context);
            codeControl.SyntaxTree            = syntaxTree;
            codeControl.Model                 = model;
            codeControl.CompilationUnitSyntax = compilationUnitSyntax;
            codeControl.Refresh();

            // var argument1 = XamlWriter.Save ( codeControl.FlowViewerDocument );
            // File.WriteAllText ( @"c:\data\out.xaml", argument1 ) ;
            // Logger.Info ( "xaml = {xaml}" , argument1 ) ;
            // var tree = Transforms.TransformTree ( context.SyntaxTree ) ;
            // Logger.Info ( "Tree is {tree}" , tree ) ;
            w.ShowDialog();
        }

        [WpfFact]
        public void TestFormattedCodeControl2()
        {
            var codeControl = new FormattedCode2();
            var w = new Window();
            w.Content = codeControl;
           
            Task t = new Task ( ( ) => { } ) ;
            w.Closed += ( sender , args ) => {
                t.Start ( ) ;
            } ;
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = LibResources.Program_Parse;
            codeControl.SourceCode = sourceText;
            
             var context = AnalysisService.Parse(sourceText, "test1");
            var (syntaxTree, model, compilationUnitSyntax) = context;
            Logger.Info("Context is {Context}", context);
            codeControl.SyntaxTree            = syntaxTree;
            codeControl.Model                 = model;
            codeControl.CompilationUnitSyntax = compilationUnitSyntax;
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Task.Run ( ( ) => codeControl.Refresh ( ) )
                .ContinueWith (
                               task => {
                                   tcs.SetResult ( true ) ;

                               }
                              ) ;

            w.Show();
            tcs.Task.Wait ( ) ;

            // var argument1 = XamlWriter.Save ( codeControl.FlowViewerDocument );
            // File.WriteAllText ( @"c:\data\out.xaml", argument1 ) ;
            // Logger.Info ( "xaml = {xaml}" , argument1 ) ;
            // var tree = Transforms.TransformTree ( context.SyntaxTree ) ;
            // Logger.Info ( "Tree is {tree}" , tree ) ;

        }
        [ WpfFact ]
        public void TestCommand ( )
        {
            // AppDomain.CurrentDomain.FirstChanceException += ( sender , args ) => {
            // HandleInnerExceptions ( args) ;

            // } ;

            var transform = new TransformScope ( code , new FormattedCode ( ) , new Visitor2 ( ) ) ;
            Logger.Info ( "Transform is {transform}" , transform ) ;
            var w = new Window ( ) { } ;
            Logger.Info ( Process.GetCurrentProcess ( ).Id ) ;
            var fmt = transform.FormattedCodeControl as IFormattedCode;
            fmt.SourceCode = transform.SourceCode ;
            w.Content      = fmt ;
            // var mi = new MakeInfo ( fmt , transform.SourceCode ) ;
            // Assert.NotNull ( mi ) ;
            // var task = fmt.TaskFactory.StartNew(ProjUtils.MakeFormattedCode, mi
            // , CancellationToken.None, 
            // TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning

            // ,
            // TaskScheduler.Default
            // ) ;
            // fmt.tasks.Add(task);
            // w.ShowDialog();
            // Task.WaitAll ( fmt.tasks.ToArray()) ;
            // TaskCompletionSource <bool> tcs = new TaskCompletionSource < bool > ();
            // w.Closed += ( sender , args ) => tcs.TrySetResult ( true ) ;
            // Logger.Info ( "{xaml}" , XamlWriter.Save ( f.Content ) ) ;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///
        /// resources.</summary>
        ///
        [ Fact ]
        public void TestSerialize ( ) { }

        [ Fact ]
        public void TestRewrite ( )
        {
            var comp = Compilation ;
            var tree = TestSyntaxTree ;
            var codeAnalyseContext = new CodeAnalyseContext (
                                                             comp.GetSemanticModel ( tree )
                                                           , null
                                                           , tree.GetRoot ( )
                                                           , new CodeSource ( "Test Source" )
                                                           , TestSyntaxTree
                                                            ) ;
            // var logUsagesRewriter = new LogUsagesRewriter (
                                                           // TestSyntaxTree
                                                         // , codeAnalyseContext.CurrentModel
                                                         // , codeAnalyseContext.Document
                                                         // , codeAnalyseContext.CurrentRoot
                                                         // , ( node , span )
                                                               // => Logger.Info ( "{span}" , span )
                                                          // ) ;
            // var syntaxNode = logUsagesRewriter.Visit ( tree.GetRoot ( ) ) ;
            // var s = new StringWriter ( ) ;
            // using ( var fileStream = File.OpenWrite ( @"out.cs" ) )
            // {
                // syntaxNode.WriteTo ( new StreamWriter ( fileStream ) ) ;
                // s.Close ( ) ;
            // }
        }


        public void Dispose ( )
        {
            // _loggingFixture?.Dispose ( ) ;

            foreach ( var finalizer in _finalizers )
            {
                try
                {
                    finalizer ( ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , ex.ToString ( ) ) ;
                }
            }
            _loggingFixture.SetOutputHelper(null);
        }

        [ WpfFact ]
        public void FormattdCode1 ( ) { }
    }
}
