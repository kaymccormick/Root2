using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit ;
using Xunit.Abstractions ;

namespace ClassLibTests
{
    
    public class Class1
    {
        private readonly ITestOutputHelper _helper ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Class1 (ITestOutputHelper helper ) { _helper = helper ; }

        [Fact]
        public void Test1 ( )
        {
            var modulesList = KayMcCormick.Dev.Container.ContainerAdjunct.GetModulesList ( ) ;
            _helper.WriteLine(string.Join ( ", ", modulesList ));
        }
    }
}
