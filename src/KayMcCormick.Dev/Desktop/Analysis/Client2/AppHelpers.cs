using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KayMcCormick.Lib.Wpf;
using Microsoft.VisualStudio.Threading;

namespace Client2
{
    static internal class AppHelpers
    {
        public static void SelectAppAction(ISelectAppModel mode, Action<ISelectAppModel> handlel)
        {
            var selectWindow = new ListBox();
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
                    handlel(mode);
                    return;
                }

            };
            TaskScheduler ctx;
                ctx = TaskScheduler.Current;

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
            //         client2App2.MainWindow = window;
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