using Microsoft.VisualStudio.Services.Common ;
using Microsoft.VisualStudio.Services.Client ;
using Microsoft.TeamFoundation.SourceControl.WebApi ;
using Microsoft.VisualStudio.Services.WebApi ;
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Net.Http ;
using System.Net.Http.Headers ;
using System.Reflection ;
using System.Runtime.ExceptionServices ;
using System.Text ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Autofac ;
using CommandLine ;
using KayMcCormick.Dev.Logging ;
using MessageTemplates ;
using MessageTemplates.Core ;
using MessageTemplates.Structure ;
using Microsoft.Build.Locator ;
using Microsoft.IdentityModel.Clients.ActiveDirectory ;
using Microsoft.TeamFoundation.Core.WebApi ;
using NLog ;
using ProjLib ;
using Module = Autofac.Module ;

namespace ConsoleApp1
{
    internal class AppContext
    {
        public ILifetimeScope Scope { get ; }

        public IWorkspacesViewModel ViewModel { get ; }

        public AppContext ( ILifetimeScope scope , IWorkspacesViewModel workspacesViewModel )
        {
            Scope     = scope ;
            ViewModel = workspacesViewModel ;
        }
    }

    internal class AppModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock<LogInvocation>(Program.Action) ;
            builder.RegisterInstance(actionBlock).As<ActionBlock <LogInvocation>>().SingleInstance();
            Pipeline pipeline = new Pipeline(actionBlock);
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

            return ;
            Logger.Debug ( "heelo" ) ;
        }

        public static void Action ( LogInvocation invocation )
        {
            Console.WriteLine (
                               $"{invocation.MethodDisplayName}\t{invocation.SourceLocation}\t{invocation.Msgval}\t{invocation.Arguments}"
                              ) ;
        }

        private static void Instances ( Logger Logger )
        {
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
        }

        private static async Task MainCommand ( Options options , AppContext context )
        {
            if ( options.FirstChance )
            {
                AppDomain.CurrentDomain.FirstChanceException +=
                    CurrentDomainOnFirstChanceException ;
            }

            var viewModel = context.ViewModel ;
            var x = ( ISupportInitialize ) viewModel ;
            x.BeginInit ( ) ;
            x.EndInit ( ) ;
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
                    Logger.Info ( "converted = {converted}" , converted ) ;
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


            var instances = MSBuildLocator.QueryVisualStudioInstances ( )
                                          .Where (
                                                  ( instance , i )
                                                      => instance.Version.Major    == 16
                                                         && instance.Version.Minor == 4
                                                 ) ;
            MSBuildLocator.RegisterInstance ( instances.First ( ) ) ;

            viewModel.AnalyzeCommand (
                                      viewModel.ProjectBrowserViewModel.RootCollection
                                               .OfType < IProjectBrowserNode > ( )
                                               .First ( )
                                     ) ;
            var pipe = viewModel.PipelineViewModel.Pipeline.PipelineInstance ;

            // pipe.LinkTo (
                         // new ActionBlock < LogInvocation > (
                                                            // invocation => {
                                                                // Console.WriteLine (
                                                                                   // $"{invocation.MethodDisplayName}\t{invocation.SourceLocation}\t{invocation.Msgval}\t{invocation.Arguments}"
                                                                                  // ) ;
                                                            // }
                                                           // )
                       // , new DataflowLinkOptions ( ) { PropagateCompletion = true }
                        // ) ;


            var timeSpan = new TimeSpan ( 0 , 15, 0 ) ;
            Logger.Info ( "waiting " + timeSpan ) ;
            // var targetBlock = context.Scope.Resolve < ActionBlock < LogInvocation > > ( ) ;
            // for ( ; ; )
            // {
            //     Thread.Sleep ( 3000 ) ;
            //     Console.WriteLine(targetBlock.InputCount);
            // }
            await pipe.Completion ;
            if ( viewModel.PipelineViewModel.Pipeline.ResultBufferBlock
                          .TryReceiveAll ( out var list ) )
            {
                Logger.Info ( "Here" ) ;
                foreach ( var logInvocation in list )
                {
                    Logger.Info ( "{logInvocation}" , logInvocation ) ;
                }
            }

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
                return ;
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
}