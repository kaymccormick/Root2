using System ;
using System.Diagnostics ;
using System.IO ;
using AnalysisAppLib ;
using Autofac ;
using KayMcCormick.Dev ;
using Xunit ;
using Xunit.Abstractions ;

namespace ModelTests
{
    public class Class1 : IDisposable
    {
        private readonly ITestOutputHelper _outputHelper ;
        private readonly ApplicationInstance _app ;
        private readonly bool _disableLogging ;

        public Class1 ( ITestOutputHelper outputHelper )
        {
            _outputHelper = outputHelper ;
            _disableLogging = false;
            _app = SetupApplicationInstance ( ) ;
            
        }

        public bool DisableLogging { get { return _disableLogging ; } }


        private ApplicationInstance SetupApplicationInstance ( )
        {
            var applicationInstance = new ApplicationInstance (
                                                               LogMethod
                                                             , null
                                                             , DisableLogging
                                                             , true
                                                             , true
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
                                         LogMethod
                                       , null
                                       , DisableLogging
                                       , true
                                       , true
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
            Debug.WriteLine ( message ) ;
            _outputHelper.WriteLine ( message ) ;
        }

        #region IDisposable
        public void Dispose ( )
        {
            _app?.Dispose ( ) ;
        }
        #endregion
    }
}