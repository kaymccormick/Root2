using System;
using System.Windows.Input;
using AnalysisControls;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using NLog;

namespace Client2
{
    internal static class CustomAppEntry
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [ STAThread ]
        public static void Main ( )
        {
            Environment.SetEnvironmentVariable("DISABLE_LOGGING", "Yes");
            var loggingConfiguration = AppLoggingConfiguration.Default ;
            loggingConfiguration.IsEnabledCacheTarget = true ;
            loggingConfiguration.MinLogLevel          = LogLevel.Trace ;

            Main1Model.SelectVsInstance();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => DebugUtils.WriteLine(message),
                loggingConfiguration);

            AppDomain.CurrentDomain.ProcessExit += ( sender , args )
                => LogManager.GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;

            var action = CheckModifiers();
            var model = new SelectAppModel();
            model.Timeout = new TimeSpan(0,0,4);
            model.Items = new[] {typeof(Client2App), typeof(Client2App2)};
            AppHelpers.SelectAppAction(model, (model0) =>
            {
                if (model0.SelectedItem is Type t)
                {
                    var app = (System.Windows.Application)Activator.CreateInstance(t);
                    app.Run();
                    return;
                } else
                {
                    try
                    {
                        var app = new Client2App();
                        app.Run();
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine(ex.ToString());
                    }
                }

            });
        }

        private static AppAction CheckModifiers()
        {
            var kb = InputManager.Current.PrimaryKeyboardDevice;
            var mod = kb.Modifiers;
            var combo = (ModifierKeys.Control | ModifierKeys.Shift);
            if ((mod & combo) == combo)
            {
                return AppAction.RunClient2;
            }

            return AppAction.RunClient2App2;
        }

        private static void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            DebugUtils.WriteLine(args.LoadedAssembly.FullName);
        }
    }
}