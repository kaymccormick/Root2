using System ;
using System.Diagnostics ;
using System.Threading.Tasks ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.MSBuild ;
using NLog ;
using ProjLib ;

public static class ProjLibUtils
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
    public static async Task < Workspace > LoadSolutionInstanceAsync(
        IProgress < ProjectLoadProgress > progress
      , string                            solutionPath
      , VisualStudioInstance              instance
    )

    {
        if ( MSBuildLocator.IsRegistered )
        {
            // MSBuildLocator.Unregister ( ) ;
        }

        if ( MSBuildLocator.CanRegister )
        {
            MSBuildLocator.RegisterInstance ( instance ) ;
        }
        else
        {
            // throw new Exception ( "Unable to register msbuildlocator" ) ;
        }

        MSBuildWorkspace workspace ;
        try
        {
            workspace = MSBuildWorkspace.Create ( ) ;
        }
        catch ( Exception ex )
        {
            throw ;
        }

        // Print message for WorkspaceFailed event to help diagnosing project load failures.
        workspace.WorkspaceFailed += ( o , e ) => Console.WriteLine ( e.Diagnostic.Message ) ;

        // ReSharper disable once LocalizableElement
        Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
        Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

        // Attach progress reporter so we print projects as they are loaded.
        await workspace.OpenSolutionAsync ( solutionPath , progress )
                       .ConfigureAwait ( true ) ;
        // , new Program.ConsoleProgressReporter()
        // );
        Logger.Debug( $"Finished loading solution '{solutionPath}'" ) ;
        return workspace ;
    }
}