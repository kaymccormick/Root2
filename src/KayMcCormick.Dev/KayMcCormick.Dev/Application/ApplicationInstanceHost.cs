#region header
// Kay McCormick (mccor)
// 
// ConfigTest
// KayMcCormick.Dev
// ApplicationInstanceHost.cs
// 
// 2020-03-09-8:43 PM
// 
// ---
#endregion
using System ;
using System.ServiceModel ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Service ;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// </summary>
    public sealed class ApplicationInstanceHost : IDisposable
    {
        private readonly Uri            _baseAddresses ;
        private readonly AppInfoService _service ;
#if NETFRAMEWORK
        private ServiceHost _host ;
#endif

        /// <summary>
        /// </summary>
        /// <param name="container"></param>
        public ApplicationInstanceHost ( [ NotNull ] IContainer container )
        {
            if ( container == null )
            {
                throw new ArgumentNullException ( nameof ( container ) ) ;
            }

            _service = new AppInfoService (
                                           DateTime.Now
                                         , container.ResolveOptional < IObjectIdProvider > ( )
                                          ) ;
            _baseAddresses = new Uri ( "http://localhost:8736/ProjInterface/App" ) ;
        }

        #region IDisposable
        /// <summary>
        /// </summary>
        public void Dispose ( )
        {
#if NETFRAMEWORK
            var disposable = _host as IDisposable ;
            disposable?.Dispose ( ) ;
#endif
        }
        #endregion

        /// <summary>
        /// </summary>
        public void HostOpen ( )
        {
#if NETFRAMEWORK
            try
            {
                _host = new ServiceHost ( _service , _baseAddresses ) ;
                _host.Open ( ) ;
            }
            catch ( Exception )

            {
                // ignored
            }
#endif
        }
    }
}