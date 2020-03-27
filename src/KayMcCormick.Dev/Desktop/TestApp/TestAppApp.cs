using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;

namespace TestApp
{
    internal class TestAppApp : IDisposable
    {
        private readonly ApplicationInstance _applicationInstance ;

        public TestAppApp ( [ NotNull ] ApplicationInstanceConfiguration config )
        {
            _applicationInstance = new ApplicationInstance( config ) ;
        }

        #region IDisposable
        public void Dispose ( )
        {
            _applicationInstance?.Dispose ( ) ;
        }
        #endregion
    }
}
