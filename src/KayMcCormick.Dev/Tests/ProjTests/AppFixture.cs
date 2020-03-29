using System ;
using System.Threading.Tasks ;
using Autofac ;
using JetBrains.Annotations ;
using ProjInterface ;
using Xunit ;

namespace ProjTests
{
    
    public sealed class AppFixture : IAsyncLifetime
    {
        private ProjInterfaceApp _projInterfaceApp ;

        public AppFixture ( )
        {
        }

        public ProjInterfaceApp InterfaceApp
        {
            get
            {
                if ( _projInterfaceApp == null )
                {
                    Action ( ) ;
                }

                return _projInterfaceApp ;
            }
        }

        public ILifetimeScope BeginLifetimeScope ( [ NotNull ] object tag )
        {
            if ( tag == null )
            {
                throw new ArgumentNullException ( nameof ( tag ) ) ;
            }

            return _projInterfaceApp.BeginLifetimeScope ( tag ) ;
        }

        public ILifetimeScope BeginLifetimeScope ( )
        {
            return _projInterfaceApp.BeginLifetimeScope ( ) ;
        }

        #region IDisposable
        public void Dispose ( ) { _projInterfaceApp.Dispose ( ) ; }
        #endregion
        #region Implementation of IAsyncLifetime
        public Task InitializeAsync ( )
        {
            return Task.CompletedTask ;
        }

        public void Action ( )
        {
            _projInterfaceApp = new ProjInterfaceApp();
            // _projInterfaceApp.InitializeComponent();
        }

        public Task DisposeAsync ( )
        {
            return Task.CompletedTask ;
        }
        #endregion
    }
}