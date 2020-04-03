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
using System.Resources ;
using System.Runtime.ExceptionServices ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Runtime.Serialization.Formatters.Soap ;
using System.Text ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Baml2006 ;
using System.Windows.Controls ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Xaml ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Serialization ;
using AnalysisControls ;
using AnalysisControls.Properties ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using AvalonDock ;
using AvalonDock.Layout ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.JSON ;
using KayMcCormick.Lib.Wpf.View ;
using Microsoft.CodeAnalysis.CSharp ;
using Moq ;
using NLog ;
using NLog.Layouts ;
using Xunit ;
using Xunit.Abstractions ;
using ColorConverter = System.Windows.Media.ColorConverter ;
using XamlReader = System.Windows.Markup.XamlReader ;
using XamlWriter = System.Windows.Markup.XamlWriter ;

namespace ProjTests
{
//    [ CollectionDefinition ( "GeneralPurpose" ) ]
    public class GeneralPurpose : ICollectionFixture < GlobalLoggingFixture >

    {
    }

    [ Collection ( "GeneralPurpose" ) ]
    // [ ClearLoggingRules ]
#if VSSETTINGS
    [ LoggingRule ( typeof ( VsCollector ) ,             nameof ( LogLevel.Info ) ) ]
#endif
    //[ LoggingRule ( typeof ( DefaultObjectIdProvider ) , nameof ( LogLevel.Warn ) ) ]
    // [ LoggingRule ( typeof ( ProjTests ) ,               nameof ( LogLevel.Trace ) ) ]
    // [ LoggingRule ( "*" ,                                nameof ( LogLevel.Info ) ) ]
    // [ BeforeAfterLogger ]
    public sealed class ProjTests 
        // : IClassFixture < LoggingFixture >
      // , IClassFixture < ProjectFixture >
      : IDisposable
    {
        private static readonly Logger Logger          = LogManager.GetCurrentClassLogger ( ) ;
        private static readonly bool   _disableLogging = true ;

        static ProjTests ( ) { LogHelper.DisableLogging = _disableLogging ; }

        private readonly ITestOutputHelper _output ;
        private readonly LoggingFixture    _loggingFixture ;
        private readonly ProjectFixture    _projectFixture ;

        private ILifetimeScope        _testScope ;
        private JsonSerializerOptions _testJsonSerializerOptions ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests (
            ITestOutputHelper            output
          // , [ CanBeNull ] LoggingFixture loggingFixture
          // , ProjectFixture               projectFixture
        )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException ;
            _output                                      =  output ;
            // _loggingFixture                              =  loggingFixture ;
            // _projectFixture                              =  projectFixture ;


            if ( ! _disableLogging )
            {
                // loggingFixture?.SetOutputHelper ( output , this ) ;

                // if ( _loggingFixture != null )
                // {
                    // _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
                // }
            }
        }

        [ WpfFact ]
        public void TestRead ( )
        {
            var x = XamlReader.Load ( new FileStream(@"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel.xaml", FileMode.Open ) ) ;
//            var json = JsonSerializer.Serialize ( x ) ;
        }
        [ WpfFact ]
        public void TEstTypesview ( )
        {
            TypesViewModel viewModel = new TypesViewModel ( ) ;
            viewModel.BeginInit();
            viewModel.EndInit();
            var stringWriter = new StringWriter() ;
            using ( XmlWriter x = XmlWriter.Create (stringWriter
                                
                                                    
                                                  , new XmlWriterSettings ( ) { Indent = true }
                                                   ) )
            { 
                XamlWriter.Save ( viewModel, x ) ;
                x.Flush();
            }

                
                XamlWriter.Save (viewModel, File.CreateText (
                                                             @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel.xaml"
                                                            ) ); 
            


            return;
            var typesView = new TypesView ( viewModel ) ;
            var w = new Window { Content = typesView } ;
            w.ShowDialog ( ) ;
        }

        [ Fact ]
        public void TestDoc ( )
        {
            // var xml = "<Summary xmlns=\"clr-namespace:AnalysisControls.ViewModel;"
                      // + "assembly=AnalysisControls\">Hello</Summary>" ;
            // var s1 = XamlReader.Parse ( xml ) ;
            // _output.WriteLine(XamlWriter.Save(s1));
            Summary s = new Summary();
            var p = new Para ( new XmlDocText ( "hello" ) ) ;
            s.DocumentElementCollection.Add (p) ;
            var x = XamlWriter.Save ( s ) ;
            _output.WriteLine(x);
        }
        [ WpfFact ]
        public void TEstTypesview2 ( )
        {
            TypesViewModel viewModel = new TypesViewModel ( ) ;
            viewModel.BeginInit ( ) ;
            viewModel.EndInit ( ) ;
            var stringWriter = new StringWriter ( ) ;
            using ( XmlWriter x = XmlWriter.Create (
                                                    stringWriter
                                                  , new XmlWriterSettings ( ) { Indent = true }
                                                   ) )
            {
                XamlWriter.Save ( viewModel , x ) ;
                x.Flush ( ) ;
            }


            XamlWriter.Save (
                             viewModel.Root
                           , File.CreateText (
                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\Types.xaml"
                                             )
                            ) ;



        }

        [WpfFact ]
        public void TestJsonSerialization ( )
        {
            var w = new Window ( ) ;
            var options = new JsonSerializerOptions { WriteIndented = true } ;
            var xaml = XamlWriter.Save ( w ) ;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xaml);
            Utf8JsonWriter writer = new Utf8JsonWriter(new MemoryStream());
            MyXmlWriter xxMyXmlWriter = new MyXmlWriter(writer);
            XamlWriter.Save ( w , xxMyXmlWriter ) ;
            XDocument xdoc = new XDocument();
            XDocument.Parse ( xaml ) ;

            

            foreach ( var xNode in xdoc.Nodes ( ) )
            {
                switch ( xNode )
                {
                    case XComment xComment : break ;
                    case XCData xcData : break ;
                    case XDocument xDocument :
                        writer.WriteStartObject();
                        writer.WriteString( "Kind" , nameof ( XDocument ) ) ;
                        break ;

                    case XElement xElement :

                    case XContainer xContainer : break ;
                    case XDocumentType xDocumentType : break ;
                    case XProcessingInstruction xProcessingInstruction : break ;
                    case XText xText : break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( xNode ) ) ;
                }
            }
            options.Converters.Add ( new JsonFrameworkElementConverter ( _output ) ) ;
            options.WriteIndented = true ;
            var json = JsonSerializer.Serialize ( w , options ) ;
            File.WriteAllText ( @"C:\data\out.json" , json ) ;
            _output.WriteLine ( json ) ;
            var parsed =
                JsonSerializer.Deserialize < Dictionary < string , JsonElement > > ( json ) ;
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
#if false
        [ WpfFact ]
        public void TestResourcesTree1 ( )
        {
            using ( var instance =
                new ApplicationInstance (
                                         new ApplicationInstance.ApplicationInstanceConfiguration ( _output.WriteLine , ApplicationGuid )
                                        ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
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

#endif
        public Guid ApplicationGuid { get ; set ; } = new Guid ( "d4870a23-f1ad-4618-b955-6b342c6afab6" ) ;

        [ WpfFact ]
        public void TestAdapter ( )
        {
            // var x = new TestApplication ( ) ;
            Debug.WriteLine ( $"{Thread.CurrentThread.ManagedThreadId} projTests" ) ;
            using (var instance =
                new ApplicationInstance(ApplicationInstance.CreateConfiguration(_output.WriteLine, ApplicationGuid)))
            {
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
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


        private int CountChildren ( [ NotNull ] DependencyObject tv )
        {
            var count = 1 ;
            foreach ( var child in LogicalTreeHelper.GetChildren ( tv ) )
            {
                count += CountChildren ( ( DependencyObject ) child ) ;
            }

            return count ;
        }
#if false
        [ WpfFact ]
        public void TestResourcesModel ( )
        {
            using ( var instance =
                new ApplicationInstance (
                                         new ApplicationInstance.ApplicationInstanceConfiguration ( _output.WriteLine , ApplicationGuid )
                                        ) )
            {
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;

                foreach ( var myJsonLayout in LogManager
                                             .Configuration.AllTargets
                                             .OfType < TargetWithLayout > ( )
                                             .Select ( t => t.Layout )
                                             .OfType < MyJsonLayout > ( ) )
                {
                    var jsonSerializerOptions = myJsonLayout.Options ;
                    var options = new JsonSerializerOptions ( ) ;
                    foreach ( var jsonConverter in jsonSerializerOptions.Converters )
                    {
                        options.Converters.Add ( jsonConverter ) ;
                    }

                    JsonConverters.AddJsonConverters ( options ) ;
                    myJsonLayout.Options = options ;
                }

                var model = new AllResourcesTreeViewModel (
                                                           lifetimescope
                                                         , lifetimescope
                                                              .Resolve < IObjectIdProvider > ( )
                                                          ) ;
                var tree = new AllResourcesTree ( model ) ;

                DumpTree ( tree , model.AllResourcesCollection ) ;
            }
        }
#endif

        [ WpfFact(Timeout = 30000) ]
        public void Test123 ( )
        {
            // AppLoggingConfigHelper.Performant = true ;
            // for (int i = 0 ; i < 100; i++ )
            // {
                // AppLoggingConfigHelper.EnsureLoggingConfiguredAsync ( message => { } )
                                      // .ContinueWith ( task => AppLoggingConfigHelper.Shutdown ( ) )
                                      // .Wait ( ) ;
            // }
        }
        [WpfFact]
        private void Dump1 ( )
        {
            JsonImageConverterFactory factory = new JsonImageConverterFactory();
            LineGeometry x = new LineGeometry ( new Point ( 0 , 0 ) , new Point ( 10 , 10 ) ) ;
            GeometryDrawing y = new GeometryDrawing (
                                                     new SolidColorBrush ( Colors.Blue )
                                                   , new Pen (
                                                              new SolidColorBrush ( Colors.Green ), 2
                                                             )
                                                   , x
                                                    ) ;
            ImageSource xx = new DrawingImage ( y ) ;
            var jsonConverter =
                (JsonConverter<ImageSource>)factory.CreateConverter ( xx.GetType ( ) , new JsonSerializerOptions ( ) ) ;
            var memoryStream = new MemoryStream ( ) ;
            var jsonwriterxx = new JsonWriterOptions();
            var jsonWriterOptions = new JsonSerializerOptions( ) ;
            var utf8JsonWriter = new Utf8JsonWriter (
                                                     memoryStream
                                                   , jsonwriterxx
                                                    ) ;
            jsonConverter.Write (
                                 utf8JsonWriter, xx, jsonWriterOptions
                                ) ;
            utf8JsonWriter.Flush();
            memoryStream.Flush();
            byte[] bytes = new byte[memoryStream.Length] ;
            memoryStream.Seek ( 0 , SeekOrigin.Begin ) ;
            var read = memoryStream.Read ( bytes , 0 , ( int ) memoryStream.Length ) ;
            var json = Encoding.UTF8.GetString ( bytes ) ;




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

        public JsonSerializerOptions TestJsonSerializerOptions
        {
            get { return _testJsonSerializerOptions ; }
            set { _testJsonSerializerOptions = value ; }
        }

        [ Fact ]
        public void TestModule1 ( )
        {
            var module = new AnalysisAppLibModule ( ) ;

            var mock = new Mock < ContainerBuilder > ( ) ;
            mock.Setup ( cb => module.DoLoad ( cb ) ) ;
        }

        [ Fact ]
        public void DeserializeLog ( )
        {
            var ctx = ( ICompilationUnitRootContext ) AnalysisService.Parse (
                                                                             Resources.Program_Parse
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
        public void TestFormattedCodeControl2 ( )
        {
            var codeControl = new FormattedCode2 ( ) ;
            var w = new Window { Content = codeControl } ;

            var t = new Task ( ( ) => { } ) ;
            w.Closed += ( sender , args ) => {
                t.Start ( ) ;
            } ;
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = Resources.Program_Parse ;
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
            var ctx = AnalysisService.Parse ( Resources.Program_Parse , "test" ) ;
            var comp = ctx.CompilationUnit ;
            var tree = ctx.CurrentModel.SyntaxTree ;
            var codeAnalyseContext = AnalysisService.Parse ( Resources.Program_Parse , "test" ) ;
            // var syntaxNode = logUsagesRewriter.Visit ( tree.GetRoot ( ) ) ;
            // var s = new StringWriter ( ) ;
            // using ( var fileStream = File.OpenWrite ( @"out.cs" ) )
            // {
            // syntaxNode.WriteTo ( new StreamWriter ( fileStream ) ) ;
            // s.Close ( ) ;
            // }
        }

        [ Fact ]
        public void TestColorConverter ( )
        {
            var c = new ColorConverter ( ) ;
        }

        public delegate void RoutedExecutionResultEventHandler (
            object                         sender
          , RoutedExecutionResultEventArgs e
        ) ;

        public static readonly RoutedEvent ExecutionResultEvent =
            EventManager.RegisterRoutedEvent (
                                              "ExecutionResult"
                                            , RoutingStrategy.Direct
                                            , typeof ( RoutedExecutionResultEventHandler )
                                            , typeof ( PythonViewModel )
                                             ) ;


        [ Fact ]
        public void TestPython1 ( )
        {
            using ( var instance =
                new ApplicationInstance (
                                         new ApplicationInstance.ApplicationInstanceConfiguration ( _output.WriteLine , ApplicationGuid )
                                        ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;
                var p = lifetimescope.Resolve < PythonViewModel > ( ) ;

                var options = JsonConverters.CreateJsonSerializeOptions ( ) ;
                var json = JsonSerializer.Serialize ( p , options ) ;
                Debug.WriteLine ( json ) ;
                var vs = DependencyPropertyHelper.GetValueSource (
                                                                  p
                                                                , PythonViewModel.InputLineProperty
                                                                 ) ;
                Logger.Debug ( "Value source is {vs}" ,             vs ) ;
                Logger.Debug ( "Current inputline is {inputLine}" , p.InputLine ) ;
                var pInputLine = "\"hello\"" ;
                p.SetCurrentValue ( PythonViewModel.InputLineProperty , pInputLine ) ;
                //p.InputLine = pInputLine ;
                p.TakeLine ( p.InputLine ) ;
                Assert.Equal ( "" , p.InputLine ) ;
                p.HistoryUp ( ) ;
                Assert.Equal ( pInputLine , p.InputLine ) ;
            }
        }

        public void Dispose ( )
        {
            // _loggingFixture?.Dispose ( ) ;
            AppDomain.CurrentDomain.FirstChanceException -= CurrentDomainOnFirstChanceException ;
            // if ( ! _disableLogging )
            // {
                // _loggingFixture.SetOutputHelper ( null ) ;
            // }
        }

        [ WpfFact ]
        public void TestExceptionUserControl ( )
        {
            var w = new Window ( ) ;
            Exception ex = new AggregateException (
                                                   new ArgumentException (
                                                                          "Boo"
                                                                        , "param"
                                                                        , new
                                                                              InvalidOperationException ( )
                                                                         )
                                                 , new InvalidOperationException ( "boo2" )
                                                  ) ;
            var dd = new ExceptionDataInfo
                     {
                         Exception = ex , ParsedExceptions = Utils.GenerateParsedException ( ex )
                     } ;

            w.Content = new ExceptionUserControl { DataContext = dd } ;
            w.ShowDialog ( ) ;
        }

        [ WpfFact ]
        public void TestView1 ( )
        {
            using ( var instance =
                new ApplicationInstance (
                                         new ApplicationInstance.ApplicationInstanceConfiguration ( _output.WriteLine , ApplicationGuid )
                                        ) )
            {
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;

                var xx = new BinaryFormatter ( ) ;
                var ee = new Exception ( ) ;
                var s = new MemoryStream ( ) ;
                xx.Serialize ( s , ee ) ;
                s.Flush ( ) ;
                s.Seek ( 0 , SeekOrigin.Begin ) ;
                var bytes = new byte[ s.Length ] ;
                var sLength = ( int ) s.Length ;
                var read = s.Read ( bytes , 0 , sLength ) ;

                var view = lifetimescope.Resolve < EventLogView > ( ) ;
                Assert.NotNull ( view.ViewModel ) ;
                var w = new Window { Content = view } ;
                w.ShowDialog ( ) ;
            }
        }

        [ WpfFact ]
        public void TEstWriteBrush ( )
        {
            var brushConverter = new JsonBrushConverter ( ) ;
            var opt = new JsonSerializerOptions ( ) ;
            opt.Converters.Add ( brushConverter ) ;
            var brush = new SolidColorBrush ( Colors.Blue ) ;
            var json = JsonSerializer.Serialize ( brush , opt ) ;
            Logger.Info ( "json is {json}" , json ) ;
            var b = new LinearGradientBrush (
                                             Colors.Blue
                                           , Colors.Green
                                           , new Point ( 0 ,  0 )
                                           , new Point ( 10 , 10 )
                                            ) ;
            Debug.WriteLine ( JsonSerializer.Serialize ( b , opt ) ) ;
        }


        [ Fact ]
        public void TestModel ( )
        {
            var t = new TypesViewModel ( ) ;
        }


        [ Fact ]
        public void TestXmlDoc ( )
        {
            var x = TypesViewModel.LoadDoc ( ) ;
            var y = TypesViewModel.DocMembers ( x ) ;
            foreach ( var codeElementDocumentation in y.Select ( TypesViewModel.HandleDocElement ) )
            {
                if ( codeElementDocumentation != null )
                {
                    Debug.WriteLine ( codeElementDocumentation ) ;
                }
            }

            // var x = TypesViewModel.LoadXmlDoc ( ) ;
            // foreach ( var keyValuePair in x )
            // {
            // foreach ( var valuePair in keyValuePair.Value.MethodDocumentation )
            // {
            // foreach ( var methodDocInfo in valuePair.Value )
            // {
            // Debug.WriteLine ( string.Join ( "" , methodDocInfo.DocNode ) ) ;
            // }
            // }
            // }
        }
        [WpfFact]
        public void Test111 ( )
        {
            ResourceManager x = new ResourceManager (
                                                     "AnalysisControls.g"
                                                   , typeof ( PythonControl ).Assembly
                                                    ) ;
            var y = x.GetStream ( "mainstatusbar.baml" ) ;
            var b = new Baml2006Reader(y, new XamlReaderSettings(){});
            var c = b.SchemaContext ;
            var t = c.GetXamlType ( typeof ( TypesViewModel ) ) ;
        }
    }


    public class RoutedExecutionResultEventArgs : RoutedEventArgs
    {
        private readonly dynamic _result ;

        public RoutedExecutionResultEventArgs (
            RoutedEvent routedEvent
          , object      source
          , dynamic     result
        ) : base ( routedEvent , source )
        {
            _result = result ;
        }

        public dynamic Result { get { return _result ; } }
    }

    public class MyXmlWriter : XmlWriter
    {
        private readonly Utf8JsonWriter writer ;
        public MyXmlWriter ( Utf8JsonWriter writer ) { this.writer = writer ; }
#region Overrides of XmlWriter
        public override void WriteStartDocument ( )
        {
            writer.WriteStartObject();
        }

        public override void WriteStartDocument ( bool standalone )
        {
            writer.WriteStartObject();
        }

        public override void WriteEndDocument ( )
        {
            writer.WriteEndObject();
        }

        public override void WriteDocType ( string name , string pubid , string sysid , string subset ) { }

        public override void WriteStartElement ( string prefix , string localName , string ns )
        {
            writer.WriteStartObject();
            //writer.WriteStartObject ( localName ) ;
        }

        public override void WriteEndElement ( )
        {
            writer.WriteEndObject();
        }

        public override void WriteFullEndElement ( )
        {
            writer.WriteEndObject();
        }

        public override void WriteStartAttribute ( string prefix , string localName , string ns )
        {
            
            writer.WriteStartObject ( localName ) ;
        }

        public override void WriteEndAttribute ( )
        {

        }

        public override void WriteCData ( string text ) { }

        public override void WriteComment ( string text ) { }

        public override void WriteProcessingInstruction ( string name , string text ) { }

        public override void WriteEntityRef ( string name ) { }

        public override void WriteCharEntity ( char ch ) { }

        public override void WriteWhitespace ( string ws ) { }

        public override void WriteString ( string text )
        {
            writer.WriteStringValue ( text ) ;
        }

        public override void WriteSurrogateCharEntity ( char lowChar , char highChar ) { }

        public override void WriteChars ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( string data ) { }

        public override void WriteBase64 ( byte[] buffer , int index , int count ) { }

        public override void Flush ( ) { }

        public override string LookupPrefix ( string ns ) { return null ; }

        public override WriteState WriteState { get ; }
#endregion
    }
}