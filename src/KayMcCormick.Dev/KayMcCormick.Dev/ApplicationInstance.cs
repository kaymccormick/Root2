using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization ;
using System.ServiceModel ;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core ;
using Autofac.Features.Decorators ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationInstanceHost : IDisposable
    {
        private  ServiceHost _host ;
        private AppInfoService _service ;
        private Uri _baseAddresses ;

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

            _service = new AppInfoService(DateTime.Now, container.ResolveOptional<IObjectIdProvider>()) ;
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
    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationInstance : IDisposable
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ILifetimeScope lifetimeScope ;
        private List <IModule> _modules = new List < IModule > ();
        private IContainer _container ;
        private ApplicationInstanceHost _host ;
        private Guid _instanceRunGuid ;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// 
        /// </summary>
        public ApplicationInstance ( )
        {
            _instanceRunGuid = Guid.NewGuid ( ) ;
            GlobalDiagnosticsContext.Set ( "RunId" , _instanceRunGuid ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize ( ) { _container = BuildContainer ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appModule"></param>
        public void AddModule ( IModule appModule ) { _modules.Add ( appModule ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILifetimeScope GetLifetimeScope ( )
        {
            if(lifetimeScope != null)
            {
                return lifetimeScope ;
            }

            if ( _container == null )
            {
                _container = BuildContainer ( ) ;
            }

            lifetimeScope = _container.BeginLifetimeScope ( ) ;
            return _container.BeginLifetimeScope ( ) ;
        }

        private IContainer BuildContainer ( )
        {
            var builder1 = new ContainerBuilder ( ) ;
            foreach ( var module in _modules )
            {
                Logger.Debug ( "Registering module {module}" , module ) ;
                builder1.RegisterModule ( module ) ;
            }

            var c
                = builder1.Build ( ) ;
//            DebugServices ( c ) ;
            return c ;
        }

        private static void DebugServices ( IContainer c )
        {
            foreach ( var componentRegistryRegistration in c.ComponentRegistry.Registrations )
            {
                Logger.Debug (
                              "services: {services}"
                            , string.Join (
                                           ", "
                                         , componentRegistryRegistration.Services.Select (
                                                                                          service => {
                                                                                              switch (
                                                                                                  service )
                                                                                              {
                                                                                                  case
                                                                                                      KeyedService
                                                                                                      keyedService
                                                                                                      :
                                                                                                      break ;
                                                                                                  case
                                                                                                      TypedService
                                                                                                      typedService
                                                                                                      :
                                                                                                      return
                                                                                                          typedService
                                                                                                             .ServiceType
                                                                                                             .FullName ;
                                                                                                  case
                                                                                                      UniqueService
                                                                                                      uniqueService
                                                                                                      :
                                                                                                      break ;
                                                                                                  case
                                                                                                      DecoratorService
                                                                                                      decoratorService
                                                                                                      :
                                                                                                      break ;
                                                                                                  default :
                                                                                                      throw
                                                                                                          new
                                                                                                              ArgumentOutOfRangeException (
                                                                                                                                           nameof
                                                                                                                                           ( service
                                                                                                                                           )
                                                                                                                                          ) ;
                                                                                              }

                                                                                              return service
                                                                                                 .Description ;
                                                                                          }
                                                                                         )
                                          )
                             ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Startup ( )
        {
            // if ( lifetimeScope == null )
            // {
            //     throw new ApplicationInstanceException ( "lifetime scope not initialized" ) ;
            // }
            _host = new ApplicationInstanceHost(_container) ;
            _host.HostOpen();
            OnAppStartup(new AppStartupEventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnAppStartup ( AppStartupEventArgs e )
        {
            AppStartup?.Invoke ( this , e ) ;
        }

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        public void Dispose ( )
        {
            _host?.Dispose();
            lifetimeScope?.Dispose ( ) ;
            _container?.Dispose ( ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationInstanceException : Exception
    {
        public ApplicationInstanceException ( ) {
        }

        public ApplicationInstanceException ( string message ) : base ( message )
        {
        }

        public ApplicationInstanceException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected ApplicationInstanceException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppStartupEventArgs : EventArgs
    {

    }
}
