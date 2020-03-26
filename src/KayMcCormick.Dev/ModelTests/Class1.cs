using System ;
using System.Collections.Generic ;
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
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;

namespace ModelTests
{
    [ BeforeAfterLogger ]
    [ LogTestMethod ]
    public sealed class Class1 : IDisposable , IClassFixture < LoggingFixture >
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly ITestOutputHelper   _outputHelper ;
        private readonly LoggingFixture      _loggingFixture ;
        private readonly ApplicationInstance _app ;
        private readonly bool                _disableLogging ;

        public Class1 ( ITestOutputHelper outputHelper , [ NotNull ] LoggingFixture loggingFixture )
        {
            _outputHelper = outputHelper ;
            loggingFixture.SetOutputHelper ( _outputHelper ) ;
            _loggingFixture = loggingFixture ;
            _disableLogging = false ;
            _app            = SetupApplicationInstance ( ) ;
        }

        public bool DisableLogging { get { return _disableLogging ; } }


        [ NotNull ]
        private ApplicationInstance SetupApplicationInstance ( )
        {
            var applicationInstance = new ApplicationInstance (
                                                               new
                                                                   ApplicationInstanceConfiguration (
                                                                                                     LogMethod
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

        [ Fact ]
        public void TestAppStartup ( )
        {
            using ( var applicationInstance =
                new ApplicationInstance (
                                         new ApplicationInstanceConfiguration (
                                                                               LogMethod
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
        public void Test1 ( )
        {
            using ( var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( ) )
            {
                var model = ls.Resolve < DockWindowViewModel > ( ) ;
                Assert.NotNull ( model ) ;
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
        public void Test3 ( )
        {
            ITypesViewModel viewModel = new TypesViewModel ( ) ;
            try
            {
                var options = new JsonSerializerOptions ( ) ;
                options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
                options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;
                options.WriteIndented = true ;
                var json = JsonSerializer.Serialize ( viewModel , options ) ;
                File.WriteAllText ( @"C:\data\logs\viewmodel.json" , json ) ;
            }
            catch ( JsonException ex )
            {
                MessageBox.Show ( "Json failure" , ex.Message ) ;
            }
        }


        [ Fact ]
        public void Te4t3 ( )
        {
            ITypesViewModel viewModel = new TypesViewModel ( ) ;
            try
            {
                var options = new JsonSerializerOptions ( ) ;
                options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
                options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;
                options.WriteIndented = true ;
                var model =
                    JsonSerializer.Deserialize < TypesViewModel > (
                                                                   File.ReadAllText (
                                                                                     @"C:\data\logs\viewmodel.json"
                                                                                    )
                                                                  ) ;
            }
            catch ( JsonException ex )
            {
                MessageBox.Show ( "Json failure" , ex.Message ) ;
            }
        }

        private void LogMethod ( string message )
        {
            //PROVIDER_GUID.EventWriteEVENT_TEST_OUTPUT ( message ) ;
            Debug.WriteLine ( message ) ;
            Logger.Info ("test output: {message}", message ) ;
            _outputHelper.WriteLine ( message ) ;

        }


        [ Fact ]
        public void Test11 ( )
        {
            using (var ls = _app.GetLifetimeScope()
                                 .BeginLifetimeScope())

            {
                DoFlow<ILogInvocation> ( ls ) ;
            }
            
        }

        // public delegate TransformManyBlock < Document , NodeInfo > Factory (
            // Func < Document , Task < IEnumerable < NodeInfo > > > transform
        // ) ;
        [Fact]
        public void Test113()
        {
            using (var ls = _app.GetLifetimeScope()
                                .BeginLifetimeScope((b) => {
                                     b.Register ( ( c , p )
                                                      => new ConcreteAnalysisBlockProvider <
                                                          Document , NodeInfo ,
                                                          IPropagatorBlock < Document , NodeInfo >
                                                      > (
                                                         transform => 
                                                             new TransformManyBlock < Document ,
                                                                 NodeInfo > ( transform )
                                                         , new ConcreteDataflowTransformFuncProvider<Document, NodeInfo>(source => Task.FromResult(Enumerable.Empty<NodeInfo>()))));
                                                        
                                     // b.RegisterGeneric ( typeof ( BlockFactory < ,, > ) ) ;
                                    // b.Register<TransformFunc <Document, Task<IEnumerable <NodeInfo> >>>(
                                                // (c) => (Document doc)
                                                  // => Task.FromResult(
                                                                      // Enumerable
                                                                         // .Empty<NodeInfo>()
                                                                     // )
                                               // );
                                    // b.Register (
                                                // ( c , p )
                                                    // => c.Resolve<BlockFactory <Document,NodeInfo,TransformManyBlock <Document, NodeInfo >>> (  )(
                                                       // p.Positional
                                                       // < Func < Document ,
                                                           // Task < IEnumerable < NodeInfo > > >> ( 0 )
                                                      // )
                                               // ) ;

                                }))

            {
                DoFlow<MyTest>(ls);
            }

        }
        private void DoFlow<T> ( ILifetimeScope ls )
        {
            var mainTask = PerformTest<T> ( ls ) ;
            var tasks = new List < Task > ( ) { mainTask } ;
            LogManager.ThrowExceptions = true ;

            TransformManyBlock < Document , T> transformManyBlock = null ;
            BufferBlock < T > bufferBlock = null ;
            IAnalysisBlockProvider < Document , T , TransformManyBlock < Document , T > > f = null ;
            while ( tasks.Any ( ) )
            {
                LogMethod ( string.Join ( "\n" , tasks ) ) ;
                var waitAnyResult = Task.WaitAny ( tasks.ToArray ( ) , 1500 ) ;
                if ( mainTask != null
                     && (mainTask.IsCompleted || mainTask.IsFaulted || mainTask.IsCanceled ))
                {
                    if ( mainTask.IsCompleted )
                    {
                        ( f , transformManyBlock , bufferBlock ) = mainTask.Result ;
                        tasks.Add ( transformManyBlock.Completion ) ;
                    }

                    if ( mainTask.IsFaulted )
                    {
                        throw (Exception)mainTask.Exception ?? new InvalidOperationException() ;
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
                        LogMethod ( "Task faulted with " + task.Exception.ToString ( ) ) ;
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
                                                                                      "reject: " + item
                                                                                         .ToString ( )
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

        private async
            Task < Tuple < IAnalysisBlockProvider < Document , T , TransformManyBlock < Document , T > > , TransformManyBlock < Document , T > , BufferBlock < T > > > PerformTest<T> ( ILifetimeScope ls )
        {
            var code = File.ReadAllText ( @"c:\temp\program-parse.cs" ) ;
            var xxxx = AnalysisService.Parse ( code , "parse" ) ;
            var workspace = new AdhocWorkspace ( ) ;
            workspace.AddSolution (
                                   SolutionInfo.Create (
                                                        SolutionId.CreateNewId ( )
                                                      , VersionStamp.Create ( )
                                                       )
                                  ) ;
            var projectInfo = ProjectInfo.Create (
                                                  ProjectId.CreateNewId ( )
                                                , VersionStamp.Create ( )
                                                , "test"
                                                , "parse"
                                                , LanguageNames.CSharp
                                                 ) ;
            var project = workspace.AddProject ( projectInfo ) ;
            project.AddMetadataReference (
                                          MetadataReference.CreateFromFile (
                                                                            typeof ( NLog.Logger )
                                                                               .Assembly.Location
                                                                           )
                                         ) ;
            var doc = project.AddDocument ( "Program-parse.cs" , SourceText.From ( code ) ) ;
            LogMethod ( doc.Id.ToString ( ) ) ;

            var f = ls
               .Resolve < IAnalysisBlockProvider < Document , T,
                    TransformManyBlock < Document , T> > > ( ) ;
            Assert.NotNull ( f ) ;
            var x = f.GetDataflowBlock ( ) ;
            var bb = new BufferBlock < T > ( ) ;
            x.LinkTo ( bb , new DataflowLinkOptions ( ) { PropagateCompletion = true } ) ;
            //var pipeline = ls.Resolve<Pipeline>();
            var t = x.Completion.ContinueWith (
                                               task => {
                                                   LogMethod ( x.OutputCount.ToString ( ) ) ;
                                               }
                                              ) ;

            var compilation = await project.GetCompilationAsync ( ) ;

            var model = await doc.GetSemanticModelAsync ( ) ;
            if ( model != null )
            {
                LogMethod ( model.SyntaxTree.ToString ( ) ) ;
            }

            if ( ! x.Post ( doc ) )
            {
                throw new InvalidOperationException ( "doco faild to post" ) ;
            }

            x.Complete ( ) ;
            return Tuple.Create (f, x , bb ) ;
        }

        [Fact]
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
            EventLog log=new EventLog("Application");
            var q =
                from entry in log.Entries.Cast < EventLogEntry > ( )
                where entry.InstanceId == 8
                orderby entry.TimeWritten descending 
                select entry;
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

        [Fact]
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
#region IDisposable
        public void Dispose ( )
        {
            _loggingFixture.SetOutputHelper ( null ) ;
            _app?.Dispose ( ) ;
        }
#endregion
    }

    public class NodeInfo
    {
    }
}