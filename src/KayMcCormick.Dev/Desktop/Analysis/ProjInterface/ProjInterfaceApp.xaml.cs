using System ;
using System.Diagnostics ;
using System.Linq ;
using System.Windows ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Build.Locator ;
using NLog ;
using ProjLib ;
using Application = System.Windows.Application ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ProjInterfaceApp : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProjInterfaceApp ( )
        {
#if DEBUG
            AppLoggingConfigHelper.EnsureLoggingConfigured (
                                                            message => Debug.WriteLine ( message )
                                                           ) ;
            Logger.Warn ( "{}" , nameof ( ProjInterfaceApp ) ) ;
#if TRACE
            PresentationTraceSources.Refresh();
            var bs = PresentationTraceSources.DataBindingSource;
            bs.Switch.Level = SourceLevels.Verbose ;
            bs.Listeners.Add(new BreakTraceListener());
            var nLogTraceListener = new NLogTraceListener ( ) ;
            nLogTraceListener.Filter = new MyTraceFilter ( ) ;
            bs.Listeners.Add ( nLogTraceListener ) ;
#endif
#endif
        }


        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup ( StartupEventArgs e )
        {
            var start = DateTime.Now ;
            base.OnStartup ( e ) ;
            Logger.Info ( "{}" , nameof ( OnStartup ) ) ;
            var lifetimeScope = InterfaceContainer.GetContainer (new ProjInterfaceModule(),
                                                                 new AnalysisControlsModule()) ;
            try
            {
                var mainWindow = lifetimeScope.Resolve < ProjMainWindow > ( ) ;
                mainWindow.Show ( ) ;
            }
            catch ( Exception ex )

            {
                Logger.Error ( ex , ex.ToString ) ;
                KayMcCormick.Dev.Utils.HandleInnerExceptions ( ex ) ;
                MessageBox.Show ( ex.Message , "Error" ) ;
            }

            var instances = MSBuildLocator.QueryVisualStudioInstances()
                                          .Where(
                                                 (instance, i)
                                                     => instance.Version.Major    == 16
                                                        && instance.Version.Minor == 4
                                                );
            MSBuildLocator.RegisterInstance(instances.First());

            var elapsed = DateTime.Now - start ;
            Console.WriteLine ( elapsed.ToString ( ) ) ;
            Logger.Info ( "Initialization took {elapsed} time." , elapsed ) ;
        }
    }

    public class ProjInterfaceModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            builder.RegisterType < ProjMainWindow > ( ).AsSelf ( ) ;

        }
        #endregion
    }

    public class BreakTraceListener : TraceListener
    {
        private bool _doBreak ;

        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write. </param>
        public override void Write ( string message )
        {
            if ( DoBreak )
            {
                Debugger.Break ( ) ;
            }
        }

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine ( string message )
        {
            if ( DoBreak ) { Debugger.Break ( ) ; }
        }

        public bool DoBreak { get => _doBreak ; set => _doBreak = value ; }
    }
}