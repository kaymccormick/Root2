using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization ;
using System.ServiceModel ;
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
        private readonly AppInfoService _service ;
        private readonly Uri _baseAddresses ;

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
    [ UsedImplicitly ]
    public sealed class ApplicationInstance : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ILifetimeScope lifetimeScope ;
        private readonly List <IModule> _modules = new List < IModule > ();
        private IContainer _container ;
        private ApplicationInstanceHost _host ;

        /// <summary>
        /// 
        /// </summary>
        public Guid InstanceRunGuid { get ; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// 
        /// </summary>
        public ApplicationInstance ( )
        {
            InstanceRunGuid = Guid.NewGuid ( ) ;
            GlobalDiagnosticsContext.Set ( "RunId" , InstanceRunGuid ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void Initialize ( ) { _container = BuildContainer ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appModule"></param>
        // ReSharper disable once UnusedMember.Global
        public void AddModule ( IModule appModule ) => _modules.Add ( appModule ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ UsedImplicitly ]
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

        // ReSharper disable once UnusedMember.Local
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
                                                                                                      _
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
                                                                                                      _
                                                                                                      :
                                                                                                      break ;
                                                                                                  case
                                                                                                      DecoratorService
                                                                                                      _
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
        [ UsedImplicitly ]
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
    [ UsedImplicitly ]
    public class ApplicationInstanceException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationInstanceException ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ApplicationInstanceException ( string message ) : base ( message )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApplicationInstanceException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
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
