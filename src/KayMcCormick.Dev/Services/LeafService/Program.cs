using System ;
using System.IO ;
using Autofac ;
using Common.Logging ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using NLog ;
using Topshelf ;
using Topshelf.Autofac ;
using Topshelf.Common.Logging ;
using Topshelf.HostConfigurators ;
using Topshelf.ServiceConfigurators ;
using LogLevel = NLog.LogLevel ;
using LogManager = Common.Logging.LogManager ;

namespace LeafService
{
    internal class Program : IDisposable
    {
        private const    string              serviceName = "LeafService" ;
        private const    string              displayName = "LeafService Service" ;
        private const    string              description = "A .NET Windows Service." ;
        private readonly ApplicationInstance _appInst ;

        public Program ( )
        {
            _appInst = new ApplicationInstance (
                                                new ApplicationInstanceConfiguration (
                                                                                      Console
                                                                                         .Error
                                                                                         .WriteLine
                                                                                     )
                                               ) ;
        }

        private ILogger Logger { get { return _appInst.Logger ; } }

        #region IDisposable
        public void Dispose ( ) { _appInst.Dispose ( ) ; }
        #endregion

        private static void Main ( )
        {
            // This will ensure that future calls to Directory.GetCurrentDirectory()
            // returns the actual executable directory and not something like C:\Windows\System32 
            Directory.SetCurrentDirectory ( AppDomain.CurrentDomain.BaseDirectory ) ;

            // Specify the base name, display name and description for the service, as it is registered in the services control manager.
            // This information is visible through the Windows Service Monitor
            using ( var p = new Program ( ) )
            {
                p.Run ( ) ;
                p.Logger.Debug ( "Returned from HostFactory.run, preparing to exit." ) ;
            }
        }

        private void Run ( )
        {
            var logger = LogManager.GetLogger < Program > ( ) ;
            AppDomain.CurrentDomain.FirstChanceException += ( sender , args )
                => Utils.HandleInnerExceptions (
                                                args.Exception
                                              , LogLevel.Info
                                              , _appInst.Logger
                                               ) ;
            HostFactory.Run ( Configure ) ;
            _appInst.Logger.Debug ( "Returned from HostFactory.run" ) ;
        }

        private void Configure ( HostConfigurator configurator )
        {
            _appInst.Logger.Debug ( "In configurator lambda" ) ;
            _appInst.AddModule ( new LeafServiceModule ( ) ) ;
            /* Initialize builds the container */
            _appInst.Initialize ( ) ;
            configurator.OnException ( HandleException ) ;
            //configurator.UseNLog ( ) ;
            configurator.UseCommonLogging ( ) ;
            configurator.UseAutofacContainer ( _appInst.GetLifetimeScope ( ) ) ;
            configurator.Service < LeafService1 > ( ConfigureService ) ;
            configurator.RunAsLocalSystem ( ) ;
            //x.RunAs("username", "password"); // predefined user
            //x.RunAsPrompt(); // when service is installed, the installer will prompt for a username and password
            //x.RunAsNetworkService(); // runs as the NETWORK_SERVICE built-in account
            //x.RunAsLocalSystem(); // run as the local system account
            //x.RunAsLocalService(); // run as the local service account

            //=> Service Instalation - These configuration options are used during the service instalation

            //x.StartAutomatically(); // Start the service automatically
            //x.StartAutomaticallyDelayed(); // Automatic (Delayed) -- only available on .NET 4.0 or later
            //x.StartManually(); // Start the service manually
            //x.Disabled(); // install the service as disabled

            //=> Service Configuration

            //x.EnablePauseAndContinue(); // Specifies that the service supports pause and continue.
            //x.EnableShutdown(); //Specifies that the service supports the shutdown service command.

            //=> Service Dependencies
            //=> Service dependencies can be specified such that the service does not start until the dependent services are started.

            //x.DependsOn("SomeOtherService");
            //x.DependsOnMsmq(); // Microsoft Message Queueing
            //x.DependsOnMsSql(); // Microsoft SQL Server
            //x.DependsOnEventLog(); // Windows Event Log
            //x.DependsOnIis(); // Internet Information Server

            configurator.SetDescription ( description ) ;
            configurator.SetDisplayName ( displayName ) ;
            configurator.SetServiceName ( serviceName ) ;
        }

        private void HandleException ( Exception obj )
        {
            Logger.Fatal (
                          obj
                        , "Fatal exception starting/configuring TopShelf: {ex}"
                        , obj.ToString ( )
                         ) ;
        }

        private void ConfigureService ( ServiceConfigurator < LeafService1 > sc )
        {
            _appInst.Logger.Debug ( "In service lambda" ) ;
            sc.ConstructUsingAutofacContainer ( ) ;
            sc.WhenStarted ( ( s , hostControl ) => s.Start ( hostControl ) ) ;
            sc.WhenStopped ( ( s , hostControl ) => s.Stop ( hostControl ) ) ;

            // optional pause/continue methods if used
            sc.WhenPaused ( ( s ,    hostControl ) => s.Pause ( hostControl ) ) ;
            sc.WhenContinued ( ( s , hostControl ) => s.Continue ( hostControl ) ) ;

            // optional, when shutdown is supported
            sc.WhenShutdown ( ( s , hostControl ) => s.Shutdown ( hostControl ) ) ;
        }
    }

    internal class LeafServiceModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterType < LeafService1 > ( ).AsSelf ( ) ;
            builder.Register (
                              ( context , parameters )
                                  => LogManager.GetLogger ( typeof ( LeafService1 ) )
                             )
                   .As < ILog > ( ) ;
        }
        #endregion
    }
}