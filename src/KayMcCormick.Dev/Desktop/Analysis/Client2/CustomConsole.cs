using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Terminal1;
using WpfTerminalControlLib;

namespace Client2
{
    internal static class CustomConsole
    {
        public static void NewWindowHandler()
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private static void ThreadStartingPoint()
        {
            WpfTerminalControl consoleTerm = new WpfTerminalControl() { AutoResize = true };
            TerminalCharacteristics.AddNumRowsChangedEventHandler(consoleTerm, (sender, e) =>
            {
                Debug.WriteLine("Rows: " + e.NewValue);
            });
            consoleTerm.FontSize = 18.0;
            consoleTerm.BackgroundColor = ConsoleColor.Black;
            consoleTerm.ForegroundColor = ConsoleColor.Green;
            consoleTerm.Background = Brushes.Black;
            
            Window consoleWindow = new Window();
            var grid = new Grid();
            grid.Children.Add(consoleTerm);
            consoleWindow.Content = grid;
            consoleWindow.Show();

            Console.SetOut(new MyConsoleWriter(consoleTerm));

            consoleWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}