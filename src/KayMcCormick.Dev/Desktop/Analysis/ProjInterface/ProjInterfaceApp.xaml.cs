using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using AnalysisControls ;
using AnalysisControls.Interfaces ;
using Autofac ;
using Autofac.Core ;
#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
#if MSBUILDLOCATOR
using Microsoft.Build.Locator ;
#endif
using NLog ;
using NLog.Targets ;

namespace ProjInterface
{
    public partial class ProjInterfaceApp : BaseApp
    {
        private readonly bool _disableLogging ;

        private readonly List < IModule > appModules = new List < IModule > ( ) ;

        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private JsonSerializerOptions _appJsonSerializerOptions ;

        private bool _testMode ;

        private Func < ProjInterfaceApp , ILifetimeScope , bool > _testCallback ;

#if COMMANDLINE
private Type[] _optionTypes ;
        private Options _options ;
#endif
        public ProjInterfaceApp ( ) : this ( false , false , false ) { }

        public ProjInterfaceApp (
            bool disableLogging
          , bool disableRuntimeConfiguration
          , bool disableServiceHost = false
        ) : base ( disableLogging , disableRuntimeConfiguration , disableServiceHost )

        {
            _disableLogging = disableLogging ;
            //PresentationTraceSources.Refresh();
            if ( ! disableLogging )
            {
                foreach ( var myJsonLayout in LogManager
                                             .Configuration.AllTargets
                                             .OfType < TargetWithLayout > ( )
                                             .Select ( t => t.Layout )
                                             .OfType < MyJsonLayout > ( ) )
                {
                    var jsonSerializerOptions = myJsonLayout.Options ;
                    AppJsonSerializerOptions = jsonSerializerOptions ;
                    AddJsonConverters ( jsonSerializerOptions ) ;
                }
            }
            else
            {
                var options = new JsonSerializerOptions ( ) ;
                AddJsonConverters ( options ) ;
                AppJsonSerializerOptions = options ;
            }

#if false
            var instances = MSBuildLocator.QueryVisualStudioInstances ( ).ToList ( ) ;
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

            if ( instances.Any (
                                ( instance )
                                    => instance.Version.Major == 16 && instance.Version.Minor == 4
                               ) )
            {
                var visualStudioInstance = instances.First ( ) ;
            }

            //MSBuildLocator.RegisterInstance(visualStudioInstance);
            // var reg = MSBuildLocator.RegisterDefaults();
            MSBuildLocator.RegisterMSBuildPath (
                                                @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"
                                               ) ;
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

        private static void AddJsonConverters ( JsonSerializerOptions jsonSerializerOptions )
        {
            jsonSerializerOptions.Converters.Add ( new JsonSyntaxNodeConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonConverterImage ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonConverterResourceDictionary ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new ProjInterfaceAppConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new HashtableConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonDependencyPropertyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonFontFamilyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonSolidColorBrushConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add (
                                                  new JsonResourceKeyWrapperConverterFactory ( )
                                                 ) ;
            jsonSerializerOptions.Converters.Add ( new JsonBrushConverter ( ) ) ;
        }

        public JsonSerializerOptions AppJsonSerializerOptions
        {
            get { return _appJsonSerializerOptions ; }
            set { _appJsonSerializerOptions = value ; }
        }

        public bool TestMode { get { return _testMode ; } set { _testMode = value ; } }

        public Func < ProjInterfaceApp , ILifetimeScope , bool > TestCallback
        {
            get { return _testCallback ; }
            set { _testCallback = value ; }
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
            Logger.Trace ( "{methodName}" , nameof ( OnStartup ) ) ;

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

            if ( TestMode )
            {
                var exitApplication = TestCallback ( this , lifetimeScope ) ;
                if ( exitApplication )
                {
                    Dispatcher.BeginInvokeShutdown ( DispatcherPriority.Send ) ;
                }
            }
            else
            {
                var windowType = typeof ( Window1 ) ;
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
                    Shutdown ( ( int ) ExitCode.ExceptionalError ) ;
                }
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
        private void ProjInterfaceApp_OnDispatcherUnhandledException (
            object                                sender
          , DispatcherUnhandledExceptionEventArgs e
        )
        {
            if ( e.Exception is InvalidCastException )
            {
                Logger.Fatal ( "First chance exception: " + e.Exception.ToString ( ) ) ;
                // e.Handled = true ;
                return ;
            }

            var m = new Xceed.Wpf.Toolkit.MessageBox ( ) ;
            m.Text = e.Exception.Message ;
            m.ShowDialog ( ) ;
            Current.Shutdown ( ) ;
        }
    }

    public class JsonBrushConverter : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( Brush ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonBrushConverter1 ( typeToConvert , options ) ;
        }

        public class JsonBrushConverter1 : JsonConverter < Brush >
        {
            public JsonBrushConverter1 ( Type typeToConvert , JsonSerializerOptions options ) { }
            #region Overrides of JsonConverter<Brush>
            public override Brush Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , Brush                 value
              , JsonSerializerOptions options
            )
            {
                var xaml = XamlWriter.Save ( value ) ;
                writer.WriteStartObject ( ) ;
                writer.WriteString ( "Xaml" , xaml ) ;
                writer.WriteEndObject ( ) ;
            }
            #endregion
        }
        #endregion
    }

    public class JsonResourceKeyWrapperConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( IResourceKeyWrapper1 ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonResourceKeyWrapaperConverter ( typeToConvert , options ) ;
        }

        public class JsonResourceKeyWrapaperConverter : JsonConverter < IResourceKeyWrapper1 >
        {
            public JsonResourceKeyWrapaperConverter (
                Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
            }

            #region Overrides of JsonConverter<IResourceKeyWrapper1>
            public override IResourceKeyWrapper1 Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , IResourceKeyWrapper1  value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue ( value.ResourceKeyObject.ToString ( ) ) ;
            }
            #endregion
        }
        #endregion
    }


    public class JsonSolidColorBrushConverter : JsonConverter < SolidColorBrush >
    {
        #region Overrides of JsonConverter<SolidColorBrush>
        public override SolidColorBrush Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , SolidColorBrush       value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "Color" , value.Color.ToString ( ) ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

    public class JsonFontFamilyConverter : JsonConverter < FontFamily >
    {
        #region Overrides of JsonConverter<FontFamily>
        public override FontFamily Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , FontFamily            value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString (
                                "FamilyName"
                              , value.FamilyNames[ XmlLanguage.GetLanguage ( "en-US" ) ]
                               ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

    public class JsonDependencyPropertyConverter : JsonConverter < DependencyProperty >
    {
        #region Overrides of JsonConverter<DependencyProperty>
        public override DependencyProperty Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , DependencyProperty    value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "DependencyPropertyName" , value.Name ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

    public class HashtableConverter : JsonConverter < Hashtable >
    {
        #region Overrides of JsonConverter<Hashtable>
        public override Hashtable Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , Hashtable             value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            foreach ( var q in value.Keys )
            {
                writer.WritePropertyName ( q.ToString ( ) ) ;
                JsonSerializer.Serialize ( writer , value[ q ] , options ) ;
            }

            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

    public class ProjInterfaceAppConverter : JsonConverter < ProjInterfaceApp >
    {
        #region Overrides of JsonConverter<ProjInterfaceApp>
        public override ProjInterfaceApp Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , ProjInterfaceApp      value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "ApplicationType" , value.GetType ( ).FullName ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

    public class JsonConverterResourceDictionary : JsonConverter < ResourceDictionary >
    {
        #region Overrides of JsonConverter<ResourceDictionary>
        public override ResourceDictionary Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , ResourceDictionary    value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "Source" , value.Source?.ToString ( ) ?? "" ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }

#if COMMANDLINE
    public class Options : BaseOptions
    {
        [ Option ( 'b' ) ]
        public bool BatchMode { get ; set ; }

    }
#endif
}