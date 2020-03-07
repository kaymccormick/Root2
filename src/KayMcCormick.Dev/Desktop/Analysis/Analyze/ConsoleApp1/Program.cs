using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.ExceptionServices ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Autofac ;
#if COMMANDLINE
using CommandLine ;
#endif
using KayMcCormick.Dev.Logging ;
using MessageTemplates ;
using MessageTemplates.Core ;
using MessageTemplates.Structure ;
using Microsoft.Build.Locator ;

using NLog ;
using ProjLib ;
using Module = Autofac.Module ;

namespace ConsoleApp1
{
    internal class AppContext
    {
        public ILifetimeScope Scope { get ; }

        public IWorkspacesViewModel ViewModel { get ; }

        public ActionBlock < ILogInvocation > actionBlock ;

        public AppContext ( ILifetimeScope scope , IWorkspacesViewModel workspacesViewModel , ActionBlock < ILogInvocation > actionBlock )
        {
            Scope     = scope ;
            ViewModel = workspacesViewModel ;
            this.actionBlock = actionBlock ;
        }
    }

    internal class AppModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock<ILogInvocation>(Program.Action) ;
            builder.RegisterInstance(actionBlock).As<ActionBlock <ILogInvocation>>().SingleInstance();
            Pipeline pipeline = new Pipeline();
            pipeline.PipelineInstance.LinkTo ( actionBlock ) ;
            builder.RegisterInstance ( pipeline ).As < Pipeline > ( ).SingleInstance ( ) ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ) ;
        }
        #endregion
    }

    internal class Program
    {
        private static Logger Logger ;

        //============= Config [Edit these with your settings] =====================
        private static async Task Main ( string[] args )
        {
            Init ( ) ;
            var scope = InterfaceContainer.GetContainer ( new AppModule ( ) ) ;

            AppContext context ;
            try
            {
                context = scope.Resolve < AppContext > ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Fatal ( ex , ex.Message ) ;
                return ;
            }
#if COMMANDLINE
            var parsed = Parser.Default.ParseArguments < Options > ( args )
                  .WithNotParsed (
                                  errors => {
                                      Logger.Error (
                                                    string.Join (
                                                                 ", "
                                                               , errors.Select (
                                                                                error => error.Tag
                                                                               )
                                                                )
                                                   ) ;
                                  }
                                 ) ;

            if (parsed.Tag == ParserResultType.Parsed)
            {
                await MainCommand((parsed as Parsed<Options>).Value, context);
            }
#else
            await MainCommand(context);
#endif
        }

        public static void Action ( ILogInvocation invocation )
        {
            var json = JsonSerializer.Serialize( invocation ) ;
            Logger.Debug ( json ) ;
            Console.WriteLine ( json ) ;
                               // $"{invocation.MethodDisplayName}\t{invocation.SourceLocation}\t{invocation.Msgval}\t{invocation.Arguments}"
                              // ) ;
        }

        private static void Instances ( )
        {
#if false
            var instances = MSBuildLocator.QueryVisualStudioInstances (
                                                                       new
                                                                       VisualStudioInstanceQueryOptions ( )
                                                                       {
                                                                           DiscoveryTypes =
                                                                               DiscoveryType
                                                                                  .VisualStudioSetup
                                                                       }
                                                                      ) ;
            foreach ( var visualStudioInstance in instances )
            {
                Logger.Info (
                             "{name} {x}"
                           , visualStudioInstance.Name
                           , visualStudioInstance.Version
                            ) ;
            }
#endif
        }

        private static async Task MainCommand ( 
            #if COMMANDLINE
            Options options ,
            #endif
            AppContext context )
        {
#if COMMANDLINE
            if ( options.FirstChance )
            {
                AppDomain.CurrentDomain.FirstChanceException +=
                    CurrentDomainOnFirstChanceException ;
            }
#endif

            var viewModel = context.ViewModel ;
            var x = ( ISupportInitialize ) viewModel ;
            x.BeginInit ( ) ;
            x.EndInit ( ) ;
#if COMMANDLINE
            if ( options.ListVsInstances )
            {
                foreach ( var vsInstance in viewModel.VsCollection )
                {
                    var format = "{I}\n" ;
                    var t = MessageTemplate.Parse ( format ) ;
                    var converter = TypeDescriptor.GetConverter ( typeof ( IVsInstance ) ) ;
                    var canConvertTo = converter.CanConvertTo ( typeof ( TemplatePropertyValue ) ) ;
                    var converted = converter.ConvertTo (
                                                         vsInstance
                                                       , typeof ( TemplatePropertyValue )
                                                        ) ;
                    Logger.Info ( "" + "converted = {converted}" , converted ) ;
                    var templateProperties = new[]
                                             {
                                                 new TemplateProperty (
                                                                       "I"
                                                                     , ( TemplatePropertyValue )
                                                                       converted
                                                                      )
                                             } ;

                    var plist = new TemplatePropertyList ( templateProperties ) ;
                    t.Render ( new TemplatePropertyValueDictionary ( plist ) , Console.Out ) ;

                    // var line = MessageTemplates.MessageTemplate.Format(format, vsInstance.DisplayName, vsInstance.InstallationVersion, vsInstance.Product.GetVersion(), vsInstance.IsPrerelease ? " [PREVIEW]" : "");
                    // Console.WriteLine(l\ine);
                }
            }
            #endif

#if MSBUILDLOCATOR
            var instances = MSBuildLocator.QueryVisualStudioInstances ( )
                                          .Where (
                                                  instance 
                                                      => instance.Version.Major    == 16
                                                         && instance.Version.Minor == 4
                                                 ) ;
            MSBuildLocator.RegisterInstance ( instances.First ( ) ) ;
#endif
            int i = 0 ;
            var browserNodeCollection = viewModel.ProjectBrowserViewModel.RootCollection ;
            List < IBrowserNode >
                nodes = new List < IBrowserNode > ( browserNodeCollection.Count ) ;
            foreach ( var browserNode in browserNodeCollection )
            {
                i += 1 ;
                Console.WriteLine ( $"{i}: {browserNode.Name}" ) ;
                nodes.Add ( browserNode ) ;
                if ( browserNode is IProjectBrowserNode project )
                {
                    Console.WriteLine ( $"\tSolutionPath is {project.SolutionPath}" ) ;
                    Console.WriteLine (
                                       $"\tConfiguration property Platform is {project.Platform ?? "Null"}"
                                           ) ;
                    Console.WriteLine ($"\tRepositoryUrl is {project.RepositoryUrl}" ) ;
                }
            }

            var key = Console.ReadKey ( ) ;
            if ( ! Char.IsDigit ( key.KeyChar ) )
            {
                return ;

            }

            var selection = (int)char.GetNumericValue ( key.KeyChar ) ;

            var projectNode = nodes[selection - 1] as IProjectBrowserNode;

            if ( projectNode != null )
            {
                Console.WriteLine ( projectNode.SolutionPath ) ;
                _ = Console.ReadLine ( ) ;
                await viewModel.AnalyzeCommand ( projectNode ) ;
            }

            var pipe = viewModel.PipelineViewModel.Pipeline.PipelineInstance ;


            var timeSpan = new TimeSpan ( 0 , 15, 0 ) ;
            Logger.Info ( "waiting " + timeSpan ) ;

            await context.actionBlock.Completion ;

            if ( pipe.Completion.IsFaulted )
            {
                Logger.Error ( pipe.Completion.Exception , pipe.Completion.Exception.ToString ) ;
            }
        }

        private static void Init ( )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured (
                                                            null
                                                          , new AppLoggingConfiguration ( )
                                                            {
                                                                IsEnabledConsoleTarget = true
                                                            }
                                                           ) ;
            Logger = LogManager.GetCurrentClassLogger ( ) ;
        }

        private static void CurrentDomainOnFirstChanceException (
            object                        sender
          , FirstChanceExceptionEventArgs e
        )
        {
            if ( e.Exception is ReflectionTypeLoadException r )
            {
                var i = 0 ;
                foreach ( var rLoaderException in r.LoaderExceptions )
                {
                    Console.WriteLine ( rLoaderException.Message ) ;
                    Console.WriteLine ( r.Types[ i ] ) ;
                    i += 1 ;
                }
            }

            Console.WriteLine ( "FIRST CHANCE EXCEPTION\n" + e.Exception.ToString ( ) ) ;
        }
    }
#if COMMANDLINE
    internal class Options
    {
        private bool _listVsInstances ;

        [ Option ( HelpText = "Version for visual studio selection" ) ]
        public string VsVersion { get ; set ; }

        [ Option ( 'l' , "list-vs-instances" ) ]
        public bool ListVsInstances { get => _listVsInstances ; set => _listVsInstances = value ; }

        [ Option ( 'f' ) ]
        public bool FirstChance { get ; set ; }
    }
#endif

}