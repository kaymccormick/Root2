
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO ;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input ;
using System.Windows.Markup ;
using System.Windows.Media ;
using Autofac;
using CodeAnalysisApp1 ;
using KayMcCormick.Dev.DataBindingTraceFilter ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Logging.Common;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog;
using ProjLib ;
using Application = System.Windows.Application;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ProjInterfaceApp : Application
    {
        public IWorkspacesViewModel ViewModel { get ;  }
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        public ProjInterfaceApp()
        {
#if DEBUG
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => Debug.WriteLine(message));
            Logger.Warn("{}", nameof(ProjInterfaceApp));
            PresentationTraceSources.Refresh();
            var bs = PresentationTraceSources.DataBindingSource;
            bs.Switch.Level = SourceLevels.Verbose ;
            bs.Listeners.Add(new BreakTraceListener());
            var nLogTraceListener = new NLogTraceListener ( ) ;
            nLogTraceListener.Filter = new MyTraceFilter ( ) ;
            bs.Listeners.Add ( nLogTraceListener ) ;
#endif
        }

      
        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {


             void TestFormattedCodeControl()
            {
                FormattedCode codeControl = new FormattedCode();
                var sourceText = ProjLib.LibResources.Program_Parse;
                codeControl.SourceCode = sourceText;
                Window w = new Window();
                w.Content = codeControl;

                CodeAnalyseContext context = CodeAnalyseContext.Parse(sourceText, "test1");
                var (syntaxTree, model, compilationUnitSyntax) = context;
                Logger.Info("Context is {Context}", context);
                codeControl.SyntaxTree            = syntaxTree;
                codeControl.Model                 = model;
                codeControl.CompilationUnitSyntax = compilationUnitSyntax;
                codeControl.Refresh();

                var argument1 = XamlWriter.Save(codeControl.FlowViewerDocument);
                File.WriteAllText(@"c:\data\out.xaml", argument1);
                Logger.Info("xaml = {xaml}", argument1);
                var tree = Transforms.TransformTree(context.SyntaxTree);
                Logger.Info("Tree is {tree}", tree);
                w.ShowDialog();

            }
             
            var start = DateTime.Now ;
            base.OnStartup(e);
            Logger.Info("{}", nameof(OnStartup));
            var lifetimeScope = InterfaceContainer.GetContainer();
            try
            {
                var mainWindow = lifetimeScope.Resolve < ProjMainWindow > ( ) ;
                mainWindow.Show ( ) ;
            }
            catch ( Exception ex )

            {
                Logger.Error ( ex , ex.ToString) ;
                KayMcCormick.Dev.Utils.HandleInnerExceptions(ex);
                MessageBox.Show ( ex.Message , "Error" ) ;
            }

            var elapsed = DateTime.Now - start ;
            Console.WriteLine ( elapsed.ToString ( ) ) ;
            Logger.Info ( "Initialization took {elapsed} time." , elapsed ) ;
        }
        

        /// <summary>Raises the <see cref="E:System.Windows.Application.Exit" /> event.</summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e) { base.OnExit(e); }
    }

    public class BreakTraceListener : TraceListener
    {
        private bool _doBreak ;

        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write. </param>
        public override void Write(string message)
        {
            if(DoBreak)
            Debugger.Break();
        }

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine(string message)
        {
            if(DoBreak) { Debugger.Break();}
        }

        public bool DoBreak { get => _doBreak ; set => _doBreak = value ; }
    }
}
