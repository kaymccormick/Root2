using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalysisControls;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
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
            void RunApp()
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

            // Environment.SetEnvironmentVariable("DISABLE_LOGGING", "Yes");
            var loggingConfiguration = AppLoggingConfiguration.Default ;
            loggingConfiguration.IsEnabledCacheTarget = true ;
            loggingConfiguration.MinLogLevel          = LogLevel.Debug ;
            loggingConfiguration.IsEnabledDebuggerTarget = false;

            Main1Model.SelectVsInstance();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => DebugUtils.WriteLine(message),
                loggingConfiguration);

            BindingOperations.CollectionRegistering += BindingOperationsOnCollectionRegistering;
            BindingOperations.CollectionViewRegistering += BindingOperationsOnCollectionViewRegistering;
            AppDomain.CurrentDomain.ProcessExit += ( sender , args )
                => LogManager.GetCurrentClassLogger ( ).Debug ( "Process exiting." ) ;

            RunApp();
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