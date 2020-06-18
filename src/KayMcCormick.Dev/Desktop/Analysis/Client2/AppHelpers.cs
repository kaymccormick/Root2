using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Lib.Wpf;

namespace Client2
{
    internal static class AppHelpers
    {
        public static void SelectAppAction(ISelectAppModel mode, Action<ISelectAppModel> handle)
        {
            var selectWindow = new ListBox();
            // ReSharper disable once UnusedVariable
            object[] winTypes = new object[] {typeof(Client2Window1), typeof(TestRibbonWindow), WpfAppCommands.QuitApplication};
            selectWindow.ItemsSource = mode.Items;

            var w = new Window
            {
                Content = selectWindow,
                Width = 200,
                Height = 400,
                FontSize = 16,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            w.Closed += (sender, args) =>
            {
                if ((bool?)w.Tag != true)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    //var dispatcherOperation = ;////client2App2.Dispatcher.InvokeAsync(() =>
                    handle(mode);
                }

            };
            var ctx = TaskScheduler.Current;

            Task.Delay(mode.Timeout).ContinueWith(task =>
            {
                w.Dispatcher.Invoke(() =>
                {

                    w.Tag = true;
                    w.Close();
                });
            }, ctx);
            selectWindow.SelectionChanged += (sender, args) =>
            {
                mode.SetSelectedApp(args.AddedItems[0]);
                w.Tag = true;
                w.Close();
            };

            w.ShowDialog();
            // if (client2App2.StartWindow == null)
            // {
            //     client2App2.Shutdown();
            // }
            // {
            //     if (client2App2.StartWindow is Type t)
            //     {
            //         var window = (Window) Activator.CreateInstance((Type) client2App2.StartWindow);
            //         client2App2.TerminalUserControl0 = window;
            //         window.Closed += (sender, args) => { SelectAppAction(client2App2); };
            //         client2App2.RunWindow(window);
            //         
            //         
            //     } else if (client2App2.StartWindow is RoutedUICommand ui)
            //     {
            //         if (ui == WpfAppCommands.QuitApplication)
            //         {
            //             client2App2.Shutdown();
            //         }
            //     }
            // }
        }
    }
}