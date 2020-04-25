using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IdentityModel ;
using System.Linq ;
using System.Reflection ;
using System.Text.Json ;
using System.Windows ;
using AnalysisAppLib ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.Metadata ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis.Host ;
using NLog ;
using NLog.Targets ;
using ProjInterface ;
using static KayMcCormick.Dev.Logging.AppLoggingConfigHelper ;
using static NLog.LogManager ;

namespace ProjInterface
{
    internal sealed partial class ProjInterfaceApp : BaseApp , IResourceResolver
    {
        private new static readonly Logger Logger = GetCurrentClassLogger ( ) ;

        public ProjInterfaceApp ( ) : this ( null ) { }

        [ UsedImplicitly ]
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
            //ExploreAssemblies ( ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private void ExploreAssemblies ( )
        {
            var resourceLocator = new Uri (
                                           "/ProjInterface;component/AppResources.xaml"
                                         , UriKind.Relative
                                          ) ;
            GetResourceStream ( resourceLocator ) ;
            foreach ( var referencedAssembly in Assembly
                                               .GetExecutingAssembly ( )
                                               .GetReferencedAssemblies ( )
                                               .Where (
                                                       referencedAssembly
                                                           => referencedAssembly.Name
                                                              == "WindowsBase"
                                                      ) )
            {
                DebugUtils.WriteLine ( referencedAssembly.ToString ( ) ) ;
                var assembly = AppDomain.CurrentDomain.GetAssemblies ( ) ;
                foreach ( var assembly1 in assembly.Where (
                                                           assembly1 => assembly1.GetName ( ).Name
                                                                        == "WindowsBase"
                                                          ) )
                {
                    DebugUtils.WriteLine ( assembly1.FullName ) ;
                    foreach ( var type in assembly1.GetTypes ( ) )
                    {
                        DebugUtils.WriteLine (
                                              $"Assembly type {type.FullName}: public={type.IsPublic}"
                                             ) ;
                        var staticFields =
                            type.GetFields ( BindingFlags.NonPublic | BindingFlags.Static ) ;
                        foreach ( var staticField in staticFields )
                        {
                            DebugUtils.WriteLine (
                                                  $"{type.FullName}.{staticField.Name}: t[{staticField.FieldType.FullName}]"
                                                 ) ;
                            try
                            {
                                var val = staticField.GetValue ( null ) ;
                                if ( val == null )
                                {
                                    DebugUtils.WriteLine ( "val is null" ) ;
                                }
                                else
                                {
                                    DebugUtils.WriteLine ( $"val is {val}" ) ;
                                    DumpTypeInstanceInfo ( val ) ;
                                }
                            }
                            catch ( Exception )
                            {
                                // ignored
                            }
                        }
                    }
                }
            }
        }

        private void DumpTypeInstanceInfo ( [ NotNull ] object val )
        {
            if ( val.GetType ( ).IsPrimitive )
            {
                return ;
            }

            var nonPublicFields = val.GetType ( )
                                     .GetFields ( BindingFlags.Instance | BindingFlags.NonPublic ) ;
            // ReSharper disable once UnusedVariable
            var publicProperties =
                val.GetType ( ).GetFields ( BindingFlags.Instance | BindingFlags.Public ) ;
            // ReSharper disable once UnusedVariable
            var nonPublicProperties =
                val.GetType ( ).GetFields ( BindingFlags.Instance | BindingFlags.NonPublic ) ;
            foreach ( var nonPublicField in nonPublicFields )
            {
                object v = null ;
                try
                {
                    v = nonPublicField.GetValue ( val ) ;
                }
                catch ( Exception )
                {
                    DebugUtils.WriteLine ( "Unable to get field value" ) ;
                }

                if ( v == null )
                {
                    continue ;
                }

                var t = v.GetType ( ) ;
                DebugUtils.WriteLine (
                                      $"nonpublic field {nonPublicField.Name} is of type {t.FullName} and value {v}"
                                     ) ;
            }
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

        protected override Guid ApplicationGuid { get ; } =
            new Guid ( "9919c0fb-916c-4804-81de-f272a1b585f7" ) ;

        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
            Logger.Trace ( "{methodName}" , nameof ( OnStartup ) ) ;
            

            {
                ShowErrorDialog (
                                 ProjInterface.Properties.Resources
                                              .ProjInterfaceApp_OnStartup_Application_Error
                               , ProjInterface
                                .Properties.Resources
                                .ProjInterfaceApp_OnStartup_Compile_time_configuration_error
                                ) ;
                Current.Shutdown ( 255 ) ;
            }
            try
            {
                mainWindow.Show ( ) ;
            }
            catch ( Exception ex )
            {
                MessageBox.Show ( ex.ToString ( ) , "error" ) ;
            }
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
        public object ResolveResource ( [ NotNull ] object resourceKey )
        {
            return TryFindResource ( resourceKey ) ;
        }
        #endregion
    }

    internal static class CustomAppEntry
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [ STAThreadAttribute ]
        public static void Main ( )
        {
            var loggingConfiguration = AppLoggingConfiguration.Default ;
            loggingConfiguration.IsEnabledCacheTarget = true ;
            loggingConfiguration.MinLogLevel          = LogLevel.Trace ;


            AppDomain.CurrentDomain.ProcessExit += ( sender , args )
                => GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;
            try
            {
                var app = new ProjInterfaceApp ( ) ;
                app.Run ( ) ;
            }
            catch ( Exception ex )
            {
                
            }
        }
    }
}