using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text.Json ;
using System.Windows ;
using System.Windows.Threading ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using NLog.Targets ;
using Application = KayMcCormick.Dev.Application ;

namespace ProjInterface
{
    public partial class ProjInterfaceApp : BaseApp
    {
        private readonly bool _disableLogging ;

        private readonly List < IModule > appModules = new List < IModule > ( ) ;

        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private JsonSerializerOptions _appJsonSerializerOptions ;

        private bool _testMode ;

        private Func < ProjInterfaceApp , ILifetimeScope , bool > _testCallback ;

        public ProjInterfaceApp ( ) : this ( null , false , false , false ) { }

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
                 )

        {
            _disableLogging = disableLogging ;
            //PresentationTraceSources.Refresh();
            if ( ! disableLogging )
            {
                foreach ( var myJsonLayout in LogManager
                                             .Configuration.AllTargets
                                             .OfType < TargetWithLayout > ( )
                                             .Select ( t => t.Layout )
                                             .OfType < MyJsonLayout > ( ) )
                {
                    var jsonSerializerOptions = myJsonLayout.Options ;
                    AppJsonSerializerOptions = jsonSerializerOptions ;
                    JsonConverters.AddJsonConverters ( jsonSerializerOptions ) ;
                }
            }
            else
            {
                var options = new JsonSerializerOptions ( ) ;
                JsonConverters.AddJsonConverters ( options ) ;
                AppJsonSerializerOptions = options ;
            }
        }

        public JsonSerializerOptions AppJsonSerializerOptions
        {
            get { return _appJsonSerializerOptions ; }
            set { _appJsonSerializerOptions = value ; }
        }

        public bool TestMode { get { return _testMode ; } set { _testMode = value ; } }

        public Func < ProjInterfaceApp , ILifetimeScope , bool > TestCallback
        {
            get { return _testCallback ; }
            set { _testCallback = value ; }
        }


        protected override IEnumerable < IModule > GetModules ( ) { return appModules ; }


        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
            Logger.Trace ( "{methodName}" , nameof ( OnStartup ) ) ;

            var lifetimeScope = Scope ;
#if false
            foreach ( var view1 in lifetimeScope.Resolve < IEnumerable < IView1 > > ( ) )
            {
                if ( view1 is Window vW )
                {
                    vW.Show ( ) ;
                }
                else
                {
                    Window w = new Window ( ) ;
                    w.Content = view1 ;
                    w.Show ( ) ;
                }
            }
#endif

            if ( ! lifetimeScope.IsRegistered <Uri > ( ) )
            {
                ShowErrorDialog (
                                 ProjInterface
                                    .Properties.Resources
                                    .ProjInterfaceApp_OnStartup_Application_Error
                               , "Error in compile-time configuration. Please contact your local administrator."
                                ) ;
                System.Windows.Application.Current.Shutdown(255);
            }

            var mainWindow = lifetimeScope.Resolve <Window1>(  ) ;
            mainWindow.Show ( ) ;
        }

        private void ShowErrorDialog ( string applicationError , string messageText )
        {
            MessageBox.Show ( messageText , applicationError, MessageBoxButton.OK , MessageBoxImage.Error ) ;
        }

        private void ProjInterfaceApp_OnDispatcherUnhandledException (
            object                                sender
          , DispatcherUnhandledExceptionEventArgs e
        )
        {
            if ( e.Exception is InvalidCastException )
            {
                Logger.Fatal ( "First chance exception: " + e.Exception.ToString ( ) ) ;
                // e.Handled = true ;
                return ;
            }

            Debug.WriteLine ( e.ToString ( ) ) ;
            if ( ! TestMode )
            {
                MessageBox.Show ( e.Exception.Message , "Error" ) ;
            }

            Current.Shutdown ( ) ;
        }
    }

}
