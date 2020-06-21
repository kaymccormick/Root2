using System ;

using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows ;
using System.Windows.Media ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.DataBindingTraceFilter;
using KayMcCormick.Dev.Logging ;
using KmDevLib;
using NLog ;
using NLog.Targets;
using Application = System.Windows.Application ;

#if COMMANDLINE
using CommandLine ;
using CommandLine.Text ;
#endif

namespace KayMcCormick.Lib.Wpf
{
#if COMMANDLINE
    public abstract class BaseOptions
    {
        [ Option ( 'q' ) ]
        public bool QuitOnError { get ; set ; }

        [ Option ( 't' ) ]
        public bool EnableTracing { get ; set ; }
    }
#endif
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public abstract class BaseApp : Application , IDisposable
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly bool _disableLogging ;

        private readonly ApplicationInstanceBase _applicationInstance ;
        private readonly ApplicationInstanceBase _createdAppInstance ;
        
        private MiscInstanceInfoProvider _miscInstanceInfoProvider ;

        /// <summary>
        /// </summary>
        protected BaseApp ( ) : this ( null ) { }

        /// <summary>
        /// </summary>
        /// <param name="applicationInstance"></param>
        /// <param name="disableLogging"></param>
        /// <param name="disableRuntimeConfiguration"></param>
        /// <param name="disableServiceHost"></param>
        /// <param name="modules"></param>
        /// <param name="initAction"></param>
        protected BaseApp (
            [ CanBeNull ] ApplicationInstanceBase applicationInstance         = null
          , bool                    disableLogging              = false
          , bool                    disableRuntimeConfiguration = false
          , bool                    disableServiceHost          = false
          , [ CanBeNull ] IModule[]               modules                     = null
            // ReSharper disable once UnusedParameter.Local
          , Action                  initAction                  = null
        )
        {
            _disableLogging = disableLogging ;

            if ( applicationInstance != null )
            {
                _applicationInstance = applicationInstance ;
            }
            else
            {
                _applicationInstance = _createdAppInstance = new ApplicationInstance (
                                                                                      new
                                                                                          ApplicationInstance.ApplicationInstanceConfiguration (
                                                                                                                                                message
                                                                                                                                                    => {
#if TRACEPROVIDER
                                                                                                                                                        PROVIDER_GUID
                                                                                                                                                       .EventWriteSETUP_LOGGING_EVENT (
                                                                                                                                                                                       message
                                                                                                                                                                                      ) ;
#endif
                                                                                                                                                }
                                                                                                                                                // ReSharper disable once VirtualMemberCallInConstructor
                                                                                                                                              , ApplicationGuid
                                                                                                                                              , null
                                                                                                                                              , disableLogging
                                                                                                                                              , disableRuntimeConfiguration
                                                                                                                                              , disableServiceHost
                                                                                                                                               )
                                                                                     ) ;
            }


            if ( modules != null )
            {
                foreach ( var module in modules )
                {
                    _applicationInstance.AddModule ( module ) ;
                }
            }

            // initAction?.Invoke ( ) ;
            _applicationInstance.Initialize ( ) ;
            _applicationInstance.Startup ( ) ;
            Scope = _applicationInstance.GetLifetimeScope ( ) ;
            MyReplaySubject<string> my1=null;
            try
            {
                my1 = Scope.Resolve<MyReplaySubject<string>>();
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }

            MyCacheTarget2 t = new MyCacheTarget2(my1);
            AppLoggingConfigHelper.CacheTarget2 = t;
            AppLoggingConfigHelper.AddTarget(t, LogLevel.Debug);

            var options = Scope.Resolve<JsonSerializerOptions>();
            options.Converters.Add(new JsonConverterLogEventInfo());
            TypeDescriptor.AddProvider(new InstanceInfoProvider(), typeof(InstanceInfo));

            var provider = Scope.Resolve<IControlsProvider>();
            foreach (var providerType in provider.Types)
            {
                // if (providerType.Assembly.FullName == typeof(CSharpSyntaxNode).Assembly.FullName)
                // {
                // DebugUtils.WriteLine(providerType.ToString());
                // }
                DebugUtils.WriteLine(providerType.ToString());
                TypeDescriptor.AddProvider(provider.Provider, providerType);
                
            }

            
            foreach ( var myJsonLayout in LogManager
            .Configuration.AllTargets.OfType < TargetWithLayout > ( )
            .Select ( t => t.Layout )
            .OfType < MyJsonLayout > ( ) )
            {
                myJsonLayout.Options = options;
             }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract Guid ApplicationGuid { get ; }

        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [ NotNull ]
        public virtual ILifetimeScope BeginLifetimeScope ( [ NotNull ] object tag )
        {
            return Scope.BeginLifetimeScope ( tag ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public virtual ILifetimeScope BeginLifetimeScope ( )
        {
            return Scope.BeginLifetimeScope ( ) ;
        }

        /// <summary>
        /// </summary>
        protected virtual ILifetimeScope Scope { get ; set ; }

        /// <summary>Gets a value indicating whether [do tracing].</summary>
        /// <value>
        ///     <see language="true" /> if [do tracing]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTracing
        protected virtual bool DoTracing { get ; } = false ;

        /// <summary>
        /// </summary>
        protected virtual void SetupTracing ( )
        {
            
            bool TraceAll = false;
            bool tracenone = true;
            PresentationTraceSources.Refresh ( ) ;
            
            foreach (var propertyInfo in typeof(PresentationTraceSources).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if (propertyInfo.PropertyType == typeof(TraceSource))
                {
                    TraceSource t = (TraceSource) propertyInfo.GetValue(null);
                    t.Switch.Level = tracenone ? SourceLevels.Off : SourceLevels.All;
            //        t.Listeners.Add(new ConsoleTraceListener());
                }
            }
            
            if ( ! DoTracing )
            {
                return ;
            }
#if TRACE
            // var nLogTraceListener = new NLogTraceListener ( ) ;
            var routedEventSource = PresentationTraceSources.RoutedEventSource ;
            // nLogTraceListener.DefaultLogLevel = LogLevel.Debug ;
            // nLogTraceListener.ForceLogLevel   = LogLevel.Warn ;
            //nLogTraceListener.LogFactory      = AppContainer.Resolve < LogFactory > ( ) ;
            // nLogTraceListener.AutoLoggerName = false ;
            //nLogTraceListener.
            // routedEventSource.Switch.Level = SourceLevels.All ;
            var db
                = PresentationTraceSources.DataBindingSource;
            var breakTraceListener = new BreakTraceListener();
            breakTraceListener.DoBreak = false;
            breakTraceListener.Filter = new BreakFilter();
            db.Listeners.Add(breakTraceListener);
            var xmlWriterTraceListener = new XmlWriterTraceListener(@"C:\temp\out2.xml");
            xmlWriterTraceListener.Filter = new BreakFilter();
            db
.Listeners.Add(xmlWriterTraceListener);
             // db
// .Listeners.Add(new XX());
            db.Switch.Level = SourceLevels.All;


            // var foo = Scope.Resolve < IEnumerable < TraceListener > > ( ) ;
            // foreach ( var tl in foo )
            // {
            // routedEventSource.Listeners.Add ( tl ) ;
            // }

            // routedEventSource.Listeners.Add ( new AppTraceListener ( ) ) ;
            // routedEventSource.Listeners.Add ( nLogTraceListener ) ;
#endif
        }

        /// <summary>
        /// </summary>
        protected virtual LogDelegates.LogMethod DebugLog { get ; set ; }

        /// <summary>Gets the configuration settings.</summary>
        /// <value>The configuration settings.</value>
        public virtual List < object > ConfigSettings { get ; } = new List < object > ( ) ;

        /// <summary>
        /// </summary>
        protected virtual ILogger Logger { get ; set ; }

        /// <summary>
        /// </summary>
        /// <param name="exitCode"></param>
        protected virtual void ErrorExit ( ExitCode exitCode = ExitCode.GeneralError )
        {
            var code = Convert.ChangeType ( ( object ) exitCode , ( TypeCode ) exitCode.GetTypeCode ( ) ) ;
            if ( code == null )
            {
                return ;
            }

            var intCode = ( int ) code ;

            if ( Current == null )
            {
                Process.GetCurrentProcess ( ).Kill ( ) ;
            }
            else
            {
                Current.Shutdown ( intCode ) ;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        protected virtual IEnumerable < IModule > GetModules ( )
        {
            return Array.Empty < IModule > ( ) ;
        }

#region Overrides of Application
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            _createdAppInstance?.Dispose ( ) ;
        }

        /// <summa
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup ( StartupEventArgs e )
        {
            
            base.OnStartup ( e ) ;
            SetupTracing();
#if COMMANDLINE
            var optionTypes = OptionTypes ;
            var args = e.Args ;
            if ( e.Args.Length       == 0
                 || e.Args[ 0 ][ 0 ] == '-' )
            {
                args = args.Prepend ( "default" ).ToArray ( ) ;
            }
            ArgParseResult = Parser.Default.ParseArguments ( args , optionTypes ) ;
             ArgParseResult.WithNotParsed ( OnArgumentParseError ) ;
#endif
        }


#if COMMANDLINE
protected abstract void OnArgumentParseError ( IEnumerable < object > obj ) ;

        public ParserResult < object > ArgParseResult
        {
            get => _argParseResult ;
            set => _argParseResult = value ;
        }
        public virtual Type[] OptionTypes => _optionType ;
#endif
#endregion

#region IDisposable
        /// <summary>
        /// </summary>
        public virtual void Dispose ( ) { _applicationInstance?.Dispose ( ) ; }
#endregion
    }

    public class BreakFilter : TraceFilter
    {
        private class Info1
        {
            public bool IgnoreBindingErrors { get; set; }
        }

        Dictionary<string, Info1> elements = new Dictionary<string, Info1>();
        public AppBindingUtils utils = new AppBindingUtils();

        public BreakFilter()
        {
            elements["MyRibbonGalleryCategory"] = new Info1 {IgnoreBindingErrors = true};
            elements["MyRibbonGallery"] = new Info1 { IgnoreBindingErrors = true };
            elements["MyRibbonGalleryItem"] = new Info1 { IgnoreBindingErrors = true };
        }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage,
            object[] args, object data1, object[] data)
        {

            var parsed = utils.ParseBindingMessage(cache, source, eventType, id, formatOrMessage, args, data1, data);
            if (parsed == null)
            {
                return false;
            }

            if (elements.TryGetValue(parsed.TargetElementType, out var info1))
            {
                if (info1.IgnoreBindingErrors)
                {
             //       DebugUtils.WriteLine("Ignore binding error for " + parsed.TargetElementType);
                    return false;
                }
            }
            var st = Utils.ParseStackTrace(cache.Callstack);
            var stackTraceEntry = st.FirstOrDefault(t => t.Method.Text != "ShouldTrace" && t.File.Text.ToLowerInvariant().StartsWith(@"c:\users"));
            if (stackTraceEntry != null)
            {
                DebugUtils.WriteLine("stack frame is " + stackTraceEntry.Frame.Text);
            }

            if (parsed.BindingExpression?.StartsWith("Path=WindowState") ?? false)
                return false;
            DebugUtils.WriteLine(parsed.ToString());
            return true;
        }
    }

    public class AppBindingUtils
    {
        private StreamWriter utilsLog ;//= new StreamWriter(@"C:\data\logs\utils2.txt");
        private Regex _rgxp;

        public AppBindingUtils()
        {
            _rgxp = new Regex(
                @"^(.*)\s(.*BindingExpression):(.*;\s*)?(?:DataItem=(.*)\s*)?target element is ('(.*)' \((.*)\)); target property is ('(.*)' \((.*)\))");
        }

        public ParsedBindingMessage ParseBindingMessage(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            var parsed = new ParsedBindingMessage();
            //utilsLog.WriteLine("MSG: " + formatOrMessage);
            var match = _rgxp.Match(formatOrMessage);
            if (!match.Success)
            {
                utilsLog?.WriteLine($"Match failed, regex is {_rgxp.ToString()}");
                if(ThrowOnFail) throw new AppInvalidOperationException(formatOrMessage);
                return null;
            }
            var expr = match.Groups[3].Success ? match.Groups[3].Captures[0].Value : null;
            parsed.BindingExpression = expr;
            parsed.DataItem = match.Groups[4].Success ? match.Groups[4].Captures[0].Value : null;
            parsed.TargetElement = match.Groups[5].Captures[0].Value;
            parsed.TargetElementType = match.Groups[6].Captures[0].Value;
            parsed.TargetElementName = match.Groups[7].Captures[0].Value;
            parsed.TargetProperty = match.Groups[8].Captures[0].Value;
            return parsed;
        }

        public bool ThrowOnFail { get; set; }
    }

    public class ParsedBindingMessage
    {
        public string BindingExpression { get; set; }
        public string DataItem { get; set; }
        public string TargetProperty { get; set; }

        public override string ToString()
        {
            return $"{nameof(BindingExpression)}: {BindingExpression}, {nameof(DataItem)}: {DataItem}, {nameof(TargetProperty)}: {TargetProperty}, {nameof(TargetElement)}: {TargetElement}";
        }

        public string TargetElement { get; set; }
        public string TargetElementName { get; set; }
        public string TargetElementType { get; set; }
    }

    public class XX : TraceListener
    {
        public XX()
        {
            Filter = new WpfBindingFilter();
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
}