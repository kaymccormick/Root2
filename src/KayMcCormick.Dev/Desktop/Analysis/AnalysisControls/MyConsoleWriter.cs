using System;
using System.IO;
using System.Text;
using System.Windows.Threading;
using WpfTerminalControlLib;

namespace AnalysisControls
{
    public class MyConsoleWriter : TextWriter
    {
        public WpfTerminalControl ConsoleTerm { get; }

        public MyConsoleWriter(WpfTerminalControl consoleTerm)
        {
            ConsoleTerm = consoleTerm;
        }

        public override Encoding Encoding { get; } = Encoding.UTF8;

        public override void WriteLine(string value)
        {
            // if (ConsoleTerm.Dispatcher.CheckAccess())
            // {
            // Task.Run(() => ConsoleTerm.WriteLine(DateTime.Now.ToString() + ": " + value));
            // return;
            // }
            ConsoleTerm.Dispatcher.InvokeAsync(() => { ConsoleTerm.WriteLine(DateTime.Now.ToString() + ": " +value); }, DispatcherPriority.Send);
        }
    }
}