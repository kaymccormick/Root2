using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using Autofac.Core ;
using Autofac.Features.Metadata ;
using ConsoleMenu ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Application ;
using Microsoft.Build.Locator ;
using NLog ;

namespace ConsoleApp1
{
    internal class AppContext

    {
        private IProjectBrowserViewModel _projectBrowserViewModel ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        public ActionBlock < ILogInvocation > actionBlock ;

        public AppContext (
            ILifetimeScope                 scope
          , ActionBlock < ILogInvocation > actionBlock
          , IProjectBrowserViewModel       projectBrowserViewModel
          , IAnalyzeCommand                analyzeCommand
            //, IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > analyzeCommands
        )
        {
            Scope            = scope ;
            this.actionBlock = actionBlock ;
            BrowserViewModel = projectBrowserViewModel ;
            AnalyzeCommand   = analyzeCommand ;
            //AnalyzeCommands = analyzeCommands ;
        }

        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }

        public IAnalyzeCommand AnalyzeCommand { get ; }
    }

    internal class AppModule : Module
    {
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock < ILogInvocation > ( Program.Action ) ;
            builder.RegisterInstance ( actionBlock )
                   .As < ActionBlock < ILogInvocation > > ( )
                   .SingleInstance ( ) ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ) ;
        }
    }

    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        //============= Config [Edit these with your settings] =====================
        private static async Task < int > Main ( )
        {
            Init ( ) ;
            using ( var appinst = new ApplicationInstance (
                                                           new ApplicationInstanceConfiguration (
                                                                                                 message
                                                                                                     => {
                                                                                                 }
                                                                                                )
                                                          ) )

            {
                appinst.AddModule ( new AppModule ( ) ) ;
                appinst.AddModule ( new AnalysisAppLibModule ( ) ) ;
                ILifetimeScope scope ;
                try
                {
                    scope = appinst.GetLifetimeScope ( ) ;
                }
                catch ( ContainerBuildException buildException )
                {
                    Console.WriteLine ( buildException.Message ) ;
                    Console.WriteLine ( "Please contact your administrator for assistance." ) ;
                    return 1 ;
                }

                AppContext context ;
                try
                {
                    context = scope.Resolve < AppContext > ( ) ;
                }
                catch ( DependencyResolutionException depex )
                {
                    Exception ex1 = depex ;
                    while ( ex1 != null )
                    {
                        Logger.Debug ( ex1.Message ) ;
                        ex1 = ex1.InnerException ;
                    }

                    return 1 ;
                }
                catch ( Exception ex )
                {
                    Logger.Fatal ( ex , ex.Message ) ;
                    return 1 ;
                }

                return await MainCommandAsync ( context ) ;
            }
        }

        public static void Action ( ILogInvocation invocation )
        {
            var json = JsonSerializer.Serialize ( invocation ) ;
            Logger.Debug ( json ) ;
            Console.WriteLine ( json ) ;
            // $"{invocation.MethodDisplayName}\t{invocation.SourceLocation}\t{invocation.Msgval}\t{invocation.Arguments}"
            // ) ;
        }

        private static async Task < int > MainCommandAsync ( [ NotNull ] AppContext context )
        {
#if MSBUILDLOCATOR
            // var instances = MSBuildLocator.RegisterDefaults ( ) ;
            var menu = new Menu ( "VS Instance" ) ;
            var vsInstances = MSBuildLocator.QueryVisualStudioInstances (
                                                                         new
                                                                         VisualStudioInstanceQueryOptions
                                                                         {
                                                                             DiscoveryTypes =
                                                                                 DiscoveryType
                                                                                    .VisualStudioSetup
                                                                         }
                                                                        ) ;
            var visualStudioInstances =
                vsInstances as VisualStudioInstance[] ?? vsInstances.ToArray ( ) ;

            string RenderFunc ( VisualStudioInstance inst1 )
            {
                return
                    $"* {inst1.Name,- 30} {inst1.Version.Major:00}.{inst1.Version.Minor:00}.{inst1.Version.Build:00000}.{inst1.Version.MinorRevision:0000}  [{inst1.VisualStudioRootPath}]" ;
            }

            var choices = visualStudioInstances.Select (
                                                        x => new MenuWrapper < VisualStudioInstance
                                                        > ( x , RenderFunc )
                                                       ) ;
            menu.Config.SelectedAppearence =
                new Configuration.SelectedColor { BackgroundColor = ConsoleColor.Yellow } ;
            var selected = menu.Render ( choices ) ;
#if false
            var i2 = (
                         from inst in visualStudioInstances
                         where inst.Version.Major == 15
                         orderby inst.Version descending
                         select inst ).FirstOrDefault ( ) ;

            if ( i2 != null )
            {
#endif
            var i2 = selected.Instance ;
            Logger.Warn ( "Selected instance {instance} {path}" , i2.Name , i2.MSBuildPath ) ;
            MSBuildLocator.RegisterInstance ( i2 ) ;
#if false
        }
#endif
            Console.WriteLine ( "" ) ;

#endif
            var i = 0 ;
            var browserNodeCollection = context.BrowserViewModel.RootCollection ;
            var nodes = new List < IBrowserNode > ( browserNodeCollection.Count ) ;
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
                    Console.WriteLine ( $"\tRepositoryUrl is {project.RepositoryUrl}" ) ;
                }
            }

            IProjectBrowserNode projectNode = null ;
            for ( ; ; )
            {
                var key = Console.ReadKey ( ) ;

                if ( ! char.IsDigit ( key.KeyChar ) )
                {
                    continue ;
                }

                var selection = ( int ) char.GetNumericValue ( key.KeyChar ) ;

                projectNode = nodes[ selection - 1 ] as IProjectBrowserNode ;
                if ( projectNode == null )
                {
                    continue ;
                }

                break ;
            }

            var j = 0 ;
            Meta < Lazy < IAnalyzeCommand3 > > command2 = null ;
            // foreach ( var cmd in context.AnalyzeCommands )
            // {
            // Console.WriteLine("Command #" + j);
            // foreach ( var keyValuePair in cmd.Metadata )
            // {
            // Console.WriteLine ( $"  {keyValuePair.Key}: {keyValuePair.Value}" ) ;
            // }

            // command2 = cmd ;
            // }

            Console.ReadLine ( ) ;

            Console.WriteLine ( projectNode.SolutionPath ) ;
            Console.ReadLine ( ) ;

            //    ITargetBlock <RejectedItem> rejectTarget = new ActionBlock < RejectedItem > (item => Console.WriteLine($"Reject: {item.Statement}"));
            if ( command2 != null )
            {
                await command2.Value.Value.AnalyzeCommandAsync ( projectNode ) ;
            }
            else
            {
                Console.WriteLine ( "No commnad" ) ;
                return 1 ;
            }

            return 0 ;
        }

        private static void Init ( ) { }
    }

    internal class MenuWrapper < T >
    {
        private readonly T                   _instance ;
        private readonly Func < T , string > _renderFunc ;

        public MenuWrapper ( T instance , Func < T , string > renderFunc )
        {
            _instance   = instance ;
            _renderFunc = renderFunc ;
        }

        public T Instance { get { return _instance ; } }

        #region Overrides of Object
        public override string ToString ( ) { return _renderFunc ( Instance ) ; }
        #endregion
    }
}