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
using System.Data ;
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
using System.Windows.Automation ;
using System.Windows.Baml2006 ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Xaml ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Serialization ;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.XmlDoc ;
using AnalysisControls ;
using AnalysisControls.Properties ;
using AnalysisControls.ViewModel ;
using Autofac ;
using AvalonDock ;
using AvalonDock.Layout ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;
using KayMcCormick.Lib.Wpf.JSON ;
using KayMcCormick.Lib.Wpf.View ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Moq ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;
using ColorConverter = System.Windows.Media.ColorConverter ;
using Condition = System.Windows.Automation.Condition ;
using File = System.IO.File ;
using MethodInfo = System.Reflection.MethodInfo;
using Process = System.Diagnostics.Process ;
using XamlReader = System.Windows.Markup.XamlReader ;
using XamlWriter = System.Windows.Markup.XamlWriter ;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedVariable
// ReSharper disable RedundantOverriddenMember

namespace ProjTests
{
    //    [ CollectionDefinition ( "GeneralPurpose" ) ]
    // ReSharper disable once UnusedType.Global
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
        private const string TYPES_VIEW_MODEL_XAML_PATH =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel_new.xaml" ;

        private const string INPUT_TYPES_VIEW_MODEL_XAML_PATH =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel.xaml" ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        // ReSharper disable once InconsistentNaming
        private const bool _disableLogging = true ;

        static ProjTests ( ) { LogHelper.DisableLogging = _disableLogging ; }

        private readonly ITestOutputHelper _output ;
#pragma warning disable 169
        private readonly LoggingFixture _loggingFixture ;
#pragma warning restore 169
#pragma warning disable 169
        private readonly ProjectFixture _projectFixture ;
#pragma warning restore 169

        private JsonSerializerOptions _testJsonSerializerOptions ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests (
            ITestOutputHelper output
            // , [ CanBeNull ] LoggingFixture loggingFixture
            // , ProjectFixture               projectFixture
        )
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException ;
            _output                                      =  output ;
            // _loggingFixture                              =  loggingFixture ;
            // _projectFixture                              =  projectFixture ;


            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if ( ! _disableLogging )
#pragma warning disable 162
                // ReSharper disable once HeuristicUnreachableCode
            {
                // loggingFixture?.SetOutputHelper ( output , this ) ;

                // if ( _loggingFixture != null )
                // {
                // _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
                // }
            }
#pragma warning restore 162
        }

        [ WpfFact ]
        public void TestRead ( )
        {
            var fileStream = new FileStream ( TYPES_VIEW_MODEL_XAML_PATH , FileMode.Open ) ;
            // ReSharper disable once UnusedVariable
            var x = XamlReader.Load ( fileStream ) ;
        }

        [ Fact ]
        public void TestSubstituteType ( )
        {
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                                , ApplicationGuid
                                                                                                 )
                                                           ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimeScope = instance.GetLifetimeScope ( ) ;
                var model = lifetimeScope.Resolve < TypesViewModel > ( ) ;
                var sts = lifetimeScope.Resolve < ISyntaxTypesService > ( ) ;
                var cMap = sts.CollectionMap ( ) ;
                var appTypeInfo = sts.GetAppTypeInfo ( typeof ( AssignmentExpressionSyntax ) ) ;
                var field = ( SyntaxFieldInfo ) appTypeInfo.Fields[ 0 ] ;
                var typeSyntax = SyntaxFactory.ParseTypeName (
                                                              typeof ( ArgumentSyntax ).FullName
                                                              ?? throw new
                                                                  InvalidOperationException ( )
                                                             ) ;
                var substType =
                    XmlDocElements.SubstituteType ( field , typeSyntax , cMap , model ) ;
            }
        }

        [ WpfFact ]
        public void TestSubstituteTyp11e ( )
        {
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                                , ApplicationGuid
                                                                                                 )
                                                           ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimeScope = instance.GetLifetimeScope ( ) ;
                var model = lifetimeScope.Resolve < Func < object , DataTable > > ( ) ;
                if ( model != null )
                {
                    var xo = model ( AppDomain.CurrentDomain ) ;
                    DebugUtils.WriteLine ( $"{xo}" ) ;
                }
            }
        }

        [ WpfFact ]
        public void TestXaml2 ( )
        {
            var model = new TypesViewModel ( new JsonSerializerOptions ( ) ) ;
            var output = new StringWriter ( ) ;
            Action < string > writeOut = output.WriteLine ;
            var pu = new ProxyUtils ( writeOut , ProxyUtilsBase.CreateInterceptor ( writeOut ) ) ;
            pu.TransformXaml ( model ) ;
            File.WriteAllText ( @"C:\data\logs\xaml.txt" , output.ToString ( ) ) ;
        }

        [ WpfFact ]
        public void TestXaml3 ( )
        {
            // ReSharper disable once UnusedVariable
            var model = new TypesViewModel ( new JsonSerializerOptions ( ) ) ;
            var output = new StringWriter ( ) ;
            Action < string > writeOut = output.WriteLine ;
            var pu = new ProxyUtils ( writeOut , ProxyUtilsBase.CreateInterceptor ( writeOut ) ) ;
            var inst = pu.TransformXaml2 ( INPUT_TYPES_VIEW_MODEL_XAML_PATH ) ;
            File.WriteAllText ( @"C:\data\logs\xaml2.txt" , output.ToString ( ) ) ;
        }

        // private static ITypesViewModel GetComponentTypesViewModel()
        // {
        //
        //     var xamlSchemaContext = new XamlSchemaContext();
        //     var settings = new XamlObjectWriter(xamlSchemaContext);
        //     
        //     var xamlXmlReaderSettings = new XamlXmlReaderSettings {} ;
        //     var objectWriter1 = new XamlWriter1(xamlSchemaContext);
        //
        //     XamlXmlReader xml = new XamlXmlReader ( TypesViewModelXamlPath, xamlSchemaContext, xamlXmlReaderSettings ) ;
        //     
        //     var model = new ComponentTypesViewModel();
        //     if ( ! model.IsInitialized )
        //     {
        //         model.BeginInit ( ) ;
        //         model.EndInit ( ) ;
        //     }
        //
        //     return model;
        // }

        // [WpfFact ]
        // public void TestTypes-view1 ( )
        // {
        // var app1 = new TestApp1();
        // var model = GetComponentTypesViewModel( ) ;

        // }

        [ WpfFact ]
        public void TestTypesView ( )
        {
            var viewModel = new TypesViewModel ( new JsonSerializerOptions ( ) ) ;
            viewModel.BeginInit ( ) ;
            viewModel.EndInit ( ) ;
            var stringWriter = new StringWriter ( ) ;
            using ( var x = XmlWriter.Create (
                                              stringWriter
                                            , new XmlWriterSettings { Indent = true }
                                             ) )
            {
                XamlWriter.Save ( viewModel , x ) ;
                x.Flush ( ) ;
            }


            XamlWriter.Save ( viewModel , File.CreateText ( TYPES_VIEW_MODEL_XAML_PATH ) ) ;




            // var typesView = new TypesView ( viewModel ) ;
            // var w = new Window { Content = typesView } ;
            // w.ShowDialog ( ) ;
        }

        [ Fact ]
        public void TestDoc ( )
        {
            // var xml = "<Summary xmlns=\"clr-namespace:AnalysisControls.ViewModel;"
            // + "assembly=AnalysisControls\">Hello</Summary>" ;
            // var s1 = XamlReader.Parse ( xml ) ;
            // _output.WriteLine(XamlWriter.Save(s1));
            var s = new Summary ( ) ;
            var p = new Para ( new XmlDocText ( "hello" ) ) ;
            s.DocumentElementCollection.Add ( p ) ;
            var x = XamlWriter.Save ( s ) ;
            _output.WriteLine ( x ) ;
        }

        [ WpfFact ]
        public void TestTypesView2 ( )
        {
            var viewModel = new TypesViewModel ( new JsonSerializerOptions ( ) ) ;
            viewModel.BeginInit ( ) ;
            viewModel.EndInit ( ) ;
            var stringWriter = new StringWriter ( ) ;
            using ( var x = XmlWriter.Create (
                                              stringWriter
                                            , new XmlWriterSettings { Indent = true }
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
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once FunctionComplexityOverflow
        public void TestJsonSerialization ( )
        {
            var w = new Window ( ) ;
            var options = new JsonSerializerOptions { WriteIndented = true } ;
            var xaml = XamlWriter.Save ( w ) ;
            var doc = new XmlDocument ( ) ;
            doc.LoadXml ( xaml ) ;
            var writer = new Utf8JsonWriter ( new MemoryStream ( ) ) ;
            var xxMyXmlWriter = new MyXmlWriter ( writer ) ;
            XamlWriter.Save ( w , xxMyXmlWriter ) ;
            var xDoc = new XDocument ( ) ;
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            XDocument.Parse ( xaml ) ;



            foreach ( var xNode in xDoc.Nodes ( ) )
            {
                switch ( xNode )
                {
                    case XComment xComment : break ;
                    case XCData xcData :     break ;
                    case XDocument xDocument :
                        writer.WriteStartObject ( ) ;
                        writer.WriteString ( "Kind" , nameof ( XDocument ) ) ;
                        break ;

                    case XElement xElement :

                    case XContainer xContainer :
                        break ;
                    case XDocumentType xDocumentType :                   break ;
                    case XProcessingInstruction xProcessingInstruction : break ;
                    case XText xText :                                   break ;
                    default :
                        throw new ArgumentOutOfRangeException ( nameof ( xNode ) ) ;
                }
            }

            //options.Converters.Add ( new JsonFrameworkElementConverter ( _output ) ) ;
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
            void Action ( string message ) { DebugUtils.WriteLine ( message ) ; }

            var proxy = ProxyUtilsBase.CreateProxy (
                                                    Action
                                                  , new BaseInterceptorImpl (
                                                                             Action
                                                                           , new ProxyGenerator ( )
                                                                            )
                                                   ) ;
            Assert.NotNull ( proxy ) ;
            // ReSharper disable once UnusedVariable
            var r = proxy.TransformXaml ( new Button { Content = "Hello" } ) ;
        }


        // ReSharper disable once UnusedMember.Global
        public void TestFE ( )
        {
            var f = new FrameworkElementFactory { Type = typeof ( Button ) } ;
            f.AppendChild ( new FrameworkElementFactory ( typeof ( TextBlock ) , "Hello" ) ) ;

            MethodInfo m1 = null ;
            MethodInfo m2 = null ;
            foreach ( var methodInfo in f.GetType ( )
                                         .GetMethods (
                                                      BindingFlags.Instance | BindingFlags.NonPublic
                                                     ) )
            {
                switch ( methodInfo.Name )
                {
                    case "InstantiateUnoptimizedTree" :
                        m1 = methodInfo ;
                        break ;
                    case "Seal" when methodInfo.GetParameters ( ).Length == 1 :
                        m2 = methodInfo ;
                        break ;
                }
            }

            if ( m2 != null )
            {
                m2.Invoke ( f , new object[] { new ControlTemplate ( ) } ) ; //
            }

            if ( m1 == null )
            {
                return ;
            }

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
        public Guid ApplicationGuid { get ; } =
            new Guid ( "d4870a23-f1ad-4618-b955-6b342c6afab6" ) ;

        [ WpfFact ]
        public void TestAdapter ( )
        {
            // var x = new TestApplication ( ) ;
            DebugUtils.WriteLine ( $"{Thread.CurrentThread.ManagedThreadId} projTests" ) ;
            using ( var instance = new ApplicationInstance (
                                                            ApplicationInstance
                                                               .CreateConfiguration (
                                                                                     _output
                                                                                        .WriteLine
                                                                                   , ApplicationGuid
                                                                                    )
                                                           ) )
            {
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimeScope = instance.GetLifetimeScope ( ) ;
                LogManager.ThrowExceptions = true ;
                var funcAry = lifetimeScope
                   .Resolve < IEnumerable < Func < LayoutDocumentPane , IDisplayableAppCommand > >
                    > ( ) ;


                var m = new DockingManager ( ) ;

                var pane = new LayoutDocumentPane ( ) ;

                var group = new LayoutDocumentPaneGroup ( pane ) ;

                var mLayoutRootPanel = new LayoutPanel ( group ) ;
                var layout = new LayoutRoot { RootPanel = mLayoutRootPanel } ;
                m.Layout = layout ;
                Window w = new AppWindow ( lifetimeScope ) ;
                w.Content = m ;

                foreach ( var func in funcAry )
                {
                    try
                    {
                        DebugUtils.WriteLine ( $"func is {func}" ) ;
                        var xx = func ( pane ) ;
                        DebugUtils.WriteLine ( xx.DisplayName ) ;
                        DebugUtils.WriteLine (
                                              $"{Thread.CurrentThread.ManagedThreadId} projTests"
                                             ) ;
                        xx.ExecuteAsync ( )
                          .ContinueWith (
                                         task => DebugUtils.WriteLine (
                                                                       // ReSharper disable once PossibleNullReferenceException
                                                                       task.IsFaulted
                                                                           ? task
                                                                            .Exception.ToString ( )
                                                                           : task
                                                                            .Result.ToString ( )
                                                                      )
                                        )
                          .Wait ( ) ;
                    }
                    catch ( Exception ex )
                    {
                        DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                    }
                }

                w.ShowDialog ( ) ;
                // var source = new TaskCompletionSource < bool > ( ) ;
                // x.TCS = source ;
                // x.Run ( w ) ;
                // Task.WaitAll ( x.TCS.Task ) ;
                // DebugUtils.WriteLine ( source.Task.Result ) ;
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


        // ReSharper disable once UnusedMember.Local
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

        [ WpfFact ( Timeout = 30000 ) ]
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

        [ WpfFact ]
        private void Dump1 ( )
        {
            var factory = new JsonImageConverterFactory ( ) ;
            var x = new LineGeometry ( new Point ( 0 , 0 ) , new Point ( 10 , 10 ) ) ;
            var y = new GeometryDrawing (
                                         new SolidColorBrush ( Colors.Blue )
                                       , new Pen ( new SolidColorBrush ( Colors.Green ) , 2 )
                                       , x
                                        ) ;
            ImageSource xx = new DrawingImage ( y ) ;
            var jsonConverter =
                ( JsonConverter < ImageSource > ) factory.CreateConverter (
                                                                           xx.GetType ( )
                                                                         , new
                                                                               JsonSerializerOptions ( )
                                                                          ) ;
            var memoryStream = new MemoryStream ( ) ;
            var writer = new JsonWriterOptions ( ) ;
            var jsonWriterOptions = new JsonSerializerOptions ( ) ;
            var utf8JsonWriter = new Utf8JsonWriter ( memoryStream , writer ) ;
            jsonConverter.Write ( utf8JsonWriter , xx , jsonWriterOptions ) ;
            utf8JsonWriter.Flush ( ) ;
            memoryStream.Flush ( ) ;
            var bytes = new byte[ memoryStream.Length ] ;
            memoryStream.Seek ( 0 , SeekOrigin.Begin ) ;
            memoryStream.Read ( bytes , 0 , ( int ) memoryStream.Length ) ;
            // ReSharper disable once UnusedVariable
            var json = Encoding.UTF8.GetString ( bytes ) ;
        }

        // ReSharper disable once UnusedMember.Local
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
                    DebugUtils.WriteLine ( xaml ) ;
                }

                Logger.Info (
                             "{x}{key} = {data}"
                           , string.Concat ( Enumerable.Repeat ( "  " , depth ) )
                           , resourceNodeInfo.Key
                           , resourceNodeInfo.Data
                            ) ;
                // ReSharper disable once AssignNullToNotNullAttribute
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
                // ReSharper disable once UnusedVariable
                var info2 = JsonSerializer.Deserialize < LogEventInfo > ( json , options ) ;
            }
            catch ( JsonException x )
            {
                var substring = "" ;
                if ( ! x.BytePositionInLine.HasValue )
                {
                    throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
                }

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

                throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
            }

            //Assert.Equal ( info1.CallerClassName , info2.CallerClassName ) ;

            var t = File.OpenText ( @"C:\data\logs\ProjInterface.json.test" ) ;
            var lineNo = 0 ;
            while ( ! t.EndOfStream )
            {
                lineNo += 1 ;
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
                    if ( ! x.BytePositionInLine.HasValue )
                    {
                        throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
                    }

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
                               , lineNo
                               , substring
                                ) ;

                    throw new UnableToDeserializeLogEventInfo ( substring , x ) ;
                }

                if ( info == null )
                {
                    continue ;
                }

                Logger.Debug ( info.FormattedMessage ) ;
                foreach ( var keyValuePair in info.Properties )
                {
                    Logger.Debug ( keyValuePair.Key ) ;
                    Logger.Debug ( keyValuePair.Value.ToString ( ) ) ;
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
                // ReSharper disable once UnusedVariable
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
                DebugUtils.WriteLine ( "Exception: " + e.Exception ) ;
                var inner = e.Exception.InnerException ;
                var seen = new HashSet < object > ( ) ;
                while ( inner != null
                        && ! seen.Contains ( inner ) )
                {
#if false
                    Logger.Error(inner, inner.ToString);
#endif

                    DebugUtils.WriteLine ( "Exception: " + e.Exception ) ;
                    seen.Add ( inner ) ;
                    inner = inner.InnerException ;
                }
            }
            catch ( Exception ex )
            {
                DebugUtils.WriteLine ( "Exception: " + ex ) ;
            }
        }

        [ WpfFact ]
        public void TestFormattedCodeControl2 ( )
        {
            var codeControl = new FormattedCode2 ( ) ;
            var w = new Window { Content = codeControl } ;

            var t = new Task ( ( ) => { } ) ;
            w.Closed += ( sender , args ) => t.Start ( ) ;
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = Resources.Program_Parse ;
            codeControl.SourceCode = sourceText ;

            var context = ( ISemanticModelContext ) AnalysisService.Parse ( sourceText , "test1" ) ;
            var syntaxTree = context.CurrentModel.SyntaxTree ;
            var model = context.CurrentModel ;
            // ReSharper disable once UnusedVariable
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            var tcs = new TaskCompletionSource < bool > ( ) ;
            Task.Run ( ( ) => codeControl.Refresh ( ) )
                .ContinueWith ( task => tcs.SetResult ( true ) ) ;

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
                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
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
            // ReSharper disable once UnusedVariable
            var comp = ctx.CompilationUnit ;
            // ReSharper disable once UnusedVariable
            var tree = ctx.CurrentModel.SyntaxTree ;
            // ReSharper disable once UnusedVariable
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
            // ReSharper disable once UnusedVariable
            var c = new ColorConverter ( ) ;
        }

#if PYTHON
        [ Fact ]
        public void TestPython1 ( )
        {
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                        , ApplicationGuid
                                                                                                 )
                                                           ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimescope = instance.GetLifetimeScope ( ) ;
                var p = lifetimescope.Resolve < PythonViewModel > ( ) ;

                var options = JsonConverters.CreateJsonSerializeOptions ( ) ;
                var json = JsonSerializer.Serialize ( p , options ) ;
                DebugUtils.WriteLine ( json ) ;
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
#endif
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
                                                                          // ReSharper disable once LocalizableElement
                                                                          "Boo"
                                                                          // ReSharper disable once NotResolvedInText
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
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                                , ApplicationGuid
                                                                                                 )
                                                           ) )
            {
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var scope = instance.GetLifetimeScope ( ) ;

                var xx = new BinaryFormatter ( ) ;
                var ee = new Exception ( ) ;
                var s = new MemoryStream ( ) ;
                xx.Serialize ( s , ee ) ;
                s.Flush ( ) ;
                s.Seek ( 0 , SeekOrigin.Begin ) ;
                var bytes = new byte[ s.Length ] ;
                var sLength = ( int ) s.Length ;
                // ReSharper disable once UnusedVariable
                var read = s.Read ( bytes , 0 , sLength ) ;

                var view = scope.Resolve < EventLogView > ( ) ;
                Assert.NotNull ( view.ViewModel ) ;
                var w = new Window { Content = view } ;
                w.ShowDialog ( ) ;
            }
        }

        [ WpfFact ]
        public void TestWriteBrush ( )
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
            DebugUtils.WriteLine ( JsonSerializer.Serialize ( b , opt ) ) ;
        }

        [ Fact ]
        public void TestApp ( )
        {
            var info = new ProcessStartInfo (
                                             @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\build\bin\debug\x86\ProjInterface\ProjInterface.exe"
                                            ) ;
            var proc = Process.Start ( info ) ;

            Thread.Sleep ( 5000 ) ;
            try
            {
                var walker = new TreeWalker ( Condition.TrueCondition ) ;

                var r = AutomationElement.RootElement ;
                AutomationElement child = null ;
                try
                {
                    child = walker.GetFirstChild ( r ) ;
                }
                catch ( Exception )
                {
                    proc?.Kill ( ) ;
                    proc = null ;
                }

                var lastChild = child ;
                for ( ; ; )
                {
                    HandleChild ( lastChild ) ;
                    var next = walker.GetNextSibling ( lastChild ) ;
                    if ( next == null )
                    {
                        break ;
                    }

                    lastChild = next ;
                }

                foreach ( AutomationElement o in r.FindAll (
                                                            TreeScope.Children
                                                          , Condition.TrueCondition
                                                           ) )
                {
                    DebugUtils.WriteLine ( o.ToString ( ) ) ;
                    try
                    {
                        DebugUtils.WriteLine (
                                              o.GetCachedPropertyValue (
                                                                        AutomationElement
                                                                           .ClassNameProperty
                                                                       )
                                               .ToString ( )
                                             ) ;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                // foreach ( AutomationElement rCachedChild in r.CachedChildren )
                // {
                // foreach ( var automationProperty in rCachedChild.GetSupportedProperties ( ) )
                // {
                // var v = rCachedChild.GetCachedPropertyValue ( automationProperty ) ;
                // DebugUtils.WriteLine ( v.ToString ( ) ) ;
                // }
                // }
            }
            finally
            {
                proc?.Kill ( ) ;
            }
        }

        private static void HandleChild ( AutomationElement child )
        {
            try
            {
                var cacheRequest = new CacheRequest ( ) ;
                cacheRequest.Add ( AutomationElement.ClassNameProperty ) ;
                foreach ( var automationProperty in child.GetSupportedProperties ( ) )
                {
                    try
                    {
                        var propValue = child.GetCurrentPropertyValue ( automationProperty ) ;
                        DebugUtils.WriteLine ( automationProperty.ProgrammaticName ) ;
#pragma warning disable CS0612 // Type or member is obsolete
                        DebugUtils.WriteLine ( propValue ) ;
#pragma warning restore CS0612 // Type or member is obsolete
                    }
                    catch ( Exception )
                    {
                        // ignored
                    }
                }

                var c = child.GetUpdatedCache ( cacheRequest ) ;
                var cn = c.GetCachedPropertyValue ( AutomationElement.ClassNameProperty ) ;
#pragma warning disable 612
                DebugUtils.WriteLine ( cn ) ;
#pragma warning restore 612
            }
            catch ( Exception ex )
            {
                DebugUtils.WriteLine ( "Got exception " + ex.Message ) ;
            }
        }


        [ Fact ]
        public void TestXmlDoc ( )
        {
            var x = TypesViewModel.LoadDoc ( ) ;
            XmlDocElements.DocMembers ( x ) ;
            // foreach ( var codeElementDocumentation in y.Select ( XmlDocElements.HandleDocElement ) )
            // {
            // if ( codeElementDocumentation != null )
            // {
            // DebugUtils.WriteLine ( codeElementDocumentation.ToString ( ) ) ;
            // }
            // }

            // var x = TypesViewModel.LoadXmlDoc ( ) ;
            // foreach ( var keyValuePair in x )
            // {
            // foreach ( var valuePair in keyValuePair.Value.MethodDocumentation )
            // {
            // foreach ( var methodDocInfo in valuePair.Value )
            // {
            // DebugUtils.WriteLine ( string.Join ( "" , methodDocInfo.DocNode ) ) ;
            // }
            // }
            // }
        }

        [ WpfFact ]
        public void TestControl111 ( )
        {
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                                , ApplicationGuid
                                                                                                 )
                                                           ) )
            {
                instance.AddModule ( new AnalysisControlsModule ( ) ) ;
                instance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                instance.Initialize ( ) ;
                var lifetimeScope = instance.GetLifetimeScope ( ) ;

                var t1 = new UiElementTypeConverter ( lifetimeScope ) ;
                var t = t1.ControlForValue ( typeof ( ProjTests ) , 1 ) ;
                var ff = new ScrollViewer ( ) { Content = t } ;
                var w1 = new Window { Content           = ff } ;
                w1.ShowDialog ( ) ;
            }
        }

        [ WpfFact ]
        public void Test111 ( )
        {
            var x = new ResourceManager (
                                         "AnalysisControls.g"
                                       , typeof ( PythonControl ).Assembly
                                        ) ;
            // ReSharper disable once ResourceItemNotResolved
            var y = x.GetStream ( "mainstatusbar.baml" ) ;
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader ( y , new XamlReaderSettings ( ) ) ;
            var c = b.SchemaContext ;
            // ReSharper disable once UnusedVariable
            var t = c.GetXamlType ( typeof ( TypesViewModel ) ) ;
        }

        [ Fact ]
        public void TestLambdaAppCommand ( )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured ( SlogMethod ) ;

#pragma warning disable 1998
            async Task < IAppCommandResult > CommandFunc ( LambdaAppCommand command )
#pragma warning restore 1998
            {
                Logger.Info ( $"{command}" ) ;
                Logger.Info ( $"{command.Argument}" ) ;
                return AppCommandResult.Success ;
            }

            var c = new LambdaAppCommand (
                                          "test"
                                        , CommandFunc
                                        , "arg"
                                        , exception
                                              => DebugUtils.WriteLine ( $"badness: {exception}" )
                                         ) ;
            c.ExecuteAsync ( )
             .ContinueWith (
                            task => {
                                if ( task.IsFaulted )
                                {
                                    DebugUtils.WriteLine ( "Faulted" ) ;
                                }

                                if ( ! task.IsCompleted )
                                {
                                    return ;
                                }

                                DebugUtils.WriteLine ( "completed" ) ;
                                DebugUtils.WriteLine ( task.Result.ToString ( ) ) ;
                            }
                           )
             .Wait ( 10000 ) ;
        }

        private void SlogMethod ( string message ) { }

        [ Fact ]
        public void TestXaml1 ( )
        {
            var context = XamlReader.GetWpfSchemaContext ( ) ;
            var xamlType = context.GetXamlType ( typeof ( Type ) ) ;
            var xamlType2 = context.GetXamlType ( typeof ( SyntaxFieldInfo ) ) ;
            // ReSharper disable once UnusedVariable
            var typeMember = xamlType2.GetMember ( "Type" ) ;

            var valueSerializer = xamlType.ValueSerializer ;
            var typeConverter = xamlType.TypeConverter ;
            // ReSharper disable once UnusedVariable
            var str1 =
                typeConverter.ConverterInstance.ConvertToString ( typeof ( List < string > ) ) ;
            // var str = valueSerializer.ConverterInstance.ConvertToString (
            // typeof ( List < string > )
            // , null
            // ) ;
            var @out = XamlWriter.Save (
                                        new SyntaxFieldInfo
                                        {
                                            Name = "test" , Type = typeof ( List < string > )
                                        }
                                       ) ;
            DebugUtils.WriteLine ( @out ) ;
            Logger.Info ( @out ) ;
        }
        [WpfFact]
        public void TestRibbonBuilder()
        {
            using (var instance = new ApplicationInstance(
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration(
                                                                                                  _output
                                                                                                     .WriteLine
                                                                                                , ApplicationGuid
                                                                                                 )
                                                           ))
            {
                instance.AddModule(new AnalysisControlsModule());
                instance.AddModule(new AnalysisAppLibModule());
                instance.Initialize();
                var lifetimeScope = instance.GetLifetimeScope();
                var builder = lifetimeScope.Resolve<RibbonBuilder>();
                var ribbon = builder.Ribbon;
                RibbonWindow w = new RibbonWindow();
                var dp = new DockPanel();
                dp.Children.Add(ribbon);
                w.ShowDialog();
            }
        }


    }

    public class TestApp1 : System.Windows.Application
    {
        #region Overrides of Application
        protected override void OnStartup ( StartupEventArgs e ) { base.OnStartup ( e ) ; }
        #endregion
    }

    public class XamlWriter1 : XamlObjectWriter
    {
        #region Overrides of XamlObjectWriter
        protected override void OnAfterBeginInit ( object value )
        {
            base.OnAfterBeginInit ( value ) ;
        }

        protected override void OnBeforeProperties ( object value )
        {
            base.OnBeforeProperties ( value ) ;
        }

        protected override void OnAfterProperties ( object value )
        {
            base.OnAfterProperties ( value ) ;
        }

        protected override void OnAfterEndInit ( object value ) { base.OnAfterEndInit ( value ) ; }

        protected override bool OnSetValue ( object eventSender , XamlMember member , object value )
        {
            return base.OnSetValue ( eventSender , member , value ) ;
        }

        public override void WriteGetObject ( ) { base.WriteGetObject ( ) ; }

        public override void WriteStartObject ( XamlType xamlType )
        {
            base.WriteStartObject ( xamlType ) ;
        }

        public override void WriteEndObject ( ) { base.WriteEndObject ( ) ; }

        public override void WriteStartMember ( XamlMember property )
        {
            base.WriteStartMember ( property ) ;
        }

        public override void WriteEndMember ( ) { base.WriteEndMember ( ) ; }

        public override void WriteValue ( object value ) { base.WriteValue ( value ) ; }

        public override void WriteNamespace ( NamespaceDeclaration namespaceDeclaration )
        {
            base.WriteNamespace ( namespaceDeclaration ) ;
        }

        protected override void Dispose ( bool disposing ) { base.Dispose ( disposing ) ; }
        #endregion

        public XamlWriter1 ( [ NotNull ] XamlSchemaContext schemaContext ) : base ( schemaContext )
        {
        }

        public XamlWriter1 (
            [ NotNull ] XamlSchemaContext schemaContext
          , XamlObjectWriterSettings      settings
        ) : base ( schemaContext , settings )
        {
        }
    }
}