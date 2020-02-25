using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Fact]
        public void Test1 ( )
        {
            var modulesList = KayMcCormick.Dev.Container.ContainerAdjunct.GetModulesList ( ) ;
            _helper.WriteLine(string.Join ( ", ", modulesList ));
        }

        [ Fact ]

        public void Test2 ( )
        {
            Win32RegistryConfiguration x = new Win32RegistryConfiguration();
            x.LoadConfiguration();
        }
    }
}
