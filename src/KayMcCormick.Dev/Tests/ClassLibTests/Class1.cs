using System ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Media ;
using Autofac ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;

namespace ClassLibTests
{
    public class Class1
    {
        private readonly ITestOutputHelper _helper ;
        
        public Class1 ( ITestOutputHelper helper )
        {
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
        }

        private void LogMethod ( string message ) { _helper.WriteLine ( message ) ; }
    }

}