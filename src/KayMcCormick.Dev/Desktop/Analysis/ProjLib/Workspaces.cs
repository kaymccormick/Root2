using System ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using System.Text.RegularExpressions ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog ;
using ProjLib ;

static internal class Workspaces
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

    public static async Task < Workspace > MakeWorkspaceAsync ( [ NotNull ] string arg)
    {
        if ( arg== null )
        {
            throw new ArgumentNullException ( nameof ( arg) ) ;
        }

        var b = ImmutableDictionary.CreateBuilder < string , string > ( ) ;
        b[ "Platform" ] = "x86" ;

        IDictionary < string , string > props = b.ToImmutable ( ) ;

        MSBuildWorkspace workspace ;
        try
        {
            workspace = MSBuildWorkspace.Create ( props ) ;
        }
        catch ( ReflectionTypeLoadException ex )
        {
            foreach ( var exLoaderException in ex.LoaderExceptions )
            {
                Logger.Info(exLoaderException.Message);
                Logger.Info(exLoaderException.GetType().FullName);
            }

            throw new UnableToInitializeWorkspace ("Unable to initialize workspace.",  ex ) ;
        }


        Logger.Info (
                     "SkipUnrecognizedProjects is {SkipUnrecognizedProjects}"
                   , workspace.SkipUnrecognizedProjects
                    ) ;
        workspace.SkipUnrecognizedProjects = true ;
        workspace.WorkspaceChanged += ( sender , args ) => Logger.Warn ( "{kind}" , args.Kind) ;
        workspace.DocumentOpened += ( sender , args )
            => Logger.Debug (
                             "{eventName}: {document}"
                           , nameof ( workspace.DocumentOpened )
                           , args.Document
                            ) ;

        // Print message for WorkspaceFailed event to help diagnosing project load failures.

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
                    Logger.Debug ( "Workspace Error: {file}, {msg}" , file1 , message ) ;
                }
            }
            catch ( Exception ex )
            {
                Logger.Warn ( ex , ex.ToString ( ) ) ;
            }

            // "Msbuild failed when processing the file 'C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\V3\\Root\\src\\KayMcCormick.Dev\\Desktop\\App1\\App1.csproj' with message: C:\\WINDOWS\\Microsoft.NET\\Framework\\v4.0.30319\\Microsoft.WinFx.targets: (268, 9): Unknown build error, 'Object reference not set to an instance of an object.' "
            Logger.Debug ( "Load workspace failed: {} {x}" , e.Diagnostic.Message, e.Diagnostic.Kind ) ;
        } ;

        List<string> solList;
        var sol = Path.Combine(arg, "solutions.txt");
        if (File.Exists(sol))
        {
            solList = File.ReadAllLines(sol).ToList();
        }
        else
        {
            solList = Directory
                     .EnumerateFiles(arg, "*.sln", SearchOption.AllDirectories)
                     .ToList();
        }

        // ReSharper disable once LocalizableElement
        var solutionPath = Path.Combine (
                                         arg
                                       , solList.First ( )
                                        ) ;
        Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
        Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

        // Attach progress reporter so we print projects as they are loaded.
        //workspace.Services.GetService < IOptionService > ( ) ;
        //workspace.Options.WithChangedOption(new OptionKey(), ))
        await workspace.OpenSolutionAsync ( solutionPath ).ConfigureAwait ( true ) ;
        // , new Program.ConsoleProgressReporter()
        // );
        Logger.Debug ( $"Finished loading solution '{solutionPath}'" ) ;
        return workspace ;
    }

    public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock ( )
    {
        return new TransformManyBlock < Workspace , Document > (
                                                                workspace => workspace
                                                                            .CurrentSolution
                                                                            .Projects
                                                                            .SelectMany (
                                                                                         project
                                                                                             => project
                                                                                                .Documents
                                                                                        )
                                                               ) ;
    }
}

internal class UnableToInitializeWorkspace : Exception
{
    public UnableToInitializeWorkspace ( ) {
    }

    public UnableToInitializeWorkspace ( string message ) : base ( message )
    {
    }

    public UnableToInitializeWorkspace ( string message , Exception innerException ) : base ( message , innerException )
    {
    }

    protected UnableToInitializeWorkspace ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
    {
    }
}