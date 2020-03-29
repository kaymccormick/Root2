using KayMcCormick.Dev ;

using KayMcCormick.Dev.TestLib.Fixtures ;
using Xunit ;
using Xunit.Abstractions ;

namespace ClassLibTests
{
    
    public class Class1 : IClassFixture <LoggingFixture>
    {
        private readonly ITestOutputHelper _helper ;

        public Class1 (ITestOutputHelper helper ) { _helper = helper ; }

        #if USEREGISTRY
        [ Fact ]

        public void Test2 ( )
        {
            Win32RegistryConfiguration x = new Win32RegistryConfiguration();
            x.LoadConfiguration();
        }
#endif
        [Fact]
        public void Test3 ( )
        {
            RegisterApplicationInstanceRequest x = new RegisterApplicationInstanceRequest();

        }
    }
}
