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

using Microsoft.Build.Locator;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;
using AnalysisAppLib;
using AnalysisAppLib.Serialization;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using AnalysisControls.Properties;
using AnalysisControls.RibbonM;
using AnalysisControls.ViewModel;
using Autofac;
using AvalonDock;
using AvalonDock.Layout;
using Castle.DynamicProxy;
using CsvHelper;
using CsvHelper.Excel;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Dev.TestLib;
using KayMcCormick.Dev.TestLib.Fixtures;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using KayMcCormick.Lib.Wpf.JSON;
using KayMcCormick.Lib.Wpf.View;
using KayMcCormick.Lib.Wpf.ViewModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using NLog;
using Xunit;
using Xunit.Abstractions;
using Binding = System.Windows.Data.Binding;
using ColorConverter = System.Windows.Media.ColorConverter;
using Condition = System.Windows.Automation.Condition;
using File = System.IO.File;
using MethodInfo = System.Reflection.MethodInfo;
using Process = System.Diagnostics.Process;
using RegionInfo = AnalysisControls.RegionInfo;

using TextBlock = System.Windows.Controls.TextBlock;
using Window = System.Windows.Window;
using XamlReader = System.Windows.Markup.XamlReader;
using XamlWriter = System.Windows.Markup.XamlWriter;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedVariable
// ReSharper disable RedundantOverriddenMember

namespace ProjTests
{
    //    [ CollectionDefinition ( "GeneralPurpose" ) ]
    // ReSharper disable once UnusedType.Global
    public class GeneralPurpose : ICollectionFixture<GlobalLoggingFixture>

    {
    }

    [Collection("GeneralPurpose")]
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
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel_new.xaml";

        private const string INPUT_TYPES_VIEW_MODEL_XAML_PATH =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel.xaml";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ReSharper disable once InconsistentNaming
        private const bool _disableLogging = true;

        static ProjTests()
        {
            LogHelper.DisableLogging = _disableLogging;
        }

        private readonly ITestOutputHelper _output;
#pragma warning disable 169
        private readonly LoggingFixture _loggingFixture;
#pragma warning restore 169
#pragma warning disable 169
        private readonly ProjectFixture _projectFixture;
#pragma warning restore 169

        private JsonSerializerOptions _testJsonSerializerOptions;
        private string solutionPath = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\KayMcCormick.Dev\src\KayMcCormick.Dev\ManagedProd.sln";

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ProjTests(
            ITestOutputHelper output
            // , [ CanBeNull ] LoggingFixture loggingFixture
            // , ProjectFixture               projectFixture
        )
        {
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
            _output = output;
            // _loggingFixture                              =  loggingFixture ;
            // _projectFixture                              =  projectFixture ;


            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!_disableLogging)
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

        [WpfFact]
        public void TestRead()
        {
            var fileStream = new FileStream(TYPES_VIEW_MODEL_XAML_PATH, FileMode.Open);
            // ReSharper disable once UnusedVariable
            var x = XamlReader.Load(fileStream);
        }

        [Fact]
        public void TestSubstituteType()
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
                var model = lifetimeScope.Resolve<TypesViewModel>();
                var sts = lifetimeScope.Resolve<ISyntaxTypesService>();
                var cMap = sts.CollectionMap();
                var appTypeInfo = sts.GetAppTypeInfo(typeof(AssignmentExpressionSyntax));
                var field = (SyntaxFieldInfo) appTypeInfo.Fields[0];
                var typeSyntax = SyntaxFactory.ParseTypeName(
                    typeof(ArgumentSyntax).FullName
                    ?? throw new
                        InvalidOperationException()
                );
                var substType =
                    XmlDocElements.SubstituteType(field, typeSyntax, cMap, model);
            }
        }

        [WpfFact]
        public void TestSubstituteTyp11e()
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
                var model = lifetimeScope.Resolve<Func<object, DataTable>>();
                if (model != null)
                {
                    var xo = model(AppDomain.CurrentDomain);
                    DebugUtils.WriteLine($"{xo}");
                }
            }
        }

        [WpfFact]
        public void TestXaml2()
        {
            var model = new TypesViewModel(new JsonSerializerOptions());
            var output = new StringWriter();
            Action<string> writeOut = output.WriteLine;
            var pu = new ProxyUtils(writeOut, ProxyUtilsBase.CreateInterceptor(writeOut));
            pu.TransformXaml(model);
            File.WriteAllText(@"C:\data\logs\xaml.txt", output.ToString());
        }

        [WpfFact]
        public void TestXaml3()
        {
            // ReSharper disable once UnusedVariable
            var model = new TypesViewModel(new JsonSerializerOptions());
            var output = new StringWriter();
            Action<string> writeOut = output.WriteLine;
            var pu = new ProxyUtils(writeOut, ProxyUtilsBase.CreateInterceptor(writeOut));
            var inst = pu.TransformXaml2(INPUT_TYPES_VIEW_MODEL_XAML_PATH);
            File.WriteAllText(@"C:\data\logs\xaml2.txt", output.ToString());
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

        [WpfFact]
        public void TestTypesView()
        {
            var viewModel = new TypesViewModel(new JsonSerializerOptions());
            viewModel.BeginInit();
            viewModel.EndInit();
            var stringWriter = new StringWriter();
            using (var x = XmlWriter.Create(
                stringWriter
                , new XmlWriterSettings {Indent = true}
            ))
            {
                XamlWriter.Save(viewModel, x);
                x.Flush();
            }


            XamlWriter.Save(viewModel, File.CreateText(TYPES_VIEW_MODEL_XAML_PATH));


            // var typesView = new TypesView ( viewModel ) ;
            // var w = new Window { Content = typesView } ;
            // w.ShowDialog ( ) ;
        }

        [Fact]
        public void TestDoc()
        {
            // var xml = "<Summary xmlns=\"clr-namespace:AnalysisControls.ViewModel;"
            // + "assembly=AnalysisControls\">Hello</Summary>" ;
            // var s1 = XamlReader.Parse ( xml ) ;
            // _output.WriteLine(XamlWriter.Save(s1));
            var s = new Summary();
            var p = new Para(new XmlDocText("hello"));
            s.DocumentElementCollection.Add(p);
            var x = XamlWriter.Save(s);
            _output.WriteLine(x);
        }

        [WpfFact]
        public void TestTypesView2()
        {
            var viewModel = new TypesViewModel(new JsonSerializerOptions());
            viewModel.BeginInit();
            viewModel.EndInit();
            var stringWriter = new StringWriter();
            using (var x = XmlWriter.Create(
                stringWriter
                , new XmlWriterSettings {Indent = true}
            ))
            {
                XamlWriter.Save(viewModel, x);
                x.Flush();
            }


            XamlWriter.Save(
                viewModel.Root
                , File.CreateText(
                    @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\Types.xaml"
                )
            );
        }

        [WpfFact]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once FunctionComplexityOverflow
        public void TestJsonSerialization()
        {
            var w = new Window();
            var options = new JsonSerializerOptions {WriteIndented = true};
            var xaml = XamlWriter.Save(w);
            var doc = new XmlDocument();
            doc.LoadXml(xaml);
            var writer = new Utf8JsonWriter(new MemoryStream());
            var xxMyXmlWriter = new MyXmlWriter(writer);
            XamlWriter.Save(w, xxMyXmlWriter);
            var xDoc = new XDocument();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            XDocument.Parse(xaml);


            foreach (var xNode in xDoc.Nodes())
                switch (xNode)
                {
                    case XComment xComment: break;
                    case XCData xcData: break;
                    case XDocument xDocument:
                        writer.WriteStartObject();
                        writer.WriteString("Kind", nameof(XDocument));
                        break;

                    case XElement xElement:

                    case XContainer xContainer:
                        break;
                    case XDocumentType xDocumentType: break;
                    case XProcessingInstruction xProcessingInstruction: break;
                    case XText xText: break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(xNode));
                }

            //options.Converters.Add ( new JsonFrameworkElementConverter ( _output ) ) ;
            options.WriteIndented = true;
            var json = JsonSerializer.Serialize(w, options);
            File.WriteAllText(@"C:\data\out.json", json);
            _output.WriteLine(json);
            var parsed =
                JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        }

        [WpfFact]
        public void TestProxyUtils()
        {
            void Action(string message)
            {
                DebugUtils.WriteLine(message);
            }

            var proxy = ProxyUtilsBase.CreateProxy(
                Action
                , new BaseInterceptorImpl(
                    Action
                    , new ProxyGenerator()
                )
            );
            Assert.NotNull(proxy);
            // ReSharper disable once UnusedVariable
            var r = proxy.TransformXaml(new Button {Content = "Hello"});
        }


        // ReSharper disable once UnusedMember.Global
        public void TestFE()
        {
            var f = new FrameworkElementFactory {Type = typeof(Button)};
            f.AppendChild(new FrameworkElementFactory(typeof(TextBlock), "Hello"));

            MethodInfo m1 = null;
            MethodInfo m2 = null;
            foreach (var methodInfo in f.GetType()
                .GetMethods(
                    BindingFlags.Instance | BindingFlags.NonPublic
                ))
                switch (methodInfo.Name)
                {
                    case "InstantiateUnoptimizedTree":
                        m1 = methodInfo;
                        break;
                    case "Seal" when methodInfo.GetParameters().Length == 1:
                        m2 = methodInfo;
                        break;
                }

            if (m2 != null) m2.Invoke(f, new object[] {new ControlTemplate()}); //

            if (m1 == null) return;

            var r = m1.Invoke(
                f
                , BindingFlags.NonPublic | BindingFlags.Instance
                , null
                , Array.Empty<object>()
                , CultureInfo.CurrentCulture
            );


            var p = r.GetType()
                .GetProperty("FE", BindingFlags.Instance | BindingFlags.NonPublic);
            if (p != null)
            {
                var fe = p.GetValue(r);
                var w = new Window {Content = fe};
                w.ShowDialog();
            }

            Logger.Info(r.GetType());
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
        public Guid ApplicationGuid { get; } =
            new Guid("d4870a23-f1ad-4618-b955-6b342c6afab6");

        [WpfFact]
        public void TestAdapter0()
        {
            // var x = new TestApplication ( ) ;
            DebugUtils.WriteLine($"{Thread.CurrentThread.ManagedThreadId} projTests");
            using (var instance = new ApplicationInstance(
                ApplicationInstance
                    .CreateConfiguration(
                        _output
                            .WriteLine
                        , ApplicationGuid
                    )
            ))
            {
                instance.AddModule(new AnalysisAppLibModule());
                instance.AddModule(new AnalysisControlsModule());
                instance.Initialize();
                var lifetimeScope = instance.GetLifetimeScope();
                LogManager.ThrowExceptions = true;
                var funcAry = lifetimeScope
                    .Resolve<IEnumerable<Func<LayoutDocumentPane, IDisplayableAppCommand>>
                    >();


                var m = new DockingManager();

                var pane = new LayoutDocumentPane();

                var group = new LayoutDocumentPaneGroup(pane);

                var mLayoutRootPanel = new LayoutPanel(group);
                var layout = new LayoutRoot {RootPanel = mLayoutRootPanel};
                m.Layout = layout;
                Window w = new AppWindow(lifetimeScope);
                w.Content = m;

                foreach (var func in funcAry)
                    try
                    {
                        DebugUtils.WriteLine($"func is {func}");
                        var xx = func(pane);
                        DebugUtils.WriteLine(xx.DisplayName);
                        DebugUtils.WriteLine(
                            $"{Thread.CurrentThread.ManagedThreadId} projTests"
                        );
                        xx.ExecuteAsync()
                            .ContinueWith(
                                task => DebugUtils.WriteLine(
                                    // ReSharper disable once PossibleNullReferenceException
                                    task.IsFaulted
                                        ? task
                                            .Exception.ToString()
                                        : task
                                            .Result.ToString()
                                )
                            )
                            .Wait();
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }

                w.ShowDialog();
                // var source = new TaskCompletionSource < bool > ( ) ;
                // x.TCS = source ;
                // x.Run ( w ) ;
                // Task.WaitAll ( x.TCS.Task ) ;
                // DebugUtils.WriteLine ( source.Task.Result ) ;
            }
        }

        [WpfFact]
        public void TestExceptionInfo()
        {
            var @in = File.OpenRead(@"c:\data\logs\exception.bin");
            var f = new BinaryFormatter();
            var exception = (Exception) f.Deserialize(@in);
            var h = new HandleExceptionImpl();
            h.HandleException(exception);
            var f2 = new SoapFormatter();
            var w = File.OpenWrite(@"C:\data\logs\exception.xml");
            f2.Serialize(w, exception);
            w.Flush();
            w.Close();
        }


        // ReSharper disable once UnusedMember.Local
        private int CountChildren([NotNull] DependencyObject tv)
        {
            var count = 1;
            foreach (var child in LogicalTreeHelper.GetChildren(tv)) count += CountChildren((DependencyObject) child);

            return count;
        }

        [WpfFact]
        public void TestResourcesModel()
        {
            using (var instance =
                new ApplicationInstance(
                    new ApplicationInstance.ApplicationInstanceConfiguration(_output.WriteLine, ApplicationGuid)
                ))
            {
                instance.AddModule(new AnalysisControlsModule());
                instance.AddModule(new AnalysisAppLibModule());
                instance.Initialize();
                var lifetimescope = instance.GetLifetimeScope();

                var model = lifetimescope.Resolve<AllResourcesTreeViewModel>();

                DumpTree(null, model.AllResourcesCollection);
            }
        }

        [WpfFact(Timeout = 30000)]
        public void Test123()
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
        private void Dump1()
        {
            var factory = new JsonImageConverterFactory();
            var x = new LineGeometry(new Point(0, 0), new Point(10, 10));
            var y = new GeometryDrawing(
                new SolidColorBrush(Colors.Blue)
                , new Pen(new SolidColorBrush(Colors.Green), 2)
                , x
            );
            ImageSource xx = new DrawingImage(y);
            var jsonConverter =
                (JsonConverter<ImageSource>) factory.CreateConverter(
                    xx.GetType()
                    , new
                        JsonSerializerOptions()
                );
            var memoryStream = new MemoryStream();
            var writer = new JsonWriterOptions();
            var jsonWriterOptions = new JsonSerializerOptions();
            var utf8JsonWriter = new Utf8JsonWriter(memoryStream, writer);
            jsonConverter.Write(utf8JsonWriter, xx, jsonWriterOptions);
            utf8JsonWriter.Flush();
            memoryStream.Flush();
            var bytes = new byte[memoryStream.Length];
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Read(bytes, 0, (int) memoryStream.Length);
            // ReSharper disable once UnusedVariable
            var json = Encoding.UTF8.GetString(bytes);
        }

        // ReSharper disable once UnusedMember.Local
        private void DumpTree(
            AllResourcesTree tree
            , [NotNull] IEnumerable<ResourceNodeInfo> modelAllResourcesCollection
            , int depth = 0
        )
        {
            foreach (var resourceNodeInfo in modelAllResourcesCollection)
            {
                try
                {
                    var json1 = JsonSerializer.Serialize(
                        resourceNodeInfo.Key
                        , TestJsonSerializerOptions
                    );

                    Logger.Debug(json1);
                    var json2 = JsonSerializer.Serialize(
                        resourceNodeInfo.Data
                        , TestJsonSerializerOptions
                    );
                    Logger.Debug(json2);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, ex.Message);
                }

                var selector = new ResourceDetailTemplateSelector();
                if (tree != null)
                {
                    var dt = selector.SelectTemplate(resourceNodeInfo.Data, tree);
                    if (dt != null)
                    {
                        var xaml = XamlWriter.Save(dt);
                        DebugUtils.WriteLine(xaml);
                    }
                }

                Logger.Info(
                    "{x}{key} = {data}"
                    , string.Concat(Enumerable.Repeat("  ", depth))
                    , resourceNodeInfo.Key
                    , resourceNodeInfo.Data
                );
                // ReSharper disable once AssignNullToNotNullAttribute
                DumpTree(tree, resourceNodeInfo.Children, depth + 1);
            }
        }

        public JsonSerializerOptions TestJsonSerializerOptions
        {
            get { return _testJsonSerializerOptions; }
            set { _testJsonSerializerOptions = value; }
        }

        [Fact]
        public void TestModule1()
        {
            var module = new AnalysisAppLibModule();

            var mock = new Mock<ContainerBuilder>();
            mock.Setup(cb => module.DoLoad(cb));
        }

        [Fact]
        public void DeserializeLog()
        {
            var ctx = (ICompilationUnitRootContext) AnalysisService.Parse(
                Resources.Program_Parse
                , "test"
            );
            var info1 = LogEventInfo.Create(LogLevel.Debug, "test", "test");
            info1.Properties["node"] = ctx.CompilationUnit;

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonConverterLogEventInfo());
            options.Converters.Add(new JsonSyntaxNodeConverter());

            var json = JsonSerializer.Serialize(info1, options);
            Logger.Info(json);
            try
            {
                // ReSharper disable once UnusedVariable
                var info2 = JsonSerializer.Deserialize<LogEventInfo>(json, options);
            }
            catch (JsonException x)
            {
                var substring = "";
                if (!x.BytePositionInLine.HasValue) throw new UnableToDeserializeLogEventInfo(substring, x);

                try
                {
                    var eBytePositionInLine = (int) x.BytePositionInLine.Value - 16;
                    if (eBytePositionInLine < 0) eBytePositionInLine = 0;

                    var length = 32;
                    var endIndex = eBytePositionInLine + length;
                    if (endIndex >= json.Length) endIndex = json.Length;

                    length = endIndex - eBytePositionInLine;
                    substring = json.Substring(eBytePositionInLine, length);
                }
                catch (ArgumentOutOfRangeException)
                {
                    substring = json;
                }

                Logger.Warn("Start of problem is {problem}", substring);

                throw new UnableToDeserializeLogEventInfo(substring, x);
            }

            //Assert.Equal ( info1.CallerClassName , info2.CallerClassName ) ;

            var t = File.OpenText(@"C:\data\logs\ProjInterface.json.test");
            var lineNo = 0;
            while (!t.EndOfStream)
            {
                lineNo += 1;
                var line = t.ReadLine();

                LogEventInfo info;
                try
                {
                    info = JsonSerializer.Deserialize<LogEventInfo>(
                        line
                        ?? throw new
                            InvalidOperationException()
                        , options
                    );
                }
                catch (JsonException x)
                {
                    var substring = "";
                    if (!x.BytePositionInLine.HasValue) throw new UnableToDeserializeLogEventInfo(substring, x);

                    try
                    {
                        var eBytePositionInLine = (int) x.BytePositionInLine.Value - 16;
                        if (eBytePositionInLine < 0) eBytePositionInLine = 0;

                        var length = 32;
                        var endIndex = eBytePositionInLine + length;
                        if (line != null
                            && endIndex >= line.Length)
                            endIndex = line.Length;

                        length = endIndex - eBytePositionInLine;
                        if (line != null) substring = line.Substring(eBytePositionInLine, length);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        substring = line;
                    }

                    Logger.Warn(
                        "Start of problem is line {lineno} {problem}"
                        , lineNo
                        , substring
                    );

                    throw new UnableToDeserializeLogEventInfo(substring, x);
                }

                if (info == null) continue;

                Logger.Debug(info.FormattedMessage);
                foreach (var keyValuePair in info.Properties)
                {
                    Logger.Debug(keyValuePair.Key);
                    Logger.Debug(keyValuePair.Value.ToString());
                }
            }
        }

        private void CurrentDomainOnFirstChanceException(
            object sender
            , FirstChanceExceptionEventArgs e
        )
        {
            HandleInnerExceptions(e);
        }

        private void HandleInnerExceptions(FirstChanceExceptionEventArgs e)
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var msg = $"{e.Exception}";
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
                DebugUtils.WriteLine("Exception: " + e.Exception);
                var inner = e.Exception.InnerException;
                var seen = new HashSet<object>();
                while (inner != null
                       && !seen.Contains(inner))
                {
#if false
                    Logger.Error(inner, inner.ToString);
#endif

                    DebugUtils.WriteLine("Exception: " + e.Exception);
                    seen.Add(inner);
                    inner = inner.InnerException;
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine("Exception: " + ex);
            }
        }

        [WpfFact]
        public void TestFormattedCodeControl2()
        {
            var codeControl = new FormattedCode2();
            var w = new Window {Content = codeControl};

            var t = new Task(() => { });
            w.Closed += (sender, args) => t.Start();
            //FormattdCode1.SetValue(ComboBox.Edit.Editable)

            var sourceText = Resources.Program_Parse;
            codeControl.SourceCode = sourceText;

            var context = (ISemanticModelContext) AnalysisService.Parse(sourceText, "test1");
            var syntaxTree = context.CurrentModel.SyntaxTree;
            var model = context.CurrentModel;
            // ReSharper disable once UnusedVariable
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
            var tcs = new TaskCompletionSource<bool>();
            Task.Run(() => codeControl.Refresh())
                .ContinueWith(task => tcs.SetResult(true));

            w.Show();

            try
            {
                var bmp = new RenderTargetBitmap(
                    (int) w.ActualWidth
                    , (int) w.ActualHeight
                    , 72
                    , 72
                    , PixelFormats.Pbgra32
                );
                bmp.Render(codeControl);
                var pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(bmp));
                using (Stream fileStream = File.Create(@"c:\data\test\out.png"))
                {
                    pngImage.Save(fileStream);
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }

            tcs.Task.Wait();

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
        [Fact]
        public void TestSerialize()
        {
        }

        [Fact]
        public void TestRewrite()
        {
            var ctx = AnalysisService.Parse(Resources.Program_Parse, "test");
            // ReSharper disable once UnusedVariable
            var comp = ctx.CompilationUnit;
            // ReSharper disable once UnusedVariable
            var tree = ctx.CurrentModel.SyntaxTree;
            // ReSharper disable once UnusedVariable
            var codeAnalyseContext = AnalysisService.Parse(Resources.Program_Parse, "test");
            // var syntaxNode = logUsagesRewriter.Visit ( tree.GetRoot ( ) ) ;
            // var s = new StringWriter ( ) ;
            // using ( var fileStream = File.OpenWrite ( @"out.cs" ) )
            // {
            // syntaxNode.WriteTo ( new StreamWriter ( fileStream ) ) ;
            // s.Close ( ) ;
            // }
        }

        [Fact]
        public void TestColorConverter()
        {
            // ReSharper disable once UnusedVariable
            var c = new ColorConverter();
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
        public void Dispose()
        {
            // _loggingFixture?.Dispose ( ) ;
            AppDomain.CurrentDomain.FirstChanceException -= CurrentDomainOnFirstChanceException;
            // if ( ! _disableLogging )
            // {
            // _loggingFixture.SetOutputHelper ( null ) ;
            // }
        }

        [WpfFact]
        public void TestExceptionUserControl()
        {
            var w = new Window();
            Exception ex = new AggregateException(
                new ArgumentException(
                    // ReSharper disable once LocalizableElement
                    "Boo"
                    // ReSharper disable once NotResolvedInText
                    , "param"
                    , new
                        InvalidOperationException()
                )
                , new InvalidOperationException("boo2")
            );
            var dd = new ExceptionDataInfo
            {
                Exception = ex, ParsedExceptions = Utils.GenerateParsedException(ex)
            };

            w.Content = new ExceptionUserControl {DataContext = dd};
            w.ShowDialog();
        }

        [WpfFact]
        public void TestView1()
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
                instance.AddModule(new AnalysisAppLibModule());
                instance.Initialize();
                var scope = instance.GetLifetimeScope();

                var xx = new BinaryFormatter();
                var ee = new Exception();
                var s = new MemoryStream();
                xx.Serialize(s, ee);
                s.Flush();
                s.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[s.Length];
                var sLength = (int) s.Length;
                // ReSharper disable once UnusedVariable
                var read = s.Read(bytes, 0, sLength);

                var view = scope.Resolve<EventLogView>();
                Assert.NotNull(view.ViewModel);
                var w = new Window {Content = view};
                w.ShowDialog();
            }
        }

        [WpfFact]
        public void TestWriteBrush()
        {
            var brushConverter = new JsonBrushConverter();
            var opt = new JsonSerializerOptions();
            opt.Converters.Add(brushConverter);
            var brush = new SolidColorBrush(Colors.Blue);
            var json = JsonSerializer.Serialize(brush, opt);
            Logger.Info("json is {json}", json);
            var b = new LinearGradientBrush(
                Colors.Blue
                , Colors.Green
                , new Point(0, 0)
                , new Point(10, 10)
            );
            DebugUtils.WriteLine(JsonSerializer.Serialize(b, opt));
        }

        [Fact]
        public void TestApp()
        {
            var info = new ProcessStartInfo(
                @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\build\bin\debug\x86\ProjInterface\ProjInterface.exe"
            );
            var proc = Process.Start(info);

            Thread.Sleep(5000);
            try
            {
                var walker = new TreeWalker(Condition.TrueCondition);

                var r = AutomationElement.RootElement;
                AutomationElement child = null;
                try
                {
                    child = walker.GetFirstChild(r);
                }
                catch (Exception)
                {
                    proc?.Kill();
                    proc = null;
                }

                var lastChild = child;
                for (;;)
                {
                    HandleChild(lastChild);
                    var next = walker.GetNextSibling(lastChild);
                    if (next == null) break;

                    lastChild = next;
                }

                foreach (AutomationElement o in r.FindAll(
                    TreeScope.Children
                    , Condition.TrueCondition
                ))
                {
                    DebugUtils.WriteLine(o.ToString());
                    try
                    {
                        DebugUtils.WriteLine(
                            o.GetCachedPropertyValue(
                                    AutomationElement
                                        .ClassNameProperty
                                )
                                .ToString()
                        );
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
                proc?.Kill();
            }
        }

        private static void HandleChild(AutomationElement child)
        {
            try
            {
                var cacheRequest = new CacheRequest();
                cacheRequest.Add(AutomationElement.ClassNameProperty);
                foreach (var automationProperty in child.GetSupportedProperties())
                    try
                    {
                        var propValue = child.GetCurrentPropertyValue(automationProperty);
                        DebugUtils.WriteLine(automationProperty.ProgrammaticName);
#pragma warning disable CS0612 // Type or member is obsolete
                        DebugUtils.WriteLine(propValue);
#pragma warning restore CS0612 // Type or member is obsolete
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                var c = child.GetUpdatedCache(cacheRequest);
                var cn = c.GetCachedPropertyValue(AutomationElement.ClassNameProperty);
#pragma warning disable 612
                DebugUtils.WriteLine(cn);
#pragma warning restore 612
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine("Got exception " + ex.Message);
            }
        }


        [Fact]
        public void TestXmlDoc()
        {
            var x = TypesViewModel.LoadDoc();
            XmlDocElements.DocMembers(x);
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

        [WpfFact]
        public void TestControl111()
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

                var t1 = new UiElementTypeConverter(lifetimeScope);
                var t = t1.ControlForValue(typeof(ProjTests), 1);
                var ff = new ScrollViewer() {Content = t};
                var w1 = new Window {Content = ff};
                w1.ShowDialog();
            }
        }

        [WpfFact]
        public void Test111()
        {
            var x = new ResourceManager(
                "AnalysisControls.g"
                , typeof(PythonControl).Assembly
            );
            // ReSharper disable once ResourceItemNotResolved
            var y = x.GetStream("mainstatusbar.baml");
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader(y, new XamlReaderSettings());
            var c = b.SchemaContext;
            // ReSharper disable once UnusedVariable
            var t = c.GetXamlType(typeof(TypesViewModel));
        }

        [Fact]
        public void TestLambdaAppCommand()
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured(SlogMethod);

#pragma warning disable 1998
            async Task<IAppCommandResult> CommandFunc(LambdaAppCommand command)
#pragma warning restore 1998
            {
                Logger.Info($"{command}");
                Logger.Info($"{command.Argument}");
                return AppCommandResult.Success;
            }

            var c = new LambdaAppCommand(
                "test"
                , CommandFunc
                , "arg"
                , exception
                    => DebugUtils.WriteLine($"badness: {exception}")
            );
            c.ExecuteAsync()
                .ContinueWith(
                    task =>
                    {
                        if (task.IsFaulted) DebugUtils.WriteLine("Faulted");

                        if (!task.IsCompleted) return;

                        DebugUtils.WriteLine("completed");
                        DebugUtils.WriteLine(task.Result.ToString());
                    }
                )
                .Wait(10000);
        }

        private void SlogMethod(string message)
        {
        }

        [Fact]
        public void TestXaml1()
        {
            var context = XamlReader.GetWpfSchemaContext();
            var xamlType = context.GetXamlType(typeof(Type));
            var xamlType2 = context.GetXamlType(typeof(SyntaxFieldInfo));
            // ReSharper disable once UnusedVariable
            var typeMember = xamlType2.GetMember("Type");

            var valueSerializer = xamlType.ValueSerializer;
            var typeConverter = xamlType.TypeConverter;
            // ReSharper disable once UnusedVariable
            var str1 =
                typeConverter.ConverterInstance.ConvertToString(typeof(List<string>));
            // var str = valueSerializer.ConverterInstance.ConvertToString (
            // typeof ( List < string > )
            // , null
            // ) ;
            var @out = XamlWriter.Save(
                new SyntaxFieldInfo
                {
                    Name = "test", Type = typeof(List<string>)
                }
            );
            DebugUtils.WriteLine(@out);
            Logger.Info(@out);
        }

        [WpfFact]
        public void TestRibbonBuilder()
        {
            // var assembly = typeof(BaseApp).Assembly;
            // var x = new ResourceManager(
            //     "WpfLib.g"
            //     , assembly
            // );
            // ReSharper disable once ResourceItemNotResolved
            var oo = ProjTestsHelper.ControlsResources();

            var w2 = new RWindow();
            //w2.ShowDialog();
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

                var workspaceReplaySubject = new ReplaySubject<AdhocWorkspace>();
                var progress = new ReplaySubject<CommandProgress>();
                var lifetimeScope = instance.GetLifetimeScope(containerBuilder =>
                {
                    containerBuilder.RegisterInstance(workspaceReplaySubject).AsSelf().AsImplementedInterfaces();
                    containerBuilder.RegisterInstance(progress).AsSelf().AsImplementedInterfaces();
                });

                var sourceDocs = new ObservableCollection<AppDoc>();

                var provider = lifetimeScope.Resolve<ControlsProvider>();
                foreach (var providerType in provider.Types)
                {
                    DebugUtils.WriteLine(providerType.FullName);
                    TypeDescriptor.AddProvider(provider, providerType);
                }

                foreach (var displayableAppCommand in lifetimeScope.Resolve<IEnumerable<IDisplayableAppCommand>>())
                    DebugUtils.WriteLine(displayableAppCommand.DisplayName);
                var builder = lifetimeScope.Resolve<RibbonBuilder>();
                var ribbon = builder.Ribbon;
                ribbon.SelectionChanged += (sender, args) => DebugUtils.WriteLine(args.AddedItems[0].ToString());
                var w = new RibbonWindow();
                var dp = new DockPanel();
                var progresses = new ObservableCollection<CommandProgress>();
                var ais = lifetimeScope.Resolve<ReplaySubject<ActivationInfo>>();
                var ci = new ObservableCollection<ActivationInfo>();
                object resources = null;
                var lb = AnalysisControlsModule.ReplayListView(ci, ais, oo);

                var lv = AnalysisControlsModule.ReplayListView(progresses, progress, oo);
                workspaceReplaySubject.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send)
                    .Subscribe(
                        workspace =>
                        {
                            workspace.WorkspaceFailed += (sender, args) =>
                            {
                                DebugUtils.WriteLine(args.Diagnostic.Message);
                            };
                            workspace.WorkspaceChanged += OnWorkspaceOnWorkspaceChanged;
                        });

                dp.Children.Add(ribbon);
                ribbon.SetValue(DockPanel.DockProperty, Dock.Top);
                var uiElement = new Grid();

                var m = new DockingManager();

                sourceDocs.CollectionChanged += (sender, args) =>
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var argsNewItem in args.NewItems)
                                DebugUtils.WriteLine("added " + argsNewItem.ToString());

                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (var argsOldItem in args.OldItems)
                                DebugUtils.WriteLine($"removed " + argsOldItem.ToString());
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            break;
                        case NotifyCollectionChangedAction.Move:
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                };
                m.DocumentsSource = sourceDocs;
                sourceDocs.Add(new AppDoc() {Title = "test"});
                var pane = new LayoutDocumentPane();
                pane.Children.Add(new LayoutDocument() {Content = lv});

                pane.Children.Add(new LayoutDocument() {Content = lb, Title = "Activations"});
                var group = new LayoutDocumentPaneGroup(pane);

                var mLayoutRootPanel = new LayoutPanel(group);
                var layout = new LayoutRoot {RootPanel = mLayoutRootPanel};
                m.Layout = layout;

                uiElement.Children.Add(m);

                dp.Children.Add(uiElement);

                dp.LastChildFill = true;
                w.Content = dp;
                w.ShowDialog();
            }
        }

        private async void OnWorkspaceOnWorkspaceChanged(object sender, WorkspaceChangeEventArgs args)
        {
            DebugUtils.WriteLine(args.Kind.ToString());
            var project = args.NewSolution.GetProject(args.ProjectId);
            switch (args.Kind)
            {
                case WorkspaceChangeKind.SolutionChanged:
                    break;
                case WorkspaceChangeKind.SolutionAdded:
                    break;
                case WorkspaceChangeKind.SolutionRemoved:
                    break;
                case WorkspaceChangeKind.SolutionCleared:
                    break;
                case WorkspaceChangeKind.SolutionReloaded:
                    break;
                case WorkspaceChangeKind.ProjectAdded:
                    break;
                case WorkspaceChangeKind.ProjectRemoved:
                    break;
                case WorkspaceChangeKind.ProjectChanged:
                    var ch = args.NewSolution.GetChanges(args.OldSolution);
                    var pcj = ch.GetProjectChanges();
                    foreach (var ch1 in pcj)
                    {
                        var docChanges = ch1.GetChangedDocuments().Any();
                        var metadata = ch1.GetAddedMetadataReferences().Any();
                        foreach (var dc in ch1.GetChangedDocuments())
                        {
                            var oldDoc = args.OldSolution.GetDocument(dc);
                            var newDoc = args.NewSolution.GetDocument(dc);
                            var x = await newDoc.GetTextChangesAsync(oldDoc);
                            foreach (var xx in x) DebugUtils.WriteLine(xx.NewText);
                        }

                        DebugUtils.WriteLine(string.Join(", ",
                            ch1.GetAddedMetadataReferences().Select(xxx => xxx.Display)));
                        if (!docChanges && !metadata) DebugUtils.WriteLine(ch1.ToString());
                    }

                    break;
                case WorkspaceChangeKind.ProjectReloaded:
                    break;
                case WorkspaceChangeKind.DocumentAdded:
                    if (project != null)
                        if (args.DocumentId != null)
                        {
                            var doc = project
                                .GetDocument(args.DocumentId);
                            if (doc != null)
                            {
                                var text = await doc.GetTextAsync();
                                // DebugUtils.WriteLine(text.ToString());
                            }
                        }


                    break;
                case WorkspaceChangeKind.DocumentRemoved:
                    break;
                case WorkspaceChangeKind.DocumentReloaded:
                    break;
                case WorkspaceChangeKind.DocumentChanged:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentAdded:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentRemoved:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentReloaded:
                    break;
                case WorkspaceChangeKind.AdditionalDocumentChanged:
                    break;
                case WorkspaceChangeKind.DocumentInfoChanged:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentAdded:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentRemoved:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentReloaded:
                    break;
                case WorkspaceChangeKind.AnalyzerConfigDocumentChanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [WpfFact]
        public void TestControl2()
        {
            var type = typeof(Generic2<Type>);
            var type2 = typeof(Dictionary<string, ConcurrentDictionary<object, Window>>);


            var d = new DevTypeControl {Type = type2};
            //CustomControl2 c = new CustomControl2() { Type = type2 };
            var w = new Window();
            //w.Padding = new Thickness(10);
            //c.Margin = new Thickness(15);
            w.Content = d;
            w.Content = d;
            w.ShowDialog();
            return;

            // StackPanel p1 = new StackPanel() {Orientation = Orientation.Vertical};
            // StackPanel p2 = new StackPanel() { Orientation = Orientation.Horizontal };
            // StackPanel p3 = new StackPanel() { Orientation = Orientation.Horizontal };
            // p1.Children.Add(p2);
            // p2.Children.Add(c);
            // CustomControl2 c2 = new CustomControl2() { Type = type };
            // p2.Children.Add(c2);
            // p1.Children.Add(p3);

            // Type type3 = typeof(object[]);

            // CustomControl2 c3 = new CustomControl2() { Type = type3 };

            // Type type4 = typeof(IEnumerable<>);
            // CustomControl2 c4 = new CustomControl2() { Type = type };
            // p3.Children.Add(c3);
            // p3.Children.Add(c4);
            // w.Content = p1;
            // w.ShowDialog();
        }

        [WpfFact]
        public void TestStore3()
        {
            var x = new CustomTextSource3(1);
            var comp = AnalysisService.Parse(Resources.Program_Parse, "test", false);
            x.Compilation = comp.Compilation;
            x.Tree = comp.SyntaxTree;
            x.Node = comp.CompilationUnit;
            foreach (var diagnostic in comp.Compilation.GetDiagnostics())
                DebugUtils.WriteLine(diagnostic.Properties.Count.ToString());
            x.Errors = comp.Compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => (CompilationError) new DiagnosticError(d)).ToList();
            Assert.Equal(7, x.Errors.Count);
            var charPos = 0;
            TextWriter writer = new StringWriter();
            TextRun tr = null;
            while (!((tr = x.GetTextRun(charPos)) is TextEndOfParagraph))
            {
                if (tr is CustomTextCharacters trc)
                {
                    Logger.Info("writing text " + trc.Text);
                    writer.Write(trc.Text);
                }
                else if (tr is TextEndOfLine teol)
                {
                    writer.WriteLine("");
                }
                else
                {
                    throw new InvalidOperationException(tr.GetType().FullName);
                }

                charPos += tr.Length;
            }

            DebugUtils.WriteLine(writer.ToString());
        }

        [WpfFact]
        public void TestControl21()
        {
            var type = typeof(Generic2<Type>);
            var type2 = typeof(Dictionary<string, ConcurrentDictionary<object, Window>>);
            var ss = new StackPanel {Orientation = Orientation.Vertical};

            var d = new FormattedTextControl() {BorderThickness = new Thickness(3), BorderBrush = Brushes.Pink};
            var xb = new TextBlock();
            xb.SetBinding(TextBlock.TextProperty, new Binding("HoverColumn") {Source = d});
            var xy = new TextBlock();
            xy.SetBinding(TextBlock.TextProperty, new Binding("HoverRow") {Source = d});
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new TextBlock {Text = "( "});
            stackPanel.Children.Add(xb);
            stackPanel.Children.Add(new TextBlock {Text = ", "});
            stackPanel.Children.Add(xy);
            stackPanel.Children.Add(new TextBlock {Text = " )"});
            ss.Children.Add(stackPanel);

            //var textBlock = new TextBlock();
            //textBlock.SetBinding(TextBlock.TextProperty, new Binding("SyntaxNode") {Source = d});
            //ss.Children.Add(textBlock);
            ss.Children.Add(d);
            d.VerticalAlignment = VerticalAlignment.Stretch;
            d.HorizontalAlignment = HorizontalAlignment.Stretch;
            var n = SyntaxFactory.ParseCompilationUnit(AnalysisAppLib.Properties.Resources.Program_Parse)
                .NormalizeWhitespace("    ");
            var stree = SyntaxFactory.SyntaxTree(n);
            d.SyntaxTree = stree;
            var compilation = AnalysisService.CreateCompilation("x", stree);
            d.Compilation = compilation;
            d.Model = compilation.GetSemanticModel(stree);
            d.Node = stree.GetRoot();


            //CustomControl2 c = new CustomControl2() { Type = type2 };
            var w = new Window();
            //w.Padding = new Thickness(10);
            //c.Margin = new Thickness(15);
            w.Content = ss;
            w.ShowActivated = true;
            w.ShowDialog();
            return;

            // StackPanel p1 = new StackPanel() {Orientation = Orientation.Vertical};
            // StackPanel p2 = new StackPanel() { Orientation = Orientation.Horizontal };
            // StackPanel p3 = new StackPanel() { Orientation = Orientation.Horizontal };
            // p1.Children.Add(p2);
            // p2.Children.Add(c);
            // CustomControl2 c2 = new CustomControl2() { Type = type };
            // p2.Children.Add(c2);
            // p1.Children.Add(p3);

            // Type type3 = typeof(object[]);

            // CustomControl2 c3 = new CustomControl2() { Type = type3 };

            // Type type4 = typeof(IEnumerable<>);
            // CustomControl2 c4 = new CustomControl2() { Type = type };
            // p3.Children.Add(c3);
            // p3.Children.Add(c4);
            // w.Content = p1;
            // w.ShowDialog();
        }

        [WpfFact]
        public void TestDiag()
        {
            ProjTestsHelper.TestSyntaxControl(new CodeDiagnostics());
        }

        [WpfFact]
        public void TestCodeControl2()
        {
            ProjTestsHelper.TestSyntaxControl(new CodeControl2());
        }

        [WpfFact]
        public void TestText()
        {
            double EmSize = 12;
            Visual h = new Border();
            var PixelsPerDip = VisualTreeHelper.GetDpi(h).PixelsPerDip;
            var syntaxTree = ProjTestsHelper.SetupSyntaxParams(out var compilation);
            var Store = FormattingHelper.UpdateTextSource(syntaxTree.GetRoot(), compilation, syntaxTree, PixelsPerDip,
                EmSize);
            var formatter = TextFormatter.Create();
            var textStorePosition = 0;
            var OutputWidth = 800;
            var Typeface = new Typeface("Courier New");

            var CurrentRendering = new FontRendering(
                EmSize,
                TextAlignment.Left,
                null,
                Brushes.Black,
                Typeface);
            double maxX = 0;
            
            var line = 0;
            
            
            var group = new DrawingGroup();
            var dc = group.Open();

            var context = new LineContext();

            IList<LineInfo> allLineInfos = new List<LineInfo>();
            while (context.TextStorePosition < Store.Length)
                using (var myTextLine = formatter.FormatLine(
                    Store,
                    context.TextStorePosition,
                    OutputWidth,
                    new GenericTextParagraphProperties(CurrentRendering, PixelsPerDip),
                    null))
                {

                    var infos = new List<RegionInfo>();
                    context.MyTextLine = myTextLine;
                    FormattingHelper.HandleTextLine(infos, ref context, dc, out var lineInfo);
                    
                    allLineInfos.Add(lineInfo);
                }

            foreach (var allLineInfo in allLineInfos)
            {
                if (allLineInfo == null)
                    throw new InvalidOperationException();
                if (allLineInfo.Regions == null) throw new InvalidOperationException(allLineInfo.LineNumber.ToString());
                foreach (var regionInfo in allLineInfo.Regions)
                {
                    if (regionInfo == null) throw new InvalidOperationException();
                    foreach (var ch in regionInfo.Characters)
                        if (ch == null)
                            throw new InvalidOperationException();
                }
            }

            var outq = (from li in allLineInfos
                let x = li
                from r in li.Regions
                from ch in r.Characters
                select ProjTestsHelper.Merge(ch, r, li)).ToList();

            using (var writer = new CsvWriter(new ExcelSerializer("c:\\temp\\out.xlsx")))
            {
                writer.WriteRecords(allLineInfos);
            }

            using (var writer = new CsvWriter(new ExcelSerializer("c:\\temp\\out2.xlsx")))
            {
                writer.WriteRecords(outq);
            }
        }

        [WpfFact]
        public void TestEnhanced()
        {
            ProjTestsHelper.TestSyntaxControl(new EnhancedCodeControl());
        }

        [WpfFact]
        public void TestFormattedControl()
        {
            ProjTestsHelper.TestSyntaxControl(new FormattedTextControl());
        }
        [WpfFact]
        public void TestFormattedControlVb()
        {
            ProjTestsHelper.TestSyntaxControlVb(new FormattedTextControl());
        }
        [WpfFact]
        public void TestSymbolControl()
        {
            Main1Model.SelectVsInstance();
            Main1Model model  = new Main1Model();
            model.LoadSolution(solutionPath).ContinueWith(async task =>
            {
                var resources = ProjTestsHelper.ControlsResources();
                // var control = new SymbolTextControl();
                // control.BorderThickness = new Thickness(3);
                // control.BorderBrush = Brushes.Pink;
                // control.VerticalAlignment = VerticalAlignment.Stretch;
                // control.HorizontalAlignment = HorizontalAlignment.Stretch;
                //                var tree = ProjTestsHelper.SetupSyntaxParams(out var compilation);

                List<ISymbol> symbols= new List<ISymbol>();
                foreach (var proj in model.Workspace.CurrentSolution.Projects)
                {
                    var comp = await proj.GetCompilationAsync();
                    foreach (var symbol in comp.GetSymbolsWithName(x => true))
                    {
                        
                        symbols.Add(symbol);
                    }
                }
                // var q = sm.Select(z => Tuple.Create(z, z.ToDisplayParts(SymbolDisplayFormat.MinimallyQualifiedFormat)))
                // .OrderByDescending(zz => zz.Item2.Length);
                // control.DisplaySymbol = q.First().Item1;

                ListBox listBox = new ListBox() {ItemsSource = symbols};

                var w = new Window {Content = listBox, ShowActivated = true, Resources = resources};
                w.ShowDialog();
            }).Wait();
        }

        [WpfFact]
        public void TestLogInstanceControl()
        {
            var c = new LogEventInstancesControl();

            var l = new LogEventInstanceObservableCollection();
            l.Add(new LogEventInstance() { Level = 1, LoggerName = "foo", FormattedMessage = "test 123"});
            c.EventsSource = l;
                var resources = ProjTestsHelper.ControlsResources();
            
                var w = new Window { Content = c, ShowActivated = true, Resources = resources };
                w.ShowDialog();
        }


        [WpfFact]
        public void TestSemanticControl1()
        {
            ProjTestsHelper.TestSyntaxControl(new SemanticControl1());
        }

        [WpfFact]
        public void TestMain1()
        {
            Main1Model.SelectVsInstance();
            var r = ProjTestsHelper.ControlsResources();
            Main1 mainw = new Main1();
            mainw.Resources = r;
            Window w = new Window()
            {
                Content = mainw
            };
            TaskCompletionSource<int> t = new TaskCompletionSource<int>();
            
            ReplaySubject<Workspace> replay = new ReplaySubject<Workspace>();
            mainw.ViewModel = new Main1Model(replay);
            mainw.AddHandler(WorkspaceView.SelectedProjectChangedEvent,
                new RoutedPropertyChangedEventHandler<ProjectModel>(Target), true);

            w.Loaded += async (sender, args) =>
            {
                try
                {
                    if (mainw.ViewModel.WorkspaceView != null)
                    {
                        mainw.ViewModel.WorkspaceView.SelectedDocumentChanged +=
                            (sender1, args1) => DebugUtils.WriteLine(args1.NewValue.Name);
                        mainw.ViewModel.WorkspaceView.SelectedProjectChanged +=
                            (sender2, args2) =>
                            {
                                DebugUtils.WriteLine(args2.NewValue.Name + $" {args2.NewValue.Documents.Count}" + " [" +
                                                     String.Join(", ",
                                                         args2.NewValue.RootPathInfo.Entries.Values
                                                             .Select(x => x.Path)) +
                                                     "]");
                            };
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    await mainw.ViewModel.LoadSolution(solutionPath);
                }
                catch(Exception ex)
                {
                    DebugUtils.WriteLine(ex.ToString());
                }
            };

            w.Closed += (sender, args) =>
            {
                t.SetResult(0);
            };
            w.ShowDialog();
            //Task.WaitAll(t.Task);

        }

        private void Target(object sender, RoutedPropertyChangedEventArgs<ProjectModel> e)
        {
            
        }


        [WpfFact]
        public void TestAllCommand()
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

                //       var workspaceReplaySubject = new ReplaySubject<AdhocWorkspace>();
                //     var progress = new ReplaySubject<CommandProgress>();
                var lifetimeScope = instance.GetLifetimeScope(containerBuilder =>
                {
                    //containerBuilder.RegisterInstance(workspaceReplaySubject).AsSelf().AsImplementedInterfaces();
                    //containerBuilder.RegisterInstance(progress).AsSelf().AsImplementedInterfaces();
                });
                var allCommands = lifetimeScope.Resolve<AllCommands>();
                foreach (var meta in allCommands.Commands1)
                {
                    DebugUtils.WriteLine("Command");
                    var props = MetaHelper.GetMetadataProps(meta.Metadata);
                    DebugUtils.WriteLine($"{props.Title} - {props.Category}");
                    // foreach (var keyValuePair in meta.Metadata)
                    // {
                        // DebugUtils.WriteLine($"{keyValuePair.Key} = {keyValuePair.Value}");
                    // }
                }
                var e = allCommands.GetComponent();

            }
        }
        [WpfFact]
        public void TestWorkspaceView()
        {
            var c = new WorkspaceView();
            System.Reactive.Subjects.ReplaySubject<Workspace> replay= new ReplaySubject<Workspace>();
            Main1Model model = new Main1Model(replay);
            model.CreateWorkspace();
            c.SetBinding(WorkspaceView.SolutionsProperty, new Binding("HierarchyRoot") {Source = model});
            model.CreateProject();
            model.AddDocument(model.HierarchyRoot[0].Projects[0], @"C:\temp\program.cs");
            
//            model.Workspace.AddProject("test", LanguageNames.CSharp);
            Window w = new Window() {Content = c};
            w.ShowDialog();
        }

        [WpfFact]
        public void TestWorkspaceView2()
        {
            AppDomain.CurrentDomain.AssemblyLoad +=
                (sender, args) => DebugUtils.WriteLine(args.LoadedAssembly.FullName);
            var c = new WorkspaceView();
            System.Reactive.Subjects.ReplaySubject<Workspace> replay = new ReplaySubject<Workspace>();
            Main1Model model = new Main1Model(replay);
            model.LoadSolution(
                solutionPath);
            c.SetBinding(WorkspaceView.SolutionsProperty, new Binding("HierarchyRoot") { Source = model });
            Window w = new Window() { Content = c };
            w.ShowDialog();
        }

        [WpfFact]
        public void TestWorkspaceModel()
        {
            
            System.Reactive.Subjects.ReplaySubject<Workspace> replay = new ReplaySubject<Workspace>();
            Main1Model model = new Main1Model(replay);
            model.LoadSolution(
                solutionPath).ContinueWith(
                task =>
                {
                    var sol = model.HierarchyRoot.FirstOrDefault();
                    foreach (var projectModel in sol.Projects)
                    {
                        foreach (var projectModelDocument in projectModel.Documents)
                        {
                            DebugUtils.WriteLine(projectModelDocument.Name);
                        }
                    }
                });
            
        }
        
        [WpfFact]
        public void TestMain1_()
        {
            Assert.True(MSBuildLocator.CanRegister);
            ProjectModel m = new ProjectModel(null);
            m.Documents.Add(new DocumentModel(m, null){FilePath = "test\\one\\tewo"});
            foreach (var kv in m.RootPathInfo.Entries)
            {
                DebugUtils.WriteLine(kv.Value.ToString());
            }
        }

        [WpfFact]
        public void TestTypeAdapter()
        {
            string code = "namespace foo.bar { public class foo { class bar { } } }";
            var tree = ProjTestsHelper.SetupSyntaxParams(out var comp, code);
            foreach (var typeSymbol in comp.GetSymbolsWithName(n => true).OfType<ITypeSymbol>())
            {
                switch (typeSymbol)
                {
                    case IArrayTypeSymbol arrayTypeSymbol:
                        break;
                    case IDynamicTypeSymbol dynamicTypeSymbol:
                        break;
                    case IErrorTypeSymbol errorTypeSymbol:
                        break;
                    case INamedTypeSymbol namedTypeSymbol:
 		    
                DebugUtils.WriteLine(namedTypeSymbol.ConstructedFrom.ToString());
                        break;
                    case IPointerTypeSymbol pointerTypeSymbol:
                        break;
                    case ITypeParameterSymbol typeParameterSymbol:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(typeSymbol));
                }
                
                TypeInfoProvider2 prov = new TypeInfoProvider2(typeSymbol);
                DebugUtils.WriteLine(typeSymbol.ToDisplayString());
                DebugUtils.WriteLine(prov.IsNested.ToString());
                DebugUtils.WriteLine(prov.Assembly.Name.ToString());
                DebugUtils.WriteLine(typeof(string).AssemblyQualifiedName);
                CustomControl2 cc = new CustomControl2() {TypeInfoProvider = prov};
                Window w = new Window {Content = cc};
                w.ShowDialog();
            }
        }

        [WpfFact]
        public void TestMdodel1()
        {
            Main1Model model = new Main1Model();
            model.CreateWorkspace();

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            model.Documents.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var eNewItem in e.NewItems)
                    {
                        if (eNewItem is DocModel d)
                        {
                            if (d.Content is FormattedTextControl fmt)
                            {
                                tcs.SetResult(fmt.SyntaxTree);
                                
                            }
                        }
                    }
                }
            }
            ;
            model.ProjectedAddedEvent += (sender, args) => { model.AddDocument(args.Model, @"C:\temp\program.cs"); };
            model.DocumentAddedEvent += async (sender, args) =>
            {
                await model.OpenSolutionItem(args.Document);
            };
            var p = model.CreateProject(); ;
            tcs.Task.Wait(5000);
            if (tcs.Task.Result is SyntaxTree t)
            {
                DebugUtils.WriteLine(t.Length.ToString());
            }


        }

        [WpfFact]
        public void TestRibbonModel()
        {
            RibbonModel m = new RibbonModel();
            RibbonTabProvider1 p = new RibbonTabProvider1();
            var t = p.ProvideModelItem(null);
            m.RibbonItems.Add(t);
            RibbonViewGroupProviderBaseImpl x = new RibbonViewGroupProviderBaseImpl();
            t.Items.Add(x.ProvideModelItem(null));
            TemplateWindow window = new TemplateWindow();
            window.Content = new Ribbon {ItemsSource = m.RibbonItems};
            window.ShowDialog();

        }



        private void Documents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
         
        }
        [WpfFact]
        public void TestCodeEntry()
        {

            FormattedTextControl  c = new FormattedTextControl();
            Window w = new Window {Content = c};
            w.Loaded += (sender, args) =>
            {
                Queue<string> texts = new Queue<string>();
                texts.Enqueue("public ");
                texts.Enqueue("class {\n ");
                texts.Enqueue("}");

                void DoText(string arg)
                {
                    var textCompositionEventArgs = new TextCompositionEventArgs(
                        InputManager.Current.PrimaryKeyboardDevice,
                        new TextComposition(InputManager.Current, c, arg));
                    textCompositionEventArgs.RoutedEvent = TextCompositionManager.PreviewTextInputEvent;
                    c.RaiseEvent(textCompositionEventArgs);
                }

                async Task DoTexts(Task t)
                {
                    
                    if (texts.Any())
                    {
                        var text = texts.Dequeue();
                        DoText(text);
                        await Task.Delay(1000);
                        await DoTexts(null);
                    }
                }

                DoTexts(null).ContinueWith(t => w.Close());
            };
            w.ShowDialog();
        }

        public TypesViewModel model()
        {
            using (var stream = typeof(AnalysisControlsModule).Assembly
                .GetManifestResourceStream(
                    "AnalysisControls.TypesViewModel.xaml"
                ))
            {
                if (stream == null)
                {
                    DebugUtils.WriteLine("no stream");
                    return new TypesViewModel(
                    );
                }

                try
                {
                    var v = (TypesViewModel) XamlReader
                        .Load(stream);
                    stream.Close();
                    return v;
                }
                catch (Exception)
                {
                    return new TypesViewModel();
                }
            }
        }
        [WpfFact]
        public void TestC1()
        {
            var model = this.model();
            // model.BeginInit();
            // model.EndInit();
            Random r = new Random();
            var tt=model.GetAppTypeInfos().Where(t=>t.Fields.Count>0).Skip(r.Next(100)).First();
            var c = new SyntaxPanel(){SyntaxTypeInfo = tt};
            Window w = new Window(){Content=c};
            w.ShowDialog();
        }
        [WpfFact]
        public void TestGradient()
        {
            var model = this.model();
            // model.BeginInit();
            // model.EndInit();
            Random r = new Random();
            var tt = model.GetAppTypeInfos().Where(t => t.Fields.Count > 0).Skip(r.Next(100)).First();
            var c = new GradientEditorControl();
            Window w = new Window() { Content = c };
            w.ShowDialog();
        }

        [WpfFact]
        void TestParseXaml()
        {
            var DESKTOPdIR = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\KayMcCormick.Dev\src\KayMcCormick.Dev\Desktop";
            var analysisDir = $@"{DESKTOPdIR}\Analysis\";
            var s = $@"{analysisDir}AnalysisControls\Themes\Generic.xaml";
            var s2 = $@"{analysisDir}AnalysisControls\EnhancedCodeWindow.xaml";
            var s1 = $@"{DESKTOPdIR}\TestApp\MainTestWindow.xaml";
            foreach(var name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
{
Assembly.Load(name);
}
            XamlFuncs.ParseXaml(
                s);
        }

        [WpfFact]
        public void TestPanel()
        {
            var c = new WrapPanel();
            TablePanel panel = new TablePanel() {RowSpacing = 3, ColumnSpacing = 10, NumColumns = 3};
            c.Children.Add(panel);
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
            {
                panel.Children.Add(new TextBlock {Text = environmentVariable.Key.ToString()});
                panel.Children.Add(new TextBlock {Text = environmentVariable.Value.ToString()});
            }

            Window w = new Window {Content = c};
            w.ShowDialog();
        }

        [WpfFact]
        public void TestPanel2()
        {
            var c = new WrapPanel();
            TablePanel panel = new TablePanel() { RowSpacing = 3, ColumnSpacing = 10, NumColumns = 3 };
            c.Children.Add(panel);
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
            {
                // var row = new TableRow();
                // row.Children.Add(new TextBlock { Text = environmentVariable.Key.ToString() });
                // row.Children.Add(new TextBlock { Text = environmentVariable.Value.ToString() });
            }

            Window w = new Window { Content = c };
            w.ShowDialog();
        }
        // [WpfFact]
        // public void TestPanel3()
        // {
            // var c = new WrapPanel();
            // TablePanel panel = new TablePanel() { RowSpacing = 3, ColumnSpacing = 10, NumColumns = 2 };
            // var x = VisualTreeHelper.GetChildrenCount(panel);
            // DebugUtils.WriteLine(x.ToString());
            // panel.Children.Add(new TextBlock { Text = "foo" });
            // panel.Children.Add(new TextBlock { Text = "foo2" });
            // var uiElement = new TableRow();
            // uiElement.Children.Add(new TextBlock { Text = "bar" });
            // var textBlock = new TextBlock { Text = "bar2" };
            // uiElement.Children.Add(textBlock);

            // DebugUtils.WriteLine(VisualTreeHelper.GetParent(textBlock));
            // panel.Children.Add(uiElement);
            // c.Children.Add(panel);
            // Window w = new Window { Content = c };
            // w.Loaded += (sender, args) => DebugUtils.WriteLine(VisualTreeHelper.GetChildrenCount(panel).ToString());
            // w.ShowDialog();
        // }

        [WpfFact]
        public void TestAssembliesControl()
        {
            var c = new WrapPanel();
            var panel = new AssembliesControl();
panel.AssemblySource = AppDomain.CurrentDomain.GetAssemblies();
            c.Children.Add(panel);

            Window w = new Window {Content = c};
            w.ShowDialog();
        }
       [WpfFact]
        public void TestMethod1()
        {
            var c = new WrapPanel();
            var model = TypesViewModelFactory.CreateModel();
            var panel = new SyntaxFactoryPanel() { AppMethodInfo = model.GetAppTypeInfos().SelectMany(x=>x.FactoryMethods).First() };
            c.Children.Add(panel);

            Window w = new Window {Content = c};
            w.ShowDialog();
        }
        [WpfFact]
        public void TestAR1()
        {
            var c = new WrapPanel();
            
            var panel = new AssemblyResourceTree() { Assembly = typeof(AssemblyResourceTree).Assembly };
            c.Children.Add(panel);

            Window w = new Window { Content = c };
            w.ShowDialog();
        }
        [WpfFact]
        public void TestAR2()
        {
            var c = new StackPanel() {Orientation = Orientation.Horizontal};
            foreach (var name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly.Load(name);
            }

            var left = new AssembliesControl {AssemblySource = AppDomain.CurrentDomain.GetAssemblies(), MaxWidth = 400};
            var panel = new AssemblyResourceTree();
            panel.SetBinding(AssemblyResourceTree.AssemblyProperty, new Binding("SelectedAssembly") {Source = left});
            c.Children.Add(left);
            c.Children.Add(panel);
            using (var hexa = new WpfHexaEditor.HexEditor())
            {
                c.Children.Add(hexa);

                panel.SelectedItemChanged += OnPanelOnSelectedItemChanged;
                Window w = new Window {Content = c};
                w.ShowDialog();
            }
        }


        [WpfFact]
        public void TestAR3()
        {
            var c = new StackPanel() { Orientation = Orientation.Horizontal };
            foreach (var name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly.Load(name);
            }

            var assemblySource = AppDomain.CurrentDomain.GetAssemblies();
            var model = new AssemblyResourceModel();
            foreach (var assembly in assemblySource)
            {
                model.Assemblies.Add(assembly);
            }

            var left = new AssembliesControl();
            left.SetBinding(AssembliesControl.AssemblySourceProperty, new Binding("Assemblies") { Source = model });
            left.SetBinding(AssembliesControl.SelectedAssemblyProperty, new Binding("SelectedAssembly") { Source = model, Mode=BindingMode.TwoWay });
            left.MaxWidth = 400;
            var panel = new AssemblyResourceTree();
            panel.SetBinding(AssemblyResourceTree.AssemblyProperty, new Binding("SelectedAssembly") { Source = model, Mode=BindingMode.OneWay});
            c.Children.Add(left);
            c.Children.Add(panel);
            using (var hexa = new WpfHexaEditor.HexEditor())
            {
                c.Children.Add(hexa);

                panel.SelectedItemChanged += OnPanelOnSelectedItemChanged;
                
                var w = new Window { Content = c };
                w.Loaded += (sender, args) =>
                {
                    model.SelectedAssembly = typeof(AnalysisControlsModule).Assembly;
                };
                w.ShowDialog();
            }
        }

        private static async void OnPanelOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            // var node = args.NewValue;
            // var subnodeData = ((NodeBase) node);
            // var result = subnodeData.CheckLoadItems(out var state);
            // if (state == NodeDataLoadState.RequiresAsync)
            // {
                // var data = await subnodeData.CheckLoadItemsAsync();
            // }

            
        }
    }

    public class AssemblyResourceModel : INotifyPropertyChanged
    {
        private INodeData _selectedNode;
        private Assembly _selectedAssembly;
        private ObservableCollection<Assembly> _assemblies = new ObservableCollection<Assembly>();

        public ObservableCollection<Assembly> Assemblies
        {
            get { return _assemblies; }
            set
            {
                if (Equals(value, _assemblies)) return;
                _assemblies = value;
                OnPropertyChanged();
            }
        }

        public Assembly SelectedAssembly
        {
            get { return _selectedAssembly; }
            set
            {
                if (Equals(value, _selectedAssembly)) return;
                _selectedAssembly = value;
                OnPropertyChanged();
            }
        }

        public INodeData SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                if (Equals(value, _selectedNode)) return;
                _selectedNode = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}