using System ;
using System.Reactive.Subjects ;
using System.Threading.Tasks ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;

namespace ClassLibTests
{
    public class Class1
    {
        private readonly ITestOutputHelper _helper ;

        public Class1 ( ITestOutputHelper helper ) { _helper = helper ; }

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
            var observable = new Subject < ILogger > ( ) ;
            var task = Task.Run (
                      ( ) => AppLoggingConfigHelper.EnsureLoggingConfiguredAsync (
                                                                                  LogMethod
                                                                                , new
                                                                                      AppLoggingConfiguration ( )
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
                inst.Subject1.Subscribe ( x => _helper.WriteLine ( x.Message ) ) ;
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
            }

            task.Wait ( ) ;
            AppLoggingConfigHelper.Shutdown();
        }

        private void LogMethod ( string message )
        {
            _helper.WriteLine(message);
        }
    }
}