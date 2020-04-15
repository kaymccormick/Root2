using System;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Application ;

namespace TestApp
{
    internal sealed class TestAppApp : IDisposable
    {
        private readonly ApplicationInstance _applicationInstance ;

        public TestAppApp ( [ NotNull ] ApplicationInstance.ApplicationInstanceConfiguration config )
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
