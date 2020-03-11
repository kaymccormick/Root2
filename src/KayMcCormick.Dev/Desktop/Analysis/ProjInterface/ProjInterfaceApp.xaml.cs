using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;
using AnalysisControls ;
using AnalysisFramework ;
using AnalysisFramework.SyntaxTransform ;
using Autofac ;
using Autofac.Core ;
#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
#if MSBUILDLOCATOR
using Microsoft.Build.Locator ;
#endif
using NLog ;
using NLog.Targets ;
using JsonConverter = System.Text.Json.Serialization.JsonConverter ;

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
        private readonly List < IModule > appModules = new List < IModule > ( ) ;
        private static readonly Logger           Logger     = LogManager.GetCurrentClassLogger ( ) ;

#if COMMANDLINE
private Type[] _optionTypes ;
        private Options _options ;
#endif
        public ProjInterfaceApp ( )

        {
            PresentationTraceSources.Refresh();
            foreach (var myJsonLayout in LogManager
                                        .Configuration.AllTargets.OfType<TargetWithLayout>()
                                        .Select(t => t.Layout)
                                        .OfType<MyJsonLayout>())
            {
                myJsonLayout.Options.Converters.Add(new JsonSyntaxNodeConverter());
            }

#if MSBUILDLOCATOR
            var instances = MSBuildLocator
                           .QueryVisualStudioInstances(
                                                      ).ToList();
            foreach ( var inst in instances )
            {
                Logger.Info (
                             "{name} {type} {msbuildpath} {version} {vspath}"
                           , inst.Name
                           , inst.DiscoveryType.ToString ( )
                           , inst.MSBuildPath
                           , inst.Version
                           , inst.VisualStudioRootPath
                            ) ;
            }
        
            if(instances.Any(
                                  (instance)
                                      => instance.Version.Major    == 16
                                         && instance.Version.Minor == 4
                                 )) {
            var visualStudioInstance = instances.First();
}
            //MSBuildLocator.RegisterInstance(visualStudioInstance);
            // var reg = MSBuildLocator.RegisterDefaults();
            MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin");
            // Logger.Debug("Registering MSBuild  instance {vs} - {path}", reg.Name, reg.MSBuildPath);
#endif
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


        public override IEnumerable < IModule > GetModules ( ) { return appModules ; }

        
        protected override void OnStartup ( StartupEventArgs e )
        {
            appModules.Add ( new ProjInterfaceModule ( ) ) ;
            #if ANALYSISCONTROLS
            appModules.Add ( new AnalysisControlsModule ( ) ) ;
#endif
            #if DEBUG
            var start = DateTime.Now ;
            #endif
            base.OnStartup ( e ) ;

            //Trace.Listeners.Add ( new NLogTraceListener ( ) ) ;


#if COMMANDLINE
            ArgParseResult.WithParsed < Options > ( TakeOptions ) ;
#endif
            Logger.Trace( "{methodName}" , nameof ( OnStartup ) ) ;

            var lifetimeScope = Scope ;
            #if ANALYSISCONTROLS
            var appViewModel = lifetimeScope.Resolve < IApplicationViewModel > ( ) ;
#endif
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

            var windowType = typeof ( ProjMainWindow ) ;
            try
            {
                var mainWindow = ( Window ) lifetimeScope.Resolve ( windowType ) ;
                mainWindow.Show ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
                Utils.HandleInnerExceptions ( ex ) ;
                MessageBox.Show ( ex.Message , "Error" ) ;
                Shutdown((int)ExitCode.ExceptionalError);
            }
            #if DEBUG
            var elapsed = DateTime.Now - start ;
            Logger.Info ( "Initialization took {elapsed} time." , elapsed ) ;
            #endif
        }



#if COMMANDLINE
protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }
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

    public class JsonSyntaxTokenConverter : JsonConverter < SyntaxToken >
    {
        #region Overrides of JsonConverter<SyntaxToken>
        public override SyntaxToken Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return default ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , SyntaxToken           value
          , JsonSerializerOptions options
        )
        {
            
        }
        #endregion
    }
    public class JsonSyntaxNodeConverter : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert ) { return typeof(CSharpSyntaxNode).IsAssignableFrom(typeToConvert) ; }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return ( JsonConverter ) Activator.CreateInstance (
                                                               typeof ( InnerConverter <> ).MakeGenericType (
                                                                                                             typeToConvert
                                                                                                            )
                                                             , BindingFlags.Instance | BindingFlags.Public
                                                             , null
                                                             , new object[] { options }
                                                             , null
                                                              ) ;
        }
        #endregion

        class InnerConverter < T > : JsonConverter < T > where T : CSharpSyntaxNode
        {
            public InnerConverter (JsonSerializerOptions options ) { }

            #region Overrides of JsonConverter<T>
            public override T Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                if ( typeToConvert == typeof ( ThisExpressionSyntax ) )
                {
                    var d = JsonSerializer.Deserialize < Dictionary < string , JsonElement > > (ref 
                                                                                        reader
                                                                                      , options
                                                                                       ) ;
                    return ( T ) ( ( CSharpSyntaxNode ) SyntaxFactory.ThisExpression ( ) ) ;
                }

                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , T                     value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStartObject();
                writer.WriteBoolean("JsonConverter", true);
                writer.WriteString("Type", value.GetType().AssemblyQualifiedName);
                writer.WritePropertyName ( "Value" ) ;
                // MemoryStream s = new MemoryStream();
                // value.SerializeTo(s);
                // writer.WriteBase64StringValue(s.GetBuffer());
                var transformed = Transforms.TransformSyntaxNode ( value ) ;
                JsonSerializer.Serialize ( writer , transformed , options ) ;
                writer.WriteEndObject();
            }
            #endregion
        }
    }

#if COMMANDLINE
    public class Options : BaseOptions
    {
        [ Option ( 'b' ) ]
        public bool BatchMode { get ; set ; }

    }
#endif
}