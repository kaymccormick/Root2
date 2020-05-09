using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Resources;
using System.Runtime.ExceptionServices ;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Media;
using System.Windows.Threading ;
using System.Xaml;
using AnalysisAppLib;
using AnalysisControls;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using XamlReader = System.Windows.Markup.XamlReader;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal sealed partial class App : Application
    {
        public App ( ) {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        public static ResourceDictionary ControlsResources()
        {
            var assembly = typeof(AnalysisControlsModule).Assembly;
            var x = new ResourceManager(
                "AnalysisControls.g"
                , assembly
            );

            var y = x.GetStream("templates.baml");
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader(y, new XamlReaderSettings());

            var oo = (ResourceDictionary)XamlReader.Load(b);
            return oo;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (var eventSource in EventSource.GetSources())
            {
                var n = eventSource.Name;
                try
                {
                    var s = new TraceSource(n);
                    if (s.Listeners != null) s.Listeners.Add(new MyTraceListener(s));
                }
                catch
                {}
            }
            PresentationTraceSources.Refresh();
            foreach (var p in typeof(PresentationTraceSources).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                var v = p.GetValue(null);
                if (v is TraceSource ts)
                {
                    DebugUtils.WriteLine(ts.Name);
                    ts.Listeners.Add(new MyTraceListener(ts));
                    var sourceSwitch = new SourceSwitch(ts.Name);
                    sourceSwitch.Level = SourceLevels.All;
                    ts.Switch = sourceSwitch;
                }
            }
            {

            }
            void NewFunction(DependencyObject enhancedCodeControl)
            {
                var c = enhancedCodeControl.GetType();
                while (c != null)
                {
                    foreach (var propertyInfo in c.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        var value = propertyInfo.GetValue(null);
                        if (value is DependencyProperty d)
                        {
                            var dpd = DependencyPropertyDescriptor.FromProperty(d, enhancedCodeControl.GetType());
                            dpd.AddValueChanged(enhancedCodeControl,
                                (sender, args) => { DebugUtils.WriteLine($"{args.GetType()}: {d.Name}"); });
                        }
                    }

                    c = c.BaseType;
                }
            }
            
            base.OnStartup(e);

            var ctx = AnalysisService.Load(@"C:\temp\program.cs", "x");
                EnhancedCodeWindow ww = new EnhancedCodeWindow();
                ww.SyntaxTree = ctx.SyntaxTree;
                ww.Compilation = ctx.Compilation;
                
                ww.DataContext = ctx;
                ww.ShowDialog();
            
                return;
            // EnhancedCodeControl control = new EnhancedCodeControl();
            // var w = new MyWindow();

            // NewFunction(w);
            // w.Content = control;

            // control.BeginInit();
            // NewFunction(control);

            // control.BorderThickness = new Thickness(3);
            // control.BorderBrush = Brushes.Pink;
            // control.VerticalAlignment = VerticalAlignment.Stretch;
            // control.HorizontalAlignment = HorizontalAlignment.Stretch;

            // control.SyntaxTree = tree;
            // control.Compilation = compilation;
            // control.Model = compilation.GetSemanticModel(tree);
            // control.EndInit();

            // w.ShowActivated = true;
            // w.ShowDialog();
        }

        public void TestSyntaxControl(SyntaxNodeControl control)
        { 
            control.BorderThickness = new Thickness(3);
            control.BorderBrush = Brushes.Pink;
            control.VerticalAlignment = VerticalAlignment.Stretch;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            var tree = SetupSyntaxParams(out var compilation);

            control.SyntaxTree = tree;
            control.Compilation = compilation;
            control.Model = compilation.GetSemanticModel(tree);
            
            var w = new MyWindow { Content = control, ShowActivated = true };
            w.ShowDialog();
        }

        public static SyntaxTree SetupSyntaxParams(out CSharpCompilation compilation, string code = null)
        {
            if (code == null)
            {
                code = AnalysisAppLib.Properties.Resources.Program_Parse;
            }
            var unitSyntax = SyntaxFactory.ParseCompilationUnit(code)
                .NormalizeWhitespace("    ");
            var tree = SyntaxFactory.SyntaxTree(unitSyntax);

            compilation = AnalysisService.CreateCompilation("x", tree);
            return tree;
        }

        private void OnDispatcherUnhandledException (
            object                                sender
          , [ NotNull ] DispatcherUnhandledExceptionEventArgs e
        )
        {
            if ( e == null )
            {
                throw new ArgumentNullException ( nameof ( e ) ) ;
            }

            if ( ! ( e.Exception is TypeLoadException ) )
            {
                return ;
            }

            e.Handled = true ;
            TestApp.MainWindow.Instance?.LogMethod ( "Handled:" ) ;
            TestApp.MainWindow.Instance?.LogMethod(e.ToString());
        }

        private void CurrentDomainOnFirstChanceException (
            object                        sender
          , [ NotNull ] FirstChanceExceptionEventArgs e
        )
        {
            if ( e.Exception is TypeLoadException )
            {
            }
        }
    }

    internal class MyTraceListener : TraceListener
    {
        private readonly TraceSource _ts;

        public MyTraceListener(TraceSource ts)
        {
            _ts = ts;
            
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            DoDebug(eventCache, source, eventType, id, data);
            //DebugUtils.WriteLine($"{eventCache.Callstack[0].ToString()}");
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            DoDebug(eventCache, source, eventType, id, data);
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            DoDebug(eventCache, source, eventType, id);
            DebugUtils.WriteLine($"{eventCache.Callstack[0].ToString()}");
            base.TraceEvent(eventCache, source, eventType, id);
        }

        private void DoDebug(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            object data = null, string message = null)
        {
            foreach (var o in eventCache.LogicalOperationStack)
            {

                DebugUtils.WriteLine($"{o}");
            }
            DebugUtils.WriteLine($"{source} {eventType} {id} {message}");
            if (data != null)
                if (data is object[] oo)
                {
                    foreach (var o in oo)
                    {
                        DebugUtils.WriteLine(o.ToString());
                    }
                }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            DoDebug(eventCache, source, eventType, id, null, message);
            //DebugUtils.WriteLine($"{eventCache.Callstack[0].ToString()}");
            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format,
            params object[] args)
        {
            //DebugUtils.WriteLine($"{eventCache.Callstack[0].ToString()}");
            base.TraceEvent(eventCache, source, eventType, id, format, args);

        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
         //   DebugUtils.WriteLine($"{eventCache.Callstack[0].ToString()}");
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        public override void Write(string message)
        {
            DebugUtils.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            DebugUtils.WriteLine(message);
        }
    }

    internal class MyWindow: Window
    {
        public MyWindow()
        {

        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            DebugUtils.WriteLine(newContent.ToString());
            base.OnContentChanged(oldContent, newContent);
        }
    }
}
