using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KayMcCormick.Dev ;

namespace TestApp
{
    internal class TestAppApp : IDisposable
    {
        private readonly ApplicationInstance _applicationInstance ;

        public TestAppApp ( ApplicationInstanceConfiguration config )
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
