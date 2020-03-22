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
using System.Globalization ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Reflection ;
using System.Runtime.ExceptionServices ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Runtime.Serialization.Formatters.Soap ;
using System.Text.Json ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using AnalysisControls ;
using AnalysisFramework ;
using Autofac ;
using AvalonDock ;
using AvalonDock.Controls ;
using AvalonDock.Layout ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.SharePoint.Client ;
using Moq ;
using NLog ;
using NLog.Layouts ;
using ProjInterface ;
using ProjInterface.JSON ;
using ProjLib ;
using Xunit ;
using Xunit.Abstractions ;
using Application = System.Windows.Application ;
using File = System.IO.File ;

namespace ProjTests
{
    [ CollectionDefinition ( "GeneralPurpose" ) ]
    [ UsedImplicitly ]
    public class GeneralPurpose : ICollectionFixture < GlobalLoggingFixture >
      , ICollectionFixture < AppFixture >
    {
    }

    [ Collection ( "GeneralPurpose" ) ]
    [ ClearLoggingRules ]
#if VSSETTINGS
    [ LoggingRule ( typeof ( VsCollector ) ,             nameof ( LogLevel.Info ) ) ]
#endif
    [ LoggingRule ( typeof ( DefaultObjectIdProvider ) , nameof ( LogLevel.Warn ) ) ]
    [ LoggingRule ( typeof ( ProjTests ) ,               nameof ( LogLevel.Trace ) ) ]
    [ LoggingRule ( "*" ,                                nameof ( LogLevel.Info ) ) ]
    [ BeforeAfterLogger ]
    public sealed class ProjTests : IClassFixture < LoggingFixture >
      , IClassFixture < ProjectFixture >
      , IDisposable
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private static readonly Logger Logger          = LogManager.GetCurrentClassLogger ( ) ;
        private static readonly bool   _disableLogging = true ;

        static ProjTests ( ) { LogHelper.DisableLogging = _disableLogging ; }

        private readonly                    ITestOutputHelper _output ;
        private readonly                    LoggingFixture    _loggingFixture ;
        [ UsedImplicitly ] private readonly ProjectFixture    _projectFixture ;
        private readonly                    AppFixture        _appFixture ;
        private                             ILifetimeScope    _testScope ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests (
            ITestOutputHelper output
          , LoggingFixture    loggingFixture
          , ProjectFixture    projectFixture
          , AppFixture        appFixture
        )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException ;
            _output                                      =  output ;
            _loggingFixture                              =  loggingFixture ;
            _projectFixture                              =  projectFixture ;
            _appFixture                                  =  appFixture ;

            if ( ! _disableLogging )
            {
                loggingFixture?.SetOutputHelper ( output , this ) ;

                _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
            }
        }

        [ Fact ]
        public void Test1 ( )
        {
            var clientContext = new ClientContext (
                                                   "https://satoridev.sharepoint.com/sites/Dev/SitePages/DevHome.aspx"
                                                  ) ;

            clientContext.AuthenticationMode = ClientAuthenticationMode.Default ;
            clientContext.Credentials = new NetworkCredential (
                                                               "110102a3-a114-4204-a2b9-16419682bc0c"
                                                             , "frvewVuRhCJTZleUy0CiHo4FUYXsp3xjn6xBIkpNhwk="
                                                              ) ;
            var oWebsite = clientContext.Web ;
            clientContext.Load ( oWebsite ) ;
            clientContext.ExecuteQuery ( ) ;
            // WebCollection w = client.Web.Webs;
            // foreach ( var web in w )
            // {
            //     _output.WriteLine(string.Join ( ", " , web.Folders.Select ( ( folder , i ) => folder.Name ) ) );
            // }
            _output.WriteLine ( "Title: {0} Created: {1}" , oWebsite.Title , oWebsite.Created ) ;
        }

        [ WpfFact ]
        public void TestProxyUtils ( )
        {
            Action < string > x = message => Debug.WriteLine ( message ) ;
            var proxy = ProxyUtilsBase.CreateProxy (
                                                    x
                                                  , new BaseInterceptorImpl (
                                                                             x
                                                                           , new ProxyGenerator ( )
                                                                            )
                                                   ) ;
            Assert.NotNull ( proxy ) ;
            var r = proxy.TransformXaml ( new Button { Content = "Hello" } ) ;
        }

        [ WpfFact ]
        public void TestFE ( )
        {
            var f = new FrameworkElementFactory ( ) ;
            f.Type = typeof ( Button ) ;
            f.AppendChild ( new FrameworkElementFactory ( typeof ( TextBlock ) , "Hello" ) ) ;

            MethodInfo m1 = null ;
            MethodInfo m2 = null ;
            foreach ( var methodInfo in f.GetType ( )
                                         .GetMethods (
                                                      BindingFlags.Instance | BindingFlags.NonPublic
                                                     ) )
            {
                if ( methodInfo.Name == "InstantiateUnoptimizedTree" )
                {
                    m1 = methodInfo ;
                }

                if ( methodInfo.Name                        == "Seal"
                     && methodInfo.GetParameters ( ).Length == 1 )
                {
                    m2 = methodInfo ;
                }
            }

            m2.Invoke ( f , new object[] { new ControlTemplate ( ) } ) ; //
            var r = m1.Invoke (
                               f
                             , BindingFlags.NonPublic | BindingFlags.Instance
                             , null
                             , Array.Empty < object > ( )
                             , CultureInfo.CurrentCulture
                              ) ;


            var p = r.GetType ( )
                     .GetProperty ( "FE" , BindingFlags.Instance | BindingFlags.NonPublic ) ;
            var fe = p.GetValue ( r ) ;
            var w = new Window ( ) ;
            w.Content = fe ;
            w.ShowDialog ( ) ;
            Logger.Info ( r.GetType ( ) ) ;
        }

        [ WpfFact ]
        public void TestResourcesTree1 ( )
        {
            var instance = new ApplicationInstance ( _output.WriteLine ) ;
            instance.AddModule ( new ProjInterfaceModule ( ) ) ;
            instance.Initialize ( ) ;
            var lifetimescope = instance.GetLifetimeScope ( ) ;
            LogManager.ThrowExceptions = true ;
            Logger.Warn ( "in callback" ) ;
            var model = new AllResourcesTreeViewModel (
                                                       lifetimescope
                                                     , lifetimescope
                                                          .Resolve < IObjectIdProvider > ( )
                                                      ) ;
            var tree = new AllResourcesTree ( model ) ;
            var tv = tree.tv ;
            var childcount = CountChildren ( tv ) ;
            Logger.Warn ( $"Child count is {childcount}" ) ;
            Window w = new AppWindow ( lifetimescope ) ;
            w.Content = tree ;
            w.Show ( ) ;

            Assert.NotEmpty ( model.AllResourcesCollection ) ;
            model.AllResourcesCollection.First ( ).IsExpanded = true ;

            Thread.Sleep ( 300 ) ;
            var childcount2 = CountChildren ( tv ) ;
            Logger.Info ( $"Child count is {childcount2}" ) ;
        }

        [ WpfFact ]
        public void TestAdapter ( )
        {
            TestApplication x = new TestApplication();
            
            var instance = new ApplicationInstance ( _output.WriteLine ) ;
            instance.AddModule ( new ProjInterfaceModule ( ) ) ;
            instance.AddModule ( new AnalysisControlsModule ( ) ) ;
            instance.Initialize ( ) ;
            var lifetimescope = instance.GetLifetimeScope ( ) ;
            LogManager.ThrowExceptions = true ;
            var funcs = lifetimescope
               .Resolve < IEnumerable < Func < LayoutDocumentPane , IDisplayableAppCommand > >
                > ( ) ;
            var pane = new LayoutDocumentPane ( ) ;
            foreach ( var func in funcs )
            {
                try
                {
                    Logger.Info ( "func is {func}" , func.ToString ( ) ) ;
                    var xx = func ( pane ) ;
                    Debug.WriteLine ( xx.DisplayName ) ;
                    xx.ExecuteAsync ( )
                      .ContinueWith (
                                     task => {
                                         if ( task.IsFaulted )
                                         {
                                             Debug.WriteLine ( task.Exception ) ;
                                         }
                                         else
                                         {
                                             Debug.WriteLine ( task.Result ) ;
                                         }
                                     }
                                    )
                      .Wait ( ) ;
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.ToString ( ) ) ;
                }

                var m = new DockingManager ( ) ;
                
                

                var group = new LayoutDocumentPaneGroup ( pane ) ;
//LayoutAnchorablePane xx = new LayoutAnchorablePane(group);
                var mLayoutRootPanel = new LayoutPanel ( group ) ;
                LayoutRoot layout = new LayoutRoot();
                layout.RootPanel = mLayoutRootPanel ;
                
                TaskCompletionSource<bool> source = new TaskCompletionSource < bool > ();
                Window w = new AppWindow ( lifetimescope ) ;
                w.Content = m ;
                x.TCS = source ;
                x.Run ( w ) ;
                Task.WaitAll ( x.TCS.Task ) ;
                Debug.WriteLine ( source.Task.Result ) ;

            }
        }

        [ WpfFact ]
        public void TestExceptionInfo ( )
        {
            var @in = File.OpenRead ( @"c:\data\logs\exception.bin" ) ;
            BinaryFormatter f = new BinaryFormatter();
            var exception = (Exception)f.Deserialize ( @in ) ;
            HandleExceptionImpl h = new HandleExceptionImpl();
            h.HandleException ( exception ) ;
            SoapFormatter f2 = new SoapFormatter();
            var  w = File.OpenWrite ( @"C:\data\logs\exception.xml" ) ;
            f2.Serialize(w, exception);
            w.Flush();
            w.Close();
            // try
            // {
            // object[] x = new object[] { null } ;
            // var y = x[ 1 ] ;
            // }
            // catch ( Exception ex )
            // {
            // h.HandleException ( ex ) ;
            // }
        }

        private ProjInterfaceApp CreateProjInterfaceApp ( ) { return _appFixture.InterfaceApp ; }

        private int CountChildren ( DependencyObject tv )
        {
            var count = 1 ;
            foreach ( var child in LogicalTreeHelper.GetChildren ( tv ) )
            {
                count += CountChildren ( ( DependencyObject ) child ) ;
            }

            return count ;
        }

        [ WpfFact ]
        public void TestResourcesModel ( )
        {
            using ( var app = CreateProjInterfaceApp ( ) )
            {
                app.TestCallback = ( app2 , lifetimeScope ) => {
                    var model = new AllResourcesTreeViewModel (
                                                               lifetimeScope
                                                             , lifetimeScope
                                                                  .Resolve < IObjectIdProvider > ( )
                                                              ) ;
                    var tree = new AllResourcesTree ( model ) ;

                    DumpTree ( app , tree , model.AllResourcesCollection ) ;
                    return true ;
                } ;
                app.Run ( ) ;
            }
        }

        private void DumpTree (
            ProjInterfaceApp                 app
          , AllResourcesTree                 tree
          , IEnumerable < ResourceNodeInfo > modelAllResourcesCollection
          , int                              depth = 0
        )
        {
            foreach ( var resourceNodeInfo in modelAllResourcesCollection )
            {
                try
                {
                    var json1 = JsonSerializer.Serialize (
                                                          resourceNodeInfo.Key
                                                        , app.AppJsonSerializerOptions
                                                         ) ;

                    Logger.Debug ( json1 ) ;
                    var json2 = JsonSerializer.Serialize (
                                                          resourceNodeInfo.Data
                                                        , app.AppJsonSerializerOptions
                                                         ) ;
                    Logger.Debug ( json2 ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Error ( ex , ex.Message ) ;
                }

                var selector = new ResourceDetailTemplateSelector ( ) ;
                var dt = selector.SelectTemplate ( resourceNodeInfo.Data , tree ) ;
                if ( dt != null )
                {
                    var xaml = XamlWriter.Save ( dt ) ;
                    Debug.WriteLine ( xaml ) ;
                }

                Logger.Info (
                             "{x}{key} = {data}"
                           , string.Concat ( Enumerable.Repeat ( "  " , depth ) )
                           , resourceNodeInfo.Key
                           , resourceNodeInfo.Data
                            ) ;
                DumpTree ( app , tree , resourceNodeInfo.Children , depth + 1 ) ;
            }
        }

        [ Fact ]
        public void TestModule1 ( )
        {
            var module = new ProjInterfaceModule ( ) ;

            var mock = new Mock < ContainerBuilder > ( ) ;
            mock.Setup ( cb => module.DoLoad ( cb ) ) ;
        }

        [ Fact ]
        public void DeserializeLog ( )
        {
            var ctx = ( ICompilationUnitRootContext ) AnalysisService.Parse (
                                                                             LibResources
                                                                                .Program_Parse
                                                                           , "test"
                                                                            ) ;
            var info1 = LogEventInfo.Create ( LogLevel.Debug , "test" , "test" ) ;
            info1.Properties[ "node" ] = ctx.CompilationUnit ;

            var options = new JsonSerializerOptions ( ) ;
            options.Converters.Add ( new JsonConverterLogEventInfo ( ) ) ;
            options.Converters.Add ( new JsonSyntaxNodeConverter ( ) ) ;

            var json = JsonSerializer.Serialize ( info1 , options ) ;
            Logger.Info ( json ) ;
            LogEventInfo info2 ;
            try
            {
                info2 = JsonSerializer.Deserialize < LogEventInfo > ( json , options ) ;
            }
            catch ( JsonException x )
            {
                var substring = "" ;
                if ( x.BytePositionInLine.HasValue )
                {
                    try
                    {
                        var eBytePositionInLine = ( int ) x.BytePositionInLine.Value - 16 ;
                        if ( eBytePositionInLine < 0 )
                        {
                            eBytePositionInLine = 0 ;
                        }

                        var length = 32 ;
                        var endIndex = eBytePositionInLine + length ;
                        if ( endIndex >= json.Length )
                        {
                            endIndex = json.Length ;
                        }

                        length    = endIndex - eBytePositionInLine ;
                        substring = json.Substring ( eBytePositionInLine , length ) ;
                    }
                    catch ( ArgumentOutOfRangeException )
                    {
                        substring = json ;
                    }

                    Logger.Warn ( "Start of problem is {problem}" , substring ) ;
                }

                throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
            }

            //Assert.Equal ( info1.CallerClassName , info2.CallerClassName ) ;

            var t = File.OpenText ( @"C:\data\logs\ProjInterface.json.test" ) ;
            var lineno = 0 ;
            while ( ! t.EndOfStream )
            {
                lineno += 1 ;
                var line = t.ReadLine ( ) ;

                LogEventInfo info ;
                try
                {
                    info = JsonSerializer.Deserialize < LogEventInfo > ( line , options ) ;
                }
                catch ( JsonException x )
                {
                    var substring = "" ;
                    if ( x.BytePositionInLine.HasValue )
                    {
                        try
                        {
                            var eBytePositionInLine = ( int ) x.BytePositionInLine.Value - 16 ;
                            if ( eBytePositionInLine < 0 )
                            {
                                eBytePositionInLine = 0 ;
                            }

                            var length = 32 ;
                            var endIndex = eBytePositionInLine + length ;
                            if ( endIndex >= line.Length )
                            {
                                endIndex = line.Length ;
                            }

                            length    = endIndex - eBytePositionInLine ;
                            substring = line.Substring ( eBytePositionInLine , length ) ;
                        }
                        catch ( ArgumentOutOfRangeException )
                        {
                            substring = line ;
                        }

                        Logger.Warn (
                                     "Start of problem is line {lineno} {problem}"
                                   , lineno
                                   , substring
                                    ) ;
                    }

                    throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
                }

                Logger.Debug ( info.FormattedMessage ) ;
                foreach ( var keyValuePair in info.Properties )
                {
                    Logger.Debug ( keyValuePair.Key ) ;
                    Logger.Debug ( keyValuePair.Value.ToString ( ) ) ;
                }
            }
        }

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
        public void TestFormattedCodeControl ( )
        {
            var codeControl = new FormattedCode ( ) ;
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = LibResources.Program_Parse ;
            codeControl.SourceCode = sourceText ;
            var w = new Window { Content = codeControl } ;

            var context = ( ISemanticModelContext ) AnalysisService.Parse ( sourceText , "test1" ) ;
            var syntaxTree = context.CurrentModel.SyntaxTree ;
            var model = context.CurrentModel ;
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            Logger.Info ( "Context is {Context}" , context ) ;
            codeControl.SyntaxTree            = syntaxTree ;
            codeControl.Model                 = model ;
            codeControl.CompilationUnitSyntax = compilationUnitSyntax ;
            codeControl.Refresh ( ) ;

            // var argument1 = XamlWriter.Save ( codeControl.FlowViewerDocument );
            // File.WriteAllText ( @"c:\data\out.xaml", argument1 ) ;
            // Logger.Info ( "xaml = {xaml}" , argument1 ) ;
            // var tree = Transforms.TransformTree ( context.SyntaxTree ) ;
            // Logger.Info ( "Tree is {tree}" , tree ) ;
            w.ShowDialog ( ) ;
        }

        [ WpfFact ]
        public void TestFormattedCodeControl2 ( )
        {
            var codeControl = new FormattedCode2 ( ) ;
            var w = new Window { Content = codeControl } ;

            var t = new Task ( ( ) => { } ) ;
            w.Closed += ( sender , args ) => {
                t.Start ( ) ;
            } ;
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = LibResources.Program_Parse ;
            codeControl.SourceCode = sourceText ;

            var context = ( ISemanticModelContext ) AnalysisService.Parse ( sourceText , "test1" ) ;
            var syntaxTree = context.CurrentModel.SyntaxTree ;
            var model = context.CurrentModel ;
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            var tcs = new TaskCompletionSource < bool > ( ) ;
            Task.Run ( ( ) => codeControl.Refresh ( ) )
                .ContinueWith (
                               task => {
                                   tcs.SetResult ( true ) ;
                               }
                              ) ;

            w.Show ( ) ;

            var bmp = new RenderTargetBitmap (
                                              ( int ) w.ActualWidth
                                            , ( int ) w.ActualHeight
                                            , 72
                                            , 72
                                            , PixelFormats.Pbgra32
                                             ) ;
            bmp.Render ( codeControl ) ;
            var pngImage = new PngBitmapEncoder ( ) ;
            pngImage.Frames.Add ( BitmapFrame.Create ( bmp ) ) ;
            using ( Stream fileStream = File.Create ( @"c:\data\test\out.png" ) )
            {
                pngImage.Save ( fileStream ) ;
            }

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
            var transform = new TransformScope (
                                                LibResources.Program_Parse
                                              , new FormattedCode ( )
                                              , new Visitor2 ( )
                                               ) ;
            Logger.Info ( "Transform is {transform}" , transform ) ;
            var w = new Window ( ) ;
            Logger.Info ( Process.GetCurrentProcess ( ).Id ) ;
            var fmt = transform.FormattedCodeControl ;
            fmt.SourceCode = transform.SourceCode ;
            w.Content      = fmt ;
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
            var ctx = AnalysisService.Parse ( LibResources.Program_Parse , "test" ) ;
            var comp = ctx.CompilationUnit ;
            var tree = ctx.CurrentModel.SyntaxTree ;
            var codeAnalyseContext = AnalysisService.Parse ( LibResources.Program_Parse , "test" ) ;
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
            AppDomain.CurrentDomain.FirstChanceException -= CurrentDomainOnFirstChanceException ;
            if ( ! _disableLogging )
            {
                _loggingFixture.SetOutputHelper ( null ) ;
            }
        }
    }

    public class TestApplication : Application
    {

        private TaskCompletionSource < bool > _tcs ;
        public TaskCompletionSource < bool > TCS { get { return _tcs ; } set { _tcs = value ; } }
        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {

            base.OnExit ( e ) ;
            _tcs.TrySetResult ( true ) ;
        }

        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;

        }
        #endregion
    }

    public class UnableToDeserializeLogEventInfo : Exception
    {
        public UnableToDeserializeLogEventInfo ( ) { }

        public UnableToDeserializeLogEventInfo ( string message ) : base ( message ) { }

        public UnableToDeserializeLogEventInfo ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        protected UnableToDeserializeLogEventInfo (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}