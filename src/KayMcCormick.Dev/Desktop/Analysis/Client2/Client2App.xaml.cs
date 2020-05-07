using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using AnalysisControls;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using CommandLine;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Targets;
using ProjInterface;
using static NLog.LogManager ;
using Application = System.Windows.Application;

namespace Client2
{
    internal sealed partial class Client2App : BaseApp , IResourceResolver
    {
        private new static readonly Logger Logger = GetCurrentClassLogger ( ) ;

        public Client2App ( ) : this ( null ) { }

        [ UsedImplicitly ]
        public Client2App (
            ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
        ) : base (
                  applicationInstance
                , disableLogging
                , disableRuntimeConfiguration
                , disableServiceHost
                , new IModule[] { new Client2Module ( ) , new AnalysisControlsModule ( ) }
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


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Client2Window1 w = new Client2Window1(null, new ClientViewModel(), null);
            w.Show();
            return;
            
            var result = CommandLine.Parser.Default.ParseArguments<ProjInterfaceOptions>(e.Args)
                .WithNotParsed(errors => MessageBox.Show(String.Join("", errors), "error"));

            result.WithParsed(options =>
            {
                Options = options;
            });

            var t = typeof(WpfAppCommands);
            RoutedUICommand selCmd = null;
            foreach (var fieldInfo in t.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var cmd = (RoutedUICommand) fieldInfo.GetValue(null);
                if (Options.Command != null && Options.Command == cmd.Text)
                {
                    selCmd = cmd;
                }
            }

            string cmdStr = null;
            if (e.Args.Any())
            {
                cmdStr = e.Args.First();
            }
            Logger.Trace("{methodName}", nameof(OnStartup));
            var lifetimeScope = Scope;
            var wins = Scope.Resolve<IEnumerable<Meta<Lazy<Window>>>>();
            var winChose = wins.Where(z => z.Metadata.ContainsKey("ShortKey") && (string)z.Metadata["ShortKey"] == Options.window);
            if (!winChose.Any())
            {
                MessageBox.Show("Unknown window " + Options.window, "error");
                Current.Shutdown(1);
            }

            var win = winChose.First().Value.Value;


            if (selCmd != null)
            {
                DebugUtils.WriteLine(selCmd.ToString());
                win.Loaded += (sender, args) => selCmd.Execute(Options.Argument, win);
            }
            win.Show();

            // if (lifetimeScope?.IsRegistered<Window1>() == false)
            // {
                // ShowErrorDialog(
                                 // ProjInterface.Properties.Resources
                                              // .ProjInterfaceApp_OnStartup_Application_Error
                               // , ProjInterface
                                // .Properties.Resources
                                // .ProjInterfaceApp_OnStartup_Compile_time_configuration_error
                                // );
                // Current.Shutdown(255);
            // }

            // Window1 mainWindow = null;
            // try
            // {
                // if (lifetimeScope != null)
                // {
                    // var test1 = Scope.Resolve<TestModel>();

                   // mainWindow = lifetimeScope.Resolve<Window1>( ) ;
                   // mainWindow.CommandStr = cmdStr;
                // }
// }
            // catch (Exception ex )
            // {
                // DebugUtils.WriteLine(ex.ToString ( ) ) ;
                // MessageBox.Show(ex.ToString ( ) , "error" ) ;
            // }

            // if (mainWindow == null )
            // {
                // MessageBox.Show( "Unable to resolve Main window" , "error" ) ;
                // return ;
            // }

            // try
            // {
                // mainWindow.Show( ) ;
            // }
            // catch (Exception ex )
            // {
                // MessageBox.Show(ex.ToString ( ) , "error" ) ;
            // }
        }

        public ProjInterfaceOptions Options { get; set; }
        public static string DefaultWindow = "window2";


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

    internal class ProjInterfaceOptions
    {
        [Option('w', "window", Default = "Client2Window1")]
        public string window { get; set; }
        [Option('c', "command")]
        public string Command { get; set; }
        [Option('a', "arg")]
        public string Argument { get; set; }

    }

    internal class Test1
    {
    }

    internal static class CustomAppEntry
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [ STAThread ]
        public static void Main ( )
        {
            var loggingConfiguration = AppLoggingConfiguration.Default ;
            loggingConfiguration.IsEnabledCacheTarget = true ;
            loggingConfiguration.MinLogLevel          = LogLevel.Trace ;

            Main1Model.SelectVsInstance();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => DebugUtils.WriteLine(message),
                loggingConfiguration);

            AppDomain.CurrentDomain.ProcessExit += ( sender , args )
                => GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;
            try
            {
                var app = new Client2App2 ( ) ;
                app.Run ( ) ;
            }
            catch ( Exception ex )
            {
                
            }
        }

        private static void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            DebugUtils.WriteLine(args.LoadedAssembly.FullName);
        }
    }

    internal class Client2App2 : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Client2Window1 window1 = new Client2Window1(null, new ClientViewModel(), null);
            window1.Show();
        }
    }
}