using System ;
using System.Linq ;
using System.Text.Json ;
using System.Windows ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using NLog.Targets ;
using static KayMcCormick.Dev.Logging.AppLoggingConfigHelper ;
using static NLog.LogManager ;

namespace ProjInterface
{
    public sealed partial class ProjInterfaceApp : BaseApp, IResourceResolver
    {
        private new static readonly Logger Logger = GetCurrentClassLogger ( ) ;

        public ProjInterfaceApp ( ) : this ( null ) { }

        public ProjInterfaceApp (
            ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
        ) : base (
                  applicationInstance
                , disableLogging
                , disableRuntimeConfiguration
                , disableServiceHost
                , new IModule[] { new ProjInterfaceModule ( ) , new AnalysisControlsModule ( ) }
                , ( ) => PopulateJsonConverters ( disableLogging )
                 )

        {
            //PresentationTraceSources.Refresh();
            //PopulateJsonConverters ( disableLogging ) ;
        }

        private static void PopulateJsonConverters ( bool disableLogging )
        {
            if ( ! disableLogging )
            {
                foreach ( var myJsonLayout in Configuration.AllTargets
                                             .OfType < TargetWithLayout > ( )
                                             .Select ( t => t.Layout )
                                             .OfType < MyJsonLayout > ( ) )
                {
                    var options = new JsonSerializerOptions ( ) ;
                    foreach ( var optionsConverter in myJsonLayout.Options.Converters )
                    {
                        options.Converters.Add ( optionsConverter ) ;
                    }

                    JsonConverters.AddJsonConverters ( options ) ;
                    myJsonLayout.Options = options ;
                }
            }
            else
            {
                var options = new JsonSerializerOptions ( ) ;
                JsonConverters.AddJsonConverters ( options ) ;
            }
        }

        public override Guid ApplicationGuid { get ; } =
            new Guid ( "9919c0fb-916c-4804-81de-f272a1b585f7" ) ;

        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
            Logger.Trace ( "{methodName}" , nameof ( OnStartup ) ) ;

            var lifetimeScope = Scope ;
            if ( ! lifetimeScope.IsRegistered < Window1 > ( ) )
            {
                ShowErrorDialog (
                                 ProjInterface.Properties.Resources
                                              .ProjInterfaceApp_OnStartup_Application_Error
                               , "Error in compile-time configuration. Please contact your local administrator."
                                ) ;
                Current.Shutdown ( 255 ) ;
            }

            var mainWindow = lifetimeScope.Resolve < Window1 > ( ) ;
            mainWindow.Show ( ) ;
        }

        private void ShowErrorDialog ( string applicationError , string messageText )
        {
            MessageBox.Show (
                             messageText
                           , applicationError
                           , MessageBoxButton.OK
                           , MessageBoxImage.Error
                            ) ;
        }

        #region Implementation of IResourceResolver
        public object ResolveResource ( object resourceKey ) { return TryFindResource(resourceKey) ; }
        #endregion
    }

    internal static class CustomAppEntry
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [ STAThreadAttribute ( ) ]
        public static void Main ( )
        {
            EnsureLoggingConfiguredAsync ( Console.WriteLine ) ;

            using ( MappedDiagnosticsLogicalContext.SetScoped ( "Test" , "CustomAppEntry" ) )
            {
                AppDomain.CurrentDomain.ProcessExit += ( sender , args ) => GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;
                var app = new ProjInterfaceApp ( ) ;
                app.Run ( ) ;
            }
        }
    }
}