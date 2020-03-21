using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Text.Json ;
using System.Windows ;
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
using ProjInterface.JSON ;

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
        public ProjInterfaceApp ( ) : this ( null , false , false , false ) { }

        public ProjInterfaceApp (
            ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
        ) : base (
                  applicationInstance
                , disableLogging
                , disableRuntimeConfiguration
                , disableServiceHost
                , new IModule[] { new ProjInterfaceModule ( ) , new AnalysisControlsModule ( ) }
                 )

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


        protected override IEnumerable < IModule > GetModules ( ) { return appModules ; }


        protected override void OnStartup ( StartupEventArgs e )
        {
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
                    if ( ! TestMode )
                    {
                        MessageBox.Show ( ex.Message , "Error" ) ;
                    }

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

            Debug.WriteLine ( e.ToString ( ) ) ;
            if ( ! TestMode )
            {
                MessageBox.Show ( e.Exception.Message , "Error" ) ;
            }

            Current.Shutdown ( ) ;
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