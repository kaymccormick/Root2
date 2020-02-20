using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Logging.Common;
using NLog;
using Application = System.Windows.Application;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.</summary>
        /// <exception cref="T:System.InvalidOperationException">More than one instance of the <see cref="T:System.Windows.Application" /> class is created per <see cref="T:System.AppDomain" />.</exception>
        public App()
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => Debug.WriteLine(message));
            Logger.Warn("{}", nameof(App));
            PresentationTraceSources.Refresh();
            var bs = PresentationTraceSources.DataBindingSource;
            bs.Switch.Level = SourceLevels.Error;
            bs.Listeners.Add(new BreakTraceListener());
        }

        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.Info("{}", nameof(OnStartup));
            var lifetimeScope = Container.GetContainer();

            var mainWindow = lifetimeScope.Resolve<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>Raises the <see cref="E:System.Windows.Application.Exit" /> event.</summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e) { base.OnExit(e); }
    }

    public class BreakTraceListener : TraceListener
    {
        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write. </param>
        public override void Write(string message)
        {
            Debugger.Break();
        }

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine(string message)
        {
            Debugger.Break();
        }
    }
}
