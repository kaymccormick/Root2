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

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationInstanceHost : IDisposable
    {
        private          ServiceHost    _host ;
        private readonly AppInfoService _service ;
        private readonly Uri            _baseAddresses ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public ApplicationInstanceHost ( [ NotNull ] IContainer container )
        {
            if ( container == null )
            {
                throw new ArgumentNullException ( nameof ( container ) ) ;
            }

            _service       = new AppInfoService(DateTime.Now, container.ResolveOptional<IObjectIdProvider>()) ;
            _baseAddresses = new Uri("http://localhost:8736/ProjInterface/App") ;
        }

        /// <summary>
        /// 
        /// </summary>
        public void HostOpen ( )
        {
            _host = new ServiceHost ( _service , _baseAddresses ) ;
            _host.Open ( ) ;
        }

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        public void Dispose ( )
        {
            var disposable = _host as IDisposable ;
            disposable?.Dispose();
        }
        #endregion
    }
}