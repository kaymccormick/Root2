using System ;
using System.Reactive.Subjects ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Media ;
using Autofac ;
using Autofac.Core.Lifetime ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;

namespace ClassLibTests
{
    public class Class1
    {
        private readonly ITestOutputHelper _helper ;
        private HelperWrapper _wrapper ;

        public Class1 ( ITestOutputHelper helper )
        {
            _wrapper = new HelperWrapper(helper) ;
            _helper = _wrapper ;
        }

#if USEREGISTRY
        [ Fact ]

        public void Test2 ( )
        {
            Win32RegistryConfiguration x = new Win32RegistryConfiguration();
            x.LoadConfiguration();
        }
#endif
        [ Fact ]
        public void Test3 ( )
        {
            var x = typeof ( TypeInfoConverter) ;
            using ( var observable = new Subject < ILogger > ( ) )
            {
                var task = Task.Run (
                                     ( ) => AppLoggingConfigHelper.EnsureLoggingConfiguredAsync (
                                                                                                 LogMethod
                                                                                               , new
                                                                                                 AppLoggingConfiguration
                                                                                                 {
                                                                                                     MinLogLevel
                                                                                                         = LogLevel
                                                                                                            .Trace
                                                                                                 }
                                                                                               , observable
                                                                                                )
                                    ) ;
                using ( var inst = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  LogMethod
                                                                                                , ApplicationInstanceIds
                                                                                                     .ClassLibTests
                                                                                                 )
                                                           ) )
                {
                    
                    //inst.AddModule(new LegacyAppBuildModule());
                    observable.Subscribe (
                                          logger => {
                                              _wrapper.Replay.Subscribe ( s => logger.Info ( s ) ) ;
                                              inst.Subject1.Subscribe (
                                                                       message
                                                                           => logger.Info (
                                                                                           message
                                                                                          )
                                                                      ) ;
                                          }
                                         ) ;
                    inst.Subject1.Subscribe ( x2 => _helper.WriteLine ( x2.Message ) ) ;
                    inst.Initialize ( ) ;
                    var c = inst.Container1 ;
                    _helper.WriteLine (
                                       $"HasLocalComponents: {c.ComponentRegistry.HasLocalComponents}"
                                      ) ;
                    _helper.WriteLine ( $"{string.Join ( "; " , c.ComponentRegistry.Sources )}" ) ;
                    foreach ( var cr in c.ComponentRegistry.Registrations )
                    {
                        var crMetadata = cr.Metadata ;
                        _helper.WriteLine ( cr.Id.ToString ( ) ) ;
                        if ( crMetadata != null )
                        {
                            foreach ( var keyValuePair in crMetadata )
                            {
                                _helper.WriteLine ( $"{keyValuePair.Key} = {keyValuePair.Value}" ) ;
                            }
                        }
                    }

                    _helper.WriteLine ( "-------------------" ) ;
                    var opts = c.Resolve < JsonSerializerOptions > ( ) ;
                    foreach ( var optsConverter in opts.Converters )
                    {
                        _helper.WriteLine ( $"\n\n{optsConverter.GetType ( ).FullName}" ) ;
                        _helper.WriteLine ( optsConverter.GetType ( ).BaseType.FullName ) ;
                        foreach ( var @interface in optsConverter.GetType ( ).GetInterfaces ( ) )
                        {
                            _helper.WriteLine ( $"  {@interface.FullName}" ) ;
                        }
                    }

                    //LogManager.GetCurrentClassLogger().Info ("scope: {scope}", inst.GetLifetimeScope()  );
                    
                    opts.WriteIndented = true ;
                    // var json = JsonSerializer.Serialize (
                                                         // inst.GetLifetimeScope ( )
                                                       // , typeof ( LifetimeScope )
                                                       // , opts
                                                        // ) ;
                    // _helper.WriteLine ( json ) ;

                    LineGeometry x1 = new LineGeometry(new Point(5, 5),new Point(50, 30) );
                    var json2 = JsonSerializer.Serialize ( x1 , typeof(Geometry), opts) ;
                    _helper.WriteLine ( json2 ) ;
                }

                task.Wait ( ) ;
                AppLoggingConfigHelper.Shutdown ( ) ;
            }
        }

        private void LogMethod ( string message ) { _helper.WriteLine ( message ) ; }
    }

    public class HelperWrapper : ITestOutputHelper
    {
        private ReplaySubject <string> _replay = new ReplaySubject < string > (100);
        private readonly ITestOutputHelper _helper ;
        public HelperWrapper ( ITestOutputHelper helper ) { _helper = helper ; }

        public ReplaySubject < string > Replay
        {
            get { return _replay ; }
            set { _replay = value ; }
        }

        #region Implementation of ITestOutputHelper
        public void WriteLine ( string message )
        {
            Replay.OnNext(new AppLogMessage(message).ToString());
            _helper.WriteLine ( message ) ;
        }

        public void WriteLine ( string format , params object[] args ) { _helper.WriteLine ( format , args ) ; }
        #endregion
    }
}