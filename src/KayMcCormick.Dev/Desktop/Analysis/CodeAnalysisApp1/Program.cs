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
    internal static partial class Program
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
            MSBuildLocator.RegisterInstance ( instance ) ;

            using ( var workspace = MSBuildWorkspace.Create ( ) )
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed +=
                    ( o , e ) => Console.WriteLine ( e.Diagnostic.Message ) ;

                Debug.Assert ( args != null , nameof ( args ) + " != null" ) ;
                var solutionPath = args?[ 0 ] ;
                // ReSharper disable once LocalizableElement
                Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
                Console.WriteLine ( $"Loading solution '{solutionPath}'" ) ;

                // Attach progress reporter so we print projects as they are loaded.
                await workspace.OpenSolutionAsync (
                                                   solutionPath
                                                 , new ConsoleProgressReporter ( )
                                                  ) ;
                // ReSharper disable once LocalizableElement
                Console.WriteLine ( $"Finished loading solution '{solutionPath}'" ) ;

                var xmlDocument = new XmlDocument ( ) ;
                var root = xmlDocument.CreateElement ( "root" ) ;
                xmlDocument.AppendChild ( root ) ;
                var bigDictionary =
                    new Dictionary < string , Dictionary < string , Dictionary < object , object > >
                    > ( ) ;
                foreach ( var project in workspace.CurrentSolution.Projects.Where (
                                                                                   project
                                                                                       => project
                                                                                             .Name
                                                                                          != "NLog"
                                                                                  ) )
                {
                    var projDict = new Dictionary < string , Dictionary < object , object > > ( ) ;
                    bigDictionary[ project.Name ] = projDict ;
                    var projectElem = xmlDocument.CreateElement ( "project" ) ;
                    projectElem.SetAttribute ( "name" , project.Name ) ;
                    root.AppendChild ( projectElem ) ;
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
                        projDict[ relativePath ] = docDict ;
                        await ProcessDocumentAsync ( args , document1 , projectElem , docDict ) ;
                    }
                }

                File.WriteAllText (
                                   "out.json"
                                 , JsonConvert.SerializeObject (
                                                                bigDictionary
                                                              , Newtonsoft.Json.Formatting.Indented
                                                               )
                                  ) ;
                xmlDocument.Save ( "out.xml" ) ;
            }

            // TODO: Do analysis on the projects in the loaded solution
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
            IEnumerable < string >          args
          , [ NotNull ] Document                        document1
          , XmlNode                         xmlNode
          , IDictionary < object , object > dict
        )
        {
            if ( document1 == null )
            {
                throw new ArgumentNullException ( nameof ( document1 ) ) ;
            }

            var doc = xmlNode.OwnerDocument ?? throw new ArgumentNullException ( nameof ( args ) ) ;
            var docElem = doc.CreateElement ( "document" ) ;

            docElem.SetAttribute (
                                  "filePath"
                                , GetRelativePath (
#pragma warning disable CS8604 // Possible null reference argument.
                                                   document1.Project.FilePath
                                                 , document1.FilePath
#pragma warning restore CS8604 // Possible null reference argument.
                                                  )
                                 ) ;

            using ( var @out = new StreamWriter ( document1.Name + ".txt" ) )
            {
                var tree = await document1.GetSyntaxTreeAsync ( ) ;
                var model = await document1.GetSemanticModelAsync ( ) ;

                // var xmlDocument = tree.Dump ( @out ) ;
                var outputFileName = document1.Name + ".xml" ;
                using ( var xmlWriter = XmlWriter.Create (
                                                          outputFileName
                                                        , new XmlWriterSettings
                                                          {
                                                              Indent              = true
                                                            , NewLineOnAttributes = true
                                                          }
                                                         ) )
                {

                    // xmlDocument.WriteTo ( xmlWriter ) ;

                    xmlWriter.Close ( ) ;
                }

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
}
