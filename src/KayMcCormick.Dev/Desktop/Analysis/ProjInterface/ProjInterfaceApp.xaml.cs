using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Globalization ;
using System.IO ;
using System.Linq ;
using System.ServiceModel ;
using System.Text ;
using System.Windows ;
using AnalysisControls ;
using Autofac ;
#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
#if MSBUILDLOCATOR
using Microsoft.Build.Locator ;
#endif
using NLog ;
using ProjLib ;

namespace ProjInterface
{
#if false
    public class UsagesFreezableCollection : FreezableCollection<Usage>
    {
        public UsagesFreezableCollection() : base()
        {
        }

        public UsagesFreezableCollection(IEnumerable<UsageInfo> usages) : base(new List<Usage>())
        {
            foreach (var u in usages)
            {
                Usage instance = new Usage();
                foreach (var example in u.Examples)
                {
                    instance.Examples.Add(new Example()
                                          {
                                              HelpText = example.MapResult.HelpText,
                                          });
                }

                Add(instance);
            }
        }

        public UsagesFreezableCollection(IEnumerable<Usage> usages) : base(usages)
        {
        }
    }

    public class Example : AppDependencyObject
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly DependencyProperty HelpTextProperty =
 DependencyProperty.Register("HelpText", typeof(string), typeof(Example), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnHelpTextChanged)));

        private static void OnHelpTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Example ex = (Example)d;
            Logger.Debug($"OnHelpTextChanged: old={args.OldValue}; new={args.NewValue}");

            ex.RaiseEvent(new RoutedPropertyChangedEventArgs<string>((string)args.OldValue, (string)args.NewValue)
            {
                RoutedEvent = Example.HelpTextChangedEvent,
            });


        }

        public string HelpText
        {
            get { return (string)GetValue(HelpTextProperty); }
            set { SetValue(HelpTextProperty, value); }
        }


        public event RoutedPropertyChangedEventHandler<string> HelpTextChanged
        {
            add
            {
                Logger.Debug("Add handler to help text changed event");
                AddHandler(HelpTextChangedEvent, value);
            }
            remove
            {
                Logger.Debug("remove handler to help text changed event");
                RemoveHandler(HelpTextChangedEvent, value);
            }
        }

        public static readonly RoutedEvent HelpTextChangedEvent =
 EventManager.RegisterRoutedEvent("HelpTextChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(Example));

        // protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<string> args)
        // {
        //     RaiseEvent(args);
        // }

        //  Example e2 = (Example) d; 
        // RoutedPropertyChangedEventArgs<string> e3 = new RoutedPropertyChangedEventArgs<string>((string) e.OldValue, (string) e.NewValue);
        //     e2.OnHelpTextChanged(e3);
        // }
        //     throw new NotImplementedException();
        // }
    }

    public class UsageConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.IsSubclassOf(typeof(UsageInfo)))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);

        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            UsageInfo i = value as UsageInfo;
            if (i != null)
            {
                Usage u = new Usage();
                TypeConverter c = TypeDescriptor.GetConverter(typeof(Example));
                u.Examples =
                    new FreezableCollection<Example>(i.Examples.ToList()
                                                      .ConvertAll<Example>(input => (Example)c.ConvertFrom(input)));
                return u;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new DependencyPropertyDescriptor[] { DependencyPropertyDescriptor.FromProperty(Usage.ExamplesProperty, value.GetType()) });
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [TypeConverter(typeof(UsageConverter))]
    [DataObject( true)]
    [DefaultBindingProperty( "Examples")]
    public class Usage : AppDependencyObject
    {
        public static readonly DependencyProperty ExamplesProperty =
            DependencyProperty.Register("Examples", typeof(FreezableCollection<Example>), typeof(Usage), new FrameworkPropertyMetadata(new FreezableCollection<Example>(), FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnExamplesChanged)));

        private static void OnExamplesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Usage u = (Usage)d;
            u.RaiseEvent(new RoutedPropertyChangedEventArgs<FreezableCollection<Example>>((FreezableCollection<Example>)e.OldValue, (FreezableCollection<Example>)e.NewValue));
        }

        [DataObjectField( false, false, false)]
        public FreezableCollection<Example> Examples
        {
            get { return (FreezableCollection<Example>)GetValue(ExamplesProperty); }
            set { SetValue(ExamplesProperty, value); }
        }

        public static readonly RoutedEvent ExamplesChangedEvent =
 EventManager.RegisterRoutedEvent("ExamplesChanged",
                                                                                                   RoutingStrategy.Bubble,
                                                                                                   typeof(RoutedPropertyChangedEventHandler<FreezableCollection<Example>>), typeof(Usage));

        public event RoutedPropertyChangedEventHandler<FreezableCollection<Example>> ExamplesChanged
        {
            add { AddHandler(ExamplesChangedEvent, value); }
            remove { RemoveHandler(ExamplesChangedEvent, value); }
        }
    }

    public class AppDependencyObject : FrameworkContentElement
    {
    }
#endif
    public partial class ProjInterfaceApp : BaseApp
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

#if COMMANDLINE
private Type[] _optionTypes ;
        private Options _options ;
#endif
        public ProjInterfaceApp ( )
        {
#if false
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
        protected override void OnStartup ( StartupEventArgs e )
        {
            var start = DateTime.Now ;
            base.OnStartup ( e ) ;

            var service = new AppInfoService ( start ) ;


            Trace.Listeners.Add ( new NLogTraceListener ( ) ) ;

            var host = new ServiceHost (
                                        service
                                      , new Uri ( "http://localhost:8736/ProjInterface/App" )
                                       ) ;
            host.Open ( ) ;
#if COMMANDLINE
            ArgParseResult.WithParsed < Options > ( TakeOptions ) ;
#endif
            Logger.Info ( "{}" , nameof ( OnStartup ) ) ;
            var lifetimeScope = InterfaceContainer.GetContainer (
                                                                 new ProjInterfaceModule ( )
                                                               , new AnalysisControlsModule ( )
                                                                ) ;
            var appViewModel = lifetimeScope.Resolve < IApplicationViewModel > ( ) ;
#if false
            foreach ( var view1 in lifetimeScope.Resolve < IEnumerable < IView1 > > ( ) )
            {
                if ( view1 is Window vW )
                {
                    vW.Show ( ) ;
                }
                else
                {
                    Window w = new Window ( ) ;
                    w.Content = view1 ;
                    w.Show ( ) ;
                }
            }
#endif
            var windowType = typeof ( Window1 ) ;
            try
            {
                var mainWindow = ( Window ) lifetimeScope.Resolve ( windowType ) ;
                // mainWindow.SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
                mainWindow.Show ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
                Utils.HandleInnerExceptions ( ex ) ;
                MessageBox.Show ( ex.Message , "Error" ) ;
            }
#if MSBUILDLOCATOR
            var instances = MSBuildLocator.QueryVisualStudioInstances ( )
                                          .Where (
                                                  ( instance , i )
                                                      => instance.Version.Major    == 16
                                                         && instance.Version.Minor == 4
                                                 ) ;
            var visualStudioInstance = instances.First ( ) ;
            MSBuildLocator.RegisterInstance ( visualStudioInstance ) ;
            Logger.Debug ( "REgistering MSBuild  instance {vs}" , visualStudioInstance.Name ) ;
#endif

            var elapsed = DateTime.Now - start ;
            Console.WriteLine ( elapsed.ToString ( ) ) ;
            Logger.Info ( "Initialization took {elapsed} time." , elapsed ) ;
        }

        protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }

#if COMMANDLINE
        private void TakeOptions ( Options obj ) { _options = obj ; }

        public override Type[] OptionTypes => new [] { typeof(Options) } ;
#endif
#if false
        protected override void OnArgumentParseError ( IEnumerable < object > obj )
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Usage));
            var usages1 = CommandLine.Text.HelpText
                                     .UsageTextAs(ArgParseResult, example => example);
            Logger.Debug(String.Join(", ", usages1.ToList().ConvertAll<string>(input =>
                                                                                        TypeDescriptor.GetConverter(typeof(UsageInfo)).ConvertToString(input))));

            IEnumerable<Usage> usages = usages1
                                       .ToList().ConvertAll<Usage>(input => (Usage)converter.ConvertFrom(input));
            Window w = new Window();
            var ctrl = new CommandLineParserMessages
                       { Usages = new UsagesFreezableCollection(usages) };
            w.Content = ctrl;
            w.ShowDialog();
//            ErrorExit(ExitCode.ArgumentsError);
            StringBuilder b = new StringBuilder(200);

            var r = new StringReader(b.ToString());
            try
            {
                string s ;
                while ( ( s = r.ReadLine ( ) ) != null )
                {
                    Logger.Error ( s ) ;
                }

                MessageBox.Show ( b.ToString ( ) , "Help text" ) ;
                ErrorExit ( ExitCode.ArgumentsError ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( "Exception {exception}" , ex ) ;

                ErrorExit ( ExitCode.ArgumentsError ) ;
            }
    }
#endif
    }
#if COMMANDLINE
    public class Options : BaseOptions
    {
        [ Option ( 'b' ) ]
        public bool BatchMode { get ; set ; }

    }
#endif

    public class ProjInterfaceModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            builder.Register (
                              ( context , parameters )
                                  => new ProjMainWindow (
                                                         context
                                                            .Resolve < IWorkspacesViewModel > ( )
                                                       , context.Resolve < ILifetimeScope > ( )
                                                        )
                             )
                   .AsSelf ( ) ;
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