using System ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Threading ;
using Autofac ;
using JetBrains.Annotations ;
using ProjInterface ;
using Xunit ;

namespace ProjTests
{
    [ UsedImplicitly ]
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
        public async Task InitializeAsync ( ) {  }

        public void Action ( )
        {
            _projInterfaceApp = new ProjInterfaceApp();
            _projInterfaceApp.InitializeComponent();
            _projInterfaceApp.TestMode = true;
        }

        public async Task DisposeAsync ( ) { }
        #endregion
    }
}