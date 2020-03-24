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
using System.Reflection ;
using System.Runtime.ExceptionServices ;
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
using AnalysisAppLib ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls ;
using AnalysisControls.Views ;
using Autofac ;
using AvalonDock ;
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
using KayMcCormick.Lib.Wpf.ViewModel ;
using Microsoft.CodeAnalysis.CSharp ;
using Moq ;
using NLog ;
using NLog.Layouts ;
using ProjInterface ;
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
        private JsonSerializerOptions _testJsonSerializerOptions ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests (
            ITestOutputHelper            output
          , [ CanBeNull ] LoggingFixture loggingFixture
          , ProjectFixture               projectFixture
          , AppFixture                   appFixture
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

                if ( _loggingFixture != null )
                {
                    _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
                }
            }
        }


        [ WpfFact ]
        public void TEstTypesview ( )
        {
            ITypesViewModel viewModel = new TypesViewModel ( ) ;
            TypesView typesView = new TypesView(viewModel);
            Window w = new Window { Content = typesView } ;
            w.ShowDialog ( ) ;
        }
        [ WpfFact ]
        public void TestJsonSerialization ( )
        {
            var w = new Window ( ) ;
            var options = new JsonSerializerOptions ( ) { WriteIndented = true } ;
            options.Converters.Add (new JsonFrameworkElementConverter(_output));
            options.WriteIndented = true ;
            var json = JsonSerializer.Serialize ( w , options ) ;
            File.WriteAllText( @"C:\data\out.json" , json ) ;
            _output.WriteLine ( json ) ;
            var parsed = JsonSerializer.Deserialize < Dictionary < string , JsonElement > > ( json ) ;

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

            if ( m2 != null )
            {
                m2.Invoke ( f , new object[] { new ControlTemplate ( ) } ) ; //
            }

            if ( m1 != null )
            {
                var r = m1.Invoke (
                                   f
                                 , BindingFlags.NonPublic | BindingFlags.Instance
                                 , null
                                 , Array.Empty < object > ( )
                                 , CultureInfo.CurrentCulture
                                  ) ;


                var p = r.GetType ( )
                         .GetProperty ( "FE" , BindingFlags.Instance | BindingFlags.NonPublic ) ;
                if ( p != null )
                {
                    var fe = p.GetValue ( r ) ;
                    var w = new Window { Content = fe } ;
                    w.ShowDialog ( ) ;
                }

                Logger.Info ( r.GetType ( ) ) ;
            }
        }

        [ WpfFact ]
        public void TestResourcesTree1 ( )
        {
            using (
                var instance = new ApplicationInstance ( _output.WriteLine ) )
            {
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
        }

        [ WpfFact ]
        public void TestAdapter ( )
        {
            // var x = new TestApplication ( ) ;
            Debug.WriteLine ( $"{Thread.CurrentThread.ManagedThreadId} projTests" ) ;
            using ( var instance = new ApplicationInstance ( _output.WriteLine ) )
            {
                instance.AddModule ( new ProjInterfaceModule ( ) ) ;
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;
                LogManager.ThrowExceptions = true ;
                var funcs = lifetimescope
                   .Resolve < IEnumerable < Func < LayoutDocumentPane , IDisplayableAppCommand > >
                    > ( ) ;


                var m = new DockingManager ( ) ;

                var pane = new LayoutDocumentPane ( ) ;

                var group = new LayoutDocumentPaneGroup ( pane ) ;

                var mLayoutRootPanel = new LayoutPanel ( group ) ;
                var layout = new LayoutRoot { RootPanel = mLayoutRootPanel } ;
                m.Layout = layout ;
                Window w = new AppWindow ( lifetimescope ) ;
                w.Content = m ;

                foreach ( var func in funcs )
                {
                    try
                    {
                        Debug.WriteLine ( $"func is {func}" ) ;
                        var xx = func ( pane ) ;
                        Debug.WriteLine ( xx.DisplayName ) ;
                        Debug.WriteLine ( $"{Thread.CurrentThread.ManagedThreadId} projTests" ) ;
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
                }

                w.ShowDialog ( ) ;
                // var source = new TaskCompletionSource < bool > ( ) ;
                // x.TCS = source ;
                // x.Run ( w ) ;
                // Task.WaitAll ( x.TCS.Task ) ;
                // Debug.WriteLine ( source.Task.Result ) ;
            }
        }

        [ WpfFact ]
        public void TestExceptionInfo ( )
        {
            var @in = File.OpenRead ( @"c:\data\logs\exception.bin" ) ;
            var f = new BinaryFormatter ( ) ;
            var exception = ( Exception ) f.Deserialize ( @in ) ;
            var h = new HandleExceptionImpl ( ) ;
            h.HandleException ( exception ) ;
            var f2 = new SoapFormatter ( ) ;
            var w = File.OpenWrite ( @"C:\data\logs\exception.xml" ) ;
            f2.Serialize ( w , exception ) ;
            w.Flush ( ) ;
            w.Close ( ) ;
        }

        private ProjInterfaceApp CreateProjInterfaceApp ( ) { return _appFixture.InterfaceApp ; }

        private int CountChildren ( [ NotNull ] DependencyObject tv )
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
            using ( var instance = new ApplicationInstance ( _output.WriteLine ) )
            {
                instance.AddModule ( new ProjInterfaceModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;

                var model = new AllResourcesTreeViewModel (
                                                           lifetimescope
                                                         , lifetimescope
                                                              .Resolve < IObjectIdProvider > ( )
                                                          ) ;
                var tree = new AllResourcesTree ( model ) ;

                DumpTree ( tree , model.AllResourcesCollection ) ;

            }
        
        }

        private void DumpTree (
            AllResourcesTree                             tree
          , [ NotNull ] IEnumerable < ResourceNodeInfo > modelAllResourcesCollection
          , int                                          depth = 0
        )
        {
            foreach ( var resourceNodeInfo in modelAllResourcesCollection )
            {
                try
                {
                    var json1 = JsonSerializer.Serialize (
                                                          resourceNodeInfo.Key
                                                        , TestJsonSerializerOptions
                                                         ) ;

                    Logger.Debug ( json1 ) ;
                    var json2 = JsonSerializer.Serialize (
                                                          resourceNodeInfo.Data
                                                        , TestJsonSerializerOptions
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
                DumpTree ( tree , resourceNodeInfo.Children , depth + 1 ) ;
            }
        }

        public JsonSerializerOptions TestJsonSerializerOptions { get { return _testJsonSerializerOptions ; } set { _testJsonSerializerOptions = value ; } }

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
            try
            {
                var info2 = JsonSerializer.Deserialize < LogEventInfo > ( json , options ) ;
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
                    info = JsonSerializer.Deserialize < LogEventInfo > (
                                                                        line
                                                                        ?? throw new
                                                                            InvalidOperationException ( )
                                                                      , options
                                                                       ) ;
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
                            if ( line        != null
                                 && endIndex >= line.Length )
                            {
                                endIndex = line.Length ;
                            }

                            length = endIndex - eBytePositionInLine ;
                            if ( line != null )
                            {
                                substring = line.Substring ( eBytePositionInLine , length ) ;
                            }
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

                if ( info != null )
                {
                    Logger.Debug ( info.FormattedMessage ) ;
                    foreach ( var keyValuePair in info.Properties )
                    {
                        Logger.Debug ( keyValuePair.Key ) ;
                        Logger.Debug ( keyValuePair.Value.ToString ( ) ) ;
                    }
                }
            }
        }

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
            Logger.Info ( "Context is {Context}" , context.ToString ( ) ) ;
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

            try
            {
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
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex ) ;
            }

            tcs.Task.Wait ( ) ;

            // var argument1 = XamlWriter.Save ( codeControl.FlowViewerDocument );
            // File.WriteAllText ( @"c:\data\out.xaml", argument1 ) ;
            // Logger.Info ( "xaml = {xaml}" , argument1 ) ;
            // var tree = Transforms.TransformTree ( context.SyntaxTree ) ;
            // Logger.Info ( "Tree is {tree}" , tree ) ;
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

    // ReSharper disable once UnusedType.Global
    public class TestApplication : Application
    {
        private TaskCompletionSource < bool > _tcs ;
        public  TaskCompletionSource < bool > TCS { get { return _tcs ; } set { _tcs = value ; } }
        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            _tcs.TrySetResult ( true ) ;
        }

        protected override void OnStartup ( StartupEventArgs e ) { base.OnStartup ( e ) ; }
        #endregion
    }
}