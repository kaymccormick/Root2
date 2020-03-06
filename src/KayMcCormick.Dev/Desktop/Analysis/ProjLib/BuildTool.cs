using System ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.IO ;
using System.Linq ;
#if USEMSBUILD
using Microsoft.Build.Evaluation ;
using Microsoft.Build.Execution ;

using NLog ;

namespace ProjLib
{
    static internal class BuildTool
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static BuildResults BuildRepository ( string arg )
        {
            Logger.Info ( "BuildRepository {arg}" , arg ) ;
            try
            {
                var b = ImmutableDictionary.CreateBuilder < string , string > ( ) ;
                b[ "Platform" ] = "x86" ;

                IDictionary < string , string > props = b.ToImmutable ( ) ;

                List < string > files ;
                var projFilesList = Path.Combine ( arg , "projects.txt" ) ;
                Logger.Debug ( "Checking for existince of project file {file}" , projFilesList ) ;
                if ( File.Exists ( projFilesList ) )
                {
                    files = File.ReadAllLines ( projFilesList ).ToList ( ) ;
                }
                else
                {
                    files = Directory
                           .EnumerateFiles ( arg , "*.csproj" , SearchOption.AllDirectories )
                           .ToList ( ) ;
                }

                List < string > solList ;
                var sol = Path.Combine ( arg , "solutions.txt" ) ;
                Logger.Debug ( "Checking for existince of poject file {file}" , projFilesList ) ;
                if ( File.Exists ( sol ) )
                {
                    solList = File.ReadAllLines ( sol ).ToList ( ) ;
                }
                else
                {
                    solList = Directory
                             .EnumerateFiles ( arg , "*.sln" , SearchOption.AllDirectories )
                             .ToList ( ) ;
                }

                var buildParameters = new BuildParameters
                                      {
                                          ProjectLoadSettings = ProjectLoadSettings.Default
                                        , Interactive         = false
                                        , Loggers             = new[] { new MyLogger ( ) }
                                      } ;

                var buildResults = new BuildResults ( )
                                   {
                                       SourceDir = arg , SolutionsFilesList = solList
                                   } ;


                var buildFiles = new[] { solList.First ( ) } ;
                BuildManager.DefaultBuildManager.ResetCaches ( ) ;
                foreach ( var f in buildFiles )
                {
                    var realF = Path.Combine ( arg , f ) ;
                    Logger.Warn ( "Building {file}" , f ) ;
                    var buildRequest = new BuildRequestData (
                                                             realF
                                                           , props
                                                           , null
                                                           , new[] { "Restore" }
                                                           , new HostServices ( )
                                                           , BuildRequestDataFlags
                                                                .ProvideProjectStateAfterBuild
                                                            ) ;

                    var result =
                        BuildManager.DefaultBuildManager.Build ( buildParameters , buildRequest ) ;

                    if ( result.OverallResult == BuildResultCode.Failure )
                    {
                        throw new FailedBuildException();
                    }

                    Logger.Info ( "Result: {buildResult}" , result.OverallResult ) ;
                }



                return buildResults ;
            }
            catch ( Exception ex )
            {
                Logger.Debug( ex , ex.ToString ( ) ) ;
            }
            
            return null ;
        }
    }
}
#endif