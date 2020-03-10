using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core ;
using Autofac.Features.Decorators ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;
using NLog ;

namespace KayMcCormick.Dev
{
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

        /// <summary>
        /// 
        /// </summary>
        ///  todo call from wpf
        // ReSharper disable once UnusedMember.Global
        public void Shutdown()
        {
            AppLoggingConfigHelper.ServiceTarget?.Dispose();
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
}
