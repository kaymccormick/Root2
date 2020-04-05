using System ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows.Forms ;
using AnalysisAppLib ;
using AnalysisAppLib.Dataflow ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;

namespace ModelTests
{
    [ BeforeAfterLogger ]
    [ LogTestMethod ]
    public sealed class TestModel : IDisposable , IClassFixture < LoggingFixture >
    {
        public TestModel ( ITestOutputHelper outputHelper , [ NotNull ] LoggingFixture loggingFixture )
        {
            _outputHelper = outputHelper ;
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

        private static readonly Logger              Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        ITestOutputHelper   _outputHelper ;
        private readonly        LoggingFixture      _loggingFixture ;
        private readonly        ApplicationInstance _app ;
        private readonly        bool                _disableLogging ;

        public bool DisableLogging { get { return _disableLogging ; } }


        [ NotNull ]
        private ApplicationInstance SetupApplicationInstance ( )
        {
            var applicationInstance = new ApplicationInstance (
                                                               new
                                                                   ApplicationInstance.ApplicationInstanceConfiguration (
                                                                                                                         LogMethod
                                                                                                                       , ApplicationGuid
                                                                                                                       , null
                                                                                                                       , disableLogging : DisableLogging
                                                                                                                       , disableRuntimeConfiguration : true
                                                                                                                       , disableServiceHost : true
                                                                                                                        )
                                                              ) ;
            applicationInstance.AddModule ( new AnalysisAppLibModule ( ) ) ;
            applicationInstance.Initialize ( ) ;
            var lifetimeScope = applicationInstance.GetLifetimeScope ( ) ;
            Assert.NotNull ( lifetimeScope ) ;
            applicationInstance.Startup ( ) ;
            return applicationInstance ;
        }

        public Guid ApplicationGuid { get ; } = new Guid ("5df44dce-af4d-4578-956d-d2b47f233fd0");

        private void LogMethod ( string message )
        {
            //PROVIDER_GUID.EventWriteEVENT_TEST_OUTPUT ( message ) ;
            Debug.WriteLine ( message ) ;
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
                                                             item => Debug.WriteLine (
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
                [ NotNull ] ILifetimeScope ls
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
            x.Completion.ContinueWith ( task => LogMethod ( x.OutputCount.ToString ( ) ) ) ;

            var compilation = await project.GetCompilationAsync ( ) ;
            foreach ( var diagnostic in compilation.GetDiagnostics ( ) )
            {
                Debug.WriteLine (new DiagnosticFormatter().Format(diagnostic)  );
            }
            foreach ( var @ref in project.MetadataReferences )
            {
                switch(@ref)
                {
                    case CompilationReference compilationReference :
                        Debug.WriteLine(compilationReference.Display);
                        break;
                    case PortableExecutableReference portableExecutableReference :
                        Debug.WriteLine(portableExecutableReference.FilePath);
                        break ;
                    case UnresolvedMetadataReference unresolvedMetadataReference :
                        Debug.WriteLine(unresolvedMetadataReference.Display);
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

            if ( ! x.Post ( project.Documents.First() ) )
            {
                throw new InvalidOperationException ( "doco faild to post" ) ;
            }

            x.Complete ( ) ;
            return Tuple.Create ( f , x , bb ) ;
        }

        private static async Task < Project > SetupMsBuildProject < T > ( )
        {
            var workspace = MSBuildWorkspace.Create ( ) ;
            var solution = await workspace.OpenSolutionAsync (
                                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v2\LogTest\LogTest.sln"
                                                             ) ;
            var project = solution.Projects.First ( ) ;
            return project ;
        }

        private static Project SetupAdHocWorkspace < T > ( )
        {
            var code = File.ReadAllText ( @"c:\temp\program-parse.cs" ) ;
            var xxxx = AnalysisService.Parse ( code , "parse" ) ;
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
                                                                                      , LanguageNames.CSharp
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
            var s2 = s.AddDocuments ( ImmutableArray < DocumentInfo >.Empty.Add ( documentInfo ) ) ;

            //var d = project.AddDocument ( "test.cs" , src ) ;
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
                                                                   , MetadataReference.CreateFromFile (
                                                                                                       typeof
                                                                                                           ( Logger
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


        // [ Fact ]
        // public void Te4t3 ( )
        // {
            // ITypesViewModel viewModel = new TypesViewModel ( ) ;
            // try
            // {
                // var options = new JsonSerializerOptions ( ) ;
                // options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
                // options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;
                // options.WriteIndented = true ;
                // var model =
                    // JsonSerializer.Deserialize < TypesViewModel > (
                                                                   // File.ReadAllText (
                                                                                     // @"C:\data\logs\viewmodel.json"
                                                                                    // )
                                                                  // ) ;
            // }
            // catch ( JsonException ex )
            // {
                // MessageBox.Show ( "Json failure" , ex.Message ) ;
            // }
        // }

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
            using ( var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( b => {
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

                                                                               b.RegisterGeneric ( typeof ( BlockFactory < ,, > ) ) ;
                                                                               b.Register<TransformFunc <Document, Task<IEnumerable <NodeInfo> >>>(
                                                                               (c) => (Document doc)
                                                                               => Task.FromResult(
                                                                               Enumerable
                                                                               .Empty<NodeInfo>()
                                                                               )
                                                                               );
                                                                               b.Register (
                                                                               ( c , p )
                                                                               => c.Resolve<BlockFactory <Document,NodeInfo,TransformManyBlock <Document, NodeInfo >>> (  )(
                                                                               p.Positional
                                                                               < Func < Document ,
                                                                               Task < IEnumerable < NodeInfo > > >> ( 0 )
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

        // [ Fact ]
        // public void Test3 ( )
        // {
        //     ITypesViewModel viewModel = new TypesViewModel ( ) ;
        //     try
        //     {
        //         var options = new JsonSerializerOptions ( ) ;
        //         options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
        //         options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;
        //         options.WriteIndented = true ;
        //         var json = JsonSerializer.Serialize ( viewModel , options ) ;
        //         File.WriteAllText ( @"C:\data\logs\viewmodel.json" , json ) ;
        //     }
        //     catch ( JsonException ex )
        //     {
        //         MessageBox.Show ( "Json failure" , ex.Message ) ;
        //     }
        // }

        [ Fact ]
        public void TestAppStartup ( )
        {
            using ( var applicationInstance =
                new ApplicationInstance (
                                         new ApplicationInstance.ApplicationInstanceConfiguration (
                                                                                                   LogMethod
                                                                                                 , ApplicationGuid
                                                                                                 , null
                                                                                                 , disableLogging : DisableLogging
                                                                                                 , disableRuntimeConfiguration : true
                                                                                                 , disableServiceHost : true
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
        public void TestLog ( )
        {
            LogManager.ThrowExceptions = false ;
            // var logName = EventLog.LogNameFromSourceName ( "Kay McCormick" , "." ) ;
            // LogMethod(logName);
            // foreach ( var eventLog1 in EventLog.GetEventLogs ( ) )
            // {
            //     LogMethod(eventLog1.ToString());
            //     LogMethod(eventLog1.LogDisplayName);
            //     LogMethod(eventLog1.Log);
            // }
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
            // foreach ( EventLogEntry logEntry in log.Entries.OrderBy<>((x) => ((EventLogEntry)x).TimeGenerated) )
            // {
            // if ( logEntry.InstanceId == 8 )
            // {
            // Debug.WriteLine(logEntry.Source);
            // StringBuilder bb = new StringBuilder();
            // foreach ( var b in logEntry.Data )
            // {
            // bb.Append(((int)b).ToString ( "x2" )) ;

            // }

            // Debug.WriteLine(bb.ToString());

            // }
            // }
        }
    }

    public class NodeInfo
    {
    }
}