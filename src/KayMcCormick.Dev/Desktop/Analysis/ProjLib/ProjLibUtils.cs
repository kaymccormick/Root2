using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Threading ;
using System.Threading.Tasks ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Host.Mef ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Options ;
using NLog ;

namespace ProjLib
{
    public static class ProjLibUtils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static async Task < Workspace > LoadSolutionInstanceAsync (
            IProgress < ProjectLoadProgress > progress
          , string                            solutionPath
          , VisualStudioInstance              instance
        )

        {
            Logger.Info (
                         "Load solution {threadid} {solution}"
                       , Thread.CurrentThread.ManagedThreadId
                       , solutionPath
                        ) ;
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
                IDictionary < string , string > props = new Dictionary < string , string > ( ) ;
                props[ "Platform" ] = "x86" ;
                workspace           = MSBuildWorkspace.Create ( props ) ;
            }
            catch ( Exception ex )
            {
                throw ;
            }

            // Print message for WorkspaceFailed event to help diagnosing project load failures.

            workspace.WorkspaceFailed += ( o , e ) => {
                // "Msbuild failed when processing the file 'C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\V3\\Root\\src\\KayMcCormick.Dev\\Desktop\\App1\\App1.csproj' with message: C:\\WINDOWS\\Microsoft.NET\\Framework\\v4.0.30319\\Microsoft.WinFx.targets: (268, 9): Unknown build error, 'Object reference not set to an instance of an object.' "
                Logger.Fatal ( "Load workspac failed: {}" , e.Diagnostic.Message ) ;
            } ;

            // ReSharper disable once LocalizableElement
            Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
            Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

            // Attach progress reporter so we print projects as they are loaded.
            await workspace.OpenSolutionAsync ( solutionPath , progress ).ConfigureAwait ( false ) ;
            // , new Program.ConsoleProgressReporter()
            // );
            Logger.Debug ( $"Finished loading solution '{solutionPath}'" ) ;
            return workspace ;
        }
    }
}