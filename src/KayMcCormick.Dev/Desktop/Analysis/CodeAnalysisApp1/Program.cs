using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Xml ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Logging.Common ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Newtonsoft.Json ;
using NLog ;
using Formatting = System.Xml.Formatting ;

namespace CodeAnalysisApp1
{
    public static partial class Program
    {
        // ReSharper disable once UnusedMember.Local
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private static async Task Main ( string[] args )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured ( ) ;

            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances ( ) ;
            var instance = visualStudioInstances.First (
                                                        studioInstance
                                                            => studioInstance.VisualStudioRootPath
                                                               == "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community"
                                                       ) ;
            // var instance = visualStudioInstances.Length == 1
            // ? visualStudioInstances[0]
            // : SelectVisualStudioInstance(visualStudioInstances);

            // ReSharper disable once LocalizableElement
            Console.WriteLine ( $"Using MSBuild at '{instance.MSBuildPath}' to load projects." ) ;

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            await NewMethod ( args[0] , instance ) ;
                
            // TODO: Do analysis on the projects in the loaded solution
        }

        public static async Task<MSBuildWorkspace> NewMethod (
            string               solutionPath
                                , VisualStudioInstance instance
        )
        {
            MSBuildLocator.RegisterInstance ( instance ) ;
            var workspace = MSBuildWorkspace.Create ( ) ;
            
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed +=
                    ( o , e ) => Console.WriteLine ( e.Diagnostic.Message ) ;

                // ReSharper disable once LocalizableElement
                Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
                Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

                // Attach progress reporter so we print projects as they are loaded.
                await workspace.OpenSolutionAsync (
                                                   solutionPath
                                                 , new ConsoleProgressReporter ( )
                                                  ) ;
                Console.WriteLine($"Finished loading solution '{solutionPath}'");
                return workspace ;
        }
        

        public static async Task P(Workspace workspace){
        // ReSharper disable once LocalizableElement
                foreach ( var project in workspace.CurrentSolution.Projects.Where (
                                                                                   project
                                                                                       => project.Name
                                                                                          != "NLog"
                                                                                  ) )
                {
                    var projDict = new Dictionary < string , Dictionary < object , object > > ( ) ;
                    foreach ( var document1 in project.Documents.Where (
                                                                        document => document
                                                                           .SupportsSyntaxTree /*&& document.Name == "AppContainerFixture.cs"*/
                                                                       ) )
                    {
                        var docDict = new Dictionary < object , object > ( ) ;

                        var relativePath = GetRelativePath (
#pragma warning disable CS8604 // Possible null reference argument.
                                                            document1.Project.FilePath
                                                          , document1.FilePath
#pragma warning restore CS8604 // Possible null reference argument.
                                                           ) ;
                        await ProcessDocumentAsync ( document1 );
                    }
                }
            }
        

        public static string GetRelativePath ( string relativeTo , string path )
        {
            var uri = new Uri ( relativeTo ) ;
            var rel = Uri
                     .UnescapeDataString ( uri.MakeRelativeUri ( new Uri ( path ) ).ToString ( ) )
                     .Replace ( Path.AltDirectorySeparatorChar , Path.DirectorySeparatorChar ) ;
            if ( rel.Contains ( Path.DirectorySeparatorChar.ToString ( ) ) == false )
            {
                rel = $".{Path.DirectorySeparatorChar}{rel}" ;
            }

            return rel ;
        }

        private static async Task ProcessDocumentAsync (
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            // ReSharper disable once UnusedParameter.Local
            [ NotNull ] Document                        document1
        )
        {
            if ( document1 == null )
            {
                throw new ArgumentNullException ( nameof ( document1 ) ) ;
            }

              var tree = await document1.GetSyntaxTreeAsync ( ) ;
                var model = await document1.GetSemanticModelAsync ( ) ;

                var root = tree.GetCompilationUnitRoot ( ) ;
                if ( model != null )
                {
                    List < StatementSyntax > query ;
                    try
                    {
                        query = Common.Query1 ( root , model ) ;
                    }
                    catch ( Exception ex )
                    {
                        Logger.Info ( ex , ex.Message ) ;
                        throw ;
                    }

                    Debug.Assert ( query != null , nameof ( query ) + " != null" ) ;
#if false
                    var d = query.Where(a => a != null).SelectMany ( tuples => tuples.Select ( tuple => tuple.Item1 ) )
                                 .Distinct ( )
                                 .Select (
                                          ( syntax , i ) => Tuple.Create (
                                                                          syntax.GetLocation ( )
                                                                                .GetMappedLineSpan ( )
                                                                                .StartLinePosition
                                                                                .Line
                                                                          + 1
                                                                        , syntax.Expression
                                                                                .ToString ( )
                                                                        , syntax.ArgumentList
                                                                                .Arguments
                                                                                .Select (
                                                                                         argumentSyntax
                                                                                             => argumentSyntax
                                                                                                .Expression
                                                                                        )
                                                                                .Select (
                                                                                         Transforms
                                                                                            .TransformExpr
                                                                                        )
                                                                         )
                                         )
                                 .ToDictionary (
                                                tuple => tuple.Item1
                                              , tuple => tuple.Item3.ToList ( )
                                               ) ;

                    foreach ( var keyValuePair in d )
                    {
                        dict[ keyValuePair.Key ] = keyValuePair.Value ;
                    }
#endif
                }
            
        }
    }
}
