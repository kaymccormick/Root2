using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Threading ;
using System.Windows ;
using System.Xml.Linq ;
using JetBrains.Annotations ;
using WpfTestApp ;
using Xunit ;
using Xunit.Abstractions ;
using TestAssemblyCleanupFailure = Xunit.Sdk.TestAssemblyCleanupFailure ;
using TestAssemblyFinished = Xunit.Sdk.TestAssemblyFinished ;
using TestAssemblyMessage = Xunit.Sdk.TestAssemblyMessage ;
using TestAssemblyStarting = Xunit.Sdk.TestAssemblyStarting ;
using TestCaseCleanupFailure = Xunit.Sdk.TestCaseCleanupFailure ;
using TestCaseDiscoveryMessage = Xunit.Sdk.TestCaseDiscoveryMessage ;
using TestCaseFinished = Xunit.Sdk.TestCaseFinished ;
using TestCaseMessage = Xunit.Sdk.TestCaseMessage ;
using TestCaseStarting = Xunit.Sdk.TestCaseStarting ;
using TestClassCleanupFailure = Xunit.Sdk.TestClassCleanupFailure ;
using TestClassConstructionFinished = Xunit.Sdk.TestClassConstructionFinished ;
using TestClassConstructionStarting = Xunit.Sdk.TestClassConstructionStarting ;
using TestClassDisposeFinished = Xunit.Sdk.TestClassDisposeFinished ;
using TestClassDisposeStarting = Xunit.Sdk.TestClassDisposeStarting ;
using TestClassFinished = Xunit.Sdk.TestClassFinished ;
using TestClassMessage = Xunit.Sdk.TestClassMessage ;
using TestClassStarting = Xunit.Sdk.TestClassStarting ;
using TestCleanupFailure = Xunit.Sdk.TestCleanupFailure ;
using TestCollectionCleanupFailure = Xunit.Sdk.TestCollectionCleanupFailure ;
using TestCollectionFinished = Xunit.Sdk.TestCollectionFinished ;
using TestCollectionMessage = Xunit.Sdk.TestCollectionMessage ;
using TestCollectionStarting = Xunit.Sdk.TestCollectionStarting ;
using TestFailed = Xunit.Sdk.TestFailed ;
using TestFinished = Xunit.Sdk.TestFinished ;
using TestMessage = Xunit.Sdk.TestMessage ;
using TestMethodCleanupFailure = Xunit.Sdk.TestMethodCleanupFailure ;
using TestMethodFinished = Xunit.Sdk.TestMethodFinished ;
using TestMethodMessage = Xunit.Sdk.TestMethodMessage ;
using TestMethodStarting = Xunit.Sdk.TestMethodStarting ;
using TestOutput = Xunit.Sdk.TestOutput ;
using TestPassed = Xunit.Sdk.TestPassed ;
using TestResultMessage = Xunit.Sdk.TestResultMessage ;
using TestSkipped = Xunit.Sdk.TestSkipped ;
using TestStarting = Xunit.Sdk.TestStarting ;

namespace WpfTestApp
{
    public class DiagnosticMessageSink : MarshalByRefObject , IMessageSink
    {
        private readonly string       assemblyDisplayName ;
        private readonly object       consoleLock ;
        private readonly ConsoleColor displayColor ;
        private readonly bool         noColor ;
        private readonly bool         showDiagnostics ;

        private DiagnosticMessageSink (
            object       consoleLock
          , string       assemblyDisplayName
          , bool         showDiagnostics
          , bool         noColor
          , ConsoleColor displayColor
        )
        {
            this.consoleLock         = consoleLock ;
            this.assemblyDisplayName = assemblyDisplayName ;
            this.noColor             = noColor ;
            this.displayColor        = displayColor ;
            this.showDiagnostics     = showDiagnostics ;
        }

        public static DiagnosticMessageSink ForDiagnostics (
            object consoleLock
          , string assemblyDisplayName
          , bool   showDiagnostics
          , bool   noColor
        )
        {
            return new DiagnosticMessageSink (
                                              consoleLock
                                            , assemblyDisplayName
                                            , showDiagnostics
                                            , noColor
                                            , ConsoleColor.Yellow
                                             ) ;
        }

        // ReSharper disable once UnusedMember.Global
        public static DiagnosticMessageSink ForInternalDiagnostics (
            object consoleLock
          , bool   showDiagnostics
          , bool   noColor
        )
        {
            return new DiagnosticMessageSink (
                                              consoleLock
                                            , null
                                            , showDiagnostics
                                            , noColor
                                            , ConsoleColor.DarkGray
                                             ) ;
        }

        public static DiagnosticMessageSink ForInternalDiagnostics (
            object consoleLock
          , string assemblyDisplayName
          , bool   showDiagnostics
          , bool   noColor
        )
        {
            return new DiagnosticMessageSink (
                                              consoleLock
                                            , assemblyDisplayName
                                            , showDiagnostics
                                            , noColor
                                            , ConsoleColor.DarkGray
                                             ) ;
        }

        public bool OnMessage ( IMessageSinkMessage message )
        {
            if ( showDiagnostics && message is IDiagnosticMessage diagnosticMessage )
            {
                lock ( consoleLock )
                {
                    // if ( ! noColor )
                    // {
                    //     ConsoleHelper.SetForegroundColor ( displayColor ) ;
                    // }

                    if ( assemblyDisplayName != null )
                    {
                        Console.WriteLine (
                                           $"   {assemblyDisplayName}: {diagnosticMessage.Message}"
                                          ) ;
                    }
                    else
                    {
                        Console.WriteLine ( $"   {diagnosticMessage.Message}" ) ;
                    }
                }
            }

            return true ;
        }

#if NETFRAMEWORK
        [System.Security.SecurityCritical]
        public override sealed object InitializeLifetimeService()
        {
            return null;
        }
#endif
    }


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
// ReSharper disable once PartialTypeWithSinglePart
    [ UsedImplicitly ]
    public partial class App : Application
    {
        public App ( )
        {
            Environment.SetEnvironmentVariable("DISABLE_LOGGING", "yes");
            Environment.SetEnvironmentVariable("DISABLE_TEST_LOGGING", "yes");
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);

        }

        private volatile bool   cancel ;
        private readonly object consoleLock = new object ( ) ;

        private readonly XunitProjectAssembly assembly = new XunitProjectAssembly
                                                         {
                                                             AssemblyFilename =
                                                                 @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\Repos\v3\NewRoot\build\test\Debug\x86\ProjTests\ProjTests.dll"
                                                         } ;

        #region Overrides of Application
        protected override void OnStartup ( StartupEventArgs e1 )
        {
//            reporterMessageHandler = MessageSinkWithTypesAdapter.Wrap(reporter.CreateMessageHandler(logger));

            bool failed = false ;
            try
            {
                var discoveryOptions =
                    TestFrameworkOptions.ForDiscovery ( assembly.Configuration ) ;
                var executionOptions =
                    TestFrameworkOptions.ForExecution ( assembly.Configuration ) ;
                executionOptions.SetStopOnTestFail ( true ) ;
                assembly.Configuration.AppDomain = AppDomainSupport.Denied ;
                var assemblyDisplayName =
                    Path.GetFileNameWithoutExtension ( assembly.AssemblyFilename ) ;
                var internalDiagnosticsMessageSink =
                    DiagnosticMessageSink.ForInternalDiagnostics (
                                                                  consoleLock
                                                                , assemblyDisplayName
                                                                , assembly
                                                                 .Configuration
                                                                 .InternalDiagnosticMessagesOrDefault
                                                                , true
                                                                 ) ;

                var diagnosticMessageSink = DiagnosticMessageSink.ForDiagnostics (
                                                                                  consoleLock
                                                                                , assemblyDisplayName
                                                                                , assembly
                                                                                 .Configuration
                                                                                 .DiagnosticMessagesOrDefault
                                                                                , true
                                                                                 ) ;
                var shadowCopy = assembly.Configuration.ShadowCopyOrDefault ;
                var serialize = false ;
                XElement assemblyElement = null ;
                var appDomainSupport = assembly.Configuration.AppDomainOrDefault ;
                var longRunningSeconds = assembly.Configuration.LongRunningTestSecondsOrDefault ;
                // using ( AssemblyHelper.SubscribeResolveForAssembly (
                // assembly.AssemblyFilename
                // , internalDiagnosticsMessageSink
                // ) )
                using ( var controller = new XunitFrontController (
                                                                   appDomainSupport
                                                                 , assembly.AssemblyFilename
                                                                 , assembly.ConfigFilename
                                                                 , shadowCopy
                                                                 , diagnosticMessageSink :
                                                                   diagnosticMessageSink
                                                                  ) )
                using ( var discoverySink = new TestDiscoverySink ( ( ) => cancel ) )
                {
                    // Discover & filter the tests
                    //reporterMessageHandler.OnMessage(new TestAssemblyDiscoveryStarting(assembly, controller.CanUseAppDomains && appDomainSupport != AppDomainSupport.Denied, shadowCopy, discoveryOptions));

                    controller.Find ( false , discoverySink , discoveryOptions ) ;
                    discoverySink.Finished.WaitOne ( ) ;


                    var testCasesDiscovered = discoverySink.TestCases.Count ;
                    var filteredTestCases =
                        discoverySink.TestCases /*.Where(filters.Filter)*/.ToList ( ) ;
                    var testCasesToRun = filteredTestCases.Count ;

//                    reporterMessageHandler.OnMessage(new TestAssemblyDiscoveryFinished(assembly, discoveryOptions, testCasesDiscovered, testCasesToRun));

                    // Run the filtered tests
                    if ( testCasesToRun == 0 )
                    {
                        // completionMessages.TryAdd (
                        // Path.GetFileName ( assembly.AssemblyFilename )
                        // , new ExecutionSummary ( )
                        // ) ;
                    }
                    else
                    {
                        if ( serialize )
                        {
                            filteredTestCases = filteredTestCases
                                               .Select ( controller.Serialize )
                                               .Select ( controller.Deserialize )
                                               .ToList ( ) ;
                        }

                        // reporterMessageHandler.OnMessage(new TestAssemblyExecutionStarting(assembly, executionOptions));

                        // IExecutionSink resultsSink = new DelegatingExecutionSummarySink(reporterMessageHandler, () => cancel, (path, summary) => completionMessages.TryAdd(path, summary));
                        if ( assemblyElement != null )
                        {
                            // resultsSink =
                            // new DelegatingXmlCreationSink (
                            // resultsSink
                            // , assemblyElement
                            // ) ;
                        }

                        if ( longRunningSeconds > 0 )
                        {
                            // resultsSink =
                            // new DelegatingLongRunningTestDetectionSink (
                            //-ja resultsSink
                            // , TimeSpan
                            // .FromSeconds (
                            // longRunningSeconds
                            // )
                            // , MessageSinkWithTypesAdapter
                            // .Wrap (
                            // diagnosticMessageSink
                            // )
                            // ) ;
                        }

                        // if ( failSkips )
                        // {
                        // resultsSink = new DelegatingFailSkipSink ( resultsSink ) ;
                        // }

                        IMessageSinkWithTypes resultsSink = new MySink ( ) ;
                        controller.RunTests ( filteredTestCases , resultsSink , executionOptions ) ;

                        //resultsSink.Finished.WaitOne ( ) ;

                        // reporterMessageHandler.OnMessage (
                        // new TestAssemblyExecutionFinished (
                        // assembly
                        // , executionOptions
                        // , resultsSink
                        // .ExecutionSummary
                        // )
                        // ) ;
                        var stopOnFail = true ;
                        // if ( stopOnFail && resultsSink.ExecutionSummary.Failed != 0 )
                        // {
                        // Console.WriteLine ( "Canceling due to test failure..." ) ;
                        // cancel = true ;
                        // }
                    }
                }
            }

            catch ( Exception ex )
            {
                failed = true ;

                var e = ex ;
                while ( e != null )
                {
                    Console.WriteLine ( $"{e.GetType ( ).FullName}: {e.Message}" ) ;

                    var internalDiagnosticMessages = true ;
                    if ( internalDiagnosticMessages )
                    {
                        Console.WriteLine ( e.StackTrace ) ;
                    }

                    e = e.InnerException ;
                }
            }


            base.OnStartup ( e1 ) ;
            // var w = Helper.Doit ( ) ;
            // w.ShowDialog ( ) ;
            // EventManager.RegisterClassHandler (
                                               // typeof ( Window )
                                             // , FrameworkElement.LoadedEvent
                                             // , new RoutedEventHandler ( Target )
                                              // ) ;
        }

        private void Target ( object sender , RoutedEventArgs e )
        {
            // using (var f = new StreamWriter(@"C:\data\logs\stream.txt"))
            // {
            // XamlWriter.Save(sender, f);
            // }
        }
        #endregion
    }

    [Serializable]
    public class MySink : IMessageSinkWithTypes, IMessageSink
    {
        #region Implementation of IDisposable
        public void Dispose ( ) { }

        public bool OnMessageWithTypes (
            IMessageSinkMessage message
          , HashSet < string >  messageTypes
        )
        {
            switch ( message )
            {
                case AfterTestFinished afterTestFinished : break ;
                case AfterTestStarting afterTestStarting : break ;
                case BeforeTestFinished beforeTestFinished : break ;
                case BeforeTestStarting beforeTestStarting :
                    System.Diagnostics.Debug.WriteLine ( $"before test starting {message}" ) ;
                    break ;
                case DiagnosticMessage diagnosticMessage : break ;
                case DiscoveryCompleteMessage discoveryCompleteMessage : break ;
                case ErrorMessage errorMessage : break ;
                case Xunit.Sdk.AfterTestFinished afterTestFinished1 : break ;
                case Xunit.Sdk.AfterTestStarting afterTestStarting1 : break ;
                case Xunit.Sdk.BeforeTestFinished beforeTestFinished1 : break ;
                case Xunit.Sdk.BeforeTestStarting beforeTestStarting1 : break ;
                case Xunit.Sdk.DiagnosticMessage diagnosticMessage1 : break ;
                case Xunit.Sdk.DiscoveryCompleteMessage discoveryCompleteMessage1 : break ;
                case Xunit.Sdk.ErrorMessage errorMessage1 : break ;
                case TestAssemblyCleanupFailure testAssemblyCleanupFailure : break ;
                case TestAssemblyFinished testAssemblyFinished : break ;
                case TestAssemblyStarting testAssemblyStarting : break ;
                case TestCaseCleanupFailure testCaseCleanupFailure : break ;
                case TestCaseDiscoveryMessage testCaseDiscoveryMessage : break ;
                case TestCaseFinished testCaseFinished : break ;
                case TestCaseStarting testCaseStarting : break ;
                case TestClassCleanupFailure testClassCleanupFailure : break ;
                case TestClassConstructionFinished testClassConstructionFinished : break ;
                case TestClassConstructionStarting testClassConstructionStarting : break ;
                case TestClassDisposeFinished testClassDisposeFinished : break ;
                case TestClassDisposeStarting testClassDisposeStarting : break ;
                case TestClassFinished testClassFinished : break ;
                case TestClassStarting testClassStarting : break ;
                case TestCleanupFailure testCleanupFailure : break ;
                case TestCollectionCleanupFailure testCollectionCleanupFailure : break ;
                case TestCollectionFinished testCollectionFinished : break ;
                case TestCollectionStarting testCollectionStarting : break ;
                case TestFailed testFailed : break ;
                case TestFinished testFinished : break ;
                case TestMethodCleanupFailure testMethodCleanupFailure : break ;
                case TestMethodFinished testMethodFinished : break ;
                case TestMethodStarting testMethodStarting : break ;
                case TestOutput testOutput : break ;
                case TestPassed testPassed : break ;
                case TestSkipped testSkipped : break ;
                case TestStarting testStarting : break ;
                case Xunit.TestAssemblyCleanupFailure testAssemblyCleanupFailure1 : break ;
                case TestAssemblyDiscoveryFinished testAssemblyDiscoveryFinished : break ;
                case TestAssemblyDiscoveryStarting testAssemblyDiscoveryStarting : break ;
                case TestAssemblyExecutionFinished testAssemblyExecutionFinished : break ;
                case TestAssemblyExecutionStarting testAssemblyExecutionStarting : break ;
                case Xunit.TestAssemblyFinished testAssemblyFinished1 : break ;
                case Xunit.TestAssemblyStarting testAssemblyStarting1 : break ;
                case Xunit.TestCaseCleanupFailure testCaseCleanupFailure1 : break ;
                case Xunit.TestCaseDiscoveryMessage testCaseDiscoveryMessage1 : break ;
                case Xunit.TestCaseFinished testCaseFinished1 : break ;
                case Xunit.TestCaseStarting testCaseStarting1 : break ;
                case Xunit.TestClassCleanupFailure testClassCleanupFailure1 : break ;
                case Xunit.TestClassConstructionFinished testClassConstructionFinished1 : break ;
                case Xunit.TestClassConstructionStarting testClassConstructionStarting1 : break ;
                case Xunit.TestClassDisposeFinished testClassDisposeFinished1 : break ;
                case Xunit.TestClassDisposeStarting testClassDisposeStarting1 : break ;
                case Xunit.TestClassFinished testClassFinished1 : break ;
                case Xunit.TestClassStarting testClassStarting1 : break ;
                case Xunit.TestCleanupFailure testCleanupFailure1 : break ;
                case Xunit.TestCollectionCleanupFailure testCollectionCleanupFailure1 : break ;
                case Xunit.TestCollectionFinished testCollectionFinished1 : break ;
                case Xunit.TestCollectionStarting testCollectionStarting1 : break ;
                case TestExecutionSummary testExecutionSummary : break ;
                case Xunit.TestFailed testFailed1 : break ;
                case Xunit.TestFinished testFinished1 : break ;
                case Xunit.TestMethodCleanupFailure testMethodCleanupFailure1 : break ;
                case Xunit.TestMethodFinished testMethodFinished1 : break ;
                case Xunit.TestMethodStarting testMethodStarting1 : break ;
                case Xunit.TestOutput testOutput1 : break ;
                case Xunit.TestPassed testPassed1 : break ;
                case Xunit.TestSkipped testSkipped1 : break ;
                case Xunit.TestStarting testStarting1 : break ;
                case IDiagnosticMessage diagnosticMessage2 : break ;
                case IAfterTestStarting afterTestStarting2 : break ;
                case IBeforeTestFinished beforeTestFinished2 : break ;
                case IBeforeTestStarting beforeTestStarting2 : break ;
                case IAfterTestFinished afterTestFinished2 : break ;
                case IDiscoveryCompleteMessage discoveryCompleteMessage2 : break ;
                case IErrorMessage errorMessage2 : break ;
                case ITestAssemblyCleanupFailure testAssemblyCleanupFailure2 : break ;
                case ITestAssemblyFinished testAssemblyFinished2 : break ;
                case ITestAssemblyStarting testAssemblyStarting2 : break ;
                case ITestCaseCleanupFailure testCaseCleanupFailure2 : break ;
                case ITestCaseDiscoveryMessage testCaseDiscoveryMessage2 : break ;
                case ITestCaseFinished testCaseFinished2 : break ;
                case ITestCaseStarting testCaseStarting2 : break ;
                case ITestClassCleanupFailure testClassCleanupFailure2 : break ;
                case ITestClassConstructionFinished testClassConstructionFinished2 : break ;
                case ITestClassConstructionStarting testClassConstructionStarting2 : break ;
                case ITestClassDisposeFinished testClassDisposeFinished2 : break ;
                case ITestClassDisposeStarting testClassDisposeStarting2 : break ;
                case ITestClassFinished testClassFinished2 : break ;
                case ITestClassStarting testClassStarting2 : break ;
                case ITestCleanupFailure testCleanupFailure2 : break ;
                case ITestCollectionCleanupFailure testCollectionCleanupFailure2 : break ;
                case ITestCollectionFinished testCollectionFinished2 : break ;
                case ITestCollectionStarting testCollectionStarting2 : break ;
                case ITestFailed testFailed2 : break ;
                case ITestFinished testFinished2 : break ;
                case ITestMethodCleanupFailure testMethodCleanupFailure2 : break ;
                case ITestMethodFinished testMethodFinished2 : break ;
                case ITestMethodStarting testMethodStarting2 : break ;
                case ITestOutput testOutput2 : break ;
                case ITestPassed testPassed2 : break ;
                case ITestSkipped testSkipped2 : break ;
                case ITestStarting testStarting2 : break ;
                case ITestAssemblyDiscoveryFinished testAssemblyDiscoveryFinished1 : break ;
                case ITestAssemblyDiscoveryStarting testAssemblyDiscoveryStarting1 : break ;
                case ITestAssemblyExecutionFinished testAssemblyExecutionFinished1 : break ;
                case ITestAssemblyExecutionStarting testAssemblyExecutionStarting1 : break ;
                case ITestExecutionSummary testExecutionSummary1 : break ;
                case TestResultMessage testResultMessage : break ;
                case Xunit.TestResultMessage testResultMessage1 : break ;
                case IFinishedMessage finishedMessage : break ;
                case ITestResultMessage testResultMessage2 : break ;
                case TestMessage testMessage : break ;
                case Xunit.TestMessage testMessage1 : break ;
                case ITestMessage testMessage2 : break ;
                case TestCaseMessage testCaseMessage : break ;
                case Xunit.TestCaseMessage testCaseMessage1 : break ;
                case ITestCaseMessage testCaseMessage2 : break ;
                case TestMethodMessage testMethodMessage : break ;
                case Xunit.TestMethodMessage testMethodMessage1 : break ;
                case ITestMethodMessage testMethodMessage2 : break ;
                case TestClassMessage testClassMessage : break ;
                case Xunit.TestClassMessage testClassMessage1 : break ;
                case ITestClassMessage testClassMessage2 : break ;
                case TestCollectionMessage testCollectionMessage : break ;
                case Xunit.TestCollectionMessage testCollectionMessage1 : break ;
                case ITestCollectionMessage testCollectionMessage2 : break ;
                case TestAssemblyMessage testAssemblyMessage : break ;
                case Xunit.TestAssemblyMessage testAssemblyMessage1 : break ;
                case IExecutionMessage executionMessage : break ;
                case ITestAssemblyMessage testAssemblyMessage2 : break ;
                default : throw new ArgumentOutOfRangeException ( nameof ( message ) ) ;
            }
            return true ;
        }
        #endregion
        #region Implementation of IMessageSink
        public bool OnMessage ( IMessageSinkMessage message ) { return false ; }
        #endregion
    }
}