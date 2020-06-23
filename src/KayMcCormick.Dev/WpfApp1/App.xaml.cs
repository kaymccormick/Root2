using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AnalysisControls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            CustomConsole.ConsoleHandler(600);
            if (e.Args.Any())
            {
                LoadFilename = e.Args[0];
            }
            base.OnStartup(e);
        }

        public string LoadFilename { get; set; }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.ToString());
            e.Handled = true;
        }
    }
}
