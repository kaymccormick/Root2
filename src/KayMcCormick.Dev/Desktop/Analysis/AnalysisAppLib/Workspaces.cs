using System ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Text.RegularExpressions ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using Buildalyzer ;
using Buildalyzer.Workspaces ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisAppLib
{
    public class Workspaces
    {
        private static readonly Logger            Logger = LogManager.GetCurrentClassLogger ( ) ;
        private                 IWorkspaceManager _manager;
        public Workspaces(IWorkspaceManager manager) {
            _manager = manager;
        }

        public TransformBlock<AnalysisRequest, Workspace> InitializeWorkspaceBlock()
        {
            var makeWs =
                new TransformBlock<AnalysisRequest, Workspace>(MakeWorkspaceAsync);
            return makeWs;
        }
        public TransformBlock<AnalysisRequest, Workspace> InitializeWorkspace2Block()
        {
            var makeWs =
                new TransformBlock<AnalysisRequest, Workspace>(MakeWorkspace2Async);
            return makeWs;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public static async Task < Workspace > MakeWorkspace2Async ( [ NotNull ] AnalysisRequest req )
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                AnalyzerManager manager = new AnalyzerManager ( req.Info.SolutionPath ) ;
                AdhocWorkspace workspace = new AdhocWorkspace ( ) ;
                foreach ( var keyValuePair in manager.Projects )
                {
                    Logger.Debug ( keyValuePair.Key ) ;
                    keyValuePair.Value.Build ( ) ;
                    keyValuePair.Value.AddToWorkspace ( workspace ) ;
                }

                return workspace ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , "here" ) ;
                throw ;
            }
        }


        public async Task < Workspace > MakeWorkspaceAsync ( [ NotNull ] AnalysisRequest req)
        {
            if ( req == null )
            {
                throw new ArgumentNullException ( nameof ( req ) ) ;
            }

            using ( MappedDiagnosticsLogicalContext.SetScoped ( "ProjectName" , req.Info.Name ) )
            {

                var arg = req.Info.SolutionPath ;
                Logger.Debug ( "[{action}] arg is {arg}" , nameof ( MakeWorkspaceAsync ) , arg ) ;

                var b = ImmutableDictionary.CreateBuilder < string , string > ( ) ;
                if ( req.Info.Platform != null )
                {
                    b[ "Platform" ] = req.Info.Platform ;
                }

                b[ "SkipGetTargetFrameworkProperties" ] = "true" ;
                IDictionary < string , string > props = b.ToImmutable ( ) ;

#if !ROSLYNMSBUILD
                Workspace workspace = _manager.CreateWorkspace(props);
#else
                MSBuildWorkspace workspace ;
                try
                {
                    workspace = MSBuildWorkspace.Create ( props ) ;
                }
                catch ( ReflectionTypeLoadException ex )
                {
                    foreach ( var exLoaderException in ex.LoaderExceptions )
                    {
                        Logger.Info ( exLoaderException.Message ) ;
                        Logger.Info ( exLoaderException.GetType ( ).FullName ) ;
                    }

                    throw new UnableToInitializeWorkspace (
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                                                           "Unable to initialize workspace."
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                                                     , ex
                                                          ) ;
                }
                 Logger.Info (
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                             "SkipUnrecognizedProjects is {SkipUnrecognizedProjects}"
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                   , workspace.SkipUnrecognizedProjects
                            ) ;
                workspace.SkipUnrecognizedProjects = true ;
#endif


                workspace.WorkspaceChanged +=
                    ( sender , args ) => Logger.Warn ( "{kind}" , args.Kind ) ;
                workspace.DocumentOpened += ( sender , args )
                    => Logger.Debug (
                                     "{eventName}: {document}"
                                   , nameof ( workspace.DocumentOpened )
                                   , args.Document
                                    ) ;

                // Print message for WorkspaceFailed event to help diagnosing project load failures.

                ConcurrentQueue < string > Errors = new ConcurrentQueue < string > ( ) ;
                workspace.WorkspaceFailed += ( o , e ) => {
                    try
                    {
                        var m = Regex.Match (
                                             e.Diagnostic.Message
                                           , "Msbuild failed when processing the file '(.*)' with message:(.*)"
                                            ) ;
                        if ( m.Success )
                        {
                            var file1 = m.Groups[ 1 ].Captures[ 0 ] ;
                            var message = m.Groups[ 2 ].Captures[ 0 ] ;
                            Logger.Debug (
                                          "Workspace Error: {file}, {msg}"
                                        , file1.ToString ( )
                                        , message.ToString ( )
                                         ) ;
                            if ( ! message.ToString ( ).Contains ( "WinFX.targets" )
                                 || ! message.ToString ( ).Contains ( "Object reference not set" ) )
                            {
                                Errors.Enqueue ( e.Diagnostic.Message ) ;
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        Logger.Debug ( ex , ex.Message ) ;
                    }

                    // "Msbuild failed when processing the file 'C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\V3\\Root\\src\\KayMcCormick.Dev\\Desktop\\App1\\App1.csproj' with message: C:\\WINDOWS\\Microsoft.NET\\Framework\\v4.0.30319\\Microsoft.WinFx.targets: (268, 9): Unknown build error, 'Object reference not set to an instance of an object.' "
                    Logger.Debug (
                                  "Load workspace failed: {} {x}"
                                , e.Diagnostic.Message
                                , e.Diagnostic.Kind
                                 ) ;
                } ;

                var solutionPath = req.Info.SolutionPath ;
                if ( solutionPath == null )
                {
                    List < string > solList ;
                    var sol = Path.Combine ( arg , "solutions.txt" ) ;
                    if ( File.Exists ( sol ) )
                    {
                        solList = File.ReadAllLines ( sol ).ToList ( ) ;
                    }
                    else
                    {
                        var dirPath = arg ;
                        try
                        {

                            solList = Directory
                                     .EnumerateFiles (
                                                      dirPath
                                                    , "*.sln"
                                                    , SearchOption.AllDirectories
                                                     )
                                     .ToList ( ) ;
                        }
                        catch ( IOException ioex )
                        {
                            var message =
                                $"Unable to enumerate directory {dirPath}: {ioex.Message}" ;
                            Logger.Debug ( ioex , message ) ;
                            throw new Exception ( message , ioex ) ;
                        }
                    }

                    // ReSharper disable once LocalizableElement
                    solutionPath = Path.Combine ( arg , solList.First ( ) ) ;
                }

                Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
                Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

                // Attach progress reporter so we print projects as they are loaded.
                //workspace.Services.GetService < IOptionervice > ( ) ;
                //workspace.Options.WithChangedOption(new OptionKey(), ))
#if ROSLYNMSBUILD   
                await workspace.OpenSolutionAsync ( solutionPath ).ConfigureAwait ( true ) ;
#endif
                await _manager.OpenSolutionAsync ( workspace , solutionPath ) ;
                // , new Program.ConsoleProgressReporter()
                // );
                // if ( ! Errors.IsEmpty )
                // {
                // throw new UnableToInitializeWorkspace ( string.Join ( ", " , Errors ) ) ;
                // }
                
                Logger.Debug ( $"Finished loading solution '{solutionPath}'" ) ;
                return workspace;
            }
        }

        public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock ( )
        {
            return new TransformManyBlock < Workspace , Document > ( workspace => ParallelEnumerable.Where < Project > (
                                                                                                                  workspace
                                                                                                                     .CurrentSolution
                                                                                                                     .Projects.AsParallel (  )
                                                                                                                , project
                                                                                                                      => {
                                                                                                                      Logger
                                                                                                                         .Warn (
                                                                                                                                "{project}"
                                                                                                                              , project
                                                                                                                                   .Name
                                                                                                                               ) ;
                                                                                                                      return
                                                                                                                          true ;
                                                                                                                  }
                                                                                                                 )
                                                                                                    .SelectMany (
                                                                                                                 project
                                                                                                                     => project
                                                                                                                        .Documents
                                                                                                                )
                                                                                                    .Where (
                                                                                                            document
                                                                                                                => {
                                                                                                                Logger
                                                                                                                   .Info (
                                                                                                                          "{document}"
                                                                                                                        , document
                                                                                                                             .Name
                                                                                                                         ) ;
                                                                                                                return
                                                                                                                    true ;
                                                                                                            }
                                                                                                           ) );

        }
    }
}