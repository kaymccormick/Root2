using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Terminal1;
using WpfTerminalControlLib;

namespace AnalysisControls
{
    public static class CustomConsole
    {
        public static void ConsoleHandler(double left)
        {
            Thread newWindowThread = new Thread(new ParameterizedThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start(Tuple.Create(left));
        }

        private static void ThreadStartingPoint(object o)
        {
            Tuple<double> i = (Tuple<double>) o;
            WpfTerminalControl consoleTerm = new WpfTerminalControl() { AutoResize = true };
            TerminalCharacteristics.AddNumRowsChangedEventHandler(consoleTerm, (sender, e) =>
            {
                Debug.WriteLine("Rows: " + e.NewValue);
            });
            consoleTerm.FontSize = 14.0;
            consoleTerm.BackgroundColor = ConsoleColor.Black;
            consoleTerm.ForegroundColor = ConsoleColor.Green;
            SolidColorBrush bg = new SolidColorBrush{Color= Colors.Black, Opacity = .6};
            consoleTerm.Background = bg;
            
            Window consoleWindow = new Window();
            consoleWindow.AllowsTransparency = true;
            consoleWindow.Background = Brushes.Transparent;
            
            consoleWindow.Left = i.Item1;
            consoleWindow.Topmost = true;
            consoleWindow.ShowActivated = false;
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
