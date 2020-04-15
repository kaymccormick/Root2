using System ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Xaml ;
using System.Xml ;
using AnalysisAppLib ;
using AnalysisAppLib.Properties ;
using AnalysisAppLib.Serialization ;
using AnalysisAppLib.ViewModel ;

// ReSharper disable once RedundantUsingDirective
using AnalysisAppLib.XmlDoc ;
using AnalysisAppLib.XmlDoc.Dataflow ;
using AnalysisAppLib.XmlDoc.Serialization ;
using AnalysisAppLib.XmlDoc.ViewModel ;
using Autofac ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using Microsoft.Graph ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;
using Directory = System.IO.Directory ;
using Document = Microsoft.CodeAnalysis.Document ;
using File = System.IO.File ;

namespace ModelTests
{
    [ BeforeAfterLogger ]
    [ LogTestMethod ]
    public sealed class TestModel : IDisposable , IClassFixture < LoggingFixture >
    {
        public TestModel (
            ITestOutputHelper          outputHelper
          , [ NotNull ] LoggingFixture loggingFixture
        )
        {
            _outputHelper = outputHelper ;
            _pocoSyntaxTokenTypeName = $"{_pocoSyntaxNamespaceName}.PocoSyntaxToken" ;
            _pocoBlockSyntaxClassName = $"{_pocoSyntaxNamespaceName}.PocoBlockSyntax" ;
            loggingFixture.SetOutputHelper ( _outputHelper ) ;
            _loggingFixture = loggingFixture ;
            _disableLogging = false ;
            _app            = SetupApplicationInstance ( ) ;
        }

        public void Dispose ( )
        {
            _loggingFixture.SetOutputHelper ( null ) ;
            _app?.Dispose ( ) ;
        }

        private const string LogTestSolution =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v2\LogTest\LogTest.sln" ;
        private const string PocoTypeNamePrefix = "Poco";
        private readonly string _pocoSyntaxNamespaceName = "PocoSyntax" ;
        private readonly string _pocoBlockSyntaxClassName ;
        private readonly string _pocoSyntaxTokenTypeName ;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly        ITestOutputHelper   _outputHelper ;
        private readonly        LoggingFixture      _loggingFixture ;
        private readonly        ApplicationInstance _app ;
        private readonly        bool                _disableLogging ;

        public bool DisableLogging { get { return _disableLogging ; } }


        [ NotNull ]
        private ApplicationInstance SetupApplicationInstance ( )
        {
            var applicationInstance = new ApplicationInstance (
                                                               new ApplicationInstance.
                                                                   ApplicationInstanceConfiguration (
                                                                                                     LogMethod
                                                                                                   , ApplicationGuid
                                                                                                   , null
                                                                                                   , DisableLogging
                                                                                                   , true
                                                                                                   , true
                                                                                                    )
                                                              ) ;
            applicationInstance.AddModule ( new AnalysisAppLibModule ( ) ) ;
            applicationInstance.Initialize ( ) ;
            var lifetimeScope = applicationInstance.GetLifetimeScope ( ) ;
            Assert.NotNull ( lifetimeScope ) ;
            applicationInstance.Startup ( ) ;
            return applicationInstance ;
        }

        public Guid ApplicationGuid { get ; } =
            new Guid ( "5df44dce-af4d-4578-956d-d2b47f233fd0" ) ;

        private void LogMethod ( string message )
        {
            //PROVIDER_GUID.EventWriteEVENT_TEST_OUTPUT ( message ) ;
            DebugUtils.WriteLine ( message ) ;
            Logger.Info ( "test output: {message}" , message ) ;
            _outputHelper.WriteLine ( message ) ;
        }

        private void DoFlow < T > ( [ NotNull ] ILifetimeScope ls )
        {
            var mainTask = PerformTest < T > ( ls ) ;
            var tasks = new List < Task > { mainTask } ;
            LogManager.ThrowExceptions = true ;

            TransformManyBlock < Document , T > transformManyBlock = null ;
            BufferBlock < T > bufferBlock = null ;
            IAnalysisBlockProvider < Document , T , TransformManyBlock < Document , T > > f = null ;
            while ( tasks.Any ( ) )
            {
                LogMethod ( string.Join ( "\n" , tasks ) ) ;
                var waitAnyResult = Task.WaitAny ( tasks.ToArray ( ) , 1500 ) ;
                if ( mainTask != null
                     && ( mainTask.IsCompleted || mainTask.IsFaulted || mainTask.IsCanceled ) )
                {
                    if ( mainTask.IsCompleted )
                    {
                        ( f , transformManyBlock , bufferBlock ) = mainTask.Result ;
                        tasks.Add ( transformManyBlock.Completion ) ;
                    }

                    if ( mainTask.IsFaulted )
                    {
                        throw ( Exception ) mainTask.Exception
                              ?? new InvalidOperationException ( ) ;
                    }

                    mainTask = null ;
                }

                if ( transformManyBlock != null )
                {
                    LogMethod ( transformManyBlock.InputCount.ToString ( ) ) ;
                    LogMethod ( transformManyBlock.OutputCount.ToString ( ) ) ;
                }

                LogMethod ( waitAnyResult.ToString ( ) ) ;
                if ( waitAnyResult < 0 )
                {
                    continue ;
                }

                var task = tasks[ waitAnyResult ] ;
                if ( task.IsFaulted )
                {
                    if ( task.Exception != null )
                    {
                        LogMethod ( "Task faulted with " + task.Exception ) ;
                    }
                }

                LogMethod ( task.Status.ToString ( ) ) ;
                tasks.RemoveAt ( waitAnyResult ) ;
            }

            if ( f is IHaveRejectBlock r )
            {
                var rj = r.GetRejectBlock ( ) ;
                rj.LinkTo (
                           new ActionBlock < RejectedItem > (
                                                             item => DebugUtils.WriteLine (
                                                                                      "reject: "
                                                                                      + item
                                                                                     )
                                                            )
                          ) ;
            }

            // while ( rj.Count != 0 )
            // {
            // if ( x11.RejectBlock.TryReceive ( out var item1 ) )
            // {
            // LogMethod ( "reject " + item1.GetType ( ).ToString ( ) ) ;
            // }
            // }

            Assert.NotNull ( bufferBlock ) ;
            while ( bufferBlock.Count != 0 )
            {
                if ( bufferBlock.TryReceive ( out var item ) )
                {
                    LogMethod ( item.GetType ( ).ToString ( ) ) ;
                }
            }
        }

        [ ItemNotNull ]
        private async
            Task < Tuple <
                IAnalysisBlockProvider < Document , T , TransformManyBlock < Document , T > > ,
                TransformManyBlock < Document , T > , BufferBlock < T > > > PerformTest < T > (
                [ NotNull ] IComponentContext ls
            )
        {
            var project = SetupAdHocWorkspace < T > ( ) ;
            //var project = await SetupMsBuildProject < T > ( ) ;


            var f = ls
               .Resolve < IAnalysisBlockProvider < Document , T ,
                    TransformManyBlock < Document , T > > > ( ) ;
            Assert.NotNull ( f ) ;
            var x = f.GetDataflowBlock ( ) ;
            var bb = new BufferBlock < T > ( ) ;
            x.LinkTo ( bb , new DataflowLinkOptions { PropagateCompletion = true } ) ;
            //var pipeline = ls.Resolve<Pipeline>();
            var continueWith = x.Completion.ContinueWith ( task => LogMethod ( x.OutputCount.ToString ( ) ) ) ;

            var compilation = await project.GetCompilationAsync ( ) ;
            if ( compilation != null )
            {
                foreach ( var diagnostic in compilation.GetDiagnostics ( ) )
                {
                    DebugUtils.WriteLine ( new DiagnosticFormatter ( ).Format ( diagnostic ) ) ;
                }
            }

            foreach ( var @ref in project.MetadataReferences )
            {
                switch ( @ref )
                {
                    case CompilationReference compilationReference :
                        DebugUtils.WriteLine ( compilationReference.Display ) ;
                        break ;
                    case PortableExecutableReference portableExecutableReference :
                        DebugUtils.WriteLine ( portableExecutableReference.FilePath ) ;
                        break ;
                    case UnresolvedMetadataReference unresolvedMetadataReference :
                        DebugUtils.WriteLine ( unresolvedMetadataReference.Display ) ;
                        break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( @ref ) ) ;
                }
            }

            var doc = project.Documents.First ( ) ;
            var model = await doc.GetSemanticModelAsync ( ) ;
            if ( model != null )
            {
                LogMethod ( model.SyntaxTree.ToString ( ) ) ;
            }

            if ( ! x.Post ( project.Documents.First ( ) ) )
            {
                throw new InvalidOperationException ( "FAILED TO POST" ) ;
            }

            x.Complete ( ) ;
            return Tuple.Create ( f , x , bb ) ;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedTypeParameter
        private static async Task < Project > SetupMsBuildProject < T > ( )
        {
            var workspace = MSBuildWorkspace.Create ( ) ;
            var solution = await workspace.OpenSolutionAsync ( LogTestSolution ) ;
            var project = solution.Projects.First ( ) ;
            return project ;
        }

        // ReSharper disable once UnusedTypeParameter
        private static Project SetupAdHocWorkspace < T > ( )
        {
            var code = File.ReadAllText ( @"c:\temp\program-parse.cs" ) ;

            var workspace = new AdhocWorkspace ( ) ;
            var projectId = ProjectId.CreateNewId ( ) ;
            var s = workspace.AddSolution (
                                           SolutionInfo.Create (
                                                                SolutionId.CreateNewId ( )
                                                              , VersionStamp.Create ( )
                                                              , null
                                                              , new[]
                                                                {
                                                                    ProjectInfo.Create (
                                                                                        projectId
                                                                                      , VersionStamp
                                                                                           .Create ( )
                                                                                      , "test"
                                                                                      , "test"
                                                                                      , LanguageNames
                                                                                           .CSharp
                                                                                      , null
                                                                                      , null
                                                                                      , new
                                                                                            CSharpCompilationOptions (
                                                                                                                      OutputKind
                                                                                                                         .ConsoleApplication
                                                                                                                     )
                                                                                       )
                                                                }
                                                               )
                                          ) ;

            var documentInfo = DocumentInfo.Create (
                                                    DocumentId.CreateNewId ( projectId )
                                                  , "test"
                                                  , null
                                                  , SourceCodeKind.Regular
                                                  , TextLoader.From (
                                                                     TextAndVersion.Create (
                                                                                            SourceText
                                                                                               .From (
                                                                                                      code
                                                                                                     )
                                                                                          , VersionStamp
                                                                                               .Create ( )
                                                                                           )
                                                                    )
                                                   ) ;
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
            var s2 = s.AddDocuments ( ImmutableArray < DocumentInfo >.Empty.Add ( documentInfo ) ) ;

            var rb1 = workspace.TryApplyChanges ( s2 ) ;
            if ( ! rb1 )
            {
                throw new InvalidOperationException ( ) ;
            }


            var refs = new[]
                       {
                           @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Microsoft.CSharp.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll"
                         , @"C:\Users\mccor.LAPTOP-T6T0BN1K\.nuget\packages\nlog\4.6.8\lib\net45\NLog.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Configuration.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.DataSetExtensions.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.IO.Compression.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Net.Http.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Runtime.Serialization.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.ServiceModel.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Transactions.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll"
                         , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll"
                       } ;
            foreach ( var ref1 in refs )
            {
                var s3 = workspace.CurrentSolution.AddMetadataReference (
                                                                         projectId
                                                                       , MetadataReference
                                                                            .CreateFromFile ( ref1 )
                                                                        ) ;
                var rb = workspace.TryApplyChanges ( s3 ) ;
                if ( ! rb )
                {
                    throw new InvalidOperationException ( ) ;
                }
            }

            var s4 = workspace.CurrentSolution.AddMetadataReference (
                                                                     projectId
                                                                   , MetadataReference
                                                                        .CreateFromFile (
                                                                                         typeof (
                                                                                                 Logger
                                                                                             ).Assembly
                                                                                              .Location
                                                                                        )
                                                                    ) ;

            var rb2 = workspace.TryApplyChanges ( s4 ) ;
            if ( ! rb2 )
            {
                throw new InvalidOperationException ( ) ;
            }

            var project = workspace.CurrentSolution.Projects.First ( ) ;
            return project ;
        }


        [ Fact ]
        public void Test1 ( )
        {
            using ( var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( ) )
            {
                var model = ls.Resolve < DockWindowViewModel > ( ) ;
                Assert.NotNull ( model ) ;
            }
        }


        [ Fact ]
        public void Test11 ( )
        {
            using ( var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( ) )

            {
                DoFlow < ILogInvocation > ( ls ) ;
            }
        }

        // public delegate TransformManyBlock < Document , NodeInfo > Factory (
        // Func < Document , Task < IEnumerable < NodeInfo > > > transform
        // ) ;
        [ Fact ]
        public void Test113 ( )
        {
            using ( var ls = _app.GetLifetimeScope ( )
                                 .BeginLifetimeScope (
                                                      b => {
                                                          b.Register (
                                                                      ( c , p )
                                                                          => new
                                                                              ConcreteAnalysisBlockProvider
                                                                              < Document , NodeInfo
                                                                                , IPropagatorBlock <
                                                                                      Document ,
                                                                                      NodeInfo > > (
                                                                                                    transform
                                                                                                        => new
                                                                                                            TransformManyBlock
                                                                                                            < Document
                                                                                                              , NodeInfo
                                                                                                            > (
                                                                                                               transform
                                                                                                              )
                                                                                                  , new
                                                                                                        ConcreteDataflowTransformFuncProvider
                                                                                                        < Document
                                                                                                          , NodeInfo
                                                                                                        > (
                                                                                                           source
                                                                                                               => Task
                                                                                                                  .FromResult (
                                                                                                                               Enumerable
                                                                                                                                  .Empty
                                                                                                                                   < NodeInfo
                                                                                                                                   > ( )
                                                                                                                              )
                                                                                                          )
                                                                                                   )
                                                                     ) ;

                                                          b.RegisterGeneric (
                                                                             typeof ( BlockFactory <
                                                                               , , > )
                                                                            ) ;
                                                          b.Register < TransformFunc < Document ,
                                                                  Task < IEnumerable < NodeInfo > >
                                                              >
                                                          > (
                                                             ( c ) => doc
                                                                 => Task.FromResult (
                                                                                     Enumerable
                                                                                        .Empty <
                                                                                             NodeInfo
                                                                                         > ( )
                                                                                    )
                                                            ) ;
                                                          b.Register (
                                                                      ( c , p )
                                                                          => c.Resolve <
                                                                              BlockFactory <
                                                                                  Document ,
                                                                                  NodeInfo ,
                                                                                  TransformManyBlock
                                                                                  < Document ,
                                                                                      NodeInfo > >
                                                                          > ( ) (
                                                                                 p.Positional <
                                                                                     Func < Document
                                                                                       , Task <
                                                                                             IEnumerable
                                                                                             < NodeInfo
                                                                                             > > >
                                                                                 > ( 0 )
                                                                                )
                                                                     ) ;
                                                      }
                                                     ) )

            {
                DoFlow < MyTest > ( ls ) ;
            }
        }

        [ Fact ]
        public void Test2 ( )
        {
            using ( var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( ) )
            {
                var model = ls.Resolve < DockWindowViewModel > ( ) ;
                Assert.NotNull ( model ) ;
                Assert.NotNull ( model.DefaultInputPath ) ;
                Assert.True ( Directory.Exists ( model.DefaultInputPath ) ) ;
            }
        }

        [ Fact ]
        public void TestAppStartup ( )
        {
            using ( var applicationInstance = new ApplicationInstance (
                                                                       new ApplicationInstance.
                                                                           ApplicationInstanceConfiguration (
                                                                                                             LogMethod
                                                                                                           , ApplicationGuid
                                                                                                           , null
                                                                                                           , DisableLogging
                                                                                                           , true
                                                                                                           , true
                                                                                                            )
                                                                      ) )
            {
                applicationInstance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                applicationInstance.Initialize ( ) ;
                var lifetimeScope = applicationInstance.GetLifetimeScope ( ) ;
                Assert.NotNull ( lifetimeScope ) ;
                applicationInstance.Startup ( ) ;
            }
        }

        [ Fact ]
        public void TestEx ( )
        {
            try
            {
                throw new InvalidOperationException ( "TesT" ) ;
            }
            catch ( Exception ex )
            {
                Utils.LogParsedExceptions ( ex ) ;
            }
        }

        [ Fact ]
        public void TestTransform1 ( )
        {
            var result =
                GenTransforms.Transform_Expression ( SyntaxFactory.ParseExpression ( "x + y" ) ) ;
            var opts = new JsonSerializerOptions ( ) ;
            opts.Converters.Add ( new JsonPocoSyntaxConverter ( ) ) ;
            var serialize = JsonSerializer.Serialize ( result , opts ) ;
            DebugUtils.WriteLine ( serialize ) ;
        }

        [ Fact ]
        public void TestLog ( )
        {
            LogManager.ThrowExceptions = false ;

#if false
            foreach (var eventLog1 in EventLog.GetEventLogs().Where (
            eventLog
                                                                          => eventLog.LogDisplayName.Contains (
                                                                                                               "McCormick"
                                                                                                              )
                                                                     ) )
            {
                LogMethod(eventLog1.Log);
                foreach ( EventLogEntry logEntry1 in eventLog1.Entries )
                {
                    if ( logEntry1.InstanceId == 8 )
                    {
                        LogMethod ( logEntry1.Message ) ;
                    }
                }
            }
#endif
            var log = new EventLog ( "Application" ) ;
            var q =
                from entry in log.Entries.Cast < EventLogEntry > ( )
                where entry.InstanceId == 8
                orderby entry.TimeWritten descending
                select entry ;
            foreach ( var bytese in q )
            {
                LogMethod ( bytese.ToString ( ) ) ;
            }
        }

        [ Fact ]
        public void TestModule1 ( )
        {
            ContainerBuilder b = new ContainerBuilder( );
            b.RegisterModule < AnalysisAppLibModule > ( ) ;
            var c = b.Build ( Autofac.Builder.ContainerBuildOptions.None ) ;
            var model = c.Resolve < ITypesViewModel > ( ) ;

        }
        [ Fact ]
        public void TestTemplate ( )
        {
            var expr = SyntaxFactory.ParseExpression("new object[] {}");
            var @expressionout = GenTransforms.Transform_Expression ( expr ) ;
            const string typePropertyName = "Type" ;
            const string fullNamePropertyName = "FullName" ;
            const string titlePropertyName = "Title" ;
            const string elementNamePropertyName = "ElementName" ;
            const string abstractnode = "AbstractNode" ;
            const string predefinednode = "PredefinedNode" ;
            const string subtypenamesPropertyName = "SubTypeNames" ;
            const string typeNamePropertyName = "TypeName";
            const string IsCollectionroprtyName = "IsCollection" ;
            const string ElementTypeMetadataNamePropertyName = "ElementTypeMetadataName" ;
            // ReSharper disable once StringLiteralTypo
            const string cDataLogsTypesJson = @"C:\data\logs\types.json" ;
            var types =
                JsonSerializer.Deserialize < JsonElement > (
                                                                     File.ReadAllText (
                                                                                       cDataLogsTypesJson
                                                                                      )
                                                                    ) ;
            
            var x = JsonSerializer.Deserialize <JsonElement>( "[1,2,3,4]" ) ;
            var expr1 = JsonElementCodeConverter.ConvertJsonElementToCode ( x ) ;
            expr1 = expr1.NormalizeWhitespace ( ) ;
            //var tree1 = CompileExpression ( expr1 ) ;
            //ConvertedExpression1.
            var expression = JsonElementCodeConverter.ConvertJsonElementToCode ( types ) ;
            var tree = CompileExpression ( expression ) ;
            File.WriteAllText(@"C:\temp\expr.cs", tree.ToString (  ));
            
            var dict = new Dictionary < string , string > ( ) ;
            var dict2 = new Dictionary < string , JsonElement > ( ) ;


            foreach ( var typ in types.EnumerateArray() )
            {
                var k = typ.GetProperty ( typePropertyName )
                           .GetProperty ( fullNamePropertyName )
                           .GetString ( ) ;
                k = k.Substring ( k.LastIndexOf ( '.' ) + 1 ) ;

                dict[ k ] = typ.GetProperty ( titlePropertyName )
                               .GetString ( )
                               .Replace ( " " , "_" ) ;
                dict2[ k ] = typ ;
            }

            using ( var fs = new StreamWriter ( @"C:\data\logs\methods.cs" ) )
            {
                foreach ( var typ in types.EnumerateArray() )
                {
                    var typeElement = typ.GetProperty ( typePropertyName ) ;
                    var typeFullName =
                        typeElement.GetProperty ( fullNamePropertyName ).GetString ( ) ;
                    var typeShortName =
                        typeFullName.Substring ( typeFullName.LastIndexOf ( '.' ) + 1 ) ;

                    var pocoClassName = string.Format ( "{0}{1}" , PocoTypeNamePrefix , typeShortName ) ;

                    string body 
                        ;
                    var elementName = typ.GetProperty ( elementNamePropertyName ).GetString ( ) ;
                    DebugUtils.WriteLine ($"{typeShortName}: {elementName}"  );
                    if ( elementName    == abstractnode
                         || elementName == predefinednode )
                    {
                        IEnumerable < string > nodes ( string cname )
                        {
                            if ( dict2[ cname ]
                                .GetProperty ( elementNamePropertyName )
                                .GetString ( )
                                 == abstractnode
                                 || dict2[ cname ]
                                   .GetProperty ( elementNamePropertyName )
                                   .GetString ( )
                                 == predefinednode )
                            {
                                return dict2[ cname ]
                                      .GetProperty ( subtypenamesPropertyName )
                                      .EnumerateArray ( )
                                      .SelectMany ( xx => nodes ( xx.GetString ( ) ) ) ;
                            }
                            else
                            {
                                return new[] { cname } ;
                            }
                        }

                        var cases = string.Join (
                                                 ""
                                               , nodes ( typeShortName )
                                                    .Select (
                                                             sn
                                                                 => $"case {sn} {sn.ToLowerInvariant()}: return Transform_{dict[ sn ]}({sn.ToLowerInvariant()}); \n"
                                                            )
                                                ) ;
                        body = $"switch(node) {{\n{cases}\n}}\nreturn null;\n";
                    }
                    else
                    {
                        var props = "" ;
                        var fields = "" ;
                        foreach ( var f in typ.GetProperty ( "Fields" ).EnumerateArray ( ) )
                        {
                            var name = f.GetProperty("Name").GetString();

                            if ( ( typeFullName.EndsWith ( "StatementSyntax" )
                                   || pocoClassName == PocoBlockSyntaxClassName )
                                 && name == "AttributeLists" )
                            {
                                continue ;
                            }

                            var t1 = f.GetProperty ( typePropertyName ) ;
                            var value = "" ;
                            string transform ;
                            string msg = null ;
                            var typeName = f.GetProperty ( typeNamePropertyName ).GetString ( ) ;
                            if ( typeName == "bool" )
                            {
                                value = $"node.{name}" ;
                                msg   = "bool" ;
                            }
                            else if ( t1.ValueKind == JsonValueKind.Object )
                            {
                                var k = t1.GetProperty ( fullNamePropertyName ).GetString ( ) ;
                                k = k.Substring ( k.LastIndexOf ( '.' ) + 1 ) ;

                                if ( dict.TryGetValue ( k , out var method ) )
                                {
                                    value = $"Transform_{method}(node.{name})" ;
                                }
                                else
                                {
                                    msg = "not found" ;
                                    if ( f.GetProperty ( typeNamePropertyName ).GetString ( )
                                         == "SyntaxToken" )
                                    {
                                        value =
                                            $"new {PocoSyntaxTokenTypeName} {{RawKind = node.{name}.RawKind, Kind = node.{name}.Kind().ToString(), Value = node.{name}.Value, ValueText = node.{name}.ValueText }}" ;
                                    }
                                }
                            }

                            // else if ( typeName.StartsWith ( "SyntaxList<" ) )
                                    // {
                                        // var t2 = typeName.Substring ( 11 , typeName.Length - 12 ) ;
                                        // if ( dict.TryGetValue ( t2 , out var m2 ) )
                                        // {
                                        // }

                                        // transform = $".Select(Transform_{m2}).ToList()" ;
                                    // }
                                    // else if ( typeName.StartsWith ( "SeparatedSyntaxList<" ) )
                                    // {
                                        // var t2 = typeName.Substring ( 20 , typeName.Length - 21 ) ;
                                        // if ( dict.TryGetValue ( t2 , out var m2 ) )
                                        // {
                                        // }
                                        // else
                                        // {
                                        // }

                                        // transform = $".Select(Transform_{m2}).ToList()" ;
                                    // }
                                    // else if ( k == "SyntaxTokenList" )
                                    // {
                                        // transform =
                                            // ".Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList()" ;

                                            
                                            var t2 = f
                                                    .GetProperty (
                                                                  ElementTypeMetadataNamePropertyName
                                                                 )
                                                    .GetString ( ) ;
                                            if ( t2 == "PocoSyntax.PocoSyntaxTokenCollection")
                                            {
                                                
                                            }
                                            var m2 = dict[ t2 ] ;
                                            transform = $".Select(Transform_{m2}).ToList()" ;
                        

                            if ( transform != "" )
                            {
                                value = $"node.{name}{transform}" ;
                            }

                            if ( value == "" )
                            {
                                value = msg ;
                            }

                            var code = $"    {name} = {value}, " ;
                            props = props + "\n" + code ;
                        }

                        body = $"{fields}\nreturn new {pocoClassName}() {{ {props} }};" ;
                    }

                    var methodBody =
                        $"\r\n        /// <summary></summary>\r\n        [NotNull]\r\n        public static {pocoClassName} Transform_{typ.GetProperty ( "Title" ).GetString ( ).Replace ( " " , "_" )} ([NotNull] {typeShortName} node) {{\r\n            if ( node == null )\r\n            {{\r\n                throw new ArgumentNullException ( nameof ( node ) ) ;\r\n            }}\r\n            {body.Replace ( "\n" , "\n            " )}\r\n\r\n        }}\r\n" ;
                    fs.WriteLine ( methodBody ) ;
                }
            }
        }

        public string PocoSyntaxTokenTypeName { get { return _pocoSyntaxTokenTypeName ; } }

        public string PocoBlockSyntaxClassName { get { return _pocoBlockSyntaxClassName ; } }

        private static SyntaxTree CompileExpression ( [ NotNull ] ExpressionSyntax expression )
        {
            var comp = SyntaxFactory.CompilationUnit ( )
                                    .WithMembers (
                                                  new SyntaxList < MemberDeclarationSyntax > ( ).Add (
                                                                                                      SyntaxFactory
                                                                                                         .ClassDeclaration (
                                                                                                                            "ConvertedExpression1"
                                                                                                                           )
                                                                                                         .WithModifiers (
                                                                                                                         SyntaxFactory
                                                                                                                            .TokenList (
                                                                                                                                        SyntaxFactory
                                                                                                                                           .Token (
                                                                                                                                                   SyntaxKind
                                                                                                                                                      .PublicKeyword
                                                                                                                                                  )
                                                                                                                                       )
                                                                                                                        )
                                                                                                         .WithMembers (
                                                                                                                       new
                                                                                                                               SyntaxList
                                                                                                                               < MemberDeclarationSyntax
                                                                                                                               > ( )
                                                                                                                          .Add (
                                                                                                                                SyntaxFactory
                                                                                                                                   .FieldDeclaration (
                                                                                                                                                      SyntaxFactory
                                                                                                                                                         .VariableDeclaration (
                                                                                                                                                                               SyntaxFactory
                                                                                                                                                                                  .PredefinedType (
                                                                                                                                                                                                   SyntaxFactory
                                                                                                                                                                                                      .Token (
                                                                                                                                                                                                              SyntaxKind
                                                                                                                                                                                                                 .ObjectKeyword
                                                                                                                                                                                                             )
                                                                                                                                                                                                  )
                                                                                                                                                                             , new
                                                                                                                                                                                       SeparatedSyntaxList
                                                                                                                                                                                       < VariableDeclaratorSyntax
                                                                                                                                                                                       > ( )
                                                                                                                                                                                  .Add (
                                                                                                                                                                                        SyntaxFactory
                                                                                                                                                                                           .VariableDeclarator (
                                                                                                                                                                                                                "ConvertedExpression"
                                                                                                                                                                                                               )
                                                                                                                                                                                           .WithInitializer (
                                                                                                                                                                                                             SyntaxFactory
                                                                                                                                                                                                                .EqualsValueClause (
                                                                                                                                                                                                                                    expression
                                                                                                                                                                                                                                   )
                                                                                                                                                                                                            )
                                                                                                                                                                                       )
                                                                                                                                                                              )
                                                                                                                                                     ).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token (SyntaxKind.PublicKeyword  )))
                                                                                                                               )
                                                                                                                      )
                                                                                                     )
                                                 )
                                    .NormalizeWhitespace ( ) ;
//            expression = expression.NormalizeWhitespace ( ) ;
            SyntaxTree tree = SyntaxFactory.SyntaxTree ( comp ) ;

            IEnumerable < MetadataReference > refs = new[]
                                                     {
                                                         MetadataReference.CreateFromFile (
                                                                                           typeof ( Object )
                                                                                              .Assembly
                                                                                              .Location
                                                                                          )
                                                     } ;
            var compilation = CSharpCompilation.Create (
                                                        "expr1"
                                                      , new[] { tree }
                                                      , refs
                                                      , new CSharpCompilationOptions (
                                                                                      OutputKind
                                                                                         .DynamicallyLinkedLibrary
                                                                                     )
                                                       ) ;

            foreach ( var diagnostic in compilation.GetDiagnostics ( ) )
            {
                DebugUtils.WriteLine ( diagnostic ) ;
            }

            compilation.Emit ( @"c:\temp\expr1.dll" , @"C:\temp\expr1.pdb" ) ;
            return tree ;
        }
        [Fact]
        public void TestXaml1 ( )
        {
            using ( XamlXmlWriter outwriter = new XamlXmlWriter ( XmlWriter.Create ( @"c:\temp\out.xml" , new XmlWriterSettings ( ) { Indent = true } ) , new XamlSchemaContext ( ) ) ) { }

            var @out = XamlServices.Save(
                                         new SyntaxFieldInfo
                                         {
                                             Name = "test" , Type = typeof ( List < string > )
                                         }
                                        ) ;
            Logger.Info (@out  );

        }


        [ Fact ]
        public void TestTypesService ( )
        {
            SyntaxTypesService sns= new SyntaxTypesService();
            sns.BeginInit();
            sns.EndInit();
        }

        [Fact]
        public void TestSerializeISymbl ( )
        {
            var conv = new JsonSymbolConverterFactory ( ) ;
            JsonSerializerOptions options = new JsonSerializerOptions ( ) ;
            options.Converters.Add ( conv ) ;
            CSharpCompilation comp = CSharpCompilation.Create("test", new[] { SyntaxFactory.ParseSyntaxTree(Resources.Program_Parse)});
            foreach ( var diagnostic in comp.GetDiagnostics() ) {
                if ( diagnostic.Severity >= DiagnosticSeverity.Info )
                {
                    DebugUtils.WriteLine ( diagnostic.ToString ( ) ) ;
                }
            }

            if ( comp.ObjectType != null )
            {
                var outjson = JsonSerializer.Serialize ( comp.ObjectType , options ) ;
                DebugUtils.WriteLine ( outjson ) ;
            }

        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class NodeInfo
    {
    }
}