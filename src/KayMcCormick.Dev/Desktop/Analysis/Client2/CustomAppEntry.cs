using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AnalysisControls;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;
using RoslynCodeControls;
using Terminal1;
using WpfTerminalControlLib;
using MyConsoleWriter = AnalysisControls.MyConsoleWriter;

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
            SplashScreen splashScreen = new SplashScreen(Assembly.GetEntryAssembly(), "Assets/SplashScreen.jpg");
            splashScreen.Show(false,true);
                
        void RunApp(SplashScreen splashScreen)
            {
                try
                {
                    var app = new Client2App();
                    app.SplashScreen = splashScreen;
                    app.Run();
                }
                catch (Exception ex)
                {
                    DebugUtils.WriteLine(ex.ToString());
                    // ExceptionInfo i = new ExceptionInfo(){DataContext=ex};
                    // Window w = new Window(){Content=i};
                    // w.WindowState = WindowState.Maximized;
                    
                    // w.Topmost = true;
                    // w.Loaded += (sender, args) => { w.CaptureMouse();
                        // w.Activate();
                    // };
                    // w.ShowDialog();
                }
            }

            // Environment.SetEnvironmentVariable("DISABLE_LOGGING", "Yes");
                var loggingConfiguration = AppLoggingConfiguration.Default ;
//            loggingConfiguration.IsEnabledCacheTarget = true ;
            loggingConfiguration.MinLogLevel          = LogLevel.Info ;
            loggingConfiguration.IsEnabledDebuggerTarget = false;

            Main1Model.TrySelectVsInstance();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => DebugUtils.WriteLine(message),
                loggingConfiguration);

            // BindingOperations.CollectionRegistering += BindingOperationsOnCollectionRegistering;
            // BindingOperations.CollectionViewRegistering += BindingOperationsOnCollectionViewRegistering;
            AppDomain.CurrentDomain.ProcessExit += ( sender , args )
                => LogManager.GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;

            RoslynCodeControl.StartSecondaryThread();
            RunApp(splashScreen);
#if false
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
                    RunApp();
                }

            });
#endif
        }

        private static void BindingOperationsOnCollectionViewRegistering(object sender, CollectionViewRegisteringEventArgs e)
        {
            StringBuilder sb = new StringBuilder(" ");
            if (e.CollectionView is IItemProperties props)
            {
                if (props.ItemProperties != null)
                    foreach (var itemPropertyInfo in props.ItemProperties)
                    {
                        sb.Append($"{itemPropertyInfo.Name} ; ");
                    }
            }
            e.CollectionView.CurrentChanged += (sender2, e2) => CollectionViewOnCurrentChanged(sender2, e2, e.CollectionView);
            e.CollectionView.CurrentChanging +=
                (sender3, e3) => CollectionViewOnCurrentChanging(sender3, e3, e.CollectionView);
            var sourceColl = e.CollectionView.SourceCollection;
            ConversionUtils.TypeToText(sourceColl.GetType(), sb);
            DebugUtils.WriteLine(sb.ToString());
        }

        private static void CollectionViewOnCurrentChanging(object sender, CurrentChangingEventArgs e,
            CollectionView eCollectionView)
        {
            StringBuilder sb = new StringBuilder();

            var currentItem = eCollectionView.CurrentItem;

            if (currentItem != null)
            {
                sb.Append(currentItem);
                sb.Append('(');
                ConversionUtils.TypeToText(currentItem.GetType(), sb);
                sb.Append(')');
            }

            DebugUtils.WriteLine($"{sender} current={sb} cancellable={e.IsCancelable}");
        }

        private static void CollectionViewOnCurrentChanged(object sender, EventArgs e, CollectionView eCollectionView)
        {
            StringBuilder sb = new StringBuilder();
            var currentItem = eCollectionView.CurrentItem;

            if (currentItem != null)
            {
                sb.Append('(');
                ConversionUtils.TypeToText(currentItem.GetType(), sb);
                sb.Append(')');
            }

            DebugUtils.WriteLine($"{sender} new={sb}");
        }

        private static void BindingOperationsOnCollectionRegistering(object sender, CollectionRegisteringEventArgs e)
        {
            var eParent = e.Parent;
            StringBuilder sb = new StringBuilder();

            ConversionUtils.TypeToText(e.Collection.GetType(), sb);
            var parent = "Parent = ";
            sb.Append(parent);

            if (eParent is DependencyObject o)
            {
                sb.Append(AttachedProperties.GetCustomDescription(o));
            }


            
            DebugUtils.WriteLine($"Registering {sb}");
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
            // DebugUtils.WriteLine(args.LoadedAssembly.FullName);
        }
    }
}