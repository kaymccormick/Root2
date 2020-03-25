using System ;
using System.Diagnostics ;
using System.IO ;
using AnalysisAppLib ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Dev.Tracing ;
using Xunit ;
using Xunit.Abstractions ;

namespace ModelTests
{

    [BeforeAfterLogger]
    [LogTestMethod]
    public sealed class Class1 : IDisposable, IClassFixture <LoggingFixture>
    {
        private readonly ITestOutputHelper _outputHelper ;
        private readonly LoggingFixture _loggingFixture ;
        private readonly ApplicationInstance _app ;
        private readonly bool _disableLogging ;

        public Class1 ( ITestOutputHelper outputHelper , [ NotNull ] LoggingFixture loggingFixture)
        {
            _outputHelper = outputHelper ;
            loggingFixture.SetOutputHelper ( _outputHelper ) ;
            _loggingFixture = loggingFixture ;
            _disableLogging = false;
            _app = SetupApplicationInstance ( ) ;
            
        }

        public bool DisableLogging { get { return _disableLogging ; } }


        [ NotNull ]
        private ApplicationInstance SetupApplicationInstance ( )
        {
            var applicationInstance = new ApplicationInstance (
                                                               new ApplicationInstanceConfiguration ( LogMethod , null , DisableLogging , true
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
                                         new ApplicationInstanceConfiguration ( LogMethod , null , DisableLogging , true
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

        [Fact]
        public void Test1 ( )
        {
            using (var ls = _app.GetLifetimeScope ( ).BeginLifetimeScope ( ) )
            {
                var model = ls.Resolve < DockWindowViewModel > ( ) ;
                Assert.NotNull(model);
            }
        }
        [Fact]
        public void Test2()
        {
            using (var ls = _app.GetLifetimeScope().BeginLifetimeScope())
            {
                var model = ls.Resolve<DockWindowViewModel>();
                Assert.NotNull(model);
                Assert.NotNull(model.DefaultInputPath);
                Assert.True ( Directory.Exists ( model.DefaultInputPath ) ) ;
                
            }
        }



        private void LogMethod ( string message )
        {
            //PROVIDER_GUID.EventWriteEVENT_TEST_OUTPUT ( message ) ;
            Debug.WriteLine ( message ) ;
            _outputHelper.WriteLine ( message ) ;
        }

        #region IDisposable
        public void Dispose ( )
        {
            _loggingFixture.SetOutputHelper ( null ) ;
            _app?.Dispose ( ) ;
        }
        #endregion
    }
}