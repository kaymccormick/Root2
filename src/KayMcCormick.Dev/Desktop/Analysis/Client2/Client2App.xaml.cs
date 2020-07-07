using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using AnalysisControls;
using AnalysisControlsCore;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
// using CommandLine;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Targets;
using static NLog.LogManager ;

namespace Client2
{
    internal sealed partial class Client2App : BaseApp
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
                , new IModule[] { new AnalysisControlsModule(),new Client2Module(),new Client2Module1 ( ) , new TypeDescriptorsModule(),  }
                , ( ) => PopulateJsonConverters ( disableLogging )
                 )

        {
            //ExploreAssemblies ( ) ;
        }

        // ReSharper disable once UnusedMember.Local

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

            //DebugUtils.DisplayCatgories = (DebugCategory) (0);
            // Client2Window1 w = new Client2Window1(null, new ClientModel(), null);
            // w.Show();
            // return;
            
            // var result = Parser.Default.ParseArguments<ProjInterfaceOptions>(e.Args)
                // .WithNotParsed(errors => MessageBox.Show(String.Join("", errors), "error"));

            // result.WithParsed(options =>
            // {
                // Options = options;
            // });

            // var t = typeof(WpfAppCommands);
            // RoutedUICommand selCmd = null;
            // foreach (var fieldInfo in t.GetFields(BindingFlags.Static | BindingFlags.Public))
            // {
                // var cmd = (RoutedUICommand) fieldInfo.GetValue(null);
                // if (Options.Command != null && Options.Command == cmd.Text)
                // {
                    // selCmd = cmd;
                // }
            // }

            if (e.Args.Any())
            {
            }
            Logger.Trace("{methodName}", nameof(OnStartup));
            var wins = Scope.Resolve<IEnumerable<Meta<Lazy<Window>>>>();

            Options = new ProjInterfaceOptions();
            var winChose = wins.Where(z => Options != null && (z.Metadata.ContainsKey("ShortKey") && (string)z.Metadata["ShortKey"] == Options.Window));
            var enumerable = winChose as Meta<Lazy<Window>>[] ?? winChose.ToArray();
            if (!enumerable.Any())
            {
                MessageBox.Show("Unknown window " + Options.Window, "error");
                Current.Shutdown(1);
            }
            // var win0 = new Main1Window();
            //win0.Content = new Main1UserControl();
            // win0.ShowDialog();
            // var wwww = new Window2();
            // wwww.Show();
            var win = enumerable.First().Value.Value;
            // var win = new RibbonWin1();

            // if (selCmd != null)
            // {
                // DebugUtils.WriteLine(selCmd.ToString());
                // win.Loaded += (sender, args) => selCmd.Execute(Options.Argument, win);
            // }
            win.Loaded += (sender, args) => SplashScreen?.Close(new TimeSpan(0,0,1));
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
        public SplashScreen SplashScreen { get; set; }

        public static string DefaultWindow = "window2";


        // ReSharper disable once UnusedMember.Local
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
        public string Window { get; set; } = "Client2Window1";
            
    }
}