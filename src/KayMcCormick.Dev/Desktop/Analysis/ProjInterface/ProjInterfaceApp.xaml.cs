﻿using System.Linq ;
using System.Text.Json ;
using System.Windows ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using NLog.Targets ;

namespace ProjInterface
{
    public sealed partial class ProjInterfaceApp : BaseApp
    {
        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

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
                  , () => PopulateJsonConverters(disableLogging)
                 )

        {
            //PresentationTraceSources.Refresh();
            //pulateJsonConverters ( disableLogging ) ;
        }

        private static void PopulateJsonConverters ( bool disableLogging )
        {
            if ( ! disableLogging )
            {
                foreach ( var myJsonLayout in LogManager
                                             .Configuration.AllTargets.OfType < TargetWithLayout > ( )
                                             .Select ( t => t.Layout )
                                             .OfType < MyJsonLayout > ( ) )
                {
                    var options = new JsonSerializerOptions();
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

        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
            Logger.Trace ( "{methodName}" , nameof ( OnStartup ) ) ;

            var lifetimeScope = Scope ;
            if ( ! lifetimeScope.IsRegistered <Window1 > ( ) )
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
    }

}
