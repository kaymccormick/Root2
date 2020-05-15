using System;
using System.ComponentModel;
using AnalysisControls;
using Autofac;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Lib.Wpf;

namespace TestApp
{
    internal sealed class TestAppApp : IDisposable
    {
        private readonly ApplicationInstance _applicationInstance ;

        public TestAppApp ( [ NotNull ] ApplicationInstance.ApplicationInstanceConfiguration config )
        {
            _applicationInstance = new ApplicationInstance( config ) ;
            _applicationInstance.AddModule(new AnalysisControlsModule());
            _applicationInstance.Initialize();
            var Scope = _applicationInstance.GetLifetimeScope();
            var provider = Scope.Resolve<IControlsProvider>();
            foreach (var providerType in provider.Types)
            {
            //    DebugUtils.WriteLine(providerType.FullName);
                TypeDescriptor.AddProvider(provider.Provider, providerType);
            }
        }

        #region IDisposable
        public void Dispose ( )
        {
            _applicationInstance?.Dispose ( ) ;
        }
        #endregion
    }
}
